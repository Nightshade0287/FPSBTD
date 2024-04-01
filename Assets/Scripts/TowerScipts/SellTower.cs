using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellTower : Interactable
{
    private TowerInfo towerInfo;
    private PlayerUI playerUI;
    protected override void Interact()
    {
        towerInfo = GetComponent<TowerInfo>();
        playerUI = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerUI>();
        playerUI.UpdateMoney(towerInfo.cost / 2);
        Destroy(gameObject);
    }
}
