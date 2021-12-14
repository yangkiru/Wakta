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
		Instance = this;
		if (!DebugManager.Instance.isDebug || !String.IsNullOrEmpty(DebugManager.Instance.sceneName))
			SceneManager.LoadScene(LastScene, LoadSceneMode.Additive);
	}

	public static string LastScene {
		get {
			string value = PlayerPrefs.GetString("lastScene");
			if (value.CompareTo(string.Empty) == 0) return "tutorial";
			else return value;
		}
		set {
			PlayerPrefs.SetString("lastScene", value);
		}
	}

	public static bool IsInLayerMask(int layer, LayerMask layermask)
	{
		return layermask == (layermask | (1 << layer));
	}
}
