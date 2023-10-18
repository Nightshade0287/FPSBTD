using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBasicBloons : MonoBehaviour
{   
    [Header("Variables")]
    public float spawnDelay;
    public float spawnRaduis;

    [Header("References")]
    public Transform[] Paths;
    public GameObject[] Bloons;
    public Transform BloonHolder;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnTimer());
    }

    private IEnumerator SpawnTimer()
    {
        yield return new WaitForSeconds(spawnDelay);
        SpawnBloons();
    }

    private void SpawnBloons()
    {
        foreach (Transform path in Paths)
        {
            Transform spawnPoint = path.GetChild(0).transform;
            Vector3 RandomSpawn = spawnPoint.position + new Vector3((Random.Range(-spawnRaduis, spawnRaduis)), 0f, (Random.Range(-spawnRaduis, spawnRaduis))); // Randomize Spawnpoint
            GameObject randomBloon = Bloons[Random.Range(0,Bloons.Length - 1)]; // Get Random Bloon type
            GameObject bloon = Instantiate(randomBloon, RandomSpawn, spawnPoint.rotation);
            bloon.transform.parent = transform.GetChild(0);

            bloon.GetComponent<Pathing>().Path = path;
        }
        StartCoroutine(SpawnTimer());
    }
    
}
