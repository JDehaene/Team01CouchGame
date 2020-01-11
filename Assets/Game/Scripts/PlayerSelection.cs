using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerSelection : MonoBehaviour
{
    public PlayerControl[] _players;
    private List<int> _activePlayers = new List<int>();

    [SerializeField] Multiplier _multiplierScript;
    [SerializeField] private GameObject _bannerPlayersJoined;
    [SerializeField] private float maxTime;
    [SerializeField] private Text _txtCounter;
    [SerializeField] private Text _playersJoined;
    [SerializeField] private StartGameBehaviour _playersEntered;
    [SerializeField] private GameObject _bannerTimer;

    private bool isCounting = false;
    private float counter = 0;
    private int _playersNeededToStart;

    private void Update()
    {
        CheckInput();
        _playersNeededToStart = _activePlayers.Count;
        UpdateUI();
    }

    public void CheckCounter()
    {
        if (isCounting)
        {
            if (_playersNeededToStart < 2)
            {
                counter = 0;
                isCounting = false;
                _bannerTimer.SetActive(false);
                
                return;
            }

            counter += Time.deltaTime;
            _txtCounter.text = "Time left until transportation: " + (int)(maxTime - counter);

            if (counter >= maxTime)
            {
                this.enabled = false;
                _txtCounter.text = "Start";
                _multiplierScript.StartPlayerCount = _activePlayers.Count;
                _multiplierScript.SetBeginMultiplier();
                _multiplierScript.GameStarted = true;
                RemoveNonPlayedChars();
                StartGame();
            }
        }
        else if (_activePlayers.Count > 1)
        {
            counter = 0;
            isCounting = true;
            _bannerTimer.SetActive(true);
        }
    }
    private void UpdateUI()
    {
        if(_playersEntered._playersJoined == 0)
        {
            _playersJoined.text = "Nobody has entered the portal";
        }
        if(_playersEntered._playersJoined == 1)
        {
            _playersJoined.text = _playersEntered._playersJoined + " player has entered the portal";
        }
        if (_playersEntered._playersJoined > 1)
        {
            _playersJoined.text = _playersEntered._playersJoined + " players have entered the portal";
        }

    }

    void CheckInput()
    {
        //to join
        for (int i = 1; i < _players.Length + 1; ++i)
        {
            if (!_activePlayers.Contains(i) && Input.GetButtonDown("AButtonP" + i))
            {
                _activePlayers.Add(i);
                _players[GetCharacter()].Active(i);
            }
        }

        //to back out
        for (int i = 1; i < 5; ++i)
        {
            if (_activePlayers.Contains(i) && Input.GetButtonDown("BButtonP" + i))
            {
                _activePlayers.Remove(i);
                DeactiveCharacter(i);
            }
        }
    }
    int GetCharacter()
    {
        for (int i = 0; i < _players.Length; ++i)
        {
            if (!_players[i].enabled)
            {
                return i;
            }
        }
        return 0;
    }

    void DeactiveCharacter(int ctrl)
    {
        for (int i = 0; i < _players.Length; ++i)
        {
            if (_players[i].enabled && _players[i].controllerID.Equals(ctrl))
            {
                _players[i].DeActive();
                return;
            }
        }
    }
    void RemoveNonPlayedChars()
    {
        foreach(PlayerControl _player in _players)
        {
            if (!_player._lockedIn)
                Destroy(_player.gameObject);
        }
    }
    void StartGame()
    {
        SceneManager.LoadScene("TestSceneMaxim");
    }

}
