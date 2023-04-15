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

    public VisualEffect[] shootingEffect;
    public int gunID;
    public Transform vfxPos;

    void Update()
    {
        // Timer for dash cooldown
        if (shootTimer > 0)
        {
            shootTimer -= Time.deltaTime;
        }
        if (shootTimer < 0)
        {
            shootTimer = 0;
        }

        if (Input.GetButton("Fire1") && shootTimer == 0 && player.canMove)
        {
            Shoot();
            shootTimer = .2f;
        }

    }

    void Shoot()
    {
        // Instantiate bullet and add force to fire
        GameObject bullet = Instantiate(bulletPrefab, firePos.position, firePos.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePos.up * bulletForce, ForceMode2D.Impulse);

        // Muzzle flash VFX
        GameObject effect = Instantiate(shootingEffect[gunID].gameObject, vfxPos.position, vfxPos.rotation, transform);
        Destroy(effect, .2f);
    }
}
