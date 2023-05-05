using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ELaser : MonoBehaviour
{
    public Enemy e;

    public Transform firePos;
    private float aimTimer = 2f;
    private float shotDelayTimer = 0.2f;

    private LineRenderer lr;
    private Vector2 endPosition = new Vector2();
    private Vector2 shotDirection = new Vector2();


    void Start()
    {
        e = GetComponent<Enemy>();
        lr = GetComponent<LineRenderer>();

    }

    void FixedUpdate()
    {
        if (aimTimer > 0)
        {
            aimTimer -= Time.deltaTime;

            endPosition = e.player.transform.position;
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, endPosition);
            lr.startWidth = aimTimer / 7.5f;
        }
        if (aimTimer <= 0 && e.delayStart)
        {
            shotDirection = endPosition - (Vector2)firePos.position;
            // start delay timer here
            Shoot();
            aimTimer = 1.5f;
            lr.startWidth = 0.2f;
        }

    }

    void Shoot()
    {
        RaycastHit2D hit = Physics2D.Raycast(firePos.position, shotDirection);
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Player")
            {
                e.player.HealthChange(-1.5f);
            }
            
        }
    }
}
