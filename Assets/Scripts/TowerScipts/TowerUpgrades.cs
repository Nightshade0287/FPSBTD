using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpgradeOption
{
    public bool changeDamage;
    public int damageChange;

    public bool changePierce;
    public int pierceChange;

    public bool changeShootDelay;
    public float shootDelayChange;
}

[System.Serializable]
public class UpgradePath
{
    public string pathName;
    public List<UpgradeOption> upgrades = new List<UpgradeOption>();
}

public class TowerUpgrades : MonoBehaviour
{
    private Dictionary<UpgradePath, int> upgradeIndices = new Dictionary<UpgradePath, int>();

    // Enum to represent different upgrade paths
    public enum UpgradePathIndex
    {
        Path1,
        Path2,
        Path3
    }

    // Reference to the tower shooting script
    public BaseTower towerShooting;

    // List of upgrade paths
    public List<UpgradePath> upgradePaths = new List<UpgradePath>();

    // Apply the upgrade based on the selected path and upgrade index
    public void ApplyUpgrade(UpgradePathIndex pathIndex, int upgradeIndex)
    {
        // Get the upgrade path
        UpgradePath selectedPath = upgradePaths[(int)pathIndex];

        // Check if the upgrade index is valid for the selected path
        if (upgradeIndex >= 0 && upgradeIndex < selectedPath.upgrades.Count)
        {
            // Apply the upgrade effects
            UpgradeOption upgrade = selectedPath.upgrades[upgradeIndex];
            
            if (upgrade.changeDamage)
                towerShooting.damage += upgrade.damageChange;
            
            if (upgrade.changePierce)
                towerShooting.pierce += upgrade.pierceChange;
            
            if (upgrade.changeShootDelay)
                towerShooting.shootDelay = upgrade.shootDelayChange;

            // Increment the upgrade index for the selected path
            upgradeIndices[selectedPath] = upgradeIndex + 1;
        }
        else
        {
            Debug.LogError("Invalid upgrade index for path " + (int)pathIndex + ": " + upgradeIndex);
        }
    }

    private void Awake()
    {
        // Initialize upgrade indices
        foreach (UpgradePath path in upgradePaths)
        {
            upgradeIndices[path] = 0;
        }
    }

    public void Update()
    {
        // Testing upgrades (example: press I for Path1, O for Path2, P for Path3)
        if (Input.GetKeyDown(KeyCode.I))
        {
            ApplyUpgrade(UpgradePathIndex.Path1, upgradeIndices[upgradePaths[(int)UpgradePathIndex.Path1]]);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            ApplyUpgrade(UpgradePathIndex.Path2, upgradeIndices[upgradePaths[(int)UpgradePathIndex.Path2]]);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            ApplyUpgrade(UpgradePathIndex.Path3, upgradeIndices[upgradePaths[(int)UpgradePathIndex.Path3]]);
        }
    }
}
