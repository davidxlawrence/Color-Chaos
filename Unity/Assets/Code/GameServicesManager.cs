using UnityEngine;
using System.Collections;
using Prime31;

public class GameServicesManager : MonoBehaviour 
{
	void Awake() 
	{
		if (instance != null && instance != this) 
		{
			Destroy(this.gameObject);
			return;
		} 
		else 
		{
			instance = this;
		}
	}

	public static GameServicesManager Instance 
	{
		get
		{
			return instance;
		}
	}

	public bool IsSignedIn
	{
		get
		{
			return PlayGameServices.isSignedIn();
		}
	}

	void Start()
	{
		PlayGameServices.authenticate();
	}

	public void ToggleSignIn()
	{
		if(IsSignedIn)
		{
			PlayGameServices.signOut();
		}
		else
		{
			PlayGameServices.authenticate();
		}
	}
	
	public void ShowAchievements()
	{
		if(IsSignedIn)
		{
			PlayGameServices.showAchievements();
		}
		else
		{
			PlayGameServices.authenticate();
		}
	}

	public void ShowLeaderboard()
	{
		if(IsSignedIn)
		{
			PlayGameServices.showLeaderboards();
		}
		else
		{
			PlayGameServices.authenticate();
		}
	}

	public void SubmitLeaderboardScore(int score)
	{
		if(score > 0)
		{
			PlayGameServices.submitScore(m_leaderboardId, (long)score);
		}
	}

	public void ReportStarAchievementProgress(int numStars)
	{
		if(numStars > 0 && numStars % 10 == 0)
		{
			int index = (numStars / 10) - 1;
			if(index >= 0 && index <= m_starAchievementIds.Length - 1)
			{
				PlayGameServices.unlockAchievement(m_starAchievementIds[index]);
			}
		}
	}

	public void ReportHeartAchievement()
	{
		PlayGameServices.unlockAchievement(m_heartAchievementId);
	}

	private static GameServicesManager instance;

	private string[] m_starAchievementIds = new string[] {"CgkI6KaJuYkUEAIQAg", 
														  "CgkI6KaJuYkUEAIQAw", 
														  "CgkI6KaJuYkUEAIQBA", 
														  "CgkI6KaJuYkUEAIQBQ", 
														  "CgkI6KaJuYkUEAIQBg", 
														  "CgkI6KaJuYkUEAIQBw",
														  "CgkI6KaJuYkUEAIQCA",
														  "CgkI6KaJuYkUEAIQCQ",
														  "CgkI6KaJuYkUEAIQCg",
														  "CgkI6KaJuYkUEAIQCw"};

	private string m_heartAchievementId = "CgkI6KaJuYkUEAIQDA";
	private string m_leaderboardId = "CgkI6KaJuYkUEAIQAQ";
}
