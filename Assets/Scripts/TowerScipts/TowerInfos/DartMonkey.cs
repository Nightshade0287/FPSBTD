using System.Collections.Generic;
using UnityEngine;

public class DartMonkey : TowerInfo
{
    [Header("Path1 References")]
    public GameObject spikeBallPrefab;
    public GameObject juggernautPrefab;
    public GameObject ultraJuggernautPrefab;
    public override void DefineUpgrades()
    {
        // Path 1
        path1[0] = new Upgrade("Sharp Shots", 170, tower =>{
            tower.pierce += 1;
        });

        path1[1] = new Upgrade("Razor Sharp Shots", 265, tower =>{
            tower.pierce += 2;
        });

        path1[2] = new Upgrade("Spike-O-Pult", 360, tower =>{
            tower.pierce = 18;
            tower.damage = 2;
            tower.dartVelocity = 15;
            tower.range = 8;
            tower.lifeSpan = 1;
            tower.shootDelay = 1.15f;
            tower.damageTypes.Add(DamageTypes.Shatter);
            tower.damageTypes.Remove(DamageTypes.Sharp);
            tower.dartPrefab = spikeBallPrefab;
            tower.shootPoint.localPosition = new Vector3(1.1f, 1.375f, 0.17f);
        });

        path1[3] = new Upgrade("Juggernaut", 2160, tower =>{
            tower.pierce = 50;
            tower.damage = 2;
            tower.dartVelocity = 25;
            tower.lifeSpan = 1;
            tower.damageTypes.Add(DamageTypes.Normal);
            tower.damageTypes.Remove(DamageTypes.Shatter);
            tower.AddOrUpdateBuff(BloonTypes.Ceramic, 3);
            tower.dartPrefab = juggernautPrefab;
        });

        path1[4] = new Upgrade("Ultra-Juggernaut", 18000, tower =>{
            tower.pierce = 150;
            tower.damage = 5;
            tower.AddOrUpdateBuff(BloonTypes.Ceramic, 5);
            tower.AddOrUpdateBuff(BloonTypes.Lead, 20);;
            tower.dartPrefab = ultraJuggernautPrefab;
        });

        // Path 2
        path2[0] = new Upgrade("Quick Shots", 120, tower =>{
            tower.shootDelayMultiplier *= 0.85f;
        });

        path2[1] = new Upgrade("Very Quick Shots", 230, tower =>{
            tower.shootDelayMultiplier *= 0.8f;
        });

        path2[2] = new Upgrade("Triple Shot", 480, tower =>{
            tower.shootDelayMultiplier *= 1f;
            tower.shootDelay = 0.47475f;
            tower.dartsPerShot = 3;
            tower.spread = 5;
        });

        path2[3] = new Upgrade("Super Monkey Fan Club", 9600, tower =>{
            tower.range = 9;
            tower.shootDelay = 0.2378f;
        });

        path2[4] = new Upgrade("Plasma Monkey Fan Club", 54000, tower =>{
            tower.pierce += 2;
            tower.damage += 3;
            tower.shootDelay = 0.2378f;
        });

        // Path 3
        path3[0] = new Upgrade("Long Range Darts", 110, tower =>{
            tower.rangeMultiplier *= 1.25f;
            tower.lifeSpanMultiplier *= 1.15f;
        });

        path3[1] = new Upgrade("Enhanced Eyesight", 215, tower =>{
            tower.rangeMultiplier *= 1.20f;
            tower.dartVelocityMultiplier *= 1.1f;
            tower.lifeSpanMultiplier *= 1.2f;
            if (!tower.damageTypes.Contains(DamageTypes.CamoDetection))
                tower.damageTypes.Add(DamageTypes.CamoDetection);
        });

        path3[2] = new Upgrade("Crossbow", 750, tower =>{
            tower.rangeMultiplier = 1;
            tower.lifeSpanMultiplier = 1;
            tower.damage = 3;
            tower.pierce += 1;
            tower.range = 12;
            tower.dartVelocity = 54;
            tower.lifeSpan = 0.316f;
        });

        path3[3] = new Upgrade("Sharp Shooter", 2160, tower =>
        {
            tower.damage += 3;
            tower.shootDelay = 0.475f;
            tower.dartVelocity = 80;
            tower.critical = new Critical(50, 10);
        });

        path3[4] = new Upgrade("Crossbow Master", 25800, tower =>
        {
            tower.pierce += 5;
            tower.damageTypes = new List<DamageTypes> { DamageTypes.Normal };
            tower.shootDelay = 0.24f;
            tower.critical = new Critical(80, 5);
        });
    }
    public override void ApplyCrosspathEffects()
    {
        //Path 1
        if (path1Index == 1 && path3Index == 5) tower.pierce = 16;
        if (path1Index == 2 && path3Index == 5) tower.pierce = 24;
    }
}
