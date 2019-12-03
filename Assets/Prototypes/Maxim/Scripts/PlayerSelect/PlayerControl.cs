using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    public int playerID = 1;
    public int controllerID = 1;

    [SerializeField] private Text _txtInfo;
    void Update()
    {
        
    }
    public void Active(int ctrl)
    {
        this.enabled = true;
        controllerID = ctrl;

        _txtInfo.text = "'B' to back out";

        //anim.SetInteger("state", 1);
    }

    public void DeActive()
    {
        this.enabled = false;
        //iTex = 0;
        //matDevil.SetTexture("_Main", texs[iTex]);

        _txtInfo.text = "'A' to join";

        //anim.SetInteger("state", 3);
    }
}
