using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MinimapControl : MonoBehaviour
{
    public Dictionary<Vector2, Minimap> mapObjs = new Dictionary<Vector2, Minimap>();
    public Minimap mapSect;
    public List<GameObject> currentMap = new List<GameObject>();
    public Transform minimap;
    public GameObject playerPos;

    Vector2 CalculatePos(Vector2 pos) // Converts room coordinates to minimap coordinates
    {
        float x = pos.x * 50f;
        float y = pos.y * 50f;
        Vector2 calcPos = new Vector2(x, y);
        return calcPos;
    }


    // Setup minimap with initial room
    public void InitializeMinimap()
    {
        Minimap sect = Instantiate(mapSect, minimap);
        sect.transform.localPosition = Vector2.zero;
        mapObjs.Add(Vector2.zero, sect);
        currentMap.Add(sect.gameObject);
    }

    public void UpdateMinimap(Vector2 currentPos, int dir) // If room entered is uncleared, update minimap by adding new section and adding a door on old section
    {
        Minimap sect = Instantiate(mapSect, minimap);
        sect.transform.localPosition = CalculatePos(currentPos);
        Vector2 prevRoom;
        switch (dir)
        {
            case 1: // T
                sect.b = true;
                prevRoom = new Vector2(currentPos.x, currentPos.y - 1f);
                mapObjs[prevRoom].t = true;
                mapObjs[prevRoom].UpdateSect();
                break;
            case 2: // R
                sect.l = true;
                prevRoom = new Vector2(currentPos.x - 1f, currentPos.y);
                mapObjs[prevRoom].r = true;
                mapObjs[prevRoom].UpdateSect();
                break;
            case 3: // B
                sect.t = true;
                prevRoom = new Vector2(currentPos.x, currentPos.y + 1f);
                mapObjs[prevRoom].b = true;
                mapObjs[prevRoom].UpdateSect();
                break;
            case 4: // L
                sect.r = true;
                prevRoom = new Vector2(currentPos.x + 1f, currentPos.y);
                mapObjs[prevRoom].l = true;
                mapObjs[prevRoom].UpdateSect();
                break;
            default:
                break;
        }
        sect.UpdateSect();

        mapObjs.Add(currentPos, sect);
        currentMap.Add(sect.gameObject);
        UpdatePlayer(currentPos);
    }

    public void UpdatePlayer(Vector2 currentPos) // Updates player icon position
    {
        playerPos.transform.localPosition = CalculatePos(currentPos);
    }

    public void ClearMinimap()
    {
        playerPos.transform.localPosition = Vector2.zero; // Reset player position
        mapObjs.Clear();
        foreach (GameObject m in currentMap) // Destroy all minimap rooms
        {
            Destroy(m);
        }
        currentMap.Clear(); // Clear list
    }
}
