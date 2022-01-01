using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Splash : MonoBehaviour
{
    public float waitTime = 3;
    public bool isAutoQuit = true;
    public bool isDontDestroy = false;
    public LoadSceneMode mode;
    IEnumerator Start() {
        FadeManager.Instance.FadeIn(1);
        AsyncOperation async = SceneManager.LoadSceneAsync("Game", mode);
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