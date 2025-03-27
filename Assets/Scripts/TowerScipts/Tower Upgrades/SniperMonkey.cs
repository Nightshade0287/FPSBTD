using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperMonkey : TowerInfo
{
    [Header("Required Upgrade Info")]
    public GameObject bouncingBulletPrefab;
    public override void DefineUpgrades()
    {
        // Path 1 Upgrades
        path1[0] = new Upgrade("Full Metal Jacket", 420, (tower) =>
        {
            tower.damage += 2;
            tower.damageTypes.Add(DamageTypes.Normal);
            tower.damageTypes.Remove(DamageTypes.Sharp);
        });
        path1[1] = new Upgrade("Large Calibre", 1560, (tower) =>
        {
            tower.damage += 3;
        });
        path1[2] = new Upgrade("Deadly Precision", 3600, (tower) =>
        {
            tower.damage += 13;
        });
        path1[3] = new Upgrade("Maim MOAB", 6780, (tower) =>
        {
            tower.damage += 10;
        });
        path1[4] = new Upgrade("Cripple MOAB", 38400, (tower) =>
        {
            tower.damage += 250;
        });
        // Path 2 Upgrades
        path2[0] = new Upgrade("Night Vision Goggles", 300, (tower) =>
        {
            tower.damageTypes.Add(DamageTypes.CamoDetection);
        });
        path2[1] = new Upgrade("Shrapnel Shot", 540, (tower) =>
        {
            tower.bulletPrefab = bouncingBulletPrefab;
        });
        path2[2] = new Upgrade("Bouncing Bullets", 2880, (tower) =>
        {
            tower.pierce = 3;
        });
        path2[3] = new Upgrade("Supply Drop", 9120, (tower) =>
        {
            tower.pierce = 4;
            bouncingBulletPrefab.GetComponent<Shrapnel_Bouncing>().shrapnelPierce = 5;
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
            tower.shootDelayMultiplier = 0.167f;
        });
        path3[3] = new Upgrade("Full Auto Rifle", 4920, (tower) =>
        {
            tower.shootDelayMultiplier = 0.0808f;
        });
        path3[4] = new Upgrade("Elite Defender", 17640, (tower) =>
        {
            tower.shootDelayMultiplier = 0.0404f;
        });
    }
}
