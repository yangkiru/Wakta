using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Panzee : MonoBehaviour, ISelectable
{
    public CinemachineImpulseSource impulseSource;
    public Transform groundCheck;

	public LineRenderer neckLine;
	public PointEffector2D gravity;

    public int maxHp = 3;
    public int currentHp = 3;
    public SpriteRenderer hpRenderer;
    public Color[] hpColor;

    public float walkSpeed = 1;
	public float runSpeed = 2;
    public float moveForce = 10;
    public float jumpForce = 50;
	public float superJumpForce = 100;

    public float scale = 1;
    public enum Command {
        Wait, Stop, Right, RightDash, Left, LeftDash, Jump, SuperJump
    }

	public TextMeshPro keyButton;
	public Transform tf;
	public TextMeshPro text;
	public Rigidbody2D rb;

	[SerializeField]
    private Command command;
    private Command lastCommand;
	private float textTime;
    private Animator animator;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

	private void Start()
	{
		Vector2 v = Random.insideUnitCircle;
		rb.velocity = v;
	}

	public void SetCommand(Command command) {
        this.command = command;
    }

    public void SetText(string text) {
        this.text.text = text;
        GameObject obj = this.text.gameObject;
        if (!obj.activeSelf)
            StartCoroutine(TextCoroutine(obj, 3));
        else
            textTime = 3;
    }

    IEnumerator TextCoroutine(GameObject obj, float t) {
        obj.SetActive(true);
        textTime = t;
        do {
            yield return null;
            textTime -= Time.deltaTime;
        } while(textTime > 0);

        obj.SetActive(false);
        yield break;
    }

    public void Damage(int damage) {
        Debug.Log("Damaged");
        currentHp -= damage;
        if (currentHp <= 0)
            OnDie();
        else {
            float s = 0.33f * currentHp;
            hpRenderer.transform.localScale = new Vector3(s, 1, 1);
            hpRenderer.color = hpColor[3 - currentHp];
        }
    }

	private void OnDie()
	{
		Debug.Log("Die");
		if (Wakta.Instance.selected != null && Wakta.Instance.selected.Equals(this)) {
			Wakta.Instance.selected = null;
			CameraManager.Instance.FocusOut();
		}
		rb.velocity = Vector2.zero;
		gameObject.SetActive(false);
	}

	void Update()
	{
		if (tf.position.y <= -20)
			OnDie();
	}

	private void LateUpdate()
	{
		Vector3 pos = tf.position;
		pos.z = pos.x * -0.001f;
		tf.position = pos;
	}

	public void Respawn()
	{
		rb.velocity = Vector2.zero;
		tf.position = GameObject.FindGameObjectWithTag("Respawn").transform.position;
		tf.rotation = Quaternion.identity;
		SetCommand(Command.Wait);
	}

	public static void RespawnAll()
	{
		for (int i = 0; i < PanzeeManager.Instance.panzeeList.Count; i++) {
			PanzeeManager.Instance.panzeeList[i].Respawn();
		}
	}

	void FixedUpdate()
    {
		RaycastHit2D[] hits = Physics2D.BoxCastAll(groundCheck.position, groundCheck.localScale, 0, Vector2.zero, 0, GameManager.Instance.jumpableLayer);// | GameManager.Instance.panzeeLayer);
		bool isGround = false;
		foreach(var h in hits) {
			if (h && !h.collider.CompareTag("Panzee") && h.transform != tf)
				isGround = true;
		}
		switch (command) {
            case Command.Right: case Command.RightDash:
                if (lastCommand != command) {
                    animator.SetFloat("speed", command == Command.RightDash ? 1 :0.7f);
                    tf.localScale = new Vector3(scale, scale, 1);
					text.transform.localScale = tf.localScale;
					keyButton.transform.localScale = tf.localScale;
                }
				if (rb.velocity.x < (command == Command.RightDash ? runSpeed : walkSpeed))
					rb.AddForce(Vector2.right * moveForce, ForceMode2D.Force);
                break;
            case Command.Left: case Command.LeftDash:
                if (lastCommand != command) {
                    animator.SetFloat("speed", command == Command.RightDash ? 1 :0.7f);
                    tf.localScale = new Vector3(-scale, scale, 1);
					text.transform.localScale = tf.localScale;
					keyButton.transform.localScale = tf.localScale;
				}
				if (rb.velocity.x > (command == Command.LeftDash ? -runSpeed : -walkSpeed))
					rb.AddForce(Vector2.left * moveForce, ForceMode2D.Force);
                break;
            case Command.Jump: case Command.SuperJump:
                if (isGround) {
                    rb.AddForce(Vector2.up * ((command == Command.SuperJump ? superJumpForce : jumpForce)), ForceMode2D.Force);
					if (command == lastCommand) command = Command.Wait;
					else
						command = lastCommand;
				}
                break;
            case Command.Wait: case Command.Stop:
				if (lastCommand != command) {
					animator.SetFloat("speed", 0);
				}
				if (command == Command.Stop && Mathf.Abs(rb.velocity.x) < runSpeed) rb.velocity = new Vector2(0, rb.velocity.y);
                break;
        }
        lastCommand = command;
    }

	private bool isApplicationQuitting;

    private void OnDisable() {
        if (isApplicationQuitting) return;
        animator.SetFloat("speed", 0);
        command = Command.Wait;
        lastCommand = Command.Wait;
        text.gameObject.SetActive(false);
        PanzeeManager.Instance.panzeeList.Remove(this);
        PanzeeManager.Instance.panzeeDict.Remove(name);
        currentHp = maxHp;
        hpRenderer.color = hpColor[0];
        hpRenderer.transform.localScale = Vector3.one;
        name = "UNKNOWN";
        CameraManager.Instance.cineGroup.RemoveMember(tf);
		int i;
		int.TryParse(keyButton.text, out i);
		PanzeeManager.Instance.panzeeDictInOrder[i] = null;
		PanzeeManager.Instance.panzeePool.EnqueueObjectPool(gameObject);
    }

    void OnApplicationQuit() {
        isApplicationQuitting = true;
    }
}
