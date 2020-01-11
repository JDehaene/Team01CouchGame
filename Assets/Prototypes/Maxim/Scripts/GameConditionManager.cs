using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameConditionManager : MonoBehaviour
{
    [SerializeField] private Text _winnerText;
    [SerializeField] private GameObject _winnerUI,_loserUI;
    [SerializeField] private float _restartGameTimer;
    [SerializeField] private Text _sacrificeText;

    private GameObject[] _activePlayers;
    private GameObject[] _activeGhosts;
    private Sacrifice _sacrificeBehaviour;
    private float _timer;
    private GameObject _uiController;
    private Generator _generatorData;
    [SerializeField]private GameObject _lastRoomBlockade;
    private bool _blockadeFound = false;

    private void Start()
    {
        _uiController = GameObject.FindGameObjectWithTag("PlayerUiController");
        _generatorData = (Generator)FindObjectOfType(typeof(Generator));  
    }

    private void Update()
    {
        _activePlayers = GameObject.FindGameObjectsWithTag("Player");
        _activeGhosts = GameObject.FindGameObjectsWithTag("Ghost");
        FindBlockade();
        
        if (_generatorData.CameraIndex == _generatorData.NumberOfRooms || _activePlayers.Length < 1 && _generatorData.CameraIndex < _generatorData.NumberOfRooms)
        {
            CheckPlayersAlive();
        }
        CheckSacrifice();
    }
    private void CheckSacrifice()
    {
        if (_generatorData.CameraIndex == _generatorData.NumberOfRooms)
        {
            _sacrificeText.gameObject.SetActive(true);
            _sacrificeText.text = "Sacrifice one of yourselves to advance";
        }
        else
            _sacrificeText.gameObject.SetActive(false);
    }
    private void FindBlockade()
    {
            _lastRoomBlockade = GameObject.FindGameObjectWithTag("LastBlockade");
    }
    private void CheckPlayersAlive()
    {
        if (_activePlayers.Length < 1)
        {
            _timer += Time.deltaTime;
            if(_timer < _restartGameTimer)
                CheckEndFight();
            if(_timer > _restartGameTimer)
                if(Input.anyKey)
                {
                    EndGame();
                }

        }
    }
    
    private void CheckEndFight()
    {
        if (_activePlayers.Length > _activeGhosts.Length)
        {
            _winnerUI.SetActive(true);
            _lastRoomBlockade.SetActive(false);
            _winnerText.text = "The strongest of the universe is wizard " + _activePlayers[0].GetComponent<PlayerControl>().controllerID;
        }
        else
        {
            _loserUI.SetActive(true);
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
