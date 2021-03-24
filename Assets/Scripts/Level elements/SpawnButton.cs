using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnButton : MonoBehaviour
{
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private Transform SpawnPoint;
    private GameObject cube;
    private Dissolving dissolveScript;
    private bool canSpawn = true;
    private const float secondsBetweenSpawns = 2f;

    void Start()
    {
        SpawnNewCube();
    }

    public void Press()
    {
        if (canSpawn)
        {
            // TODO: play success sound, animate press down, stay pressed
            dissolveScript.Dissolve();
            SpawnNewCube();
        }
        // TODO: play fail sound
    }

    public void SpawnNewCube()
    {
        cube = Instantiate(cubePrefab, SpawnPoint.position, Quaternion.identity);
        dissolveScript = cube.GetComponent<Dissolving>();
        dissolveScript.OnDestroyed += SpawnNewCube;
        StartCoroutine(ToggleCanSpawn(secondsBetweenSpawns));
    }

    private IEnumerator ToggleCanSpawn(float delay)
    {
        canSpawn = false;
        yield return new WaitForSeconds(delay);
        canSpawn = true;
    }
}
