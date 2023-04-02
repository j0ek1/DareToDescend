using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public int[] openingDirection; // T1, R2, B3, L4
    public GameObject[] spawnPoint;
    private Rooms rooms;
    private int rand;
    private bool canSpawn = true;
    private GameObject newRoom;
    public bool cleared = false;
    public GameControl gameControl;

    public List<GameObject> vertDoors;
    public List<GameObject> horiDoors;
    public Animator animator;

    
    void Start()
    {
        rooms = GameObject.FindGameObjectWithTag("Rooms").GetComponent<Rooms>();
        gameControl = GameObject.FindObjectOfType<GameControl>();
        Invoke("Spawn", .1f);
    }

    void Update()
    {

    }

    void Spawn()
    {
        int i = 0;
        foreach (int dir in openingDirection) // Check if a room is in the space we want to spawn one
        {
            Collider2D[] roomCheck = Physics2D.OverlapCircleAll(spawnPoint[i].transform.position, 1f);
            foreach (Collider2D roomCol in roomCheck)
            {
                if (roomCol.CompareTag("RoomPoint"))
                {
                    canSpawn = false;
                }
            }
            if (canSpawn && gameControl.roomCount < 14)
            {
                if (gameControl.roomCount > 8) // After 8 rooms, close off all exits
                {
                    rand = 0;
                }
                else if (gameControl.roomCount < 5) // First few rooms generated cant be a room with only 1 door (dead end)
                {
                    rand = Random.Range(0, 4);
                    if (rand > 2) // 25% chance to spawn a room with 3 doors (index 4 - 6)
                    {
                        rand = Random.Range(4, 7);
                    }
                    else // 75% chance to spawn room with 2 doors (index 1 - 3)
                    {
                        rand = Random.Range(1, 4);
                    }
                }
                else
                {
                    rand = Random.Range(0, 4);
                    if (rand > 2) // 25% chance to spawn a room with 3 doors (index 4 - 6)
                    {
                        rand = Random.Range(4, 7);
                    }
                    else // 75% chance to spawn room with 2 or less doors (index 0 - 3)
                    {
                        rand = Random.Range(0, 4);
                    }
                }
                switch (dir)
                {
                    case 1: // Top opening so spawn a bottom room
                        newRoom = Instantiate(rooms.bRooms[rand], spawnPoint[i].transform.position, Quaternion.identity) as GameObject;
                        AddGameControl(i);
                        break;
                    case 2: // Right opening so spawn a left room
                        newRoom = Instantiate(rooms.lRooms[rand], spawnPoint[i].transform.position, Quaternion.identity) as GameObject;
                        AddGameControl(i);
                        break;
                    case 3: // Bottom opening so spawn a top room
                        newRoom = Instantiate(rooms.tRooms[rand], spawnPoint[i].transform.position, Quaternion.identity) as GameObject;
                        AddGameControl(i);
                        break;
                    case 4: // Left opening so spawn a right room
                        newRoom = Instantiate(rooms.rRooms[rand], spawnPoint[i].transform.position, Quaternion.identity) as GameObject;
                        AddGameControl(i);
                        break;
                    default:
                        break;
                }
            }
            i++;
            canSpawn = true;
        }       
    }

    void AddGameControl(int i) // Add all rooms to dictionary with coordinates so they can be controlled
    {
        gameControl.roomCoord.Add(new Vector2((int)spawnPoint[i].transform.position.x / 18, (int)spawnPoint[i].transform.position.y / 10), newRoom);    
        gameControl.roomList.Add(newRoom);
        gameControl.roomCount++;
    }

    public void MoveDoors(bool shouldOpen)
    {
        if (shouldOpen)
        {
            animator.SetBool("Open", true);
        }
        else
        {
            animator.SetBool("Open", false);
        }
    }
}
