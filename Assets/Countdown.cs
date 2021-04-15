using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Countdown : MonoBehaviour
{

    public GameObject weapons, cubePlacer;
    public PlaceCube ghostCube;
    TextManager text;
    float startTime, updateSteps = 0.2f, countdownTime = 100f , endTime;

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

}
