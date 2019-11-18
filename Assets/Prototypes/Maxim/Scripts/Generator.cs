﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public GameObject[] Rooms;

    public List<Vector3> _roomPositionList = new List<Vector3>();
    private List<GameObject> _roomList = new List<GameObject>();

    [SerializeField]
    private int _numberOfRooms = 5;

    private int _currentNumberOfRooms = 5;

    private float _offset = 31.5f;

    private Vector3 _position = Vector3.zero;
    private Vector3 _lastPosition = Vector3.zero;

    private GameObject _room;

    private void Start()
    {
        GeneratePositions();
        AddRooms();
    }

    private void Update()
    {
        if (_currentNumberOfRooms != _numberOfRooms)
        {
            for (int i = 0; i < _roomPositionList.Count; i++)
            {
                _roomPositionList.Remove(_roomPositionList[i]);
            }

            foreach (GameObject room in _roomList)
            {
                Destroy(room.gameObject);
            }

            _roomPositionList.Clear();
            _roomList.Clear();

            this.transform.position = Vector3.zero;

            GeneratePositions();
            AddRooms();

            _currentNumberOfRooms = _numberOfRooms;
        }
    }

    private void AddRooms()
    {
        Vector3 previousPosition = Vector3.zero;
        Vector3 nextPosition = _roomPositionList[1];
        Vector3 position = _roomPositionList[0];

        Vector3 previousPositionCalculation;
        Vector3 nextPositionCalculation;      

        if (nextPosition == Vector3.left * _offset)
        {
            _room = Instantiate(Rooms[2], position, Quaternion.identity);
        }
        else if (nextPosition == Vector3.right * _offset)
        {
            _room = Instantiate(Rooms[2], position, Quaternion.Euler(0, 180, 0));
        }
        else if (nextPosition == Vector3.forward * _offset)
        {
            _room = Instantiate(Rooms[2], position, Quaternion.Euler(0, 90, 0));
        }
        else if (nextPosition == Vector3.back * _offset)
        {
            _room = Instantiate(Rooms[2], position, Quaternion.Euler(0, -90, 0));
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
            if (previousPositionCalculation == Vector3.left * _offset && nextPositionCalculation == Vector3.right * _offset
                || previousPositionCalculation == Vector3.right * _offset && nextPositionCalculation == Vector3.left * _offset)
            {
                _room = Instantiate(Rooms[1], position, Quaternion.identity);
            }
            //boven onder
            else if (previousPositionCalculation == Vector3.forward * _offset && nextPositionCalculation == Vector3.back * _offset
                || previousPositionCalculation == Vector3.back * _offset && nextPositionCalculation == Vector3.forward * _offset)
            {
                _room = Instantiate(Rooms[1], position, Quaternion.Euler(0, 90, 0));
            }
            //links boven
            else if (previousPositionCalculation == Vector3.left * _offset && nextPositionCalculation == Vector3.forward * _offset
                || previousPositionCalculation == Vector3.forward * _offset && nextPositionCalculation == Vector3.left * _offset)
            {
                _room = Instantiate(Rooms[0], position, Quaternion.identity);
            }
            //links onder
            else if (previousPositionCalculation == Vector3.left * _offset && nextPositionCalculation == Vector3.back * _offset
                || previousPositionCalculation == Vector3.back * _offset && nextPositionCalculation == Vector3.left * _offset)
            {
                _room = Instantiate(Rooms[0], position, Quaternion.Euler(0, -90, 0));
            }
            //Rechts boven
            else if (previousPositionCalculation == Vector3.right * _offset && nextPositionCalculation == Vector3.forward * _offset
                || previousPositionCalculation == Vector3.forward * _offset && nextPositionCalculation == Vector3.right * _offset)
            {
                _room = Instantiate(Rooms[0], position, Quaternion.Euler(0, 90, 0));
            }
            //Rechts onder
            else if (previousPositionCalculation == Vector3.right * _offset && nextPositionCalculation == Vector3.back * _offset
                || previousPositionCalculation == Vector3.back * _offset && nextPositionCalculation == Vector3.right * _offset)
            {
                _room = Instantiate(Rooms[0], position, Quaternion.Euler(0, 180, 0));
            }

            _roomList.Add(_room);
        }

        previousPosition = _roomPositionList[_roomPositionList.Count - 2];
        position = _roomPositionList[_roomPositionList.Count - 1];

        previousPositionCalculation = previousPosition - position;

        if (previousPositionCalculation == Vector3.left * _offset)
        {
            _room = Instantiate(Rooms[3], position, Quaternion.identity);
        }
        else if (previousPositionCalculation == Vector3.right * _offset)
        {
            _room = Instantiate(Rooms[3], position, Quaternion.Euler(0, 180, 0));
        }
        else if (previousPositionCalculation == Vector3.forward * _offset)
        {
            _room = Instantiate(Rooms[3], position, Quaternion.Euler(0, 90, 0));
        }
        else if (previousPositionCalculation == Vector3.back * _offset)
        {
            _room = Instantiate(Rooms[3], position, Quaternion.Euler(0, -90, 0));
        }

        _roomList.Add(_room);
    }

    private void GeneratePositions()
    {
        int overlappingRooms;
        _position = Vector3.zero;
        _lastPosition = Vector3.zero;
        //add the spawn position
        _roomPositionList.Add(_position);

        Debug.Log(_position + " added to list");

        //add the other rooms position, including the end room
        for (int roomIndex = 0; roomIndex < _numberOfRooms; roomIndex++)
        {
            //do this calculation while the room position already exists
            do
            {
                overlappingRooms = 0;

                int randomDirection = UnityEngine.Random.Range(0, 4);

                switch (randomDirection)
                {
                    case 0:
                        _position = _lastPosition + Vector3.left * _offset;
                        break;
                    case 1:
                        _position = _lastPosition + Vector3.forward * _offset;
                        break;
                    case 2:
                        _position = _lastPosition + Vector3.right * _offset;
                        break;
                    case 3:
                        _position = _lastPosition + Vector3.back * _offset;
                        break;
                }

                //Check if the roomposition already exists
                for (int positionIndex = 0; positionIndex < _roomPositionList.Count; positionIndex++)
                {
                    if (_roomPositionList[positionIndex] == _position)
                    {
                        overlappingRooms++;
                    }
                }
            } while (overlappingRooms > 0);

            _roomPositionList.Add(_position);

            _lastPosition = _position;

            Debug.Log(_position + " added to list");
        }
    }
}