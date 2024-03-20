using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartBehavior : MonoBehaviour
{
    [Header("Variables")]
    public int damage;
    public int sharpness;
    public float range;
    public float gravity = 0;
    private Health bl;
    public Rigidbody rb;
    private BoxCollider cd;
    
    private int bloonsHit = 0;
    private Vector3 startPoint;
    public Vector3 direction;
    public float bulletSpeed;
    private float timeSinceShot;
    private void Awake()
    {
        timeSinceShot = 0f;
        transform.SetParent(GameObject.Find("Darts").transform);
        rb = GetComponent<Rigidbody>();
        cd = GetComponent<BoxCollider>();
        startPoint = transform.position;
        gameObject.name = GetInstanceID().ToString();
    }

    private void Update()
    {
        timeSinceShot += Time.deltaTime;
        CheckDistance();
        CheckBloonsHit();
        rb.velocity = (CalculatePosition(startPoint, direction, bulletSpeed, -gravity, timeSinceShot + 0.1f) - transform.position).normalized * bulletSpeed;
    }

    private void CheckDistance() // Checks if current position is further than maxDistance if so destroys game object
    {
        if ((transform.position - startPoint).magnitude >= range)
            Destroy(gameObject);
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Bloons"))
        {
            bl = collision.gameObject.GetComponent<Health>();
            bl.dartID = gameObject.name;
            bl.TakeDamage(damage);
            bloonsHit += 1;
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
            StartCoroutine(Decay());
        }
    }
    private IEnumerator Decay()
    {
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

    Vector3 CalculatePosition(Vector3 initialPosition, Vector3 initialDireciton, float speed, float gravity, float time)
    {
        Vector3 newPosition = initialPosition + bulletSpeed * time * initialDireciton;
        newPosition.y += 0.5f * gravity * time * time;
        return newPosition;
    }
}
