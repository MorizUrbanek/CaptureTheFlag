using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectFlag : MonoBehaviour
{
    public PlayerTarget playerTarget;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Flag" && playerTarget.isAttacker)
        {
            Destroy(hit.gameObject);
            RoundWin();
        }
    }

    private void RoundWin()
    {
        NetworkManagerOverride.instance.RoundOver(false);
    }
}
