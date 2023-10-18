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
    public GameObject projectilePrefab;
    public Transform shootPoint;

    private Vector3 closestBloon;
    private bool canShoot = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GetClosestBloon();
        if (Vector3.Distance(gameObject.transform.position, closestBloon) <= range)
        {
            Shoot();
            Debug.Log("Bloon in range");
        }
        //Debug.Log(closestBloon);
    }

    public void GetClosestBloon()
    {
        foreach(Transform bloon in BloonHolder)
        {
            //Debug.Log(bloon.position);
            float distance = Vector3.Distance(transform.position, bloon.position);
            if (distance <= Vector3.Distance(transform.position, closestBloon))
                closestBloon = bloon.position;
            else if (distance == 0)
                closestBloon = bloon.position;
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
            GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
            Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();

            // Calculate bullet direction with spread
            Vector3 shootDirection = (closestBloon - shootPoint.position).normalized;
            transform.forward = new Vector3(shootDirection.x, transform.position.y, shootDirection.z);

            // Calculate spread offset
            float spreadX = Random.Range(-spread, spread);
            float spreadY = Random.Range(-spread, spread);

            Vector3 spreadOffset = shootPoint.right * spreadX + shootPoint.up * spreadY;
            shootDirection += spreadOffset;

            projectileRigidbody.velocity = shootDirection * shootSpeed;
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
