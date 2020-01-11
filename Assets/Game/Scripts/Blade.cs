using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
    [SerializeField]
    private float _damage = 1;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerBehaviour _playerBehaviour = other.GetComponent<PlayerBehaviour>();
            _playerBehaviour.PlayerTakesDamage(_damage);
        }
    }
}
