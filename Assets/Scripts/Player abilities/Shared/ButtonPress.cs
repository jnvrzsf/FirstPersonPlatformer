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
            SpawnButton button = ray.hitInfo.collider.GetComponent<SpawnButton>();
            if (ray.hitSomething && ray.hitInfo.distance < maxDistance && button != null)
            {
                button.TryPress();
            }
        }
    }
}
