using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.IK;

public class Health : MonoBehaviour
{
    public int health = 1;
    public GameObject[] ChildBloons;
    public string dartID;

    private SphereCollider cd;
    private Transform dart;

    private SphereCollider dartCd;

    private bool dartTooClose = false;

    void Start()
    {
        cd = GetComponent<SphereCollider>();
    }

    void Update()
    {   
        if(GameObject.Find(dartID)!= null)
        {
            dart = GameObject.Find(dartID).transform;
            dartCd = dart.GetComponent<SphereCollider>();
            Debug.Log("passed");
            float distanceToDart = Vector3.Distance(transform.position, dart.position);
            if(distanceToDart <= cd.radius + dartCd.radius + 0.5f)
            {
                dartTooClose = true;
                Physics.IgnoreCollision(cd, dartCd);
            }
            else
            {
                dartTooClose = false;
                Physics.IgnoreCollision(cd, dartCd, false);
                dartID = "";
            }
        }
    }
    
    public void TakeDamage(int damage)
    {
        if(dartTooClose) return;
        health -= damage;
        if (health <= 0)
        {
            Popped(-health);
        }
    }

    public void Popped(int damage)
    {
        foreach(GameObject child in ChildBloons)
        {
            GameObject bloon = Instantiate(child, transform.position, transform.rotation);
            GetComponent<Pathing>().UpdateChild(bloon);
            bloon.GetComponent<Health>().dartID = dartID;
            bloon.GetComponent<Health>().TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
