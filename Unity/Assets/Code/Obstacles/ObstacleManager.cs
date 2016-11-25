using UnityEngine;
using System.Collections.Generic;

public class ObstacleManager : MonoBehaviour {

	public string spawnPointTagName;
	public Obstacle obstacleType;

	void Start()
	{
		GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag(spawnPointTagName);
		m_spawnPoints = new List<Transform>();
		
		foreach (GameObject g in spawnPoints)
		{
			m_spawnPoints.Add(g.transform);
		}
	}

	protected void RandomizeSpawnPoints()
	{
		for (int i = m_spawnPoints.Count - 1; i > 0; i--)
		{
			int j = Random.Range(0, i);
			Transform temp = m_spawnPoints[i];
			m_spawnPoints[i] = m_spawnPoints[j];
			m_spawnPoints[j] = temp;
		}
	}

	virtual public void SpawnObstacle()
	{
		GameObject obstacle = ObjectPool.instance.GetObjectForType (obstacleType.name, false);
		
		if(obstacle != null)
		{
			Obstacle o = obstacle.GetComponent<Obstacle>();
			
			RandomizeSpawnPoints();
			
			Transform s = null;
			
			foreach(Transform spawnPoint in m_spawnPoints)
			{
				if(spawnPoint.childCount == 0)
				{
					s = spawnPoint;
				}
			}
			
			if(s != null)
			{
				o.transform.SetParent(s);
				o.transform.localPosition = Vector3.zero;
				o.transform.localRotation = Quaternion.identity;
				o.transform.localScale = Vector3.one;
				
				o.Activate();
				o.OnDeactivated += OnObstacleDeactivated;
			}
		}
	}

	virtual public bool CanSpawnObstacle()
	{
		int freeSpaceCount = 0;
		
		foreach(Transform spawnPoint in m_spawnPoints)
		{
			if(spawnPoint.childCount == 0)
			{
				freeSpaceCount++;
			}
		}
		
		return freeSpaceCount > 0;
	}

	private void OnObstacleDeactivated(Spawnable s)
	{
		s.OnDeactivated -= OnObstacleDeactivated;
	}

	protected List<Transform> m_spawnPoints;
}
