using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _pickups;
    private int _randItem;


    private void Start()
    {
        _randItem = Random.Range(0, _pickups.Length);
        Instantiate(_pickups[_randItem], transform.position, transform.rotation);
    }

}
