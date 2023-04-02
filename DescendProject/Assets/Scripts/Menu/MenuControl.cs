using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static UnityEngine.GraphicsBuffer;

public class MenuControl : MonoBehaviour
{
    [Header("Player")]
    public Movement player;
    private Vector2 playerStartPos = new Vector2();

    [Header("Camera")]
    public GameObject mainCamera;
    private Vector3 camStart = new Vector3(0f, 0f, -10f);
    private Vector3 camEnd = new Vector3();
    private bool camMove = false;
    private float lerpTime = 1f;

    [Header("Doors")]
    public List<GameObject> horiDoors;
    public Animator bRoom;

    public Light2D flickerLight;
    private bool shouldFlicker = true;


    void Start()
    {
        // Set variables for room change
        camEnd = new Vector3(camStart.x, camStart.y - 10f, -10f);
        playerStartPos = new Vector2(player.transform.position.x, - 5.9f);

        StartCoroutine(Flicker());
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // When player touches room change trigger
        if (col.gameObject.tag == "Player")
        {
            camMove = true; // Move camera

            GetComponent<BoxCollider2D>().enabled = false; // Disable trigger

            shouldFlicker = false;

            // Freeze player position temporarily
            player.canMove = false;
            player.rb.velocity = Vector2.zero;
            player.transform.position = playerStartPos;

            bRoom.SetBool("Open", false); // Close doors
        }        
    }

    void Update()
    {
        // Camera transition and moving door triggers
        if (camMove)
        {
            float t = 6f * Time.deltaTime;
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, camEnd, t);
            lerpTime -= Time.deltaTime;
        }
        if (lerpTime <= 0f) // Reset necessary variables after camera has moved
        {
            camMove = false;
            mainCamera.transform.position = camEnd;
            player.canMove = true;
            lerpTime = 1f;
        }

        
    }

    private void FixedUpdate()
    {
        if (!shouldFlicker && flickerLight.intensity > 0)
        {
            flickerLight.intensity -= .1f;
        }
    }

    IEnumerator Flicker()
    {
        while (shouldFlicker)
        {
            float randomIntensity = Random.Range(1.5f, 3.5f);
            flickerLight.intensity = randomIntensity;

            float randomTime = Random.Range(0f, 0.1f);
            yield return new WaitForSeconds(randomTime);
        }
        if (!shouldFlicker)
        {
            yield return null;
        }
    }

}
