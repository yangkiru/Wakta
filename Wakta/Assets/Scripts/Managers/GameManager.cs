using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    public LayerMask blockLayer;
	public LayerMask panzeeLayer;
	public LayerMask waktaLayer;
	public LayerMask jumpableLayer;

	private void Awake()
	{
		if (!PlayerPrefs.HasKey("2022Reset")) {
			PlayerPrefs.SetInt("2022Reset", 1);
			PlayerPrefs.SetString("lastScene", "Tutorial1");
		}

		if (!DebugManager.Instance.isDebug || !String.IsNullOrEmpty(DebugManager.Instance.sceneName)) {
			if (PlayerPrefs.GetString("lastScene").Equals("Game")) {
				LastScene = "Tutorial1";
			}
			SceneManager.LoadScene(LastScene, LoadSceneMode.Additive);
		}
	}

	public static string LastScene {
		get {
			string value = PlayerPrefs.GetString("lastScene");
			Debug.Log("get:"+value);
			if (value.CompareTo(string.Empty) == 0) return "Tutorial1";
			else return value;
		}
		set {
			Debug.Log("set:"+value);
			PlayerPrefs.SetString("lastScene", value);
		}
	}

	public static bool IsInLayerMask(int layer, LayerMask layermask)
	{
		return layermask == (layermask | (1 << layer));
	}
}
