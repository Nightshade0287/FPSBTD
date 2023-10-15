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
    public bool FullAuto;

    [Header("References")]
    public Transform bulletSpawnPoint;
    public Transform cam;
    public GameObject bulletPrefab;
    public LayerMask Player;

    private bool canShoot = true;
    private Vector3 shootVector;

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
        CalculateShootVector();
    }

    private void Shoot()
    {
        if (!canShoot)
        {
            return;
        }

        for (int i = 0; i < bulletsPerShot; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();

            // Calculate bullet direction with spread
            Vector3 bulletDirection = shootVector.normalized;

            // Calculate spread offset
            float spreadX = Random.Range(-spreadAngle, spreadAngle);
            float spreadY = Random.Range(-spreadAngle, spreadAngle);

            Vector3 spreadOffset = bulletSpawnPoint.right * spreadX + bulletSpawnPoint.up * spreadY;
            bulletDirection += spreadOffset;

            bulletRigidbody.velocity = bulletDirection.normalized * bulletSpeed;
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
            shootVector = hit.point - bulletSpawnPoint.position;
        }
        else 
        {
            shootVector = cam.forward;
        }
        Debug.DrawRay(bulletSpawnPoint.position, shootVector * 100, Color.red);
    }
}


