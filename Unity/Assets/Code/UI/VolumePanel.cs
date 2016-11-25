using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VolumePanel : MonoBehaviour 
{
	public Image sfxOn;
	public Image sfxOff;
	public Image musicOn;
	public Image musicOff;

	void Start () 
	{
		m_audioManager = AudioManager.GetInstance();
		SetVolumeImages();
	}

	public void OnSfxButtonClicked()
	{
		AudioManager.GetInstance().ToggleSounds();
		SetVolumeImages();
	}

	public void OnMusicButtonClicked()
	{
		AudioManager.GetInstance().ToggleMusic();
		SetVolumeImages();
	}

	private void SetVolumeImages()
	{
		sfxOn.gameObject.SetActive(m_audioManager.IsSfxEnabled());
		sfxOff.gameObject.SetActive(!m_audioManager.IsSfxEnabled());

		musicOn.gameObject.SetActive(m_audioManager.IsMusicEnabled());
		musicOff.gameObject.SetActive(!m_audioManager.IsMusicEnabled());
	}

	private AudioManager m_audioManager;
}
