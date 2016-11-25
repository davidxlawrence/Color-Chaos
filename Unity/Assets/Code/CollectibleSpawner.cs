using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectibleSpawner : Spawner {

	public RectTransform collectibleSpawnParent;
	public List<CollectibleData> collectibleTypes;

	override protected void Initialize()
	{
		base.Initialize ();

		m_spawnBounds = new Bounds(Vector3.zero, collectibleSpawnParent.rect.size);
	}

	override protected void OnGameEnded()
	{
		base.OnGameEnded();
		
		Collectible[] collectibles = GameObject.FindObjectsOfType<Collectible>();
		foreach(Collectible c in collectibles)
		{
			c.Deactivate();
		}
	}

	override protected void DoUpdate()
	{
		if (!gameState.IsPlayingGame)
			return;

		m_spawnTimer += Time.deltaTime;
		if(m_spawnTimer > m_spawnRate && m_currentActiveCollectible == null) 
		{
			m_spawnTimer = 0.0f;

			float rand = Random.Range(0f, 1f);
			CollectibleData chosenData = null;

			List<CollectibleData> spawnableCollectibles = new List<CollectibleData>();

			foreach(CollectibleData data in collectibleTypes)
			{
				if(data.collectible.CanSpawn())
				{
					spawnableCollectibles.Add(data);
				}
			}

			foreach(CollectibleData cd in spawnableCollectibles)
			{
				if(rand <= cd.spawnChanceWeight || spawnableCollectibles.Count == 1)
				{
					chosenData = cd;
					break;
				}

				rand -= cd.spawnChanceWeight;
			}

			if(chosenData != null)
			{
				Collectible c = chosenData.collectible;
				GameObject collectible = ObjectPool.instance.GetObjectForType (c.name, false);

				m_currentActiveCollectible = collectible.GetComponent<Collectible>();

				Vector2 randomPosition = new Vector2(Random.Range(m_spawnBounds.min.x, m_spawnBounds.max.x), 
				                                     Random.Range(m_spawnBounds.min.y, m_spawnBounds.max.y));

				int safetyCheck = 500;
				do
				{
					randomPosition = new Vector2(Random.Range(m_spawnBounds.min.x, m_spawnBounds.max.x), 
					                             Random.Range(m_spawnBounds.min.y, m_spawnBounds.max.y));
					safetyCheck--;
				}
				while(((Vector2)gameState.player.transform.position - randomPosition).magnitude < 25f && safetyCheck > 0);

				m_currentActiveCollectible.transform.position = randomPosition;
				m_currentActiveCollectible.transform.SetParent(collectibleSpawnParent.transform);
				m_currentActiveCollectible.OnDeactivated += OnCurrentActiveCollectibleCollected;
				m_currentActiveCollectible.Activate();
			}
			else 
			{
				Debug.LogWarning("Unable to choose a collectible to spawn!");
			}
		}
	}

	private void OnCurrentActiveCollectibleCollected(Spawnable s)
	{
		s.OnDeactivated -= OnCurrentActiveCollectibleCollected;
		m_currentActiveCollectible = null;
		m_spawnTimer = 0.0f;
		m_spawnRate = 0.5f;
	}

	private Bounds m_spawnBounds;
	private Collectible m_currentActiveCollectible;
}
