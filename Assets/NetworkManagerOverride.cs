using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;

public class NetworkManagerOverride : NetworkManager
{
    public static NetworkManagerOverride instance = null;
    public static int roundcount;
    static private int playercount;
    public int[] playerScores;
    static private GameObject[] players;
    [SerializeField] private Countdown countdown;
    [SerializeField] private ScoreManager score;

    public override void OnStartServer()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            return;
        }
        playercount = 0;
        roundcount = 0;
        playerScores = new int[] {0,0,0,0};
        players = new GameObject[4];
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        Transform startPos = GetStartPosition();
        GameObject player = startPos != null
            ? Instantiate(playerPrefab, startPos.position, startPos.rotation)
            : Instantiate(playerPrefab);

        // instantiating a "Player" prefab gives it the name "Player(clone)"
        // => appending the connectionId is WAY more useful for debugging!
        //player.name = $"{playerPrefab.name} [connId={conn.connectionId}] {playercount}";
        players[playercount] = player;
        playercount++;
        player.name = $"Player{playercount}";
        PlayerTarget playertarget = player.GetComponent<PlayerTarget>();
        if (playertarget != null)
        {
            if (playercount % 2 == 0)
            {
                playertarget.SetIsAttacker(true);
            }
            else
            {
                playertarget.SetIsAttacker(false);
            }
        }
        playertarget.SetSpawnPosition(startPos.position);
        NetworkServer.AddPlayerForConnection(conn, player);
        if (playercount == 2)
        {
            countdown.StartCountDown();
            EnableCubePlacer(true);
        }
    }

    public void EnableCubePlacer(bool placeCubeState)
    {
        Enabler enabler;
        foreach (GameObject player in players.Where(i=> i != null))
        {
            enabler = player.GetComponent<Enabler>();
            if (enabler != null)
            {
                enabler.SetPlaceCubeState(placeCubeState);
            }
        }
    }

    public void SetIsDeath(bool isDeath,GameObject deathPlayer)
    {
        Enabler enabler;
        foreach (GameObject player in players.Where(i => i == deathPlayer))
        {
            enabler = player.GetComponent<Enabler>();
            if (enabler != null)
            {
                enabler.SetIsDeathState(isDeath);
            }
        }
    }

    public void ChangeIsAttacker()
    {
        PlayerTarget playertarget;
        int count = 1;
        foreach (GameObject player in players.Where(i => i != null))
        {
            playertarget = player.GetComponent<PlayerTarget>();
            if (playertarget != null)
            {
                if (count % 2 != 0)
                {
                    playertarget.SetIsAttacker(true);
                }
                else
                {
                    playertarget.SetIsAttacker(false);
                }
            }
            count++;
        }
    }

    public void RoundOver(bool coutdownover)
    {
        bool addToscore = true;
        int count = 0;
        PlayerTarget playerTarget;
        if (roundcount == 0 || roundcount == 4)
        {
            instance.EnableCubePlacer(false);
            addToscore = false;
        }

        foreach (GameObject player in players.Where(i => i != null))
        {
            playerTarget = player.GetComponent<PlayerTarget>();
            if (playerTarget != null)
            {
                if (addToscore)
                {
                    if (coutdownover)
                    {
                        if (!playerTarget.isAttacker)
                        {
                            playerScores[count]++;
                        }
                    }
                    else
                    {
                        if (playerTarget.isAttacker)
                        {
                            playerScores[count]++;
                        }
                    }
                }
                playerTarget.ResetToSpawnPoint();
            }
            count++;
        }
        score.RpcUpdateScore(playerScores[0],playerScores[1]);
        
        if (roundcount == 3)
        {
            instance.ChangeIsAttacker();
            instance.EnableCubePlacer(true);
        }
        if (roundcount == 7)
        {
            instance.GameOver();
        }
        else
        {
            countdown.StartCountDown();
        }
        roundcount++;
    }

    public void GameOver()
    {
        score.RpcUpdateScore(playerScores[0], playerScores[1]);
        Debug.Log("GameOver");
    }
}
