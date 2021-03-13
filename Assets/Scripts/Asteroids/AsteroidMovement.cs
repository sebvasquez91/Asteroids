using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidMovement : MonoBehaviour
{
    [Tooltip("Default constant movement speed for this asteroid type.")]
    [SerializeField] private float asteroidSpeed = 1.5f;  // 1.0 for large, 1.5 for medium, and 2.0 for small asteroid
    [Tooltip("Is random torque applied when asteroid is spawn?")]
    [SerializeField] private bool applyTorque = false;
    [Tooltip("Maximum amount of torque applied on each direction.")]
    [SerializeField] private float maxTorque = 10;

    private Rigidbody asteroidRb;
    private Vector3 movDirection;

    private void Start()
    {
        asteroidRb = GetComponent<Rigidbody>();
        movDirection = Random.insideUnitCircle.normalized;  // Finds a random 2D direction on which the asteroid will move

        // A random torque force might be applied to each spawned asteroid for visual effect
        if (applyTorque)
        {
            asteroidRb.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {
        // The asteroid will keep on moving at a constant speed on the same random direction
        transform.Translate(movDirection * asteroidSpeed * Time.fixedDeltaTime, Space.World);
    }

    /// <summary>
    /// Returns a random torque value between -maxTorque and +maxTorque.
    /// </summary>
    /// <returns>A float torque value.</returns>
    private float RandomTorque()
    {
        return Random.Range(-maxTorque, maxTorque);
    }
}
