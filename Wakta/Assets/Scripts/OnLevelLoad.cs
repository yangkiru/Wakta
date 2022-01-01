using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class OnLevelLoad : MonoBehaviour
{
	// Use this for initialization
	void Start()
	{
		Wakta.Instance.Respawn();
		Wakta.Instance.UnPause();
		for (int i = 0; i < PanzeeManager.Instance.panzeeArray.Length; i++) {
			Panzee panzee = PanzeeManager.Instance.panzeeArray[i];
			if (panzee != null && panzee.gameObject.activeSelf) {
				panzee.Respawn();
				panzee.UnPause();
			}
		}
		if (FadeManager.Instance != null)
			FadeManager.Instance.FadeIn(1);
	}
}
