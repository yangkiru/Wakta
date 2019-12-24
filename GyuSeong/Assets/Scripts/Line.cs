using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    private LineRenderer render;
    private Transform tf;
    void Awake()
    {
        tf = GetComponent<Transform>();
        render = GetComponent<LineRenderer>();
    }

    void Update() {
        render.SetPosition(0, tf.position);
        render.SetPosition(1, Wakta.Instance.leftHand.position);
    }
}
