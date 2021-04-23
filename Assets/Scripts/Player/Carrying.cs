﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrying : MonoBehaviour
{
    public Transform carryPoint;
    private InputManager input;
    private RayFromCamera ray;
    private Pickupable pickupObject;
    private CursorController cursor;
    private PlayerMovement player;
    private PlayerTrigger playerTrigger;

    private bool isCarrying => pickupObject != null;
    private float distance => Vector3.Distance(carryPoint.position, pickupObject.transform.position);
    private float maxDistance = 5f;

    private void Awake()
    {
        input = FindObjectOfType<InputManager>();
        ray = GetComponent<RayFromCamera>();
        cursor = FindObjectOfType<CursorController>();
        player = GetComponent<PlayerMovement>();
        playerTrigger = GetComponentInChildren<PlayerTrigger>();
    }

    private void Update()
    {
        if (isCarrying)
        {
            if (!playerTrigger.isPickupableOverlapping && (input.PressedAction || distance > maxDistance))
            {
                Drop();
            }
        }
        else
        {
            if (input.PressedAction)
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
            if (player.pickupablesUnderPlayer.Count > 0)
            {
                foreach (GameObject pickupable in player.pickupablesUnderPlayer)
                {
                    if (GameObject.ReferenceEquals(ray.hitInfo.collider.gameObject, pickupable))
                    {
                        return;
                    }
                }
            }

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