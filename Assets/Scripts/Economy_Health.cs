using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Economy_Health : MonoBehaviour
{
    public int cash;
    public int health;
    public bool infinteCash;
    public bool infinteHealth;
    public PlayerUI playerUI;
    // Start is called before the first frame update
    void Start()
    {
        playerUI = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerUI>();
        if (infinteCash)
            cash = 999999;
    }
    public void UpdateMoney(int amount)
    {
        if (infinteCash)
            return;
        cash += amount;
        playerUI.UpdateMoney();
    }
    public void UpdateHealth(int amount)
    {
        if(infinteHealth)
            return;
        if (health - amount > 0)
        {
            health -= amount;
            playerUI.UpdateMoney();
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene("TitleScreen");
        }
    }
}
