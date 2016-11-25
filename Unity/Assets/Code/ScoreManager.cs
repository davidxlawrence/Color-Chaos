using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {
	public GameStateManager gameState;
	public ShareScoreManager shareManager;

	public AudioClip collectCoinSfx;
	
	public float scoreIncreaseIntervalTime;
	public int scoreIncreasePerInterval;

	void Start () {
		Score = 0;
		BestScore = PlayerPrefs.GetInt(m_highScoreKey, 0);

		gameState.GameStarted += OnGameStarted;
		gameState.GameEnded += OnGameEnded;
	}

	private void OnGameStarted()
	{
		Score = 0;
		BestScore = PlayerPrefs.GetInt(m_highScoreKey, 0);
	}

	private void OnGameEnded()
	{
		FinalScoreText.text = m_score.ToString();
		if(Score > PlayerPrefs.GetInt(m_highScoreKey, 0))
		{
			PlayerPrefs.SetInt(m_highScoreKey, Score);
			PlayerPrefs.Save();
			BestScore = Score;
		}

		GameServicesManager.Instance.SubmitLeaderboardScore(Score);
	}

	public void IncreaseScore(int amount)
	{
		Score += amount;
		ScoreText.GetComponent<Animator> ().SetTrigger("ScoreIncreased");
		AudioManager.GetInstance().PlayOneShot(collectCoinSfx);
	}

	public void OnShareButtonClicked()
	{
		string text = "Just got " + Score + " on #colorchaos. Beat my best of " + BestScore + "!";
		shareManager.ShareScore(text, "My Color Chaos score", text, gameState.Screenshot);
	}

	public int Score
	{
		get { return m_score; }
		private set {
			m_score = value;
			ScoreText.text = m_score.ToString();
		}
	}

	public int BestScore
	{
		get { return m_bestScore; }
		private set {
			m_bestScore = value;
			BestScoreText.text = m_bestScore.ToString();
			BestScoreGameOverText.text = m_bestScore.ToString();
		}
	}

	public Text scoreText;
	public Text ScoreText
	{
		get { return scoreText; }
	}

	public Text finalScoreText;
	public Text FinalScoreText
	{
		get { return finalScoreText; }
	}

	public Text bestScoreText;
	public Text BestScoreText
	{
		get { return bestScoreText; }
	}

	public Text bestScoreGameOverText;
	public Text BestScoreGameOverText
	{
		get { return bestScoreGameOverText; }
	}

	private int m_score;
	private int m_bestScore;

	private const string m_highScoreKey = "HighScore";
}
