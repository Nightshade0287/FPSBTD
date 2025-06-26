using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[System.Serializable]
public class Shrapnel
{
    public GameObject prefab;
    public int count;
    public List<DamageTypes> damageTypes;
    public List<DamageBuff> damageBuffs;
    public int damage;
    public int pierce;
    public float range;
    public float velocity;
    [Range(0f, 360f)]
    public float spreadAngle;
    public float sphereRadius;

    public Shrapnel(GameObject shrapnelPrefab, int shrapnelCount, List<DamageTypes> damageTypes, List<DamageBuff> damageBuffs, int shrapnelDamage, int shrapnelPierce, float shrapnelRange, float shrapnelVelocity, float spreadAngle, float sphereRadius)
    {
        prefab = shrapnelPrefab;
        count = shrapnelCount;
        damage = shrapnelDamage;
        this.damageTypes = damageTypes;
        this.damageBuffs = damageBuffs;
        pierce = shrapnelPierce;
        range = shrapnelRange;
        velocity = shrapnelVelocity;
        this.spreadAngle = spreadAngle;
        this.sphereRadius = sphereRadius;
    }

    public void ReplaceDamageType(DamageTypes remove, DamageTypes replace)
    {
        if (damageTypes.Contains(remove))
        {
            damageTypes.Remove(remove);
            damageTypes.Add(replace);
        }
        else if (!damageTypes.Contains(replace)) damageTypes.Add(replace);
    }
    public void AddOrUpdateBuff(BloonTypes type, int amount)
    {
        DamageBuff existingBuff = damageBuffs.Find(buff => buff.bloonType == type);

        if (existingBuff != null)
        {
            existingBuff.buffAmount += amount;
        }
        else
        {
            damageBuffs.Add(new DamageBuff { bloonType = type, buffAmount = amount });
        }
    }
}
public class Frag : MonoBehaviour
{
    [Header("Variables")]
    public int fragmentCount;
    public int damage;
    public List<DamageTypes> damageTypes = new List<DamageTypes>();
    public List<DamageBuff> damageBuffs = new List<DamageBuff>();
    public Critical critical;
    public int pierce;
    public float lifeSpan;
    public float dartVelocity;
    [Range(0f, 360f)]
    public float spreadAngle;
    public float sphereRadius;
    [HideInInspector]
    public Vector3 direction;
    public GameObject fragmentPrefab;
    public GameObject parentDart = null;

    // This method sets all the necessary variables for the dart
    public void Initialize(int fragmentCount, int damage, List<DamageTypes> damageTypes, List<DamageBuff> damageBuffs, int pierce, float lifeSpan, float dartVelocity, float spreadAngle, float sphereRadius, Vector3 direction, GameObject fragmentPrefab)
    {
        this.fragmentCount = fragmentCount;
        this.damageTypes = damageTypes;
        this.damageBuffs = damageBuffs;
        this.damage = damage;
        this.pierce = pierce;
        this.lifeSpan = lifeSpan;
        this.dartVelocity = dartVelocity;
        this.spreadAngle = spreadAngle;
        this.sphereRadius = sphereRadius;
        this.direction = direction;
        this.fragmentPrefab = fragmentPrefab;
    }
    void Start()
    {
        SpreadFragments();
        direction = transform.forward;
        //Destroy(gameObject);
    }

    void Update()
    {
        //SpreadFragments();
    }
    void SpreadFragments()
    {
        for (int i = 0; i < fragmentCount; i++)
        {
            // Generate a random direction within the specified angle
            Vector3 localDirection = RandomDirectionWithinAngle(spreadAngle);

            // Rotate the local direction to align with the desired explosion direction
            Vector3 globalDirection = RotateDirection(localDirection, direction);

            // Calculate the position of the fragment by multiplying by the radius
            Vector3 fragmentPosition = transform.position + globalDirection * sphereRadius;

            // Instantiate the fragment at the calculated position
            GameObject fragment = Instantiate(fragmentPrefab, fragmentPosition, Quaternion.identity);
            DartBehavior dartScript = fragment.GetComponent<DartBehavior>();
            PhysicsBasedDarts physicsBasedDarts = fragment.GetComponent<PhysicsBasedDarts>();
            if (parentDart != null) dartScript.bloonHitList = new List<int>(parentDart.GetComponent<DartBehavior>().bloonHitList);
            InitializeFragments(dartScript, globalDirection);
        }
    }

    Vector3 RandomDirectionWithinAngle(float angle)
    {
        // Convert angle from degrees to radians
        float angleInRadians = Mathf.Deg2Rad * angle / 2f;  // Half angle for cone

        // Generate random values for spherical coordinates within the specified angle range
        float z = Random.Range(Mathf.Cos(angleInRadians), 1f);  // Controls spread along the z-axis (forward direction)
        float theta = Random.Range(0f, 2f * Mathf.PI);          // Azimuthal angle (around the cone)

        // Convert spherical coordinates to Cartesian coordinates (a cone-shaped spread)
        float x = Mathf.Sqrt(1 - z * z) * Mathf.Cos(theta);
        float y = Mathf.Sqrt(1 - z * z) * Mathf.Sin(theta);

        return new Vector3(x, y, z);  // This is still in local space relative to the cone
    }

    Vector3 RotateDirection(Vector3 localDirection, Vector3 targetDirection)
    {
        // Rotate the local direction vector so that it aligns with the target direction
        return Quaternion.LookRotation(targetDirection.normalized) * localDirection;
    }

    public void InitializeFragments(DartBehavior dart, Vector3 dir)
    {
        dart.damage = damage;
        dart.damageTypes = damageTypes;
        dart.damageBuffs = damageBuffs;
        dart.pierce = pierce;
        dart.lifeSpan = lifeSpan;
        dart.direction = dir.normalized;
        dart.dartSpeed = dartVelocity;
    }
}

