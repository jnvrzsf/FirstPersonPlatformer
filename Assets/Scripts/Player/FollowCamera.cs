using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform followTarget;

    private void Update()
    {
        transform.position = followTarget.position;
    }
}
