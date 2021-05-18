using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Bridge : MonoBehaviour
{
    [SerializeField] private Transform start;
    [SerializeField] private Transform stop;
    [SerializeField] private PlacementField placementField;
    private Collider col;
    private Coroutine coroutine;
    private float maxLength;
    private const float minLength = 1;
    private const float speed = 30f;
    

    private void Awake()
    {
        col = GetComponent<Collider>();
        maxLength = Vector3.Distance(start.position, stop.position);
        placementField.FieldPressed += OnFieldPressed;
        placementField.FieldReleased += OnFieldReleased;
    }

    private void OnFieldPressed()
    {
        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = StartCoroutine(Grow());
    }

    private void OnFieldReleased()
    {
        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = StartCoroutine(Shrink());
    }

    private IEnumerator Grow()
    {
        while (col.bounds.extents.z * 2 < maxLength)
        {
            Vector3 scaleChange = new Vector3(0f, 0f, 1f) * Time.deltaTime;
            transform.localScale += scaleChange * speed;
            transform.position += scaleChange * speed / 2;
            yield return null;
        }
        SnapToLength(maxLength);
    }

    private IEnumerator Shrink()
    {
        while (col.bounds.extents.z * 2 > minLength)
        {
            Vector3 scaleChange = new Vector3(0f, 0f, 1f) * Time.deltaTime;
            transform.localScale -= scaleChange * speed;
            transform.position -= scaleChange * speed / 2;
            yield return null;
        }
        SnapToLength(minLength);
    }

    private void SnapToLength(float length)
    {
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, length);
        transform.position = new Vector3(transform.position.x, transform.position.y, start.position.z + length / 2);
    }
}
