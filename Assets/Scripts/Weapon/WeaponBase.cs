using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour, IWeapon
{
    public event IWeapon.WeaponReloaded OnWeaponReloaded;
    
    #region Serializable variables accesable from inspector

    [Header("Materials which weapon can destroy")]
    [SerializeField] private List<MaterialSO> destroyableMaterialsList = new List<MaterialSO>();

    [Header("Weapon magazine capacity")]
    [SerializeField] private int magazineCapacity;
    
    [Header("If weapon have infinity ammo")]
    [SerializeField] private bool infinityAmmo;
    
    [Header("Weapon reload time")]
    [SerializeField] private float reloadTime = 2f;
    
    [Header("Damage that the weapon deals to the obstacle ")]
    [SerializeField] private float damageValue;
    
    [Header("If weapon can rapid fire")]
    [SerializeField] private bool canRapidFire = false;
    
    [Header("Delay for rapid fire")]
    [SerializeField] private float rapidShootDelayTime;
    
    [Header("Weapon type")]
    [SerializeField] private WeaponType weaponType;

    #endregion
    
    #region Private local variables
    
    private int _actualAmmo;
    private bool _isShootDelayed = false;
    private bool _isReloading = false;
    
    #endregion
   

    private void Awake()
    {
        if(!infinityAmmo && magazineCapacity == 0)
            Debug.LogError($"This {transform.name} weapon has wrong ammo settings! Should be infinityAmmo or magazineCapacity>0");
            
        _actualAmmo = magazineCapacity;
    }
  
    
    public virtual void Shoot(PlayerMain playerMain, bool shooting = true) //called when player shoot
    {
        if (_actualAmmo > 0 || InfinityAmmo())
        {
            if (!InfinityAmmo())
                _actualAmmo--;

            if (WeaponType() == global::WeaponType.Ammo && shooting)
                RaycastShoot(playerMain);                              
            
            if (canRapidFire && rapidShootDelayTime > 0)
                StartCoroutine(ShootDelayIe());     //coroutine to implement shooting delay
            else
                _isShootDelayed = false;                
        }
    }

    private void RaycastShoot(PlayerMain playerMain)             //shoot on raycast if weapon type is ammo (no special shoot action or animation for that weapon)
    {
        var camTransform = playerMain.GetPlayerCamera().transform;
        if (Physics.Raycast(camTransform.position, camTransform.forward, out var hit,
                Mathf.Infinity, layerMask: playerMain.GetShootableLayer()))
        {
            if (hit.collider.GetComponent<IObstacle>() != null)
            {
                hit.collider.GetComponent<IObstacle>().Damage(playerMain.GetActualWeapon());
            }
        }
    }

    IEnumerator ShootDelayIe()
    {
        _isShootDelayed = true;
        yield return new WaitForSeconds(rapidShootDelayTime);
        _isShootDelayed = false;
    }

    public bool CanRapidFire()
    {
        return canRapidFire;
    }

    public virtual bool CanShoot()   //return if weapon can shoot
    {
        bool canShoot = !canRapidFire || !_isShootDelayed;
        return canShoot && (_actualAmmo > 0 || infinityAmmo) && !_isReloading;
    }
    
    public WeaponType WeaponType()
    {
        return weaponType;
    }

    public float DamageValue()
    {
        return damageValue;
    }

    public int ActualAmmo()
    {
        return _actualAmmo;
    }

    public int MagazineCapacity()
    {
        return magazineCapacity;
    }

    public float ReloadTime()
    {
        return reloadTime;
    }

    public bool IsReloading()
    {
        return _isReloading;
    }
    
    public bool InfinityAmmo()
    {
        return infinityAmmo;
    }


    public virtual void Reload()
    {
        if (_actualAmmo < magazineCapacity)
            StartCoroutine(ReloadingDelayIe());   //coroutine for lock shooting when reloading and start reloading animations on any weapon type
    }

    IEnumerator ReloadingDelayIe()
    {
        _isReloading = true;
        yield return new WaitForSeconds(ReloadTime());
        _actualAmmo = magazineCapacity;
        _isReloading = false;
        OnWeaponReloaded?.Invoke();    //event invoked when weapon is after reload (for ui manager to set correct ammo info)
    }

    public List<MaterialSO> DestroyableMaterials()
    {
        return destroyableMaterialsList;
    }
}