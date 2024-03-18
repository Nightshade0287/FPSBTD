using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Optic : MonoBehaviour
{
    private GameObject currentGun;
    private GameObject optic;
    private GameObject scope;

    public GameObject cursor;

    public LayerMask opticLayer;

    private void Start()
    {
        
    }

    void Update()
    {
        if(GameObject.FindGameObjectWithTag("Player").transform.Find("PlayerCam").Find("GunHolder").childCount != 0)
        {
            currentGun = GameObject.FindGameObjectWithTag("Gun");
            optic = currentGun.transform.Find("Optic").gameObject;
            scope = currentGun.transform.Find("Scope").gameObject;
            if(CanSeeOptic())
            {
                optic.SetActive(true);
                cursor.SetActive(false);
            }
            else
            {
                optic.SetActive(false);
                cursor.SetActive(true);
            }
        }
    }

    public bool CanSeeOptic()
    {
        Vector3 targetDirection = optic.transform.position - transform.position;
        Ray ray = new Ray(transform.position, targetDirection);
        RaycastHit hitInfo = new RaycastHit();
        if(Physics.Raycast(ray, out hitInfo, Mathf.Infinity, opticLayer))
        {
            if(hitInfo.collider.transform == scope.transform)
            {
                return true;
            }
        }   
        return false;
    }
}

