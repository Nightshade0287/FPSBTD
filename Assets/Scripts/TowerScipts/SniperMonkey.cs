using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperMonkey : BaseTower
{
    public int damage;
    public LayerMask bloonLayer;
    private Vector3 bloonDir;


    protected override void Update()
    {
        GetClosestBloon();
        Shoot();
        bloonDir = new Vector3(TargetBloon.position.x, shootPoint.position.y, TargetBloon.position.z);
        transform.LookAt(bloonDir);
    }

    protected override void Shoot()
    {
        if (!canShoot)
        {
            return;
        }
        bloonDir = new Vector3(TargetBloon.position.x, shootPoint.position.y, TargetBloon.position.z);

        if (Physics.Raycast(shootPoint.position, bloonDir, bloonLayer))
        {
            TargetBloon.GetComponent<Health>().TakeDamage(damage);
        }

        // Set a cooldown before the next shot
        canShoot = false;
        StartCoroutine(ResetShootCooldown());
    }
}
