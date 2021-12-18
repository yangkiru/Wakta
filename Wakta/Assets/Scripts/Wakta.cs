using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Wakta : MonoSingleton<Wakta>, ISelectable
{
    public int damage = 1;

    private Panzee currentPanzee;
    public Transform tf;
    public Rigidbody2D rb;

    public Transform leftHand;
    
	public Transform keyButton;

    public ISelectable selected = null;

    public float scale = 1;
	public List<Panzee> insidePanzee = new List<Panzee>();

	public Animator animator;
	public Animator atkEffAnimator;

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
                //Panzee panzee = (selected as MonoBehaviour).GetComponent<Panzee>();
                //panzee.Damage(damage);
                //attackSound.PlayOneShot(attackClips[Random.Range(0, attackClips.Length)]);
                //panzee.impulseSource.GenerateImpulse();
                animator.SetTrigger("attack");
                //atkEffAnimator.gameObject.SetActive(true);
            }
        }

		if(Input.GetKeyDown(KeyCode.LeftShift)) { //연결
			if (selected != null && !selected.Equals(this)) {
				Panzee panzee = (selected as MonoBehaviour).GetComponent<Panzee>();
				panzee.gravity.enabled = !panzee.gravity.enabled;
				if (insidePanzee.Contains(panzee)) {
					panzee.neckLine.gameObject.SetActive(panzee.gravity.enabled);
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
			//Debug.Log("Panzee In");
			Panzee panzee = collision.GetComponent<Panzee>();
			//if(!insidePanzee.Contains(panzee))
			insidePanzee.Add(panzee);
			if (panzee.gravity.enabled) {
				panzee.neckLine.gameObject.SetActive(true);
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Panzee")) {
			//Debug.Log("Panzee Out");
			Panzee panzee = collision.GetComponent<Panzee>();
			insidePanzee.Remove(panzee);
			panzee.neckLine.gameObject.SetActive(false);
		}
	}
}
