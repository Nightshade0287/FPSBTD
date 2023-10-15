using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Inputs input;
    private Vector2 moveVector;
    private Rigidbody rb;
    private Vector3 moveDirection;

    [Header("Variables")]
    public float moveSpeed = 10f;
    public Vector3 test;

    [Header("References")]
    public Transform orientation;

    private void Awake()
    {
        input = new Inputs();
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        input.Enable();
        input.Default.Movement.performed += OnMovementPerformed;
        input.Default.Movement.canceled += OnMovementCanceled;
    }

    private void OnDisable()
    {
        input.Disable();
        input.Default.Movement.performed -= OnMovementPerformed;
        input.Default.Movement.canceled -= OnMovementCanceled;

    }

    private void FixedUpdate()
    {
        rb.velocity =  new Vector3(moveVector.x, 0, moveVector.y) * moveSpeed;
        test = rb.velocity;
    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        moveVector = value.ReadValue<Vector2>();
    }

    private void OnMovementCanceled(InputAction.CallbackContext value)
    {
        moveVector = Vector2.zero;
    }
}
