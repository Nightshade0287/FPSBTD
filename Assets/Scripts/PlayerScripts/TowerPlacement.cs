using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class TowerPlacement : MonoBehaviour
{
    [Header("Variables")]
    public int MaxPlaceDistance;
    [Header("References")]
    public Transform cam;
    public GameObject[] towerPrefabs;
    public Transform BloonHolder;
    public Material placeMaterial;
    public Material unPlaceMaterial;
    protected Vector3 placePos;
    private bool Placing = false;
    private bool canPlace = true;
    protected GameObject newTower;
    private int selectedTowerIndex = 0;
    private Material[] originalMaterials;
    private PlayerUI playerUI;
    private TowerInfo towerInfo;
    // Start is called before the first frame update
    void Start()
    {
        playerUI = GetComponent<PlayerUI>();
    }

    public void PlaceMode(InputAction.CallbackContext ctx)
    {
        if(ctx.performed)
        {
            if (!Placing)
            {
                ChangeSelectedTower(0);
                Placing = true;
                return;
            }
            if (Placing)
            {
                Destroy(newTower);
                Placing = false;
                playerUI.UpdateText(string.Empty);
            }
        }
    }

    public void PlaceTower(InputAction.CallbackContext ctx)
    {
        if(ctx.performed)
        {
            if (Placing && canPlace)
                {
                    if(playerUI.cash - towerInfo.cost > 0)
                    {
                        playerUI.UpdateMoney(-towerInfo.cost);
                        Placing = false;
                        newTower.GetComponent<BaseTower>().BloonHolder = BloonHolder;
                        newTower.GetComponent<BaseTower>().enabled = true;
                        ResetMaterials();
                        newTower.gameObject.layer = LayerMask.NameToLayer("Tower");
                        playerUI.UpdateText(string.Empty);
                        newTower = null;
                        return;
                    }
                }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Placing)
        {
            CalculatePlacePos();
            newTower.transform.position = placePos;
            newTower.transform.rotation = gameObject.transform.rotation;
        }
    }
    public void GetScroll(InputAction.CallbackContext ctx)
    {
        float scroll = ctx.ReadValue<float>();
        if(ctx.performed && Placing)
        {
            if(scroll > 0)
                ChangeSelectedTower(1);
            else if(scroll < 0)
                ChangeSelectedTower(-1);
        }
    }
    public void ChangeSelectedTower(int direction)
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
        towerInfo = newTower.GetComponent<TowerInfo>();
        playerUI.UpdateText(newTower.GetComponent<TowerInfo>().towerName + " $" + towerInfo.cost);
        newTower.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        StoreMaterials();
    }


    public void CalculatePlacePos()
    {
        RaycastHit hit;
        Ray ray = new Ray(cam.position, cam.forward);
        if (Physics.Raycast(cam.position, cam.forward, out hit, MaxPlaceDistance))
        {
            placePos = hit.point;
            if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                canPlace = true;
                ChangeToMaterial(placeMaterial);
            }
            else
            {
                canPlace = false;
                ChangeToMaterial(unPlaceMaterial);
            }
        }
    }
    void StoreMaterials()
    {
        // Get all Renderer components attached to this GameObject and its children
        Renderer[] renderers = newTower.transform.Find("Model").GetComponentsInChildren<Renderer>();

        // Initialize the array to store original materials
        originalMaterials = new Material[renderers.Length];

        // Store original materials for each renderer
        for (int i = 0; i < renderers.Length; i++)
        {
            originalMaterials[i] = renderers[i].material;
        }
    }

    void ChangeToMaterial(Material material)
    {
        // Get all Renderer components attached to this GameObject and its children
        Renderer[] renderers = newTower.transform.Find("Model").GetComponentsInChildren<Renderer>();

        // Change materials for each renderer
        foreach (Renderer renderer in renderers)
        {
            renderer.material = material;
        }
    }

    public void ResetMaterials()
    {
        // Get all Renderer components attached to this GameObject and its children
        Renderer[] renderers = newTower.transform.Find("Model").GetComponentsInChildren<Renderer>();

        // Restore original materials for each renderer
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material = originalMaterials[i];
        }
    }
}
