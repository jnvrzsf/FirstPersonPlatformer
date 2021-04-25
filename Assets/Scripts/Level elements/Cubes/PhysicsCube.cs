using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCube : Pickupable
{
    public override void SetToPickedUp(Carrier c)
    {
        carrier = c;
        gameObject.layer = 11;
    }

    public override void SetToDropped()
    {
        carrier = null;
        gameObject.layer = 10;
        // instead: Physics.IgnoreCollision(col, collision.collider, false); delete layer, get it on collision, save reference to collider, reset when dropped
        // ne lehessen lerakni ha benne vok, különben ki is lőheti a playert felfele
    }
}
