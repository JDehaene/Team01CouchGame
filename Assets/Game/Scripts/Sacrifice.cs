using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Sacrifice : MonoBehaviour
{
    private GameObject[] _activePlayers;
    private GameObject[] _activeGhosts;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {           
            Destroy(other.GetComponent<DontDestroyOnLoad>());
            other.GetComponent<PlayerBehaviour>().PlayerDies();
            Destroy(other.gameObject);
            SceneManager.LoadScene(2);           
        }
        else
        {          
            Destroy(other.gameObject);
        }
    }
}
