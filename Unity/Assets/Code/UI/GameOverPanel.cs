using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GameOverPanel : MonoBehaviour {

	public GameObject retryButton;

	void OnEnable()
	{
		StartCoroutine(ShowRetryButton());
	}

	void OnDisable()
	{
		retryButton.SetActive (false);
	}

	private IEnumerator ShowRetryButton()
	{
		float time = 0.0f;
		while(time < 1.5f)
		{
			time += Time.deltaTime;
			yield return null;
		}

		retryButton.SetActive(true);
	}
}
