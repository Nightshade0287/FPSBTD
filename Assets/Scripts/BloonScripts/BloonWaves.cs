using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.AI;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using UnityEngine.InputSystem;
using System.Threading;

[System.Serializable]
public class Bloon
{
    public GameObject prefab;
    public int amount;
    public Vector2 timeStamps;
}

[System.Serializable]
public class Round
{
    public Bloon[] bloons;
}
public class BloonWaves : MonoBehaviour
{
    public float spawnRadius;
    public Round[] rounds;
    public int roundIndex = 0;
    public int bloonsLeftInRound = 0;
    public int totalBloons;
    [Range(0, 100)]
    public float roundPercentage;
    public bool roundOver = true;
    [Header("References")]
    public Path[] paths;
    public Transform BloonHolder;
    private bool speedUp;
    private PlayerUI playerUI;
    private Economy_Health economy;
    private int rewardAmount = 100;
    void Start()
    {
        playerUI = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerUI>();
        economy = GameObject.Find("GameManager").GetComponent<Economy_Health>();
        playerUI.UpdateRound(roundIndex);
    }
    void Update()
    {
        if (!roundOver)
        {
            roundPercentage = (float)(bloonsLeftInRound + BloonHolder.childCount) / totalBloons * 100;
            if (roundPercentage == 0)
            {
                roundOver = true;
                economy.UpdateMoney(rewardAmount + roundIndex - 1);
                playerUI.UpdateRound(roundIndex);
            }
        }
    }

    public void SpeedUpTime(InputAction.CallbackContext ctx)
    {
        if(ctx.performed)
        {
            speedUp = !speedUp;
            if(speedUp)
                Time.timeScale = 3f;
            else
                Time.timeScale = 1f;
        }
    }
    public void StartNextRound()
    {
        if(roundOver)
        {
            roundOver = false;
            foreach(Bloon bloon in rounds[(roundIndex - 1) % rounds.Length].bloons)
            {
                bloon.amount *= (int)Mathf.Pow(4, (roundIndex - 1) / rounds.Length);
                StartCoroutine(StartBloonSpawn(bloon));
                totalBloons += bloon.amount;
            }
            bloonsLeftInRound = totalBloons;
            roundIndex++;
        }
    }
    IEnumerator StartBloonSpawn(Bloon bloon)
    {
        yield return new WaitForSeconds(bloon.timeStamps.x);
        int bloonsLeft = bloon.amount;
        if(bloonsLeft != 0)
        {
            SpawnBloonOnRandomPath(bloon.prefab);
            bloonsLeft--;
        }
        float timer = 0f;
        float timeDelay = (bloon.timeStamps.y - bloon.timeStamps.x) / bloonsLeft;
        while(bloonsLeft > 0)
        {
            timer += Time.deltaTime;
            int bloonsPerFrame = Mathf.Clamp(Mathf.FloorToInt(timer / timeDelay), 0, 1);
            if(bloonsLeft - bloonsPerFrame < 0)
                bloonsPerFrame = bloonsLeft;
            for(int i = 0; i < bloonsPerFrame; i++)
            {
                SpawnBloonOnRandomPath(bloon.prefab);
                bloonsLeft--;
                timer = timer % timeDelay;
            }
            yield return null;
        }
    }
    private void SpawnBloonOnRandomPath(GameObject bloon)
    {
        bloonsLeftInRound--;
        Path randomPath = paths[Random.Range(0, paths.Length)];
        Vector3 spawnPoint = randomPath.transform.GetChild(0).transform.position;
        spawnPoint.y += bloon.GetComponent<NavMeshAgent>().baseOffset;
        Vector3 randomSpawn = GetRandomSpawnPoint(spawnPoint);
        Quaternion spawnRotation = Quaternion.LookRotation(-(spawnPoint - randomPath.transform.GetChild(1).transform.position).normalized);
        GameObject currentbloon = Instantiate(bloon, randomSpawn, spawnRotation);
        SetupBloonPath(currentbloon, randomPath);
    }
    private void SetupBloonPath(GameObject bloon, Path path)
    {
        bloon.transform.parent = BloonHolder;
        bloon.GetComponent<BloonMovement>().path = path;
    }
    private Vector3 GetRandomSpawnPoint(Vector3 spawnPoint)
    {
        float randomX = Random.Range(-spawnRadius, spawnRadius);
        float randomZ = Random.Range(-spawnRadius, spawnRadius);
        return spawnPoint + new Vector3(randomX, 0f, randomZ);
    }
}
