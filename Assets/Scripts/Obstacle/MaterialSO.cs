using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Obstacle/Material")]
public class MaterialSO : ScriptableObject
{
    [SerializeField] private ObstacleMaterialType obstacleMaterialType;
    [SerializeField] private string materialName;


    public ObstacleMaterialType GetObstacleMaterialType()
    {
        return obstacleMaterialType;
    }

    public string GetObstacleMaterialName()
    {
        return materialName;
    }
}
