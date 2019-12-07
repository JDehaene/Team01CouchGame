using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    private float _timer;
    [SerializeField]
    private float _interval = 2;
    [SerializeField]
    private float _spikeSpeed = 5;
    private Vector3 _scaleOut;
    private Vector3 _scaleIn;
    private Vector3 _scale;

    [SerializeField]
    private float _damage = 1;

    private BoxCollider _collider;

    private void Start()
    {
        _scaleOut = this.transform.localScale;
        _scaleIn = new Vector3(_scaleOut.x, 0, _scaleOut.z);
        _scale = _scaleIn;
        _collider = GetComponent<BoxCollider>();
        _collider.enabled = false;
    }

    void Update()
    {
        _timer += Time.deltaTime;

        this.transform.localScale = Vector3.Lerp(this.transform.localScale, _scale, _spikeSpeed * Time.deltaTime);

        if (_timer >= _interval)
        {
            ChangeScale(_scale);
            _timer -= _interval;
        }
    }

    private void ChangeScale(Vector3 scale)
    {
        if (scale == _scaleIn)
        {
            _scale = _scaleOut;
            _collider.enabled = true;
        }
        else
        {
            _scale = _scaleIn;
            _collider.enabled = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerBehaviour _playerBehaviour = other.GetComponent<PlayerBehaviour>();
            _playerBehaviour.PlayerTakesDamage(_damage);
        }
    }
}
