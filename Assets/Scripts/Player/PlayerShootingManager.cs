using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerShootingManager : MonoBehaviour
{
    /// <summary>
    /// Event triggered when player shoot weapon
    /// </summary>
    public static event PlayerShoot OnPlayerShoot;
    public delegate void PlayerShoot();
    
    [Header("layer where are all Damagable objects")]
    [SerializeField] private LayerMask shootableLayer;

    #region private local variables
    
    private PlayerMain _playerMain;
    private bool _canRapidFire;     //for rapid fire in update
    private bool _lockOnStart;  //prevent from shoot in start lag in editor
    private bool _shooting;     //for rapid fire in update
    
    #endregion
   
   
    private void Start()
    {
        StartCoroutine(LockShootingOnStartIe());
        _playerMain = GetComponent<PlayerMain>();

        _playerMain.GetPlayerControls().WeaponUse.Shoot.performed += i => Shoot(true);
        _playerMain.GetPlayerControls().WeaponUse.Shoot.canceled += i => Shoot(false);
        _playerMain.GetPlayerControls().WeaponUse.Reload.performed += i => Reload();
    }

    IEnumerator LockShootingOnStartIe()
    {
        _lockOnStart = true;
        yield return new WaitForSeconds(1f);
        _lockOnStart = false;
    }
    

    private void Shoot(bool isTriggered)
    {
        if (_lockOnStart) return;
        
        _shooting = isTriggered;
        
        if(_shooting)
        {
            _canRapidFire = _playerMain.CanRapidFire();
            
            if (!_canRapidFire && _playerMain.CanShoot())
            {
                _playerMain.ShootWeapon(_shooting);
                OnPlayerShoot?.Invoke();
            }
        }
        else
            _playerMain.ShootWeapon(false);
    }

  
    
    public LayerMask GetShootableLayer()
    {
        return shootableLayer;
    }
    
    private void Reload()
    {
        _playerMain.ReloadWeapon();
    }

    private void Update()
    {
        if(_canRapidFire && _shooting && _playerMain.CanShoot())
        {
            _playerMain.ShootWeapon(true);
            OnPlayerShoot?.Invoke();
        }
    }
}
