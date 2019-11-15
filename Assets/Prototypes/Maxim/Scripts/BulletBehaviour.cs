using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    private float _speed;
    private float _damage;

    public GameObject BulletPrefab;

    private void Start()
    {
        _speed = BulletPrefab.GetComponent<BulletStats>().BulletSpeed;
        _damage = BulletPrefab.GetComponent<BulletStats>().CurrentBulletDamage;
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Enemie")
        {
            //deal damage
            Destroy(gameObject);
        }

        if (col.tag == "Wall")
        {
            Destroy(gameObject);
        }

    }

}
