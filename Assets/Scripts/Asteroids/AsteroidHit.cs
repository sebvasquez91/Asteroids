using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidHit : MonoBehaviour
{
    [Tooltip("How many points are awarded when specific asteroid type is hit.")]
    [SerializeField] private int score = 50;  // 20 for large, 50 for medium, and 100 for small asteroid
    [Tooltip("Reference to the model asteroid child object.")]
    [SerializeField] private GameObject modelObject;
    [Tooltip("Reference to the dust particle child object.")]
    [SerializeField] private ParticleSystem explosionParticle;
    [Tooltip("Reference to the explosion sound component.")]
    [SerializeField] private AudioSource explosionSound;
    [Tooltip("Prefab of the asteroids to be spawned after this one is destroyed.")]
    [SerializeField] private GameObject splitAsteroidsPrefab;

    private Behaviour[] behavioursToDisable;
    private bool asteroidHit = false;
    private float waitToDestroyTime = 1.0f;

    private void Start()
    {
        behavioursToDisable = new Behaviour[] {
            gameObject.GetComponent<AsteroidMovement>(),
            gameObject.GetComponent<WrappingEffect>()
        };
    }

    // When asteroid is hit by a bullet shot by the player, play sound & particle effect, then destroy asteroid
    private void OnTriggerEnter(Collider other)
    {
        if (!asteroidHit && other.gameObject.CompareTag("PlayerBullet"))
        {
            asteroidHit = true;
            // Disable collider and some of the object behaviours while destroy effects are being played
            gameObject.GetComponent<Collider>().enabled = false;
            foreach (Behaviour script in behavioursToDisable)
            {
                script.enabled = false;
            }
            modelObject.SetActive(false);
            explosionParticle.Play();
            explosionSound.Play();

            GameManager.Instance.UpdateScore(score);

            Destroy(other.gameObject);  // Bullets are destroyed in the process

            // Instantiate two smaller asteroids if a prefab has been set
            if (splitAsteroidsPrefab != null)
            {
                Instantiate(splitAsteroidsPrefab, transform.position, transform.rotation);
                Instantiate(splitAsteroidsPrefab, transform.position, transform.rotation);
            }

            StartCoroutine(waitToDestroy());
        }
    }

    /// <summary>
	/// Waits some time to play the particle and sound effects, before destroying game object.
	/// </summary>
    private IEnumerator waitToDestroy()
    {
        yield return new WaitForSeconds(waitToDestroyTime);
        if (GameObject.FindGameObjectsWithTag("Asteroid").Length == 1)
        {
            SpawnManager.Instance.LastAsteroidDestroyed();      // If this is the last remaining asteroid, call the spawner of a new batch
        }
        Destroy(gameObject);
    }
}
