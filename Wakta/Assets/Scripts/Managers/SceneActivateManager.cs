using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneActivateManager : MonoBehaviour
{
	public float waitTime = 3;
	public bool isAutoQuit = true;
	public bool isDontDestroy = false;
	public LoadSceneMode mode;
	public string sceneName = string.Empty;
    IEnumerator Start() {
	    Wakta.Instance.Pause();
	    for (int i = 0; i < PanzeeManager.Instance.panzeeArray.Length; i++) {
		    Panzee panzee = PanzeeManager.Instance.panzeeArray[i];
		    if (panzee != null && panzee.gameObject.activeSelf) {
			    panzee.Pause();
		    }
	    }
		FadeManager.Instance.FadeIn(1);
		AsyncOperation async = SceneManager.LoadSceneAsync(sceneName.CompareTo(string.Empty) == 0 ? GameManager.LastScene : sceneName, mode);
		async.allowSceneActivation = false;
		float t = waitTime;
		do {
			if (t <= waitTime - 1 && Input.anyKeyDown)
				break;
			yield return null;
			t -= Time.deltaTime;
		} while (t >= 0 || !isAutoQuit);

		t = 1;
		do {
			yield return null;
			t -= Time.deltaTime;
		} while (t >= 0);
		async.allowSceneActivation = true;
		yield return null;
    }
}
