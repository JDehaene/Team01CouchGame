using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    private float _speed;
    private float _damage;
    private bool _isGhost = false;

    public GameObject BulletPrefab;

    private void Start()
    {
        _speed = BulletPrefab.GetComponent<BulletStats>().BulletSpeed;
        _damage = BulletPrefab.GetComponent<BulletStats>().CurrentBulletDamage;
        _isGhost = BulletPrefab.GetComponent<BulletStats>().IsGhost;
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Ghost" && !_isGhost)
        {
            col.GetComponent<ghostController>().GhostTakesDamage(_damage);
            Destroy(gameObject);
        }

        if (col.tag == "Wall")
        {
            Destroy(gameObject);
        }

        if (col.tag == "Player" && _isGhost)
        {
            col.GetComponent<PlayerBehaviour>().PlayerTakesDamage(_damage);
            Destroy(gameObject);
        }

    }

}
