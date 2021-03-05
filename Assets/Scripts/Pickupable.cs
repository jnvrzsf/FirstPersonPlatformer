using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickupable : MonoBehaviour
{
    protected Vector3 originalPosition;
    protected Rigidbody rb;
    protected Carrier carrier;
    protected bool isPickedUp => carrier != null;
    protected float minSpeed = 0;
    protected float maxSpeed = 8000;
    protected float magnitudeLimit = 15;

    private void Start()
    {
        originalPosition = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    public void FollowCarrier(float maxDistance)
    {
        Vector3 direction = (carrier.carryPoint.transform.position - transform.position).normalized;
        float distance = Vector3.Distance(carrier.carryPoint.transform.position, transform.position);
        float speed = Mathf.SmoothStep(minSpeed, maxSpeed, distance / maxDistance) * Time.fixedDeltaTime;
        rb.velocity = direction * speed;
    }
    public abstract void SetToPickedUp(Carrier c);
    public abstract void SetToDropped();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "DestructiveField")
        {
            Debug.Log("Pickupable destroyed");
            carrier?.Drop();
            SetToDropped();
            rb.position = originalPosition;
        }
    }
}
