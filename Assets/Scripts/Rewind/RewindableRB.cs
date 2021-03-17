using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindableRB : Rewindable
{
    private LinkedList<RBRecord> records;
    private Rigidbody rb;

    protected override void Start()
    {
        records = new LinkedList<RBRecord>();
        rb = GetComponent<Rigidbody>();
    }

    protected override void StartRewinding()
    {
        base.StartRewinding();
        rb.isKinematic = true;
    }

    protected override void StopRewinding()
    {
        base.StopRewinding();
        rb.isKinematic = false;
    }

    protected override void Record()
    {
        if (records.Count > Mathf.Round(recordTimeInSeconds / Time.fixedDeltaTime))
        {
            records.RemoveLast();
        }
        records.AddFirst(new RBRecord(transform.position, transform.rotation, rb.velocity, rb.angularVelocity));
    }

    protected override void Rewind()
    {
        if (records.Count > 0)
        {
            RBRecord record = records.First.Value;
            transform.position = record.Position;
            transform.rotation = record.Rotation;
            rb.velocity = record.Velocity;
            rb.angularVelocity = record.AngularVelocity;
            records.RemoveFirst();
        }
    }
}
