using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sacrifice : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(other.GetComponent<DontDestroyOnLoad>());
            SceneManager.LoadScene(0);
        }
        else
        {          
            Destroy(other.gameObject);
        }
    }
}
