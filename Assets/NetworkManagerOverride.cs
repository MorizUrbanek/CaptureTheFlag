using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkManagerOverride : NetworkManager
{
    static private int playercount;


    public override void OnStartServer()
    {
        base.OnStartServer();
        playercount = 0;
    }
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        Transform startPos = GetStartPosition();
        GameObject player = startPos != null
            ? Instantiate(playerPrefab, startPos.position, startPos.rotation)
            : Instantiate(playerPrefab);

        // instantiating a "Player" prefab gives it the name "Player(clone)"
        // => appending the connectionId is WAY more useful for debugging!
        player.name = $"{playerPrefab.name} [connId={conn.connectionId}] {playercount}";
        playercount++;
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
        NetworkServer.AddPlayerForConnection(conn, player);
        
    }
}
