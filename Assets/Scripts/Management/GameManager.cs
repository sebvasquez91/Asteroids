using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Creates a public static reference to the GameManager class so that other
    //scripts can access it from anywhere without needing to find a reference to it
    public static GameManager Instance;                     

    [Header("Gameplay Properties")]
    [SerializeField] private int nPlayerLives = 5;

    [Header("UI Properties")]
    [SerializeField] private GameObject titleScreenUI;
    [SerializeField] private GameObject pauseScreenUI;
    [SerializeField] private GameObject gameOverScreenUI;
    [SerializeField] private GameObject playTimeUI;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject lifeSpritePrefab;
    [SerializeField] private float lifeSpriteSpacing = 30;

    [Header("Debugging Properties")]
    [SerializeField] public bool invinciblePlayer = false;
    [SerializeField] public bool infiniteLives = false;

    [HideInInspector] public float[] playAreaLimits;
    [HideInInspector] public bool gameStarted = false;
    [HideInInspector] public bool gamePaused = false;
    [HideInInspector] public bool gameOver = false;

    private float originalTimeScale;
    private float camDistance;
    private int currentScore;
    private int livesLeft;
    private List<GameObject> lifeSprites;
    private AudioSource[] allAudioSources;

    private void Awake()
    {
        // Makes sure that one and only one static Instance of the GameManager class is active per scene
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
        playAreaLimits = GetPlayAreaLimits();
        gameStarted = false;
        gamePaused = false;
        gameOver = false;
    }

    /// <summary>
    /// Finds the limits of the visible play area in world coordinates, to adapt game to different screen sizes.
    /// </summary>
    /// <returns>A float array with world coordinates [left, right, bottom, top].</returns>
    private float[] GetPlayAreaLimits()
    {
        camDistance = Mathf.Abs(transform.position.z - Camera.main.transform.position.z);

        return new float[] {
            Camera.main.ScreenToWorldPoint(new Vector3(0, 0, camDistance)).x,               // Left limit
            Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, camDistance)).x,    // Right limit
            Camera.main.ScreenToWorldPoint(new Vector3(0, 0, camDistance)).y,               // Bottom limit
            Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, camDistance)).y    // Top limit
        };
    }


    private void Update()
    {
        if (gameStarted && !gameOver && Input.GetButtonDown("Cancel"))
        {
            PauseGame();
        }
    }

    private void OnMouseUp()
    {
        if (!gameOver)
        {
            if (!gameStarted)
            {
                StartGame();
            }
            else
            {
                PauseGame();
            }
        }
        else
        {
            RestartGame();
        }
    }

    /// <summary>
    /// Transitions from the title screen into the game, setting up game UI and calling spawners.
    /// </summary>
    private void StartGame()
    {
        titleScreenUI.SetActive(false);
        playTimeUI.SetActive(true);
        if (!infiniteLives)
        {
            AddLives(nPlayerLives);
        }
        currentScore = 0;
        UpdateScore(0);
        StartCoroutine(SpawnManager.Instance.SpawnPlayer());
        StartCoroutine(SpawnManager.Instance.AsteroidSpawnTimer());
        StartCoroutine(SpawnManager.Instance.UFOSpawnTimer());
        originalTimeScale = Time.timeScale;
        gameStarted = true;
    }

    /// <summary>
    /// Adds lives to the player life counter and to the UI.
    /// </summary>
    /// <param name="nLivesToAdd">The number of lives to add (non-negative integer).</param>
    private void AddLives(int nLivesToAdd)
    {
        livesLeft += nLivesToAdd;
        if (lifeSprites == null)
        {
            lifeSprites = new List<GameObject>();
        }
        for (int i = 0; i < nLivesToAdd; i++)
        {
            lifeSprites.Add(Instantiate(lifeSpritePrefab, playTimeUI.transform) as GameObject);
            lifeSprites[lifeSprites.Count - 1].GetComponent<RectTransform>().Translate(lifeSpriteSpacing * lifeSprites.Count, 0, 0);
        }
    }

    /// <summary>
    /// Removes lives from the player life counter and from the UI.
    /// </summary>
    /// <param name="nLivesToRemove">The number of lives to remove (non-negative integer).</param>
    private void RemoveLives(int nLivesToRemove)
    {
        if (lifeSprites != null)
        {
            livesLeft -= nLivesToRemove;
            for (int i = 0; i < nLivesToRemove; i++)
            {
                Destroy(lifeSprites[lifeSprites.Count - 1 - i]);
                lifeSprites.RemoveAt(lifeSprites.Count - 1 - i);
            }
        }
    }

    /// <summary>
    /// Adds (or substracts) a value to the counter counter of the UI.
    /// </summary>
    /// <param name="scoreToAdd">Any integer to add (or substract) to the score counter.</param>
    public void UpdateScore(int scoreToAdd)
    {
        currentScore += scoreToAdd;
        scoreText.text = currentScore.ToString();
    }

    /// <summary>
    /// Calls player respawner spending one player life. If no lives are left calls GameOver.
    /// </summary>
    public void PlayerDied()
    {
        if (!infiniteLives)
        {
            if (livesLeft > 0)
            {
                RemoveLives(1);
                StartCoroutine(SpawnManager.Instance.SpawnPlayer());
            }
            else
            {
                GameOver();
            }
        }
        else
        {
            StartCoroutine(SpawnManager.Instance.SpawnPlayer());
        }
    }

    /// <summary>
    /// Pauses/unpauses the game and shows/hides pause screen.
    /// </summary>
    private void PauseGame()
    {
        gamePaused = !gamePaused;
        pauseScreenUI.SetActive(gamePaused);
        
        if (gamePaused)
        {
            originalTimeScale = Time.timeScale;
            Time.timeScale = 0f;                    //Sets the timescale to 0 (which freezes time)
        }
        else
        {
            Time.timeScale = originalTimeScale;     //Set the timescale back to its original value
        }

        AudioListener.pause = gamePaused;
    }

    /// <summary>
    /// Ends the game and shows Game Over screen.
    /// </summary>
    private void GameOver()
    {
        Time.timeScale = 0f; //Sets the timescale to 0 (which freezes time)
        StopAllAudio();
        gameOverScreenUI.SetActive(true);
        gameOver = true;
    }

    /// <summary>
    /// Stops all sound sources currently playing.
    /// </summary>
    private void StopAllAudio()
    {
        allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        foreach (AudioSource audioS in allAudioSources)
        {
            if (audioS.isPlaying)
            {
                audioS.Stop();
            }
        }
    }

    /// <summary>
    /// Restarts the game by reloading the scene.
    /// </summary>
    private void RestartGame()
    {
        Time.timeScale = originalTimeScale;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
