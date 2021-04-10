using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceCube : MonoBehaviour
{
    public Transform anchorPoint;
    public Camera fpsCam;
    public GameObject cube;
    public GameObject ghostCube;
    public float cubePlaceRange = 40f;


    [SerializeField] private int blockCount = 150;
    private RaycastHit hit;
    private Ray ray;
    private GameObject ghostCubeClone;


    // Start is called before the first frame update
    void Start()
    {
        ghostCubeClone = Instantiate(ghostCube, new Vector3(0, 0, 0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        ray = fpsCam.ScreenPointToRay(Input.mousePosition);
        if (Input.GetButtonDown("Fire1") && blockCount > 0)
        {
            SetBlock();
        }
        if (Input.GetButtonDown("Fire2"))
        {
            RemoveBlock();
        }
    }

    private void FixedUpdate()
    {
        Physics.Raycast(ray, out hit, cubePlaceRange);
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Block" || hit.collider.gameObject.layer == 8)
            {
                if (!ghostCubeClone.activeSelf)
                {
                    ghostCubeClone.SetActive(true);
                }
                Vector3 postition = GetBlockPosition();
                ghostCubeClone.transform.position = postition;
            }
            else if (ghostCubeClone.activeSelf)
            {
                ghostCubeClone.SetActive(false);
            }
        }
        else if (ghostCubeClone.activeSelf)
        {
            ghostCubeClone.SetActive(false);
        }
    }

    private void RemoveBlock()
    {
        Physics.Raycast(ray, out hit, cubePlaceRange);
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
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Block" || hit.collider.gameObject.layer == 8)
            {
                Instantiate(cube, ghostCubeClone.transform.position, Quaternion.identity);
                blockCount--;
            }
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

    public void SetGhostCubeActive(bool active)
    {
        ghostCubeClone.SetActive(active);
    }
}
