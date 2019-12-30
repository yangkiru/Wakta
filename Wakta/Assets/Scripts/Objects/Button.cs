using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    public bool IsPush { get { return IsPush; } }
    [SerializeField]
    private bool isPush;
	[Tooltip("isPassEnter 또는 isPassExit를 활성화 해야함")]
	public bool isToggle;
	public bool IsPassEnter { set { isPassEnter = value; } }
	public bool IsPassExit { set { isPassExit = value; } }
	[Tooltip("OnTriggerEnter2D를 무시")]
	public bool isPassEnter;
	[Tooltip("OnTriggerExit2D를 무시")]
	public bool isPassExit;
	private Animator anim;

    public LayerMask pushableLayer;

    public UnityEvent onPushDown;
    public UnityEvent onPushUp;

    private void Awake() {
        anim = GetComponent<Animator>();
    }
    public void PushDown() {
        //Debug.Log("PushDown");
        anim.SetTrigger("pushDown");
        isPush = true;
        onPushDown.Invoke();
    }

    public void PushUp() {
        //Debug.Log("PushUp");
        anim.SetTrigger("pushUp");
        isPush = false;
        onPushUp.Invoke();
    }

	public void Toggle()
	{
		if (isPush) PushUp();
		else PushDown();
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		//Debug.Log("Enter");
		if (!isPassEnter && GameManager.IsInLayerMask(collision.gameObject.layer, pushableLayer)) {
			if (isToggle) Toggle();
			else if(!isPush)
				PushDown();
		}
    }

    private void OnTriggerExit2D(Collider2D collision) {
		//Debug.Log("Exit");
		if (!isPassExit && GameManager.IsInLayerMask(collision.gameObject.layer, pushableLayer)) {
			if (isToggle)
				Toggle();
			else if (isPush)
				PushUp();
		}
    }
}
