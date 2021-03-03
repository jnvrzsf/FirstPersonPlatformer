using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : MonoBehaviour
{
    Camera cam;
    GameObject carriedObject;
    bool isCarryingObject;
    public float carryDistance;
    public float pickupDistance;
    public float smoothing;

    void Start()
    {
        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }

    void Update()
    {
        if (isCarryingObject)
        {
            Carry(carriedObject);
            if (Input.GetKeyDown(KeyCode.E))
            {
                Drop();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Pickup();
            }
        }
    }

    private void Pickup()
    {
        Vector3 pos = new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2);
        Ray ray = cam.ScreenPointToRay(pos);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, pickupDistance))
        {
            Pickupable p = hitInfo.collider.GetComponent<Pickupable>();
            if (p != null)
            {
                Debug.Log("Started carrying");
                isCarryingObject = true;
                carriedObject = p.gameObject;
            }
        }
    }

    private void Carry(GameObject o)
    {
        o.GetComponent<Pickupable>().Follow(cam.transform, carryDistance, smoothing);
    }

    private void Drop()
    {
        Debug.Log("Dropped object");
        isCarryingObject = false;
        carriedObject = null;
    }
}
