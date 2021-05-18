using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerState))]
public class RewindablePlayer : RewindableRB
{
    [SerializeField] private Transform cam;
    private LinkedList<Quaternion> cameraRecords;
    private PlayerState playerState;

    protected override void Awake()
    {
        base.Awake();
        cameraRecords = new LinkedList<Quaternion>();
        playerState = GetComponent<PlayerState>();
    }

    public override void StartRewinding()
    {
        base.StartRewinding();
        playerState.isRewinding = true;
    }

    public override void StopRewinding()
    {
        base.StopRewinding();
        playerState.isRewinding = false;
    }

    protected override void Record()
    {
        base.Record();
        cameraRecords.AddFirst(cam.rotation);
        if (cameraRecords.Count > maximumCount)
        {
            cameraRecords.RemoveLast();
        }
    }

    protected override void Rewind()
    {
        if (cameraRecords.Count > 1)
        {
            cam.rotation = cameraRecords.First.Value;
            cameraRecords.RemoveFirst();
        }
        base.Rewind();
    }
}
