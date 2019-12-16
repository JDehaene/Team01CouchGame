﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameConditionManager : MonoBehaviour
{
    private GameObject[] _activePlayers;
    private GameObject[] _activeGhosts;
    private Sacrifice _sacrificeBehaviour;
    [SerializeField]private Generator _generator;
    [SerializeField] private Text _winnerText;
    private float _timer;
    [SerializeField] private float _restartGameTimer;


    private void Update()
    {
        _activePlayers = GameObject.FindGameObjectsWithTag("Player");
        _activeGhosts = GameObject.FindGameObjectsWithTag("Ghost");

        if (_generator.CameraIndex == _generator.NumberOfRooms || _activePlayers.Length < 1 && _generator.CameraIndex < _generator.NumberOfRooms)        
            CheckPlayersAlive();

    }

    private void CheckPlayersAlive()
    {
        if (_activePlayers.Length < 1)
        {
            _timer += Time.deltaTime;
            Debug.Log(_timer);
            if(_timer < _restartGameTimer)
                CheckEndFight();
            if(_timer > _restartGameTimer)
                EndGame();

        }
    }
    
    private void CheckEndFight()
    {
        if (_activePlayers.Length > _activeGhosts.Length)
            _winnerText.text = "Player wins!";
        else
        {
            _winnerText.text = "Game Over";

        }
    }

    private void EndGame()
    {
        //for(int i = 0; i < _activeGhosts.Length; ++i)
        //{
        //    Destroy(_activeGhosts[i].GetComponent<DontDestroyOnLoad>());
        //}
        SceneManager.LoadScene("CharacterSelection");
    }
}
