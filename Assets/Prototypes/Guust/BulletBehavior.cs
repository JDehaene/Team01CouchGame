using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private int _damage;

    private void Update()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            col.GetComponent<TestPlayerController>().TakeDamage(_damage);
            Destroy(gameObject);
        }
        
        if(col.tag == "Wall")
        {
            Destroy(gameObject);
        }

    }

   

}
