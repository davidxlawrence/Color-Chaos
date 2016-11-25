using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class StraightPathObstacle : Obstacle {

	public string m_spawnPointTagName;

	public float minSpeed;
	public float maxSpeed;

	override protected void DoUpdate() {
		if(m_activated && m_gameState.IsPlayingGame)
		{
			transform.Translate(transform.right * m_currentSpeed * Time.deltaTime, Space.World);
			
			if(ShouldDeactivate())
			{
				Deactivate();
			}
		}
	}
	
	override public void Activate()
	{
		base.Activate();

		m_currentSpeed = Random.Range(Mathf.Min(minSpeed + gameState.CurrentDifficulty, maxSpeed), maxSpeed);
	}
	
	override public void Deactivate()
	{
		base.Deactivate();
	}

	protected override bool ShouldDeactivate()
	{
		return Mathf.Abs (transform.position.x) >= Screen.width * 1.5f ||
			Mathf.Abs (transform.position.y) >= Screen.height * 1.5f;
	}
	
	private float m_currentSpeed;
}
