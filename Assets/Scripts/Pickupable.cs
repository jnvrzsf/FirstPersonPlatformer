using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickupable : MonoBehaviour
{
    protected Rigidbody rb;
    protected Carrier carrier;
    protected bool isPickedUp => carrier != null;
    protected float minSpeed = 0;
    protected float maxSpeed = 8000;
    protected float magnitudeLimit = 15;

    private void Start()
    {
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
}
