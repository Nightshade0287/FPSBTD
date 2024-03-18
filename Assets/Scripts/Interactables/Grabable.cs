using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Grabable : Interactable
{
    //private PlayerHand playerHandScript;
    private Rigidbody rb;
    private GameObject playerHand;
    private GameObject interactables;
    public void Start()
    {
        //playerHandScript = GameObject.Find("Player").GetComponent<PlayerHand>();
        playerHand = GameObject.Find("PlayerHand");
        interactables = GameObject.Find("Interactables");
        rb = GetComponent<Rigidbody>();
    }
    
    protected override void Interact()
    {
        gameObject.transform.SetParent(playerHand.transform);
        rb.isKinematic = true;
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    public void Throw(UnityEngine.Vector3 dir, float force)
    {
        gameObject.transform.SetParent(interactables.transform);
        rb.isKinematic = false;
        rb.AddForce(dir * force, ForceMode.Impulse);
        gameObject.layer = LayerMask.NameToLayer("Interactable");
    }
}
