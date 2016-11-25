using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterMoveNode : MonoBehaviour {
	
	public PlayerCharacter player;

	public Image Background
	{
		get
		{
			if(m_background == null)
			{
				m_background = GetComponent<Image>();
			}

			return m_background;
		}
	}

	private Image m_background;
}
