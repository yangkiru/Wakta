using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PanzeeBlocker : MonoBehaviour {
    public bool isBlockOnStart;
    private void Start() {
        PanzeeManager.Instance.SetSpawnable(!isBlockOnStart);
    }
}
