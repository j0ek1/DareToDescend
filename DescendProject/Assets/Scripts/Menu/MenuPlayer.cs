using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPlayer : MonoBehaviour
{

    private bool canLadder = false;

    // Update is called once per frame
    void Update()
    {
        if (canLadder)
        {
            // UI CODE FOR PRESSING E HERE "DARE TO DESCEND?"
            if (Input.GetKeyDown(KeyCode.E))
            {
                SceneManager.LoadScene(1);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
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
