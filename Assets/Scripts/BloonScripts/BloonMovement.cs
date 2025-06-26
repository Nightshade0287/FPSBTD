using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BloonMovement : MonoBehaviour
{
    [Header("Variables")]
    public float speedMultiplier;
    public int rbe; //Red Bloon Equivilent
    [Header("References")]
    public Path path;
    private NavMeshAgent agent;
    public int currentWaypoint = 0;
    private float baseSpeed = 3f;
    private Economy_Health economy;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = baseSpeed * speedMultiplier;
        agent.SetDestination(path.waypoints[currentWaypoint].position);
        economy = GameObject.Find("Economy/Health").GetComponent<Economy_Health>();
    }

    public void Update()
    {
        PathMovement();
        if(agent.velocity.magnitude > agent.speed * 5)
        {
            agent.velocity = Vector3.zero;
        }
        
    }
    public void UpdateChild(GameObject bloon)
    {
        bloon.GetComponent<BloonMovement>().currentWaypoint = currentWaypoint;
        bloon.GetComponent<BloonMovement>().path = path;
        bloon.GetComponent<NavMeshAgent>().velocity = GetComponent<NavMeshAgent>().velocity;
        bloon.transform.SetParent(transform.parent);
    }

    public void PathMovement()
    {
        if(Vector3.Distance(transform.position, path.waypoints[currentWaypoint].position) < 3f)
        {
            if(currentWaypoint < path.waypoints.Count - 1)
            {
                currentWaypoint++;
                agent.SetDestination(path.waypoints[currentWaypoint].position);
            }
            else
            {
                economy.UpdateHealth(rbe);
                //GlobalEvents.BloonPassed();
                Destroy(gameObject);
            }
        }
    }
}
