using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float bulletMaxTime;

    private float timeAlive;

    void Start()
    {
        timeAlive = 0;
    }

    void FixedUpdate()
    {
        transform.Translate(Vector3.up * bulletSpeed * Time.fixedDeltaTime, Space.Self);
        timeAlive += Time.deltaTime;
        if (timeAlive > bulletMaxTime)
            Destroy(gameObject);
    }
}
