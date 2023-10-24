using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;

public class PlayerMain : MonoBehaviour
{
    #region References

    private InputManager inputManager;
    private PlayerShootingManager playerShootingManager;
    private PlayerWeaponsManager playerWeaponsManager;
    
    #endregion

    #region Variables accesable from inspector

    [SerializeField] private CinemachineVirtualCamera cmVirtual;
    [SerializeField] private Camera playerCamera;

    #endregion

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerShootingManager = GetComponent<PlayerShootingManager>();
        playerWeaponsManager = GetComponent<PlayerWeaponsManager>();
    }

    void Start()
    {
        cmVirtual.ForceCameraPosition(cmVirtual.transform.position,quaternion.Euler(0,0,0));    //setting cm camera on correct start rotation
    }


    #region WeaponManagment

    public LayerMask GetShootableLayer()
    {
        return playerShootingManager.GetShootableLayer();
    }

    public IWeapon GetActualWeapon()
    {
        return playerWeaponsManager.GetActualWeapon();
    }

    public bool CanRapidFire()
    {
        return playerWeaponsManager.CanRapidFire();
    }

    public bool CanShoot()
    {
        return playerWeaponsManager.CanShoot();
    }

    public void ShootWeapon(bool shooting = true)
    {
        playerWeaponsManager.ShootWeapon(this, shooting);
    }

    public void ReloadWeapon()
    {
        playerWeaponsManager.ReloadWeapon();
    }

    #endregion

    public Camera GetPlayerCamera()
    {
        return playerCamera;
    }

    public PlayerControls GetPlayerControls()
    {
        return inputManager.GetPlayerControls();
    }

    public Vector2 GetPlayerMovementVector()
    {
        return inputManager.GetPlayerMovementVector();
    }

    public Vector2 GetMouseDelta()
    {
        return inputManager.GetMouseDelta();
    }
}