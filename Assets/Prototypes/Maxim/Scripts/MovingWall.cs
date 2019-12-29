using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingWall : MonoBehaviour
{
    private float _timer;

    [SerializeField] private float _amplitude = 1;
    [SerializeField] private float _frequency = 1;

    void Update()
    {
        float y = _amplitude * Mathf.Sin(_timer * _frequency);
        this.transform.localScale = new Vector3(1, Mathf.Abs(y), 1);

        _timer += Time.deltaTime;
    }
}
