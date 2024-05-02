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
        towerUpgrades = GameObject.Find("TowerUI").transform.Find("Upgrades");
        baseGameplay = inputActions.FindActionMap("BaseGameplay");
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hitInfo;
        if(Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            tower = hitInfo.collider.gameObject;
            towerInfo = tower.GetComponent<TowerInfo>();
            if(tower != null)
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
        if(ctx.performed)
        {
            if(tower != null)
            {
                if(!upgrading)
                {
                    towerUpgrades.gameObject.SetActive(true);
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    baseGameplay.Disable();
                    upgrading = true;
                    tower.GetComponent<TowerInfo>().UpdateTowerUpgradeUI();
                }

                else if(upgrading)
                {
                    towerUpgrades.gameObject.SetActive(false);
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    baseGameplay.Enable();
                    upgrading = false;
                }
            }
        }
    }

    public void UpgradeTower(int path)
    {
        if(upgrading)
        {
            towerInfo.UpgradeTower(path);
        }
    }

    public void CycleTargetingMode(int direction)
    {
        if(upgrading)
        {
            towerInfo.CycleTargetingMode(direction);
        }
    }

    public void Sell()
    {
        if(upgrading)
        {
            towerUpgrades.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            baseGameplay.Enable();
            upgrading = false;
            towerInfo.Sell();
        }
    }
}
