using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.IK;

public class Health : MonoBehaviour
{
    public int health = 1;
    public GameObject[] ChildBloons;
    public GameObject dart;
    void Start()
    {

    }

    void Update()
    {   
        // if(GameObject.Find(dartID)!= null)
        // {
        //     dart = GameObject.Find(dartID).transform;
        //     dartCd = dart.GetComponent<SphereCollider>();
        //     float distanceToDart = Vector3.Distance(cd.center + transform.position, dart.position);
        //     if(distanceToDart >= cd.radius + dartCd.radius + 0.25f)
        //     {
        //         dartCd.isTrigger = false;
        //     }
        // }
    }
    public void TakeDamage(int damage)
    {
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
            Health bloonHealth = bloon.GetComponent<Health>();
            GetComponent<BloonMovement>().UpdateChild(bloon);
            bloonHealth.dart = dart;
            dart.GetComponent<DartBehavior>().bloonHitList.Add(bloon);
            bloonHealth.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
