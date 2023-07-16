using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Shooting : MonoBehaviour
{
    public Transform firePos;
    public GameObject bulletPrefab;
    public float bulletForce;
    private float intervalTimer = 0f;
    public Movement player;
    public UIManager uiM;

    public int activeGunSlot;
    private float reloadTimer = 0f;
    public GunData[] gunSlot = new GunData[2];
    public GunData[] guns;
    public Transform vfxPos;

    void Start()
    {
        activeGunSlot = 0;
        gunSlot[0] = guns[0];
        gunSlot[1] = guns[1];
    }
    void Update()
    {
        // Timer for the delay between shots
        if (intervalTimer > 0)
        {
            intervalTimer -= Time.deltaTime;
        }
        if (intervalTimer < 0)
        {
            intervalTimer = 0;
        }

        if (gunSlot[activeGunSlot].currentAmmo == 0)
        {
            if (!gunSlot[activeGunSlot].isReloading)
            {
                reloadTimer = gunSlot[activeGunSlot].reloadTime;
                StartCoroutine(uiM.SpinCrosshair(reloadTimer));
            }
            Reload();
        }

        // Fire inputs
        if (!gunSlot[activeGunSlot].isReloading && intervalTimer == 0 && player.canMove)
        {
            if (gunSlot[activeGunSlot].isAutomatic) // If gun is automatic player can hold down fire button
            {
                if (Input.GetButton("Fire1"))
                {
                    Shoot();
                    intervalTimer = gunSlot[activeGunSlot].shootInterval;
                }
            }
            else // If gun is not automatic player cannot hold down fire button to shoot
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    Shoot();
                    intervalTimer = gunSlot[activeGunSlot].shootInterval;
                }
            }
        }

        // Weapon switching for num keys and scroll wheel
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetAxis("Mouse ScrollWheel") > 0f || Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            activeGunSlot = 1 - activeGunSlot;
            if (gunSlot[activeGunSlot].currentAmmo == 0)
            {
                reloadTimer = gunSlot[activeGunSlot].reloadTime;
            }
        }
    }

    private void Shoot()
    {
        // Instantiate bullet and add force to fire
        GameObject bullet = Instantiate(bulletPrefab, firePos.position, firePos.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePos.up * bulletForce, ForceMode2D.Impulse);

        // Muzzle flash VFX
        GameObject effect = Instantiate(gunSlot[activeGunSlot].shootingVFX.gameObject, vfxPos.position, vfxPos.rotation, transform);
        Destroy(effect, .2f);

        // Decrease current ammo
        gunSlot[activeGunSlot].currentAmmo -= 1;
    }

    // Handles timer for reloading and the associated gun variables
    private void Reload()
    {
        gunSlot[activeGunSlot].isReloading = true;
        if (reloadTimer > 0)
        {
            reloadTimer -= Time.deltaTime;
        }
        if (reloadTimer < 0)
        {
            reloadTimer = 0;
            gunSlot[activeGunSlot].currentAmmo = gunSlot[activeGunSlot].magSize;
            gunSlot[activeGunSlot].isReloading = false;
        }
    }

    public float XHairZRotation()
    {
        float z = -((reloadTimer / gunSlot[activeGunSlot].reloadTime) * 360f) / 360f;
        return z;
    }
}
