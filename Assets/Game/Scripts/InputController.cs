﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private bool _aButton;
    private bool _bButton;

    private float _horizontalL;
    private float _horizontalR;
    private float _verticalL;
    private float _verticalR;
    private float _rightTrigger;

    public bool AButton(int _playerID)
    {
        _aButton = Input.GetButtonDown("AButtonP" + _playerID);
        return _aButton;
    }
    public bool BButton(int _playerID)
    {
        _bButton = Input.GetButtonDown("BButtonP"+ _playerID);
        return _bButton;
    }
    public bool PauseButton(int _playerID)
    {
        _bButton = Input.GetButtonDown("BButtonP" + _playerID);
        return _bButton;
    }
    public float LeftStickHorizontal(int _playerID)
    {
        _horizontalL = Input.GetAxisRaw("HorizontalLP" + _playerID);
        return _horizontalL;
    }
    public float LeftStickVertical(int _playerID)
    {
        _verticalL = Input.GetAxisRaw("VerticalLP" + _playerID);
        return _verticalL;
    }
    public float RightTrigger(int _playerID)
    {
        _rightTrigger = Input.GetAxis("RightTriggerP" + _playerID);
        return _rightTrigger;
    }
    public float RightStickHorizontal(int _playerID)
    {
        _horizontalR = Input.GetAxisRaw("HorizontalRP" + _playerID);
        return _horizontalR;
    }
    public float RightStickVertical(int _playerID)
    {
        _verticalR = Input.GetAxisRaw("VerticalRP" + _playerID);
        return _verticalR;
    }

}
