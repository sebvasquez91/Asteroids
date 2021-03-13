using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOHit : MonoBehaviour
{
    [Tooltip("How many points are awarded when UFO enemy is hit.")]
    [SerializeField] private int score = 200;
    [Tooltip("Reference to the model UFO child object.")]
    [SerializeField] private GameObject modelObject;
    [Tooltip("ference to the explosion particle child object.")]
    [SerializeField] private ParticleSystem explosionParticle;
    [Tooltip("Reference to the explosion sound component.")]
    [SerializeField] private AudioSource explosionSound;

    private Behaviour[] behavioursToDisable;
    private bool ufoHit = false;
    private float waitToDestroyTime = 1.0f;

    private void Start()
    {
        behavioursToDisable = new Behaviour[] {
            gameObject.GetComponent<UFOAttack>(),
            gameObject.GetComponent<UFOMovement>(),
            gameObject.GetComponent<WrappingEffect>()
        };
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!ufoHit && other.gameObject.CompareTag("PlayerBullet"))
        {
            ufoHit = true;
            foreach (Behaviour script in behavioursToDisable)
            {
                script.enabled = false;
            }
            modelObject.SetActive(false);
            explosionParticle.Play();
            explosionSound.Play();

            GameManager.Instance.UpdateScore(score);

            Destroy(other.gameObject);

            StartCoroutine(waitToDestroy());
        }
    }

    /// <summary>
	/// Waits some time to play the particle and sound effects, before destroying game object.
	/// </summary>
    private IEnumerator waitToDestroy()
    {
        yield return new WaitForSeconds(waitToDestroyTime);
        SpawnManager.Instance.UFODestroyed();
        Destroy(gameObject);
    }
}
