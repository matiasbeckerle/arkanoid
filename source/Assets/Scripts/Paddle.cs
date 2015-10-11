using UnityEngine;
using System.Collections;

public class Paddle : MonoBehaviour
{
    /// <summary>
    /// Horizontal limits to move the paddle.
    /// </summary>
    public Boundary boundary;

    /// <summary>
    /// Paddle's speed.
    /// </summary>
    public float speed = 20;

    /// <summary>
    /// Store the direction of the paddle's movement.
    /// </summary>
    Vector3 _movement;

    /// <summary>
    /// Rigidbody reference.
    /// </summary>
    private Rigidbody _rigidbody;

    protected void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    protected void FixedUpdate()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        _movement.Set(horizontalInput, 0, 0);
        _movement = _movement.normalized * speed * Time.deltaTime;

        Vector3 nextPosition = transform.position + _movement;
        nextPosition.x = Mathf.Clamp(nextPosition.x, boundary.xMin, boundary.xMax);

        _rigidbody.MovePosition(nextPosition);
    }

    [System.Serializable]
    public class Boundary
    {
        public float xMin, xMax;
    }
}
