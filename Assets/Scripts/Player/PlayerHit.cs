using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Asteroid") | other.gameObject.CompareTag("UFOBullet")) 
        {
            GameManager.Instance.PlayerDied();

            if (other.gameObject.CompareTag("UFOBullet"))
            {
                Destroy(other.gameObject);
            }

            if (!GameManager.Instance.invinciblePlayer)
            {
                Destroy(gameObject);
            }
        }
    }
}
