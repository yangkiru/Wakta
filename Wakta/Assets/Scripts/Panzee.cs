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

public class Panzee : MonoBehaviour, ISelectable
{
    public CinemachineImpulseSource impulseSource;
    public Transform groundCheck;

	public LineRenderer neckLine;
	public PointEffector2D gravity;

    public int maxHp = 5;
    public int currentHp = 5;
    public Image hpRenderer;

    public float walkSpeed = 1;
	public float runSpeed = 2;
    public float moveForce = 10;
    public float jumpForce = 50;
    public float cmdTimer = 0;
    public float jumpTimer = 0;
    public float jumpTimerSet = 0;
	
    public float scale = 1;
    public enum Command {
        Wait, Stop, Jump, Left, LeftRun, LeftJump, LeftJumpRun, Right, RightRun, RightJump, RightJumpRun
    }
	public bool isJumpAuto = false;

	public TextMeshPro keyButton;
	public Transform tf;
	public TextMeshProUGUI text;
	public Rigidbody2D rb;

	[SerializeField]
    private Command command;
    private Command lastCommand;
    
	private float textTime;
	[SerializeField]
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
	
	void Update()
	{
		if (tf.position.y <= -20)
			OnDie();
		if (textTime > 0)
			textTime -= Time.deltaTime;
		else
			text.gameObject.SetActive(false);
	}

	private void LateUpdate()
	{
		Vector3 pos = tf.position;
		pos.z = pos.x * -0.001f;
		tf.position = pos;
	}
	
	void FixedUpdate()
	{
		if (cmdTimer > 0) cmdTimer -= Time.fixedDeltaTime;
		RaycastHit2D[] hits = Physics2D.BoxCastAll(groundCheck.position, groundCheck.localScale, 0, Vector2.zero, 0, GameManager.Instance.jumpableLayer);// | GameManager.Instance.panzeeLayer);
		bool isGround = false;
		for (var index = 0; index < hits.Length; index++)
		{
			var h = hits[index];
			if (h && !h.collider.CompareTag("Panzee") && h.transform != tf)
				isGround = true;
		}
		if (jumpTimer > 0) jumpTimer -= Time.fixedDeltaTime;
		if (command == Command.Jump ||
		    command == Command.LeftJump ||
		    command == Command.RightJump ||
		    command == Command.LeftJumpRun ||
		    command == Command.RightJumpRun ||
		    (isJumpAuto && (jumpTimer <= 0)))
		{
			jumpTimer = jumpTimerSet;
			if (isGround) {
				rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Force);

				if (command == Command.Jump) (command, lastCommand) = (lastCommand, command);
				else command -= 2;
			}
		}
		
		
		switch (command) {
            case Command.Right: case Command.RightRun:
                if (lastCommand != command) {
                    animator.SetFloat(Speed, command == Command.RightRun ? 1.5f :1f);
                    tf.localScale = new Vector3(scale, scale, 1);
					text.transform.localScale = tf.localScale;
					keyButton.transform.localScale = tf.localScale;
                }

                if (cmdTimer <= 0)
                {
	                lastCommand = command;
	                command = Command.Stop;
                }
                rb.velocity = new Vector2(moveForce * (command == Command.RightRun ? runSpeed : walkSpeed), rb.velocity.y);
                break;
            case Command.Left: case Command.LeftRun:
                if (lastCommand != command) {
                    animator.SetFloat(Speed, command == Command.RightRun ? 1.5f :1f);
                    tf.localScale = new Vector3(-scale, scale, 1);
					text.transform.localScale = tf.localScale;
					keyButton.transform.localScale = tf.localScale;
				}
                if (cmdTimer <= 0)
                {
	                lastCommand = command;
	                command = Command.Stop;
                }
                rb.velocity = new Vector2(-moveForce * (command == Command.LeftRun ? runSpeed : walkSpeed), rb.velocity.y);
                break;
            case Command.Stop: case Command.Wait:
	            if (isGround)
	            {
		            rb.velocity = new Vector2(0, rb.velocity.y);
		            animator.SetFloat(Speed, 0);
	            }
	            break;
        }

		lastCommand = command;
	}

	public void SetCommand(Command command) {
        this.command = command;
        isJumpAuto = false;
	}

    public void SetText(string text) {
        this.text.text = text;
        StringBuilder sb = new StringBuilder("[");
        sb.Append(keyButton.text).Append(']').Append(name).Append(':').Append(text);
        ChatManager.Instance.AddText(sb.ToString());
        this.text.gameObject.SetActive(true);
        textTime = 7;
    }

    public void Damage(int damage) {
        Debug.Log("Damaged");
        currentHp -= damage;
        if (currentHp <= 0)
            OnDie();
        else {
	        float s = (float)currentHp / maxHp;
	        hpRenderer.fillAmount = s;
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
		RIPManager.Instance.SpawnRIP(this, text.text);
		gameObject.SetActive(false);
		
	}

	public void Suicide(string lastWord) {
		Debug.Log("Suicide");
		OnDie();
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

	private bool isApplicationQuitting;
	private static readonly int Speed = Animator.StringToHash("speed");

	private void OnDisable() {
        if (isApplicationQuitting) return;
        animator.SetFloat("speed", 0);
        command = Command.Wait;
        lastCommand = Command.Wait;
        text.gameObject.SetActive(false);
        PanzeeManager.Instance.panzeeList.Remove(this);
        PanzeeManager.Instance.panzeeDict.Remove(name);
        currentHp = maxHp;
        hpRenderer.fillAmount = 1;
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
