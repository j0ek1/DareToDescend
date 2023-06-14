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
    public bool automatic;

    [Header("Reloading")]
    public int currentAmmo;
    public int magSize;
    public float reloadTime;
    public bool reloading;

    [Header("UI/VFX")]
    public VisualEffect shootingVFX;
    public Transform vfxPos;

}
