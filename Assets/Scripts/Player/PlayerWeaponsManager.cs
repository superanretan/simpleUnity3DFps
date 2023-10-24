using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponsManager : MonoBehaviour
{
    public delegate void PlayerChangeWeapon(IWeapon weapon);
    public static event PlayerChangeWeapon OnPlayerChangeWeapon;
    
    #region Variables accesable from inspector
    
    [Header("List of avaliable weapons")]
    [SerializeField] private List<WeaponBase> weaponsList = new List<WeaponBase>();

    [Header("weapons holder to animate weapon when player is moving")]
    [SerializeField] private Transform weaponsHolder;

    [Header("weapon y swing offset")]
    [SerializeField] private float weaponAnimationYOffset = 0.1f;
    
    #endregion
    
    #region private local variables

    private Sequence _weaponSwingSequence;
    private Vector3 _weaponsHolderStartLocalPosition;
    private PlayerMain _playerMain;
    private IWeapon _actualWeapon;
    private float _scrollY;
    private int _actualWeaponIndex;

    #endregion
  

    private void Start()
    {
        _playerMain = GetComponent<PlayerMain>();
        _actualWeaponIndex = 0;
        _weaponsHolderStartLocalPosition = weaponsHolder.localPosition;
        PlayerMovement.OnPlayerIsMoving += WeaponAnimationOnPlayerMove;
        SwitchWeapon(_actualWeaponIndex);
    }

    private void Update()
    {
        ChangeWeaponByMouseScrollListener();
    }

    private void WeaponAnimationOnPlayerMove(float playerSpeed) //animate weapon up and down to make better feeling of player movement
    {
        switch (playerSpeed)
        {
            case > 0 when _weaponSwingSequence == null:
                _weaponSwingSequence = DOTween.Sequence();
                _weaponSwingSequence
                    .Append(weaponsHolder.DOLocalMoveY(_weaponsHolderStartLocalPosition.y - weaponAnimationYOffset, 0.3f))
                    .Append(weaponsHolder.DOLocalMove(_weaponsHolderStartLocalPosition, 0.3f))
                    .SetLoops(-1);
                break;
            case 0:
            {
                if (_weaponSwingSequence != null)
                {
             
                    _weaponSwingSequence.Kill();
                    _weaponSwingSequence = null;
                    weaponsHolder.DOLocalMove(_weaponsHolderStartLocalPosition, 0.2f);
                }

                break;
            }
        }
    }

    private void ChangeWeaponByMouseScrollListener() //using mouse scroll to change player weapon
    {
        if (_actualWeapon.IsReloading()) return;
        
        Vector2 vec = _playerMain.GetPlayerControls().WeaponUse.MouseScroll.ReadValue<Vector2>();
        vec.Normalize();
        var scrollYValue = vec.y;

        if (scrollYValue > 0)
        {
            if (_actualWeaponIndex < weaponsList.Count - 1)
                _actualWeaponIndex++;
            else
                _actualWeaponIndex = 0;
            SwitchWeapon(_actualWeaponIndex);
        }
        else if (scrollYValue < 0)
        {
            if (_actualWeaponIndex > 0)
                _actualWeaponIndex--;
            else
                _actualWeaponIndex = weaponsList.Count - 1;
            SwitchWeapon(_actualWeaponIndex);
        }
    }

    public bool CanRapidFire()
    {
        return _actualWeapon.CanRapidFire();
    }

    public IWeapon GetActualWeapon()
    {
        return _actualWeapon;
    }

    public bool CanShoot()
    {
        return _actualWeapon.CanShoot();
    }

    public void ShootWeapon(PlayerMain playerMain, bool shooting = true)
    {
        _actualWeapon.Shoot(playerMain, shooting);
    }

    public void ReloadWeapon()
    {
        _actualWeapon.Reload();
    }

    private void SwitchWeapon(int weaponToSwitchIndex) //switching weapon
    {
        _actualWeapon = weaponsList[weaponToSwitchIndex];

        foreach (var weapon in weaponsList)
        {
            if (weaponsList.IndexOf(weapon) == weaponToSwitchIndex)
                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);
        }

        OnPlayerChangeWeapon?.Invoke(_actualWeapon);
    }
}