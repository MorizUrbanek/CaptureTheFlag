using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerTarget : NetworkBehaviour
{
    [SerializeField] private const float maxHealth = 50f;

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

    #endregion

    #region Client
    //Todo death and respawn
    //private void Update()
    //{
    //    if (!hasAuthority){return;}

    //    if (currentHealth != maxHealth)
    //    {
    //        Debug.Log(currentHealth);
    //    }
    //}

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Max(currentHealth - damage, 0);
    }
    #endregion


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Flag" && isAttacker)
        {
            Destroy(other.gameObject);
        }
    }

    public void SetIsAttacker(bool isAttacker)
    {
        this.isAttacker = isAttacker;
    }
}
