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
    public GameObject[] towerPrefabs;
    public Transform BloonHolder;

    protected Vector3 placePos;
    protected bool Placing = false;
    protected GameObject newTower;
    private int selectedTowerIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(placeKey) && !Placing)
        {
            selectedTowerIndex = 0;
            newTower = Instantiate(towerPrefabs[selectedTowerIndex], placePos, gameObject.transform.rotation);
            Placing = true;
        }

        if (Placing)
        {
            CalculatePlacePos();
            StartCoroutine(SelectTower());
            newTower.transform.position = placePos;
            newTower.transform.rotation = gameObject.transform.rotation;
        }

        if (Input.GetMouseButton(0) && Placing)
        {
            Placing = false;
            newTower.GetComponent<CapsuleCollider>().enabled = true;  
            newTower.GetComponent<BaseTower>().BloonHolder = BloonHolder;
            newTower.GetComponent<BaseTower>().enabled = true;
        }
    }

    IEnumerator SelectTower()
    {
        // Handle tower selection with scroll input
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f)
        {
            ChangeSelectedTower(1); // Scroll up
        }
        else if (scroll < 0f)
        {
            ChangeSelectedTower(-1); // Scroll down
        }

        yield return null;
    }

    private void ChangeSelectedTower(int direction)
    {
        selectedTowerIndex += direction;

        // Wrap around to the beginning if at the end of the array
        if (selectedTowerIndex < 0)
        {
            selectedTowerIndex = towerPrefabs.Length - 1;
        }
        // Wrap around to the end if at the beginning of the array
        else if (selectedTowerIndex >= towerPrefabs.Length)
        {
            selectedTowerIndex = 0;
        }

        Destroy(newTower);
        newTower = Instantiate(towerPrefabs[selectedTowerIndex], placePos, gameObject.transform.rotation);
    }


    public void CalculatePlacePos()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, MaxPlaceDistance, Ground))
        {
            placePos = hit.point; //new Vector3 (hit.point.x ,hit.point.y + (newTower.GetComponent<CapsuleCollider>().height / 2), hit.point.y);
        }
    }
}
