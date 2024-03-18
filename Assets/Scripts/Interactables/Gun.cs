using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Gun : Interactable
{
    private Rigidbody rb;
    private BoxCollider cd;
    private GunController gc;
    private Transform playerCam;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cd = GetComponent<BoxCollider>();
        gc = GetComponent<GunController>();
        playerCam = GameObject.FindGameObjectWithTag("Player").transform.Find("PlayerCam");
    }

    // Update is called once per frame
    void Update()
    {
    }
    protected override void Interact()
    {
        transform.SetParent(playerCam.Find("GunHolder"));
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        gc.enabled = true;
        rb.isKinematic = true;
        cd.enabled = false;
        gameObject.layer = LayerMask.NameToLayer("Default");
        gameObject.tag = "Gun";
    }

    public void Throw(UnityEngine.Vector3 dir, float force)
    {
        transform.SetParent(transform.Find("Guns"));
        gc.aiming = false;
        gc.enabled = false;
        rb.isKinematic = false;
        cd.enabled = true;
        rb.AddForce(dir * force, ForceMode.Impulse);
        gameObject.layer = LayerMask.NameToLayer("Interactable");
        gameObject.tag = "Untagged";
    }
}
