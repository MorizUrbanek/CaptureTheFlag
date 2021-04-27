using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerTarget : NetworkBehaviour
{
    public float maxHealth = 50f;
    public float damageperHit = 10f;
    
    public bool isAttacker;
    private bool isDeath = false;// tookDamage = false;

    [SyncVar]
    [SerializeField] private float currentHealth;

    public delegate void HealthChangedDelegate(float currentHealth);
    public event HealthChangedDelegate EventHealthChanged;

    #region Server
    [Server]
    private void SetHealth(float value)
    {
        currentHealth = value;
        EventHealthChanged?.Invoke(currentHealth);
    }

    public override void OnStartServer()
    {
        SetHealth(maxHealth);
    }

    [Command]
    private void CmdDealDamage()
    {
        Debug.Log("dealingdamage");
        SetHealth(Mathf.Max(currentHealth - damageperHit, 0));
    }

    #endregion

    #region Client

    [ClientCallback]
    private void Update()
    {
        if (!hasAuthority) { return; }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            CmdDealDamage();
        }
        // Debug.Log("tookdamage :"+tookDamage);
        //if (tookDamage) 
        //{
        //    Debug.Log("dealdamge");
        //    CmdDealDamage();
        //    tookDamage = false;
        //}
    }

    [ClientCallback]
    public void TakeDamage()
    {
        if (!isDeath)
        {
            Debug.Log("tookdamge");
           // tookDamage = true;
        }
    }
    #endregion


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Flag" && isAttacker)
        {
            Destroy(other.gameObject);
        }
    }

    //private void OnEnable()
    //{
    //    EventHealthChanged += HandleHealthChange;
    //}

    //private void OnDisable()
    //{
    //    EventHealthChanged -= HandleHealthChange;
    //}

    //[ClientRpc]
    //private void HandleHealthChange(float currentHealth)
    //{
    //    this.currentHealth = currentHealth;
    //}

}
