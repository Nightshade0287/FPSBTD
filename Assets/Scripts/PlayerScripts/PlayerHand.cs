using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    public float throwForce = 5f;
    private float timeToThrow = 0.1f;
    private float maxTimeToThrow = 0.35f;
    private bool counting= false;
    private float pressTimer = 0f;
    private GameObject playerCam;
    private Transform gunHolder;
    private GameObject gun;

    void Start()
    {
        playerCam = GameObject.Find("PlayerCam");
        gunHolder = playerCam.transform.Find("GunHolder");
    }
    void Update()
    {
        if(counting)
            pressTimer += Time.deltaTime;
        else
            pressTimer = 0f;
    }
    public void Count()
    {
        if(gunHolder.transform.childCount != 0)
        {
            gun = GameObject.FindGameObjectWithTag("Gun");
            if(counting)
            {    
                if(pressTimer >= timeToThrow)
                {
                    float force;
                    if(pressTimer - timeToThrow >= maxTimeToThrow)
                        force = throwForce;
                    else
                        force = ((pressTimer - timeToThrow)/maxTimeToThrow) * throwForce;
                    gun.GetComponent<Gun>().Throw(playerCam.transform.forward, force);
                }
                else
                {
                    gun.GetComponent<Gun>().Throw(Vector3.zero, 0f);
                }
            }
            counting = !counting;
        }
    }
}
