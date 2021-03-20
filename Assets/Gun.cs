using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float cubePlaceRange = 40f;
    public float impactForce = 30f;

    public Camera fpsCam;
    public GameObject cube;
    public GameObject ghostCube;
    public Transform shootPoint;
    public Transform anchorPoint;
    public LineRenderer bullettrail;
    public GameObject gun;

    bool shootMode;
    [SerializeField] private int blockCount = 150;
    private RaycastHit hit;
    private Ray ray;
    private GameObject ghostCubeClone;

    private void Start()
    {
        ghostCubeClone = Instantiate(ghostCube, new Vector3(0, 0, 0), Quaternion.identity);
        SetShootMode(false);
    }

    // Update is called once per frame
    void Update()
    {
        ray = fpsCam.ScreenPointToRay(Input.mousePosition);

        if (Input.GetButtonDown("Fire1"))
        {
            if (shootMode)
            {
                Shoot();
            }
            else if (!shootMode && blockCount > 0)
            {
                SetBlock();
                //DoRayCast(true);
            }
        }

        if (Input.GetButtonDown("Fire2") && !shootMode)
        {
            RemoveBlock();
           //DoRayCast(false);
        }
        
    }
    private void FixedUpdate()
    {
        Physics.Raycast(ray, out hit, cubePlaceRange);
        if (hit.collider != null)
        {
            if (!ghostCubeClone.activeSelf && !shootMode)
            {
                ghostCubeClone.SetActive(true);
            }
            Vector3 postition = GetBlockPosition();
            ghostCubeClone.transform.localPosition = postition;
        }
        else if(ghostCubeClone.activeSelf)
        {
            ghostCubeClone.SetActive(false);
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

    private void RemoveBlock()
    {
        Physics.Raycast(ray, out hit,cubePlaceRange);
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Block")
            {
                Destroy(hit.collider.gameObject);
                blockCount++;
            }
        }
    }

    private void SetBlock()
    {
        Physics.Raycast(ray, out hit, cubePlaceRange);
        if (hit.collider != null)
        {
            Vector3 postition = GetBlockPosition();
            Instantiate(cube, postition, Quaternion.identity);
            blockCount--;
        }
    }

    private Vector3 GetBlockPosition()
    {
        hit.point -= anchorPoint.position;
        return (new Vector3(
            Mathf.Round(hit.point.x + hit.normal.x / 2),
            Mathf.Round(hit.point.y + hit.normal.y / 2),
            Mathf.Round(hit.point.z + hit.normal.z / 2))
            + anchorPoint.position);
    }

    public void SetShootMode(bool mode)
    {
        shootMode = mode;
        ghostCubeClone.SetActive(!shootMode);
        gun.SetActive(shootMode);
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
