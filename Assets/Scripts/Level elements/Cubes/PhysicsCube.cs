using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCube : Pickupable
{
    private bool isGrounded;

    private void FixedUpdate()
    {
        //if (!isPickedUp)
        //{
        //    // check if grounded
        //    RaycastHit hitInfo;
        //    Physics.BoxCast(transform.position, new Vector3(0.5f, 0.5f, 0.5f), Vector3.down, out hitInfo);


        //    if (!isGrounded)
        //    {
        //        ApplyGravity();
        //    }
        //}
    }

    private void ApplyGravity()
    {
        rb.MovePosition(transform.position + Vector3.down * 5 * Time.fixedDeltaTime);
    }

    public override void SetToPickedUp(Carrier c)
    {
        carrier = c;
        rb.useGravity = false;
    }

    public override void SetToDropped()
    {
        carrier = null;
        rb.useGravity = true;
    }
}
