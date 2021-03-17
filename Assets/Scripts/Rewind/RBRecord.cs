using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RBRecord : Record
{
    public Vector3 Velocity { get; set; }
    public Vector3 AngularVelocity { get; set; }

    public RBRecord(Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 angularVelocity) : base (position, rotation)
    {
        Velocity = velocity;
        AngularVelocity = angularVelocity;
    }
}
