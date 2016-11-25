using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	public GameStateManager gameState;
	
	void Start () 
	{
		Initialize();
		
		gameState.GameStarted += OnGameStarted;
		gameState.GameEnded += OnGameEnded;
		gameState.IncreaseDifficulty += OnDifficultyIncreased;
		gameState.GamePaused += OnGamePaused;
	}

	void Update()
	{
		DoUpdate ();
	}

	virtual protected void Initialize()
	{
		m_spawnRate = 2f;
		m_spawnTimer = 0f;
	}

	virtual protected void OnGameStarted()
	{
		Initialize();
	}

	virtual protected void OnGameEnded()
	{
	}

	virtual protected void OnDifficultyIncreased(bool addNewObstacle)
	{
	}

	virtual protected void OnGamePaused(bool paused)
	{
	}

	virtual protected void DoUpdate()
	{
	}

	protected float m_spawnRate = 2f;
	protected float m_spawnTimer = 0f;
}
