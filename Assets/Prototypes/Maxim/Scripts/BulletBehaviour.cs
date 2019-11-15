using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _damage;

    public void BulletPower(float playerPower)
    {
        _damage = _damage * playerPower;
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Enemie")
        {
            
            Destroy(gameObject);
        }

        if (col.tag == "Wall")
        {
            Destroy(gameObject);
        }

    }

}
