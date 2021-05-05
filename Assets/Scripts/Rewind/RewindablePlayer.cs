using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindablePlayer : Rewindable
{
    private Rigidbody rb;
    [SerializeField] private Transform cam;
    [SerializeField] private Transform orientation;

    private PlayerState playerState;
    private LinkedList<Record> bodyRecords;
    private LinkedList<Quaternion> cameraRecords;
    public override int currentRecordCount { get => bodyRecords.Count; }

    protected override void Awake()
    {
        playerState = GetComponent<PlayerState>();
        bodyRecords = new LinkedList<Record>();
        cameraRecords = new LinkedList<Quaternion>();
        rb = GetComponent<Rigidbody>();
    }

    public override void StartRewinding()
    {
        base.StartRewinding();
        rb.isKinematic = true;
        playerState.isRewinding = true;
    }

    public override void StopRewinding()
    {
        base.StopRewinding();
        rb.isKinematic = false;
        playerState.isRewinding = false;
        ReapplyForces();
    }

    private void ReapplyForces()
    {
        Record record = bodyRecords.First.Value;
        rb.velocity = record.Velocity;
        rb.angularVelocity = record.AngularVelocity;
    }

    protected override void Record()
    {
        bodyRecords.AddFirst(new Record(transform.position, orientation.rotation, rb.velocity, rb.angularVelocity));
        cameraRecords.AddFirst(cam.rotation);
        if (bodyRecords.Count > maximumCount && cameraRecords.Count > maximumCount)
        {
            bodyRecords.RemoveLast();
            cameraRecords.RemoveLast();
        }
    }

    protected override void Rewind()
    {
        if (bodyRecords.Count > 1 && cameraRecords.Count > 1)
        {
            Record record = bodyRecords.First.Value;
            transform.position = record.Position;
            orientation.rotation = record.Rotation;
            bodyRecords.RemoveFirst();

            cam.rotation = cameraRecords.First.Value;
            cameraRecords.RemoveFirst();
        }
        else
        {
            OnOutOfRecords();
        }
    }
}
