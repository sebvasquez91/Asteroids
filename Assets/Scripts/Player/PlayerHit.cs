using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    [Tooltip("Reference to the model player ship child object.")]
    [SerializeField] private GameObject modelObject;
    [Tooltip("ference to the explosion particle child object.")]
    [SerializeField] private ParticleSystem explosionParticle;
    [Tooltip("Reference to the explosion sound component.")]
    [SerializeField] private AudioSource explosionSound;

    private float waitToDestroyTime = 1.0f;

    // When player is hit by an asteroid or a bullet shot by the UFO, play sound & particle effect, then destroy player
    private void OnTriggerEnter(Collider other)
    {
        if (!SpawnManager.Instance.playerIsDead && (other.gameObject.CompareTag("Asteroid") || other.gameObject.CompareTag("UFOBullet")))
        {
            if (!GameManager.Instance.invinciblePlayer)
            {
                SpawnManager.Instance.playerIsDead = true;
                gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                modelObject.SetActive(false);
                explosionParticle.Play();
                explosionSound.Play();
                StartCoroutine(waitToDestroy());
            }

            if (other.gameObject.CompareTag("UFOBullet"))
            {
                Destroy(other.gameObject);  // Bullets are destroyed in the process
            }
        }
    }

    /// <summary>
	/// Waits some time to play the particle and sound effects, before destroying game object.
	/// </summary>
    private IEnumerator waitToDestroy()
    {
        yield return new WaitForSeconds(waitToDestroyTime);
        GameManager.Instance.PlayerDied();             // Calls player respawner spending one life. If no lives are left, calls GameOver
        Destroy(gameObject);
    }
}
