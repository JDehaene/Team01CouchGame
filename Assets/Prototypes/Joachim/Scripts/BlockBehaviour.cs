using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBehaviour : MonoBehaviour
{
    [SerializeField] private float _fallSpeed;
    [SerializeField] private GameObject _blockTransporter;
    [SerializeField] private Transform[] _spawnPoints;

    void Update()
    {
        transform.position -= Vector3.down * _fallSpeed * -1 * Time.deltaTime;

        if (transform.position.y < _blockTransporter.transform.position.y)
            TransportBlock();
    }
    void TransportBlock()
    {
        int _randomSpot = Random.Range(0, 4);
        transform.position = _spawnPoints[_randomSpot].position;
    }
}
