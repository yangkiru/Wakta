using UnityEngine;
using System.Collections;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour {
    private static T instance = null;
	public static bool IsInstance { get { return instance != null; } }
	public static T Instance {
        get {
			T[] temp = FindObjectsOfType(typeof(T)) as T[];
			if (temp.Length == 0) {
				instance = new GameObject("@" + typeof(T).ToString(),
										   typeof(T)).AddComponent<T>();
			} else if (instance == null) {
				instance = temp[0];
			} else {
				for (int i = 0; i < temp.Length; i++) {
					if (instance != temp[i])
						Destroy(temp[i].gameObject);
				}
			}
			return instance;
        }
		set {
			if (instance == null) instance = value;
			value.transform.SetParent(null);
			DontDestroyOnLoad(instance.gameObject);
		}
	}
}
