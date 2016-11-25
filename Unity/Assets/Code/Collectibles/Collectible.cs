using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Collectible : Spawnable {

	public float m_maximumDuration;
	public Animator animator;
	public AudioClip collectingSfx;

	override protected void DoUpdate()
	{
		if(gameState.IsPlayingGame && m_activated)
		{
			if(m_collecting)
			{
				transform.position = Vector3.MoveTowards(transform.position, TargetPosition, Time.deltaTime * m_collectingSpeed);
				if((transform.position - TargetPosition).magnitude <= 0.1f)
				{
					OnCollectionFinished();
				}
			}
			else
			{
				m_currentDuration += Time.deltaTime;
				if(m_currentDuration / m_maximumDuration >= m_expiringSoonPercent && !m_expiringSoon)
				{
					m_expiringSoon = true;
					StartCoroutine("ExpiringSoonAnimation");
				}

				if(m_currentDuration >= m_maximumDuration && m_maximumDuration > 0)
				{
					OnExpired();
				}
			}
		}
	}

	public override void Activate()
	{
		base.Activate ();

		m_currentDuration = 0f;
		m_expiringSoon = false;
		m_collecting = false;

		transform.localScale = Vector3.one;
	}

	public override void Deactivate()
	{
		m_expiringSoon = false;
		m_collecting = false;
		CancelExpiringSoon();
		base.Deactivate();
	}

	override public void OnCollidedWithPlayer(PlayerCharacter p)
	{	
		if(!m_collecting)
		{
			m_collecting = true;
			if(collectingSfx != null)
			{
				AudioManager.GetInstance().PlayOneShot(collectingSfx);
			}
		}
	}

	private IEnumerator ExpiringSoonAnimation()
	{
		Image i = GetComponent<Image>();

		while(m_currentDuration < m_maximumDuration)
		{
			while(!gameState.IsPlayingGame) yield return null;

			Color c = i.color;
			c.a = 0.5f;
			i.color = c;
			yield return new WaitForSeconds(0.05f);
			c.a = 0f;
			i.color = c;
			yield return new WaitForSeconds(0.05f);
			c.a = 0.5f;
			i.color = c;
			yield return new WaitForSeconds(0.05f);
			c.a = 1.0f;
			i.color = c;
			yield return new WaitForSeconds(0.05f / (m_currentDuration / m_maximumDuration));
		}
	}

	protected void CancelExpiringSoon()
	{
		StopCoroutine("ExpiringSoonAnimation");

		Image i = GetComponent<Image>();
		Color c = i.color;
		c.a = 1.0f;
		i.color = c;
		
		transform.localScale = Vector3.one;
	}

	virtual protected void OnExpired()
	{
		Deactivate();
	}

	virtual protected void OnCollectionFinished()
	{
	}

	virtual protected Vector3 TargetPosition
	{
		get
		{
			return Vector3.zero;
		}
	}

	protected override void OnGamePaused (bool paused)
	{
		if(animator != null)
		{
			animator.speed = paused ? 0 : 1;
		}
	}

	protected float m_currentDuration;

	private float m_expiringSoonPercent = 0.5f;
	private bool m_expiringSoon = false;

	private const float m_collectingSpeed = 500f;

	private bool m_collecting;
}
