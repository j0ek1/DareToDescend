using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PBullet : MonoBehaviour
{
    void Start()
    {
        Physics2D.IgnoreLayerCollision(6, 3, true);
        Physics2D.IgnoreLayerCollision(6, 6, true);
    }

    void OnCollisionEnter2D(Collision2D col)
    {    
        if (col.gameObject.tag != "Player" && col.gameObject.tag != "EBullet")
        {
            DestroyBullet();
        }  
    }

    public void DestroyBullet()
    {
        // Add destroy effects here
        Destroy(gameObject);
    }
}
