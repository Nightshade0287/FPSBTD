using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.InputSystem;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;

    private PlayerInput playerInput;
    private Vector3 playerVelocity;
    private bool isGrounded;
    private float speed;

    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;
    public float gravity = -9.8f;
    public float jumpHeight = 3f;

    private Vector3 moveDirection;
    // Start is called before the first frame update
    void Start()
    {
        playerInput = new PlayerInput();
        controller = GetComponent<CharacterController>();
        speed = walkSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;
        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
        playerVelocity.y += gravity * Time.deltaTime;
        if(isGrounded && playerVelocity.y < 0)
            playerVelocity.y = -2f;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void Movement(InputAction.CallbackContext ctx)
    {
        Vector2 input = ctx.ReadValue<Vector2>();
        moveDirection = new Vector3(input.x, 0, input.y);
    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        if(isGrounded && ctx.performed)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }


    public void Sprint(InputAction.CallbackContext ctx)
    {
        if(ctx.performed)
            speed = sprintSpeed;
        else if (ctx.canceled)
            speed = walkSpeed;
    }
}
