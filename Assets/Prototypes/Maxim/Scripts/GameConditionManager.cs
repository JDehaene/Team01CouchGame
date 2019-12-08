using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameConditionManager : MonoBehaviour
{
    private GameObject[] _activePlayers;
    private Sacrifice _sacrificeBehaviour;
    private void Update()
    {
        CheckPlayersAlive();
    }

    private void CheckPlayersAlive()
    {
        _activePlayers = GameObject.FindGameObjectsWithTag("Player");

        if (_activePlayers.Length < 1)
            EndGame();
    }
    private void CheckSacrifice()
    {

    }
    private void EndGame()
    {
        SceneManager.LoadScene("CharacterSelection");
    }
}
