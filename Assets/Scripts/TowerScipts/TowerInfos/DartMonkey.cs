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
        path1[0] = new Upgrade("Sharp Shots", 170, twr =>{
            twr.pierce += 1;
        });

        path1[1] = new Upgrade("Razor Sharp Shots", 265, twr =>{
            twr.pierce += 2;
        });

        path1[2] = new Upgrade("Spike-O-Pult", 360, twr =>{
            twr.pierce = 18;
            twr.damage = 2;
            twr.dartVelocity = 15;
            twr.range = 8;
            twr.lifeSpan = 1;
            twr.shootDelay = 1.15f;
            twr.damageTypes.Add(DamageTypes.Shatter);
            twr.damageTypes.Remove(DamageTypes.Sharp);
            twr.dartPrefab = spikeBallPrefab;
            twr.shootPoint.localPosition = new Vector3(1.1f, 1.375f, 0.17f);
        });

        path1[3] = new Upgrade("Juggernaut", 2160, twr =>{
            twr.pierce = 50;
            twr.damage = 2;
            twr.dartVelocity = 25;
            twr.lifeSpan = 1;
            twr.damageTypes.Add(DamageTypes.Normal);
            twr.damageTypes.Remove(DamageTypes.Shatter);
            twr.AddOrUpdateBuff(BloonTypes.Ceramic, 3);
            twr.dartPrefab = juggernautPrefab;
        });

        path1[4] = new Upgrade("Ultra-Juggernaut", 18000, twr =>{
            twr.pierce = 150;
            twr.damage = 5;
            twr.AddOrUpdateBuff(BloonTypes.Ceramic, 5);
            twr.AddOrUpdateBuff(BloonTypes.Lead, 20);;
            twr.dartPrefab = ultraJuggernautPrefab;
        });

        // Path 2
        path2[0] = new Upgrade("Quick Shots", 120, twr =>{
            twr.shootDelayMultiplier *= 0.85f;
        });

        path2[1] = new Upgrade("Very Quick Shots", 230, twr =>{
            twr.shootDelayMultiplier *= 0.8f;
        });

        path2[2] = new Upgrade("Triple Shot", 480, twr =>{
            twr.shootDelayMultiplier *= 1f;
            twr.shootDelay = 0.47475f;
            twr.dartsPerShot = 3;
            twr.spread = 5;
        });

        path2[3] = new Upgrade("Super Monkey Fan Club", 9600, twr =>{
            twr.range = 9;
            twr.shootDelay = 0.2378f;
        });

        path2[4] = new Upgrade("Plasma Monkey Fan Club", 54000, twr =>{
            twr.pierce += 2;
            twr.damage += 3;
            twr.shootDelay = 0.2378f;
        });

        // Path 3
        path3[0] = new Upgrade("Long Range Darts", 110, twr =>{
            twr.rangeMultiplier *= 1.25f;
            twr.lifeSpanMultiplier *= 1.15f;
        });

        path3[1] = new Upgrade("Enhanced Eyesight", 215, twr =>{
            twr.rangeMultiplier *= 1.20f;
            twr.dartVelocityMultiplier *= 1.1f;
            twr.lifeSpanMultiplier *= 1.2f;
            if (!twr.damageTypes.Contains(DamageTypes.CamoDetection))
                twr.damageTypes.Add(DamageTypes.CamoDetection);
        });

        path3[2] = new Upgrade("Crossbow", 750, twr =>{
            twr.rangeMultiplier = 1;
            twr.lifeSpanMultiplier = 1;
            twr.damage = 3;
            twr.pierce += 1;
            twr.range = 12;
            twr.dartVelocity = 54;
            twr.lifeSpan = 0.316f;
        });

        path3[3] = new Upgrade("Sharp Shooter", 2160, twr =>
        {
            twr.damage += 3;
            twr.shootDelay = 0.475f;
            twr.dartVelocity = 80;
            twr.critical = new Critical(50, 10);
        });

        path3[4] = new Upgrade("Crossbow Master", 25800, twr =>
        {
            twr.pierce += 5;
            twr.damageTypes = new List<DamageTypes> { DamageTypes.Normal };
            twr.shootDelay = 0.24f;
            twr.critical = new Critical(80, 5);
        });
    }
    public override void ApplyCrosspathEffects()
    {
        //Path 1
        if (path1Index == 1 && path3Index == 5) twr.pierce = 16;
        if (path1Index == 2 && path3Index == 5) twr.pierce = 24;
    }
}
