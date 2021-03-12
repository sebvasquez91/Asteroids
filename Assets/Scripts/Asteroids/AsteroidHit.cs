using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidHit : MonoBehaviour
{
    [SerializeField] private int score;
    [SerializeField] private GameObject modelPrefab;
    [SerializeField] private ParticleSystem explosionParticle;
    [SerializeField] private AudioSource explosionSound;
    [SerializeField] public GameObject[] smallerAsteroids;
    [SerializeField] private Behaviour[] behavioursToDisable;

    private bool asteroidHit = false;
    private float waitToDestroyTime = 1.0f;

    void OnTriggerEnter(Collider other)
    {
        if (!asteroidHit & other.gameObject.CompareTag("PlayerBullet"))
        {
            asteroidHit = true;
            foreach (Behaviour script in behavioursToDisable)
                script.enabled = false;
            modelPrefab.SetActive(false);
            explosionParticle.Play();
            explosionSound.Play();

            GameManager.Instance.UpdateScore(score);

            Destroy(other.gameObject);

            if (smallerAsteroids.Length > 0)
            {
                foreach (GameObject asteroid in smallerAsteroids)
                {
                    if (asteroid != null)
                    {
                        Instantiate(asteroid, transform.position, transform.rotation);
                    }
                }
            }

            if (GameObject.FindGameObjectsWithTag("Asteroid").Length == 1)
                SpawnManager.Instance.SpawnAsteroids();

            StartCoroutine(waitToDestroy());
        }
    }

    IEnumerator waitToDestroy()
    {
        yield return new WaitForSeconds(waitToDestroyTime);
        Destroy(gameObject);
    }
}
