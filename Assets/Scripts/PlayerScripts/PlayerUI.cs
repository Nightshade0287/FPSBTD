using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

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
    // Start is called before the first frame update
    void Start()
    {
        //moneyText.text = "$" + cash;
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
        cash += amount;
        moneyText.text = "$" + cash;
    }
    public void UpdateRound(int number)
    {
        roundText.text = "Round " + number;
    }
    public void UpdateHealth(int amount)
    {
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
