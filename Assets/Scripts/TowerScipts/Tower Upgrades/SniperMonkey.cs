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
        path1[0] = new Upgrade("Full Metal Jacket", 420, (baseTower) =>
        {
            baseTower.damage += 2;
        });
        path1[1] = new Upgrade("Large Calibre", 1560, (baseTower) =>
        {
            baseTower.damage += 3;
        });
        path1[2] = new Upgrade("Deadly Precision", 3600, (baseTower) =>
        {
            baseTower.damage += 13;
        });
        path1[3] = new Upgrade("Maim MOAB", 6780, (baseTower) =>
        {
            baseTower.damage += 10;
        });
        path1[4] = new Upgrade("Cripple MOAB", 38400, (baseTower) =>
        {
            baseTower.damage += 250;
        });
        // Path 2 Upgrades
        path2[0] = new Upgrade("Night Vision Goggles", 300, (baseTower) =>
        {

        });
        path2[1] = new Upgrade("Shrapnel Shot", 540, (baseTower) =>
        {
            baseTower.bulletPrefab = bouncingBulletPrefab;
        });
        path2[2] = new Upgrade("Bouncing Bullets", 2880, (baseTower) =>
        {
            baseTower.pierce = 3;
        });
        path2[3] = new Upgrade("Supply Drop", 9120, (baseTower) =>
        {
            baseTower.pierce = 4;
            bouncingBulletPrefab.GetComponent<Shrapnel_Bouncing>().shrapnelPierce = 5;
        });
        path2[4] = new Upgrade("Elite Sniper", 17400, (baseTower) =>
        {
            baseTower.shootDelayMultiplier *= 0.4f;
        });
        // Path 3 Upgrades
        path3[0] = new Upgrade("Fast Firing", 540, (baseTower) =>
        {
            baseTower.shootDelayMultiplier *= 0.7f;
        });
        path3[1] = new Upgrade("Even Faster Firing", 540, (baseTower) =>
        {
            baseTower.shootDelayMultiplier *= 0.7f;
        });
        path3[2] = new Upgrade("Semi-Automatic", 3480, (baseTower) =>
        {
            baseTower.shootDelayMultiplier = 0.167f;
        });
        path3[3] = new Upgrade("Full Auto Rifle", 4920, (baseTower) =>
        {
            baseTower.shootDelayMultiplier = 0.0808f;
        });
        path3[4] = new Upgrade("Elite Defender", 17640, (baseTower) =>
        {
            baseTower.shootDelayMultiplier = 0.0404f;
        });
    }
}
