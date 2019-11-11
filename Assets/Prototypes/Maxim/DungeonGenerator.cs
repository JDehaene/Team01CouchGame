using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public List<GameObject> Rooms;
    private List<Vector3> _positionList = new List<Vector3>();

    public List<GameObject> RoomList = new List<GameObject>();

    private int _spacing = 18;
    private int _maxRooms = 10;
    private int _randomNumber;
    private int _lastNumber = 5;

    private Vector3 _oldPosition = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        _positionList.Add(_oldPosition);

        for (int roomIndex = 0; roomIndex < _maxRooms; roomIndex++)
        {
            //eerste generate (na spawn)
            _randomNumber = UnityEngine.Random.Range(-2, 3);
            Debug.Log("Random Number: " + _randomNumber);

            while (_randomNumber == _lastNumber || _randomNumber == 0)
            {
                _randomNumber = UnityEngine.Random.Range(-2, 3);
                Debug.Log("Random Number: " + _lastNumber);
            }         

            Vector3 position = GenerateRandomPosition(_randomNumber);

            _positionList.Add(position);

            for (int positionIndex = 0; positionIndex < _positionList.Count; positionIndex++)
            {
                if (position + _oldPosition == _positionList[positionIndex])
                {
                    do
                    {
                        //eerste generate (na spawn)
                        _randomNumber = UnityEngine.Random.Range(-2, 3);
                        Debug.Log("Random Number: " + _randomNumber);

                        while (_randomNumber == _lastNumber || _randomNumber == 0)
                        {
                            _randomNumber = UnityEngine.Random.Range(-2, 3);
                            Debug.Log("Random Number: " + _lastNumber);
                        }

                        position = GenerateRandomPosition(_randomNumber);
                    } while (position == _positionList[positionIndex]);
                }

                Debug.Log("List:  n" + positionIndex + ": pos" + position);
            }

            if (_randomNumber < 0)
            {
                _lastNumber = Mathf.Abs(_randomNumber);
            }
            else
            {
                _lastNumber = -_randomNumber;
            }

            Debug.Log("Last Number: " + _lastNumber);

            GameObject room;

            if (roomIndex + 1 == _maxRooms)
            {
                room = Instantiate(Rooms[1], _oldPosition + position, Quaternion.identity);
            }
            else
            {
                room = Instantiate(Rooms[0], _oldPosition + position, Quaternion.identity);
            }

            RoomList.Add(room);
          
            _oldPosition = room.transform.position;

            Debug.Log("New room Position: " + room.transform.position);
            Debug.Log("Old Position: " + _oldPosition);         
        }
    }

    private Vector3 GenerateRandomPosition(int number)
    {
        if (number == -2)
        {
            return new Vector3(-_spacing, 0, 0);
        }
        else if (number == 2)
        {
            return new Vector3(_spacing, 0, 0);
        }
        else if (number == 1)
        {
            return new Vector3(0, 0, _spacing);
        }
        else
        {
            return new Vector3(0, 0, -_spacing);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
