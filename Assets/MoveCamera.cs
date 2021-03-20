using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Material m;
    float xMove, yMove, speed = 60f;
    Color color;

    //private void Start()
    //{
    //    Debug.Log(m.GetColor("GridColor"));
    //}
    // Update is called once per frame
    void Update()
    {
        xMove = Input.GetAxisRaw("Horizontal");
        yMove = Input.GetAxisRaw("Vertical");
        MoveCam();
        Debug.Log(Mathf.Clamp((1f / 360f) * Mathf.Abs(transform.rotation.eulerAngles.x), 0, 1));
        color = new Vector4(Mathf.Clamp((1f / 360f) * Mathf.Abs(transform.rotation.eulerAngles.x), 0, 1),
                            Mathf.Clamp((1f / 360f) * Mathf.Abs(transform.rotation.eulerAngles.y), 0, 1),
                            Mathf.Clamp((1f / 360f) * Mathf.Abs(transform.rotation.eulerAngles.z), 0, 1), 1f);
        m.SetColor("GridColor", color);
    }

    private void MoveCam()
    {
        transform.Rotate(new Vector3(1, 0, 0), -yMove * 1 * Time.deltaTime * speed);
        transform.Rotate(new Vector3(0, 1, 0), xMove * 1 * Time.deltaTime * speed);
    }
}
