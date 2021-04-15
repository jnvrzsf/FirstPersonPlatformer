using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindOnDeath : MonoBehaviour
{
    [SerializeField] private FirstPersonCharacterController playerController;
    [SerializeField] private CameraController cameraController;
    private InputManager input;
    private LinkedList<Record> bodyRecords;
    private LinkedList<Quaternion> cameraRecords;
    private bool isDead;
    private bool isRewinding;
    private const float recordTimeInSeconds = 5f;
    public float maximumCount => Mathf.Round(recordTimeInSeconds / Time.fixedDeltaTime);

    void Awake()
    {
        bodyRecords = new LinkedList<Record>();
        cameraRecords = new LinkedList<Quaternion>();
        input = FindObjectOfType<InputManager>();
    }

    private void Update()
    {
        if (isDead)
        {
            if (input.PressedRewind)
            {
                isRewinding = true;
            }
            if (input.ReleasedRewind)
            {
                isRewinding = false;
                UnfreezePlayer();
                isDead = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (isDead)
        {
            if (isRewinding)
            {
                Rewind();
            }
            else
            {
                FreezePlayer();
            }
        }
        else
        {
            Record();
        }
    }

    private void Record()
    {
        bodyRecords.AddFirst(new Record(transform.position, transform.rotation));
        cameraRecords.AddFirst(cameraController.transform.rotation);
        if (bodyRecords.Count > maximumCount && cameraRecords.Count > maximumCount)
        {
            bodyRecords.RemoveLast();
            cameraRecords.RemoveLast();
        }
    }

    private void Rewind()
    {
        if (bodyRecords.Count > 0 && cameraRecords.Count > 0)
        {
            Record record = bodyRecords.First.Value;
            transform.position = record.Position;
            transform.rotation = record.Rotation;
            bodyRecords.RemoveFirst();

            cameraController.transform.rotation = cameraRecords.First.Value;
            cameraRecords.RemoveFirst();
        }
    }

    private void FreezePlayer()
    {
        //playerController.Freeze();
        //cameraController.Freeze();
    }

    private void UnfreezePlayer()
    {
        //playerController.Unfreeze();
        //cameraController.Unfreeze();
    }
}
