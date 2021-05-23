using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Tooltip("Reference to prefab of bullets shot by the player.")]
    [SerializeField] private GameObject bulletPrefab;
    [Tooltip("Cool-down time for player attack.")]
    [SerializeField] private float spacebarCoolDown = 0.05f;

    private float cooldownTimer = 0.0f;


    private void Update()
    {
        if (!SpawnManager.Instance.playerIsDead && !GameManager.Instance.gamePaused)
        {
            // Implements a shooting cool-down time to prevent spamming of the spacebar
            if (cooldownTimer > spacebarCoolDown)
            {
                // On spacebar press, shoot bullet
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    cooldownTimer = 0;
                    Instantiate(bulletPrefab, transform.position, transform.rotation);
                }
            }
            cooldownTimer += Time.deltaTime;
        }
    }
}
