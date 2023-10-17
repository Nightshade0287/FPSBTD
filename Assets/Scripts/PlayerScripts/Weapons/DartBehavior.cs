using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartBehavior : MonoBehaviour
{
    [Header("Variables")]
    public int damage;
    public int sharpness;
    public float maxDistance;
    public int BloonsLayer;
    public int GroundLayer;

    private Health bl;
    private Rigidbody rb;
    private BoxCollider cd;
    
    private int bloonsHit = 0;
    private Vector3 startPoint;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cd = GetComponent<BoxCollider>();
        startPoint = transform.position;
    }

    private void Update()
    {
        CheckDistance();
        CheckBloonsHit();
    }

    private void CheckDistance() // Checks if current position is further than maxDistance if so destroys game object
    {
        if ((transform.position - startPoint).magnitude >= maxDistance)
            Destroy(gameObject);
    }

    void OnTriggerExit(Collider collider) //if hits bloon subtracks bloon health else destroys game object
    {
        if (collider.gameObject.layer == BloonsLayer)
        {
            bl = collider.gameObject.GetComponent<Health>();
            bl.TakeDamage(damage);
            bloonsHit += 1;
        }
        else if (collider.gameObject.layer == GroundLayer)
        {
            Destroy(gameObject);
        }
    }

    

    private void CheckBloonsHit()
    {
        if (bloonsHit >= sharpness)
        {
            Destroy(gameObject);
        }
    }
}
