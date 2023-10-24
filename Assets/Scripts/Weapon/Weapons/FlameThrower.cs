using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrower : WeaponBase
{
    [SerializeField] private ParticleSystem flameParticle;
    [SerializeField] private Transform rayPoint;
    [SerializeField] private float raycastLenght;

    public override void Shoot(PlayerMain playerMain, bool shooting = true)
    {
        base.Shoot(playerMain, shooting);
        
        FlameShootAnimation(shooting);
        
        if (shooting)
            RaycastShoot(playerMain);
    }

    private void RaycastShoot(PlayerMain playerMain)
    {
        var camTransform = playerMain.GetPlayerCamera().transform;
        if (Physics.Raycast(camTransform.position, camTransform.forward, out var hit,
                raycastLenght, layerMask: playerMain.GetShootableLayer()))
        {
            if (hit.collider.GetComponent<IObstacle>() != null)
                hit.collider.GetComponent<IObstacle>().Damage(this);
        }
    }

    private void FlameShootAnimation(bool shooting)
    {
        if (shooting)
            flameParticle.Play();
        else
            flameParticle.Stop();
    }
}