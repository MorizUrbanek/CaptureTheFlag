using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Enabler : NetworkBehaviour
{
    [SerializeField] private PlaceCube placeCube;
    [SerializeField] private Weapon weapon;
    [SerializeField] private GameObject weaponholder;
    [SerializeField] private MovePlayer move;
    [SerializeField] private PlayerTarget playerTarget;
    public GameObject body;

    [SyncVar]
    [SerializeField] private bool placeCubeisEnabled;

    public delegate void PlaceCubesChangedDelegate(bool placeCubeState);
    public event PlaceCubesChangedDelegate EventPlaceCubeChanged;

    [SyncVar]
    [SerializeField] private bool isDeath;

    public delegate void IsDeathChangedDelegate(bool IsDeathState);
    public event IsDeathChangedDelegate EventIsDeathChanged;

    private void OnEnable()
    {
        EventPlaceCubeChanged += RpcEnabelPlaceCube;
        EventIsDeathChanged += RpcHandleIsDeathChange;
    }
    private void OnDisable()
    {
        EventPlaceCubeChanged -= RpcEnabelPlaceCube;
        EventIsDeathChanged -= RpcHandleIsDeathChange;
    }


    [Server]
    public void SetPlaceCubeState(bool value)
    {
        placeCubeisEnabled = value;
        EventPlaceCubeChanged?.Invoke(placeCubeisEnabled);
    }

    [Server]
    public void SetIsDeathState(bool value)
    {
        isDeath = value;
        EventIsDeathChanged?.Invoke(isDeath);
    }

    [ClientRpc]
    public void RpcEnabelPlaceCube(bool placeCubeState)
    {
        if (placeCubeState)
        {
            placeCube.enabled = true;
            weapon.enabled = false;
            weaponholder.SetActive(false);
            move.enabled = true;
        }
        else
        {
            weapon.enabled = true;
            weaponholder.SetActive(true);
            placeCube.enabled = false;
            move.enabled = true;
        }

    }

    [ClientRpc]
    public void RpcHandleIsDeathChange(bool IsDeathState)
    {
        if (IsDeathState)
        {
            move.enabled = false;
            placeCube.enabled = false;
            weapon.enabled = false;
            body.SetActive(false);
            playerTarget.enabled = false;
            Invoke("Respawn", 3f);
        }
        else
        {
            move.enabled = true;
            placeCube.enabled = false;
            weapon.enabled = true;
            playerTarget.enabled = true;
            body.SetActive(true);
        }
    }


    private void Respawn()
    {
        if (hasAuthority)
        {
            CmdStartRespawn();
        }
    }

    [Command]
    public void CmdStartRespawn()
    {
        SetIsDeathState(false);
    }
}
