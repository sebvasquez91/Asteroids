using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOMovement : MonoBehaviour
{
    [SerializeField] private float ufoSpeed;
    [SerializeField] private float dirChangeRate;
    [SerializeField] private AudioSource UFOSound;

    private Vector3 movDirection;

    void Start()
    {
        movDirection = (-transform.position).normalized;
        InvokeRepeating("RandomDirection", dirChangeRate, dirChangeRate);
        UFOSound.Play();
    }


    void FixedUpdate()
    {
        transform.Translate(movDirection * ufoSpeed * Time.fixedDeltaTime, Space.World);

    }

    void RandomDirection()
    {
        movDirection = Random.insideUnitCircle.normalized;
    }

    void OnDisable()
    {
        CancelInvoke();
        UFOSound.Stop();
    }

    public void UnPauseUFOSound()
    {
        UFOSound.UnPause();
    }

    public void PauseUFOSound()
    {
        UFOSound.Pause();
    }

}
