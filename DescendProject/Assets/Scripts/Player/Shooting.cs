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
    private int[] currentAmmo = new int[2];
    private bool[] isReloading = new bool[2];
    public GunData[] gunSlot = new GunData[2];
    public GunData[] guns;
    public Transform vfxPos;

    private Coroutine lastCrosshairCoroutine = null;
    private Coroutine lastAmmoCoroutine = null;

    void Start()
    {
        activeGunSlot = 0;
        gunSlot[0] = guns[0];
        currentAmmo[0] = guns[0].magSize;
        isReloading[0] = false;
        gunSlot[1] = guns[1];
        currentAmmo[1] = guns[1].magSize;
        isReloading[1] = false;
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

        if (currentAmmo[activeGunSlot] == 0 || Input.GetKeyDown(KeyCode.R))
        {
            if (!isReloading[activeGunSlot])
            {
                reloadTimer = gunSlot[activeGunSlot].reloadTime;
                lastCrosshairCoroutine = StartCoroutine(uiM.SpinCrosshair(reloadTimer));
                lastAmmoCoroutine = StartCoroutine(uiM.ReloadAmmo(reloadTimer));
            }
            Reload();
        }

        // Fire inputs
        if (!isReloading[activeGunSlot] && intervalTimer == 0 && player.canMove)
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
            if (gunSlot[1 - activeGunSlot] != null) // If gunslot is null (no weapon), player can't switch weapon
            {
                // Stop previous coroutines because weapon changed
                StopCoroutine(lastCrosshairCoroutine);
                StopCoroutine(lastAmmoCoroutine);

                // Change active gun
                activeGunSlot = 1 - activeGunSlot;

                // If the gun still needs to reload, start the reload process again
                if (currentAmmo[activeGunSlot] == 0)
                {
                    reloadTimer = gunSlot[activeGunSlot].reloadTime;
                    lastCrosshairCoroutine = StartCoroutine(uiM.SpinCrosshair(reloadTimer));
                    lastAmmoCoroutine = StartCoroutine(uiM.ReloadAmmo(reloadTimer));
                }

                // Update the gun UI variables as the weapon has been changed
                uiM.UpdateGun(guns[activeGunSlot].ID);
                uiM.UpdateAmmo(currentAmmo[activeGunSlot], gunSlot[activeGunSlot].magSize);
                uiM.ResetCrosshair();
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
        currentAmmo[activeGunSlot] -= 1;

        // Update ammo UI
        uiM.UpdateAmmo(currentAmmo[activeGunSlot], gunSlot[activeGunSlot].magSize);
    }

    // Handles timer for reloading and the associated gun variables
    private void Reload()
    {
        isReloading[activeGunSlot] = true;
        if (reloadTimer > 0)
        {
            reloadTimer -= Time.deltaTime;
        }
        if (reloadTimer < 0)
        {
            reloadTimer = 0;
            currentAmmo[activeGunSlot] = gunSlot[activeGunSlot].magSize;
            isReloading[activeGunSlot] = false;
        }
    }
}