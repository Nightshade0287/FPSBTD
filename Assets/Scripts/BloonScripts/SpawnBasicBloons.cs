using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Vector3 = UnityEngine.Vector3;

[System.Serializable]
public class BloonSpawnOption
{
    public GameObject bloonPrefab;
    public float spawnWeight;
}

public class SpawnBasicBloons : MonoBehaviour
{
    [Header("Variables")]
    public float spawnDelay;
    public float spawnRadius;
    public BloonSpawnOption[] bloonSpawnOptions;

    [Header("References")]
    public Path[] paths;
    public Transform BloonHolder;

    private float totalSpawnWeight;

    // Start is called before the first frame update
    void Start()
    {
        CalculateTotalSpawnWeight();
        StartCoroutine(SpawnBloonsWithDelay());
    }

    private void CalculateTotalSpawnWeight()
    {
        foreach (var option in bloonSpawnOptions)
        {
            totalSpawnWeight += option.spawnWeight;
        }
    }

    private IEnumerator SpawnBloonsWithDelay()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnDelay);
            SpawnBloonOnRandomPath();
        }
    }

    private void SpawnBloonOnRandomPath()
    {
        Path randomPath = paths[Random.Range(0, paths.Length)];
        GameObject randomBloon = GetRandomWeightedBloon();
        Vector3 spawnPoint = randomPath.transform.GetChild(0).transform.position;
        spawnPoint.y =  + randomBloon.GetComponent<NavMeshAgent>().baseOffset;
        Vector3 randomSpawn = GetRandomSpawnPoint(spawnPoint);
        GameObject bloon = Instantiate(randomBloon, randomSpawn, transform.rotation);
        SetupBloonPath(bloon, randomPath);
    }

    private Vector3 GetRandomSpawnPoint(Vector3 spawnPoint)
    {
        float randomX = Random.Range(-spawnRadius, spawnRadius);
        float randomZ = Random.Range(-spawnRadius, spawnRadius);
        return spawnPoint + new Vector3(randomX, 0f, randomZ);
    }

    private GameObject GetRandomWeightedBloon()
    {
        float randomValue = Random.Range(0f, totalSpawnWeight);

        foreach (var option in bloonSpawnOptions)
        {
            if (randomValue <= option.spawnWeight)
            {
                return option.bloonPrefab;
            }

            randomValue -= option.spawnWeight;
        }

        return bloonSpawnOptions[0].bloonPrefab; // Default to the first Bloon if something goes wrong.
    }

    private void SetupBloonPath(GameObject bloon, Path path)
    {
        bloon.transform.parent = BloonHolder;
        bloon.GetComponent<BloonMovement>().path = path;
    }
}



