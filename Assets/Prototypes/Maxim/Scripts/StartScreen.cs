using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
    [SerializeField] private GameObject _startScreenUI, _mainMenuUI;

    private void Start()
    {
        _mainMenuUI.active = false;
    }

    void Update()
    {
        if(Input.anyKey)
        {
            _mainMenuUI.active = true;
            _startScreenUI.active = false;
        }
    }
}
