using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEditor.SceneManagement;
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
        if(agent.velocity.magnitude > agent.speed + 2)
        {
            agent.velocity = Vector3.zero;
        }
        
    }
    public void UpdateChild(GameObject bloon)
    {
        bloon.GetComponent<BloonMovement>().path = path;
        bloon.GetComponent<BloonMovement>().currentWaypoint = currentWaypoint;
        bloon.GetComponent<NavMeshAgent>().velocity = GetComponent<NavMeshAgent>().velocity;
        bloon.transform.SetParent(transform.parent);
    }

    public void PathMovement()
    {
        if(agent.remainingDistance < 3f)
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
