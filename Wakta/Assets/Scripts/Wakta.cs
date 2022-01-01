using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Wakta : MonoSingleton<Wakta>, ISelectable
{
    public int damage = 1;
    
    public Transform tf;
    public Rigidbody2D rb;

    public Transform leftHand;
    
	public Transform keyButton;

    public ISelectable selected = null;

    public float scale = 1;
	public List<Panzee> insidePanzee = new List<Panzee>();

	public Animator animator;

	public SpriteRenderer spriteRenderer;
	public CanvasGroup canvasGroup;
	private Color color = Color.white;

	public bool IsPause {
		get { return isPause; }
	}
	private bool isPause;

	public void Respawn()
	{
		rb.velocity = Vector2.zero;
		GameObject respawn = GameObject.FindGameObjectWithTag("WaktaRespawn");
		if (respawn == null) respawn = GameObject.FindGameObjectWithTag("Respawn");
		tf.position = respawn.transform.position;
		tf.rotation = Quaternion.identity;
	}

	void Update() {
		if (tf.position.y <= -20) {
			for (int i = 0; i < PanzeeManager.Instance.panzeeArray.Length; i++) {
				Panzee panzee = PanzeeManager.Instance.panzeeArray[i];
				if (panzee != null) {
					panzee.Suicide(panzee.text.text);
				}
			}
			SceneManager.LoadScene("Loading");
		}
		if (rb.velocity.x > 0.5f) {
			tf.localScale = new Vector3(scale, scale, 1);
			keyButton.localScale = tf.localScale;
		} else if (rb.velocity.x < -0.5f) {
			tf.localScale = new Vector3(-scale, scale, 1);
			keyButton.localScale = tf.localScale;
		}

        if (Input.GetKeyDown(KeyCode.BackQuote))
            Select(0);
        else if (Input.GetKeyDown(KeyCode.Alpha1))
            Select(1);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            Select(2);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            Select(3);
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            Select(4);
        else if (Input.GetKeyDown(KeyCode.Alpha5))
            Select(5);
        else if (Input.GetKeyDown(KeyCode.Alpha6))
	        Select(6);

        if (Input.GetKeyDown(KeyCode.Space)) { //호치
            if (selected != null && !selected.Equals(this)) {
                Panzee panzee = (selected as MonoBehaviour).GetComponent<Panzee>();
                if(panzee != null)
	                animator.SetTrigger("attack");
            }
        }

		if(Input.GetKeyDown(KeyCode.LeftShift)) { //연결
			if (selected != null && !selected.Equals(this)) {
				Panzee panzee = (selected as MonoBehaviour).GetComponent<Panzee>();

				if (!panzee.joint.enabled && insidePanzee.Contains(panzee)) {
					panzee.joint.enabled = true;
					
					panzee.neckLine.gameObject.SetActive(true);
					if (panzee.joint.connectedBody == null)
						panzee.joint.connectedBody = rb;
				}
				else {
					panzee.joint.enabled = false;
					panzee.neckLine.gameObject.SetActive(false);
				}
			}
		}

		if (Input.GetKeyDown(KeyCode.R)) {
			SceneManager.LoadScene("Loading");
		}
	}

	private void LateUpdate()
	{
		Vector3 pos = tf.position;
		pos.z = pos.y * -0.001f;
		tf.position = pos;
	}
	
	public void Pause() {
		rb.velocity = Vector2.zero;
		rb.rotation = 0;
		rb.angularVelocity = 0;
		rb.isKinematic = true;
		tf.position = GameObject.FindGameObjectWithTag("Respawn").transform.position;
		selected = null;
		isPause = true;
	}
	
	public void UnPause() {
		rb.isKinematic = false;
		isPause = false;
	}

	private void Select(int index) {
		if (index == 0) {
	        if (this.Equals(selected)) { // Foucs Out
				selected = null;
				CameraManager.Instance.FocusOut();
				SetAlpha(1);
				PanzeeManager.Instance.SetAlphaFocusPanzee(true);
	        }
	        else {
		        selected = this;
		        CameraManager.Instance.Focus(tf);
		        SetAlpha(1);
		        PanzeeManager.Instance.SetAlphaFocusPanzee(false);
	        }
        }else {
			Panzee panzee = PanzeeManager.Instance.panzeeArray[index-1];
			if (panzee != null && panzee.Equals(selected)) { // Focus Out
				selected = null;
				PanzeeManager.Instance.SetAlphaFocusPanzee();
				CameraManager.Instance.FocusOut();
				SetAlpha(1);
			}
			else if (panzee != null) {
				selected = panzee;
				PanzeeManager.Instance.SetAlphaFocusPanzee(panzee);
				CameraManager.Instance.Focus(panzee.tf);
				SetAlpha(1);
			}
		}
    }

	private void SetAlpha(float a) {
		color.a = a;
		spriteRenderer.color = color;
		canvasGroup.alpha = a;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.CompareTag("Panzee")) {
			Panzee panzee = collision.GetComponent<Panzee>();
			insidePanzee.Add(panzee);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Panzee")) {
			Panzee panzee = collision.GetComponent<Panzee>();
			insidePanzee.Remove(panzee);
		}
	}
}
