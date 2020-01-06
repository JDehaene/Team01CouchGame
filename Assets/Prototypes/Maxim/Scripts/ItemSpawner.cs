using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _commonPickups;
    [SerializeField] private GameObject[] _unCommonPickups;
    [SerializeField] private GameObject[] _rarePickups;
    [SerializeField] private GameObject[] _ultraRarePickups;
    private int _random;
    private GameObject _randomizedItem;
    private GameObject _itemToSpawn;
    
    private void Start()
    {
        _random = Random.Range(0, 11);
        ChooseDrop();
        _itemToSpawn = Instantiate(_randomizedItem, transform.position, transform.rotation);
        _itemToSpawn.transform.parent = this.gameObject.transform;
    }
    void ChooseDrop()
    {
        switch(_random)
        {
            case 0:
                ChooseCommon();
                break;
            case 1:
                ChooseCommon();
                break;
            case 2:
                ChooseUncommon();
                break;
            case 3:
                ChooseUncommon();
                break;
            case 4:
                ChooseUncommon();
                break;
            case 5:
                ChooseUncommon();
                break;
            case 6:
                ChooseRare();
                break;
            case 7:
                ChooseRare();
                break;
            case 8:
                ChooseRare();
                break;
            case 9:
                ChooseUltrarare();
                break;
            case 10:
                ChooseUltrarare();
                break;
        }
    }
    private void ChooseCommon()
    {
        int randomValue = Random.Range(0, _commonPickups.Length);
        _randomizedItem = _commonPickups[randomValue];
    }
    private void ChooseUncommon()
    {
        int randomValue = Random.Range(0, _unCommonPickups.Length);
        _randomizedItem = _unCommonPickups[randomValue];
    }
    private void ChooseRare()
    {
        int randomValue = Random.Range(0, _rarePickups.Length);
        _randomizedItem = _rarePickups[randomValue];
    }

    private void ChooseUltrarare()
    {
        int randomValue = Random.Range(0, _ultraRarePickups.Length);
        _randomizedItem = _ultraRarePickups[randomValue];
    }
}
