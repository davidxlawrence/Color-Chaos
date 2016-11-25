using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreView : MonoBehaviour {

	public Text scoreText;
	public Text ScoreText
	{
		get { return scoreText; }
	}

	public Text gameOverScoreText;
	public Text GameOverScoreText
	{
		get { return gameOverScoreText; }
	}

	public Text gameOverHighScoreText;
	public Text GameOverHighScoreText
	{
		get { return gameOverHighScoreText; }
	}
}
