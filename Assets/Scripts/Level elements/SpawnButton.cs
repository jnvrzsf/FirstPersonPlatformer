using System.Collections;
using UnityEngine;

public class SpawnButton : MonoBehaviour
{
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private Transform SpawnPoint;
    private Pickupable cube;
    private bool canBePressed = true;
    private Vector3 originalPosition;

    private const float secondsBetweenPresses = 2f;
    private const float buttonDip = 0.05f;

    private AudioObject successSound;
    private AudioObject failSound;

    private void Start()
    {
        originalPosition = transform.position;
        successSound = AudioManager.instance.AddAudioToGameObject(AudioType.ButtonPressSuccess, gameObject);
        failSound = AudioManager.instance.AddAudioToGameObject(AudioType.ButtonPressFail, gameObject);
        SpawnNewCube();
    }

    public void TryPress()
    {
        if (canBePressed)
        {
            Press();
            successSound.source.Play();
        }
        else
        {
            failSound.source.PlayOneShot(failSound.source.clip);
        }
    }

    private void Press()
    {
        cube?.Destroy();
        transform.position = new Vector3(transform.position.x, transform.position.y - buttonDip, transform.position.z);
        StartCoroutine(RestoreButton(secondsBetweenPresses));
        SpawnNewCube();
        AudioManager.instance.PlayOnGameObject(AudioType.CubeSpawn, gameObject);
    }

    public void SpawnNewCube()
    {
        cube = Instantiate(cubePrefab, SpawnPoint.position, Quaternion.identity).GetComponent<Pickupable>();
        cube.spawner = this;
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
