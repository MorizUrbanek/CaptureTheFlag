using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineFade : MonoBehaviour
{
    [SerializeField] private Color color;

    [SerializeField] private float speed;

    LineRenderer lr;
    float lifetime = .5f;
    

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        color.a = Mathf.Lerp(color.a, 0, Time.deltaTime * speed);
        lr.startColor = color;
        lr.endColor = color;
    }
}
