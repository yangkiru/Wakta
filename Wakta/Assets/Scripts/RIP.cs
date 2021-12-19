using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RIP : MonoBehaviour {
    private float t;
    public TextMeshPro text;

    private void OnEnable() {
        t = 7;
    }

    private void Update() {
        t -= Time.deltaTime;
        if (t < 0) RIPManager.Instance.RIPPool.EnqueueObjectPool(gameObject);
    }
}
