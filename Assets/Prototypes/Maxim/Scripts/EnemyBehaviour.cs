using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Enemy variables")]
    private Collider[] _colliders;
    [SerializeField]
    private float _radius = 5f;
    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private int _enemyType;
    [Header("Charge variables")]

    [SerializeField]
    private float _timeUntilCharge;
    [SerializeField]
    private float _chargeSpeedModifier;
    [SerializeField]
    private GameObject _player;

    private void Start()
    {
        //assign random enemy type
        //_enemyType = Random.Range(1, 2);
    }

    void Update()
    {
        _colliders = Physics.OverlapSphere(this.transform.position, _radius);

        //foreach (Collider collider in _colliders)
        //{
        //    if (collider.CompareTag("Player"))
        //    {
        //        transform.LookAt(collider.transform);
        //        transform.position = Vector3.MoveTowards(transform.position, collider.transform.position, _speed * Time.deltaTime);
        //    }
        //}
        //EnemyPicker();
        EnemyBehaviourTwo();
    }

    void EnemyPicker()
    {
        switch(_enemyType)
        {
            case 1:
                EnemyBehaviourOne();
                break;
            case 2:
                EnemyBehaviourTwo();
                break;

        }
    }

    private void EnemyBehaviourOne()
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
    private void EnemyBehaviourTwo()
    {
        foreach (Collider collider in _colliders)
        {
            if (collider.CompareTag("Player"))
            {
                transform.LookAt(collider.transform);
                transform.position = Vector3.MoveTowards(transform.position, collider.transform.position, _speed * Time.deltaTime);

                if(Vector3.Distance(transform.position, collider.transform.position) > _radius / 2)
                {
                    StartCoroutine(("Charge"),_colliders);
                }
            }
        }
    }

    private IEnumerator Charge()
    {
        yield return new WaitForSeconds(_timeUntilCharge);
        Debug.Log("Charge bro");
        transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, _speed* _chargeSpeedModifier * Time.deltaTime);
    }
}
