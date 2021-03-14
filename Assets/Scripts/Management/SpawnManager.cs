using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	//Creates a public static reference to the SpawnManager class so that other
	//scripts can access it from anywhere without needing to find a reference to it
	public static SpawnManager Instance;                            

	[Header("Player Properties")]
	[SerializeField] private GameObject playerPrefab;
	[SerializeField] private GameObject defaultPlayerRespawnPoint;

	[Header("Asteroid Properties")]
	[SerializeField] private GameObject[] asteroidsPrefabs;
	[SerializeField] private int[] spawnedAsteroidPerType;
	[SerializeField] private float safeZoneRange;

	[Header("UFO Properties")]
	[SerializeField] private GameObject ufoPrefab;
	[SerializeField] private float ufoSpawnRate = 10.0f;

	[HideInInspector] public GameObject playerObject;
	[HideInInspector] public GameObject ufoObject;
	[HideInInspector] public bool playerIsDead = false;
	private float[] playAreaLimits;
	private GameObject[] allRespawnPoints;
	private List<GameObject> usedRespawnPoints;
	private bool safePointFound = false;
	private Vector3 safeRespawnPos;

	private void Awake()
	{
		// Makes sure that one and only one static Instance of the SpawnManager class is active per scene
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy(this);
		}
	}

	private void Start()
    {
		playAreaLimits = GameManager.Instance.playAreaLimits;  // Instance.playAreaLimits is defined on GameManager's Awake() so keep this on Start() to prevent exceptions
		FindVisibleSpawnPoints();
	}

	/// <summary>
	/// Finds the spawn points within the visible play area, in order to adapt game to different screen sizes.
	/// </summary>
	private void FindVisibleSpawnPoints()
	{
		allRespawnPoints = GameObject.FindGameObjectsWithTag("Respawn");
		usedRespawnPoints = new List<GameObject>();
		foreach (GameObject respawnPoint in allRespawnPoints)
        {
			if (respawnPoint.transform.position.x > playAreaLimits[0] &&
				respawnPoint.transform.position.x < playAreaLimits[1] &&
				respawnPoint.transform.position.y > playAreaLimits[2] &&
				respawnPoint.transform.position.y < playAreaLimits[3])
			{
				usedRespawnPoints.Add(respawnPoint);
            }
        }
	}

	/// <summary>
	/// Spawns player at a safe location. If one is not found, wait until next FixedUpdate and try again.
	/// </summary>
	public IEnumerator SpawnPlayer()
    {
		safePointFound = false;
		while (!safePointFound)
		{
			yield return new WaitForFixedUpdate();
			safeRespawnPos = GetSafeRespawnPos();
		}
		playerObject = Instantiate(playerPrefab, safeRespawnPos, playerPrefab.transform.rotation) as GameObject;
		playerIsDead = false;
	}

	/// <summary>
	/// Checks if the center of the play area or any of the used spawn points is free of colliding objects.
	/// </summary>
	/// <returns>A Vector3 with the safe player spawning position.</returns>
	private Vector3 GetSafeRespawnPos()
	{
		if (defaultPlayerRespawnPoint.GetComponent<SafeSpawnHelper>().isSafe)
		{
			safePointFound = true;
			safeRespawnPos = defaultPlayerRespawnPoint.transform.position;
		}
		else
		{
			foreach (GameObject spawnPoint in usedRespawnPoints)
			{
				if (spawnPoint.GetComponent<SafeSpawnHelper>().isSafe)
				{
					safePointFound = true;
					return spawnPoint.transform.position;
				}
			}
		}
		return Vector3.zero;
	}

	/// <summary>
	/// Called by the AsteroidHit script of the last remaining asteroid to trigger the respawning of a new batch.
	/// </summary>
	public void LastAsteroidDestroyed()
	{
		StartCoroutine(AsteroidSpawnTimer());
	}

	/// <summary>
	/// Waits until a player object has been respawned and the last asteroid removed to call the spawner of a new batch of asteroids.
	/// </summary>
	public IEnumerator AsteroidSpawnTimer()
	{
		if (playerObject == null || GameObject.FindGameObjectsWithTag("Asteroid").Length > 0)
        {
			yield return new WaitForFixedUpdate();
		}
		SpawnAsteroids();
	}

	/// <summary>
	/// Spawns of a new batch of asteroids at random locations and at a given safe distance from the player.
    /// A specific numbers of asteroids of each size might be spawned.
	/// </summary>
	private void SpawnAsteroids()
    {
		for (int a = 0; a < asteroidsPrefabs.Length; a++)
		{
			for (int i = 0; i < spawnedAsteroidPerType[a]; i++)
			{
				Instantiate(asteroidsPrefabs[a], RandomAsteroidSpawnPosition(), asteroidsPrefabs[a].transform.rotation);
			}
		}
	}

	/// <summary>
	/// Called by the UFOHit script of a hit UFO to start the spawn timer of the next one.
	/// </summary>
	public void UFODestroyed()
	{
		StartCoroutine(UFOSpawnTimer());
	}

	/// <summary>
	/// Starts a timer for spawning a new UFO enemy.
	/// </summary>
	public IEnumerator UFOSpawnTimer()
	{
		yield return new WaitForSeconds(ufoSpawnRate);
		SpawnUFO();
	}

	/// <summary>
	/// Spawns a new UFO enemy at a random location on the edge of the screen.
	/// </summary>
	private void SpawnUFO()
    {
		ufoObject = Instantiate(ufoPrefab, RandomEdgePosition(), ufoPrefab.transform.rotation) as GameObject; 
	}

	/// <summary>
	/// Finds a random XY location that is outside of the safe zone surrounding the player.
	/// </summary>
	/// <returns>A Vector3 with a random XY spawning position, and zero in the Z position.</returns>
	private Vector3 RandomAsteroidSpawnPosition()
	{
		float[] x_choices = new float[] {
			Random.Range(playAreaLimits[0], playerObject.transform.position.x - safeZoneRange),
			Random.Range(playerObject.transform.position.x + safeZoneRange, playAreaLimits[1])};
		float[] y_choices = new float[] {
			Random.Range(playAreaLimits[2], playerObject.transform.position.y - safeZoneRange),
			Random.Range(playerObject.transform.position.y + safeZoneRange, playAreaLimits[3])};
		return new Vector3(x_choices[Random.Range(0,2)], y_choices[Random.Range(0, 2)], 0);
	}

	/// <summary>
	/// Finds a random XY location on one of the edges of the play area, for spawning a UFO enemy.
	/// </summary>
	/// <returns>A Vector3 with a random XY edge spawning position, and zero in the Z position.</returns>
	private Vector3 RandomEdgePosition()
	{
		float[] x_choices = new float[] { playAreaLimits[0], playAreaLimits[1] };
		float[] y_choices = new float[] { playAreaLimits[2], playAreaLimits[3] };
		if (Random.value > 0.5)
        {
			return new Vector3(x_choices[Random.Range(0, 2)], Random.Range(playAreaLimits[2], playAreaLimits[3]), 0);
		}
		else
		{
			return new Vector3(Random.Range(playAreaLimits[0], playAreaLimits[1]), y_choices[Random.Range(0, 2)], 0);

		}
	}
}
