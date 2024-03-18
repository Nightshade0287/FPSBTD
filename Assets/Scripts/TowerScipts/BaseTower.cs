using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTower : MonoBehaviour
{
    [Header("Variables")]
    public int damage;
    public int sharpness;
    public float range;
    public float DartVelocity;
    public float ShootDelay;
    public float spread;
    public int bulletsPerShot;

    [Header("References")]
    public Transform BloonHolder;
    public GameObject bulletPrefab;
    public Transform shootPoint;

    protected Transform TargetBloon;
    protected bool canShoot = true;

    protected void Start()
    {
        spread /= 1000;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        GetClosestBloon();
        if (Vector3.Distance(gameObject.transform.position, TargetBloon.position) <= range)
        {
            Shoot();
        }
    }

    // Find the closest bloon
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
                    closestBloon = bloon;
                    closestDistance = distance;
                }
            }

            if (closestBloon != null)
            {
                TargetBloon = closestBloon;
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
            transform.LookAt(new Vector3(TargetBloon.position.x, transform.position.y, TargetBloon.position.z));
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
            DartBehavior bulletScript = bullet.GetComponent<DartBehavior>();
            bulletScript.damage = damage;
            bulletScript.sharpness = sharpness;
            bulletScript.range = range;

            // Calculate bullet direction with spread
            Vector3 bulletDirection = (TargetBloon.position - shootPoint.position).normalized;

            // Calculate spread offset
            float spreadX = Random.Range(-spread, spread);
            float spreadY = Random.Range(-spread, spread);

            Vector3 spreadOffset = shootPoint.right * spreadX + shootPoint.up * spreadY;
            bulletDirection += spreadOffset;

            bulletScript.velocity = bulletDirection * DartVelocity;
        }

        // Set a cooldown before the next shot
        canShoot = false;
        StartCoroutine(ResetShootCooldown());
    }

    // Reset the shooting cooldown
    protected IEnumerator ResetShootCooldown()
    {
        yield return new WaitForSeconds(ShootDelay);
        canShoot = true;
    }
}
