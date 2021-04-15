using UnityEngine;

public class PlacementField : MonoBehaviour
{
    [SerializeField] private Transform bridge;
    [SerializeField] private Transform start;
    [SerializeField] private Transform stop;
    [SerializeField] private Collider bridgeCollider;
    private float length;
    private bool cubeIsPlaced;

    // separate bridge script amin meghív a drow shrink

    private void Awake()
    {
        length = Vector3.Distance(start.position, stop.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Pickupable>() != null)
        {
            cubeIsPlaced = true;
            // animate down press
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<Pickupable>() != null)
        {
            cubeIsPlaced = false;
        }
    }

    private void Update() // TODO: coroutine instead
    {
        float speed = 10f;
        Vector3 scaleChange = new Vector3(0f, 0f, 1f) * Time.deltaTime; // direction

        if (bridgeCollider.bounds.extents.z > length / 2)
        {
            cubeIsPlaced = false;
        }

        if (cubeIsPlaced)
        {
            //bridge.localScale += scaleChange;
            bridge.localScale += scaleChange * speed;
            bridge.position += scaleChange * speed / 2;
        }

    }
}
