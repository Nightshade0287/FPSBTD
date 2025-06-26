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
    public Economy_Health economy;
    // Start is called before the first frame update
    void Start()
    {
        economy = GameObject.Find("GameManagemer").GetComponent<Economy_Health>();
        UpdateHealth();
        UpdateHealth();
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

    public void UpdateMoney()
    {
        moneyText.text = "$" + economy.cash;
    }
    public void UpdateRound(int number)
    {
        roundText.text = "Round " + number;
    }
    public void UpdateHealth()
    {
        healthText.text = "Health " + economy.health;
    }
}
