using UnityEngine;
using Mirror;

public class Target : NetworkBehaviour
{
    public float health;

    public override void OnStartAuthority()
    {
        health = 50f;
    }

    public bool TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            // assignAuthorityObj.GetComponent<NetworkIdentity>().AssignClientAuthority(this.GetComponent<NetworkIdentity>().connectionToClient);
            //this.GetComponent<NetworkIdentity>().AssignClientAuthority(player.GetComponent<NetworkIdentity>().connectionToClient);

            return true;
        }
        return false;
    }


    
}
