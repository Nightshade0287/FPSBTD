using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    public Camera cam;
    private float xRotation = 0f;
    public float xSensitivity = 30f;
    public float ySensitivity = 30f;
    public Vector2 moveVector;

    public float aimMultiplier;
    public void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void LateUpdate()
    {
        xRotation -= (moveVector.y * Time.deltaTime) * ySensitivity * aimMultiplier;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        transform.Rotate(Vector3.up * (moveVector.x * Time.deltaTime) * xSensitivity * aimMultiplier);
    }
    public void TakeRecoil(Vector2 recoil)
    {

        xRotation -= (recoil.y + moveVector.y);
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        transform.Rotate(Vector3.up * (recoil.x + moveVector.x));
    }

    public void OnLook(InputAction.CallbackContext ctx)
    {
        moveVector = ctx.ReadValue<Vector2>();
    }
}
