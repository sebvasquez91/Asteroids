using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOHit : MonoBehaviour
{
    [SerializeField] private int score;
    [SerializeField] private GameObject modelPrefab;
    [SerializeField] private ParticleSystem explosionParticle;
    [SerializeField] private Behaviour[] behavioursToDisable;

    private bool ufoHit = false;
    private float waitToDestroyTime = 1.0f;

    void OnTriggerEnter(Collider other)
    {
        if (!ufoHit & other.gameObject.CompareTag("PlayerBullet"))
        {
            ufoHit = true;
            foreach (Behaviour script in behavioursToDisable)
                script.enabled = false;
            modelPrefab.SetActive(false);
            explosionParticle.Play();

            GameManager.Instance.UpdateScore(score);

            Destroy(other.gameObject);

            StartCoroutine(waitToDestroy());
        }
    }

    IEnumerator waitToDestroy()
    {
        yield return new WaitForSeconds(waitToDestroyTime);
        SpawnManager.Instance.SpawnUFO();
        Destroy(gameObject);
    }
}
