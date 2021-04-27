using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Commands : NetworkBehaviour
{
    private GameObject objectToSpawn;

    public void SpawnObjectOnServer(Vector3 position, GameObject objectToSpawnRef, GameObject owner)
    {
        this.objectToSpawn = objectToSpawnRef;
        CmdSpawnObject(position, owner);
    }

    [Command]
    public void CmdSpawnObject(Vector3 position, GameObject owner)
    {
        GameObject serverCube = Instantiate(objectToSpawn, position, Quaternion.identity);
        NetworkServer.Spawn(serverCube, owner);
    }

    [Command]
    public void CmdDestroyObject(GameObject cube)
    {
        NetworkServer.Destroy(cube);
    }
}
