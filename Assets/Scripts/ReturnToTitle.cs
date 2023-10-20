using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToTitle : MonoBehaviour
{
    public KeyCode key;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene("TitleScreen");
        }
    }
}
