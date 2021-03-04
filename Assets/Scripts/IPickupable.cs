using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IPickupable
{
    void FollowCarrier(float maxDistance);
    void SetToPickedUp(Carrier c);
    void SetToDropped();
}
