using UnityEngine;

public class IgnoreCollider : MonoBehaviour
{
    public string ignoreTag; // Tag to ignore collisions with

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object has the specified tag
        if (collision.gameObject.CompareTag(ignoreTag))
        {
            // Ignore the collision between the current object's collider and the other object's collider
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        }
    }
}
