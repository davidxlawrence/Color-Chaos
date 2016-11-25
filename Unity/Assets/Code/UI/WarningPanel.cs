using UnityEngine;
using System.Collections;

public class WarningPanel : MonoBehaviour 
{
	public AudioClip warningSfx;

	public void PlayWarningSound()
	{
		AudioManager.GetInstance().PlayOneShot(warningSfx);
	}
}
