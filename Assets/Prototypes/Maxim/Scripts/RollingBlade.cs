using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingBlade : MonoBehaviour
{
    [SerializeField]
    private GameObject _blade;
    [SerializeField]
    private Transform _waypointStart;
    [SerializeField]
    private Transform _waypointEnd;

    private float _bladeSpeed = 5;
    private Vector3 _destination;
    private bool _changeDirection = false;

    private void Start()
    {
        _destination = _waypointEnd.position;
    }

    void Update()
    {
        MoveToWaypoint();
    }

    private void MoveToWaypoint()
    {
        _blade.transform.position = Vector3.MoveTowards(_blade.transform.position, _destination, _bladeSpeed * Time.deltaTime);

        if (Vector3.Distance(_blade.transform.position, _destination) < 0.2)
        {
            _changeDirection = true;
        }

        if (_changeDirection)
        {
            ChangeDestination(_destination);
            _changeDirection = false;
        }
    }

    private void ChangeDestination(Vector3 destination)
    {
        if (destination == _waypointStart.position)
        {
            _destination = _waypointEnd.position;
        }
        else
        {
            _destination = _waypointStart.position;
        }
    }
}
