using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletStats : MonoBehaviour
{
    public float BulletSpeed, StartBulletDamage;
    [HideInInspector] public float CurrentBulletDamage;
    
    public void BulletPower(float playerPower)
    {
        CurrentBulletDamage = StartBulletDamage;
        CurrentBulletDamage = CurrentBulletDamage * playerPower;
    }
    

}
