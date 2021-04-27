using UnityEngine;

public class NonPlayerTrigger : MonoBehaviour
{
    public bool isPickupableOverlapping;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PickedUp"))
        {
            isPickupableOverlapping = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PickedUp"))
        {
            isPickupableOverlapping = false;
        }
    }
}
