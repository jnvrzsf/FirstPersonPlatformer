using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityCube : MonoBehaviour, IPickupable
{
    private Rigidbody rb;
    private Carrier carrier;
    private bool isPickedUp => carrier != null;

    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;

    private float currentDistance => Vector3.Distance(carrier.carryPoint.transform.position, transform.position);

    [SerializeField] private float magnitudeLimit;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void FollowCarrier(float maxDistance)
    {
        Vector3 direction = (carrier.carryPoint.transform.position - transform.position).normalized;
        float speed = Mathf.SmoothStep(minSpeed, maxSpeed, currentDistance / maxDistance) * Time.fixedDeltaTime;
        rb.velocity = direction * speed;
    }

    public void SetToPickedUp(Carrier c)
    {
        carrier = c;
        rb.useGravity = false;
    }

    public void SetToDropped()
    {
        carrier = null;
        rb.useGravity = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isPickedUp && collision.relativeVelocity.magnitude > magnitudeLimit)
        {
            carrier.Drop();
        }
    }
}
