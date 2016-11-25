using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerCharacter : MonoBehaviour {

	public event System.Action<int> OnCharacterHealthChanged;

	public GameStateManager gameState;
	public GameObject shield;

	public AudioClip healSfx;
	public AudioClip getHitSfx;

	public float m_damageImmunityDuration;
	public float initialMovementSpeed;
	public float maxMovementSpeed;
	public float movementSpeedIncreaseAmount;

	void Start() 
	{
		m_animator = GetComponent<Animator>();

		m_damageImmunityTimer = 0f;
		m_immuneToDamage = false;

		gameState.GameStarted += OnGameStarted;
		gameState.IncreaseDifficulty += OnDifficultyIncreased;
		gameState.GamePaused += OnGamePaused;
		gameState.GameEnded += OnGameEnded;
	}
	
	void Update() 
	{
		if (!gameState.IsPlayingGame)
			return;

		if(MovementTarget != null)
		{
			transform.position = Vector3.MoveTowards(transform.position, MovementTarget.transform.position, m_movementSpeed * Time.deltaTime * 300f);
			transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
		}

		if(m_immuneToDamage)
		{
			m_damageImmunityTimer += Time.deltaTime;
			if(m_damageImmunityTimer >= m_damageImmunityDuration)
			{
				m_animator.SetInteger(m_damagedEvent, 0);
				m_immuneToDamage = false;
			}
		}
	}

	public void ChangeHealth(int delta)
	{
		if((!m_immuneToDamage && !gameState.IsGameOver) || delta > 0)
		{
			if(delta < 0)
			{
				m_immuneToDamage = true;
				m_damageImmunityTimer = 0f;
				m_animator.SetInteger(m_damagedEvent, 1);
				AudioManager.GetInstance().PlayOneShot(getHitSfx);
			}
			else
			{
				AudioManager.GetInstance().PlayOneShot(healSfx);
			}

			if(OnCharacterHealthChanged != null) OnCharacterHealthChanged(delta);
		}
	}

	public void AddShield()
	{
		shield.SetActive(true);
	}

	public void RemoveShield()
	{
		shield.SetActive(false);
	}

	void OnTriggerEnter2D(Collider2D c)
	{
		CheckCollision(c);
	}
	
	void OnTriggerStay2D(Collider2D c) 
	{
		CheckCollision(c);
	}
	
	private void CheckCollision(Collider2D c)
	{
		Spawnable s = c.GetComponent<Spawnable> ();
		if (s != null) 
		{
			s.OnCollidedWithPlayer(this);
		}
	}

	private void OnGameStarted()
	{
		m_animator.SetInteger(m_damagedEvent, 0);
		transform.position = Vector3.zero;
		m_damageImmunityTimer = 0f;
		m_immuneToDamage = false;
		MovementTarget = null;
		m_movementSpeed = initialMovementSpeed;
	}

	private void OnGameEnded()
	{
		m_animator.SetInteger(m_damagedEvent, 0);
	}

	private void OnDifficultyIncreased(bool addNewObstacle)
	{
		m_movementSpeed = Mathf.Min(m_movementSpeed + movementSpeedIncreaseAmount, maxMovementSpeed);
	}

	private void OnGamePaused(bool paused)
	{
		m_animator.speed = paused ? 0 : 1;
	}

	public CharacterMoveNode MovementTarget
	{
		get { return m_movementTarget; }
		set { m_movementTarget = value; }
	}

	private CharacterMoveNode m_movementTarget;
	private bool m_immuneToDamage;
	private float m_damageImmunityTimer;
	private float m_movementSpeed;

	private Animator m_animator;

	private const string m_damagedEvent = "Damaged";
}
