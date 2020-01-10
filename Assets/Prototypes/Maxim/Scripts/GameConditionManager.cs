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
    [SerializeField]private Generator _generator;
    [SerializeField] private Text _winnerText;
    private float _timer;
    [SerializeField] private float _restartGameTimer;
    private GameObject _uiController;

    private float _multiplier = 1;

    public float Multiplier { get => _multiplier; }

    private void Start()
    {
        _activePlayers = GameObject.FindGameObjectsWithTag("Player");
        _uiController = GameObject.FindGameObjectWithTag("PlayerUiController");

        switch (_activePlayers.Length)
        {
            case 1:
                _multiplier = 2.5f;
                break;
            case 2:
                _multiplier = 2;
                break;
            case 3:
                _multiplier = 1.5f;
                break;
            case 4:
                _multiplier = 1;
                break;
            default:
                break;
        }

        Debug.Log("Check amount of players, chose case: " + _activePlayers.Length);
    }

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
        if(_activeGhosts.Length != 0)
        {
            foreach (var item in _activeGhosts)
            {
                Destroy(item);
            }
        }
        _uiController.GetComponent<PlayerUiController>().GameHasBeenRestarted();
        SceneManager.LoadScene("CharacterSelection");
    }

  
}
