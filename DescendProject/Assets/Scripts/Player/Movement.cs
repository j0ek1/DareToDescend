using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public GameObject player;
    public Rigidbody2D rb;
    private float maxSpeed = 3f;
    public float accel;
    private Vector2 direction;
    private float hori;
    private float vert;
    private Vector2 drag;
    public float dragVal;

    public Camera cam;
    private Vector2 mousePos;

    private bool isDashing = false;
    private float dashForce = 3f;
    private float dashTimer = 0f;
    private float dashTime = 0.15f;

    public bool canMove;


    void Update()
    {
        // Movement inputs
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        hori = direction.x;
        vert = direction.y;

        // Timer for dash cooldown
        if (dashTimer > 0)
        {
            dashTimer -= Time.deltaTime;
        }
        if (dashTimer < 0)
        {
            dashTimer = 0;
        }

        // Dash input
        if (Input.GetKeyDown(KeyCode.Space) && dashTimer == 0 && canMove)
        {
            StartCoroutine(Dash(direction));
            dashTimer = 1;
        }

        // Get world position of mouse
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }
    void FixedUpdate()
    {
        // Applying movement force
        if (!isDashing && canMove)
        {
            rb.AddForce(new Vector2(hori, vert) * accel);
        }

        // Capping speed
        if (Mathf.Abs(rb.velocity.x) > maxSpeed)
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
        }
        if (Mathf.Abs(rb.velocity.y) > maxSpeed)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Sign(rb.velocity.y) * maxSpeed);
        }

        // Applying drag on the horizontal and vertical axis seperately
        drag = rb.velocity;
        if (Mathf.Abs(hori) < 0.4f)
        {
            drag.x *= dragVal;
            rb.velocity = drag;
        }
        if (Mathf.Abs(vert) < 0.4f)
        {
            drag.y *= dragVal;
            rb.velocity = drag;
        }
        if (Mathf.Abs(hori) > 0.4f || Mathf.Abs(vert) > 0.4f)
        {
            drag = Vector2.zero;
        }

        // Calculate look direction and apply to player rotation
        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;

    }

    IEnumerator Dash(Vector2 dir)
    {
        isDashing = true;
        maxSpeed = 15f;
        if (dir.x != 0 && dir.y != 0) // Dash duration decreased for diagonal dashes
        {
            dashTime = 0.1f;
        }
        else
        {
            dashTime = 0.15f;
        }
        rb.AddForce(dashForce * dir, ForceMode2D.Impulse);
        yield return new WaitForSeconds(dashTime);
        maxSpeed = 3f;
        isDashing = false;
    }
}
