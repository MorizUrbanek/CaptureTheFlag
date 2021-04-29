using UnityEngine;
using Mirror;

public class Weapon : NetworkBehaviour
{
    [SerializeField] private Camera fpsCam;
    [SerializeField] private GameObject bullettrail;

    private Ray ray;
    private float damage = 0;
    private float range = 0;
    private Transform shootPoint;
    private bool damagePlayer;

    // Update is called once per frame
    void Update()
    {
        ray = fpsCam.ScreenPointToRay(Input.mousePosition);

        if (Input.GetButtonDown("Fire1"))
        {
            CmdShoot(ray.origin, ray.direction, shootPoint.position, range, damagePlayer, gameObject, damage);
        }
    }

    [Command]
    public void CmdShoot(Vector3 origin, Vector3 direction, Vector3 shootpiont, float range, bool damagePlayer, GameObject owner, float damage)
    {
        RaycastHit hit;
        if (Physics.Raycast(origin, direction, out hit, range))
        {
            RpcSpawnBulletTrail(hit.point, shootpiont);
            if (damagePlayer)
            {
                if (hit.transform.tag == "Player")
                {
                    DoDamageToPlayer(hit, damage);
                }
            }
            else
            {
                if (hit.transform.tag == "Block")
                {
                    DoDamageToBlock(hit, owner, damage);
                }
            }
        }
    }

    [ClientRpc]
    private void RpcSpawnBulletTrail(Vector3 hitpoint, Vector3 shootPoint)
    { 
        GameObject bulletTrailEffect = Instantiate(bullettrail, Vector3.zero, Quaternion.identity);

        LineRenderer lineR = bulletTrailEffect.GetComponent<LineRenderer>();

        lineR.SetPosition(0, shootPoint);
        lineR.SetPosition(1, hitpoint);
        //NetworkServer.Spawn(bulletTrailEffect);
    }

    [Server]
    private void DoDamageToBlock(RaycastHit hit , GameObject owner, float damage)
    {
        Target target = hit.transform.GetComponent<Target>();
        if (target != null)
        {
            if (target.TakeDamage(damage))
            {
                CmdSetAuthority(owner.GetComponent<NetworkIdentity>(), hit.transform.gameObject.GetComponent<NetworkIdentity>());
                CmdDestroyObject(hit.transform.gameObject);
            }
        }

        //if (hit.rigidbody != null)
        //{
        //    hit.rigidbody.AddForce(-hit.normal * impactForce);
        //}
    }

    [Server]
    public void CmdSetAuthority(NetworkIdentity master,NetworkIdentity slave)
    {
        slave.RemoveClientAuthority();
        slave.GetComponent<NetworkIdentity>().AssignClientAuthority(master.GetComponent<NetworkIdentity>().connectionToClient);
    }

    [Server]
    public void CmdDestroyObject(GameObject ObjectToDestory)
    {
        NetworkServer.Destroy(ObjectToDestory);
    }

    [Server]
    private void DoDamageToPlayer(RaycastHit hit, float damage)
    {
        PlayerTarget target = hit.transform.GetComponent<PlayerTarget>();
        if (target != null)
        {
            target.TakeDamage(damage);
        }
    }

    public void SetWeapon(float damage,float range,Transform shootPoint,bool damagePlayer)
    {
        this.damage = damage;
        this.range = range;
        this.shootPoint = shootPoint;
        this.damagePlayer = damagePlayer;
    }

}
