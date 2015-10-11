using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
    public float initialVelocity = 500f;
    public AudioClip brickExplosionSound1;
    public AudioClip brickExplosionSound2;
    public AudioClip brickExplosionSound3;
    public AudioClip wallHitSound1;
    public AudioClip wallHitSound2;
    public GameObject sparks;

    private Rigidbody rb;
    private float initialVelocityPerLevel;
    private CameraShake shaker;

    private Camera mainCamera;
    private Camera ballCamera;
    private bool cameraSwitchEnabled = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        shaker = GetComponent<CameraShake>();

        // Each level adds a little bit of velocity to make it challenger.
        // TODO: review this.
        initialVelocityPerLevel = initialVelocity + (50 * GameManager.Instance.GetCurrentLevel());

        SetupCameras();
    }

    void Update()
    {
        if (Input.GetAxisRaw("Fire1") != 0)
        {
            if (!GameManager.Instance.IsPlaying())
            {
                cameraSwitchEnabled = false;
                Invoke("EnableCameraSwitch", 1);

                GameManager.Instance.SetPlaying(true);
                transform.parent = null;
                rb.isKinematic = false;

                // Add some natural force.
                rb.AddForce(new Vector3(initialVelocityPerLevel, initialVelocityPerLevel, 0));

                // Add some rotation.
                rb.AddTorque(new Vector3(7, 7, 7));
            }
            else if (cameraSwitchEnabled)
            {
                mainCamera.enabled = false;
                ballCamera.enabled = true;
            }
        }
        else
        {
            ballCamera.enabled = false;
            mainCamera.enabled = true;
        }
    }

    void SetupCameras()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        ballCamera = transform.Find("BallCamera").GetComponent<Camera>();
    }

    void EnableCameraSwitch()
    {
        cameraSwitchEnabled = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Brick")
        {
            SoundManager.Instance.RandomizeSfx(brickExplosionSound1, brickExplosionSound2, brickExplosionSound3);
        }
        else
        {
            shaker.Shake(1);

            // Add sparks particles and removes it after some seconds.
            var sparksInstance = Instantiate(sparks, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(sparksInstance, 3);

            SoundManager.Instance.RandomizeSfx(wallHitSound1, wallHitSound2);
        }
    }

    void OnDestroy()
    {
        if (mainCamera != null)
        {
            mainCamera.enabled = true;
        }
    }
}
