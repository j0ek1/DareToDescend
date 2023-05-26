using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public GameControl gameControl;
    private bool canLadder = false;
    public float health;
    public UIManager uiM;

    void Start()
    {
        
    }

    void Update()
    {
        if (canLadder && gameControl.currentRoom.cleared)
        {
            // UI CODE FOR PRESSING E HERE "PRESS E TO DESCEND"
            if (Input.GetKeyDown(KeyCode.E))
            {
                gameControl.StartCoroutine("Descend");
            }
        }

        if (health <= 0f)
        {
            // game over here
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "EBullet")
        {
            HealthChange(-0.5f);
        }
    }

    public void HealthChange(float change)
    {
        health += change;
        uiM.UpdateHealth();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // If player collides with door trigger, move room based on which direction the trigger was
        switch (col.gameObject.name)
        {
            case "TTrig":
                gameControl.MoveToRoom(1);
                break;
            case "RTrig":
                gameControl.MoveToRoom(2);
                break;
            case "BTrig":
                gameControl.MoveToRoom(3);
                break;
            case "LTrig":
                gameControl.MoveToRoom(4);
                break;
            default:
                break;
        }

        if (col.gameObject.tag == "Ladder")
        {
            canLadder = true;
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        canLadder = false;
    }
}
