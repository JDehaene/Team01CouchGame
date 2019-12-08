using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public GameObject[] SpawnRooms;
    public GameObject[] EndRooms;
    public GameObject[] HorizontalRooms;
    public GameObject[] LWayRooms;
    public GameObject[] Doors;

    public List<Vector3> _roomPositionList = new List<Vector3>();
    private List<GameObject> _roomList = new List<GameObject>();

    [SerializeField]
    public int NumberOfRooms = 5;

    //private int _currentNumberOfRooms = 5;

    private float _offset = 31.5f;

    private Vector3 _position = Vector3.zero;
    private Vector3 _lastPosition = Vector3.zero;

    private GameObject _room;
    private GameObject _doorway;

    private int _randomRoomIndex = 0;
    public int CameraIndex = 0;

    private GameObject[] _activePlayers;
    [SerializeField] private int _roomsPerPlayer;

    private void Start()
    {
        _activePlayers = GameObject.FindGameObjectsWithTag("Player");
        NumberOfRooms = _activePlayers.Length * _roomsPerPlayer;

        if(_activePlayers.Length == 1)
        {
            Debug.Log("LastPlayer remaining");
        }

        GeneratePositions();
        AddRooms();
    }

    //private void Update()
    //{
    //    if (_currentNumberOfRooms != _numberOfRooms)
    //    {
    //        for (int i = 0; i < _roomPositionList.Count; i++)
    //        {
    //            _roomPositionList.Remove(_roomPositionList[i]);
    //        }

    //        foreach (GameObject room in _roomList)
    //        {
    //            Destroy(room.gameObject);
    //        }

    //        _roomPositionList.Clear();
    //        _roomList.Clear();

    //        this.transform.position = Vector3.zero;

    //        GeneratePositions();
    //        AddRooms();

    //        _currentNumberOfRooms = _numberOfRooms;
    //    }
    //}

    private void AddRooms()
    {
        Vector3 previousPosition = Vector3.zero;
        Vector3 nextPosition = _roomPositionList[1];
        Vector3 position = _roomPositionList[0];

        Vector3 previousPositionCalculation;
        Vector3 nextPositionCalculation;

        _randomRoomIndex = UnityEngine.Random.Range(0, SpawnRooms.Length);

        if (nextPosition == Vector3.left * _offset)
        {
            _room = Instantiate(SpawnRooms[_randomRoomIndex], position, Quaternion.identity);
            _doorway = Instantiate(Doors[0], Vector3.left * (_offset / 2), Quaternion.Euler(0, 180, 0));
            _doorway.transform.SetParent(this.transform);
        }
        else if (nextPosition == Vector3.right * _offset)
        {
            _room = Instantiate(SpawnRooms[_randomRoomIndex], position, Quaternion.Euler(0, 180, 0));
            _doorway = Instantiate(Doors[0], Vector3.right * (_offset / 2), Quaternion.identity);
            _doorway.transform.SetParent(this.transform);
        }
        else if (nextPosition == Vector3.forward * _offset)
        {
            _room = Instantiate(SpawnRooms[_randomRoomIndex], position, Quaternion.Euler(0, 90, 0));
            _doorway = Instantiate(Doors[0], Vector3.forward * (_offset / 2), Quaternion.Euler(0, -90, 0));
            _doorway.transform.SetParent(this.transform);
        }
        else if (nextPosition == Vector3.back * _offset)
        {
            _room = Instantiate(SpawnRooms[_randomRoomIndex], position, Quaternion.Euler(0, -90, 0));
            _doorway = Instantiate(Doors[0], Vector3.back * (_offset / 2), Quaternion.Euler(0, 90, 0));
            _doorway.transform.SetParent(this.transform);
        }

        _roomList.Add(_room);

        for (int positionIndex = 1; positionIndex < _roomPositionList.Count - 1; positionIndex++)
        {
            position = _roomPositionList[positionIndex];
            previousPosition = _roomPositionList[positionIndex - 1];
            nextPosition = _roomPositionList[positionIndex + 1];

            previousPositionCalculation = previousPosition - position;
            nextPositionCalculation = nextPosition - position;

            //links rechts
            if (previousPositionCalculation == Vector3.left * _offset && nextPositionCalculation == Vector3.right * _offset)
            {
                _randomRoomIndex = UnityEngine.Random.Range(0, HorizontalRooms.Length);
                _room = Instantiate(HorizontalRooms[_randomRoomIndex], position, Quaternion.identity);
                _doorway = Instantiate(Doors[0], position + Vector3.right * (_offset / 2), Quaternion.identity);
                _doorway.transform.SetParent(this.transform);
            }
            else if (previousPositionCalculation == Vector3.right * _offset && nextPositionCalculation == Vector3.left * _offset)
            {
                _randomRoomIndex = UnityEngine.Random.Range(0, HorizontalRooms.Length);
                _room = Instantiate(HorizontalRooms[_randomRoomIndex], position, Quaternion.identity);
                _doorway = Instantiate(Doors[0], position + Vector3.left * (_offset / 2), Quaternion.Euler(0, 180, 0));
                _doorway.transform.SetParent(this.transform);
            }
            //boven onder
            else if (previousPositionCalculation == Vector3.forward * _offset && nextPositionCalculation == Vector3.back * _offset)
            {
                _randomRoomIndex = UnityEngine.Random.Range(0, HorizontalRooms.Length);
                _room = Instantiate(HorizontalRooms[_randomRoomIndex], position, Quaternion.Euler(0, 90, 0));
                _doorway = Instantiate(Doors[0], position + Vector3.back * (_offset / 2), Quaternion.Euler(0, 90, 0));
                _doorway.transform.SetParent(this.transform);
            }
            else if (previousPositionCalculation == Vector3.back * _offset && nextPositionCalculation == Vector3.forward * _offset)
            {
                _randomRoomIndex = UnityEngine.Random.Range(0, HorizontalRooms.Length);
                _room = Instantiate(HorizontalRooms[_randomRoomIndex], position, Quaternion.Euler(0, 90, 0));
                _doorway = Instantiate(Doors[0], position + Vector3.forward * (_offset / 2), Quaternion.Euler(0, -90, 0));
                _doorway.transform.SetParent(this.transform);
            }
            //links boven
            else if (previousPositionCalculation == Vector3.left * _offset && nextPositionCalculation == Vector3.forward * _offset)
            {
                _randomRoomIndex = UnityEngine.Random.Range(0, LWayRooms.Length);
                _room = Instantiate(LWayRooms[_randomRoomIndex], position, Quaternion.identity);
                _doorway = Instantiate(Doors[0], position + Vector3.forward * (_offset / 2), Quaternion.Euler(0, -90, 0));
                _doorway.transform.SetParent(this.transform);
            }
            else if (previousPositionCalculation == Vector3.forward * _offset && nextPositionCalculation == Vector3.left * _offset)
            {
                _randomRoomIndex = UnityEngine.Random.Range(0, LWayRooms.Length);
                _room = Instantiate(LWayRooms[_randomRoomIndex], position, Quaternion.identity);
                _doorway = Instantiate(Doors[0], position + Vector3.left * (_offset / 2), Quaternion.Euler(0, 180, 0));
                _doorway.transform.SetParent(this.transform);
            }
            //links onder
            else if (previousPositionCalculation == Vector3.left * _offset && nextPositionCalculation == Vector3.back * _offset)
            {
                _randomRoomIndex = UnityEngine.Random.Range(0, LWayRooms.Length);
                _room = Instantiate(LWayRooms[_randomRoomIndex], position, Quaternion.Euler(0, -90, 0));
                _doorway = Instantiate(Doors[0], position + Vector3.back * (_offset / 2), Quaternion.Euler(0, 90, 0));
                _doorway.transform.SetParent(this.transform);
            }
            else if (previousPositionCalculation == Vector3.back * _offset && nextPositionCalculation == Vector3.left * _offset)
            {
                _randomRoomIndex = UnityEngine.Random.Range(0, LWayRooms.Length);
                _room = Instantiate(LWayRooms[_randomRoomIndex], position, Quaternion.Euler(0, -90, 0));
                _doorway = Instantiate(Doors[0], position + Vector3.left * (_offset / 2), Quaternion.Euler(0, 180, 0));
                _doorway.transform.SetParent(this.transform);
            }
            //Rechts boven
            else if (previousPositionCalculation == Vector3.right * _offset && nextPositionCalculation == Vector3.forward * _offset)
            {
                _randomRoomIndex = UnityEngine.Random.Range(0, LWayRooms.Length);
                _room = Instantiate(LWayRooms[_randomRoomIndex], position, Quaternion.Euler(0, 90, 0));
                _doorway = Instantiate(Doors[0], position + Vector3.forward * (_offset / 2), Quaternion.Euler(0, -90, 0));
                _doorway.transform.SetParent(this.transform);
            }
            else if (previousPositionCalculation == Vector3.forward * _offset && nextPositionCalculation == Vector3.right * _offset)
            {
                _randomRoomIndex = UnityEngine.Random.Range(0, LWayRooms.Length);
                _room = Instantiate(LWayRooms[_randomRoomIndex], position, Quaternion.Euler(0, 90, 0));
                _doorway = Instantiate(Doors[0], position + Vector3.right * (_offset / 2), Quaternion.identity);
                _doorway.transform.SetParent(this.transform);
            }
            //Rechts onder
            else if (previousPositionCalculation == Vector3.right * _offset && nextPositionCalculation == Vector3.back * _offset)
            {
                _randomRoomIndex = UnityEngine.Random.Range(0, LWayRooms.Length);
                _room = Instantiate(LWayRooms[_randomRoomIndex], position, Quaternion.Euler(0, 180, 0));
                _doorway = Instantiate(Doors[0], position + Vector3.back * (_offset / 2), Quaternion.Euler(0, 90, 0));
                _doorway.transform.SetParent(this.transform);
            }
            else if (previousPositionCalculation == Vector3.back * _offset && nextPositionCalculation == Vector3.right * _offset)
            {
                _randomRoomIndex = UnityEngine.Random.Range(0, LWayRooms.Length);
                _room = Instantiate(LWayRooms[_randomRoomIndex], position, Quaternion.Euler(0, 180, 0));
                _doorway = Instantiate(Doors[0], position + Vector3.right * (_offset / 2), Quaternion.identity);
                _doorway.transform.SetParent(this.transform);
            }

            _roomList.Add(_room);
        }

        previousPosition = _roomPositionList[_roomPositionList.Count - 2];
        position = _roomPositionList[_roomPositionList.Count - 1];

        previousPositionCalculation = previousPosition - position;

        _randomRoomIndex = UnityEngine.Random.Range(0, EndRooms.Length);

        if (previousPositionCalculation == Vector3.left * _offset)
        {
            _room = Instantiate(EndRooms[_randomRoomIndex], position, Quaternion.identity);
        }
        else if (previousPositionCalculation == Vector3.right * _offset)
        {
            _room = Instantiate(EndRooms[_randomRoomIndex], position, Quaternion.Euler(0, 180, 0));
        }
        else if (previousPositionCalculation == Vector3.forward * _offset)
        {
            _room = Instantiate(EndRooms[_randomRoomIndex], position, Quaternion.Euler(0, 90, 0));
        }
        else if (previousPositionCalculation == Vector3.back * _offset)
        {
            _room = Instantiate(EndRooms[_randomRoomIndex], position, Quaternion.Euler(0, -90, 0));
        }

        _roomList.Add(_room);
    }

    private void GeneratePositions()
    {
        bool positionsGenerated = false;
        while (!positionsGenerated)
        {
            bool hasOverLap = false;
            _position = Vector3.zero;
            _lastPosition = Vector3.zero;
            //add the spawn position
            _roomPositionList.Add(_position);

            //add the other rooms position, including the end room
            for (int roomIndex = 0; roomIndex < NumberOfRooms; roomIndex++)
            {
                //do this calculation while the room position already exists
                int randomDirection = UnityEngine.Random.Range(0, 4);
                int counter = 0;
                do
                {
                    ++counter;
                    hasOverLap = false;
                    switch (randomDirection)
                    {
                        case 0:
                            _position = _lastPosition + Vector3.left * _offset;
                            if (IsOverlapping())
                            {
                                hasOverLap = true;
                                ++randomDirection;
                            }
                            break;
                        case 1:
                            _position = _lastPosition + Vector3.forward * _offset;
                            if (IsOverlapping())
                            {
                                hasOverLap = true;
                                ++randomDirection;
                            }
                            break;
                        case 2:
                            _position = _lastPosition + Vector3.right * _offset;
                            if (IsOverlapping())
                            {
                                hasOverLap = true;
                                ++randomDirection;
                            }
                            break;
                        case 3:
                            _position = _lastPosition + Vector3.back * _offset;
                            if (IsOverlapping())
                            {
                                hasOverLap = true;
                                randomDirection = 0;
                            }
                            break;
                    }
                } while (hasOverLap && counter >= 4);

                if (hasOverLap)
                {
                _roomPositionList.Clear();
                    break;
                }

                _roomPositionList.Add(_position);

                _lastPosition = _position;
            }
            if (!hasOverLap)
            {
                positionsGenerated = true;
            }
        }
    }
    bool IsOverlapping()
    {
        //Check if the roomposition already exists

        for (int positionIndex = 0; positionIndex < _roomPositionList.Count; positionIndex++)
        {
            if (_roomPositionList[positionIndex] == _position)
            {
                return true;
            }
        }
        return false;
    }
}
