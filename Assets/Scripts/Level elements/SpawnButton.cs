using System.Collections;
using UnityEngine;

public class SpawnButton : MonoBehaviour
{
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private Transform SpawnPoint;
    private Pickupable cube;
    private bool canBePressed = true;
    private Vector3 originalPosition;
    private Coroutine coroutine;

    private const float secondsBetweenPresses = 2f;
    private const float buttonDip = 0.04f;

    private void Start()
    {
        originalPosition = transform.position;
        SpawnNewCube();
    }

    public void TryPress()
    {
        if (canBePressed)
        {
            Press();
            AudioManager.instance.Play(AudioType.ButtonPressSuccess);
        }
        else
        {
            AudioManager.instance.Play(AudioType.ButtonPressFail);
        }
    }

    private void Press()
    {
        cube?.Destroy();
        transform.position = new Vector3(transform.position.x, transform.position.y - buttonDip, transform.position.z);
        coroutine = StartCoroutine(RestoreButton(secondsBetweenPresses));
        SpawnNewCube();
    }

    public void SpawnNewCube()
    {
        cube = Instantiate(cubePrefab, SpawnPoint.position, Quaternion.identity).GetComponent<Pickupable>();
        cube.spawner = this;
        AudioManager.instance.Play(AudioType.CubeSpawn, SpawnPoint.position);
    }

    private IEnumerator RestoreButton(float seconds)
    {
        canBePressed = false;
        Vector3 currentPosition = transform.position;
        float t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / seconds;
            transform.position = Vector3.Lerp(currentPosition, originalPosition, t);
            yield return null;
        }
        canBePressed = true;
    }
}
