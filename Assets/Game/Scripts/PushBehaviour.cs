using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBehaviour : MonoBehaviour
{
    private InputController _inputController;
    private int _controllerID;
    private bool _bButton = false;

    private float _timer;

    [SerializeField]
    private float _pushingDelay = 1;

    [SerializeField]
    private float _pushingPower = 5;

    private PlayerBehaviour _playerBehaviour;
    private void Start()
    {
        _inputController = (InputController)FindObjectOfType(typeof(InputController));
        
        _playerBehaviour = GetComponentInParent<PlayerBehaviour>();
    }
    private void Update()
    {
        _controllerID = GetComponentInParent<PlayerControl>().controllerID;
        _bButton = _inputController.BButton(_controllerID);
        _timer += Time.deltaTime;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();

            if (_bButton && _timer > _pushingDelay)
            {
                _timer = 0;
                rb.AddForce(transform.forward * _pushingPower * _playerBehaviour.PlayerPower, ForceMode.Impulse);
            }
        }
    }
}
