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
	[SerializeField]
	private bool isDev = false;
    public CinemachineImpulseSource impulseSource;
    public Transform groundCheck;

	public LineRenderer neckLine;
	public DistanceJoint2D joint;

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
	public RectTransform textBubble;
	public TextMeshProUGUI text;
	public Rigidbody2D rb;

	[SerializeField]
    private Command command;
    private Command lastCommand;
    
	private float textTime;
	[SerializeField]
    private Animator animator;

    private bool isInit = false;

    public bool IsPause {
	    get { return isPause; }
    }
    private bool isPause = false;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

	private void Start()
	{
		Vector2 v = Random.insideUnitCircle;
		rb.velocity = v;
	}
	
	private void LateUpdate()
	{
		Vector3 pos = tf.position;
		pos.z = pos.x * -0.001f;
		tf.position = pos;
	}
	void Update()
	{
		if(!isInit)
			isInit = true;
		if (tf.position.y <= -20)
			OnDie();
		if (textTime > 0)
			textTime -= Time.deltaTime;
		else
			textBubble.gameObject.SetActive(false);
		if (cmdTimer > 0) cmdTimer -= Time.deltaTime;
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
	
	public void Pause() {
		rb.velocity = Vector2.zero;
		rb.isKinematic = true;
		tf.position = GameObject.FindGameObjectWithTag("Respawn").transform.position;
		isPause = true;
	}
	
	public void UnPause() {
		rb.isKinematic = false;
		isPause = false;
	}

	public void SetCommand(Command command) {
        this.command = command;
        isJumpAuto = false;
	}

    public void SetText(string text) {
	    if (text == String.Empty) return;
        this.text.text = text;
        StringBuilder sb = new StringBuilder("[");
        sb.Append(keyButton.text).Append(']').Append(name).Append(':').Append(text);
        ChatManager.Instance.AddText(sb.ToString());
        this.textBubble.gameObject.SetActive(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate(this.textBubble);
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
		joint.enabled = false;
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
			if(PanzeeManager.Instance.panzeeList[i].gameObject.activeSelf)
				PanzeeManager.Instance.panzeeList[i].Respawn();
		}
	}
	private static readonly int Speed = Animator.StringToHash("speed");

	private bool isQuit = false;

	private void OnApplicationQuit() {
		isQuit = true;
	}

	private void OnDisable() {
        if (isQuit || !isInit) return;
        if (!isDev) {
	        animator.SetFloat("speed", 0);
	        command = Command.Wait;
	        lastCommand = Command.Wait;
	        text.gameObject.SetActive(false);

	        currentHp = maxHp;
	        hpRenderer.fillAmount = 1;
	        name = "UNKNOWN";
	        CameraManager.Instance.cineGroup.RemoveMember(tf);
	        int i;
	        int.TryParse(keyButton.text, out i);
	        PanzeeManager.Instance.panzeeList.Remove(this);
	        PanzeeManager.Instance.panzeeDict.Remove(name);
	        PanzeeManager.Instance.panzeeDictInOrder[i] = null;
	        PanzeeManager.Instance.panzeePool.EnqueueObjectPool(gameObject);
        }

        if (Wakta.Instance.selected.Equals(this)) Wakta.Instance.selected = null;
		PanzeeManager.Instance.panzeeDict.Remove(name);
		PanzeeManager.Instance.panzeeList.Remove(this);
		int output;
		if(int.TryParse(keyButton.text, out output))
			PanzeeManager.Instance.panzeeDictInOrder.Remove(output);
    }


}
