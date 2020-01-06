using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayerControl : MonoBehaviour
{
    //public int playerID = 1;
    public int controllerID = 1;
    public bool _lockedIn = false;

    [SerializeField] private Text _txtInfo;
    [SerializeField] private GameObject _joinButton;
    [SerializeField] private Material[] _buttonMaterial;
    private void Start()
    {
        
    }
    public void Active(int ctrl)
    {
        this.enabled = true;
        _lockedIn = true;
        controllerID = ctrl;
        _joinButton.GetComponent<Renderer>().material = _buttonMaterial[1];
        this.gameObject.AddComponent<DontDestroyOnLoad>();

        _txtInfo.text = "'B' to back out";

    }

    public void DeActive()
    {
        Destroy(this.gameObject.GetComponent<DontDestroyOnLoad>());
        this.enabled = false;
        _lockedIn = false;
        _joinButton.GetComponent<Renderer>().material = _buttonMaterial[0];
        _txtInfo.text = "'A' to join";

    }
    public void StartGame()
    {
        SceneManager.LoadScene("TestSceneMaxim");
    }
}
