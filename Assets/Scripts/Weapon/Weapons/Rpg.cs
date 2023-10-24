using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Rpg : WeaponBase
{
    #region Variables accesable from inspector

    [Header("rpg animation values")]
    [SerializeField] private float shootZDumpMove = 0.2f;
    
    [Header("shoot animation time")]
    [SerializeField] private float shootDumpAnimationTime = 0.2f;
    
    [Header("rpg rocket prefab")]
    [SerializeField] private RpgRocket rocketPrefab;

    [Header("rpg reload positions")]
    [SerializeField] private Transform rpgRocketStartPosition;
    [SerializeField] private Transform rpgRocketReloadInFrontOfPosition;
    [SerializeField] private Transform rpgRocketReloadUnderPosition;

    #endregion
    
    #region Private local variables

    private Vector3 _rpgStartLocalPosition;
    private Sequence _reloadSequence;
    private Sequence _shootSequence;
    private RpgRocket _actualRocket;

    #endregion


    public void Start()
    {
        _rpgStartLocalPosition = transform.localPosition;
        _actualRocket = GetComponentInChildren<RpgRocket>();
    }

    public override void Shoot(PlayerMain playerMain, bool shooting = true)
    {
        base.Shoot(playerMain);
        if (shooting)
            RaycastShootRocketTargetSet(playerMain);
    }

    private void RaycastShootRocketTargetSet(PlayerMain playerMain)
    {
        var camTransform = playerMain.GetPlayerCamera().transform;
        if (Physics.Raycast(camTransform.position, camTransform.forward, out var hit,
                Mathf.Infinity, layerMask: playerMain.GetShootableLayer()))
        {
            _actualRocket.SetRocketTargetPosition(hit.point);       //setting rocket target to fly to
            _actualRocket.ShootRocket(this);                   //start rocket shoot
            _actualRocket.transform.SetParent(playerMain.transform.parent);     //change rocket parent out from weapon
            RpgShootAnimation();                                    //trigger rpg recoil animation
        }
    }

    private bool CheckIfTarget() //prevents  from shooting into nothingness because rocket must have target to fly to
    {
        Camera playerCam = transform.GetComponentInParent<PlayerMain>().GetPlayerCamera();

        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out var hit,
                Mathf.Infinity, layerMask: transform.GetComponentInParent<PlayerMain>().GetShootableLayer()))
            return true;
        else
            return false;
    }

    public override bool CanShoot()
    {
        return base.CanShoot() && CheckIfTarget();
    }

    private void RpgShootAnimation()
    {
        _shootSequence = DOTween.Sequence();

        var afterShootPosition =
            new Vector3(_rpgStartLocalPosition.x, _rpgStartLocalPosition.y, _rpgStartLocalPosition.z - shootZDumpMove);

        _shootSequence.Append(transform.DOLocalMove(afterShootPosition, shootDumpAnimationTime / 2))
            .Append(transform.DOLocalMove(_rpgStartLocalPosition, shootDumpAnimationTime / 2));
    }

    public override void Reload()
    {
        base.Reload();
        _actualRocket = Instantiate(rocketPrefab, this.transform);
        _actualRocket.transform.localPosition = rpgRocketReloadUnderPosition.localPosition;

        ReloadAnimation();
    }

    private void ReloadAnimation()
    {
        _reloadSequence = DOTween.Sequence();
        _reloadSequence
            .Append(_actualRocket.transform
                .DOLocalMove(rpgRocketReloadInFrontOfPosition.localPosition, ReloadTime() / 2)
                .SetEase(Ease.Linear))
            .Append(_actualRocket.transform.DOLocalMove(rpgRocketStartPosition.localPosition, ReloadTime() / 2)
                .SetEase(Ease.Linear));
    }
}