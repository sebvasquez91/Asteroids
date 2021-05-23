using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOAttack : MonoBehaviour
{
    [Tooltip("Reference to prefab of bullets shot by the UFO enemy.")]
    [SerializeField] private GameObject bulletPrefab;
    [Tooltip("Delay in start of shooting in seconds.")]
    [SerializeField] private float shootDelay = 2.0f;
    [Tooltip("UFO shooting rate in seconds.")]
    [SerializeField] private float shootRate = 1.0f;
    [Tooltip("Sets the range of posible shooting angles, with 1.0 being the largest and most imprecise (180 deg) and 0.0 giving no error.")]
    [SerializeField] private float errorFactor = 0.3f;

    private void Start()
    {
        // The UFO enemy will shoot a bullet every shootRate seconds, starting shootDelay seconds after spawning
        InvokeRepeating("ShootBullet", shootDelay, shootRate);
    }

    /// <summary>
	/// Shoots a bullet from the UFO towards the current direction of the player.
	/// </summary>
    private void ShootBullet()
    {
        if (!SpawnManager.Instance.playerIsDead)
        {
            Instantiate(bulletPrefab, transform.position, GetBulletDirection());
        }
    }

    /// <summary>
	/// Finds the direction at which the bullet from the UFO to the player should be shot, plus some error determined by errorFactor.
	/// </summary>
    /// /// <returns>A Quaternion that will rotate the spawned bullet towards the player.</returns>
    private Quaternion GetBulletDirection()
    {
        Vector3 targetDirection = (SpawnManager.Instance.playerObject.transform.position - transform.position).normalized;
        Vector3 noiseVector = Random.Range(-errorFactor, errorFactor) * Vector2.Perpendicular(new Vector2(targetDirection.x, targetDirection.y));
        targetDirection += noiseVector;
        return Quaternion.LookRotation(Vector3.forward, targetDirection);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}
