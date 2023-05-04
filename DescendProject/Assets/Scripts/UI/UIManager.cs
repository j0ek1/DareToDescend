using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public PlayerControl player;
    public Movement movement;

    public TMP_Text floorTxt;
    public Image[] healthPoints;
    public Image dashBar;

    public Sprite fullHP;
    public Sprite halfHP;
    public Sprite emptyHP;

    void Update()
    {
        dashBar.fillAmount = (1f - movement.dashTimer) / 1f;
    }

    public void UpdateHealth()
    {
        for (int i = 0; i < healthPoints.Length; i++)
        {
            if (i < player.health)
            {
                if (i + 0.5 == player.health)
                {
                    healthPoints[i].sprite = halfHP;
                }
                else
                {
                    healthPoints[i].sprite = fullHP;
                }
            }
            else
            {
                healthPoints[i].sprite = emptyHP;
            }
        }
    }

    public void UpdateFloor(int floorNum)
    {
        floorTxt.text = "FLOOR: " + floorNum;
    }

    
}
