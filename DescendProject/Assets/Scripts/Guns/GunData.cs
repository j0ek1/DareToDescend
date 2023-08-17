using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[CreateAssetMenu(fileName = "Gun", menuName = "Gun")]
public class GunData : ScriptableObject
{
    [Header("Info")]
    public string weaponName;
    public int ID;
    
    [Header("Shooting")]
    public float damage;
    public float shootInterval;
    public bool isAutomatic;

    [Header("Reloading")]
    public int magSize;
    public float reloadTime;

    [Header("UI/VFX")]
    public VisualEffect shootingVFX;
    public Transform vfxPos;

}
