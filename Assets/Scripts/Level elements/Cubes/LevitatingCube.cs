public class LevitatingCube : Pickupable
{
    protected override float maxSpeed { get => 3000; }

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
