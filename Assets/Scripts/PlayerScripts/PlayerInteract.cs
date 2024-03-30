using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private Camera cam;
    [SerializeField]
    private float distance = 3f;
    [SerializeField]
    private LayerMask mask;
    private PlayerUI playerUI;
    private Interactable interactable;
    // Start is called before the first frame update
    void Awake()
    {
        cam = GetComponent<PlayerLook>().cam;
        playerUI = GetComponent<PlayerUI>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hitInfo;
        if(Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            interactable = hitInfo.collider.GetComponent<Interactable>();
            if(hitInfo.collider.GetComponent<Interactable>() != null)
            {
                if(hitInfo.collider.GetComponent<Gun>() == null || (hitInfo.collider.GetComponent<Gun>() != null && GameObject.Find("GunHolder").transform.childCount == 0))
                {
                    playerUI.UpdateText(interactable.promptMessage);
                }
                else
                    playerUI.UpdateText("Hands Full");
            }
        }
        // else
        //     playerUI.UpdateText(string.Empty);
    }
    public void Interact()
    {
        if(interactable != null)
        {
                interactable.BaseInteract();
        }
    }
}
