using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
	private const int volumeMax = 4;
	public float min;
	public float max;
	private int musicLevel = 0;
	private int SFXLevel = 0;
	public AudioMixer musicMixer;
	public Image musicImage;
	public Image SFXImage;
	public Sprite[] sprites;
	public AudioSource btnSFX;

	public void MusicVolumeUp()
	{
		musicLevel = (musicLevel + 1) % volumeMax;
		MusicVolumeUpdate();
		btnSFX.Play();
	}

	public void SFXVolumeUp() {
		SFXLevel = (SFXLevel + 1) % volumeMax;
		SFXVolumeUpdate();
		btnSFX.Play();
	}

	private void Start()
	{
		musicLevel = PlayerPrefs.GetInt("musicVolume");
		SFXLevel = PlayerPrefs.GetInt("SFXVolume");
		MusicVolumeUpdate();
		SFXVolumeUpdate();
	}

	private void MusicVolumeUpdate()
	{
		if (musicLevel == volumeMax-1)
			musicMixer.SetFloat("musicVolume", -80f);//MUTE
		else
			musicMixer.SetFloat("musicVolume", Mathf.Lerp(min, max, musicLevel * 0.33f));
		PlayerPrefs.SetInt("Volume", musicLevel);
		musicImage.sprite = sprites[musicLevel];
	}
	
	private void SFXVolumeUpdate()
	{
		if (SFXLevel == volumeMax-1)
			musicMixer.SetFloat("SFXVolume", -80f);//MUTE
		else
			musicMixer.SetFloat("SFXVolume", Mathf.Lerp(min, max, SFXLevel * 0.33f));
		PlayerPrefs.SetInt("Volume", SFXLevel);
		SFXImage.sprite = sprites[SFXLevel];
	}

}
