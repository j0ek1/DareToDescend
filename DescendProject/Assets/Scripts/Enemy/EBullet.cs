using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EBullet : MonoBehaviour
{
    void Start()
    {
        Physics2D.IgnoreLayerCollision(3, 6, true);
        Physics2D.IgnoreLayerCollision(3, 3, true);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag != "EBullet" && col.gameObject.tag != "PBullet")
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
