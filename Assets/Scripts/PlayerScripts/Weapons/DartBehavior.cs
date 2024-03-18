using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartBehavior : MonoBehaviour
{
    [Header("Variables")]
    public int damage;
    public int sharpness;
    public float range;

    private Health bl;
    private Rigidbody rb;
    private BoxCollider cd;
    
    private int bloonsHit = 0;
    private Vector3 startPoint;
    public Vector3 velocity;

    private void Awake()
    {
        transform.SetParent(GameObject.Find("Darts").transform);
        rb = GetComponent<Rigidbody>();
        cd = GetComponent<BoxCollider>();
        startPoint = transform.position;
        gameObject.name = GetInstanceID().ToString();
    }

    private void Update()
    {
        CheckDistance();
        CheckBloonsHit();
        rb.velocity = velocity;
    }

    private void CheckDistance() // Checks if current position is further than maxDistance if so destroys game object
    {
        if ((transform.position - startPoint).magnitude >= range)
            Destroy(gameObject);
    }
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("hit");
        if (collision.gameObject.layer ==  LayerMask.NameToLayer("Bloons"))
        {
            bl = collision.gameObject.GetComponent<Health>();
            bl.dartID = gameObject.name;
            bl.TakeDamage(damage);
            bloonsHit += 1;
        }
        else if (collision.gameObject.layer ==  LayerMask.NameToLayer("Ground"))
        {
            rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
            StartCoroutine(Decay());
        }
    }
    private IEnumerator Decay()
    {
        Debug.Log("Decay");
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
    
    private void CheckBloonsHit()
    {
        if (bloonsHit >= sharpness)
        {
            Destroy(gameObject);
        }
    }
}
