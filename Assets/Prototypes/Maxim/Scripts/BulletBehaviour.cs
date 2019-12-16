using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    private float _speed;
    private float _damage;
    private bool _isHostile = false;

    public GameObject BulletPrefab;

    private void Start()
    {
        _speed = BulletPrefab.GetComponent<BulletStats>().BulletSpeed;
        _damage = BulletPrefab.GetComponent<BulletStats>().CurrentBulletDamage;
        _isHostile = BulletPrefab.GetComponent<BulletStats>().IsHostile;
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }

    public void OnTriggerEnter(Collider col)
    {
        
        if (col.tag == "Wall")
        {
            Destroy(gameObject);
        }
        
        if (col.tag == "Ghost" && !_isHostile)
        {
            col.GetComponent<ghostController>().GhostTakesDamage(_damage);
            Destroy(gameObject);
        }

        if(col.tag == "Enemy" && !_isHostile)
        {
            col.GetComponent<EnemyBehaviour>().EnemyTakesDamage(_damage);
            Debug.Log("hit's enemy for " + _damage);
            Destroy(gameObject);
        }
        
        if (col.tag == "Player" && _isHostile)
        {
            col.GetComponent<PlayerBehaviour>().PlayerTakesDamage(_damage);
            Destroy(gameObject);
        }

    }

}
