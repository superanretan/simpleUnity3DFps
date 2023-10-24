using DG.Tweening;
using UnityEngine;

public class Ak47 : WeaponBase
{
    #region  Public variables accesable from inspector

    [Header("Ak animation values")]
    [SerializeField] private float shootZDumpMove = 0.2f;
    [SerializeField] private float shootXrotationAngle = 15f;
    [SerializeField] private float shootDumpAnimationTime = 0.2f;
    [SerializeField] private ParticleSystem shootPs;

    [Header("Bullet hole decal prefab")]
    [SerializeField] private GameObject decalPrefab;
    
    [Header("reload magazine values")]
    [SerializeField] private Transform magazine;
    [SerializeField] private float magazineYReloadEndPosOffset = 0.300f; //position offset where magazine is going when reloading

    #endregion
   

    #region Private local variables

    private Vector3 _akStartLocalPosition;
    private Vector3 _akStartLocalRotation;
    private Vector3 _magazineStartLocalPosition;
    private Sequence _shootAnimationSequence;
    private Sequence _magazineReloadSequence;

    #endregion
   
    public void Start()
    {
        _akStartLocalPosition = transform.localPosition;
        _akStartLocalRotation = transform.localRotation.eulerAngles;
        _magazineStartLocalPosition = magazine.transform.localPosition;
    }

    public override void Shoot(PlayerMain playerMain, bool shooting = true)
    {
        base.Shoot(playerMain);
        if(shooting)
        {
            RaycastShootBulletHole(playerMain);
            AkShootAnimation();
            shootPs.Play();
        }
    }
    
    private void RaycastShootBulletHole(PlayerMain playerMain)              //spawning Decal with hole sprite on raycast point
    {
        var camTransform = playerMain.GetPlayerCamera().transform;
        if (Physics.Raycast(camTransform.position, camTransform.forward, out var hit,
                Mathf.Infinity ,layerMask: playerMain.GetShootableLayer()))
        {
            var decal = Instantiate(decalPrefab, hit.point, Quaternion.identity, hit.transform);
            decal.transform.rotation = Quaternion.LookRotation(-hit.normal);
            Destroy(decal.gameObject,2f);
        }
    }

    private void AkShootAnimation()             //weapon shoot recoil animation
    {
        _shootAnimationSequence = DOTween.Sequence();
        var shootLocalPos = new Vector3(_akStartLocalPosition.x, _akStartLocalPosition.y,
            _akStartLocalPosition.z - shootZDumpMove);

        var shootLocalRotation = new Vector3(_akStartLocalRotation.x + shootXrotationAngle, _akStartLocalRotation.y,
            _akStartLocalRotation.z);
        
        _shootAnimationSequence
            .Append(transform.DOLocalMove(shootLocalPos, shootDumpAnimationTime/2).SetEase(Ease.Linear))
            .Join(transform.DOLocalRotate(shootLocalRotation, shootDumpAnimationTime/2)
                .SetEase(Ease.Linear))
            .Append(transform.DOLocalMove(_akStartLocalPosition, shootDumpAnimationTime/2).SetEase(Ease.Linear))
            .Join(transform.DOLocalRotate(_akStartLocalRotation,shootDumpAnimationTime/2).SetEase(Ease.Linear));
    }

    public override void Reload()
    {
        base.Reload();
        MagazineReloadAnimation();
    }

    private void MagazineReloadAnimation()
    {
        _magazineReloadSequence = DOTween.Sequence();

        var magazineReloadEndPosition = new Vector3(_magazineStartLocalPosition.x,
            _magazineStartLocalPosition.y - magazineYReloadEndPosOffset, _magazineStartLocalPosition.z);
        _magazineReloadSequence
            .Append(magazine.transform.DOLocalMove(magazineReloadEndPosition, ReloadTime() / 2).SetEase(Ease.Linear))
            .Append(magazine.transform.DOLocalMove(_magazineStartLocalPosition, ReloadTime() / 2).SetEase(Ease.Linear));
    }
}