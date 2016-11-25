using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameServicesPanel : MonoBehaviour {

	public Text signedInText;
	public Text signedOutText;

	void Start()
	{
		SetAuthenticatedText();
	}

	void Update()
	{
		SetAuthenticatedText();
	}

	public void OnGameServicesButtonClicked()
	{
		GameServicesManager.Instance.ToggleSignIn();
	}

	public void OnAchievementsButtonClicked()
	{
		GameServicesManager.Instance.ShowAchievements();
	}

	public void OnLeaderboardsButtonClicked()
	{
		GameServicesManager.Instance.ShowLeaderboard();
	}

	private void SetAuthenticatedText()
	{
		if(signedInText != null) signedInText.gameObject.SetActive(GameServicesManager.Instance.IsSignedIn);
		if(signedOutText != null) signedOutText.gameObject.SetActive(!GameServicesManager.Instance.IsSignedIn);
	}
}
