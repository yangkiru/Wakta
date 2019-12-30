using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
	private const int volumeMax = 3;
	private const float volumePerLevel = 1f / volumeMax;
	public float min;
	public float max;
	public int VolumeLevel { get { return volumeLevel; } set { volumeLevel = value % (volumeMax + 1); } }
	private int volumeLevel = 0;
	public float Volume {
		get {
			return Mathf.Lerp(min, max, (volumeMax - volumeLevel) * volumePerLevel);
		}
	}
	public AudioMixer mixer;
	public Image image;
	public Sprite[] sprites;

	public void VolumeDown()
	{
		VolumeLevel += 1;
		VolumeUpdate();
	}

	private void Start()
	{
		volumeLevel = PlayerPrefs.GetInt("Volume");
		VolumeUpdate();
	}

	private void VolumeUpdate()
	{
		if (volumeLevel == volumeMax)
			mixer.SetFloat("Volume", -80f);//MUTE
		else
			mixer.SetFloat("Volume", Volume);
		PlayerPrefs.SetInt("Volume", volumeLevel);
		image.sprite = sprites[volumeLevel];
	}

}
