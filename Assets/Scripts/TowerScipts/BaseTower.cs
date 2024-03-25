using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTower : MonoBehaviour
{
    [Header("Variables")]
    public int damage;
    public int pierce;
    public float range;
    public float DartVelocity;
    public float shootDelay;
    public float spread;
    public int bulletsPerShot;

    [Header("References")]
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
        Quaternion rotation = Quaternion.LookRotation((new Vector3(bloon.position.x, transform.position.y, bloon.position.z) - transform.position).normalized);
        Vector3 rotatedShootPos = rotation * shootPoint.localPosition;
        Vector3 newShootPos = transform.position + rotatedShootPos;

        Ray ray = new Ray(newShootPos, (bloon.position - newShootPos));
        Debug.DrawRay(ray.origin, ray.direction * Vector3.Distance(newShootPos, bloon.position));
        RaycastHit hit; 
        if(!Physics.Raycast(ray, out hit, range, bloon.gameObject.layer))
        {
            return true;
        }
        return false;
    }
    protected void GetClosestBloon()
    {
        Transform closestBloon = null;
        float closestDistance = Mathf.Infinity;

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
            float spreadX = Random.Range(-spread, spread);
            float spreadY = Random.Range(-spread, spread);

            Vector3 spreadOffset = shootPoint.right * spreadX + shootPoint.up * spreadY;
            bulletDirection += spreadOffset;

            bulletScript.direction = bulletDirection.normalized;
            bulletScript.bulletSpeed = DartVelocity;
        }

        // Set a cooldown before the next shot
        canShoot = false;
        StartCoroutine(ResetShootCooldown());
    }

    // Reset the shooting cooldown
    protected IEnumerator ResetShootCooldown()
    {
        yield return new WaitForSeconds(shootDelay);
        canShoot = true;
    }
}
