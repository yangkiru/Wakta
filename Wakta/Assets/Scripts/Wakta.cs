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

	public bool IsPause {
		get { return isPause; }
	}
	private bool isPause;
	
	private void Start()
	{
		Respawn();
	}

	public void Respawn()
	{
		rb.velocity = Vector2.zero;
		tf.position = GameObject.FindGameObjectWithTag("Respawn").transform.position;
		tf.rotation = Quaternion.identity;
	}

	void Update() {
		if (tf.position.y <= -20)
			Respawn();
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

        if (Input.GetKeyDown(KeyCode.Space)) { //호치
            if (selected != null && !selected.Equals(this)) {
                Panzee panzee = (selected as MonoBehaviour).GetComponent<Panzee>();
                if(insidePanzee.Contains(panzee))
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
			Respawn();
			Panzee.RespawnAll();
		}
	}

	private void LateUpdate()
	{
		Vector3 pos = tf.position;
		pos.z = pos.x * -0.001f;
		tf.position = pos;
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

	private void Select(int index) {
		ISelectable current;
        if (index == 0) {
			current = this;

        }else {
			Panzee temp;
			PanzeeManager.Instance.panzeeDictInOrder.TryGetValue(index, out temp);
			current = temp;
		}
        if (selected != null && selected.Equals(current)) selected = null; //같은걸 다시 선택하면 전체 보기
        else selected = current;
        if (selected != null)
            CameraManager.Instance.Focus((selected as MonoBehaviour).transform);
        else
            CameraManager.Instance.FocusOut();
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
			Debug.Log("Out");
			Panzee panzee = collision.GetComponent<Panzee>();
			insidePanzee.Remove(panzee);
		}
	}
}
