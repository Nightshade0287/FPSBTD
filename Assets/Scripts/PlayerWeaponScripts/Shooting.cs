using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [Header("Variables")]
    public float bulletSpeed;
    public float ShootDelay;
    public float spreadAngle = 1f;
    public float reloadTime = 1f;
    public int magazineSize;
    public int bulletsPerShot = 1;
    public bool FullAuto;

    [Header("References")]
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;

    private bool canShoot = true;
    private bool isReloading = false;
    private int bulletsInMagazine;

    private void Start()
    {
        bulletsInMagazine = magazineSize;
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

        if (!isReloading)
        {
            StartCoroutine(Reload());
        }
    }

    private void Shoot()
    {
        if (!canShoot || isReloading || bulletsInMagazine <= 0)
        {
            return;
        }

        for (int i = 0; i < bulletsPerShot; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();

            // Calculate bullet direction with spread
            Vector3 bulletDirection = bulletSpawnPoint.forward;

            // Calculate spread offset
            float spreadX = Random.Range(-spreadAngle, spreadAngle);
            float spreadY = Random.Range(-spreadAngle, spreadAngle);

            Vector3 spreadOffset = bulletSpawnPoint.right * spreadX + bulletSpawnPoint.up * spreadY;
            bulletDirection += spreadOffset;

            bulletRigidbody.velocity = bulletDirection.normalized * bulletSpeed;
        }
        bulletsInMagazine--;

        // Set a cooldown before the next shot
        canShoot = false;
        StartCoroutine(ResetShootCooldown());
    }

    private IEnumerator ResetShootCooldown()
    {
        yield return new WaitForSeconds(ShootDelay);
        canShoot = true;
    }

    private IEnumerator Reload()
    {
        if (bulletsInMagazine == magazineSize)
        {
            yield break;
        }

        isReloading = true;

        yield return new WaitForSeconds(reloadTime);
        bulletsInMagazine = magazineSize;
        isReloading = false;
    }

}


