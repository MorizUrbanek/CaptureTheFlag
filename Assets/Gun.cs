using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float impactForce = 30f;

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
            Target target = hit.transform.GetComponent<Target>();
            SpawnBulletTrail(hit.point);
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

    private void SpawnBulletTrail(Vector3 hitpoint)
    {
        GameObject bulletTrailEffect = Instantiate(bullettrail.gameObject, Vector3.zero, Quaternion.identity);

        LineRenderer lineR = bulletTrailEffect.GetComponent<LineRenderer>();

        lineR.SetPosition(0, shootPoint.position);
        lineR.SetPosition(1, hitpoint);

    }

    //void DoRayCast(bool build)
    //{
    //    RaycastHit hit;
    //    if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, cubePlaceRange))
    //    {
    //        if (build)
    //        {
    //            PlaceCube(hit);
    //        }
    //        else
    //        {
    //            if (hit.transform.gameObject.layer != 8)
    //            {
    //                grid.SetPosition((int)hit.transform.position.z, (int)hit.transform.position.x, false);
    //                Destroy(hit.transform.gameObject);
    //            }

    //        }
    //    }
    //}

    //void PlaceCube(RaycastHit hit)
    //{
    //    bool isUsed;
    //    Vector3 truePosition = grid.GetNearestPointOnGrid(hit.point, out isUsed);
    //    if (!isUsed)
    //    {
    //        Instantiate(cube, truePosition, Quaternion.identity);
    //        grid.SetPosition((int)truePosition.z, (int)truePosition.x, true);
    //    }

    //}
}
