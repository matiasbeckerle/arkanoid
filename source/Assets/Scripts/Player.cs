using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public Boundary boundary;
    public float speed = .5f;

    void Update()
    {
        // Capture user input.
        var horizontalInput = Input.GetAxis("Horizontal");

        // Create a new movement based on user input and speed.
        var movement = new Vector3(horizontalInput, 0, 0);
        movement = movement * speed;

        // Move the player to it's current position plus the movement.
        var newPosition = transform.position + movement;
        newPosition = new Vector3(Mathf.Clamp(newPosition.x, boundary.xMin, boundary.xMax), newPosition.y, newPosition.z);
        transform.position = newPosition;
    }

    [System.Serializable]
    public class Boundary
    {
        public float xMin, xMax;
    }
}
