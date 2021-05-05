using UnityEngine;

public class PushableCube : MonoBehaviour
{
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// When the cube falls off the platform its Y axis position isn't constrained anymore, it can fall.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == Layers.Ground)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.DeathZone))
        {
            Destroy(gameObject);
        }
    }
}
