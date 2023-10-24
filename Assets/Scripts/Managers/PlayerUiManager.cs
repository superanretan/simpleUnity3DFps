using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUiManager : MonoBehaviour
{
    [SerializeField] private Image ammoImage;
    [SerializeField] private TextMeshProUGUI ammoCounterText;
    [SerializeField] private GameObject reloadWeaponWarningText;

    private IWeapon actualWeapon;

    private void Awake()
    {
        PlayerWeaponsManager.OnPlayerChangeWeapon += OnPlayerChangeWeapon;
        PlayerShootingManager.OnPlayerShoot += OnPlayerShoot;
    }


    private void OnDisable()
    {
        PlayerWeaponsManager.OnPlayerChangeWeapon -= OnPlayerChangeWeapon;
        PlayerShootingManager.OnPlayerShoot -= OnPlayerShoot;
    }

    private void OnWeaponReloaded()
    {
        ActualizeDisplays();
    }

    private void OnPlayerShoot()
    {
        ActualizeDisplays();
    }

    private void OnPlayerChangeWeapon(IWeapon weapon)
    {
        if (actualWeapon != null)
            actualWeapon.OnWeaponReloaded -= OnWeaponReloaded;

        actualWeapon = weapon;
        actualWeapon.OnWeaponReloaded += OnWeaponReloaded;
      ActualizeDisplays();
    }


    private void ActualizeDisplays()
    {
        ChangeDisplayColors();
        ChangeWeaponText();
        ShowWarningText();
    }
    
    private void ChangeWeaponText()
    {
        ammoCounterText.text = actualWeapon.InfinityAmmo() ? "inf" : $"{actualWeapon.ActualAmmo()}/{actualWeapon.MagazineCapacity()}";
    }


    private void ShowWarningText()
    {
        if (actualWeapon.ActualAmmo() == 0 && !actualWeapon.InfinityAmmo())
            reloadWeaponWarningText.SetActive(true);
        if(actualWeapon.ActualAmmo() > 0 || actualWeapon.InfinityAmmo())
            reloadWeaponWarningText.SetActive(false);
    }

    private void ChangeDisplayColors()
    {
        Color newColor = (actualWeapon.ActualAmmo() == 0 && !actualWeapon.InfinityAmmo()) ? Color.red : Color.white;

        ammoCounterText.color = newColor;
        ammoImage.color = newColor;
    }
}