using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float rotationInput;

	[SerializeField] private float rotationSpeed ;				//Speed at which that the player rotates
	[SerializeField] private float thrustForce;					//Amount of impulse force applied to player per FixedUpdate
    [SerializeField] private float maxSpeed;					//Player speed limit
	[SerializeField] private ParticleSystem thurstParticle;

	private Rigidbody rigidBody;								//Reference to the rigidbody component


	void Start()
	{
		rigidBody = GetComponent<Rigidbody>();
	}

	void FixedUpdate()
	{
		if ((Input.GetKey(KeyCode.W) | Input.GetKey(KeyCode.UpArrow)) & rigidBody.velocity.magnitude < maxSpeed)
        {
			rigidBody.AddForce(transform.up * thrustForce * Time.fixedDeltaTime, ForceMode.Impulse);
			thurstParticle.Play();
		}
		else
			thurstParticle.Stop();

		rotationInput = Input.GetAxis("Horizontal");
		transform.Rotate(Vector3.back * rotationInput * rotationSpeed * Time.fixedDeltaTime, Space.Self);

	}
}
