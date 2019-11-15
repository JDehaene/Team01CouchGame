using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    private Camera _camera;

    private int _roomIndex = 0;
    private bool _goToMyRoom = false;
    private bool _startTimer = false;

    private float _timer;

    void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (_startTimer)
        {
            _timer += Time.deltaTime;

            if (_timer >= 3)
            {
                foreach (Collider collider in GetComponents<Collider>())
                {
                    collider.enabled = true;
                }

                _timer -= 3;
            }
        }
    }

    void LateUpdate()
    {
        if (_goToMyRoom)
        {
            _startTimer = true;

            _camera.transform.position =  Vector3.Lerp(_camera.transform.position, transform.parent.transform.position + new Vector3(0, 13, -8), 5 * Time.deltaTime);

            if (Vector3.Distance(_camera.transform.position, transform.parent.transform.position + new Vector3(0, 13, -8)) < 0.25f)
            {
                _goToMyRoom = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _goToMyRoom = true;           
        }
    }
}
