using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ESplitter : MonoBehaviour
{
    public Enemy e;
    public float speed;
    public float health;
    private float maxSpeed = 2.5f;
    public GameControl gameControl;

    private bool canAttack = true;
    private float attackTimer = 0f;
    private float attackRange = 0.75f;
    public VisualEffect attackEffect;

    private List<ESplitter> nextSplitter = new List<ESplitter>();
    public int splitNumber = 1;

    void Start()
    {
        e = GetComponent<Enemy>();
        gameControl = FindObjectOfType<GameControl>();
        speed = 0f;
    }

    void FixedUpdate()
    {
        if (speed < maxSpeed) // Accelerate to max speed
        {
            speed += .05f;
        }
        e.rb.velocity = transform.up * speed; // Move the enemy

        // If in attack range, attack player
        if (Vector2.Distance(transform.position, e.player.transform.position) < attackRange && canAttack)
        {
            e.player.HealthChange(-.5f);
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

        // When this splitter dies, instantiate new splitters
        if (health <= 0)
        {   
            for (int i = 0; i < 3; i++)
            {
                GameObject splitterObj = gameControl.RespawnSplitter(transform.position);
                nextSplitter.Add(splitterObj.GetComponent<ESplitter>());
                if (splitNumber == 1) // If it is the first split, spawn 3 more with less health and smaller scale
                {
                    nextSplitter[i].health = 3;
                    nextSplitter[i].maxSpeed = 3.5f;
                    nextSplitter[i].gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
                    nextSplitter[i].splitNumber = 2;

                }
                if (splitNumber == 2) // If it is the second split, reduce scale but no need to change health as it is handled in main Enemy script
                {
                    nextSplitter[i].maxSpeed = 4.5f;
                    nextSplitter[i].gameObject.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
                    nextSplitter[i].splitNumber = 3;
                }               
            }
            nextSplitter.Clear();
            gameControl.currentEnemies.Remove(gameObject);
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // Reduce health if it is not on the last split
        if (col.gameObject.tag == "PBullet")
        {
            if (splitNumber != 3)
            {
                health -= 1;
            }

        }
    }
}
