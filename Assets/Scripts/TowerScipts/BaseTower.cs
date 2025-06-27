using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTower : MonoBehaviour
{
    [Header("Variables")]
    public List<DamageTypes> damageTypes = new List<DamageTypes>();
    public List<DamageBuff> damageBuffs = new List<DamageBuff>();
    public Critical critical;
    public int damage;
    public int pierce;
    public float range;
    public float rangeMultiplier = 1;
    public float lifeSpan;
    public float lifeSpanMultiplier = 1;
    public float dartVelocity;
    public float dartVelocityMultiplier = 1;
    public float shootDelay;
    public float shootDelayMultiplier = 1;
    public float spread;
    public Vector3 shootOffset = Vector3.zero;
    public int dartsPerShot = 1;

    [Header("References")]
    public LayerMask mask;
    public Transform BloonHolder;
    public GameObject dartPrefab;
    public Transform shootPoint;
    protected Transform targetBloon;
    protected bool canShoot = true;
    public TowerInfo towerInfo;

    protected void Start()
    {
        spread /= 1000;
        towerInfo = GetComponent<TowerInfo>();
        if (towerInfo != null) InitializeTower();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        GetClosestBloon();
        if (targetBloon != null && Vector3.Distance(gameObject.transform.position, targetBloon.position) <= range * rangeMultiplier)
        {
            Shoot();
        }
    }

    public void ReplaceDamageType(DamageTypes remove, DamageTypes replace)
    {
        if (damageTypes.Contains(remove))
        {
            damageTypes.Remove(remove);
            damageTypes.Add(replace);
        }
        else if (!damageTypes.Contains(replace)) damageTypes.Add(replace);
    }
    public void AddOrUpdateBuff(BloonTypes type, int amount)
    {
        DamageBuff existingBuff = damageBuffs.Find(buff => buff.bloonType == type);

        if (existingBuff != null)
        {
            existingBuff.buffAmount += amount;
        }
        else
        {
            damageBuffs.Add(new DamageBuff { bloonType = type, buffAmount = amount });
        }
    }
    protected bool CanSeeBloon(Transform bloon)
    {
        if (!DamageBloonInteraction.CanPop(bloon.GetComponent<Health>().bloonTypes, damageTypes))
        {
            return false;
        }
        Transform bloonModel = bloon.Find("Model");
        Quaternion rotation = Quaternion.LookRotation((new Vector3(bloonModel.position.x, transform.position.y, bloonModel.position.z) - transform.position).normalized);
        Vector3 rotatedShootPos = rotation * shootPoint.localPosition;
        Vector3 newShootPos = transform.position + rotatedShootPos;

        Ray ray = new Ray(newShootPos, bloonModel.position - newShootPos);
        RaycastHit hit;
        //Debug.DrawRay(ray.origin, ray.direction * Vector3.Distance(newShootPos, bloonModel.position));
        if (Physics.Raycast(ray, out hit, range * rangeMultiplier, mask))
        {
            if (hit.collider.gameObject.layer == bloon.gameObject.layer)
                return true;
        }
        return false;
    }
    protected void GetClosestBloon()
    {
        Transform closestBloon = null;
        float closestDistance = Mathf.Infinity;
        BloonHolder = GameObject.Find("Bloons").transform;
        if (BloonHolder.childCount != 0)
        {
            foreach (Transform bloon in BloonHolder)
            {
                float distance = Vector3.Distance(transform.position, bloon.position);

                if (distance < closestDistance)
                {
                    if (CanSeeBloon(bloon))
                    {
                        closestBloon = bloon;
                        closestDistance = distance;
                    }
                }
            }

            if (closestBloon != null)
            {
                targetBloon = closestBloon;
            }
        }
    }

    protected void GetStrongestBloon()
    {
        Transform strongestBloon = null;
        float strongest = 0;

        if (BloonHolder.childCount != 0)
        {
            foreach (Transform bloon in BloonHolder)
            {
                float health = bloon.GetComponent<Health>().health;

                if (health > strongest)
                {
                    if (CanSeeBloon(bloon))
                    {
                        strongestBloon = bloon;
                        strongest = health;
                    }
                }
            }

            if (strongestBloon != null)
            {
                targetBloon = strongestBloon;
            }
        }
    }

    // Shoot darts
    protected virtual void Shoot()
    {
        if (!canShoot)
        {
            return;
        }

        for (int i = 0; i < dartsPerShot; i++)
        {
            Transform target = targetBloon.Find("Model").GetChild(0);
            transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
            GameObject dart = Instantiate(dartPrefab, shootPoint.position, shootPoint.rotation);
            DartBehavior dartScript = dart.GetComponent<DartBehavior>();

            // Calculate dart direction with spread
            Vector3 dartDirection = (target.position - shootPoint.position + shootOffset).normalized;

            // Calculate spread offset
            float spreadX = Random.Range(-spread / 2, spread / 2);
            float spreadY = Random.Range(-spread / 2, spread / 2);

            dartDirection = (Quaternion.AngleAxis(spreadX, Vector3.up) * Quaternion.AngleAxis(spreadY, Vector3.right) * dartDirection).normalized;
            dartScript.InitializeDart(this, dartDirection);
        }

        // Set a cooldown before the next shot
        canShoot = false;
        StartCoroutine(ResetShootCooldown());
    }

    // Reset the shooting cooldown
    protected IEnumerator ResetShootCooldown()
    {
        yield return new WaitForSeconds(shootDelay * shootDelayMultiplier);
        canShoot = true;
    }

    public void InitializeTower()
    {
        dartPrefab = towerInfo.dartPrefab;
        damage = towerInfo.damage;
        damageTypes = towerInfo.damageTypes;
        damageBuffs = towerInfo.damageBuffs;
        pierce = towerInfo.pierce;
        range = towerInfo.range;
        lifeSpan = towerInfo.lifeSpan;
        dartVelocity = towerInfo.dartVelocity;
        shootDelay = towerInfo.shootDelay;
        spread = towerInfo.spread;
        dartsPerShot = towerInfo.DartsPerShot;
    }
}
