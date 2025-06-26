using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ultra_Juggernaut : PhysicsBasedDarts
{
    [Header("Shrapnel Variables")]
    public GameObject fragPrefab;
    public Shrapnel shrapnel;
    public override void BloonHitAction(GameObject bloon, List<GameObject> bloonOrder)
    {
        Health bloonHealth = bloon.GetComponent<Health>();
        bloonHealth.dart = gameObject;
        bloonHealth.TakeDamage(damage);
        bloonHitList.Add(bloon.GetInstanceID());
        bloonsHit += 1;
        if (bloonsHit == pierce / 2) DeployMiniSpikeBalls(6);
        if (bloonsHit >= pierce)
        {
            DeployMiniSpikeBalls(6);
            Destroy(gameObject);
        }    
    }
    public void DeployMiniSpikeBalls(int count)
    {
        GameObject frag = Instantiate(fragPrefab, transform.position, Quaternion.identity);
        Frag fragScript = frag.GetComponent<Frag>();
        direction = GetComponent<Rigidbody>().velocity;
        fragScript.Initialize(shrapnel.count, shrapnel.damage, damageTypes, damageBuffs, shrapnel.pierce,
        shrapnel.range, shrapnel.velocity, shrapnel.spreadAngle, shrapnel.sphereRadius, direction, shrapnel.prefab);
        fragScript.parentDart = gameObject;
    }
}
