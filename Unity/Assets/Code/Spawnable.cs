using UnityEngine;
using System.Collections;

public class Spawnable : MonoBehaviour {

	public event System.Action<Spawnable> OnDeactivated;

	void Awake()
	{	
		Initialize();
	}

	void Start()
	{
		gameState.GamePaused += OnGamePaused;
	}

	void Update()
	{
		DoUpdate();
	}

	virtual public void Activate()
	{
		m_activated = true;
	}
	
	virtual public void Deactivate()
	{
		m_activated = false;
		ObjectPool.instance.PoolObject(gameObject);
		if(OnDeactivated != null) OnDeactivated(this);
	}

	virtual public void OnCollidedWithPlayer(PlayerCharacter p)
	{
	}

	virtual protected void Initialize()
	{
	}

	virtual public bool CanSpawn()
	{
		return true;
	}

	virtual protected void DoUpdate()
	{
	}

	virtual protected void OnGamePaused(bool paused)
	{
	}

	protected GameStateManager gameState
	{
		get {
			if(m_gameState == null)
			{
				GameObject g = GameObject.FindGameObjectWithTag("GameController");
				if(g != null)
				{
					m_gameState = g.GetComponent<GameStateManager>();
				}
			}

			return m_gameState;
		}
	}

	protected bool m_activated = false;

	protected GameStateManager m_gameState;
}
