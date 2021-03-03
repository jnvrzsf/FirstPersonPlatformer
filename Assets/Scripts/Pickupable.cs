using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : MonoBehaviour
{
    Vector3 lastPosition;

    public void Follow(Transform target, float distance, float smoothing)
    {
        transform.position = Vector3.Lerp(transform.position, target.transform.position + target.transform.forward * distance, smoothing * Time.deltaTime);
        transform.LookAt(new Vector3(
            target.transform.position.x,
            transform.position.y,
            target.transform.position.z
            ));
        lastPosition = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector3 collisionPoint = collision.GetContact(i).point;

        }
        
    }
}
