using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOHit : MonoBehaviour
{
    [SerializeField] private int score;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerBullet"))
        {
            GameManager.Instance.UpdateScore(score);
            SpawnManager.Instance.SpawnUFO();
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
