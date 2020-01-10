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

    public GameObject bannerTimer;
    private bool isCounting = false;
    public Text txtCounter;
    public float maxTime = 30;
    private float counter = 0;
    private int _playersNeededToStart;

    private void Update()
    {
        CheckInput();
        _playersNeededToStart = _activePlayers.Count;
    }

    public void CheckCounter()
    {
        if (isCounting)
        {
            if (_playersNeededToStart < 2)
            {
                counter = 0;
                isCounting = false;
                bannerTimer.SetActive(false);
                return;
            }

            counter += Time.deltaTime;
            txtCounter.text = "Time Left: " + (int)(maxTime - counter);

            if (counter >= maxTime)
            {
                this.enabled = false;
                txtCounter.text = "Start";
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
            bannerTimer.SetActive(true);
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
