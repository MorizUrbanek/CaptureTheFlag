using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float impactForce = 30f;
    [SerializeField] private bool damagePlayer;

    public Camera fpsCam;
    public Transform shootPoint;
    
    public LineRenderer bullettrail;

    private Ray ray;

    // Update is called once per frame
    void Update()
    {
        ray = fpsCam.ScreenPointToRay(Input.mousePosition);

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, range))
        {
            SpawnBulletTrail(hit.point);
            if (damagePlayer)
            {
                if (hit.transform.tag == "Player")
                {
                    DoDamage(hit);
                }
            }
            else
            {
                if (hit.transform.tag == "Block")
                {
                    DoDamage(hit);
                }
            }
        }
    }

    private void SpawnBulletTrail(Vector3 hitpoint)
    {
        GameObject bulletTrailEffect = Instantiate(bullettrail.gameObject, Vector3.zero, Quaternion.identity);

        LineRenderer lineR = bulletTrailEffect.GetComponent<LineRenderer>();

        lineR.SetPosition(0, shootPoint.position);
        lineR.SetPosition(1, hitpoint);

    }

    private void DoDamage(RaycastHit hit)
    {
        Target target = hit.transform.GetComponent<Target>();
        if (target != null)
        {
            target.TakeDamage(damage);
        }

        if (hit.rigidbody != null)
        {
            hit.rigidbody.AddForce(-hit.normal * impactForce);
        }
    }

}
