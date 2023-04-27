using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
    [Header("Room")]
    public int roomCount;
    public Rooms rooms;
    private int rand;
    public Dictionary<Vector2, GameObject> roomCoord = new Dictionary<Vector2, GameObject>();
    public List<GameObject> roomList = new List<GameObject>();
    private GameObject startRoom;
    public RoomSpawner currentRoom;
    public Vector2 currentRoomCoord;
    public GameObject ladderPrefab;
    public int floor;
    public int roomDir;
    public GameObject doorTriggers;

    [Header("Camera")]
    public GameObject mainCamera;
    private Vector3 camStart = new Vector3();
    private Vector3 camEnd = new Vector3();
    private bool camMove = false;
    private float lerpTime = 1f;

    [Header("UI")]
    public MinimapControl minimap;
    public Animator blackScreen;
    public UIManager uiM;

    [Header("Entities")]
    public Movement player;
    private Vector2 playerStartPos = new Vector2();
    public List<GameObject> currentEnemies = new List<GameObject>();
    public List<GameObject> enemyType;
    public GameObject splitterPrefab;

    private int xLBound = -5;
    private int xUBound = 6;
    private int yLBound = -3;
    private int yUBound = 4;
    private List<int> xPrev = new List<int>();
    private List<int> yPrev = new List<int>();

    void Start()
    {
        floor = 0;
        InitializeFloor();
    }

    void InitializeFloor()
    {
        rand = Random.Range(0, 4);
        switch (rand) // Generate random start room
        {
            case 0:
                startRoom = Instantiate(rooms.tRooms[Random.Range(1, 7)], Vector3.zero, Quaternion.identity) as GameObject;
                break;
            case 1:
                startRoom = Instantiate(rooms.rRooms[Random.Range(1, 7)], Vector3.zero, Quaternion.identity) as GameObject;
                break;
            case 2:
                startRoom = Instantiate(rooms.bRooms[Random.Range(1, 7)], Vector3.zero, Quaternion.identity) as GameObject;
                break;
            case 3:
                startRoom = Instantiate(rooms.lRooms[Random.Range(1, 7)], Vector3.zero, Quaternion.identity) as GameObject;
                break;
        }
        roomCoord.Add(currentRoomCoord, startRoom); // Add start room to dictionary
        currentRoom = roomCoord[currentRoomCoord].GetComponent<RoomSpawner>();
        currentRoom.cleared = true;

        minimap.InitializeMinimap();

        player.canMove = true; // Player can move
        Invoke("ClearPoints", 2f);
    }

    void Update()
    {
        if (currentEnemies.Count <= 0 && !currentRoom.cleared) // When all enemies defeated
        {
            currentRoom.MoveDoors(true); // Open doors
            currentRoom.cleared = true;
        }

        // Camera transition and moving door triggers
        if (camMove)
        {
            doorTriggers.SetActive(false);
            doorTriggers.transform.position = new Vector2(currentRoomCoord.x * 18f, currentRoomCoord.y * 10f);
            float t = 6f * Time.deltaTime;
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, camEnd, t);
            lerpTime -= Time.deltaTime;
        }
        if (lerpTime <= 0f) // Reset necessary variables
        {    
            camMove = false;
            mainCamera.transform.position = camEnd;
            doorTriggers.SetActive(true);
            player.canMove = true;
            lerpTime = 1f;
        }
    }

    private void ClearPoints()
    {
        // Destroy all unnecessary points containing colliders
        GameObject[] sPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        GameObject[] rPoints = GameObject.FindGameObjectsWithTag("RoomPoint");
        foreach (GameObject point in sPoints)
        {
            Destroy(point);
        }
        foreach (GameObject point in rPoints)
        {
            Destroy(point);
        }

        // Spawn ladder in final room
        Instantiate(ladderPrefab, roomList[roomList.Count - 1].gameObject.transform.position, Quaternion.identity);
        roomList.Clear(); // Clear list after use as data is stored in dictionary
    }

    public void MoveToRoom(int dir)
    {
        // Set up camera transition and where the player will start in the next room
        camStart = mainCamera.transform.position;
        roomDir = dir;
        switch (dir)
        {
            case 1: // T
                camEnd = new Vector3(camStart.x, camStart.y + 10f, -10f);
                playerStartPos = new Vector2(player.transform.position.x, (currentRoomCoord.y * 10f) + 5.9f);
                currentRoomCoord = new Vector2(currentRoomCoord.x, currentRoomCoord.y + 1);
                yLBound += 1;
                break;
            case 2: // R
                camEnd = new Vector3(camStart.x + 18f, camStart.y, -10f);
                playerStartPos = new Vector2((currentRoomCoord.x * 18f) + 9.9f, player.transform.position.y);
                currentRoomCoord = new Vector2(currentRoomCoord.x + 1, currentRoomCoord.y);
                xLBound += 1;
                break;
            case 3: // B
                camEnd = new Vector3(camStart.x, camStart.y - 10f, -10f);
                playerStartPos = new Vector2(player.transform.position.x, (currentRoomCoord.y * 10f) - 5.9f);
                currentRoomCoord = new Vector2(currentRoomCoord.x, currentRoomCoord.y - 1);
                yUBound -= 1;
                break;
            case 4: // L
                camEnd = new Vector3(camStart.x - 18f, camStart.y, -10f);
                playerStartPos = new Vector2((currentRoomCoord.x * 18f) - 9.9f, player.transform.position.y);
                currentRoomCoord = new Vector2(currentRoomCoord.x - 1, currentRoomCoord.y);
                xUBound -= 1;
                break;
            default:
                break;
        }
        camMove = true;

        // Freeze player position temporarily
        player.canMove = false;
        player.rb.velocity = Vector2.zero;
        player.transform.position = playerStartPos;

        // Set the current room
        currentRoom = roomCoord[currentRoomCoord].GetComponent<RoomSpawner>();

        // If room is uncleared, close doors, update minimap, and spawn entities
        if (!currentRoom.cleared)
        {
            currentRoom.MoveDoors(false);
            minimap.UpdateMinimap(currentRoomCoord, dir);

            SpawnEntities();
        }
        else // Else update player icon only
        {
            minimap.UpdatePlayer(currentRoomCoord);
        }
    }

    private void SpawnEntities()
    {
        for (int i = 0; i < 3; i++) // the "3" will increase overtime, change to a variable.
        {
            int xRand = Random.Range(xLBound, xUBound);
            int yRand = Random.Range(yLBound, yUBound);
            for (int j = 0; j < xPrev.Count; j++)
            {
                if (xRand == xPrev[j] && yRand == yPrev[j]) // If another enemy is occupying that space, set new random and check again
                {
                    xRand = Random.Range(xLBound, xUBound);
                    yRand = Random.Range(yLBound, yUBound);
                    j = 0;
                }
            }
            Vector2 spawnPos = new Vector2((currentRoomCoord.x * 18) + xRand, (currentRoomCoord.y * 10) + yRand);

            // this is where enemies are spawned
            // enemies with a higher number in the list will be harder/difficult
            // on each floor less common to spawn a lower number?
            Instantiate(enemyType[2], spawnPos, Quaternion.identity);

            // Add enemies spawn position to list to prevent spawn overlapping
            xPrev.Add(xRand);
            yPrev.Add(yRand);
        }
        // Reset variables
        xPrev.Clear();
        yPrev.Clear();
        xLBound = -5;
        xUBound = 6;
        yLBound = -3;
        yUBound = 4;
    }

    // Respawn code to help splitter respawn with a new prefab and not a clone
    public GameObject RespawnSplitter(Vector3 respawnPos)
    {
        GameObject splitter = Instantiate(splitterPrefab, respawnPos, Quaternion.identity);
        return splitter;
    }

    // Controls changing floor
    public IEnumerator Descend()
    {
        blackScreen.Play("FadeToBlack");

        yield return new WaitForSeconds(0.22f);

        // Freeze player and reset position
        player.canMove = false;
        player.rb.velocity = Vector2.zero;
        player.transform.position = Vector2.zero;

        floor++; // Increase floor completion count
        uiM.UpdateFloor(floor);

        // Reset necessary variables
        doorTriggers.transform.position = Vector2.zero;
        mainCamera.transform.position = new Vector3(0f, 0f, -10f);
        currentRoomCoord = Vector2.zero;
        roomCount = 1;

        // Remove ladder from scene
        GameObject thisLadder = GameObject.FindGameObjectWithTag("Ladder");
        Destroy(thisLadder);

        roomCoord.Clear(); // Clear dictionary of rooms and coordinates

        // Delete all current rooms in scene
        GameObject[] allRooms = GameObject.FindGameObjectsWithTag("Room");
        foreach (GameObject r in allRooms)
        {
            Destroy(r);
        }

        minimap.ClearMinimap(); // Reset minimap

        InitializeFloor(); // Setup next floor
    }
}
