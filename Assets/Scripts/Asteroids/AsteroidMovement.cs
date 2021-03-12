using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidMovement : MonoBehaviour
{
    [SerializeField] private float asteroidSpeed;
    [SerializeField] private float maxTorque;

    private Rigidbody asteroidRb;
    private Vector3 movDirection;

    void Start()
    {
        asteroidRb = GetComponent<Rigidbody>();
        asteroidRb.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse);
        movDirection = Random.insideUnitCircle.normalized;
    }

    void FixedUpdate()
    {
        transform.Translate(movDirection * asteroidSpeed * Time.fixedDeltaTime, Space.World);
    }

    float RandomTorque()
    {
        return Random.Range(-maxTorque, maxTorque);
    }
}
