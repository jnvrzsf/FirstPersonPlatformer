using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Record
{
    public Vector3 Position { get; set; }
    public Quaternion Rotation { get; set; }

    public Record(Vector3 position, Quaternion rotation)
    {
        Position = position;
        Rotation = rotation;
    }
}
