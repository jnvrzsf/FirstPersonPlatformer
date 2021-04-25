using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindablePlayer : Rewindable
{
    private Rigidbody rb;
    [SerializeField] private Transform cam;
    [SerializeField] private Transform orientation;

    private PlayerState playerState;
    private LinkedList<RBRecord> bodyRecords;
    private LinkedList<Quaternion> cameraRecords;

    protected override void Awake()
    {
        playerState = GetComponent<PlayerState>();
        bodyRecords = new LinkedList<RBRecord>();
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
        if (bodyRecords.Count > 0)
        {
            RBRecord record = bodyRecords.First.Value;
            rb.velocity = record.Velocity;
            rb.angularVelocity = record.AngularVelocity;
        }
    }

    protected override void Record()
    {
        bodyRecords.AddFirst(new RBRecord(transform.position, orientation.rotation, rb.velocity, rb.angularVelocity));
        cameraRecords.AddFirst(cam.rotation);
        if (bodyRecords.Count > maximumCount && cameraRecords.Count > maximumCount)
        {
            bodyRecords.RemoveLast();
            cameraRecords.RemoveLast();
        }
    }

    protected override void Rewind()
    {
        if (bodyRecords.Count > 0 && cameraRecords.Count > 0)
        {
            RBRecord record = bodyRecords.First.Value;
            transform.position = record.Position;
            orientation.rotation = record.Rotation;
            bodyRecords.RemoveFirst();

            cam.rotation = cameraRecords.First.Value;
            cameraRecords.RemoveFirst();
        }
    }
}
