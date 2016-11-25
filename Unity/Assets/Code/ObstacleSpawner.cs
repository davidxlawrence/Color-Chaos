using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstacleSpawner : Spawner {

	public List<ObstacleManager> obstacleTypes;

	private void SpawnNewObstacle()
	{
		ObstacleManager o = obstacleTypes[m_currentObstacleCycle];

		if(o.CanSpawnObstacle())
		{
			o.SpawnObstacle();
			IncrementCurrentObstacleCycle();
		}
	}
	
	override protected void OnGameEnded()
	{
		base.OnGameEnded();

		Obstacle[] obstacles = GameObject.FindObjectsOfType<Obstacle>();
		foreach(Obstacle o in obstacles)
		{
			o.Deactivate();
		}
	}
	
	override protected void DoUpdate()
	{
		if(gameState.IsPlayingGame)
		{
			m_spawnTimer += Time.deltaTime;
			if(m_spawnTimer > m_spawnRate) {
				SpawnNewObstacle();
				m_spawnTimer = 0.0f;
			}
		}
	}

	override protected void Initialize()
	{
		m_spawnRate = 3f;
		m_spawnTimer = 0f;
		m_obstacleTier = 0;
		m_currentObstacleCycle = 0;
	}

	override protected void OnDifficultyIncreased(bool addNewObstacles)
	{
		if (addNewObstacles && !IsAtFinalObstacleTier()) 
		{
			m_spawnRate = 2.0f;
			m_spawnTimer = 0f;
			m_obstacleTier = Mathf.Min(m_obstacleTier + 1, obstacleTypes.Count - 1);
			m_currentObstacleCycle = m_obstacleTier;
		}
		else
		{
			m_spawnRate -= 0.25f;
			if(m_spawnRate < 0.75f) m_spawnRate = 0.75f;
		}
	}

	private void IncrementCurrentObstacleCycle()
	{
		m_currentObstacleCycle++;
		if(m_currentObstacleCycle > m_obstacleTier)
		{
			m_currentObstacleCycle = 0;
		}
	}

	public bool IsAtFinalObstacleTier()
	{
		return m_obstacleTier >= obstacleTypes.Count - 1;
	}

	private int m_obstacleTier = 0;
	private int m_currentObstacleCycle = 0;
}
