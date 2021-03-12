using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeSpawnHelper : MonoBehaviour
{
    public bool isSafe = true;

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Asteroid") | other.gameObject.CompareTag("UFO"))
            isSafe = false;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Asteroid") | other.gameObject.CompareTag("UFO"))
            isSafe = true;
    }
}
