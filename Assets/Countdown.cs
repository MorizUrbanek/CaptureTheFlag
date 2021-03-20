using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Countdown : MonoBehaviour
{

    public Gun gun;
    TextManager text;
    float startTime, updateSteps = 0.2f, countdownTime = 20f , endTime;

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
            gun.SetShootMode(true);
            CancelInvoke("UpdateCoundDown");
            text.ClearText();
        }
        else
        {
            countdownTime = endTime - Time.time;
            text.ChangeText(countdownTime.ToString("0.00"));
        }
    }

}
