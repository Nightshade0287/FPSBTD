using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frag : MonoBehaviour
{
    [Header("Variables")]
    public int fragmentCount;
    public int damage;
    public int pierce;
    public float range;
    public float dartVelocity;
    [Range(0f, 360f)]
    public float spreadAngle;
    public float sphereRadius;
    [HideInInspector]
    public Vector3 direction;
    public GameObject fragmentPrefab;
    public GameObject parentDart = null;

    // This method sets all the necessary variables for the dart
    public void Initialize(int fragmentCount, int damage, int pierce, float range, float dartVelocity, float spreadAngle, float sphereRadius, Vector3 direction, GameObject fragmentPrefab)
    {
        this.fragmentCount = fragmentCount;
        this.damage = damage;
        this.pierce = pierce;
        this.range = range;
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
        Destroy(gameObject);
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
            DartBehavior bulletScript = fragment.GetComponent<DartBehavior>();
            if (parentDart != null) bulletScript.bloonHitList = new List<int>(parentDart.GetComponent<DartBehavior>().bloonHitList);
            bulletScript.damage = damage;
            bulletScript.pierce = pierce;
            bulletScript.range = range;
            bulletScript.direction = globalDirection.normalized;
            bulletScript.bulletSpeed = dartVelocity;
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
}

