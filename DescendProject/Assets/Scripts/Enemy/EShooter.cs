using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EShooter : MonoBehaviour
{
    public Enemy e;
    public float speed;
    private float maxSpeed = 3f;

    public Transform firePos;
    public GameObject bulletPrefab;
    public float bulletForce;
    private float shootTimer = 0f;

    private Vector2 targetPosition;
    private Vector2 targetMidpoint;
    private bool accelerate = false;
    private bool decelerate = false;
    private bool newPos = true;
    private float deltaSpeed = 0.1f;
    private int xlr8Counter = 0;
    private int framesUntilXlr8 = 2;
    private float stuckTime = 0f;

    void Start()
    {
        e = GetComponent<Enemy>();

        targetPosition = (Vector2)transform.position + Random.insideUnitCircle * 5;

    }

    void FixedUpdate()
    {
        if (shootTimer > 0)
        {
            shootTimer -= Time.deltaTime;
        }
        if (shootTimer <= 0 && e.delayStart)
        {
            Shoot();
            shootTimer = 1f + Random.Range(-.5f, .5f);
        }

        xlr8Counter++;
        if (newPos)
        {
            // Calculate new coordinate bounds of the current room
            float xLBound = (e.gameControl.currentRoomCoord.x * 18f) - 8f;
            float xUBound = (e.gameControl.currentRoomCoord.x * 18f) + 8f;
            float yLBound = (e.gameControl.currentRoomCoord.y * 10f) - 3.9f;
            float yUBound = (e.gameControl.currentRoomCoord.y * 10f) + 3.9f;

            // Calculate new target position, if out of bounds then redo
            targetPosition = (Vector2)transform.position + Random.insideUnitCircle * 5;
            while (targetPosition.x < xLBound || targetPosition.x > xUBound || targetPosition.y < yLBound || targetPosition.y > yUBound)
            {
                targetPosition = (Vector2)transform.position + Random.insideUnitCircle * 5;
            }

            targetMidpoint = new Vector2((transform.position.x + targetPosition.x) / 2, (transform.position.y + targetPosition.y) / 2); // Calculate midpoint
            framesUntilXlr8 = Mathf.RoundToInt(Vector2.Distance(transform.position, targetMidpoint)); // This variable decides how often the enemies speed will be increased/decreased based on distance away from target
            if (framesUntilXlr8 == 0) // Prevent code from dividing by zero
            {
                framesUntilXlr8 = 1;
            }
            newPos = false;
            accelerate = true;
        }

        // Accelerate towards the midpoint
        if (accelerate)
        {
            e.rb.velocity = (targetMidpoint - (Vector2)transform.position).normalized * speed;
            if (speed < maxSpeed && xlr8Counter % framesUntilXlr8 == 0) // Rate at which the enemy accelerates
            {
                speed += deltaSpeed;
            }
            if (Vector2.Distance(transform.position, targetMidpoint) <= 0.1f) // Once midpoint is reached start decelerating
            {
                stuckTime = 0;
                accelerate = false;
                decelerate = true;
            }
        }

        // Decelerate from midpoint to target
        if (decelerate)
        {
            e.rb.velocity = (targetPosition - (Vector2)transform.position).normalized * speed;
            if (speed > 1f && xlr8Counter % framesUntilXlr8 == 0) // Rate at which the enemy decelerates
            {
                speed -= deltaSpeed;
            }
            if (Vector2.Distance((Vector2)transform.position, targetPosition) <= 0.1f) // Once target is reached calculate new position
            {
                stuckTime = 0;
                decelerate = false;
                newPos = true;
            }
        }
        if (xlr8Counter > 999)
        {
            xlr8Counter = 0;
        }

        // If the enemy hasnt changed state in a while find new position and retry
        stuckTime += Time.deltaTime;
        if (stuckTime > 4f)
        {
            accelerate = false;
            decelerate = false;
            speed = 1f;
            newPos = true;
        }

    }

    void Shoot()
    {
        // Instantiate bullet and add force to fire
        GameObject bullet = Instantiate(bulletPrefab, firePos.position, firePos.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePos.up * bulletForce, ForceMode2D.Impulse);
    }
}
