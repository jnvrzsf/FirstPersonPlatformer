using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    private InputManager input;
    private RayFromCamera ray;
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
