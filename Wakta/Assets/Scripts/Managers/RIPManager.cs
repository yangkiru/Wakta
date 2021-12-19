using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RIPManager : MonoSingleton<RIPManager> {
    public ObjectPool RIPPool;
    public GameObject RIPObj;
    
    private void Awake() {
        for (int i = 0; i < 10; i++) {
            RIPPool.EnqueueObjectPool(Instantiate(RIPObj));
        }
    }

    public void SpawnRIP(Panzee target, string lastWord) {
        GameObject obj = RIPPool.DequeueObjectPool();
        Debug.Log("SpawnRIP");
        Vector2 pos = target.transform.position;
        pos.y += 2;
        obj.transform.position = pos;
        if (!lastWord.Equals(String.Empty)) {
            obj.GetComponent<RIP>().text.text = lastWord;
        }
        else {
            obj.GetComponent<RIP>().text.text = target.text.text;
        }

        obj.SetActive(true);
    } 
}
