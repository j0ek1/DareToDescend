using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    public Movement movement;

    [Header("Variables")]
    public bool hasSwitched;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text floorTxt;
    [SerializeField] private Image[] healthPoints;
    [SerializeField] private Image dashBar;
    [SerializeField] private GameObject crosshair;
    [SerializeField] private Image gunImage;
    [SerializeField] private Slider ammoSlider;
    [SerializeField] private Slider ammoBgSlider;

    [Header("Resources")]
    [SerializeField] private Sprite fullHP;
    [SerializeField] private Sprite halfHP;
    [SerializeField] private Sprite emptyHP;

    [SerializeField] private Sprite[] gunSprites;

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

    public void UpdateGun(int gunID)
    {
        gunImage.sprite = gunSprites[gunID];
    }

    public void UpdateAmmo(int currentAmmo, int maxAmmo)
    {
        ammoSlider.value = (float)currentAmmo / (float)maxAmmo;
        ammoBgSlider.value = ((float)maxAmmo - (float)currentAmmo) / (float)maxAmmo;
    }

    // Ammo slider reload effect
    public IEnumerator ReloadAmmo(float duration)
    {
        float startValue = 0f;
        float endValue = 1f;
        float t = 0.0f;
        while (t < duration)
        {
            if (hasSwitched)
            {
                yield break;
            }
            t += Time.deltaTime;
            float fillValue = Mathf.Lerp(startValue, endValue, t / duration);
            ammoSlider.value = fillValue;
            ammoBgSlider.value = 1f - fillValue;
            yield return null;
        }
    }

    public void ResetCrosshair()
    {
        crosshair.transform.eulerAngles = Vector3.zero;
    }

    // Crosshair spin effect for reloading
    public IEnumerator SpinCrosshair(float duration)
    {
        float startRotation = 0f;
        float endRotation = -360f;
        float t = 0.0f;
        while (t < duration)
        {
            if (hasSwitched)
            {
                yield break;
            }
            t += Time.deltaTime;
            float zRotation = Mathf.Lerp(startRotation, endRotation, t / duration) % 360f;
            crosshair.transform.eulerAngles = new Vector3(crosshair.transform.eulerAngles.x, crosshair.transform.eulerAngles.y, zRotation);
            yield return null;
        }
    }
}
