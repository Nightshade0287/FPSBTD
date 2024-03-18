using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    public PlayerInput.OnFootActions onFoot;
    public PlayerInput.ShootingActions shooting;
    private PlayerMotor motor;
    private PlayerLook look;
    private PlayerHand hand;
    private GunController gun;
    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;
        shooting = playerInput.Shooting;

        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();
        hand = GetComponent<PlayerHand>();
        onFoot.Jump.performed += ctx => motor.Jump();
        onFoot.Crouch.performed += ctx => motor.Crouch();
        onFoot.Crouch.performed += ctx => DeBugFunction();
        //onFoot.Crouch.canceled += ctx => motor.Crouch();
        onFoot.Sprint.performed += ctx => motor.Sprint();
        onFoot.Sprint.canceled += ctx => motor.Sprint();
        onFoot.DropThrow.performed += ctx => hand.Count();
        onFoot.DropThrow.canceled += ctx => hand.Count();
        shooting.Shoot.performed += ctx => 
        { 
            if (gun != null)
            {
                gun.Shoot();
                gun.shooting = true;
            } 
        };
        shooting.Shoot.canceled += ctx => { if (gun != null) gun.shooting = false; };
        shooting.Aim.performed += ctx => { if (gun != null) gun.aiming = true; };
        shooting.Aim.canceled += ctx => { if (gun != null) gun.aiming= false; };
        shooting.Reload.performed += ctx => { if (gun != null) StartCoroutine(gun.Reload()); }; 
        shooting.ChangeFireMode.performed += ctx => { if (gun != null) gun.CycleFireMode(); };   
    }

    private void Update()
    {
        if(transform.Find("PlayerCam").Find("GunHolder").childCount != 0)
            gun = GameObject.FindGameObjectWithTag("Gun").GetComponent<GunController>();
    }

    void FixedUpdate()
    {
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
    }

    void LateUpdate()
    {
        if(gun != null)
            if(gun.aiming)
                look.moveVector = onFoot.Look.ReadValue<Vector2>() * gun.sensMultiplier;
            else
                look.moveVector = onFoot.Look.ReadValue<Vector2>();
        else
            look.moveVector = onFoot.Look.ReadValue<Vector2>();
    }

    private void OnEnable()
    {
        onFoot.Enable();
        shooting.Enable();
    }

    private void OnDisable()
    {
        onFoot.Disable();
        shooting.Disable();
    }

    private void DeBugFunction()
    {
        Debug.Log("DebugFuntion");
    }
}
