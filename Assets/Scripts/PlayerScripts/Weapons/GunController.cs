using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [Header("Aiming")]
    public Vector3 hipPosition;
    public Vector3 aimPosition;
    public float adsSpeed = 10f;

    [Header("Specs")]
    public int damage;
    public int sharpness;
    public float range;
    public float bulletSpeed = 100f;
    public float spread;
    public int roundsPerMinute = 600;
    public float reloadTime = 1f;

    public bool UnlimitedMag = false;
    public int magazineSize = 10;
    public int bulletsPerShot = 1;
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
    public AudioClip shootSound;
    public AudioClip reloadSound;

    public List<FireMode> availableFireModes = new List<FireMode> { FireMode.FullAuto, FireMode.SemiAuto, FireMode.Safe };
    private bool canShoot = true;
    public bool shooting = false;
    public bool isReloading = false;

    public bool aiming = false;
    private Quaternion lastRotation;
    private Transform playerCam;
    [SerializeField]
    private int bulletsInMagazine;
    private int currentFireModeIndex = 0;
    private AudioSource audioSource;

    private void Start()
    {
        bulletsInMagazine = magazineSize;
        audioSource = GetComponent<AudioSource>();
        playerCam = GameObject.FindGameObjectWithTag("Player").transform.Find("PlayerCam").transform;
    }
    private void Update()
    {
        DetermineAim();
        DetermineSway();
    }
    
    [SerializeField]
    public FireMode currentFireMode
    {
        get { return availableFireModes[currentFireModeIndex]; }
    }

    public void Shoot()
    {
        bool MagEmpty()
        {
            if(!UnlimitedMag && bulletsInMagazine <= 0)
                return true;
            return false;
        }
        if (!canShoot || isReloading || MagEmpty() || currentFireMode == FireMode.Safe)
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

            Vector3 bulletDirection = ShootDirection();
            // Calculate spread offset
            float spreadX = Random.Range(-spread, spread);
            float spreadY = Random.Range(-spread, spread);

            Vector3 spreadOffset = bulletSpawnPoint.right * spreadX + bulletSpawnPoint.up * spreadY;
            bulletDirection += (spreadOffset / 100);

            bulletScript.velocity = bulletDirection.normalized * bulletSpeed;
        }

        bulletsInMagazine--;

        // Play shoot sound
        audioSource.PlayOneShot(shootSound);

        // Set a cooldown before the next shot
        canShoot = false;
        DetermineRecoil();
        StartCoroutine(ResetShootCooldown());
    }

    public IEnumerator ResetShootCooldown()
    {
        yield return new WaitForSeconds(60f / roundsPerMinute);
        canShoot = true;
        if (shooting && currentFireMode == FireMode.FullAuto) 
            Shoot();
    }

    public IEnumerator Reload()
    {
        if (bulletsInMagazine == magazineSize)
        {
            yield break;
        }

        isReloading = true;

        // Play reload sound
        audioSource.PlayOneShot(reloadSound);

        yield return new WaitForSeconds(reloadTime);
        bulletsInMagazine = magazineSize;
        isReloading = false;
        if (shooting && currentFireMode == FireMode.FullAuto) 
            Shoot();
    }

    public void CycleFireMode()
    {
        currentFireModeIndex = (currentFireModeIndex + 1) % availableFireModes.Count;
    }

    public void DetermineAim()
    {
        Vector3 target = hipPosition;
        if(aiming) target = aimPosition;

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
            int currentStep = magazineSize + 1 - bulletsInMagazine;
            currentStep = Mathf.Clamp(currentStep, 0, RecoilPattern.Length - 1);
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLook>().TakeRecoil(RecoilPattern[currentStep]);

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

public enum FireMode
{
    FullAuto,
    SemiAuto,
    Safe
}
