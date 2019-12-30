using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
	public float time;
	public UnityEvent onTimer;
	private void OnEnable()
	{
		Invoke("ActivateEvent", time);
	}

	private void ActivateEvent()
	{
		if (!gameObject.activeSelf) return;
		onTimer.Invoke();
	}
}
