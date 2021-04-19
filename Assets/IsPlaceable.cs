using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsPlaceable : MonoBehaviour
{
    private bool isPlaceable = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Block")
        {
            if (other.transform.position == transform.position)
            {
                isPlaceable = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Block")
        {
            if (other.transform.position != transform.position)
            {
                isPlaceable = true;
            }
        }
    }

    public bool GetisPlaceable()
    {
        return isPlaceable;
    }
}
