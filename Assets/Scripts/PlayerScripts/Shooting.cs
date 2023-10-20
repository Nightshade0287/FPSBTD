using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [Header("Variables")]
    public float bulletSpeed;
    public float ShootDelay;
    public float spreadAngle = 1f;
    public int bulletsPerShot = 1;
    public float range = 20f;
    public bool FullAuto;

    [Header("References")]
    public Transform shootPoint;
    public Transform cam;
    public GameObject bulletPrefab;

    private bool canShoot = true;
    private Vector3 shootVector;

    private void Start()
    {
        spreadAngle /= 1000f;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && FullAuto)
        {
            Shoot();
        }

        else if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (!canShoot)
        {
            return;
        }

        for (int i = 0; i < bulletsPerShot; i++)
        {
            CalculateShootVector();
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
            bullet.GetComponent<DartBehavior>().range = range;
            Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();

            // Calculate bullet direction with spread
            Vector3 bulletDirection = shootPoint.forward;

            // Calculate spread offset
            float spreadX = Random.Range(-spreadAngle, spreadAngle);
            float spreadY = Random.Range(-spreadAngle, spreadAngle);

            Vector3 spreadOffset = shootPoint.right * spreadX + shootPoint.up * spreadY;
            bulletDirection += spreadOffset;

            bulletRigidbody.velocity = bulletDirection * bulletSpeed;
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

    private void CalculateShootVector()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, 50))
        {
            shootVector = hit.point - shootPoint.position;
        }
        else 
        {
            shootVector = cam.forward;
        }
        Debug.DrawRay(shootPoint.position, shootVector * 100, Color.red);
    }
}


