﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Look : MonoBehaviour
{
    private float mouseSensetivity = 100f;

    public Transform playerBody;
    public MovePlayer player;

    float xRotation;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.isLocalPlayer)
        {
            return;
        }
        float mouseX = Input.GetAxis("Mouse X") * mouseSensetivity * OptionsMenu.GetSensitivity() * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensetivity * OptionsMenu.GetSensitivity() * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);

    }
}
