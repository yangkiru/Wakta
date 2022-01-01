using System;
using System.Text;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Panzee : MonoBehaviour, ISelectable {
	public CinemachineImpulseSource impulseSource;

	public LineRenderer neckLine;
	public DistanceJoint2D joint;
	public BoxCollider2D coll;

	public int maxHp = 5;
	public int currentHp = 5;
	public int panzeeIdx;
	public String chatTag;
	public GameObject hpParent;
	public Image hpRenderer;
	public SpriteRenderer spriteRenderer;
	public SpriteRenderer itemRenderer;
	public CanvasGroup canvasGroup;

	public float walkSpeed = 1;
	public float runSpeed = 2;
	public float moveForce = 10;
	public float jumpForce = 50;
	public float cmdTimer = 0;
	public float jumpTimer = 9999;
	public float jumpTimerSet = 9999;

	public float scale = 1;

	public Color rendererColor = Color.white;

	public enum Command {
		Wait,
		Stop,
		Jump,
		JumpAuto,
		Left,
		LeftRun,
		LeftJump,
		LeftJumpRun,
		Right,
		RightRun,
		RightJump,
		RightJumpRun
	}

	public TextMeshProUGUI keyButton;
	public Transform tf;
	public RectTransform textBubble;
	public TextMeshProUGUI text;
	public Rigidbody2D rb;

	[SerializeField] private Command command;
	private Command lastCommand;

	private float textTime;
	[SerializeField] private Animator animator;

	private bool isInit = false;
	private bool isGround = false;

	public bool IsPause {
		get { return isPause; }
	}

	private bool isPause = false;

	private void Awake() {
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponentInChildren<Animator>();
	}

	private void Start() {
		Vector2 v = Random.insideUnitCircle;
		rb.velocity = v;
	}

	private void LateUpdate() {
		Vector3 pos = tf.position;
		pos.z = pos.y * -0.001f;
		tf.position = pos;
	}

	void Update() {
		if (!isInit)
			isInit = true;
		if (tf.position.y <= -20)
			OnDie();
		if (textTime > 0)
			textTime -= Time.deltaTime;
		else
			textBubble.gameObject.SetActive(false);
		if (cmdTimer > 0 && cmdTimer < 9999) cmdTimer -= Time.deltaTime;
		isGround = IsGrounded();
		// RaycastHit2D[] hits = Physics2D.BoxCastAll(groundCheck.position, groundCheck.localScale, 0, Vector2.zero, 0,
		// 	GameManager.Instance.jumpableLayer); // | GameManager.Instance.panzeeLayer);
		// for (var index = 0; index < hits.Length; index++) {
		// 	var h = hits[index];
		// 	if (h && !h.collider.CompareTag("Panzee") && h.transform != tf)
		// 		isGround = true;
		// }

		if (jumpTimer > 0 && jumpTimer < 9999) jumpTimer -= Time.deltaTime;

		if (jumpTimer <= 0)
			Jump();
		switch (command) {
			case Command.Right:
			case Command.RightRun:
				if (cmdTimer <= 0) {
					lastCommand = command;
					command = Command.Stop;
				} {
				float move = moveForce * (command == Command.RightRun ? runSpeed : walkSpeed);
				float result = Mathf.Max(move, rb.velocity.x);
				rb.velocity = new Vector2(result, rb.velocity.y);
			}
				break;
			case Command.Left:
			case Command.LeftRun:
				if (cmdTimer <= 0) {
					lastCommand = command;
					command = Command.Stop;
				} {
				float move = -moveForce * (command == Command.LeftRun ? runSpeed : walkSpeed);
				float result = Mathf.Min(move, rb.velocity.x);
				rb.velocity = new Vector2(result, rb.velocity.y);
			}
				break;
			case Command.Stop:
			case Command.Wait:
				if (isGround) {
					rb.velocity = new Vector2(rb.velocity.x * 0.9f, rb.velocity.y);
					animator.SetFloat(Speed, 0);
				}

				break;
		}

		if (command != Command.Jump)
			lastCommand = command;
	}

	private bool IsGrounded() {
		float extraHeightText = 0.1f;
		RaycastHit2D[] raycastHits = Physics2D.BoxCastAll(coll.bounds.center, coll.bounds.size*0.8f, 0f, Vector2.down,
			extraHeightText, GameManager.Instance.jumpableLayer);
		Color rayColor;
		bool result = false;
		for (int i = 0; i < raycastHits.Length; i++) {
			if (raycastHits[i].collider != coll) {
				result = true;
				break;
			}
		}
		if (result) {
			rayColor = Color.green;
		}
		else {
			rayColor = Color.red;
		}
		return result;
	}

	private void MoveInit() {
		animator.SetFloat(Speed, command == Command.LeftRun || command == Command.RightRun ? 1.5f : 1f);
		float vec = (command == Command.Left || command == Command.LeftRun) ? -1 : 1;
		tf.localScale = new Vector3(vec * scale, scale, 1);
		text.transform.localScale = tf.localScale;
		keyButton.transform.localScale = tf.localScale;
	}

	public void Jump() {
		if (isGround) {
			rb.velocity = new Vector2(rb.velocity.x, jumpForce);
			Debug.Log("last:"+lastCommand + " current:"+command);
			command = lastCommand;
			jumpTimer = jumpTimerSet;
		}
	}

	public void Pause() {
		rb.velocity = Vector2.zero;
		rb.isKinematic = true;
		SetCommand(Command.Stop);
		tf.position = GameObject.FindGameObjectWithTag("Respawn").transform.position;
		joint.enabled = false;
		neckLine.gameObject.SetActive(false);
		isPause = true;
	}

	public void UnPause() {
		rb.isKinematic = false;
		isPause = false;
	}

	public void SetCommand(Command cmd) {
		switch (cmd) {
			case Command.JumpAuto:
				jumpTimer = 0;
				break;
			case Command.Jump:
				break;
			case Command.LeftJump:
				lastCommand = Command.Left;
				command = Command.Left;
				MoveInit();
				jumpTimer = 0;
				jumpTimerSet = 9999;
				break;
			case Command.LeftJumpRun:
				lastCommand = Command.LeftRun;
				command = Command.LeftRun;
				MoveInit();
				jumpTimer = 0;
				jumpTimerSet = 9999;
				break;
			case Command.RightJump:
				lastCommand = Command.Right;
				command = Command.Right;
				MoveInit();
				jumpTimer = 0;
				jumpTimerSet = 9999;
				break;
			case Command.RightJumpRun:
				lastCommand = Command.RightRun;
				command = Command.RightRun;
				MoveInit();
				jumpTimer = 0;
				jumpTimerSet = 9999;
				break;
			case Command.Left:
			case Command.LeftRun:
			case Command.Right:
			case Command.RightRun:
				command = cmd;
				MoveInit();
				jumpTimerSet = 9999;
				jumpTimer = 9999;
				break;
			case Command.Stop:
				jumpTimer = 9999;
				jumpTimerSet = 9999;
				command = cmd;
				break;
			default:
				command = cmd;
				break;
		}
	}

	public void SetText(string text) {
		if (text == String.Empty) return;
		this.text.text = text;
		StringBuilder sb = new StringBuilder("[");
		sb.Append(chatTag).Append(']').Append(name).Append(':').Append(text);
		ChatManager.Instance.AddText(sb.ToString());
		this.textBubble.gameObject.SetActive(true);
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.textBubble);
		textTime = 7;
	}

	public void Damage(int damage) {
		currentHp -= damage;
		if (!hpParent.activeSelf)
			hpParent.SetActive(true);
		if (currentHp <= 0)
			OnDie();
		else {
			float s = (float) currentHp / maxHp;
			hpRenderer.fillAmount = s;
		}
	}

	private void OnDie(string lastWord = "") {
		if (Wakta.Instance.selected != null && Wakta.Instance.selected.Equals(this)) {
			Wakta.Instance.selected = null;
			CameraManager.Instance.FocusOut();
		}

		rb.velocity = Vector2.zero;
		joint.enabled = false;
		RIPManager.Instance.SpawnRIP(this, lastWord);
		if (panzeeIdx != 5)
			PortraitManager.Instance.RemovePanzee(panzeeIdx);
		gameObject.SetActive(false);
	}

	public void Suicide(string lastWord) {
		OnDie(lastWord);
	}

	public void SetAlpha(float a) {
		rendererColor.a = a;
		spriteRenderer.color = rendererColor;
		itemRenderer.color = rendererColor;
		canvasGroup.alpha = a;
	}

	public static int respawnNum = 0;
	public void Respawn() {
		rb.velocity = Vector2.zero;
		GameObject[] respawn = GameObject.FindGameObjectsWithTag("PanzeeRespawn");
		
		if (respawn.Length == 0) respawn = GameObject.FindGameObjectsWithTag("Respawn");
		Vector2 pos = respawn[(respawnNum++)%respawn.Length].transform.position;
		tf.position = pos;
		tf.rotation = Quaternion.identity;
		SetCommand(Command.Wait);
	}

	public static void RespawnAll() {
		for (int i = 0; i < PanzeeManager.Instance.panzeeArray.Length; i++) {
			Panzee panzee = PanzeeManager.Instance.panzeeArray[i];
			if (panzee != null && panzee.gameObject.activeSelf)
				panzee.Respawn();
		}
	}

	private static readonly int Speed = Animator.StringToHash("speed");

	private bool isQuit = false;

	private void OnApplicationQuit() {
		isQuit = true;
	}

	private void OnDisable() {
		if (isQuit || !isInit) return;
		if (Wakta.Instance.selected != null && Wakta.Instance.selected.Equals(this)) Wakta.Instance.selected = null;
		Panzee panzee;
		if (PanzeeManager.Instance.panzeeDict.TryGetValue(name, out panzee)) {
			PanzeeManager.Instance.panzeeArray[panzee.panzeeIdx] = null;
			PanzeeManager.Instance.panzeeDict.Remove(name);
		}

		if (panzeeIdx != 5) {
			animator.SetFloat("speed", 0);
			command = Command.Wait;
			lastCommand = Command.Wait;
			text.gameObject.SetActive(false);

			currentHp = maxHp;
			hpRenderer.fillAmount = 1;
			name = "UNKNOWN";
			CameraManager.Instance.cineGroup.RemoveMember(tf);
			PanzeeManager.Instance.panzeePool.EnqueueObjectPool(gameObject);
		}
	}
}
