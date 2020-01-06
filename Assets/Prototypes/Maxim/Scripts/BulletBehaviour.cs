using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    private float _speed;
    private float _damage;
    private bool _isHostile = false;
    private float _timer;

    public GameObject BulletPrefab;
    public bool _isFireSpell= false, _isEletricSpell = false;

    private void Start()
    {
        _speed = BulletPrefab.GetComponent<BulletStats>().BulletSpeed;
        _damage = BulletPrefab.GetComponent<BulletStats>().CurrentBulletDamage;
        _isHostile = BulletPrefab.GetComponent<BulletStats>().IsHostile;
    }

    private void Update()
    {
        if(!_isEletricSpell)
        {
            transform.Translate(Vector3.forward * _speed * Time.deltaTime);
        }
        
        _timer += Time.deltaTime;
        if(_timer >= 0.6f && _isFireSpell)
        {
            Destroy(gameObject);
        }
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
            if(!_isFireSpell)
            {
                Destroy(gameObject);
            }
        }

        if(col.tag == "Enemy" && !_isHostile)
        {
            col.GetComponent<EnemyBehaviour>().EnemyTakesDamage(_damage);
            if (!_isFireSpell)
            {
                Destroy(gameObject);
            }
        }
        
        if (col.tag == "Player" && _isHostile)
        {
            col.GetComponent<PlayerBehaviour>().PlayerTakesDamage(_damage);
            if (!_isFireSpell)
            {
                Destroy(gameObject);
            }
        }

    }

}
