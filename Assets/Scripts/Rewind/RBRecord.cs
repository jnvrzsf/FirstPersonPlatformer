﻿using UnityEngine;

public struct RBRecord
{ 
    public Vector3 Position { get; set; }
    public Quaternion Rotation { get; set; }
    public Vector3 Velocity { get; set; }
    public Vector3 AngularVelocity { get; set; }

    public RBRecord(Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 angularVelocity)
    {
        Position = position;
        Rotation = rotation;
        Velocity = velocity;
        AngularVelocity = angularVelocity;
    }
}
