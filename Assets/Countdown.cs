using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;

public class Countdown : NetworkBehaviour
{
    //public GameObject weapons;
    //public Weapon weapon;
    //public PlaceCube placeCube;
    TextManager text;
    float startTime, updateSteps = 0.01f , endTime;
    [SerializeField] private const float countdownTime = 10f;

    [SyncVar]
    float currentime;

    public delegate void TimeChangedDelegate(float currentime);
    public event TimeChangedDelegate EventTimeChanged;

    void Start()
    {
        text = GetComponent<TextManager>();
    }

    #region Server
    [Server]
    private void SetCountdownTime(float value)
    {
        currentime = value;
        EventTimeChanged?.Invoke(currentime);
    }

    public override void OnStartServer()
    {
        SetCountdownTime(countdownTime);
    }

    #endregion

    private void OnEnable()
    {
        EventTimeChanged += HandleTimeChanged;
    }

    private void OnDisable()
    {
        EventTimeChanged -= HandleTimeChanged;
    }

    [ClientRpc]
    private void HandleTimeChanged(float currentime)
    {
        if (text == null)
        {
            return;
        }
        if (currentime == 0)
        {
            text.ClearText();
        }
        else
        {
            text.ChangeText(currentime.ToString("0.0"));
        }
    }

    public void StartCountDown()
    {
        currentime = countdownTime;
        startTime = Time.time;
        endTime = startTime + currentime;

        InvokeRepeating("UpdateCoundDown", 0, updateSteps);
    }

    
    private void UpdateCoundDown()
    {
        if (Time.time >= endTime)
        {
            currentime = 0;
            CancelInvoke("UpdateCoundDown");
            text.ClearText();
            if (NetworkManagerOverride.roundcount == 0 || NetworkManagerOverride.roundcount == 4)
            {
                NetworkManagerOverride.instance.EnableCubePlacer(false);
            }
            else if (NetworkManagerOverride.roundcount == 3)
            {
                NetworkManagerOverride.instance.ChangeIsAttacker();
                NetworkManagerOverride.instance.EnableCubePlacer(true);
            }
            if (NetworkManagerOverride.roundcount == 7)
            {
                NetworkManagerOverride.instance.GameOver();
            }
            NetworkManagerOverride.instance.RoundOver(true);
        }
        else
        {
            currentime = endTime - Time.time;
        }
        EventTimeChanged?.Invoke(currentime);
    }

    public void StopCountDown()
    {
        currentime = 0;
        CancelInvoke("UpdateCoundDown");
        text.ClearText();
        EventTimeChanged?.Invoke(currentime);
    }
}
