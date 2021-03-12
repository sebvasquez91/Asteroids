using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidHit : MonoBehaviour
{
    [SerializeField] private int score;
    [SerializeField] public GameObject[] smallerAsteroids;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerBullet"))
        {
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

            Destroy(gameObject);
        }
    }
}
