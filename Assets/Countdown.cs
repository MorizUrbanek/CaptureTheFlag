using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Countdown : MonoBehaviour
{

    public GameObject weapons, cubePlacer;
    public PlaceCube ghostCube;
    TextManager text;
    float startTime, updateSteps = 0.01f, countdownTime = 40f , endTime;

    void Start()
    {
        text = GetComponent<TextManager>();
        text.ChangeText(countdownTime.ToString());
        startTime = Time.time;
        endTime = startTime + countdownTime;

        InvokeRepeating("UpdateCoundDown", updateSteps, updateSteps);
    }

    private void UpdateCoundDown()
    {
        if (Time.time >= endTime)
        {
            weapons.SetActive(true);
            ghostCube.SetGhostCubeActive(false);
            cubePlacer.SetActive(false);
            CancelInvoke("UpdateCoundDown");
            text.ClearText();
        }
        else
        {
            countdownTime = endTime - Time.time;
            text.ChangeText(countdownTime.ToString("0.0"));
        }
    }

    //void StartCountDown(Func<int,string> callBack)
    //{

    //}

}
