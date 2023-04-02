using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Enemy : MonoBehaviour
{
    public int health;
    public PlayerControl player;
    public GameControl gameControl;
    public float lookSpeed;
    public Rigidbody2D rb;
    private float delayTime = 1f;
    public bool delayStart;

    public VisualEffect[] deathEffect;
    public int deathEID;

    void Start()
    {
        delayTime += Random.Range(0f, .5f);
        gameControl = GameObject.FindObjectOfType<GameControl>();
        gameControl.currentEnemies.Add(gameObject);
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindObjectOfType<PlayerControl>();
        int randDir = Random.Range(0, 360);
        transform.rotation = Quaternion.Euler(0, 0, randDir);
    }

    void Update()
    {
        if (!delayStart)
        {
            delayTime -= Time.deltaTime;
        }
        if (delayTime <= 0f)
        {
            delayStart = true;
        }
        if (health <= 0)
        {
            GameObject effect = Instantiate(deathEffect[deathEID].gameObject, transform.position, Quaternion.identity);
            Destroy(effect, .6f);
            gameControl.currentEnemies.Remove(gameObject);
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (true)
        {
            // Calculate angle to look towards the player and rotate at a speed of 180 degrees per second
            Vector2 lookDir = player.transform.position - transform.position;
            float angle = (Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg) - 90f;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, lookSpeed * Time.deltaTime);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "PBullet")
        {
            health -= 1;
        }
    }
}
