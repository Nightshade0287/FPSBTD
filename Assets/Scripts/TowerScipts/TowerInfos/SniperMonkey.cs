using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperMonkey : TowerInfo
{
    [Header("Required Upgrade Info")]
    public GameObject bouncingBulletPrefab;
    public float bounceRange;
    public int bounceAmount;
    private BloonWaves bloonWaves;
    private bool panicShoot;
    private int lastHealth;
    public Shrapnel shrapnel;
    public void Update()
    {
        if (path3Index == 5)
        {
            if (!bloonWaves.roundOver)
            {
                twr.shootDelayMultiplier = Mathf.Clamp(bloonWaves.roundPercentage / 100, 0, 1);
                if (panicShoot && !bloonWaves.roundOver) twr.shootDelayMultiplier *= 0.25f;
                else if (economy.health < lastHealth)
                {
                    StopCoroutine(PanicShoot(7));
                    StartCoroutine(PanicShoot(7));
                }
                lastHealth = economy.health;
            }
            else
            {
                twr.shootDelayMultiplier = 1;
                StopCoroutine(PanicShoot(7));
                panicShoot = false;
            }
        }
    }
    IEnumerator PanicShoot(float time)
    {
        panicShoot = true;
        yield return new WaitForSeconds(time);
        panicShoot = false; 
    }
    public override void DefineUpgrades()
    {
        // Path 1 Upgrades
        path1[0] = new Upgrade("Full Metal Jacket", 420, (twr) =>
        {
            twr.damage += 2;
            twr.ReplaceDamageType(DamageTypes.Sharp, DamageTypes.Normal);
        });
        path1[1] = new Upgrade("Large Calibre", 1560, (twr) =>
        {
            twr.damage += 3;
        });
        path1[2] = new Upgrade("Deadly Precision", 3600, (twr) =>
        {
            twr.damage = 20;
            twr.AddOrUpdateBuff(BloonTypes.Ceramic, 15);
        });
        path1[3] = new Upgrade("Maim MOAB", 6780, (twr) =>
        {
            twr.damage = 30;
        });
        path1[4] = new Upgrade("Cripple MOAB", 38400, (twr) =>
        {
            twr.damage = 280;
        });
        // Path 2 Upgrades
        path2[0] = new Upgrade("Night Vision Goggles", 300, (twr) =>
        {
            twr.damageTypes.Add(DamageTypes.CamoDetection);
            twr.AddOrUpdateBuff(BloonTypes.Camo, 2);
        });
        path2[1] = new Upgrade("Shrapnel Shot", 540, (twr) =>
        {
            twr.dartPrefab = bouncingBulletPrefab;
        });
        path2[2] = new Upgrade("Bouncing Bullets", 2880, (twr) =>
        {
            twr.pierce = 3;
            bounceAmount = 3;
            bounceRange = 10;
        });
        path2[3] = new Upgrade("Supply Drop", 9120, (twr) =>
        {
            twr.pierce = 5;
            shrapnel.pierce = 5;
            twr.ReplaceDamageType(DamageTypes.Sharp, DamageTypes.Normal);
        });
        path2[4] = new Upgrade("Elite Sniper", 17400, (twr) =>
        {
            twr.shootDelayMultiplier *= 0.4f;
        });
        // Path 3 Upgrades
        path3[0] = new Upgrade("Fast Firing", 540, (twr) =>
        {
            twr.shootDelayMultiplier *= 0.7f;
        });
        path3[1] = new Upgrade("Even Faster Firing", 540, (twr) =>
        {
            twr.shootDelayMultiplier *= 0.7f;
        });
        path3[2] = new Upgrade("Semi-Automatic", 3480, (twr) =>
        {
            twr.shootDelayMultiplier = 1;
            twr.shootDelay = 0.2597f;
        });
        path3[3] = new Upgrade("Full Auto Rifle", 4920, (twr) =>
        {
            twr.shootDelay = 0.1299f;
            twr.AddOrUpdateBuff(BloonTypes.Moab, 2);
        });
        path3[4] = new Upgrade("Elite Defender", 17640, (twr) =>
        {
            twr.shootDelay = 0.0649f;
            twr.AddOrUpdateBuff(BloonTypes.Moab, 2);
            bloonWaves = GameObject.Find("BloonManager").GetComponent<BloonWaves>();
            lastHealth = economy.health;
        });
    }
    public override void ApplyCrosspathEffects()
    {
        //Path 
        if (path1Index == 1 && path2Index == 3) bounceRange = 20;
        if (path1Index == 1 && path2Index == 4)
        {
            bounceAmount = 4;
            shrapnel.pierce = 5;
        }
        //Path2
        if (path2Index == 2 && path1Index == 1){
            shrapnel.ReplaceDamageType(DamageTypes.Sharp, DamageTypes.Normal);
            shrapnel.damage = 2;
        }
        if (path2Index == 2 && path1Index == 2) shrapnel.damage = 3;
        if (path2Index == 2 && path1Index == 3) shrapnel.damage = 4;
        if (path2Index == 2 && path1Index == 4) shrapnel.damage = 6;
        if (path2Index == 2 && path1Index == 5){
            shrapnel.damage = 12;
            shrapnel.pierce = 3;
        }
        if (path2Index == 2 && path3Index == 4) shrapnel.AddOrUpdateBuff(BloonTypes.Moab, 1);
    }
}
