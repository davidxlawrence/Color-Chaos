using UnityEngine;
using System.Collections;
using Prime31;

public class ChartboostManager : MonoBehaviour 
{
	public GameStateManager gameState;

	void Start()
	{
		Chartboost.init("55bb8a96c909a66315763faa", 
		                "5ee308bc8a482c70f6408d8a75a5789cad00d580", 
		                "55bb8a96c909a66315763fa9", 
		                "c70bb9fe554de9639e875dc6ccf2da3618a2517b");

		ChartboostAndroid.setImpressionsUseActivities(true);

		Chartboost.cacheInterstitial();
		Chartboost.cacheRewardedVideo();
	}

	void OnEnable()
	{
		Chartboost.didCompleteRewardedVideoEvent += DidCompleteRewardedVideo;
	}

	void OnDisable()
	{
		Chartboost.didCompleteRewardedVideoEvent -= DidCompleteRewardedVideo;
	}

	public void ShowGameOverInterstitial()
	{
		if (Chartboost.hasCachedInterstitial()) 
		{
			Chartboost.showInterstitial();
		}
		else 
		{
			Chartboost.cacheInterstitial();
		}
	}

	public void ShowGameOverRewardedVideo()
	{
		if (Chartboost.hasCachedRewardedVideo()) 
		{
			Chartboost.showRewardedVideo();
		}
		else 
		{
			Chartboost.cacheRewardedVideo();
		}
	}

	private void DidCompleteRewardedVideo(int reward)
	{
		gameState.ShieldRewarded(reward);
	}
}
