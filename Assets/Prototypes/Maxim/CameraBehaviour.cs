using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    private Camera _camera;
    private DungeonGenerator _dungeonGenerator;

    private int _roomIndex = 0;
    private bool _goNextRoom = false;
    private bool _startTimer = false;

    private float _timer;

    void Start()
    {
        _camera = Camera.main;
        _dungeonGenerator = transform.GetChild(0).GetComponent<DungeonGenerator>();
    }

    private void Update()
    {
        if (_startTimer)
        {
            _timer += Time.deltaTime;

            if (_timer >= 3)
            {
                foreach (Collider collider in GetComponents<Collider>())
                {
                    collider.enabled = true;
                }

                _timer -= 3;
            }
        }
    }

    void LateUpdate()
    {
        if (_goNextRoom)
        {
            GameObject waypoint = _dungeonGenerator.RoomList[_roomIndex].transform.GetChild(0).gameObject;
            _camera.transform.position =  Vector3.Lerp(_camera.transform.position, waypoint.transform.position, 5 * Time.deltaTime);

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

            foreach (Collider collider in GetComponents<Collider>())
            {
                collider.enabled = false;
            }

            _startTimer = true;
        }
    }
}
