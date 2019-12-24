using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonScript : MonoBehaviour
{
    public bool IsPush { get { return IsPush; } }
    [SerializeField]
    private bool isPush;
    private Animator anim;

    public LayerMask pushableLayer;

    public UnityEvent onPushDown;
    public UnityEvent onPushUp;

    private void Awake() {
        anim = GetComponent<Animator>();
    }
    public void PushDown() {
        Debug.Log("PushDown");
        anim.SetTrigger("pushDown");
        isPush = true;
        onPushDown.Invoke();
    }

    public void PushUp() {
        Debug.Log("PushUp");
        anim.SetTrigger("pushUp");
        isPush = false;
        onPushUp.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log("Enter");
        if ((pushableLayer.value & (1 << collision.gameObject.layer)) == (1 << collision.gameObject.layer))
            PushDown();
    }

    private void OnTriggerExit2D(Collider2D collision) {
        Debug.Log("Exit");
        if ((pushableLayer.value & (1 << collision.gameObject.layer)) == (1 << collision.gameObject.layer))
            PushUp();
    }
}
