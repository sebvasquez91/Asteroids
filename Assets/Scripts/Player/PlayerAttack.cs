using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float spacebarCoolDown;

    private float timer = 0.0f;


    private void Update()
    {
        if (!SpawnManager.Instance.playerIsDead && !GameManager.Instance.gamePaused)
        {
            // Implements a shooting cool-down time to prevent spamming of the spacebar
            if (timer > spacebarCoolDown)
            {
                // On spacebar press, shoot bullet
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    timer = 0;
                    Instantiate(bulletPrefab, transform.position, transform.rotation);
                }
            }
            timer += Time.deltaTime;
        }
    }
}
