using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class PhysicsBasedDarts : DartBehavior
{
    protected override void Start()
    {
        transform.SetParent(GameObject.Find("Darts").transform);
        GetComponent<Rigidbody>().velocity = direction * dartSpeed;
        StartCoroutine(Decay());
    }
    protected override void Update()
    {
        CheckForBloons();
        lastPostition = transform.position;
    }
}

