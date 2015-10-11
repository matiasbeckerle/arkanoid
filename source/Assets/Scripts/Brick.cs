using UnityEngine;
using System.Collections;

public class Brick : MonoBehaviour
{
    public GameObject explosion; // Particles to be used on destroy.

    void OnCollisionEnter(Collision collision)
    {
        // Notify GameManager.
        GameManager.Instance.DestroyBrick();

        // Add explosion particles and remove it after some seconds.
        var explosionInstance = Instantiate(explosion, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(explosionInstance, 3);

        // Remove brick.
        Destroy(gameObject);
    }
}
