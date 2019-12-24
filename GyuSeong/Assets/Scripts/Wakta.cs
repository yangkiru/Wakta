using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wakta : MonoSingleton<Wakta>, ISelectable
{
    public int damage = 1;

    private Panzee currentPanzee;
    public Transform Tf { get { return tf; } }
    private Transform tf;
    public Rigidbody2D rb;

    public Transform leftHand;

    public AudioClip[] attackClips;
    public AudioSource attackSound;

    public ISelectable selected = null;

    public float scale = 1;

    private void Awake() {
        tf = transform;
    }

    // Update is called once per frame
    void Update() {
        if (rb.velocity.x > 0.5f)
            tf.localScale = new Vector3(scale, scale, 1);
        else if (rb.velocity.x < -0.5f)
            tf.localScale = new Vector3(-scale, scale, 1);

        if (Input.GetKeyDown(KeyCode.BackQuote))
            Select(-1);
        else if (Input.GetKeyDown(KeyCode.Alpha1))
            Select(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            Select(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            Select(2);
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            Select(3);
        else if (Input.GetKeyDown(KeyCode.Alpha5))
            Select(4);

        if (Input.GetKeyDown(KeyCode.Space)) { //호치
            if (selected != null && !selected.Equals(this)) {
                Panzee panzee = (selected as MonoBehaviour).GetComponent<Panzee>();
                panzee.Damage(damage);
                attackSound.PlayOneShot(attackClips[Random.Range(0, attackClips.Length)]);
                panzee.impulseSource.GenerateImpulse();
            }
        }
    }

    private void Select(int index) {
        ISelectable current;
        if (index >= 0) {
            if (PanzeeManager.Instance.panzeeList.Count <= index) return;
             current = PanzeeManager.Instance.panzeeList[index];
        }else {
            current = this;
        }
        if (selected != null && selected.Equals(current)) selected = null; //같은걸 다시 선택하면 전체 보기
        else selected = current;
        if (selected != null)
            CameraManager.Instance.Focus((selected as MonoBehaviour).transform);
        else
            CameraManager.Instance.FocusOut();
    }
}
