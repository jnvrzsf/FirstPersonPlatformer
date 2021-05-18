using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RewindableRB : Rewindable
{
    protected LinkedList<RBRecord> records;
    protected Rigidbody rb;
    public override int currentRecordCount { get => records.Count; }

    protected override void Awake()
    {
        records = new LinkedList<RBRecord>();
        rb = GetComponent<Rigidbody>();
    }

    public override void StartRewinding()
    {
        base.StartRewinding();
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        rb.isKinematic = true;
    }

    public override void StopRewinding()
    {
        base.StopRewinding();
        if (GetComponent<LevitatingCube>() == null)
        {
            rb.isKinematic = false;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }
        ReapplyForces();
    }

    protected void ReapplyForces()
    {
        RBRecord record = records.First.Value;
        rb.velocity = record.Velocity;
        rb.angularVelocity = record.AngularVelocity;
    }

    protected override void Record()
    {
        records.AddFirst(new RBRecord(transform.position, transform.rotation, rb.velocity, rb.angularVelocity));
        if (records.Count > maximumCount)
        {
            records.RemoveLast();
        }
    }

    protected override void Rewind()
    {
        if (records.Count > 1)
        {
            RBRecord record = records.First.Value;
            transform.position = record.Position;
            transform.rotation = record.Rotation;
            records.RemoveFirst();
        }
        else
        {
            OnOutOfRecords();
        }
    }
}
