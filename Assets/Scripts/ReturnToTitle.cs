using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ReturnToTitle : MonoBehaviour
{
    public void ExitToMenu(InputAction.CallbackContext ctx)
    {
        if(ctx.performed)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene("TitleScreen");
        }
    }
}
