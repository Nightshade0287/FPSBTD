using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Globalization;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI promptText;
    [SerializeField]
    private TextMeshProUGUI towerText;
    [SerializeField]
    private TextMeshProUGUI moneyText;
    [SerializeField]
    private TextMeshProUGUI roundText;
    [SerializeField]
    private TextMeshProUGUI healthText;
    public int cash;
    public int health;
    public bool infinteCash;
    public bool infinteHealth;
    // Start is called before the first frame update
    void Start()
    {
        // For loop iterating from 0 to 555
        int num = 0;
        for (int i = 0; i <= 555; i++)
        {
            int a = i / 100 % 10;
            int b = i / 10 % 10;
            int c = i % 10;
            if(a <= 5 && b <= 5 && c <= 5)
            {
                if(a == 0 || b == 0 || c == 0)
                {
                    if((a == 0 && !(b > 2 && c > 2)) || (b == 0 && !(a > 2 && c > 2)) || (c == 0 && !(b > 2 && a > 2)))
                    {
                        num++;
                        Debug.Log(num + ": " + a + " " + b + " " + c);
                    }
                }
            }
        }
        if(infinteCash)
            cash = 100000;
        UpdateMoney(0);
        UpdateHealth(0);
    }

    // Update is called once per frame
    public void UpdateText(string message)
    {
        promptText.text = message;
    }
    public void UpdateTowerText(string message)
    {
        towerText.text = message;
    }

    public void UpdateMoney(int amount)
    {
        if(infinteCash)
            return;
        cash += amount;
        moneyText.text = "$" + cash;
    }
    public void UpdateRound(int number)
    {
        roundText.text = "Round " + number;
    }
    public void UpdateHealth(int amount)
    {
        if(infinteHealth)
            return;
        if(health - amount > 0)
        {
            health -= amount;
            healthText.text = "Health " + health;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene("TitleScreen");
        }
    }
}
