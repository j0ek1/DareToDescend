using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    public Movement movement;

    [Header("UI Elements")]
    public TMP_Text floorTxt;
    public Image[] healthPoints;
    public Image dashBar;
    public GameObject crosshair;

    [Header("Resources")]
    public Sprite fullHP;
    public Sprite halfHP;
    public Sprite emptyHP;

    void Update()
    {
        dashBar.fillAmount = (1f - movement.dashTimer) / 1f;
    }

    // Called everytime player takes damage and updates health UI accordingly
    public void UpdateHealth(float playerHealth)
    {
        for (int i = 0; i < healthPoints.Length; i++)
        {
            if (i < playerHealth)
            {
                if (i + 0.5 == playerHealth)
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

    // Changes floor number when player descends
    public void UpdateFloor(int floorNum)
    {
        floorTxt.text = "FLOOR: " + floorNum;
    }

    // Crosshair spin effect for reloading
    public IEnumerator SpinCrosshair(float duration)
    {
        float startRotation = 0f;
        float endRotation = -360f;
        float t = 0.0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float zRotation = Mathf.Lerp(startRotation, endRotation, t / duration) % 360f;
            crosshair.transform.eulerAngles = new Vector3(crosshair.transform.eulerAngles.x, crosshair.transform.eulerAngles.y, zRotation);
            yield return null;
        }
    }
}
