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
    public float spreadAngleX;
    [Range(0f, 360f)]
    public float spreadAngleY;
    public float sphereRadius;

    public Shrapnel(GameObject shrapnelPrefab, int shrapnelCount, List<DamageTypes> damageTypes, List<DamageBuff> damageBuffs, int shrapnelDamage, int shrapnelPierce, float shrapnelRange, float shrapnelVelocity, float spreadAngleX, float spreadAngleY, float sphereRadius)
    {
        prefab = shrapnelPrefab;
        count = shrapnelCount;
        damage = shrapnelDamage;
        this.damageTypes = damageTypes;
        this.damageBuffs = damageBuffs;
        pierce = shrapnelPierce;
        range = shrapnelRange;
        velocity = shrapnelVelocity;
        this.spreadAngleX = spreadAngleX;
        this.spreadAngleY = spreadAngleY;
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
    public float range;
    public float velocity;
    [Range(0f, 360f)]
    public float spreadAngleX;
    [Range(0f, 360f)]
    public float spreadAngleY;
    public float sphereRadius;
    [HideInInspector]
    public Vector3 direction;
    public GameObject fragmentPrefab;
    public GameObject parentDart = null;

    // This method sets all the necessary variables for the dart
    public void Initialize(Shrapnel shrap, Vector3 dir)
    {
        fragmentCount = shrap.count;
        damageTypes = shrap.damageTypes;
        damageBuffs = shrap.damageBuffs;
        damage = shrap.damage;
        pierce = shrap.pierce;
        range = shrap.range;
        velocity = shrap.velocity;
        spreadAngleX = shrap.spreadAngleX;
        spreadAngleY = shrap.spreadAngleY;
        sphereRadius = shrap.sphereRadius;
        direction = dir.normalized;
        fragmentPrefab = shrap.prefab;
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
            Vector3 localDirection = RandomDirectionWithinEllipticalCone(spreadAngleX, spreadAngleY);

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
        Destroy(gameObject);
    }

    Vector3 RandomDirectionWithinEllipticalCone(float horizontalAngle, float verticalAngle)
    {
        // Convert angles from degrees to radians
        float xRad = Mathf.Deg2Rad * horizontalAngle / 2f; // Half horizontal angle
        float yRad = Mathf.Deg2Rad * verticalAngle / 2f;   // Half vertical angle

        // Generate a point on a unit circle, then scale it to an ellipse
        float angle = Random.Range(0f, 2f * Mathf.PI);
        float r = Mathf.Sqrt(Random.value); // Uniform distribution within ellipse
        float x = r * Mathf.Cos(angle) * Mathf.Tan(xRad);
        float y = r * Mathf.Sin(angle) * Mathf.Tan(yRad);
        float z = 1f;

        // Create direction and normalize
        Vector3 direction = new Vector3(x, y, z).normalized;
        return direction;
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
        dart.lifeSpan = range/velocity;
        dart.direction = dir.normalized;
        dart.dartSpeed = velocity;
    }
}

