using System;
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

	public LayerMask pushableLayer;

    public UnityEvent onPushDown;
    public UnityEvent onPushUp;

    public SpriteRenderer spriteRenderer;
    public Sprite pushUpSpr;
    public Sprite pushDownSpr;

    private int pushed = 0;
    public void PushDown() {
        Debug.Log("PushDown");
        spriteRenderer.sprite = pushDownSpr;
        isPush = true;
        onPushDown.Invoke();
    }

    public void PushUp() {
        Debug.Log("PushUp");
        spriteRenderer.sprite = pushUpSpr;
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
		pushed++;
		if (!isPassEnter && GameManager.IsInLayerMask(collision.gameObject.layer, pushableLayer)) {
			if (pushed > 0) {
				if (isToggle) Toggle();
				else if (!isPush)
					PushDown();
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision) {
		//Debug.Log("Exit");
		pushed--;
		if (!isPassExit && GameManager.IsInLayerMask(collision.gameObject.layer, pushableLayer)) {
			if (pushed <= 0) {
				if (isToggle)
					Toggle();
				else if (isPush)
					PushUp();
			}
		}
    }
}
