using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Condition : MonoBehaviour
{
	public bool IsTrue { get { return isTrue; } set { isTrue = value; } }
	public bool isTrue;
	public bool isEventOnStart;

	public List<GameObject> objs = new List<GameObject>();
	public UnityEvent onTrue;
	public UnityEvent onFalse;

	private bool last;
	private void FixedUpdate()
	{
		last = isEventOnStart ? !isTrue : isTrue;
		bool temp = true;
		for (int i = 0; i < objs.Count; i++) {
			if (!objs[i].activeSelf) {
				temp = false;
				break;
			}
		}
		isTrue = temp;
		if (isTrue != last) {
			if (isTrue) onTrue.Invoke();
			else onFalse.Invoke();
		}
		last = isTrue;
	}
}
