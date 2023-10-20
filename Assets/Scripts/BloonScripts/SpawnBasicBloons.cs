using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public Transform[] Paths;
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
        Transform randomPath = Paths[Random.Range(0, Paths.Length)];
        Transform spawnPoint = randomPath.GetChild(0).transform;
        Vector3 randomSpawn = GetRandomSpawnPoint(spawnPoint);
        GameObject randomBloon = GetRandomWeightedBloon();
        GameObject bloon = Instantiate(randomBloon, randomSpawn, spawnPoint.rotation);
        SetupBloonPath(bloon, randomPath);
    }

    private Vector3 GetRandomSpawnPoint(Transform spawnPoint)
    {
        float randomX = Random.Range(-spawnRadius, spawnRadius);
        float randomZ = Random.Range(-spawnRadius, spawnRadius);
        return spawnPoint.position + new Vector3(randomX, 0f, randomZ);
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

    private void SetupBloonPath(GameObject bloon, Transform path)
    {
        bloon.transform.parent = BloonHolder;
        bloon.GetComponent<Pathing>().Path = path;
    }
}



