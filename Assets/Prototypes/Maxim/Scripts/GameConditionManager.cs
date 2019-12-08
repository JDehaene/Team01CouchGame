using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameConditionManager : MonoBehaviour
{
    private GameObject[] _activePlayers;
    private GameObject[] _activeGhosts;
    private Sacrifice _sacrificeBehaviour;
    private Generator _generator;
    [SerializeField] private Text _winnerText;
    private float _timer;
    [SerializeField] private float _restartGameTimer;

    private void Update()
    {
        CheckPlayersAlive();

        if (_generator.CameraIndex <= _generator.NumberOfRooms)
        {
            CheckEndFight();
        }
    }

    private void CheckPlayersAlive()
    {
        _activePlayers = GameObject.FindGameObjectsWithTag("Player");
        _activeGhosts = GameObject.FindGameObjectsWithTag("Ghost");
        if (_activePlayers.Length < 1)
        {
            _timer += Time.deltaTime;
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
            _winnerText.text = "Ghosts win!";
    }

    private void EndGame()
    {
        SceneManager.LoadScene("CharacterSelection");
    }
}
