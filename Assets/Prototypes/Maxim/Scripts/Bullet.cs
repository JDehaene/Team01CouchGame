using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5;

    private void Update()
    {
        transform.position += transform.forward * _speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(this.gameObject);

        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
        }
    }
}
