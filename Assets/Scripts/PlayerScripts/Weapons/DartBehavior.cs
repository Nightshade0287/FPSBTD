using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class DartBehavior : MonoBehaviour
{
    [Header("Variables")]
    public int damage;
    public int sharpness;
    public float range;
    public float gravity = 0;
    public float HitRaduis;
    [Header("References")]
    public LayerMask hitLayers;
    public Vector3 direction;
    public float bulletSpeed;
    [SerializeField]
    private int bloonsHit = 0;
    private float timeSinceShot;
    private Vector3 startPoint;
    private Vector3 lastPostition;
    public List<GameObject> bloonHitList = new List<GameObject>();
    private void Awake()
    {
        timeSinceShot = 0f;
        transform.SetParent(GameObject.Find("Darts").transform);
        startPoint = transform.position;
        //gameObject.name = GetInstanceID().ToString();
    }
    private void Update()
    {
        timeSinceShot += Time.deltaTime;
        CheckDistance();
        lastPostition = transform.position;
        transform.position = CalculatePosition(startPoint, direction, bulletSpeed, -gravity, timeSinceShot);
        CheckForBloon();
    }
    private void CheckDistance() // Checks if current position is further than maxDistance if so destroys game object
    {
        if ((new Vector3(transform.position.x, 0f, transform.position.z) - new Vector3(startPoint.x, 0f, startPoint.z)).magnitude >= range)
            Destroy(gameObject);
    }
    private IEnumerator Decay()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
    private void BloonsHit(int hit)
    {
        bloonsHit += hit;
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
    private void CheckForBloon()
    {
        Vector3 direction = (transform.position - lastPostition).normalized;
        float maxDistance = Vector3.Distance(transform.position, lastPostition);
        Ray ray = new Ray(lastPostition, direction);
        Debug.DrawRay(lastPostition, direction * maxDistance);
        RaycastHit[] hits = Physics.SphereCastAll(ray, HitRaduis,  maxDistance, hitLayers, QueryTriggerInteraction.UseGlobal);
        foreach(RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Bloons"))
            {
                GameObject bloonParent = hit.collider.transform.parent.parent.gameObject;
                if(!bloonHitList.Contains(bloonParent))
                {
                    if(bloonsHit < sharpness)
                    {
                        Health bloonHealth = bloonParent.GetComponent<Health>();
                        bloonHealth.dart = gameObject;
                        bloonHealth.TakeDamage(damage);
                        bloonHitList.Add(hit.collider.gameObject);
                        bloonsHit += 1;
                    }
                    else Destroy(gameObject);
                }
            }
            else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                Destroy(gameObject);
            }
            
        }
    }
}
