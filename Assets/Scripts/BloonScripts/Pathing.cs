using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathing : MonoBehaviour
{
    [Header("Variables")]
    public float speedMultiplier;

    [Header("References")]
    public Transform Path;

    private float baseSpeed = 1.8f;
    private Vector3 waypoint;
    public int currentWaypoint = 1;
    private List<Vector3> Waypoints = new List<Vector3>();
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        GetWaypoints();
        waypoint = Waypoints[currentWaypoint];
        //Physics.IgnoreLayerCollision(7, 7, true);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWaypoint < Waypoints.Count)
        {
            Debug.Log(currentWaypoint);
            rb.velocity = (waypoint - transform.position).normalized * baseSpeed * speedMultiplier;
        }

        else
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.position == waypoint)
        {
            if (currentWaypoint == Path.childCount - 1)
                Destroy(gameObject);
            else
                currentWaypoint++;
                waypoint = Waypoints[currentWaypoint];
        }      
    }

    private void GetWaypoints()
    {
        for (int i = 0; i < Path.childCount; i++)
        {
            Waypoints.Add(Path.GetChild(i).transform.position);
        }
    }

    public void UpdateChild(GameObject bloon)
    {
        bloon.GetComponent<Pathing>().Path = Path;
        bloon.GetComponent<Pathing>().currentWaypoint = currentWaypoint;
    }
}
