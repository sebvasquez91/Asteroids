using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;                     //Creates a public static reference to this script to that other scripts
                                                            //can access it from anywhere without needing to find a reference to it

    [Header("Gameplay Properties")]
    [SerializeField] private int nPlayerLives;

    [Header("UI Properties")]
    [SerializeField] private GameObject TitleScreenUI;
    [SerializeField] private GameObject PauseScreenUI;
    [SerializeField] private GameObject GameOverScreenUI;
    [SerializeField] private GameObject PlayTimeUI;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject lifeSprite;
    [SerializeField] private float lifeSpriteSpacing;

    [Header("Debugging Properties")]
    [SerializeField] public bool invinciblePlayer;
    [SerializeField] public bool infiniteLives;

    [HideInInspector] public float[] screenEdges;           // edges of the screen in world coordinates [left, right, bottom, top]
    public bool gameStarted;
    public bool gamePaused;
    public bool gameOver;

    private float originalTimeScale;
    private float camDistance;
    private int currentScore;
    public int livesLeft;
    private GameObject[] lifeSprites;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);

        screenEdges = GetScreenLimits();
        gameStarted = false;
        gamePaused = false;
        gameOver = false;
    }

    // Find the edges of the screen in world coordinates [left, right, bottom, top]
    float[] GetScreenLimits()
    {
        camDistance = Mathf.Abs(transform.position.z - Camera.main.transform.position.z);

        return new float[] {
            Camera.main.ScreenToWorldPoint(new Vector3(0, 0, camDistance)).x,               // Left edge
            Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, camDistance)).x,    // Right edge
            Camera.main.ScreenToWorldPoint(new Vector3(0, 0, camDistance)).y,               // Bottom edge
            Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, camDistance)).y    // Top edge
        };
    }


    void Update()
    {
        if (Input.GetButtonDown("Cancel") & gameStarted & !gameOver)
        {
            PauseGame();
        }
    }

    void OnMouseUp()
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

    void StartGame()
    {
        TitleScreenUI.SetActive(false);
        PlayTimeUI.SetActive(true);
        currentScore = 0;
        UpdateScore(0);
        DrawLifeSprites();
        SpawnManager.Instance.SpawnPlayer();
        SpawnManager.Instance.SpawnAsteroids();
        SpawnManager.Instance.SpawnUFO();
        originalTimeScale = Time.timeScale;     //Records the current timescale
        gameStarted = true;
    }

    void DrawLifeSprites()
    {
        if (!infiniteLives)
        {
            lifeSprites = new GameObject[nPlayerLives];
            for (int i = 0; i < nPlayerLives; i++)
            {
                lifeSprites[i] = Instantiate(lifeSprite, PlayTimeUI.transform) as GameObject;
                lifeSprites[i].GetComponent<RectTransform>().Translate(lifeSpriteSpacing * i, 0, 0);
            }
            livesLeft = nPlayerLives;
        }
    }

    public void UpdateScore(int score)
    {
        currentScore += score;
        scoreText.text = currentScore.ToString();
    }

    public void PlayerDied()
    {
        if (!infiniteLives)
        {
            if (livesLeft > 0)
            {
                livesLeft--;
                Destroy(lifeSprites[livesLeft]);
                SpawnManager.Instance.SpawnPlayer();
            }
            else
            {
                GameOver();
            }
        }
        else
            SpawnManager.Instance.SpawnPlayer();
    }

    void PauseGame()
    {
        if (!gamePaused)
        {
            //Sets the timescale to 0 (which freezes time)
            Time.timeScale = 0f;

            PauseScreenUI.SetActive(true);
            gamePaused = true;
        }
        else
        {
            //Set the timescale back to its original value
            Time.timeScale = originalTimeScale;

            PauseScreenUI.SetActive(false);
            gamePaused = false;
        }
    }

    void GameOver()
    {
        //Sets the timescale to 0 (which freezes time)
        Time.timeScale = 0f;

        GameOverScreenUI.SetActive(true);
        gameOver = true;
    }

    void RestartGame()
    {
        Time.timeScale = originalTimeScale;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
