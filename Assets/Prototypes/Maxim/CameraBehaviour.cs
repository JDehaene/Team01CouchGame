using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    private Camera _camera;
    private DungeonGenerator _dungeonGenerator;

    private int _roomIndex = 0;
    private bool _goNextRoom = false;

    void Start()
    {
        _camera = Camera.main;
        _dungeonGenerator = transform.GetChild(1).GetComponent<DungeonGenerator>();
    }

    void LateUpdate()
    {
        if (_goNextRoom)
        {
            GameObject waypoint = _dungeonGenerator.RoomList[_roomIndex].gameObject;
            Vector3.Lerp(_camera.transform.position, waypoint.transform.position, 5 * Time.deltaTime);

            if (Vector3.Distance(_camera.transform.position, waypoint.transform.position) < 0.25f)
            {
                _goNextRoom = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _goNextRoom = true;
            _roomIndex++;
        }
    }
}
