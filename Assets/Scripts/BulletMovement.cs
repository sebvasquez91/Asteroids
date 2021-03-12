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

    void Update()
    {
        transform.Translate(Vector3.up * bulletSpeed * Time.deltaTime, Space.Self);
        timeAlive += Time.deltaTime;
        if (timeAlive > bulletMaxTime)
            Destroy(gameObject);
    }
}
