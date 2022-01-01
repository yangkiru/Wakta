using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RIP : MonoBehaviour {
    public Transform tf;
    public Rigidbody2D rb;
    private float t;
    public RectTransform textBubble;
    public TextMeshProUGUI text;
    public TextMeshProUGUI nameText;

    private void OnEnable() {
        t = 7;
    }

    private void Update() {
        if (tf.position.y <= -20) {
            Vector2 pos = Wakta.Instance.tf.position;
            rb.velocity = Vector2.zero;
            tf.position = pos;
        }
        t -= Time.deltaTime;
        if (t < 0) {
            RIPManager.Instance.RIPList.Remove(this);
            RIPManager.Instance.RIPPool.EnqueueObjectPool(gameObject);
        }
    }
}
