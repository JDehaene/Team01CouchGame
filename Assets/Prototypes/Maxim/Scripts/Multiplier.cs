using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multiplier : MonoBehaviour
{
    private float _multiplyNumber = 1;

    public float MultiplyNumber { get => _multiplyNumber; set => _multiplyNumber = value; }
    public float StartPlayerCount { get; set; }

    private float _timer;
    private bool _gameStarted = false;

    private int _changeStatsTimer;

    public bool GameStarted { set => _gameStarted = value; }
    public bool MutliplierChanged { get; set; }
    void Update()
    {
        if (_gameStarted)
        {
            _timer += Time.deltaTime;

            if (_timer >= _changeStatsTimer)
            {
                _timer = 0;
                _multiplyNumber += 0.1f;
                MutliplierChanged = true;
                Debug.Log("Stats enemies increased with a multiplier of: " + _multiplyNumber);
            }
        }
    }

    public void SetBeginMultiplier()
    {
        switch (StartPlayerCount)
        {
            case 2:
                _changeStatsTimer = 15;
                _multiplyNumber = 1;
                break;
            case 3:
                _changeStatsTimer = 30;
                _multiplyNumber = 1.2f;
                break;
            case 4:
                _changeStatsTimer = 45;
                _multiplyNumber = 1.4f;
                break;
            default:
                _changeStatsTimer = 15;
                _multiplyNumber = 1;
                break;
        }
        Debug.Log(StartPlayerCount + " players started the game, the multiplier is set to " + _multiplyNumber);
    }
}
