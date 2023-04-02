using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Minimap : MonoBehaviour
{
    public Image image;
    public List<Sprite> sprites = new List<Sprite>();
    public bool t;
    public bool r;
    public bool b;
    public bool l;

    void Start()
    {
        image = gameObject.GetComponent<Image>();
    }

    public void UpdateSect() // Could update if statements to be asking for 1 truth first, divides checks by a potential 14 to 7
    {
        if (t && !r && !b && !l)
        {
            image.sprite = sprites[0];
        }
        else if (!t && r && !b && !l)
        {
            image.sprite = sprites[1];
        }
        else if (!t && !r && b && !l)
        {
            image.sprite = sprites[2];
        }
        else if (!t && !r && !b && l)
        {
            image.sprite = sprites[3];
        }
        else if (!t && !r && b && l)
        {
            image.sprite = sprites[4];
        }
        else if (!t && r && !b && l)
        {
            image.sprite = sprites[5];
        }
        else if (t && !r && !b && l)
        {
            image.sprite = sprites[6];
        }
        else if (!t && r && b && !l)
        {
            image.sprite = sprites[7];
        }
        else if (t && !r && b && !l)
        {
            image.sprite = sprites[8];
        }
        else if (t && r && !b && !l)
        {
            image.sprite = sprites[9];
        }
        else if (t && !r && b && l)
        {
            image.sprite = sprites[10];
        }
        else if (t && r && !b && l)
        {
            image.sprite = sprites[11];
        }
        else if (!t && r && b && l)
        {
            image.sprite = sprites[12];
        }
        else if (t && r && b && !l)
        {
            image.sprite = sprites[13];
        }
    }
}
