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
    private Vector2 lineExtendedPosition = new Vector2();
    private bool delayShot = false;


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
            if (aimTimer > 0.2f)
            {
                lr.startWidth = aimTimer / 7.5f;
            }
            else
            {
                lr.startWidth = 0.026f;
            }
            
        }
        if (aimTimer <= 0 && !delayShot)
        {
            shotDirection = endPosition - (Vector2)firePos.position; // Calculate final shot direction
            lineExtendedPosition = endPosition + (shotDirection.normalized * 5f);
            lr.SetPosition(1, lineExtendedPosition);
            lr.startColor = Color.red; // CAN FADE COLORS INSTEAD OF STRAIGHT UP CHANGE
            lr.endColor = Color.red;
            delayShot = true;
            aimTimer = 0;
        }
        if (aimTimer == 0)
        {
            shotDelayTimer -= Time.deltaTime;
        }
        if (shotDelayTimer <= 0)
        {
            Shoot();
            aimTimer = 1.5f;
            shotDelayTimer = 0.2f;
            lr.startColor = Color.white;
            lr.endColor = Color.white;
            delayShot = false;
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
