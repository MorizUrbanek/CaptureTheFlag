using UnityEngine;
using Mirror;

public class Weapon : NetworkBehaviour
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
                    DoDamageToPlayer(hit);
                }
            }
            else
            {
                if (hit.transform.tag == "Block")
                {
                    DoDamageToBlock(hit);
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

    private void DoDamageToBlock(RaycastHit hit)
    {
        Target target = hit.transform.GetComponent<Target>();
        if (target != null)
        {
            if (target.TakeDamage(damage))
            {
                CmdSetAuthority(this.GetComponent<NetworkIdentity>(), hit.transform.gameObject.GetComponent<NetworkIdentity>());
                CmdDestroyObject(hit.transform.gameObject);
            }
        }

        //if (hit.rigidbody != null)
        //{
        //    hit.rigidbody.AddForce(-hit.normal * impactForce);
        //}
    }

    [Command]
    public void CmdSetAuthority(NetworkIdentity master,NetworkIdentity slave)
    {
        slave.RemoveClientAuthority();
        slave.GetComponent<NetworkIdentity>().AssignClientAuthority(master.GetComponent<NetworkIdentity>().connectionToClient);
    }

    [Command]
    public void CmdDestroyObject(GameObject ObjectToDestory)
    {
        NetworkServer.Destroy(ObjectToDestory);
    }

    private void DoDamageToPlayer(RaycastHit hit)
    {
        PlayerTarget target = hit.transform.GetComponent<PlayerTarget>();
        if (target != null)
        {
            target.TakeDamage();
        }
    }

}
