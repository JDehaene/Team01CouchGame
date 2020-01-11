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
    [SerializeField]private GameObject _uiController;
    private Generator _generatorData;
    [SerializeField]private GameObject _lastRoomBlockade;
    private bool _blockadeDestroyed = false;
    private UiManager _UI;

    private void Start()
    {
        _uiController = GameObject.FindGameObjectWithTag("PlayerUiController");
        _generatorData = (Generator)FindObjectOfType(typeof(Generator));  
        _UI = (UiManager)FindObjectOfType(typeof(UiManager));
    }

    private void Update()
    {
        _activePlayers = GameObject.FindGameObjectsWithTag("Player");
        _activeGhosts = GameObject.FindGameObjectsWithTag("Ghost");
        FindBlockade();
        CheckSacrifice();
        CheckEndFight();
        CheckPlayersAlive();
        
    }
    private void CheckSacrifice()
    {
        if(_generatorData.CameraIndex == _generatorData.NumberOfRooms && _activePlayers.Length > 1)
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

        if (_lastRoomBlockade != null && _blockadeDestroyed == false && _generatorData.CameraIndex == _generatorData.NumberOfRooms && _activeGhosts.Length < _activePlayers.Length)
        {
            _lastRoomBlockade.SetActive(false);
            _blockadeDestroyed = true;
        }
    }
    private void CheckPlayersAlive()
    {
        {
            if (_activePlayers.Length == 0)
            {
                _timer += Time.deltaTime;
                if(_timer > _restartGameTimer)
                {
                    _loserUI.SetActive(true);
                    if (Input.anyKey)
                    {
                        EndGame();
                    }
                }
            }
        }
    }
    
    private void CheckEndFight()
    {
        if (_generatorData.CameraIndex == _generatorData.NumberOfRooms && _activePlayers.Length > _activeGhosts.Length)
        {
            _winnerUI.SetActive(true);
            
            _winnerText.text = "The strongest of the universe is wizard " + _activePlayers[0].GetComponent<PlayerControl>().controllerID;
        }       
    }

    private void EndGame()
    {
        //if(_activeGhosts.Length != 0)
        //{
        //    foreach (var item in _activeGhosts)
        //    {
        //        Destroy(item);
        //    }
        //}
        _UI.RemoveDDOLS();
        _uiController.GetComponent<PlayerUiController>().GameHasBeenRestarted();
        SceneManager.LoadScene("CharacterSelection");
    }

  
}
