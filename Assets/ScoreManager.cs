using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ScoreManager : NetworkBehaviour
{
    public TextManager score1;
    public TextManager score2;

    [ClientRpc]
    public void RpcUpdateScore(int score1int,int score2int)
    {
        Debug.Log("score changed");
        score1.ChangeText(score1int.ToString());
        score2.ChangeText(score2int.ToString());
    }
}
