using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    private float X;
    private float Y;
    public float sensX;
    public float sensY;
    public float aimSensX;
    public float aimSensY;

    public Transform orientation;
    public Transform camHolder;

    public KeyCode aimKey = KeyCode.Mouse1;

    float xRotation;
    float yRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        sens();
        // get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * X * 100;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * Y * 100;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // rotate cam and orientation
        camHolder.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    private void sens()
    {
        if (Input.GetKey(aimKey))
        {
            X = aimSensX;
            Y = aimSensY;
        } 
        else
        {
            X = sensX;
            Y = sensY;
        }
    }
}
