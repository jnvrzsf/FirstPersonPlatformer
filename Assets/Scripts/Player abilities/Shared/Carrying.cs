using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrying : MonoBehaviour
{
    public Transform carryPoint;
    private PlayerInput playerInput;
    private RayFromCamera ray;
    private Pickupable pickupObject;
    private CursorController cursor;

    private bool isCarrying => pickupObject != null;
    private float distance => Vector3.Distance(carryPoint.position, pickupObject.transform.position);
    private float maxDistance = 5f;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        ray = GetComponent<RayFromCamera>();
        cursor = FindObjectOfType<CursorController>();
    }

    private void Update()
    {
        if (isCarrying)
        {
            if (playerInput.IsActionPressed || distance > maxDistance)
            {
                Drop();
            }
        }
        else
        {
            if (playerInput.IsActionPressed)
            {
                PickUp();
            }
        }
    }

    private void FixedUpdate()
    {
        if (isCarrying)
        {
            pickupObject.FollowCarrier(maxDistance);
        }
    }

    private void PickUp()
    {
        if (ray.hitSomething && ray.hitInfo.distance < maxDistance)
        {
            Pickupable p = ray.hitInfo.collider.GetComponent<Pickupable>();
            if (p != null)
            {
                if (p.canBePickedUp)
                {
                    Debug.Log("Picked up object");
                    pickupObject = p;
                    pickupObject.SetToPickedUp(this);
                    cursor.HideOnCarry();
                    AudioManager.instance.Play(AudioType.ObjectPickUp);
                }
            }
        }
    }

    public void Drop()
    {
        Debug.Log("Dropped object");
        pickupObject.SetToDropped();
        pickupObject = null;
        cursor.ShowOnDrop();
        AudioManager.instance.Play(AudioType.ObjectDrop);
    }
}
