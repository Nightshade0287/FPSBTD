using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunController : MonoBehaviour
{
    [Header("Aiming")]
    public Vector3 hipPosition;
    public Vector3 aimPosition;
    public float adsSpeed = 10f;
    public float aimMultiplier;

    [Header("Specs")]
    public int damage;
    public int sharpness;
    public float range;
    public float bulletSpeed = 100f;
    public float spread;
    public int roundsPerMinute = 600;
    public int bulletsPerShot = 1;
    public bool fullAuto = false;
    public bool bulletDrop = false;
    [Header("Recoil and Sway")]
    [Range(0f, 1f)]
    public float sway = 0.5f;
    public float sensMultiplier = 1f;
    public float recoilForce = 1f;
    public bool randomizeRecoil;
    public Vector2 randomRecoilContraints;
    public Vector2[] RecoilPattern;

    [Header("References")]
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    private bool canShoot = true;
    public bool shooting = false;
    public bool aiming = false;
    private Quaternion lastRotation;
    private Transform playerCam;
    private WaitForSeconds fireWait;

    private void Start()
    {
        playerCam = GameObject.FindGameObjectWithTag("Player").transform.Find("PlayerCam").transform;
        fireWait = new WaitForSeconds(60f / roundsPerMinute);
    }
    private void Update()
    {
        DetermineAim();
        DetermineSway();
    }

    public void StartShoot(InputAction.CallbackContext ctx)
    {
        if(ctx.performed)
        {
            Shoot();
            shooting = true;
        }
        else if(ctx.canceled)
        {
            shooting = false;
        }
    }

    public void Aiming(InputAction.CallbackContext ctx)
    {
        if(ctx.performed)
        {
            aiming= true;
        }
        else if(ctx.canceled)
        {
            aiming = false;
        }
    }
    void Shoot()
    {
        if (!canShoot)
        {
            return;
        }

        for (int i = 0; i < bulletsPerShot; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            DartBehavior bulletScript = bullet.GetComponent<DartBehavior>();
            bulletScript.damage = damage;
            bulletScript.sharpness = sharpness;
            bulletScript.range = range;
            if(bulletDrop) bulletScript.gravity = 9.8f;

            Vector3 bulletDirection = ShootDirection();
            // Calculate spread offset
            float spreadX = Random.Range(-spread, spread);
            float spreadY = Random.Range(-spread, spread);

            Vector3 spreadOffset = bulletSpawnPoint.right * spreadX + bulletSpawnPoint.up * spreadY;
            bulletDirection += (spreadOffset / 100);

            bulletScript.direction = bulletDirection.normalized;
            bulletScript.bulletSpeed = bulletSpeed;
        }

        // Set a cooldown before the next shot
        canShoot = false;
        DetermineRecoil();
        StartCoroutine(ResetShootCooldown());
    }

    public IEnumerator ResetShootCooldown()
    {
        yield return new WaitForSeconds(60f / roundsPerMinute);
        canShoot = true;
        if(shooting && fullAuto)
            Shoot();
    }
    public void DetermineAim()
    {
        Vector3 target;
        PlayerLook look = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLook>();
        if(aiming) 
        {
            target = aimPosition;
            look.aimMultiplier = aimMultiplier;
        }
        else
        {
            target = hipPosition;
            look.aimMultiplier = 1;
        }
        Vector3 desiredPosition = Vector3.Lerp(transform.localPosition, target, Time.deltaTime * adsSpeed);
        transform.localPosition = desiredPosition;
    }

    public void DetermineSway()
    {
        // Calculate the rotation change since the last frame
        Quaternion currentRotation = playerCam.rotation;
        Quaternion deltaRotation = Quaternion.Inverse(lastRotation) * currentRotation;

        // Convert the rotation change to euler angles
        Vector3 eulerDeltaRotation = deltaRotation.eulerAngles;

        // Calculate the rotation speed vector
        Vector2 rotationSpeedVector = new Vector2(
            Mathf.DeltaAngle(lastRotation.eulerAngles.y, currentRotation.eulerAngles.y),
            Mathf.DeltaAngle(lastRotation.eulerAngles.x, currentRotation.eulerAngles.x)
        ) / Time.deltaTime;

        // Update lastRotation for the next frame
        lastRotation = currentRotation;

        // Update gun sway based on rotation and movement speed
        transform.localPosition += ((Vector3)rotationSpeedVector) * -sway / 210526;

    }

    void DetermineRecoil()
    {
        transform.localPosition -= Vector3.forward * recoilForce / 10;
        if(randomizeRecoil)
        {
            float xRocoil = Random.Range(-randomRecoilContraints.x, randomRecoilContraints.x);
            float yRocoil = Random.Range(-randomRecoilContraints.y, randomRecoilContraints.y);

            Vector2 recoil = new Vector2(xRocoil, yRocoil);
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLook>().TakeRecoil(recoil);
        }
        else
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLook>().TakeRecoil(RecoilPattern[0]);
        }
    }

    Vector3 ShootDirection()
    {
        Ray ray = new Ray(playerCam.position, playerCam.forward);
        RaycastHit hitInfo;
        if(aiming || !Physics.Raycast(ray.origin, ray.direction, out hitInfo))
        {
            return bulletSpawnPoint.forward;
        }
        else
        {
            return hitInfo.point - transform.position;
        }
    }
}