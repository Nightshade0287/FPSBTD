using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using System.Security.Cryptography.X509Certificates;

[System.Serializable]
public class Upgrade
{
    public string name;
    public int price;
    public bool upgradeInfoFoldout = true; // Flag to indicate whether upgrade info is folded out
    public Action<BaseTower> UpgradeAction; // Delegate to define upgrade behavior

    public Upgrade(string name, int price, Action<BaseTower> upgradeAction)
    {
        this.name = name;
        this.price = price;
        UpgradeAction = upgradeAction;
    }
}
public class TowerInfo : MonoBehaviour
{
    [Header("Base Tower Stats")]
    public string towerName;
    public int cost;
    public GameObject dartPrefab;
    public List<DamageTypes> damageTypes = new List<DamageTypes>();
    public List<DamageBuff> damageBuffs = new List<DamageBuff>();
    public int damage;
    public int pierce;
    public float range;
    public float lifeSpan;
    public float dartVelocity;
    public float shootDelay;
    public float spread;
    public int DartsPerShot = 1;
    public BaseTower tower;
    public List<TargetingMode> availableTargetingModes = new List<TargetingMode> { TargetingMode.First, TargetingMode.Last, TargetingMode.Closest, TargetingMode.Strongest };
    private int currentTargetingMode;
    private int value = 0; // Value of the Tower, Cost + upgrades
    private Transform towerUpgrades;
    public Economy_Health economy;
    [HideInInspector]
    [SerializeField]
    [Range(5, 5)]
    public Upgrade[] path1 = new Upgrade[5];

    [HideInInspector]
    [SerializeField]
    [Range(5, 5)]
    public Upgrade[] path2 = new Upgrade[5];

    [HideInInspector]
    [SerializeField]
    [Range(5, 5)]
    public Upgrade[] path3 = new Upgrade[5];
    [HideInInspector]
    public int path1Index = 0;
    [HideInInspector]
    public int path2Index = 0;
    [HideInInspector]
    public int path3Index = 0;
    // Method to handle upgrading the tower

    public void Start()
    {
        value += cost;
        economy = GameObject.Find("Economy/Health").GetComponent<Economy_Health>();
        DefineUpgrades();
    }
    public virtual void DefineUpgrades() {} // Defined in Individual tower Scripts
    public virtual void ApplyCrosspathEffects() {} // Defined in Individual tower Scripts
    public void UpgradeTower(int path)
    {
        Upgrade[] selectedPath;
        int selectedIndex;

        switch (path)
        {
            case 1:
                if (path2Index > 0 && path3Index > 0 || ((path1Index == 2) && (path2Index > 2 || path3Index > 2)))
                    return;
                selectedPath = path1;
                selectedIndex = path1Index;
                break;
            case 2:
                if (path1Index > 0 && path3Index > 0 || ((path2Index == 2) && (path1Index > 2 || path3Index > 2)))
                    return;
                selectedPath = path2;
                selectedIndex = path2Index;
                break;
            case 3:
                if (path1Index > 0 && path2Index > 0 || ((path3Index == 2) && (path1Index > 2 || path2Index > 2)))
                    return;
                selectedPath = path3;
                selectedIndex = path3Index;
                break;
            default:
                Debug.LogError("Invalid path selected.");
                return;
        }

        if (selectedIndex < selectedPath.Length)
        {
            value += selectedPath[selectedIndex].price;
            economy.cash -= selectedPath[selectedIndex].price;
            selectedPath[selectedIndex].UpgradeAction(tower);
            selectedIndex++;
        }
        else
        {
            Debug.LogWarning("Path " + path + " upgrade index out of range.");
        }

        // Update the path index
        switch (path)
        {
            case 1:
                path1Index = selectedIndex;
                break;
            case 2:
                path2Index = selectedIndex;
                break;
            case 3:
                path3Index = selectedIndex;
                break;
        }
        ApplyCrosspathEffects();
        UpdateTowerUpgradeUI();
    }
    public void CycleTargetingMode(int direction)
    {
        Debug.Log(currentTargetingMode);
        currentTargetingMode = (currentTargetingMode + direction + availableTargetingModes.Count) % availableTargetingModes.Count;
        UpdateTowerUpgradeUI();
    }

    public void Sell()
    {
        economy.UpdateMoney(value / 2);
        Destroy(gameObject);
    }
    public void UpdateTowerUpgradeUI()
    {
        ResetUpgradeUI();
        Upgrade[][] paths = new Upgrade[][] { path1, path2, path3 };
        int[] pathIndexs = new int[] { path1Index, path2Index, path3Index };
        towerUpgrades = GameObject.Find("TowerUI").transform.Find("Upgrades");
        towerUpgrades.Find("Tower Name").GetComponent<TextMeshProUGUI>().text = towerName;
        towerUpgrades.Find("Targeting Mode").Find("Name").GetComponent<TextMeshProUGUI>().text = Enum.GetName(typeof(TargetingMode), currentTargetingMode);
        for (int i = 0; i <= 2; i++)
        {
            Transform path = towerUpgrades.Find("Upgrades").Find("Path " + (i + 1));
            Transform upgradeButton = path.Find("Upgrade Button");
            //Upgrade Indicater
            Transform upgradeIndex = path.Find("Upgrade Index");
            for (int x = 0; x < pathIndexs[i]; x++)
            {
                upgradeIndex.GetChild(x).Find("On").gameObject.SetActive(true);
            }

            //Current Upgrade
            Transform currentUpgrade = path.Find("Current Upgrade");
            if (pathIndexs[i] > 0)
            {
                currentUpgrade.Find("Not Upgraded").gameObject.SetActive(false);
                currentUpgrade.Find("Upgrade Name").gameObject.SetActive(true);
                currentUpgrade.Find("Upgrade Name").GetComponent<TextMeshProUGUI>().text = paths[i][pathIndexs[i] - 1].name;
            }

            //Upgrade Button
            if (pathIndexs[i] < 5)
            {
                upgradeButton.Find("Name").GetComponent<TextMeshProUGUI>().text = paths[i][pathIndexs[i]].name;
                upgradeButton.Find("Price").GetComponent<TextMeshProUGUI>().text = "$" + paths[i][pathIndexs[i]].price;
                if (economy.cash >= paths[i][pathIndexs[i]].price)
                {
                    upgradeButton.Find("Background").Find("Buyable").gameObject.SetActive(true);
                    upgradeButton.Find("Price").GetComponent<TextMeshProUGUI>().color = Color.white;
                }
                else
                {
                    upgradeButton.Find("Background").Find("Buyable").gameObject.SetActive(false);
                    upgradeButton.Find("Price").GetComponent<TextMeshProUGUI>().color = Color.red;
                }
            }
            else
            {
                upgradeButton.Find("Name").GetComponent<TextMeshProUGUI>().text = "Max Upgrades";
                upgradeButton.Find("Background").Find("Buyable").gameObject.SetActive(false);
                upgradeButton.Find("Price").gameObject.SetActive(false);
            }
            if (pathIndexs[(i + 4) % 3] > 0 && pathIndexs[(i + 2) % 3] > 0)
            {
                upgradeButton.gameObject.SetActive(false);
                upgradeIndex.gameObject.SetActive(false);
                path.Find("Cant Upgrade").gameObject.SetActive(true);
                currentUpgrade.Find("Not Upgraded").GetComponent<TextMeshProUGUI>().text = "Path Closed";
            }
            if ((pathIndexs[i] == 2) && (pathIndexs[(i + 4) % 3] > 2 || pathIndexs[(i + 2) % 3] > 2))
            {
                upgradeButton.Find("Name").GetComponent<TextMeshProUGUI>().text = "Max Upgrades";
                upgradeButton.Find("Background").Find("Buyable").gameObject.SetActive(false);
                upgradeButton.Find("Price").gameObject.SetActive(false);
            }
        }
        towerUpgrades.Find("Sell").Find("Sell Amount").GetComponent<TextMeshProUGUI>().text = "$ " + (value / 2);
    }

    public void ResetUpgradeUI()
    {
        Upgrade[][] paths = new Upgrade[][] { path1, path2, path3 };
        int[] pathIndexs = new int[] { path1Index, path2Index, path3Index };
        towerUpgrades = GameObject.Find("TowerUI").transform.Find("Upgrades");
        for (int i = 0; i <= 2; i++)
        {
            Transform path = towerUpgrades.Find("Upgrades").Find("Path " + (i + 1));
            Transform upgradeButton = path.Find("Upgrade Button");
            //Upgrade Indicater

            upgradeButton.gameObject.SetActive(true);
            path.Find("Cant Upgrade").gameObject.SetActive(false);
            Transform upgradeIndex = path.Find("Upgrade Index");
            upgradeIndex.gameObject.SetActive(true);
            for (int x = 0; x < 5; x++)
            {
                upgradeIndex.GetChild(x).Find("On").gameObject.SetActive(false);
            }

            //Current Upgrade
            Transform currentUpgrade = path.Find("Current Upgrade");
            currentUpgrade.Find("Not Upgraded").GetComponent<TextMeshProUGUI>().text = "Not Upgraded";
            currentUpgrade.Find("Not Upgraded").gameObject.SetActive(true);
            currentUpgrade.Find("Upgrade Name").gameObject.SetActive(false);

            //Upgrade Button
            upgradeButton.Find("Name").GetComponent<TextMeshProUGUI>().text = "";
            upgradeButton.Find("Price").gameObject.SetActive(true);
        }
        towerUpgrades.Find("Sell").Find("Sell Amount").GetComponent<TextMeshProUGUI>().text = "$ " + (value / 2).ToString();
    }
}

public enum TargetingMode
{
    First,
    Last,
    Closest,
    Strongest
}



