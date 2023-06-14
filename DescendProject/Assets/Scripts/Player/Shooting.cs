using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Shooting : MonoBehaviour
{
    public Transform firePos;
    public GameObject bulletPrefab;
    public float bulletForce;
    private float shootTimer = 0f;
    public Movement player;

    public int activeGunSlot;
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
        // Timer for shoot cooldown
        if (shootTimer > 0)
        {
            shootTimer -= Time.deltaTime;
        }
        if (shootTimer < 0)
        {
            shootTimer = 0;
        }

        // Fire inputs
        if (gunSlot[activeGunSlot].automatic) // If gun is automatic player can hold down fire button
        {
            if (Input.GetButton("Fire1") && shootTimer == 0 && player.canMove)
            {
                Shoot();
                shootTimer = gunSlot[activeGunSlot].shootInterval;
            }
        }
        else // If gun is not automatic player cannot hold down fire button to shoot
        {
            if (Input.GetButtonDown("Fire1") && shootTimer == 0 && player.canMove)
            {
                Shoot();
                shootTimer = gunSlot[activeGunSlot].shootInterval;
            }
        }

        // Weapon switching
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            activeGunSlot = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            activeGunSlot = 1;
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0f || Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            activeGunSlot = 1 - activeGunSlot;
            Debug.Log("active slot = " + activeGunSlot);
        }
    }

    void Shoot()
    {
        // Instantiate bullet and add force to fire
        GameObject bullet = Instantiate(bulletPrefab, firePos.position, firePos.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePos.up * bulletForce, ForceMode2D.Impulse);

        // Muzzle flash VFX
        GameObject effect = Instantiate(gunSlot[activeGunSlot].shootingVFX.gameObject, vfxPos.position, vfxPos.rotation, transform);
        Destroy(effect, .2f);
    }
}
