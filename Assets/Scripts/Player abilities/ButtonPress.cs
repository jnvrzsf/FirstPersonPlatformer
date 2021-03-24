using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    private RayFromCamera ray;
    private PlayerInput playerInput;
    private const float maxDistance = 1;

    void Start()
    {
        ray = GetComponent<RayFromCamera>();
        playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        if (playerInput.isActionPressed)
        {
            if (ray.hitSomething && ray.hitInfo.distance < maxDistance && ray.hitInfo.transform.CompareTag("Button"))
            {
                ray.hitInfo.transform.GetComponent<SpawnButton>().Press();
            }
        }
    }
}
