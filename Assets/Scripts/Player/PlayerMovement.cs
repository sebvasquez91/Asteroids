using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Tooltip("Speed at which that the player rotates.")]
	[SerializeField] private float rotationSpeed = 500;
	[Tooltip("Amount of forward impulse force applied to player per FixedUpdate.")]
	[SerializeField] private float thrustForce = 50;
	[Tooltip("Player speed limit.")]
	[SerializeField] private float maxSpeed = 10;
	[Tooltip("Reference to the thrust combustion particle child object.")]
	[SerializeField] private ParticleSystem thurstParticle;
	[Tooltip("Reference to the thrust combustion sound component.")]
	[SerializeField] private AudioSource thurstSound;

	private Rigidbody rigidBody;
	private float rotationInput;

	private void Start()
	{
		rigidBody = GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		PlayerForwardMovement();
		PlayerRotation();
	}

	/// <summary>
	/// Checks for forward user input and applies impulse force to the player rigid body if its speed is less than maxSpeed.
    /// Plays accompanning particle and sound effects, and stops them when user input stops.
	/// </summary>
	private void PlayerForwardMovement()
    {
		if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && !SpawnManager.Instance.playerIsDead)
		{
			if (rigidBody.velocity.magnitude < maxSpeed)
			{
				rigidBody.AddForce(transform.up * thrustForce * Time.fixedDeltaTime, ForceMode.Impulse);
			}
			if (thurstParticle.isStopped)
			{
				thurstParticle.Play();
			}
			if (!thurstSound.isPlaying)
			{
				thurstSound.Play();
			}
		}
		else
		{
			if (thurstParticle.isPlaying)
			{
				thurstParticle.Stop();
			}
			if (thurstSound.isPlaying)
			{
				thurstSound.Stop();
			}
		}
	}

	/// <summary>
	/// Checks for horizontal user input rotates the player transform at a constant rotationSpeed.
	/// </summary>
	private void PlayerRotation()
    {
		rotationInput = Input.GetAxis("Horizontal");
		if (rotationInput != 0 && !SpawnManager.Instance.playerIsDead)
		{
			transform.Rotate(Vector3.back * rotationInput * rotationSpeed * Time.fixedDeltaTime, Space.Self);
		}
	}
}
