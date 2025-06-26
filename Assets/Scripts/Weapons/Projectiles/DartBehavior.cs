using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;
public class DartBehavior : MonoBehaviour
{
    [Header("Variables")]
    public List<DamageTypes> damageTypes = new List<DamageTypes>();
    public List<DamageBuff> damageBuffs = new List<DamageBuff>();
    public int damage;
    public int pierce;
    public float lifeSpan = 2f;
    public float gravity = 0;
    public float HitRaduis;
    [Header("References")]
    public LayerMask hitLayers;
    public Vector3 direction;
    public float dartSpeed;
    [SerializeField]
    public int bloonsHit = 0;
    protected float timeSinceShot;
    protected Vector3 startPoint;
    protected Vector3 lastPostition;
    public List<int> bloonHitList = new List<int>();
    protected virtual void Start()
    {
        timeSinceShot = 0f;
        transform.SetParent(GameObject.Find("Darts").transform);
        startPoint = transform.position;
        StartCoroutine(Decay());
    }
    protected virtual void Update()
    {
        timeSinceShot += Time.deltaTime;
        lastPostition = transform.position;
        transform.position = CalculatePosition(startPoint, direction, dartSpeed, -gravity, timeSinceShot);
        CheckForBloons();
        transform.rotation = Quaternion.LookRotation(transform.position - lastPostition, Vector3.up);
    }
    protected virtual IEnumerator Decay()
    {
        yield return new WaitForSeconds(lifeSpan);
        Destroy(gameObject);
    }
    Vector3 CalculatePosition(Vector3 initialPosition, Vector3 initialDireciton, float speed, float gravity, float time)
    {
        Vector3 newPosition = initialPosition + dartSpeed * time * initialDireciton;
        newPosition.y += 0.5f * gravity * time * time;
        return newPosition;
    }

    public virtual void BloonHitAction(GameObject bloon, List<GameObject> bloonOrder)
    {
        Health bloonHealth = bloon.GetComponent<Health>();
        bloonHealth.dart = gameObject;
        bloonHealth.TakeDamage(damage + AddDamageBuff(bloonHealth));
        bloonHitList.Add(bloon.GetInstanceID());
        bloonsHit += 1;
        if (bloonsHit >= pierce) Destroy(gameObject);
    }
    public virtual void CheckForBloons()
    {
        Vector3 direction = (transform.position - lastPostition).normalized;
        float maxDistance = Vector3.Distance(transform.position, lastPostition);
        Ray ray = new Ray(lastPostition, direction);
        Debug.DrawRay(lastPostition, direction * maxDistance);
        RaycastHit[] hits = Physics.SphereCastAll(ray, HitRaduis, maxDistance, hitLayers, QueryTriggerInteraction.UseGlobal);

        // Create a list to hold the bloons ordered by distance
        List<GameObject> bloonOrder = new List<GameObject>();

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Bloons"))
            {
                if (!bloonHitList.Contains(hit.collider.gameObject.GetInstanceID())) bloonOrder.Add(hit.collider.gameObject);
            }
            else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                Destroy(gameObject);
            }
        }

        // Sort the list by distance from last position
        bloonOrder.Sort((bloon1, bloon2) =>
        {
            float distanceToBloon1 = Vector3.Distance(bloon1.transform.position, lastPostition);
            float distanceToBloon2 = Vector3.Distance(bloon2.transform.position, lastPostition);
            return distanceToBloon1.CompareTo(distanceToBloon2);
        });

        foreach (GameObject bloon in bloonOrder)
        {
            Health bloonHealth = bloon.GetComponent<Health>();
            bool canPop = false; // Start as false, we will change to true if any match is found
            canPop = DamageBloonInteraction.CanPop(bloonHealth.bloonTypes, damageTypes);
            if (canPop && bloonsHit < pierce)
            {
                BloonHitAction(bloon, bloonOrder);
            }
        }
    }
    public int AddDamageBuff(Health bloonHealth)
    {
        int totalBuff = 0;
        foreach (BloonTypes bloonType in bloonHealth.bloonTypes)
        {
            foreach (DamageBuff damageBuff in damageBuffs)
            {
                if (damageBuff.bloonType == bloonType) totalBuff += damageBuff.buffAmount;
            }
        }
        return totalBuff;
    }
}
