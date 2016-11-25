using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
	public AudioSource sfxSource;
	public AudioSource musicSource;

	void Awake() 
	{
		if (instance != null && instance != this) 
		{
			Destroy(this.gameObject);
			return;
		} 
		else 
		{
			instance = this;
		}

		sfxSource.mute = PlayerPrefs.GetInt(m_sfxEnabledKey, 1) == 0;
		musicSource.mute = PlayerPrefs.GetInt(m_musicEnabledKey, 1) == 0;
	}
	
	public static AudioManager GetInstance() 
	{
		return instance;
	}

	public void PlayOneShot(AudioClip clip, float volume)
	{
		sfxSource.PlayOneShot(clip, volume);
	}

	public void PlayOneShot(AudioClip clip)
	{
		sfxSource.PlayOneShot(clip, 1.0f);
	}

	public void PlaySoundEffect(AudioClip clip)
	{
		sfxSource.clip = clip;
		sfxSource.Play();
	}

	public void PlayMusic(AudioClip clip, bool loop)
	{
		musicSource.clip = clip;
		musicSource.loop = loop;
		musicSource.Play();
	}

	public void PauseMusic(bool pause)
	{
		if(IsMusicEnabled())
		{
			if (pause) 
			{
				musicSource.Pause();
			}
			else
			{
				musicSource.UnPause();
			}
		}
	}

	public bool IsMusicPlaying()
	{
		return musicSource.isPlaying;
	}

	public void ToggleSounds()
	{
		sfxSource.mute = !sfxSource.mute;
		PlayerPrefs.SetInt(m_sfxEnabledKey, (!sfxSource.mute) ? 1 : 0);
		PlayerPrefs.Save();
	}

	public void ToggleMusic()
	{
		musicSource.mute = !musicSource.mute;
		PlayerPrefs.SetInt(m_musicEnabledKey, (!musicSource.mute) ? 1 : 0);
		PlayerPrefs.Save();
	}

	public bool IsSfxEnabled()
	{
		return !sfxSource.mute;
	}

	public bool IsMusicEnabled()
	{
		return !musicSource.mute;
	}

	private static AudioManager instance;

	private const string m_sfxEnabledKey = "SfxEnabled";
	private const string m_musicEnabledKey = "MusicEnabled";
}