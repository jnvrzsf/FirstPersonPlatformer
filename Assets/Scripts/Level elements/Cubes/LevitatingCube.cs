using UnityEngine;

public class LevitatingCube : Pickupable
{
    public override void SetToPickedUp(Carrier c)
    {
        carrier = c;
        rb.isKinematic = false;
        gameObject.layer = LayerMask.NameToLayer("PickedUp");
    }

    public override void SetToDropped()
    {
        carrier = null;
        rb.isKinematic = true;
        gameObject.layer = LayerMask.NameToLayer("Pickupable");
    }
}
