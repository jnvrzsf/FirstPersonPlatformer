using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    private RayFromCamera ray;
    private InputManager input;
    private const float maxDistance = 2;

    void Awake()
    {
        ray = GetComponent<RayFromCamera>();
        input = FindObjectOfType<InputManager>();
    }

    void Update()
    {
        if (input.PressedAction)
        {
            if (ray.hitSomething && ray.hitInfo.distance < maxDistance && ray.hitInfo.transform.CompareTag("Button"))
            {
                ray.hitInfo.transform.GetComponent<SpawnButton>().TryPress();
            }
        }
    }
}
