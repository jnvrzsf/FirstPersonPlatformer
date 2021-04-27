using UnityEngine;

public class PushableCube : MonoBehaviour
{
    private Rigidbody rb;
    private Color edgeColor = Color.green;

    private Vector3 v3FrontTopLeft;
    private Vector3 v3FrontTopRight;
    private Vector3 v3FrontBottomLeft;
    private Vector3 v3FrontBottomRight;
    private Vector3 v3BackTopLeft;
    private Vector3 v3BackTopRight;
    private Vector3 v3BackBottomLeft;
    private Vector3 v3BackBottomRight;

    private Bounds bounds;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        bounds = GetComponent<MeshFilter>().mesh.bounds;
    }

    void Update()
    {
        CalculateVertexPositions();
        DrawEdges();
    }

    void CalculateVertexPositions()
    {
        Vector3 v3Center = bounds.center;
        Vector3 v3Extents = bounds.extents;

        v3FrontTopLeft = new Vector3(v3Center.x - v3Extents.x, v3Center.y + v3Extents.y, v3Center.z - v3Extents.z);  // Front top left corner
        v3FrontTopRight = new Vector3(v3Center.x + v3Extents.x, v3Center.y + v3Extents.y, v3Center.z - v3Extents.z);  // Front top right corner
        v3FrontBottomLeft = new Vector3(v3Center.x - v3Extents.x, v3Center.y - v3Extents.y, v3Center.z - v3Extents.z);  // Front bottom left corner
        v3FrontBottomRight = new Vector3(v3Center.x + v3Extents.x, v3Center.y - v3Extents.y, v3Center.z - v3Extents.z);  // Front bottom right corner
        v3BackTopLeft = new Vector3(v3Center.x - v3Extents.x, v3Center.y + v3Extents.y, v3Center.z + v3Extents.z);  // Back top left corner
        v3BackTopRight = new Vector3(v3Center.x + v3Extents.x, v3Center.y + v3Extents.y, v3Center.z + v3Extents.z);  // Back top right corner
        v3BackBottomLeft = new Vector3(v3Center.x - v3Extents.x, v3Center.y - v3Extents.y, v3Center.z + v3Extents.z);  // Back bottom left corner
        v3BackBottomRight = new Vector3(v3Center.x + v3Extents.x, v3Center.y - v3Extents.y, v3Center.z + v3Extents.z);  // Back bottom right corner

        v3FrontTopLeft = transform.TransformPoint(v3FrontTopLeft);
        v3FrontTopRight = transform.TransformPoint(v3FrontTopRight);
        v3FrontBottomLeft = transform.TransformPoint(v3FrontBottomLeft);
        v3FrontBottomRight = transform.TransformPoint(v3FrontBottomRight);
        v3BackTopLeft = transform.TransformPoint(v3BackTopLeft);
        v3BackTopRight = transform.TransformPoint(v3BackTopRight);
        v3BackBottomLeft = transform.TransformPoint(v3BackBottomLeft);
        v3BackBottomRight = transform.TransformPoint(v3BackBottomRight);
    }

    void DrawEdges()
    {
        Debug.DrawLine(v3FrontTopLeft, v3FrontTopRight, edgeColor);
        Debug.DrawLine(v3FrontTopRight, v3FrontBottomRight, edgeColor);
        Debug.DrawLine(v3FrontBottomRight, v3FrontBottomLeft, edgeColor);
        Debug.DrawLine(v3FrontBottomLeft, v3FrontTopLeft, edgeColor);

        Debug.DrawLine(v3BackTopLeft, v3BackTopRight, edgeColor);
        Debug.DrawLine(v3BackTopRight, v3BackBottomRight, edgeColor);
        Debug.DrawLine(v3BackBottomRight, v3BackBottomLeft, edgeColor);
        Debug.DrawLine(v3BackBottomLeft, v3BackTopLeft, edgeColor);

        Debug.DrawLine(v3FrontTopLeft, v3BackTopLeft, edgeColor);
        Debug.DrawLine(v3FrontTopRight, v3BackTopRight, edgeColor);
        Debug.DrawLine(v3FrontBottomRight, v3BackBottomRight, edgeColor);
        Debug.DrawLine(v3FrontBottomLeft, v3BackBottomLeft, edgeColor);
    }

    /// <summary>
    /// When the cube falls off the platform its Y axis position isn't constrained anymore, it can fall.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeathZone"))
        {
            Destroy(gameObject);
        }
    }
}
