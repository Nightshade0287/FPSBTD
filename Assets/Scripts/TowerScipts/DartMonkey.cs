using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartMonkey : MonoBehaviour
{
    [Header("Variables")]
    public float range;
    public float DartVelocity;
    public float ShootDelay;
    public float spread;
    public int bulletsPerShot;
    public float leadTime; // Additional time to lead the target

    [Header("References")]
    public Transform BloonHolder;
    public GameObject bulletPrefab;
    public Transform shootPoint;

    private Transform targetBloon;
    private bool canShoot = true;

    // Start is called before the first frame update
    void Start()
    {
        // Initialization code (if any)
    }

    // Update is called once per frame
    void Update()
    {
        FindTargetBloon();
        if (targetBloon != null)
        {
            Shoot();
        }
    }

    // Find the target bloon
    public void FindTargetBloon()
    {
        targetBloon = null;
        float closestDistance = Mathf.Infinity;

        foreach (Transform bloon in BloonHolder)
        {
            float distance = Vector3.Distance(transform.position, bloon.position);

            if (distance < closestDistance)
            {
                targetBloon = bloon;
                closestDistance = distance;
            }
        }
    }

    // Shoot darts
    private void Shoot()
    {
        if (!canShoot)
        {
            return;
        }

        for (int i = 0; i < bulletsPerShot; i++)
        {
            if (targetBloon == null)
            {
                continue;
            }

            // Calculate where the bloon will be with a slight delay
            Vector3 predictedPosition = CalculateInterceptPosition(targetBloon, DartVelocity, -leadTime);

            // Rotate to face the predicted position
            transform.LookAt(new Vector3(predictedPosition.x, transform.position.y, predictedPosition.z));

            // Instantiate a dart
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
            bullet.GetComponent<DartBehavior>().range = range;

            // Calculate bullet direction with spread
            Vector3 shootDirection = (predictedPosition - shootPoint.position).normalized;

            // Calculate spread offset
            float spreadX = Random.Range(-spread, spread);
            float spreadY = Random.Range(-spread, spread);

            Vector3 spreadOffset = shootPoint.right * spreadX + shootPoint.up * spreadY;
            shootDirection += spreadOffset;

            // Set dart's velocity
            Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
            bulletRigidbody.velocity = shootDirection * DartVelocity;
        }

        // Set a cooldown before the next shot
        canShoot = false;
        StartCoroutine(ResetShootCooldown());
    }

    // Reset the shooting cooldown
    private IEnumerator ResetShootCooldown()
    {
        yield return new WaitForSeconds(ShootDelay);
        canShoot = true;
    }

    // Calculate where to shoot to intercept the target with a time delay
    private Vector3 CalculateInterceptPosition(Transform target, float bulletSpeed, float timeDelay)
    {
        Vector3 targetPosition = target.position;
        Vector3 targetVelocity = target.GetComponent<Rigidbody>().velocity;

        float timeToIntercept = (Vector3.Distance(shootPoint.position, targetPosition) - timeDelay) / bulletSpeed;

        // Predict where the target will be in the future
        Vector3 predictedPosition = targetPosition + targetVelocity * timeToIntercept;

        return predictedPosition;
    }
}

