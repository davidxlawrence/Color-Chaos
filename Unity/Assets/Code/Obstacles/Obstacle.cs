using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Obstacle : Spawnable {

	public Image[] images;
	public Animator animator;
	
	public override void Activate()
	{
		base.Activate();
		
		Color c = GameObject.FindObjectOfType<ColorPanelChanger>().CurrentColor;
		c.a = 1.0f;

		foreach(Image i in images)
		{
			i.color = c;
		}
	}
	
	protected override void OnGamePaused(bool paused)
	{
		animator.speed = paused ? 0 : 1;
	}

	override public void OnCollidedWithPlayer(PlayerCharacter p)
	{
		if(p != null)
		{
			p.ChangeHealth(-1);
		}
	}

	virtual protected bool ShouldDeactivate()
	{
		return false;
	}
}
