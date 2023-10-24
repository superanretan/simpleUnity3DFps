using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class RpgRocket : MonoBehaviour
{
    private bool _canShoot = false;
    private Vector3 _targetPosition;
    [SerializeField] private float rocketSpeed = 5f;
    private Rigidbody _rigidbody;

    [SerializeField] private ParticleSystem rocketParticleSystem;
    [SerializeField] private GameObject rpgExplosionPrefab;

    private IWeapon _weapon;
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        
        if(rocketParticleSystem ==null || rpgExplosionPrefab == null)
            Debug.LogError($"This {transform.name}  has parameters not set!");
    }

    public void SetRocketTargetPosition(Vector3 targetPosition)
    {
        this._targetPosition = targetPosition;
    }

    public void ShootRocket(IWeapon weapon)
    {
        _weapon = weapon;
        _canShoot = true;
        rocketParticleSystem.Play();
        _rigidbody.isKinematic = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (_canShoot)
        {
            var direction = _targetPosition - transform.position;
            direction.Normalize();
            var velocity = direction * rocketSpeed;
            _rigidbody.velocity = velocity;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player")) return;

        if (collision.transform.GetComponent<IObstacle>() != null)
            collision.transform.GetComponent<IObstacle>().Damage(_weapon);
        
        Instantiate(rpgExplosionPrefab, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
