using UnityEngine;

[System.Serializable]
public class Upgrade
{
    public string upgradeName;
    public int price;
}

public class TowerInfo : MonoBehaviour
{
    public string towerName;
    public int cost;

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
}

