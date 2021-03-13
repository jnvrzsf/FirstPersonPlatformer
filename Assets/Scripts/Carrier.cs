using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrier : MonoBehaviour
{
    public Transform carryPoint;
    private Camera cam;
    private Pickupable pickupObject;

    private bool isCarrying => pickupObject != null;
    private float distance => Vector3.Distance(carryPoint.position, pickupObject.transform.position);
    [SerializeField] private float maxDistance;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        bool keyWasPressed = Input.GetKeyDown(KeyCode.E);

        if (isCarrying)
        {
            if (keyWasPressed || distance > maxDistance)
            {
                Drop();
            }
        }
        else
        {
            if (keyWasPressed)
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
        Vector3 pos = new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2);
        Ray ray = cam.ScreenPointToRay(pos);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, maxDistance))
        {
            Pickupable p = hitInfo.collider.GetComponent<Pickupable>();
            if (p != null)
            {
                Debug.Log("Picked up object");
                pickupObject = p;
                pickupObject.SetToPickedUp(this); 
            }
        }
    }

    public void Drop()
    {
        Debug.Log("Dropped object");
        pickupObject.SetToDropped();
        pickupObject = null;
    }
}
