using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI promptText;
    // Start is called before the first frame update
    void Start()
    {
        UpdateText(string.Empty);
    }

    // Update is called once per frame
    public void UpdateText(string promptMessage)
    {
        Debug.Log("Text Updated");
        promptText.text = promptMessage;
    }
}
