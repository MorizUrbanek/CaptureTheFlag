using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPlacer : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera screen;
    private RaycastHit hit;
    private Ray ray;
    public GameObject prefabBlock;

    [SerializeField] private int blockCount = 150;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        ray = screen.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (blockCount > 0)
            {
                SetBlock();
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            RemoveBlock();
        }
    }

    private void RemoveBlock()
    {
        Physics.Raycast(ray, out hit);
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
        Physics.Raycast(ray, out hit);
        if (hit.collider != null)
        {
            Vector3 postition = GetBlockPosition();
            Instantiate(prefabBlock, postition, Quaternion.identity);
            blockCount--;
        }
    }

    private Vector3 GetBlockPosition()
    {
        return new Vector3(
            Mathf.Round(hit.point.x + hit.normal.x / 2),
            Mathf.Round(hit.point.y + hit.normal.y / 2),
            Mathf.Round(hit.point.z + hit.normal.z / 2));
    }
}
