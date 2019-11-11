using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    private Collider[] _colliders;
    [SerializeField]
    private float _radius = 5f;
    [SerializeField]
    private float _speed = 5f;

    // Update is called once per frame
    void Update()
    {
        _colliders = Physics.OverlapSphere(this.transform.position, _radius);

        foreach (Collider collider in _colliders)
        {
            if (collider.CompareTag("Player"))
            {
                transform.LookAt(collider.transform);
                transform.position = Vector3.MoveTowards(transform.position, collider.transform.position, _speed * Time.deltaTime);
            }
        }
    }
}
