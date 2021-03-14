using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Checks if the collider around a spawning point is free of enemies.
public class SafeSpawnHelper : MonoBehaviour
{
    public bool isSafe = true;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Asteroid") || other.gameObject.CompareTag("UFO"))
        {
            isSafe = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Asteroid") || other.gameObject.CompareTag("UFO"))
        {
            isSafe = true;
        }
    }
}
