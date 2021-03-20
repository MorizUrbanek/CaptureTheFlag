using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridScript : MonoBehaviour
{
    private int site = 40;
    public float size = 1f;

    private bool[,] usedPositions;

    private void Start()
    {
        usedPositions = new bool[site, site];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //Debug.Log($"0,0 {usedPositions[0, 0]} 0,1 {usedPositions[0, 1]} 0,2 {usedPositions[0, 2]} 0,3 {usedPositions[0, 3]} 0,4 {usedPositions[0, 4]}");
            //Debug.Log($"1,0 {usedPositions[1, 0]} 1,0 {usedPositions[1, 0]} 2,0 {usedPositions[2, 0]} 3,0 {usedPositions[3, 0]} 4,0 {usedPositions[4, 0]}");
        }
    }


    public Vector3 GetNearestPointOnGrid(Vector3 clickpoint, out bool isUsed)
    {
        clickpoint -= transform.position;

        int xCount = Mathf.RoundToInt(clickpoint.x / size);
        int yCount = Mathf.RoundToInt(clickpoint.y / size);
        int zCount = Mathf.RoundToInt(clickpoint.z / size);

        Vector3 result = new Vector3((float)xCount * size, (float)yCount * size, (float)zCount * size);

        result += transform.position;
        isUsed = usedPositions[Mathf.RoundToInt(result.z + Mathf.Abs(transform.position.z)), Mathf.RoundToInt(result.x + Mathf.Abs(transform.position.x))];
        Debug.Log($"Hit the Point in the Array [{Mathf.RoundToInt(result.z + Mathf.Abs(transform.position.z))},{Mathf.RoundToInt(result.x + Mathf.Abs(transform.position.x))}] with the Status {isUsed}");
        return result;
    }
    public void SetPosition(int z, int x, bool isUsed)
    {
        usedPositions[Mathf.RoundToInt(z + Mathf.Abs(transform.position.z)), Mathf.RoundToInt(x + Mathf.Abs(transform.position.x))] = isUsed;
        Debug.Log($"Array Position [{z + Mathf.Abs(transform.position.z)},{x + Mathf.Abs(transform.position.x)}] ");
        Debug.Log($"Set the Point in the Array [{Mathf.RoundToInt(z + Mathf.Abs(transform.position.z))},{Mathf.RoundToInt(x + Mathf.Abs(transform.position.x))}] with the Status {isUsed}");

    }

}
