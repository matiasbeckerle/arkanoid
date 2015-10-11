using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public float shakeAmount = 0.1f;
    public float shakeDecreaseFactor = 1f;

    private GameObject mainCamera;
    private bool isShaking;

    void Awake()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    /// <summary>
    /// Shakes the camera.
    /// </summary>
    /// <param name="duration">Seconds to end the movement.</param>
    public void Shake(float duration)
    {
        if (!isShaking)
        {
            StartCoroutine(ShakeItOff(duration));
        }
    }

    /// <summary>
    /// Shakes the camera.
    /// </summary>
    /// <param name="duration">Seconds to end the movement.</param>
    /// <returns></returns>
    IEnumerator ShakeItOff(float duration)
    {
        // One shake at the time.
        isShaking = true;

        // Keep a reference of the current position.
        Vector3 cameraOriginalPosition = mainCamera.transform.position;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            // Shake the camera.
            mainCamera.transform.localPosition += Random.insideUnitSphere * shakeAmount * elapsed;

            // Increase the elapsed time of camera being shake.
            elapsed += Time.deltaTime * shakeDecreaseFactor;

            yield return null;
        }

        // Back to the original position.
        mainCamera.transform.localPosition = cameraOriginalPosition;

        isShaking = false;
    }
}
