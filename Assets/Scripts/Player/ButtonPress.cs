using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(RayFromCamera)]
public class ButtonPress : MonoBehaviour
{
    private InputManager input;
    private RayFromCamera ray;
    private const float maxDistance = 2;

    void Awake()
    {
        input = FindObjectOfType<InputManager>();
        Assert.IsNotNull(input, "InputManager not found.");
        ray = GetComponent<RayFromCamera>();
    }

    void Update()
    {
        if (input.PressedAction)
        {
            if (ray.hitSomething && ray.hitInfo.distance < maxDistance)
            {
                SpawnButton button = ray.hitInfo.collider.GetComponent<SpawnButton>();
                if (button != null)
                {
                    button.TryPress();
                }
            }
        }
    }
}
