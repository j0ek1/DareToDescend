using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[CreateAssetMenu(fileName = "Gun", menuName = "Gun")]
public class GunData : ScriptableObject
{
    [Header("Info")]
    public string gunName;
    
    [Header("Shooting")]
    public float damage;
    public float shootInterval;
    public bool isAutomatic;

    [Header("Reloading")]
    public int magSize;
    public float reloadTime;
    public bool isReloading;

    [Header("UI/VFX")]
    public VisualEffect shootingVFX;
    public Transform vfxPos;

}
