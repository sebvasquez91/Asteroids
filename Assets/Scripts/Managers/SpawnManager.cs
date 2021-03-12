using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	public static SpawnManager Instance;                            //Creates a public static reference to this script to that other scripts
																	//can access it from anywhere without needing to find a reference to it

	[Header("Player Properties")]
	[SerializeField] private GameObject playerPrefab;
	[SerializeField] private GameObject defaultPlayerRespawnPoint;

	[Header("Asteroid Properties")]
	[SerializeField] private GameObject[] asteroidsPrefabs;
	[SerializeField] private int[] maxAsteroidPerType;
	[SerializeField] private float safeZoneRange;

	[Header("UFO Properties")]
	[SerializeField] private GameObject UFO;
	[SerializeField] private float ufoSpawnTime = 10.0f;


	public GameObject playerObject;
	private float[] screenEdges;                                    // edges of the screen in world coordinates [left, right, bottom, top]
	private GameObject[] respawnPoints;
	private Vector3 safeRespawnPos;
	
	void Awake()
	{
		if (Instance == null)
			Instance = this;
		else if (Instance != this)
            Destroy(this);
	}

	void Start()
    {
		screenEdges = GameManager.Instance.screenEdges;
		RemoveOffScreenSpawnPoints();
	}

	void RemoveOffScreenSpawnPoints()
	{
		respawnPoints = GameObject.FindGameObjectsWithTag("Respawn");
		foreach (GameObject respawnPoint in respawnPoints)
        {
			if (respawnPoint.transform.position.x < screenEdges[0] |
				respawnPoint.transform.position.x > screenEdges[1] |
				respawnPoint.transform.position.y < screenEdges[2] |
				respawnPoint.transform.position.y > screenEdges[3])
			{
				Destroy(respawnPoint);
            }
        }
	}

	public void SpawnPlayer()
    {
		if (defaultPlayerRespawnPoint.GetComponent<SafeSpawnHelper>().isSafe)
        {
			safeRespawnPos = defaultPlayerRespawnPoint.transform.position;
		}
		else
        {
			safeRespawnPos = GetSafeRespawnPos();
		}

		playerObject = Instantiate(playerPrefab, safeRespawnPos, playerPrefab.transform.rotation) as GameObject;
    }

	public void SpawnAsteroids()
    {
		StartCoroutine(WaitSomeTime());
		for (int a = 0; a < asteroidsPrefabs.Length; a++)
		{
			for (int i = 0; i < maxAsteroidPerType[a]; i++)
			{
				Instantiate(asteroidsPrefabs[a], RandomPosition(), asteroidsPrefabs[a].transform.rotation);
			}
		}
	}

	public void SpawnUFO()
    {
		StartCoroutine(UFOTimer());
	}

	IEnumerator UFOTimer()
	{
		yield return new WaitForSeconds(ufoSpawnTime);
		Instantiate(UFO, RandomEdgePosition(), UFO.transform.rotation);
	}

	Vector3 GetSafeRespawnPos()
    {
		bool safePointFound = false;
		respawnPoints = GameObject.FindGameObjectsWithTag("Respawn");

		while (!safePointFound)
        {
			foreach (GameObject spawnPoint in respawnPoints)
			{
				if (spawnPoint.GetComponent<SafeSpawnHelper>().isSafe)
				{
					return spawnPoint.transform.position;
				}
			}
			StartCoroutine(WaitSomeTime());
		}
		return Vector3.zero;
	}

	IEnumerator WaitSomeTime()
    {
		yield return new WaitForFixedUpdate();
	}

	Vector3 RandomPosition()
	{
		float[] x_choices = new float[] {
			Random.Range(screenEdges[0], playerObject.transform.position.x - safeZoneRange),
			Random.Range(playerObject.transform.position.x + safeZoneRange, screenEdges[1])
		};
		float[] y_choices = new float[] {
			Random.Range(screenEdges[2], playerObject.transform.position.y - safeZoneRange),
			Random.Range(playerObject.transform.position.y + safeZoneRange, screenEdges[3])
		};
		return new Vector3(x_choices[Random.Range(0,2)], y_choices[Random.Range(0, 2)], 0);
	}

	Vector3 RandomEdgePosition()
	{
		float[] x_choices = new float[] { screenEdges[0], screenEdges[1] };
		float[] y_choices = new float[] { screenEdges[2], screenEdges[3] };
		if (Random.value > 0.5)
        {
			return new Vector3(x_choices[Random.Range(0, 2)], Random.Range(screenEdges[2], screenEdges[3]), 0);
		}
		else
		{
			return new Vector3(Random.Range(screenEdges[0], screenEdges[1]), y_choices[Random.Range(0, 2)], 0);

		}
	}

}
