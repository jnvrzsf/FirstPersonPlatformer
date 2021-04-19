using UnityEngine;

public class RotateSun : MonoBehaviour
{
    private const float rotationSpeed = 50;

    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}
