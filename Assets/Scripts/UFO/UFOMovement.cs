using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOMovement : MonoBehaviour
{
    [Tooltip("Default constant movement speed for the UFO enemy.")]
    [SerializeField] private float ufoSpeed = 1.5f;
    [Tooltip("How often the UFO enemy changes movement direction (in seconds).")]
    [SerializeField] private float dirChangeRate = 8;
    [Tooltip("Reference to the UFO sound component.")]
    [SerializeField] private AudioSource ufoSound;

    private Vector3 movDirection;

    private void Start()
    {
        movDirection = (-transform.position).normalized;                    // The UFO enemy will start moving towards the center of the play area
        InvokeRepeating("RandomDirection", dirChangeRate, dirChangeRate);   // The XY direction of movement will randomly change every dirChangeRate seconds
        ufoSound.Play();
    }

    private void FixedUpdate()
    {
        // The UFO will keep on moving at a constant speed on the current movDirection XY direction
        transform.Translate(movDirection * ufoSpeed * Time.fixedDeltaTime, Space.World);
    }

    // Finds a random 2D direction on which the UFO enemy will move
    private void RandomDirection()
    {
        movDirection = Random.insideUnitCircle.normalized;
    }

    private void OnDisable()
    {
        CancelInvoke();
        ufoSound.Stop();
    }

}
