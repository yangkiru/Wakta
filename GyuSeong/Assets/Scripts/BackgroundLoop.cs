using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundLoop : MonoBehaviour {

    public float speed = 1;
    public Renderer renderer;
    private void Update() {
        Vector2 offset = new Vector2(Time.time * speed, 0);

        renderer.material.mainTextureOffset = offset;
    }
}