using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class Move : MonoBehaviour
{
	public Vector2 velocity;
	public Vector2 goal;
	public UnityEvent onGoal;

	private Rigidbody2D rb;
	private float checkStop;
	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	public void SetGoal(Transform tf)
	{
		goal = tf.position;
		this.enabled = true;
	}

	public void SetVelocity(Transform tf)
	{
		velocity = tf.localPosition;
		checkStop = velocity.magnitude * 0.001f;
	}

	public void SetVelocityX(float x)
	{
		velocity = new Vector2(x, velocity.y);
		checkStop = velocity.magnitude * 0.001f;
	}

	public void SetVelocityY(float y)
	{
		velocity = new Vector2(velocity.x, y);
		checkStop = velocity.magnitude * 0.001f;
	}

	private void FixedUpdate()
	{
		if (!rb) return;
		if (Vector2.SqrMagnitude(rb.position - goal) <= checkStop) {

			rb.MovePosition(goal);
			rb.velocity = Vector2.zero;
			onGoal.Invoke();
			this.enabled = false;
		} else {
			rb.velocity = velocity;
		}
	}
}
