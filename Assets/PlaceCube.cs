using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Mirror;
using Cinemachine;

public class PlaceCube : NetworkBehaviour
{
    //public Transform anchorPoint;
    public Camera fpsCam;
    public GameObject cube;
    public GameObject ghostCube;
    public float cubePlaceRange = 40f;


    [SerializeField] private int blockCount = 150;
    private RaycastHit hit;
    private Ray ray;
    private int ghostCubeCount = 0, mode = 0;
    private static int maxGhostCubes = 8;
    private GameObject[] ghostCubes = new GameObject[maxGhostCubes];
    private Vector3 addGhostCubeDirection = new Vector3(1, 0, 0);
    private bool isMirrored = false, currentState;
    private Vector3 anchorPoint = new Vector3(-49.5f,-.49999f,-49.5f);
    private GameObject ghostCubesParent;



    //Start is called before the first frame update
    //void Start()
    //{
    //    ghostCubesParent = new GameObject("ghostCubesParent");
    //    CubesParent = new GameObject("CubesParent");
    //    ghostCubes[ghostCubeCount] = Instantiate(ghostCube, new Vector3(0, 0, 0), Quaternion.identity, ghostCubesParent.transform);
    //}

    public override void OnStartAuthority()
    {
        ghostCubesParent = new GameObject("ghostCubesParent");
        ghostCubes[ghostCubeCount] = Instantiate(ghostCube, new Vector3(0, 0, 0), Quaternion.identity, ghostCubesParent.transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        ray = fpsCam.ScreenPointToRay(Input.mousePosition);

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            ghostCubeCount++;
            if (ghostCubeCount < maxGhostCubes)
            {
                ghostCubes[ghostCubeCount] = Instantiate(ghostCube, ghostCubes[ghostCubeCount - 1].transform.position + addGhostCubeDirection, Quaternion.identity, ghostCubesParent.transform);
            }
            else
            {
                ghostCubeCount = maxGhostCubes - 1;
            }
                
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            
            if (ghostCubeCount > 0)
            {
                Destroy(ghostCubes[ghostCubeCount]);
                ghostCubeCount--;
            }
        }

        if (Input.GetButtonDown("Fire1") && blockCount > 0)
        {
            SetBlock();
        }

        if (Input.GetButtonDown("Fire2"))
        {
            RemoveBlock();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && mode != 0)
        {
            ghostCubesParent.transform.eulerAngles = new Vector3(0, 0, 0);
            addGhostCubeDirection = new Vector3(1, 0, 0);
            mode = 0;
            isMirrored = false;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && mode != 1)
        {
            ghostCubesParent.transform.eulerAngles = new Vector3(0, -90, 0);
            addGhostCubeDirection = new Vector3(0, 0, 1);
            mode = 1;
            isMirrored = false;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && mode != 2)
        {
            ghostCubesParent.transform.eulerAngles = new Vector3(0, 0, 90);
            addGhostCubeDirection = new Vector3(0, 1, 0);
            mode = 2;
            isMirrored = false;
        }
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        Physics.Raycast(ray, out hit, cubePlaceRange);
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Block" || hit.collider.gameObject.layer == 8)
            {
                if (!ghostCubesParent.activeSelf)
                {
                    ghostCubesParent.SetActive(true);
                }
                ghostCubesParent.transform.position = GetBlockPosition();
                currentState = isMirrored;
                switch(mode)
                {
                    case 0:
                        if (hit.normal.x == -1)
                            isMirrored = true;
                        else
                            isMirrored = false;
                        break;

                    case 1:
                        if (hit.normal.z == -1)
                            isMirrored = true;
                        else
                            isMirrored = false;
                        break;

                    case 2:
                        if (hit.normal.y == -1)
                            isMirrored = true;
                        else
                            isMirrored = false;
                        break;
                }
                if (isMirrored != currentState)
                {
                    if (mode == 0)
                    {
                        ghostCubesParent.transform.eulerAngles += new Vector3(0, 180, 0);
                    }
                    else
                    {
                        ghostCubesParent.transform.eulerAngles *= -1;
                    }
                    addGhostCubeDirection *= -1;
                }
            }
            else if (ghostCubesParent.activeSelf)
            {
                ghostCubesParent.SetActive(false);
            }
        }
        else if (ghostCubesParent.activeSelf)
        {
            ghostCubesParent.SetActive(false);
        }

    }

    private void RemoveBlock()
    {
        Physics.Raycast(ray, out hit, cubePlaceRange);
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Block")
            {
                CmdDestroyObject(hit.collider.gameObject);
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
                IsPlaceable isPlaceable;
                foreach (GameObject ghostCube in ghostCubes.Where(i => i != null))
                {
                    isPlaceable = ghostCube.GetComponent<IsPlaceable>();
                    if (isPlaceable != null 
                        && Mathf.Abs(ghostCube.transform.position.x) <= Mathf.Abs(anchorPoint.x)
                        && ghostCube.transform.position.y >= Mathf.Abs(anchorPoint.y)
                        && Mathf.Abs(ghostCube.transform.position.z) <= Mathf.Abs(anchorPoint.z))
                    {
                        if (isPlaceable.GetisPlaceable())
                        {
                            CmdSpawnObject(ghostCube.transform.position,gameObject);
                            blockCount--;
                        }
                    }

                }
                
            }
        }
    }

    [Command]
    public void CmdSpawnObject(Vector3 position,GameObject owner)
    {
        GameObject serverCube = Instantiate(cube, position, Quaternion.identity);
        NetworkServer.Spawn(serverCube, owner);
    }

    [Command]
    public void CmdDestroyObject(GameObject cube)
    {
        NetworkServer.Destroy(cube);
    }



    private Vector3 GetBlockPosition()
    {
        hit.point -= anchorPoint;
        return (new Vector3(
            Mathf.Round(hit.point.x + hit.normal.x / 2),
            Mathf.Round(hit.point.y + hit.normal.y / 2),
            Mathf.Round(hit.point.z + hit.normal.z / 2))
            + anchorPoint);
    }

    public void SetGhostCubeActive(bool active)
    {
        ghostCubesParent.SetActive(active);
    }
}
