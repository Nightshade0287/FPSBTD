using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TowerInteract : MonoBehaviour
{
    [Header("References")]
    private Camera cam;
    [SerializeField]
    private float distance = 3f;
    [SerializeField]
    private LayerMask mask;
    public InputActionAsset inputActions;
    private PlayerUI playerUI;
    TowerPlacement towerPlacement;
    private GameObject tower;
    private TowerInfo towerInfo;
    private Transform towerUpgrades;
    private InputActionMap baseGameplay;
    private bool upgrading = false;

    // Start is called before the first frame update
    void Awake()
    {
        cam = GetComponent<PlayerLook>().cam;
        playerUI = GetComponent<PlayerUI>();
        towerPlacement = GetComponent<TowerPlacement>();
        towerUpgrades = GameObject.Find("TowerUI").transform.Find("Upgrades");
        baseGameplay = inputActions.FindActionMap("BaseGameplay");
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            tower = hitInfo.collider.gameObject;
            towerInfo = tower.GetComponent<TowerInfo>();
            if (tower != null)
            {
                playerUI.UpdateText(tower.GetComponent<TowerInfo>().towerName);
            }
        }
        else
        {
            tower = null;
            playerUI.UpdateText(string.Empty);
        }
    }
    public void Interact(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (tower != null)
            {
                if (!upgrading)
                {
                    OpenWindow();
                }

                else if (upgrading)
                {
                    CloseWindow();
                }
            }
        }
    }

    public void UpgradeTower(int path)
    {
        if (upgrading)
        {
            towerInfo.UpgradeTower(path);
        }
    }

    public void CycleTargetingMode(int direction)
    {
        if (upgrading)
        {
            towerInfo.CycleTargetingMode(direction);
        }
    }

    public void Sell()
    {
        if (upgrading)
        {
            CloseWindow();
            towerInfo.Sell();
        }
    }
    private void OpenWindow()
    {
        towerUpgrades.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        baseGameplay.Disable();
        towerPlacement.enabled = false;
        GetComponent<TowerPlacement>().enabled = false;
        upgrading = true;
        tower.GetComponent<TowerInfo>().UpdateTowerUpgradeUI();
    }
    private void CloseWindow()
    {
        towerUpgrades.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        baseGameplay.Enable();
        towerPlacement.enabled = true;
        towerInfo.enabled = true;
        upgrading = false;
    }
}
