using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    private Camera _camera;

    private Transform[] _waypointsNextRoom = new Transform[4];
    private Transform[] _waypointsPreviousRoom = new Transform[4];

    private Transform[] _waypointsGhostsNextRoom = new Transform[3];
    private Transform[] _waypointsGhostsPreviousRoom = new Transform[3];
    private Generator _generator;

    //private PlayerSelection _playerSelection;

    private GameObject[] _activePlayers;
    private PlayerBehaviour[] _playerBehaviours;
    private ghostController[] _ghostBehaviours;
    private GameObject[] _activeGhosts; //biep

    public bool _enteredNextRoom = false;
    public bool _moveCamera = false;

    void Start()
    {
        _camera = Camera.main;
        _generator = transform.parent.GetComponentInParent<Generator>();

        _activePlayers = GameObject.FindGameObjectsWithTag("Player");
        _playerBehaviours = new PlayerBehaviour[_activePlayers.Length];
        _activeGhosts = GameObject.FindGameObjectsWithTag("Ghost");

        for (int i = 0; i < 4; i++)
        {
            _waypointsNextRoom[i] = transform.GetChild(i).transform;
        }

        for (int i = 0; i < 4; i++)
        {
            _waypointsPreviousRoom[i] = transform.GetChild(i + 4).transform;
        }
        for (int i = 0; i < 3; i++)
        {
            _waypointsGhostsNextRoom[i] = transform.GetChild(i + 8).transform;
        }
        for (int i = 0; i < 3; i++)
        {
            _waypointsGhostsPreviousRoom[i] = transform.GetChild(i + 11).transform;
        }

        //Biep
        for (int i = 0; i < _activePlayers.Length; i++)
        {
            _activePlayers[i].transform.position = new Vector3(1 + i, 0, 1 + i);
            _playerBehaviours[i] = _activePlayers[i].GetComponent<PlayerBehaviour>();
        }
        if (_activeGhosts.Length >= 1)
        {
            for (int i = 0; i < _activeGhosts.Length; i++)
            {
                _activeGhosts[i].transform.position = new Vector3(-1 - i, 0, -1 - i);
            }

        }


    }

    void LateUpdate()
    {
        _activePlayers = GameObject.FindGameObjectsWithTag("Player");
        _activeGhosts = GameObject.FindGameObjectsWithTag("Ghost");

        if (_moveCamera)
        {
            _camera.transform.position = Vector3.Lerp(_camera.transform.position, _generator._roomPositionList[_generator.CameraIndex] + new Vector3(0, 21, -12), 5 * Time.deltaTime);

            if (Vector3.Distance(_camera.transform.position, _generator._roomPositionList[_generator.CameraIndex] + new Vector3(0, 21, -12)) < 0.25f)
            {
                _moveCamera = false;
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

                for (int i = 0; i < _activePlayers.Length; i++)
                {
                    _activePlayers[i].transform.position = _waypointsPreviousRoom[i].transform.position;
                    _playerBehaviours[i].PlayerTeleport();
                }
                for (int i = 0; i < _activeGhosts.Length; i++)
                {
                    _activeGhosts[i].transform.position = _waypointsGhostsPreviousRoom[i].transform.position;
                    _activeGhosts[i].GetComponent<ghostController>().GhostTeleport();
                }

            }
            else
            {
                _generator.CameraIndex++;
                _enteredNextRoom = true;

                for (int i = 0; i < _activePlayers.Length; i++)
                {
                    _activePlayers[i].transform.position = _waypointsNextRoom[i].transform.position;
                    _playerBehaviours[i].PlayerTeleport();
                }
                for (int i = 0; i < _activeGhosts.Length; i++)
                {
                    _activeGhosts[i].transform.position = _waypointsGhostsNextRoom[i].transform.position;
                    _activeGhosts[i].GetComponent<ghostController>().GhostTeleport();
                }
            }
            _moveCamera = true;
        }
    }
}
