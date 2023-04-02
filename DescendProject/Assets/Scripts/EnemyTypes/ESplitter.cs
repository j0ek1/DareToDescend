using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ESplitter : MonoBehaviour
{
    public Enemy e;
    public float speed;
    private float maxSpeed = 2.5f;
    private bool canAttack = true;
    private float attackTimer = 0f;
    public VisualEffect attackEffect;
    public GameObject splitter;
    private int splitNumber = 3;

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

        if (Vector2.Distance(transform.position, e.player.transform.position) < .75f && canAttack)
        {
            e.player.health -= 1;
            GameObject effect = Instantiate(attackEffect.gameObject, transform.position, Quaternion.identity, transform);
            Destroy(effect, 1f);
            attackTimer = .5f;
            canAttack = false;
        }
        if (!canAttack)
        {
            attackTimer -= Time.deltaTime;
        }
        if (attackTimer < 0f)
        {
            canAttack = true;
        }
    }
}
