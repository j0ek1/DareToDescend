using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EWalker : MonoBehaviour
{
    public Enemy e;
    public float speed;
    private float maxSpeed = 2.5f;

    private bool canAttack = true;
    private float attackTimer = 0f;
    private float attackRange = 0.75f;
    public VisualEffect attackEffect;

    void Start()
    {
        e = GetComponent<Enemy>();
        speed = 0f;
    }

    void FixedUpdate()
    {
        if (e.delayStart)
        {
            if (speed < maxSpeed) // Accelerate to max speed
            {
                speed += .05f;
            }
            e.rb.velocity = transform.up * speed; // Move the enemy
        }

        // If in attack range, attack player
        if (Vector2.Distance(transform.position, e.player.transform.position) < attackRange && canAttack)
        {
            e.player.HealthChange(-1f);
            GameObject effect = Instantiate(attackEffect.gameObject, transform.position, Quaternion.identity, transform);
            Destroy(effect, 1f);
            attackTimer = .5f;
            canAttack = false;
        }
        if (!canAttack) // Countdown attack timer
        {
            attackTimer -= Time.deltaTime;
        }
        if (attackTimer < 0f)
        {
            canAttack = true;
        }
    }
}
