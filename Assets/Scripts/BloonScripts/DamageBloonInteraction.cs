using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DamageBloonInteraction
{
    // A dictionary mapping DamageTypes to a list of BloonTypes that they can pop
    // A dictionary mapping DamageTypes to a list of BloonTypes that they can pop
    public static Dictionary<DamageTypes, List<BloonTypes>> damageBloonMap = new Dictionary<DamageTypes, List<BloonTypes>>()
    {
        { DamageTypes.Normal, new List<BloonTypes> { BloonTypes.Basic, BloonTypes.Black, BloonTypes.White, BloonTypes.Lead, BloonTypes.Ceramic, BloonTypes.Purple, BloonTypes.Frozen } },
        { DamageTypes.Acid, new List<BloonTypes> { BloonTypes.Basic, BloonTypes.Black, BloonTypes.White, BloonTypes.Lead, BloonTypes.Ceramic, BloonTypes.Purple, BloonTypes.Frozen } },
        { DamageTypes.Sharp, new List<BloonTypes> { BloonTypes.Basic, BloonTypes.Black, BloonTypes.White, BloonTypes.Ceramic, BloonTypes.Frozen } },
        { DamageTypes.Explosion, new List<BloonTypes> { BloonTypes.Basic, BloonTypes.White, BloonTypes.Lead, BloonTypes.Ceramic, BloonTypes.Purple, BloonTypes.Frozen } },
        { DamageTypes.Cold, new List<BloonTypes> { BloonTypes.Basic, BloonTypes.Black, BloonTypes.Ceramic, BloonTypes.Purple } },
        { DamageTypes.Glacier, new List<BloonTypes> { BloonTypes.Basic, BloonTypes.Black, BloonTypes.Ceramic, BloonTypes.Purple, BloonTypes.Frozen } },
        { DamageTypes.Shatter, new List<BloonTypes> { BloonTypes.Basic, BloonTypes.Black, BloonTypes.White, BloonTypes.Ceramic, BloonTypes.Purple, BloonTypes.Frozen } },
        { DamageTypes.Energy, new List<BloonTypes> { BloonTypes.Basic, BloonTypes.Black, BloonTypes.White, BloonTypes.Ceramic, BloonTypes.Frozen } },
        { DamageTypes.Arctic, new List<BloonTypes> { BloonTypes.Basic, BloonTypes.Black, BloonTypes.Lead, BloonTypes.Ceramic, BloonTypes.Frozen } },
        { DamageTypes.Plasma, new List<BloonTypes> { BloonTypes.Basic, BloonTypes.Black, BloonTypes.White, BloonTypes.Lead, BloonTypes.Ceramic, BloonTypes.Purple, BloonTypes.Frozen } },
        { DamageTypes.Fire, new List<BloonTypes> { BloonTypes.Basic, BloonTypes.Black, BloonTypes.White, BloonTypes.Lead, BloonTypes.Ceramic, BloonTypes.Frozen } },
        { DamageTypes.CamoDetection, new List<BloonTypes> { BloonTypes.Basic, BloonTypes.Black, BloonTypes.White, BloonTypes.Lead, BloonTypes.Ceramic, BloonTypes.Purple, BloonTypes.Frozen, BloonTypes.Camo } },
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

