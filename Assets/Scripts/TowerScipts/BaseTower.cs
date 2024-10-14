using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTower : MonoBehaviour
{
    [Header("Variables")]
    public int damage;
    public int pierce;
    public float range;
    public float rangeMultiplier = 1;
    public float dartVelocity;
    public float shootDelay;
    public float shootDelayMultiplier = 1;
    public float spread;
    public int bulletsPerShot;

    [Header("References")]
    public LayerMask mask;
    public Transform BloonHolder;
    public GameObject bulletPrefab;
    public Transform shootPoint;
    protected Transform targetBloon;
    protected bool canShoot = true;

    protected void Start()
    {
        spread /= 1000;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        GetClosestBloon();
        if (targetBloon != null && Vector3.Distance(gameObject.transform.position, targetBloon.position) <= range)
        {
            Shoot();
        }
    }

    protected bool CanSeeBloon(Transform bloon)
    {
        Transform bloonModel = bloon.Find("Model");
        Quaternion rotation = Quaternion.LookRotation((new Vector3(bloonModel.position.x, transform.position.y, bloonModel.position.z) - transform.position).normalized);
        Vector3 rotatedShootPos = rotation * shootPoint.localPosition;
        Vector3 newShootPos = transform.position + rotatedShootPos;

        Ray ray = new Ray(newShootPos, bloonModel.position - newShootPos);
        RaycastHit hit; 
        //Debug.DrawRay(ray.origin, ray.direction * Vector3.Distance(newShootPos, bloonModel.position));
        if(Physics.Raycast(ray, out hit, range, mask))
        {
            if(hit.collider.gameObject.layer == bloon.gameObject.layer)
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
                    if(CanSeeBloon(bloon))
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
                    if(CanSeeBloon(bloon))
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

        for (int i = 0; i < bulletsPerShot; i++)
        {    
            Transform target = targetBloon.Find("Model").GetChild(0);
            transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
            DartBehavior bulletScript = bullet.GetComponent<DartBehavior>();
            bulletScript.damage = damage;
            bulletScript.pierce = pierce;
            bulletScript.range = range;

            // Calculate bullet direction with spread
            Vector3 bulletDirection = (target.position - shootPoint.position).normalized;

            // Calculate spread offset
            float spreadX = Random.Range(-spread/2, spread/2);
            float spreadY = Random.Range(-spread/2, spread/2);

            bulletDirection = (Quaternion.AngleAxis(spreadX, Vector3.up) * Quaternion.AngleAxis(spreadY, Vector3.right) * bulletDirection).normalized;

            bulletScript.direction = bulletDirection.normalized;
            bulletScript.bulletSpeed = dartVelocity;
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
}
