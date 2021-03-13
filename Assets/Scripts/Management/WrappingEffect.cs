using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrappingEffect : MonoBehaviour
{
    private float[] playAreaLimits;
    [SerializeField] private float edgePadding = 1.0f;      //  (1.0 by default)

    private void Start()
    {
        playAreaLimits = GameManager.Instance.playAreaLimits;
    }

    private void FixedUpdate()
    {
        ScreenWrapper();
    }

    /// <summary>
	/// Keeps the transform of the gameObject within the visible play area.
    /// If the object goes over one of the side limits, it is translated to the opposite side.
	/// </summary>
    private void ScreenWrapper()
    {
        transform.position = new Vector3(
            KeepWithinLimits(transform.position.x, playAreaLimits[0], playAreaLimits[1]),
            KeepWithinLimits(transform.position.y, playAreaLimits[2], playAreaLimits[3]),
            transform.position.z);
    }

    /// <summary>
	/// Checks if a 1D position is between two values (the play area limits).
    /// If the position exceeds one of the values (plus some padding), the opposite value is returned.
    /// If the position is already between the limits, it is returned unchanged.
	/// </summary>
    /// <param name="position">A float value for the 1D position to be kept wrapped between two limits.</param>
    /// <param name="sideA">A float value for one play area limit.</param>
    /// <param name="sideB">A float value for the opposite play area limit.</param>
	/// <returns>A float that is always between the limits of sideA and sideB.</returns>
    private float KeepWithinLimits(float position, float sideA, float sideB)
    {
        if (position < (sideA - edgePadding))
        {
            return sideB;
        }
        else if (position > (sideB + edgePadding))
        {
            return sideA;
        }
        else
        {
            return position;
        }
    }

}
