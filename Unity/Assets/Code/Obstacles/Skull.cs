using UnityEngine;
using System.Collections;

public class Skull : Obstacle 
{
	public float lifetime;

	public override void Activate()
	{
		base.Activate();
		m_activated = false;
		m_currentTime = 0;
		animator.SetTrigger(m_deactivateKey);
		animator.SetTrigger(m_activateKey);
	}

	public void OnActivateAnimationFinished()
	{
		m_activated = true;
	}

	protected override void DoUpdate()
	{
		if(gameState.IsPlayingGame && m_activated)
		{
			m_currentTime += Time.deltaTime;

			if(ShouldDeactivate())
			{
				Deactivate();
			}
		}
	}

	public override void Deactivate()
	{
		animator.SetTrigger(m_deactivateKey);
		base.Deactivate();
	}

	protected override bool ShouldDeactivate()
	{
		return m_currentTime >= lifetime;
	}

	private float m_currentTime = 0;
	private const string m_activateKey = "Activate";
	private const string m_deactivateKey = "Deactivate";
}
