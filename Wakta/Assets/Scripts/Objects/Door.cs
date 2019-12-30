using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public bool IsOpen { get { return IsOpen; } }
    [SerializeField]
    private bool isOpen;
    private Animator anim;
    
	public UnityEvent onEnter;

	private void Awake() {
        anim = GetComponent<Animator>();
        if (isOpen) Open();
        else Close();
		Wakta.Instance.Respawn();
		for (int i = 0; i < PanzeeManager.Instance.panzeeList.Count; i++)
			PanzeeManager.Instance.panzeeList[i].Respawn();
	}

	private void Start()
	{
		FadeManager.Instance.FadeIn(2);
	}
	public void Open() {
        anim.SetTrigger("open");
        isOpen = true;
    }

    public void Close() {
        anim.SetTrigger("close");
        isOpen = false;
    }

    public void Toggle() {
        if (isOpen) Close();
        else Open();
    }

	private void OnTriggerStay2D(Collider2D collision)
	{
		bool isWakta = GameManager.IsInLayerMask(collision.gameObject.layer, GameManager.Instance.waktaLayer);
		if (!isOpen || isEnter || !isWakta)
			return;

		Debug.Log("Stay");
		isEnter = true;
		onEnter.Invoke();
	}

	bool isEnter;

	public void Enter(string scene) {
		GameManager.LastScene = scene;
		FadeManager.Instance.FadeOut(1);
		Invoke("LoadScene", 1);
	}

	public void LoadScene()
	{
		SceneManager.LoadScene("Loading");
	}
}
