using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperMonkey : TowerInfo
{
    [Header("Required Upgrade Info")]
    public GameObject bouncingBulletPrefab;
    public GameObject shrapnelPrefab;
    public Shrapnel_Bouncing bouncingBullet;
    public BloonWaves bloonWaves;
    public bool panicShoot;
    public int health;
    public int lastHealth;
    public void Update()
    {
        if (path3Index == 5)
        {
            tower.shootDelayMultiplier = !bloonWaves.roundOver ? bloonWaves.roundPercentage / 100 : 1;
            if (panicShoot) tower.shootDelayMultiplier *= 0.25f;
        }
    }

    public void PanicShoot()
    {
        StartCoroutine(Panic(7));
    }

    IEnumerator Panic(float time)
    {
        panicShoot = true;
        yield return new WaitForSeconds(7);
        panicShoot = false; 
    }
    public override void DefineUpgrades()
    {
        // Path 1 Upgrades
        path1[0] = new Upgrade("Full Metal Jacket", 420, (tower) =>
        {
            tower.damage += 2;
            tower.ReplaceDamageType(DamageTypes.Sharp, DamageTypes.Normal);
        });
        path1[1] = new Upgrade("Large Calibre", 1560, (tower) =>
        {
            tower.damage += 3;
        });
        path1[2] = new Upgrade("Deadly Precision", 3600, (tower) =>
        {
            tower.damage = 20;
            tower.AddOrUpdateBuff(BloonTypes.Ceramic, 15);
        });
        path1[3] = new Upgrade("Maim MOAB", 6780, (tower) =>
        {
            tower.damage = 30;
        });
        path1[4] = new Upgrade("Cripple MOAB", 38400, (tower) =>
        {
            tower.damage = 280;
        });
        // Path 2 Upgrades
        path2[0] = new Upgrade("Night Vision Goggles", 300, (tower) =>
        {
            tower.damageTypes.Add(DamageTypes.CamoDetection);
            tower.AddOrUpdateBuff(BloonTypes.Camo, 2);
        });
        path2[1] = new Upgrade("Shrapnel Shot", 540, (tower) =>
        {
            tower.dartPrefab = bouncingBulletPrefab;
            bouncingBullet = tower.GetComponent<Shrapnel_Bouncing>();
        });
        path2[2] = new Upgrade("Bouncing Bullets", 2880, (tower) =>
        {
            tower.pierce = 3;
        });
        path2[3] = new Upgrade("Supply Drop", 9120, (tower) =>
        {
            tower.pierce = 5;
            bouncingBullet.shrapnel.pierce = 5;
            tower.ReplaceDamageType(DamageTypes.Sharp, DamageTypes.Normal);
        });
        path2[4] = new Upgrade("Elite Sniper", 17400, (tower) =>
        {
            tower.shootDelayMultiplier *= 0.4f;
        });
        // Path 3 Upgrades
        path3[0] = new Upgrade("Fast Firing", 540, (tower) =>
        {
            tower.shootDelayMultiplier *= 0.7f;
        });
        path3[1] = new Upgrade("Even Faster Firing", 540, (tower) =>
        {
            tower.shootDelayMultiplier *= 0.7f;
        });
        path3[2] = new Upgrade("Semi-Automatic", 3480, (tower) =>
        {
            tower.shootDelayMultiplier = 1;
            tower.shootDelay = 0.2597f;
        });
        path3[3] = new Upgrade("Full Auto Rifle", 4920, (tower) =>
        {
            tower.shootDelay = 0.1299f;
            tower.AddOrUpdateBuff(BloonTypes.Moab, 2);
        });
        path3[4] = new Upgrade("Elite Defender", 17640, (tower) =>
        {
            tower.shootDelay = 0.0649f;
            tower.AddOrUpdateBuff(BloonTypes.Moab, 2);
            bloonWaves = GameObject.Find("BloonManager").GetComponent<BloonWaves>();
        });
    }
    public override void ApplyCrosspathEffects()
    {
        //Path 
        if (path1Index == 1 && path2Index == 3) bouncingBullet.bounceRange = 20;
        if (path1Index == 1 && path2Index == 4)
        {
            bouncingBullet.bounceAmount = 4;
            bouncingBullet.shrapnel.pierce = 5;
        }
        //Path2
        if (path2Index == 2 && path1Index == 1){
            bouncingBullet.shrapnel.ReplaceDamageType(DamageTypes.Sharp, DamageTypes.Normal);
            bouncingBullet.shrapnel.damage = 2;
        }
        if (path2Index == 2 && path1Index == 2) bouncingBullet.shrapnel.damage = 3;
        if (path2Index == 2 && path1Index == 3) bouncingBullet.shrapnel.damage = 4;
        if (path2Index == 2 && path1Index == 4) bouncingBullet.shrapnel.damage = 6;
        if (path2Index == 2 && path1Index == 5){
            bouncingBullet.shrapnel.damage = 12;
            bouncingBullet.shrapnel.pierce = 3;
        }
        if (path2Index == 2 && path3Index == 4) bouncingBullet.shrapnel.AddOrUpdateBuff(BloonTypes.Moab, 1);
    }
}
