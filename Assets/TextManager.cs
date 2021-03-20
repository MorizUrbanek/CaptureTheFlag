﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextManager : MonoBehaviour
{
    TextMeshProUGUI text;
    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void ChangeText(string newtext,bool autoremove = false, float cleartime = 5)
    {
        text.text = newtext;
        if (autoremove)
        {
            Invoke("ClearText", cleartime);
        }
    }

    public void ClearText()
    {
        text.text = string.Empty;
    }
}
