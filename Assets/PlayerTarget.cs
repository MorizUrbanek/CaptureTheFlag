using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTarget : MonoBehaviour
{
    public float health = 50f;
    private float respwanTime = 5;
    private bool isDeath = false;

    public void TakeDamage(float damage)
    {
        if (!isDeath)
        {
            health -= damage;
            if (health <= 0)
            {
                Die();
                //isDeath = true;
            }
        }
    }

    void Die()
    {
        //Invoke("Respawn", respwanTime);
        Respawn();
        respwanTime += 2;
    }

    void Respawn()
    {
        health = 50f;
        transform.position = new Vector3(Random.Range(-49, 49), 0.5f, Random.Range(-49, 49));
        isDeath = false;
    }
}
