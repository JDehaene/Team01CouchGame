using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayerControl : MonoBehaviour
{
    public int playerID = 1;
    public int controllerID = 1;
    public bool _lockedIn = false;

    [SerializeField] private Text _txtInfo;
    
    public void Active(int ctrl)
    {
        this.enabled = true;
        _lockedIn = true;
        controllerID = ctrl;

        _txtInfo.text = "'B' to back out";

        //anim.SetInteger("state", 1);
    }

    public void DeActive()
    {
        this.enabled = false;
        _lockedIn = false;
        //iTex = 0;
        //matDevil.SetTexture("_Main", texs[iTex]);

        _txtInfo.text = "'A' to join";

        //anim.SetInteger("state", 3);
    }
    public void StartGame()
    {
        SceneManager.LoadScene("TestSceneMaxim");
    }
}
