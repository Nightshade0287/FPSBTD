using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellTower : MonoBehaviour
{
    private TowerInfo towerInfo;
    private PlayerUI playerUI;
    public void Sell()
    {
        towerInfo = GetComponent<TowerInfo>();
        playerUI = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerUI>();
        playerUI.UpdateMoney(towerInfo.cost / 2);
        Destroy(gameObject);
    }
}
