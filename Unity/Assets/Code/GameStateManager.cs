using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour 
{
	public event System.Action GameStarted;
	public event System.Action GameEnded;
	public event System.Action<bool> GamePaused;
	public event System.Action<bool> IncreaseDifficulty;

	public AudioClip gameMusic;
	public AudioClip dieSfx;

	public Image[] healthIcons;
	public Animator instructionText;
	
	public GameObject gameOverPanel;
	public GameObject pausePanel;
	public GameObject warningPanel;

	public Animator[] textAnimators;

	public ObstacleSpawner obstacleSpawner;
	public ScoreManager scoreManager;
	public ChartboostManager chartboostManager;
	public PlayerCharacter player;

	void Start()
	{
		m_maxHealth = healthIcons.Length;
		m_currentHealth = m_maxHealth;
		m_currentDifficulty = 0;
		m_numGamesStarted = 0;

		m_playerHasShield = false;

		gameOverPanel.SetActive(false);
		pausePanel.SetActive(false);
		m_currentGameState = GameState.PRE_GAME;

		player.OnCharacterHealthChanged += OnCharacterHealthChanged;

		AudioManager.GetInstance().PlayMusic(gameMusic, true);
	}

	void Update()
	{
		if(IsPlayingGame)
		{
			if(scoreManager.Score > m_currentDifficulty)
			{
				if(scoreManager.Score == 1)
				{
					instructionText.SetTrigger("CollectedStar");
				}

				m_currentDifficulty++;
				bool newTier = m_currentDifficulty % 10 == 0 && !obstacleSpawner.IsAtFinalObstacleTier();

				if(newTier)
				{
					GameServicesManager.Instance.ReportStarAchievementProgress(m_currentDifficulty);
					warningPanel.SetActive(true);
					warningPanel.GetComponentInChildren<Animator>().SetTrigger("ShowWarning");
				}

				if(IncreaseDifficulty != null) IncreaseDifficulty(newTier);
			}
		}
	}

	private void OnCharacterHealthChanged(int delta)
	{
		if(m_playerHasShield)
		{
			m_playerHasShield = false;
			player.RemoveShield();
		}
		else
		{
			m_currentHealth = Mathf.Clamp(m_currentHealth + delta, 0, m_maxHealth);

			for(int i = 0; i < healthIcons.Length; i++)
			{
				healthIcons[i].enabled = (m_currentHealth > i);
			}

			if (m_currentHealth <= 0)
			{
				AudioManager.GetInstance().PlayOneShot(dieSfx);
				OnGameOver();
			}
		}
	}

	public void StartGame()
	{
		gameOverPanel.SetActive(false);
		pausePanel.SetActive(false);

		m_numGamesStarted++;

		instructionText.SetTrigger("GameStarted");

		foreach(Image healthIcon in healthIcons)
		{
			healthIcon.enabled = true;
		}

		if(!AudioManager.GetInstance().IsMusicPlaying())
		{
			AudioManager.GetInstance().PauseMusic(false);
			AudioManager.GetInstance().PlayMusic(gameMusic, true);
		}

		m_currentHealth = m_maxHealth;
		m_currentGameState = GameState.PLAYING;
		m_currentDifficulty = 0;

		if(GameStarted != null) GameStarted();
	}

	public void OnGamePaused()
	{
		if (m_currentGameState == GameState.PLAYING) 
		{
			pausePanel.SetActive(true);
			AudioManager.GetInstance().PauseMusic(true);
			m_currentGameState = GameState.PAUSED;
		} 
		else if (m_currentGameState == GameState.PAUSED) 
		{
			pausePanel.SetActive(false);
			AudioManager.GetInstance().PauseMusic(false);
			m_currentGameState = GameState.PLAYING;
		}

		bool paused = m_currentGameState == GameState.PAUSED;
		float speed = paused ? 0 : 1;

		foreach(Animator a in textAnimators)
		{
			a.speed = speed;
		}

		if (GamePaused != null) GamePaused(paused);
	}

	public void OnGameOver()
	{
		m_screenTexture = new Texture2D(Screen.width, Screen.height,TextureFormat.RGB24,false);
		m_screenTexture.ReadPixels(new Rect(0f, 0f, Screen.width, Screen.height),0,0);
		m_screenTexture.Apply();

		m_currentGameState = GameState.GAME_OVER;
		m_playerHasShield = false;
		gameOverPanel.SetActive(true);
		warningPanel.SetActive(false);
		if(GameEnded != null) GameEnded();

		AudioManager.GetInstance().PauseMusic(true);

		if(m_numGamesStarted > 1)
		{
			chartboostManager.ShowGameOverRewardedVideo();
		}
	}

	public void ShieldRewarded(int amount)
	{
		player.AddShield();
		m_playerHasShield = true;
	}

	public int CurrentDifficulty
	{
		get
		{
			return m_currentDifficulty;
		}
	}

	public Texture2D Screenshot
	{
		get
		{
			return m_screenTexture;
		}
	}

	public Transform NextHealthIcon
	{
		get
		{
			return healthIcons[m_currentHealth].transform;
		}
	}

	public bool IsGameOver
	{
		get 
		{
			return m_currentGameState == GameState.GAME_OVER;
		}
	}

	public bool IsPlayingGame
	{
		get 
		{
			return m_currentGameState == GameState.PLAYING;
		}
	}

	public bool IsPreGame
	{
		get 
		{
			return m_currentGameState == GameState.PRE_GAME;
		}
	}

	public bool IsPlayerDamaged
	{
		get 
		{
			return m_currentHealth != m_maxHealth;
		}
	}

	private enum GameState
	{
		PRE_GAME, PLAYING, GAME_OVER, PAUSED
	};
	
	private int m_maxHealth;
	private int m_currentHealth;
	private int m_currentDifficulty;

	private GameState m_currentGameState;

	private int m_numGamesStarted;

	private bool m_playerHasShield;

	private Texture2D m_screenTexture;
}
