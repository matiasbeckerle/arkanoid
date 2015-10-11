using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    /// <summary>
    /// Shake amount.
    /// </summary>
    public float shakeAmount = 0.1f;

    /// <summary>
    /// Shake decrease factor.
    /// </summary>
    public float shakeDecreaseFactor = 1f;

    /// <summary>
    /// MainCamera's reference.
    /// </summary>
    private GameObject _mainCamera;

    /// <summary>
    /// Flag to know when the camera is shaking.
    /// </summary>
    private bool _isShaking;

    void Awake()
    {
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    /// <summary>
    /// Shakes the camera.
    /// </summary>
    /// <param name="duration">Seconds to end the movement.</param>
    public void Shake(float duration)
    {
        if (!_isShaking)
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
        _isShaking = true;

        // Keep a reference of the current position.
        Vector3 cameraOriginalPosition = _mainCamera.transform.position;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            // Shake the camera.
            _mainCamera.transform.localPosition += Random.insideUnitSphere * shakeAmount * elapsed;

            // Increase the elapsed time of camera being shake.
            elapsed += Time.fixedDeltaTime * shakeDecreaseFactor;

            yield return null;
        }

        // Back to the original position.
        _mainCamera.transform.localPosition = cameraOriginalPosition;

        _isShaking = false;
    }
}
