using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartMonkey : TowerInfo
{
    public override void DefineUpgrades()
    {
        // Path 1 Upgrades
        path1[0] = new Upgrade("Sharp Shots", 170, (baseTower) =>
        {
            baseTower.pierce += 1;
            if(path3Index == 5) baseTower.pierce += 5;
        });
        path1[1] = new Upgrade("Razor Sharp Shots", 265, (baseTower) =>
        {
            baseTower.pierce += 2;
            if(path3Index == 5 && baseTower.pierce == 8) baseTower.pierce += 5;
        });
        path1[2] = new Upgrade("Spike-O-Pult", 360, (baseTower) =>
        {
            baseTower.pierce = 22;
            baseTower.damage = 2;
            baseTower.rangeMultiplier *= 1.15f;
            baseTower.shootDelay = 1.15f;
        });
        path1[3] = new Upgrade("Juggernaut", 2160, (baseTower) =>
        {
            baseTower.pierce = 55;
            baseTower.damage = 2;
            baseTower.pierce += 2;
        });
        path1[4] = new Upgrade("Ultra-Juggernaut", 18000, (baseTower) =>
        {
            baseTower.pierce = 200;
            baseTower.damage = 5;
            baseTower.pierce += 2;
        });
        // Path 2 Upgrades
        path2[0] = new Upgrade("Quick Shots", 120, (baseTower) =>
        {
            baseTower.shootDelayMultiplier = 1.176f;
        });
        path2[1] = new Upgrade("Very Quick Shots", 230, (baseTower) =>
        {
            baseTower.shootDelayMultiplier = 1.50f;
        });
        path2[2] = new Upgrade("Triple Shot", 480, (baseTower) =>
        {
            baseTower.spread = 30;
            baseTower.bulletsPerShot = 3;
        });
        path2[3] = new Upgrade("Super Monkey Fan Club", 9600, (baseTower) =>
        {
            baseTower.shootDelayMultiplier *= 1.5f;
        });
        path2[4] = new Upgrade("Plasma Monkey Fan Club", 54000, (baseTower) =>
        {
            baseTower.pierce += 2;
            baseTower.damage += 3;
            baseTower.shootDelayMultiplier *= 1.5f;
        });
        // Path 3 Upgrades
        path3[0] = new Upgrade("Long Range Darts", 110, (baseTower) =>
        {
            baseTower.range += 8;
        });
        path3[1] = new Upgrade("Enhanced Eyesight", 215, (baseTower) =>
        {
            baseTower.range += 8;
            baseTower.dartVelocity *= 1.1f;
        });
        path3[2] = new Upgrade("Crossbow", 750, (baseTower) =>
        {
            baseTower.damage += 3;
            baseTower.pierce += 1;
            baseTower.range += 8;
            baseTower.dartVelocity += 1;
        });
        path3[3] = new Upgrade("Sharp Shooter", 2160, (baseTower) =>
        {
            baseTower.damage += 3;
            baseTower.shootDelay = 0.6f;
            baseTower.dartVelocity += 3;
            baseTower.range += 4;
        });
        path3[4] = new Upgrade("Crossbow Master", 25800, (baseTower) =>
        {
            baseTower.pierce += 5;
            if(path1Index == 1 || path1Index == 2) baseTower.pierce += 5;
            baseTower.dartVelocity += 2;
        });
    }
}