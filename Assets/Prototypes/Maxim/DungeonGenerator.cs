using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public List<GameObject> Rooms;

    private int _spacing = 18;

    // Start is called before the first frame update
    void Start()
    {
        Rooms.Add(Instantiate(Rooms[0], new Vector3(0, 0, 0), Quaternion.identity));
        Rooms.Add(Instantiate(Rooms[0], new Vector3(-_spacing, 0, 0), Quaternion.identity));
        Rooms.Add(Instantiate(Rooms[0], new Vector3(_spacing, 0, 0), Quaternion.identity));
        Rooms.Add(Instantiate(Rooms[0], new Vector3(0, 0, _spacing), Quaternion.identity));
        Rooms.Add(Instantiate(Rooms[0], new Vector3(0, 0, -_spacing), Quaternion.identity));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
