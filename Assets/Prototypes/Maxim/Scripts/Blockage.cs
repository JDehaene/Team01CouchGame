using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blockage : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemies;

    private List<GameObject> _walls = new List<GameObject>();

    private BoxCollider _collider;
    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<BoxCollider>();

        for (int childIndex = 0; childIndex < this.transform.childCount; childIndex++)
        {
            _walls.Add(this.transform.GetChild(childIndex).gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_enemies.transform.childCount == 0)
        {
            EnableWalls(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _collider.enabled = false;
            EnableWalls(true);
        }
    }

    private void EnableWalls(bool state)
    {
        foreach (GameObject wall in _walls)
        {
            wall.SetActive(state);
        }
    }
}
