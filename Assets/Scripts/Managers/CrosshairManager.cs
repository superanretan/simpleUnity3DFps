using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairManager : MonoBehaviour
{
    [SerializeField] private RectTransform crosshairImageTransform;
    private Vector2 _startSizeDelta;
    private bool _shooting;
    private void Start()
    {
        PlayerShootingManager.OnPlayerShoot += OnPlayerShoot;
        PlayerMovement.OnPlayerIsMoving += OnPlayerMove;
        _startSizeDelta = crosshairImageTransform.sizeDelta;
    }

    private void OnDisable()
    {
        PlayerShootingManager.OnPlayerShoot -= OnPlayerShoot;
        PlayerMovement.OnPlayerIsMoving -= OnPlayerMove;
    }

    private void OnPlayerMove(float speed)
    {
        if (speed > 0)
        {
            crosshairImageTransform
                .DOSizeDelta(_startSizeDelta * 2.2f, 0.1f);
        }
        else
        {
            if (!_shooting)
                crosshairImageTransform.DOSizeDelta(_startSizeDelta, 0.13f);
        }
    }


    private void OnPlayerShoot()
    {
        _shooting = true;
        crosshairImageTransform
            .DOSizeDelta(_startSizeDelta * 1.5f, 0.2f).OnComplete(() =>
            {
                _shooting = false;
            });
    }
}