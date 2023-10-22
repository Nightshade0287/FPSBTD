using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
    [Header("Variables")]
    public KeyCode placeKey = KeyCode.E;
    public int MaxPlaceDistance;

    [Header("References")]
    public LayerMask Ground;
    public Transform cam;
    public GameObject towerPrefab;
    public Transform BloonHolder;

    private Vector3 placePos;
    private bool Placing = false;
    private GameObject newTower;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(placeKey) && !Placing)
        {
            newTower = Instantiate(towerPrefab, placePos, gameObject.transform.rotation);
            newTower.GetComponent<DartMonkey>().BloonHolder = BloonHolder;
            Placing = true;
        }

        if (Placing)
        {
            CalculatePlacePos();
            newTower.transform.position = placePos;
            newTower.transform.rotation = gameObject.transform.rotation;
        }

        if (Input.GetMouseButton(0) && Placing)
        {
            Placing = false;
            newTower.GetComponent<CapsuleCollider>().enabled = true;  
            newTower.GetComponent<DartMonkey>().enabled = true;
        }
    }

    public void CalculatePlacePos()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, MaxPlaceDistance, Ground))
        {
            placePos = hit.point;
        }
    }
}