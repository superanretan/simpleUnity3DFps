using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ObstacleBase : MonoBehaviour, IObstacle
{
    #region Variables accesible from inspector

    [Header("Material scriptable object")] [SerializeField] private MaterialSO obstacleMaterialSo;

    [Header("Obstacle durablity")] [Tooltip("Should be set to above 0")] [SerializeField]
    private float obstacleDurablity = 100;

    [Space] [SerializeField] private TextMeshPro hpDisplayText; //for show damage dealing

    [Header("Delay time after destroying obstacle, after which the event will be triggered")] [SerializeField]
    private float destroyEventActivationDelay = 0.1f;

    [Header("Event/s to trigger after destroy")] [SerializeField]
    private UnityEvent eventToTriggerAfterDestroy;

    #endregion

    #region private local variables

    private float _actualDurablity;

    #endregion
    
    private void Start()
    {
        if (obstacleMaterialSo == null)
            Debug.LogError($"This {transform.name} Obstacle has no set material Scriptable Object!");

        if (obstacleDurablity <= 0)
            Debug.LogError($"This {transform.name} Obstacle have wrong durablity setted!");

        _actualDurablity = obstacleDurablity;
        RefreshHpText();
    }
    
    public virtual void Damage(IWeapon weapon)
    {
        if (weapon.DestroyableMaterials().Exists(x => x == obstacleMaterialSo))
        {
            if(_actualDurablity >0)
            {  
                _actualDurablity -= weapon.DamageValue();
                RefreshHpText();
            }

            if (_actualDurablity <= 0)
                DestroyObstacle();
        }
    }
    
    private void RefreshHpText()
    {
        if (hpDisplayText != null)
            hpDisplayText.text = $"{_actualDurablity}/{obstacleDurablity}";
    }

    public virtual void DestroyObstacle()
    {
        ObstacleEventsManager.Instance.ObstacleDestroyEventToTrigger(eventToTriggerAfterDestroy,
            destroyEventActivationDelay);
        Destroy(this.gameObject);
    }
}