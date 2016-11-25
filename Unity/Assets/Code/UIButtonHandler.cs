using UnityEngine;
using System.Collections;

public class UIButtonHandler : MonoBehaviour {

	public event System.Action OnButtonPressed;

	void OnPress(bool isDown)
	{
		if(isDown)
		{
			if(OnButtonPressed != null) OnButtonPressed();
		}
	}
}
