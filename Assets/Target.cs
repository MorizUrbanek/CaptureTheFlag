using UnityEngine;

public class Target : MonoBehaviour
{
    public float health = 50f;
    Collider c;
    Rigidbody rb;

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log(collision.gameObject.name);
    //}
}
