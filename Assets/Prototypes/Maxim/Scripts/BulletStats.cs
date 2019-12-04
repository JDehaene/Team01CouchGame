using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletStats : MonoBehaviour
{
    public float BulletSpeed, StartBulletDamage;
    [HideInInspector] public float CurrentBulletDamage;
    [HideInInspector] public bool IsHostile = false;

    public void BulletPower(float playerPower, bool ishostile)
    {
        CurrentBulletDamage = StartBulletDamage;
        CurrentBulletDamage = CurrentBulletDamage * playerPower;
        IsHostile = ishostile;
    }
    

}
