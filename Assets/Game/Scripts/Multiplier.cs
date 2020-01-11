using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multiplier : MonoBehaviour
{
    [SerializeField] private int _statsTimer2Players = 15;
    [SerializeField] private int _statsTimer3Players = 30;
    [SerializeField] private int _statsTimer4Players = 45;

    private float _multiplyHp = 1;
    private float _multiplyPower = 1;

    public float StartPlayerCount { get; set; }

    private float _timer;
    private bool _gameStarted = false;
    private int _changeStatsTimer;

    public bool GameStarted { set => _gameStarted = value; }

    private EnemyBehaviour[] _enemyArray;

    void Update()
    {
        if (_gameStarted)
        {
            _timer += Time.deltaTime;

            if (_timer >= _changeStatsTimer)
            {
                _timer = 0;
                _enemyArray = FindObjectsOfType<EnemyBehaviour>();

                foreach (EnemyBehaviour enemy in _enemyArray)
                {
                    enemy.ChangeStats(_multiplyHp, _multiplyPower);
                }

                Debug.Log("Stats enemies increased with a multiplier of: " + _multiplyHp);
            }
        }
    }

    public void SetBeginMultiplier()
    {
        switch (StartPlayerCount)
        {
            case 2:
                _changeStatsTimer = _statsTimer2Players;
                _multiplyHp = 5;
                _multiplyPower = 0.05f;
                break;
            case 3:
                _changeStatsTimer = _statsTimer3Players;
                _multiplyHp = 7.5f;
                _multiplyPower = 0.075f;
                break;
            case 4:
                _changeStatsTimer = _statsTimer4Players;
                _multiplyHp = 10;
                _multiplyPower = 0.2f;
                break;
            default:
                _changeStatsTimer = _statsTimer2Players;
                _multiplyHp = 1;
                _multiplyPower = 0.1f;
                break;
        }
        Debug.Log(StartPlayerCount + " players started the game, the multiplier is set to " + _multiplyHp);
    }
}
