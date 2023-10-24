using System.Collections.Generic;

public interface IWeapon
{
    /// <summary>
    /// To trigger weapon shoot
    /// </summary>
    /// <param name="playerMain">Player main to set</param>
    /// <param name="shooting">Is actually shooting true or false? True is for weapon to shoot and trigger shoot animation, if false it can cancel some weapons shooting animations </param>
    public void Shoot(PlayerMain playerMain, bool shooting);

    /// <summary>
    /// to trigger weapon reload
    /// </summary>
    public void Reload();

    /// <summary>
    /// Return if weapon can do rapid fire
    /// </summary>
    /// <returns></returns>
    public bool CanRapidFire();

    /// <summary>
    /// Return if weapon can actually shoot
    /// </summary>
    /// <returns>if weapon can actually shoot</returns>
    public bool CanShoot();

    /// <summary>
    /// Actual weapon ammo in magazine
    /// </summary>
    /// <returns>Weapon actual ammo in magazine</returns>
    public int ActualAmmo();

    /// <summary>
    /// Weapon magazine capacity
    /// </summary>
    /// <returns>Returns weapon magazine capacity</returns>
    public int MagazineCapacity();

    /// <summary>
    /// Weapon reload time
    /// </summary>
    /// <returns>Returns the time after which the weapon is reloaded</returns>
    public float ReloadTime();

    /// <summary>
    /// Have that weapon got infinity ammo?
    /// </summary>
    /// <returns>Returns if weapon have infinity ammo</returns>
    public bool InfinityAmmo();


    public delegate void WeaponReloaded();

    /// <summary>
    /// Event triggered after reloading the weapon
    /// </summary>
    public event WeaponReloaded OnWeaponReloaded;

    /// <summary>
    /// Weapon damage value
    /// </summary>
    /// <returns>How much damage the weapon does</returns>
    public float DamageValue();

    /// <summary>
    /// Weapon type
    /// </summary>
    /// <returns></returns>
    public WeaponType WeaponType();

    /// <summary>
    /// Weapon destroyable materials
    /// </summary>
    /// <returns>Which materials weapon can destroy</returns>
    public List<MaterialSO> DestroyableMaterials();

    /// <summary>
    /// Is weapon reloading?
    /// </summary>
    /// <returns></returns>
    public bool IsReloading();
}