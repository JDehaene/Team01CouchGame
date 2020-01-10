using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{

    private bool _gamePaused = false, _help =false, _options= false, _mainMenu = true;

    [SerializeField] private GameObject _mainMenuScreen ,_pauseScreen, _optionScreen, _helpScreen;

    private int _amountOfDdols;
    private DontDestroyOnLoad[] _ddols;

    private void Start()
    {
        _pauseScreen.active = false;
        _optionScreen.active = false;
        _helpScreen.active = false;
        Time.timeScale = 1.0f;
    }

    void Update()
    {
        
        if (_mainMenuScreen.active)
        {
            _mainMenu = true;
        }
        else
        {
            _mainMenu = false;
        }

        if (!_gamePaused && Input.GetButtonDown("Pause") && !_mainMenu)
        {
            PauseGame();
        }
        else if (_gamePaused && Input.GetButtonDown("Pause") || _gamePaused && Input.GetButtonDown("B") && !_options && !_help && !_mainMenu)
        {
            ResumeGame();
        }

        if (_help && Input.GetButtonDown("B") || _options && Input.GetButtonDown("B"))
        {
            Back();
        }

    }

    public void MainMenu()
    {
        _mainMenu = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1.0f;
        _mainMenuScreen.active = true;
        RemoveDDOLS();
        SceneManager.LoadScene(0);
    }

    public void StartGame()
    {
        _mainMenuScreen.active = false;
        _mainMenu = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1.0f;

        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void RestartGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(1);
    }

    //ui options with canvas overlays
    public void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0.0f;
        _pauseScreen.active = true;
        _gamePaused = true;
        _helpScreen.active = false;
        _optionScreen.active = false;
        _options = false;
        _help = false;
    }

    public void ResumeGame()
    {
        _gamePaused = false;
        _pauseScreen.active = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1.0f;
    }

    public void Options()
    {
        _options = true;
        _mainMenuScreen.active = false;
        _pauseScreen.active = false;
        _optionScreen.active = true;
    }

    public void Help()
    {
        _help = true;
        _mainMenuScreen.active = false;
        _pauseScreen.active = false;
        _helpScreen.active = true;
    }

    private void MainMenuScreen()
    {
        _mainMenuScreen.active = true;
        _helpScreen.active = false;
        _optionScreen.active = false;
    }

    public void Back()
    {
        if(!_gamePaused)
        {
            MainMenuScreen();
        }

        if(_gamePaused)
        {
            PauseGame();
        }

    }
    private void RemoveDDOLS()
    {
        _amountOfDdols = GameObject.FindObjectsOfType(typeof(DontDestroyOnLoad)).Length;
        _ddols = FindObjectsOfType<DontDestroyOnLoad>();

        GameObject sacrifice = Instantiate(new GameObject("sacrifice"), Vector3.zero, Quaternion.identity);
        foreach (var item in _ddols)
        {
            item.gameObject.transform.SetParent(sacrifice.transform);
            Destroy(sacrifice);
        }
    }

}
