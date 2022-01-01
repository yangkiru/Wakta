using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RIPManager : MonoSingleton<RIPManager> {
    public ObjectPool RIPPool;
    public GameObject RIPObj;
    public List<RIP> RIPList = new List<RIP>();
    
    private void Awake() {
        for (int i = 0; i < 10; i++) {
            RIPPool.EnqueueObjectPool(Instantiate(RIPObj));
        }
    }

    public void SpawnRIP(Panzee target, string lastWord) {
        GameObject obj = RIPPool.DequeueObjectPool();
        Vector2 pos = target.transform.position;
        obj.transform.position = pos;
        RIP rip = obj.GetComponent<RIP>();
        if (!lastWord.Equals(String.Empty)) {
            rip.text.text = lastWord;
        }
        else {
            rip.text.text = target.text.text;
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(rip.textBubble);
        rip.textBubble.gameObject.SetActive(!rip.text.text.Equals(String.Empty));
        rip.nameText.text = target.name;
        RIPList.Add(rip);
        
        obj.SetActive(true);
    } 
}
