using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOAttack : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float shootDelay;          // in seconds
    [SerializeField] private float shootFreq;           // in seconds
    [SerializeField] private float errorFactor;         // in seconds

    private Vector3 targetDirection;
    private Quaternion bulletRotation;

    void Start()
    {
        InvokeRepeating("ShootBullet", shootDelay, shootFreq);
    }

    void ShootBullet()
    {
        if (!SpawnManager.Instance.playerIsDead)
        {
            targetDirection = (SpawnManager.Instance.playerObject.transform.position - transform.position).normalized;
            Vector3 noiseVector = Random.Range(-errorFactor, errorFactor) * Vector2.Perpendicular(new Vector2(targetDirection.x, targetDirection.y));
            targetDirection += noiseVector;
            bulletRotation = Quaternion.LookRotation(Vector3.forward, targetDirection);
            Instantiate(bulletPrefab, transform.position, bulletRotation);
        }
    }

    void OnDisable()
    {
        CancelInvoke();
    }
}
