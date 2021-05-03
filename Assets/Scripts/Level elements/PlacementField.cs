using System;
using UnityEngine;

public class PlacementField : MonoBehaviour
{
    public event Action FieldPressed;
    public event Action FieldReleased;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Pickupable>() != null)
        {
            FieldPressed?.Invoke();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<Pickupable>() != null)
        {
            FieldReleased?.Invoke();
        }
    }
}
