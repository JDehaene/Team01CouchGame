﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField]
    private float _velocity = 5;
    [SerializeField]
    private float _turnSpeed = 10;

    private Vector2 _input;
    private float _angle;
    private Quaternion _targetRotation;

    public GameObject Bullet;
    private float _timer;
    [SerializeField]
    private float _reloadTime = 2;

    private Transform _holster;

    private void Start()
    {
        _holster = transform.GetChild(0).GetComponent<Transform>();
    }

    private void Update()
    {
        GetInput();

        if (Mathf.Abs(_input.x) < 0.2 && Mathf.Abs(_input.y) < 0.2) return;

        CalculateDirection();
        Rotate();
        Move();
    }

    private void Move()
    {
        transform.position += transform.forward * _velocity * Time.deltaTime;
    }

    private void Rotate()
    {
        _targetRotation = Quaternion.Euler(0, _angle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, _turnSpeed * Time.deltaTime);
    }

    private void CalculateDirection()
    {
        _angle = Mathf.Atan2(_input.x, _input.y);
        _angle = Mathf.Rad2Deg * _angle;
    }

    private void GetInput()
    {
        _input.x = Input.GetAxisRaw("Horizontal");
        _input.y = Input.GetAxisRaw("Vertical");

        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
        }

        if (Input.GetButtonDown("A"))
        {
            if (_timer <= 0)
            {
                Instantiate(Bullet, _holster.position, transform.rotation);
                _timer = _reloadTime;
            }
        }
    }
}
