using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletStats : MonoBehaviour
{
    public float BulletSpeed, StartBulletDamage;
    [HideInInspector] public float CurrentBulletDamage;
    [HideInInspector] public bool IsGhost = false;

    public void BulletPower(float playerPower, bool isghost)
    {
        CurrentBulletDamage = StartBulletDamage;
        CurrentBulletDamage = CurrentBulletDamage * playerPower;
        IsGhost = isghost;
    }
    

}
