using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ELaser : MonoBehaviour
{
    public Enemy e;

    public Transform firePos;
    private float aimTimer = 1.5f;
    private float shotDelayTimer = 0.15f;
    private float shotDelay = 0.15f;
    private bool delayShot = false;
    private bool changeColor = false;

    private LineRenderer lr;
    private Vector2 endPosition = new Vector2();
    private Vector2 shotDirection = new Vector2();
    private Vector2 lineExtendedPosition = new Vector2();

    void Start()
    {
        e = GetComponent<Enemy>();
        lr = GetComponent<LineRenderer>();
    }

    void FixedUpdate()
    {
        if (aimTimer > 0 && e.delayStart)
        {
            aimTimer -= Time.deltaTime;

            // Calculate position of line from enemy to player
            endPosition = e.player.transform.position;
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, endPosition);

            // Change laser colour back to white
            if (changeColor)
            {
                lr.startColor = Color.white;
                lr.endColor = Color.white;
                changeColor = false;
            }

            // Reduce width of laser based on aim timer
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
            lineExtendedPosition = endPosition + (shotDirection.normalized * 10f); // Calculate extended line
            lr.SetPosition(1, lineExtendedPosition); // Display extended line when shot is fired
            delayShot = true;
            aimTimer = 0;
        }
        if (aimTimer == 0)
        {
            shotDelayTimer -= Time.deltaTime;

            // Fade laser colour from white to red when charging up before shooting
            float fadeColor = (shotDelayTimer / shotDelay);
            lr.startColor = new Color(1f, fadeColor, fadeColor);
            lr.endColor = new Color(1f, fadeColor, fadeColor);
        }

        // Shoot and reset necessary variables
        if (shotDelayTimer <= 0)
        {
            Shoot();
            aimTimer = 1.5f;
            shotDelayTimer = shotDelay;
            changeColor = true;
            delayShot = false;
        }
    }

    void Shoot()
    {
        // Shoot raycast from the enemy in the calculated shot direction, if player is hit induce damage
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
