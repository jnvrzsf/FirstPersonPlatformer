using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    /// <summary>
    /// Can be collected by player or other objects.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        // save data
        Destroy(gameObject);
        Debug.Log("Collectible destroyed");
    }
}
