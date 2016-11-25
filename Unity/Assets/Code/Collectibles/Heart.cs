using UnityEngine;
using System.Collections;

public class Heart : Collectible {
	
	protected override void OnCollectionFinished()
	{
		if(gameState.player != null && gameState.IsPlayingGame)
		{
			gameState.NextHealthIcon.GetComponent<Animator>().SetTrigger("HeartFilled");
			gameState.player.ChangeHealth(1);
			GameServicesManager.Instance.ReportHeartAchievement();
		}
		
		Deactivate();
	}

	protected override Vector3 TargetPosition 
	{
		get
		{
			Transform t = gameState.NextHealthIcon;
			return t.position;
		}
	}

	public override bool CanSpawn()
	{
		return gameState.IsPlayerDamaged;
	}

	private PlayerCharacter m_player;
}
