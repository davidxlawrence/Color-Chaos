using UnityEngine;
using System.Collections;

public class Star : Collectible {

	public override void Activate()
	{
		base.Activate ();

		m_score = GameObject.FindGameObjectWithTag("Score");
	}

	protected override void OnCollectionFinished()
	{
		GameObject g = GameObject.FindGameObjectWithTag("ScoreManager");
		if(g != null && gameState.IsPlayingGame)
		{
			g.GetComponent<ScoreManager>().IncreaseScore(1);
		}
		
		Deactivate();
	}

	protected override Vector3 TargetPosition 
	{
		get
		{
			return m_score.transform.position;
		}
	}

	private GameObject m_score;
}