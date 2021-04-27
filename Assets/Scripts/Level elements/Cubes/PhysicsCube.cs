using UnityEngine;

public class PhysicsCube : Pickupable
{
    public override void SetToPickedUp(Carrier c)
    {
        carrier = c;
        gameObject.layer = LayerMask.NameToLayer("PickedUp");
    }

    public override void SetToDropped()
    {
        carrier = null;
        gameObject.layer = LayerMask.NameToLayer("Pickupable");
    }
}
