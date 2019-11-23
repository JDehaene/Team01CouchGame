using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    private Camera _camera;

    private Transform _waypointNextRoom;
    private Transform _waypointPreviousRoom;

    private Generator _generator;

    public bool _enteredNextRoom = false;
    public bool _moveTowardsRoom = false;

    void Start()
    {
        _camera = Camera.main;
        _waypointNextRoom = transform.GetChild(0);
        _waypointPreviousRoom = transform.GetChild(1);
        _generator = transform.parent.GetComponentInParent<Generator>();
    }

    void LateUpdate()
    {
        if (_moveTowardsRoom)
        {
            _camera.transform.position = Vector3.Lerp(_camera.transform.position, _generator._roomPositionList[_generator.CameraIndex] + new Vector3(0, 21, -12), 5 * Time.deltaTime);

            if (Vector3.Distance(_camera.transform.position, _generator._roomPositionList[_generator.CameraIndex] + new Vector3(0, 21, -12)) < 0.25f)
            {
                _moveTowardsRoom = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {       
            if (_enteredNextRoom)
            {
                _generator.CameraIndex--;
                _enteredNextRoom = false;
                //set player position to next room entrance
                //support mulitple players / add effects / lerp transform
                other.transform.position = _waypointPreviousRoom.transform.position;
            }
            else
            {
                _generator.CameraIndex++;
                _enteredNextRoom = true;
                //set player position to next room entrance
                //support mulitple players / add effects / lerp transform
                other.transform.position = _waypointNextRoom.transform.position;
            }
            _moveTowardsRoom = true;
        }
    }
}
