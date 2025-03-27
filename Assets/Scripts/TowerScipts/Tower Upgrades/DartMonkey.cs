using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartMonkey : TowerInfo
{
    public GameObject spikeBallPrefab;
    public override void DefineUpgrades()
    {
        // Path 1 Upgrades
        path1[0] = new Upgrade("Sharp Shots", 170, (tower) =>
        {
            tower.pierce += 1;
            if(path3Index == 5) tower.pierce += 5;
        });
        path1[1] = new Upgrade("Razor Sharp Shots", 265, (tower) =>
        {
            tower.pierce += 2;
            if(path3Index == 5 && tower.pierce == 8) tower.pierce += 5;
        });
        path1[2] = new Upgrade("Spike-O-Pult", 360, (tower) =>
        {
            tower.pierce = 22;
            tower.damage = 2;
            tower.rangeMultiplier *= 1.15f;
            tower.shootDelay = 1.15f;
            tower.damageTypes = new List<DamageTypes> {DamageTypes.Shatter};
            tower.bulletPrefab = spikeBallPrefab;
        });
        path1[3] = new Upgrade("Juggernaut", 2160, (tower) =>
        {
            tower.pierce = 55;
            tower.damage = 2;
            tower.pierce += 2;
            tower.damageTypes = new List<DamageTypes> {DamageTypes.Normal};
        });
        path1[4] = new Upgrade("Ultra-Juggernaut", 18000, (tower) =>
        {
            tower.pierce = 200;
            tower.damage = 5;
            tower.pierce += 2;
        });
        // Path 2 Upgrades
        path2[0] = new Upgrade("Quick Shots", 120, (tower) =>
        {
            tower.shootDelayMultiplier = 0.85f;
        });
        path2[1] = new Upgrade("Very Quick Shots", 230, (tower) =>
        {
            tower.shootDelayMultiplier = 0.67f;
        });
        path2[2] = new Upgrade("Triple Shot", 480, (tower) =>
        {
            tower.spread = 5;
            tower.bulletsPerShot = 3;
        });
        path2[3] = new Upgrade("Super Monkey Fan Club", 9600, (tower) =>
        {
            tower.shootDelayMultiplier = 0.5025f;
        });
        path2[4] = new Upgrade("Plasma Monkey Fan Club", 54000, (tower) =>
        {
            tower.pierce += 2;
            tower.damage += 3;
            tower.shootDelayMultiplier = 0.335f;
        });
        // Path 3 Upgrades
        path3[0] = new Upgrade("Long Range Darts", 110, (tower) =>
        {
            tower.range += 8;
        });
        path3[1] = new Upgrade("Enhanced Eyesight", 215, (tower) =>
        {
            tower.range += 8;
            tower.dartVelocity *= 1.1f;
        });
        path3[2] = new Upgrade("Crossbow", 750, (tower) =>
        {
            tower.damage += 3;
            tower.pierce += 1;
            tower.range += 8;
            tower.dartVelocity += 1;
        });
        path3[3] = new Upgrade("Sharp Shooter", 2160, (tower) =>
        {
            tower.damage += 3;
            tower.shootDelay = 0.6f;
            tower.dartVelocity += 3;
            tower.range += 4;
        });
        path3[4] = new Upgrade("Crossbow Master", 25800, (tower) =>
        {
            tower.pierce += 5;
            if(path1Index == 1 || path1Index == 2) tower.pierce += 5;
            tower.dartVelocity += 2;
            tower.damageTypes = new List<DamageTypes> {DamageTypes.Normal};
        });
    }
}