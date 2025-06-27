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
    public InputActionAsset inputActions;
    protected Vector3 placePos;
    public bool placingTower = false;
    public bool placingMode = false;
    private bool canPlace = true;
    protected GameObject newTower;
    private Material[] originalMaterials;
    private Economy_Health economy;
    private Transform towerMenu;
    private TowerInfo towerInfo;
    private InputActionMap baseGameplay;
    // Start is called before the first frame update
    void Start()
    {
        economy = GameObject.Find("Economy/Health").GetComponent<Economy_Health>();
        towerMenu = GameObject.Find("TowerUI").transform.Find("TowerMenu");
        baseGameplay = inputActions.FindActionMap("BaseGameplay");
        inputActions.FindActionMap("PlayerUI").Enable();
    }
    // Update is called once per frame
    void Update()
    {
        if (placingTower)
        {
            CalculatePlacePos();
            newTower.transform.position = placePos;
            newTower.transform.rotation = gameObject.transform.rotation;
        }
    }
    public void PlaceMode(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (!placingMode)
            {
                towerMenu.gameObject.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                baseGameplay.Disable();
                placingMode = true;
                return;
            }

            else if (placingMode && !placingTower)
            {
                towerMenu.gameObject.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                placingTower = false;
                placingMode = false;
                baseGameplay.Enable();
            }

            else if (placingMode && placingTower)
            {
                Debug.Log("ran");
                Destroy(newTower);
                towerMenu.gameObject.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                placingTower = false;
                baseGameplay.Disable();
                return;
            }
        }
    }

    public void PlaceTower(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (placingTower && canPlace)
            {

                economy.UpdateMoney(-towerInfo.cost);
                placingTower = false;
                placingMode = false;
                newTower.GetComponent<BaseTower>().BloonHolder = BloonHolder;
                newTower.GetComponent<BaseTower>().enabled = true;
                newTower.transform.Find("RangeIndicator").gameObject.SetActive(false);
                ResetMaterials();
                newTower.gameObject.layer = LayerMask.NameToLayer("Tower");
                newTower = null;
                return;
            }
        }
    }

    public void SelectTower(GameObject tower)
    {
        if (economy.cash - tower.GetComponent<TowerInfo>().cost >= 0)
        {
            Destroy(newTower);
            baseGameplay.Enable();
            towerMenu.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            placingTower = true;
            newTower = Instantiate(tower, placePos, gameObject.transform.rotation);
            newTower.transform.Find("RangeIndicator").gameObject.SetActive(true);
            towerInfo = newTower.GetComponent<TowerInfo>();
            newTower.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            StoreMaterials();
        }
    }
    public void CalculatePlacePos()
    {
        RaycastHit hit;
        Ray ray = new Ray(cam.position, cam.forward);
        if (Physics.Raycast(cam.position, cam.forward, out hit, MaxPlaceDistance))
        {
            placePos = hit.point;
            if (hit.collider.gameObject.tag == "Placeable")
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
