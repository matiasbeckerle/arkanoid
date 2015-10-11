using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
    /// <summary>
    /// Initial force to apply over the ball as constant from the first level.
    /// </summary>
    public float initialVelocity = 500f;

    /// <summary>
    /// Sparks particle prefab to instantiate on collisions.
    /// </summary>
    public GameObject sparks;

    /// <summary>
    /// Sound to reproduce on brick explosion.
    /// </summary>
    public AudioClip brickExplosionSound1;

    /// <summary>
    /// Sound to reproduce on brick explosion.
    /// </summary>
    public AudioClip brickExplosionSound2;

    /// <summary>
    /// Sound to reproduce on brick explosion.
    /// </summary>
    public AudioClip brickExplosionSound3;

    /// <summary>
    /// Sound to reproduce on wall hit.
    /// </summary>
    public AudioClip wallHitSound1;

    /// <summary>
    /// Sound to reproduce on wall hit.
    /// </summary>
    public AudioClip wallHitSound2;

    /// <summary>
    /// Rigidbody's reference.
    /// </summary>
    private Rigidbody _rigidbody;

    /// <summary>
    /// Force to apply over the ball on per level.
    /// </summary>
    private float _initialVelocityPerLevel;

    /// <summary>
    /// CameraShake effect script reference.
    /// </summary>
    private CameraShake _shaker;

    /// <summary>
    /// Main camera's reference.
    /// </summary>
    private Camera _mainCamera;

    /// <summary>
    /// Ball camera's reference.
    /// </summary>
    private Camera _ballCamera;

    /// <summary>
    /// Flag to know when the camera switching could be possible.
    /// </summary>
    private bool _cameraSwitchEnabled = false;

    protected void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _shaker = GetComponent<CameraShake>();

        // Each level adds a little bit of velocity to make it challenger.
        // TODO: review this.
        _initialVelocityPerLevel = initialVelocity + (50 * GameManager.Instance.GetCurrentLevel());

        SetupCameras();
    }

    protected void Update()
    {
        if (Input.GetAxisRaw("Fire1") != 0)
        {
            if (!GameManager.Instance.IsPlaying())
            {
                _cameraSwitchEnabled = false;
                Invoke("EnableCameraSwitch", 1);

                GameManager.Instance.SetPlaying(true);
                transform.parent = null;
                _rigidbody.isKinematic = false;

                // Add some natural force.
                _rigidbody.AddForce(new Vector3(_initialVelocityPerLevel, _initialVelocityPerLevel, 0));

                // Add some rotation.
                _rigidbody.AddTorque(new Vector3(7, 7, 7));
            }
            else if (_cameraSwitchEnabled)
            {
                _mainCamera.enabled = false;
                _ballCamera.enabled = true;
            }
        }
        else
        {
            _ballCamera.enabled = false;
            _mainCamera.enabled = true;
        }
    }

    /// <summary>
    /// Saves a reference for each camera.
    /// </summary>
    private void SetupCameras()
    {
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        _ballCamera = transform.Find("BallCamera").GetComponent<Camera>();
    }

    /// <summary>
    /// Enables camera switching.
    /// </summary>
    private void EnableCameraSwitch()
    {
        _cameraSwitchEnabled = true;
    }

    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Brick")
        {
            SoundManager.Instance.RandomizeSfx(brickExplosionSound1, brickExplosionSound2, brickExplosionSound3);
        }
        else
        {
            _shaker.Shake(1);

            // Add sparks particles and removes it after some seconds.
            var sparksInstance = Instantiate(sparks, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(sparksInstance, 3);

            SoundManager.Instance.RandomizeSfx(wallHitSound1, wallHitSound2);
        }
    }

    protected void OnDestroy()
    {
        if (_mainCamera != null)
        {
            _mainCamera.enabled = true;
        }
    }
}
