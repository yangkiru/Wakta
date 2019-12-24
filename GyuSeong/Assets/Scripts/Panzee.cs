using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Panzee : MonoBehaviour, ISelectable
{
    public CinemachineImpulseSource impulseSource;
    public Transform groundCheck;

    public int maxHp = 3;
    public int currentHp = 3;
    public SpriteRenderer hpRenderer;
    public Color[] hpColor;

    public float speed = 1;
    public float moveForce = 10;
    public float jumpForce = 50;

    public float scale = 1;
    public enum Command {
        Wait, Right, Left, Jump
    }

    [SerializeField]
    private Command command;
    private Command lastCommand;
    private Rigidbody2D rb;
    public Transform Tf { get { return tf; } }
    private Transform tf;
    public TextMeshPro Text { get { return text; } }
    [SerializeField]
    private TextMeshPro text;
    private float textTime;
    private Animator animator;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        tf = transform;
        animator = GetComponentInChildren<Animator>();
    }

    public void SetCommand(Command command) {
        this.command = command;
    }

    public void SetText(string text) {
        this.text.text = text;
        GameObject obj = this.text.gameObject;
        if (!obj.activeSelf)
            StartCoroutine(TextCoroutine(obj, 2));
        else
            textTime = 2;
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

    private void OnDie() {
        Debug.Log("Die");
        if (Wakta.Instance.selected.Equals(this)) {
            Wakta.Instance.selected = null;
            CameraManager.Instance.FocusOut();
        }
        gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        RaycastHit2D isGround = Physics2D.CircleCast(groundCheck.position, groundCheck.lossyScale.y, Vector2.zero, 0, GameManager.Instance.groundLayer);
        switch (command) {
            case Command.Right:
                if (lastCommand != command) {
                    animator.SetFloat("speed", 0.7f);
                    tf.localScale = new Vector3(scale, scale, 1);
                }
                rb.AddForce(Vector2.right * (isGround ? moveForce : moveForce * 0.4f), ForceMode2D.Force);
                break;
            case Command.Left:
                if (lastCommand != command) {
                    animator.SetFloat("speed", 0.7f);
                    tf.localScale = new Vector3(-scale, scale, 1);
                }
                rb.AddForce(Vector2.left * (isGround ? moveForce : moveForce * 0.4f), ForceMode2D.Force);
                break;
            case Command.Jump:
                command = lastCommand;
                
                if (isGround) {
                    rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Force);
                }
                break;
            case Command.Wait:
                if (lastCommand != command)
                    animator.SetFloat("speed", 0);
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
        PanzeeManager.Instance.panzeePool.EnqueueObjectPool(gameObject);
    }

    void OnApplicationQuit() {
        isApplicationQuitting = true;
    }
}
