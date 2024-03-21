using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BloonMovement : MonoBehaviour
{
    [Header("Variables")]
    public float speedMultiplier;
    [Header("References")]
    public Path path;
    private NavMeshAgent agent;
    public int currentWaypoint = 0;
    private float baseSpeed = 2f;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = baseSpeed * speedMultiplier;
    }

    public void Update()
    {
        PathMovement();
    }
    public void UpdateChild(GameObject bloon)
    {
        bloon.GetComponent<BloonMovement>().path = path;
        bloon.GetComponent<BloonMovement>().currentWaypoint = currentWaypoint;
        bloon.GetComponent<NavMeshAgent>().velocity = GetComponent<NavMeshAgent>().velocity;
        bloon.transform.SetParent(transform.parent);
        Debug.Log(currentWaypoint);
    }

    public void PathMovement()
    {
        if(agent.remainingDistance < 1f)
        {
            if(currentWaypoint < path.waypoints.Count - 1)
            {
                currentWaypoint++;
                agent.SetDestination(path.waypoints[currentWaypoint].position);
            }
            else
                Destroy(gameObject);
        }
    }
}
