using UnityEngine;

public class NonPlayerTrigger : MonoBehaviour
{
    public bool isPickupableOverlapping { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Layers.PickedUp)
        {
            isPickupableOverlapping = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == Layers.PickedUp)
        {
            isPickupableOverlapping = false;
        }
    }
}
