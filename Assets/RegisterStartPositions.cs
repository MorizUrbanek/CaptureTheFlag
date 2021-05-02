using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class RegisterStartPositions : NetworkBehaviour
{
    public override void OnStartServer()
    {
        foreach (Transform spawnPoint in transform)
        {
            NetworkManager.RegisterStartPosition(spawnPoint);
        }
    }
}
