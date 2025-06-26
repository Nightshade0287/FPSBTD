using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public enum DamageTypes
{
    Normal,
    Acid,
    Sharp,
    Explosion,
    Cold,
    Glacier,
    Shatter,
    Energy,
    Arctic,
    Plasma,
    Fire,
    CamoDetection
}
[System.Serializable]
public enum BloonTypes
{
    Basic,
    Moab,
    Black,
    White,
    Lead,
    Ceramic,
    Purple,
    Frozen,
    Camo
}
[System.Serializable]
public class DamageBuff
{
    public BloonTypes bloonType;
    public int buffAmount;
}

[System.Serializable]
public class Critical
{
    public int critAmount;
    public int shotThreshold;
    private int shotCount;
    public int CheckForCritical()
    {
        shotCount++;
        if (shotCount >= shotThreshold)
        {
            shotCount = 0;
            return critAmount;
        }
        else return 0;
    }

    public Critical(int critAmount, int shotThreshold)
    {
        this.critAmount = critAmount;
        this.shotThreshold = shotThreshold;
    }
}
public static class DamageBloonInteraction
{
    // A dictionary mapping DamageTypes to a list of BloonTypes that they can pop
    // A dictionary mapping DamageTypes to a list of BloonTypes that they can pop
    public static Dictionary<DamageTypes, List<BloonTypes>> damageBloonMap = new Dictionary<DamageTypes, List<BloonTypes>>()
    {
        { DamageTypes.Normal, new List<BloonTypes> { BloonTypes.Basic, BloonTypes.Moab, BloonTypes.Black, BloonTypes.White, BloonTypes.Lead, BloonTypes.Ceramic, BloonTypes.Purple, BloonTypes.Frozen } },
        { DamageTypes.Acid, new List<BloonTypes> { BloonTypes.Basic, BloonTypes.Moab, BloonTypes.Black, BloonTypes.White, BloonTypes.Lead, BloonTypes.Ceramic, BloonTypes.Purple, BloonTypes.Frozen } },
        { DamageTypes.Sharp, new List<BloonTypes> { BloonTypes.Basic, BloonTypes.Moab, BloonTypes.Black, BloonTypes.White, BloonTypes.Ceramic, BloonTypes.Frozen } },
        { DamageTypes.Explosion, new List<BloonTypes> { BloonTypes.Basic, BloonTypes.Moab, BloonTypes.White, BloonTypes.Lead, BloonTypes.Ceramic, BloonTypes.Purple, BloonTypes.Frozen } },
        { DamageTypes.Cold, new List<BloonTypes> { BloonTypes.Basic, BloonTypes.Moab, BloonTypes.Black, BloonTypes.Ceramic, BloonTypes.Purple } },
        { DamageTypes.Glacier, new List<BloonTypes> { BloonTypes.Basic, BloonTypes.Moab, BloonTypes.Black, BloonTypes.Ceramic, BloonTypes.Purple, BloonTypes.Frozen } },
        { DamageTypes.Shatter, new List<BloonTypes> { BloonTypes.Basic, BloonTypes.Moab, BloonTypes.Black, BloonTypes.White, BloonTypes.Ceramic, BloonTypes.Purple, BloonTypes.Frozen } },
        { DamageTypes.Energy, new List<BloonTypes> { BloonTypes.Basic, BloonTypes.Moab, BloonTypes.Black, BloonTypes.White, BloonTypes.Ceramic, BloonTypes.Frozen } },
        { DamageTypes.Arctic, new List<BloonTypes> { BloonTypes.Basic, BloonTypes.Moab, BloonTypes.Black, BloonTypes.Lead, BloonTypes.Ceramic, BloonTypes.Frozen } },
        { DamageTypes.Plasma, new List<BloonTypes> { BloonTypes.Basic, BloonTypes.Moab, BloonTypes.Black, BloonTypes.White, BloonTypes.Lead, BloonTypes.Ceramic, BloonTypes.Purple, BloonTypes.Frozen } },
        { DamageTypes.Fire, new List<BloonTypes> { BloonTypes.Basic, BloonTypes.Moab, BloonTypes.Black, BloonTypes.White, BloonTypes.Lead, BloonTypes.Ceramic, BloonTypes.Frozen } },
        { DamageTypes.CamoDetection, new List<BloonTypes> { BloonTypes.Camo } },
    };

    // Method to check if a given damage type can pop a specific bloon
    public static bool CanPop(List<BloonTypes> bloonTypes, List<DamageTypes> damageTypes)
    {
        foreach (BloonTypes bloonType in bloonTypes)
        {
            bool canBePopped = false;

            foreach (DamageTypes damage in damageTypes)
            {
                // Check if the current damage type can pop the current bloon type
                if (damageBloonMap.ContainsKey(damage) && damageBloonMap[damage].Contains(bloonType))
                {
                    canBePopped = true;
                    break; // Exit the inner loop as soon as we find a matching damage type for this bloon type
                }
            }

            // If any bloon type cannot be popped by any of the damage types, return false
            if (!canBePopped)
            {
                return false;
            }
        }

        // If all bloon types can be popped by at least one damage type, return true
        return true;
    }
}

