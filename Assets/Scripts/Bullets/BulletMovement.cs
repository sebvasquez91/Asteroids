using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 10;
    [SerializeField] private float bulletLifeTime = 1.0f;

    private float timeAlive;

    private void Start()
    {
        timeAlive = 0;
    }

    private void FixedUpdate()
    {
        // The bullet will keep on moving forward along its Y axes at a constant speed and be destroyed after bulletLifeTime seconds
        transform.Translate(Vector3.up * bulletSpeed * Time.fixedDeltaTime, Space.Self);
        timeAlive += Time.fixedDeltaTime;
        if (timeAlive > bulletLifeTime)
        {
            Destroy(gameObject);
        }
    }
}
