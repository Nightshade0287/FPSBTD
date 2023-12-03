using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperMonkey : BaseTower
{
    public int damage;

    protected override void Update()
    {
        GetClosestBloon();
        Shoot();
    }

    protected override void Shoot()
    {
        if (!canShoot)
        {
            return;
        }

        for (int i = 0; i < bulletsPerShot; i++)
        {    
            transform.LookAt(new Vector3(TargetBloon.position.x, transform.position.y, TargetBloon.position.z));

            // Calculate bullet direction with spread
            Vector3 bulletDirection = (TargetBloon.position - shootPoint.position).normalized;
            TargetBloon.GetComponent<Health>().TakeDamage(damage);
        }

        // Set a cooldown before the next shot
        canShoot = false;
        StartCoroutine(ResetShootCooldown());
    }
}
