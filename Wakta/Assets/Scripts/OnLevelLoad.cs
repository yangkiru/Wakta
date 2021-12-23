using UnityEngine;
using System.Collections;

public class OnLevelLoad : MonoBehaviour
{
	// Use this for initialization
	void Start()
	{
		Wakta.Instance.Respawn();
		Wakta.Instance.UnPause();
		for (int i = 0; i < PanzeeManager.Instance.panzeeList.Count; i++) {
			if (PanzeeManager.Instance.panzeeList[i].gameObject.activeSelf) {
				PanzeeManager.Instance.panzeeList[i].Respawn();
				PanzeeManager.Instance.panzeeList[i].UnPause();
			}
		}
		if (FadeManager.Instance != null)
			FadeManager.Instance.FadeIn(1);
	}
}
