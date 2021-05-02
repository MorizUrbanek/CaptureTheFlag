using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerTarget : NetworkBehaviour
{
    [SerializeField] private const float maxHealth = 50f;

    [SyncVar]
    [SerializeField] private Vector3 spawnPoint;

    [SyncVar]
    [SerializeField] public bool isAttacker;

    [SyncVar]
    [SerializeField] private float currentHealth;

    //public delegate void HealthChangedDelegate(float currentHealth);
    //public event HealthChangedDelegate EventHealthChanged;

    #region Server
    [Server]
    private void SetHealth(float value)
    {
        currentHealth = value;
    }

    public override void OnStartServer()
    {
        SetHealth(maxHealth);
    }

    private void OnEnable()
    {
        currentHealth = maxHealth;
    }

    #endregion

    #region Client

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Max(currentHealth - damage, 0);
        if (currentHealth == 0)
        {
            NetworkManagerOverride.instance.SetIsDeath(true, this.gameObject);
        }
    }
    #endregion


    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "Flag" && isAttacker)
    //    {
    //        Destroy(other.gameObject);
    //    }
    //}

    public void SetIsAttacker(bool isAttacker)
    {
        this.isAttacker = isAttacker;
    }

    public void SetSpawnPosition(Vector3 spawnPosition)
    {
        this.spawnPoint = spawnPosition;
    }

    public Vector3 GetSpawnPosition()
    {
        return spawnPoint;
    }

    public void ResetToSpawnPoint()
    {
        RpcResetToSpawnPoint();
    }

    [ClientRpc]
    private void RpcResetToSpawnPoint()
    {
        transform.position = spawnPoint;
    }

}
