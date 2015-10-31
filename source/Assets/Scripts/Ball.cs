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
    /// Used in order to limit the ball speed (maximum).
    /// </summary>
    private float _maxVelocityMagnitude = 19;

    /// <summary>
    /// Used in order to limit the ball speed (minimum).
    /// </summary>
    private float _minVelocityMagnitude = 16;

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

    /// <summary>
    /// Used to generate a random movement using secondary button.
    /// </summary>
    private int[] _negativeOrPositiveAxeOptions = new int[] { -1, 1 };

    protected void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _shaker = GetComponent<CameraShake>();

        _initialVelocityPerLevel = initialVelocity + (50 * GameManager.Instance.GetCurrentLevel());

        SetupCameras();
    }

    protected void FixedUpdate()
    {
        // MAIN button
        //
        // On enabled
        // AND when the ball isn't moving yet
        // ---> Launch the ball.
        if (Input.GetAxisRaw("Fire1") != 0 && Input.GetAxisRaw("Fire2") == 0 && !GameManager.Instance.IsPlaying())
        {
            LaunchBall();
        }
        // On enabled
        // AND the camera switching is enabled too
        // AND the ball is already moving on
        // ---> Activate the ball camera.
        else if (Input.GetAxisRaw("Fire1") != 0 && _cameraSwitchEnabled && GameManager.Instance.IsPlaying())
        {
            EnableBallCamera();
        }
        // On disabled
        // ---> deactivate the ball camera
        else
        {
            DisableBallCamera();
        }

        // SECONDARY button
        // On enabled
        // AND the camera switching is enabled too
        // AND the ball is already movin on
        // ---> The time slows down and a random force is applied to the ball.
        if (Input.GetAxisRaw("Fire2") != 0 && _cameraSwitchEnabled  && GameManager.Instance.IsPlaying())
        {
            RandomizeAndSlowBall();
        }
        // On disabled
        // ---> Time is normal.
        else
        {
            Time.timeScale = 1;
        }

        HandleSpeedBoundaries();
    }

    /// <summary>
    /// Detaches the ball from paddle and applies a force to launch the ball.
    /// </summary>
    private void LaunchBall()
    {
        GameManager.Instance.SetPlaying(true);

        // Enable camera switching after first second of playing.
        _cameraSwitchEnabled = false;
        Invoke("EnableCameraSwitch", 1);

        // Detach from paddle.
        transform.parent = null;
        _rigidbody.isKinematic = false;

        // Add some natural force.
        _rigidbody.AddForce(new Vector3(_initialVelocityPerLevel, _initialVelocityPerLevel, 0));

        // Add some rotation.
        _rigidbody.AddTorque(new Vector3(7, 7, 0));
    }

    /// <summary>
    /// Adds a random force to the ball and slows down the timeScale.
    /// </summary>
    private void RandomizeAndSlowBall()
    {
        _rigidbody.AddForce(new Vector3(
                _initialVelocityPerLevel * _negativeOrPositiveAxeOptions[Random.Range(0, _negativeOrPositiveAxeOptions.Length)],
                _initialVelocityPerLevel * _negativeOrPositiveAxeOptions[Random.Range(0, _negativeOrPositiveAxeOptions.Length)],
                0));
        Time.timeScale = 0.2f;
    }

    /// <summary>
    /// Yes, this is a hack. Sometimes the ball increses the speed.
    /// With this hack I'm limiting to an a reasonable ammount.
    /// </summary>
    private void HandleSpeedBoundaries()
    {
        if (_rigidbody.velocity.magnitude > _maxVelocityMagnitude)
        {
            _rigidbody.velocity = _rigidbody.velocity.normalized * _maxVelocityMagnitude;
        }
        else if (_rigidbody.velocity.magnitude < _minVelocityMagnitude)
        {
            _rigidbody.velocity = _rigidbody.velocity.normalized * _minVelocityMagnitude;
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

    /// <summary>
    /// Activates the ball camera and disables main.
    /// </summary>
    private void EnableBallCamera()
    {
        _mainCamera.enabled = false;
        _ballCamera.enabled = true;
    }

    /// <summary>
    /// Deactivates the ball camera and activates main.
    /// </summary>
    private void DisableBallCamera()
    {
        _ballCamera.enabled = false;
        _mainCamera.enabled = true;
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
