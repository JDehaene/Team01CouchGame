using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public GameObject Walls;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pushable"))
        {
            Walls.SetActive(false);
        }
    }
}
