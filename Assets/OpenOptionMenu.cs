using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenOptionMenu : MonoBehaviour
{
    public GameObject optionMenu;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (optionMenu.activeSelf)
            {
                optionMenu.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                optionMenu.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    public void ReturnToGame()
    {
        optionMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
}
