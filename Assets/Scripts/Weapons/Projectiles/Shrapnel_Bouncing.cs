using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Shrapnel_Bouncing : DartBehavior
{
    [Header("Bounce and Shrapnel Variables")]
    public GameObject fragPrefab;
    public Shrapnel shrapnel;
    public int bounceRange = 40;
    public int bounceAmount;
    public Transform targetBloon;
    public override void BloonHitAction(GameObject bloon, List<GameObject> bloonOrder)
    {
        if(bloon.GetInstanceID() == bloonOrder[0].GetInstanceID())
        {
            Health bloonHealth = bloon.GetComponent<Health>();
            GetClosestBloon(bloon);
            bloonHealth.dart = gameObject;
            bloonHealth.TakeDamage(damage);
            bloonHitList.Add(bloon.GetInstanceID());
            bloonsHit += 1;
            Shrapnel(bloon.transform);
            if(bloonsHit >= pierce) Destroy(gameObject);
            if(targetBloon != null && bloonsHit >= bounceAmount) BounceBullet(bloon.transform);
            targetBloon = null;
        }
    }

    protected bool CanSeeBloon(Transform bloon, Transform hitBloon)
    {
        Transform bloonModel = bloon.Find("Model");;
        Ray ray = new Ray(hitBloon.position, bloonModel.position - hitBloon.position);
        RaycastHit hit; 
        int layerMask = 1 << LayerMask.NameToLayer("Ground");;
        //Debug.DrawRay(ray.origin, ray.direction * Vector3.Distance(hitBloon.position, bloonModel.position));
        if(Physics.Raycast(ray, out hit, bounceRange, hitLayers))
        {
            if(hit.collider.transform.position == bloon.position)
                return true;
        }
        return false;
    }
    protected void GetClosestBloon(GameObject hitBloon)
    {
        Transform closestBloon = null;
        float closestDistance = Mathf.Infinity;
        Transform BloonHolder = GameObject.Find("Bloons").transform;
        if (BloonHolder.childCount != 0)
        {
            foreach (Transform bloon in BloonHolder)
            {
                float distance = Vector3.Distance(hitBloon.transform.position, bloon.position);
                if (distance < closestDistance && !bloonHitList.Contains(bloon.gameObject.GetInstanceID()) && bloon.gameObject.GetInstanceID() != hitBloon.GetInstanceID())
                {
                    if(CanSeeBloon(bloon, hitBloon.transform))
                    {
                        closestBloon = bloon;
                        closestDistance = distance;
                    }
                }
            }

            if (closestBloon != null)
            {
                targetBloon = closestBloon;
            }
        }
    }

    public void BounceBullet(Transform bloon)
    {
        transform.position = bloon.position;
        startPoint = bloon.position;
        timeSinceShot = 0f;
        direction = (targetBloon.position - bloon.position).normalized;
    }

    public void Shrapnel(Transform bloon)
    {
        GameObject frag = Instantiate(fragPrefab, bloon.position, Quaternion.identity);
        Frag fragScript = frag.GetComponent<Frag>();
        fragScript.Initialize(shrapnel.count, shrapnel.damage, damageTypes, damageBuffs, shrapnel.pierce,
        shrapnel.range, shrapnel.velocity, shrapnel.spreadAngle, shrapnel.sphereRadius, direction, shrapnel.prefab);
        fragScript.parentDart = gameObject;
    }
}
