using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartMonkey : MonoBehaviour
{
    [Header("Variables")]
    public float range;
    public float shootSpeed;
    public float ShootDelay;
    public float spread;
    public int bulletsPerShot;

    [Header("References")]
    public Transform BloonHolder;
    public GameObject bulletPrefab;
    public Transform shootPoint;

    private Vector3 TargetBloon;
    private bool canShoot = true;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        GetClosestBloon();
        if (Vector3.Distance(gameObject.transform.position, TargetBloon) <= range)
        {
            Shoot();
        }
    }

    public void GetClosestBloon()
    {
        Transform closestBloon = null;
        float closestDistance = Mathf.Infinity;

        // Iterate through the children of the parentObject.
        foreach (Transform bloon in BloonHolder)
        {
            float distance = Vector3.Distance(transform.position, bloon.position);

            if (distance < closestDistance)
            {
                closestBloon = bloon;
                closestDistance = distance;
            } 
        }
        // if (closestBloon == null)
        //         canShoot = false;
        // else
        //     canShoot = true;
        if (closestBloon != null)
            TargetBloon = closestBloon.position;
    }

    private void Shoot()
    {
        if (!canShoot)
        {
            return;
        }

        for (int i = 0; i < bulletsPerShot; i++)
        {
            transform.LookAt(new Vector3(TargetBloon.x, transform.position.y, TargetBloon.z));
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
            bullet.GetComponent<DartBehavior>().range = range;
            Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();

            // Calculate bullet direction with spread
            Vector3 shootDirection = (TargetBloon - shootPoint.position).normalized;

            // Calculate spread offset
            float spreadX = Random.Range(-spread, spread);
            float spreadY = Random.Range(-spread, spread);

            Vector3 spreadOffset = shootPoint.right * spreadX + shootPoint.up * spreadY;
            shootDirection += spreadOffset;

            bulletRigidbody.velocity = shootDirection * shootSpeed;
        }

        // Set a cooldown before the next shot
        canShoot = false;
        StartCoroutine(ResetShootCooldown());
    }

    private IEnumerator ResetShootCooldown()
    {
        yield return new WaitForSeconds(ShootDelay);
        canShoot = true;
    }
}
