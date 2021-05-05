public class LevitatingCube : Pickupable
{
    public override void SetToPickedUp(Carrier c)
    {
        base.SetToPickedUp(c);
        rb.isKinematic = false;
    }

    public override void SetToDropped()
    {
        base.SetToDropped();
        rb.isKinematic = true;
    }
}
