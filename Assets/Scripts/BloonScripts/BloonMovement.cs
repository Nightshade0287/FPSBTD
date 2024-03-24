using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
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
    private Rigidbody rb;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = baseSpeed * speedMultiplier;
        rb = GetComponent<Rigidbody>();
    }

    public void Update()
    {
        PathMovement();
         rb.velocity = Vector3.ClampMagnitude(Vector3.zero, agent.speed);
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
