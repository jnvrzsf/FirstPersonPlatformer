using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindableRB : Rewindable
{
    private LinkedList<Record> records;
    private Rigidbody rb;
    public override int currentRecordCount { get => records.Count; }

    protected override void Awake()
    {
        records = new LinkedList<Record>();
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

    private void ReapplyForces()
    {
        Record record = records.First.Value;
        rb.velocity = record.Velocity;
        rb.angularVelocity = record.AngularVelocity;
    }

    protected override void Record()
    {
        records.AddFirst(new Record(transform.position, transform.rotation, rb.velocity, rb.angularVelocity));
        if (records.Count > maximumCount)
        {
            records.RemoveLast();
        }
    }

    protected override void Rewind()
    {
        if (records.Count > 1)
        {
            Record record = records.First.Value;
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
