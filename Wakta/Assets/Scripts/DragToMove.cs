using UnityEngine;
using System.Collections;

public class DragToMove : MonoBehaviour {

    public bool isDragToMove = false;
    public LayerMask dragLayer;

    private Transform tf;
    private Vector2 mOffset;

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mp, Vector2.zero, 0, dragLayer);
            if (hit) {
                tf = hit.transform;
                mOffset = tf.position - mp;
            }
        }
        else if (Input.GetMouseButton(0)) {
            if (tf) {
                MouseDrag();
            }
        }
        else if(Input.GetMouseButtonUp(0)) {
            //tf = null;
            if (tf)
                MouseUp();
        }
    }


    void MouseDrag() {
        Vector3 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mp += (Vector3)mOffset;
        mp.z = tf.position.z;
        tf.position = mp;
    }

    public void MouseUp() {
        tf = null;
    }
}
