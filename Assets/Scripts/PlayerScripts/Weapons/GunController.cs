using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunController : MonoBehaviour
{
    [Header("Aiming")]
    public bool canADS = false;
    public Vector3 hipPosition;
    public Vector3 aimPosition;
    public float adsSpeed = 10f;
    public float aimMultiplier;

    [Header("Specs")]

    public List<DamageTypes> damageTypes = new List<DamageTypes>();
    public List<DamageBuff> damageBuffs = new List<DamageBuff>();
    public int damage;
    public int pierce;
    public float lifeSpan;
    public float dartSpeed = 100f;
    public float spread;
    public float ShootDelay = 1;
    public int dartsPerShot = 1;
    public bool fullAuto = false;
    public bool dartDrop = false;
    [Header("Recoil and Sway")]
    [Range(0f, 1f)]
    public float sway = 0.5f;
    public float recoilForce = 1f;
    public bool randomizeRecoil;
    public Vector2 randomRecoilContraints;
    public Vector2[] RecoilPattern;
    [Header("References")]
    public Transform dartSpawnPoint;
    public GameObject dartPrefab;
    private bool canShoot = true;
    public bool shooting = false;
    public bool aiming = false;
    private Quaternion lastRotation;
    private Transform playerCam;
    private WaitForSeconds fireWait;

    private void Start()
    {
        playerCam = GameObject.FindGameObjectWithTag("Player").transform.Find("PlayerCam").transform;
    }
    private void Update()
    {
        DetermineAim();
        DetermineSway();
        if (canShoot) transform.Find("Model").gameObject.SetActive(true);
        else transform.Find("Model").gameObject.SetActive(false);
    }

    public void StartShoot(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Shoot();
            shooting = true;
        }
        else if (ctx.canceled)
        {
            shooting = false;
        }
    }

    public void Aiming(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            aiming = true;
        }
        else if (ctx.canceled)
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

        for (int i = 0; i < dartsPerShot; i++)
        {
            GameObject dart = Instantiate(dartPrefab, dartSpawnPoint.position, dartSpawnPoint.rotation);
            DartBehavior dartScript = dart.GetComponent<DartBehavior>();
            InitializeDart(dartScript);

            Vector3 dartDirection = ShootDirection();
            // Calculate spread offset
            float spreadX = Random.Range(-spread, spread);
            float spreadY = Random.Range(-spread, spread);

            Vector3 spreadOffset = dartSpawnPoint.right * spreadX + dartSpawnPoint.up * spreadY;
            dartDirection += spreadOffset / 100;

            dartScript.direction = dartDirection.normalized;
            dartScript.dartSpeed = dartSpeed;
        }

        // Set a cooldown before the next shot
        canShoot = false;
        DetermineRecoil();
        StartCoroutine(ResetShootCooldown());
    }

    public IEnumerator ResetShootCooldown()
    {
        yield return new WaitForSeconds(ShootDelay);
        canShoot = true;
        if (shooting && fullAuto)
            Shoot();
    }
    public void DetermineAim()
    {
        Vector3 target;
        PlayerLook look = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLook>();
        if (aiming && canADS)
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
        if (randomizeRecoil)
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
        if (aiming || !Physics.Raycast(ray.origin, ray.direction, out hitInfo))
        {
            return dartSpawnPoint.forward;
        }
        else
        {
            return (hitInfo.point - transform.position).normalized;
        }
    }

    public void InitializeDart(DartBehavior dart)
    {
        dart.damageTypes = damageTypes;
        dart.damageBuffs = damageBuffs;
        dart.damage = damage;
        dart.pierce = pierce;
        dart.lifeSpan = lifeSpan;
        if (dartDrop) dart.gravity = 9.8f;
    }
}