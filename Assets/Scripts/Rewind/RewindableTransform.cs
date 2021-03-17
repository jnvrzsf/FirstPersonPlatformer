using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindableTransform : Rewindable
{
    private LinkedList<Vector3> records;

    protected override void Start()
    {
        records = new LinkedList<Vector3>();
    }

    protected override void Record()
    {
        if (records.Count > Mathf.Round(recordTimeInSeconds / Time.fixedDeltaTime))
        {
            records.RemoveLast();
        }
        records.AddFirst(transform.position);
    }

    protected override void Rewind()
    {
        if (records.Count > 0)
        {
            transform.position = records.First.Value;
            records.RemoveFirst();
        }
    }
}
