using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    [SerializeField] private GameObject modelPrefab;
    [SerializeField] private ParticleSystem explosionParticle;
    [SerializeField] private Behaviour[] behavioursToDisable;

    private bool playerHit = false;
    private float waitToDestroyTime = 1.0f;

    void OnTriggerEnter(Collider other)
    {
        if (!playerHit & (other.gameObject.CompareTag("Asteroid") | other.gameObject.CompareTag("UFOBullet")))
        {
            if (!GameManager.Instance.invinciblePlayer)
            {
                playerHit = true;
                SpawnManager.Instance.playerIsDead = true;
                foreach (Behaviour script in behavioursToDisable)
                    script.enabled = false;
                modelPrefab.SetActive(false);
                explosionParticle.Play();
                StartCoroutine(waitToDestroy());
            }

            if (other.gameObject.CompareTag("UFOBullet"))
            {
                Destroy(other.gameObject);
            }
        }
    }

    IEnumerator waitToDestroy()
    {
        yield return new WaitForSeconds(waitToDestroyTime);
        GameManager.Instance.PlayerDied();
        Destroy(gameObject);
    }
}
