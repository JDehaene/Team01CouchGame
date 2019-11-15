using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyPrefabCheck : MonoBehaviour
{
    private void Update()
    {
        if (transform.childCount <= 0)
        {
            Destroy(gameObject);
        }
    }
}
