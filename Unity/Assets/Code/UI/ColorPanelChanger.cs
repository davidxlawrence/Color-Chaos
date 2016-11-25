using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ColorPanelChanger : MonoBehaviour {

	public Image tweenImage;
	public GameStateManager gameState;

	void Start() 
	{
		m_currentColor = SetNewColor();
		m_targetColor = SetNewColor();
		tweenImage.color = m_currentColor;
	}
	
	void Update() 
	{
		if (!gameState.IsPlayingGame)
			return;

		ColorHSV hsv = new ColorHSV (Color.Lerp (m_currentColor, m_targetColor, t));
		hsv.a = 0.5f;
		tweenImage.color = hsv.ToColor();
		if (t < 1)
		{
			t += Time.deltaTime / 8.0f;
		}
		else if(t >= 1)
		{
			t = 0f;
			m_currentColor = m_targetColor;
			m_targetColor = SetNewColor();
		}
	}

	public Color CurrentColor
	{
		get { return tweenImage.color; }
	}

	private Color SetNewColor()
	{
		ColorHSV randomHSV = new ColorHSV(Random.Range(0.0f, 360f), 1f, 1f);
		randomHSV.a = 0.5f;
		Color color = randomHSV.ToColor();
		return color;
	}

	private float t = 0f;

	private Color m_currentColor;
	private Color m_targetColor;
}
