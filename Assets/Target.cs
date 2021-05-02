using UnityEngine;
using Mirror;

public class Target : NetworkBehaviour
{
    [SerializeField] private const float maxHealth = 50f;

    [SyncVar]
    [SerializeField] private float currentHealth;

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

    public bool TakeDamage(float damage)
    {
        currentHealth = Mathf.Max(currentHealth - damage, 0);
        if (currentHealth == 0)
        {
            return true;
        }
        return false;
    }

    #endregion
}
