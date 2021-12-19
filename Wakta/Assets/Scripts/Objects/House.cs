using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class House : MonoBehaviour
{
    public bool IsOpen { get { return IsOpen; } }
    [SerializeField]
    private bool isOpen;

    public UnityEvent onEnter;
    public SpriteRenderer spriteRenderer;
    public Sprite closeSpr;
    public Sprite openSpr;

	private void Awake() {
        if (isOpen) Open();
        else Close();
	}
	
	public void Open() {
        isOpen = true;
        spriteRenderer.sprite = openSpr;
	}

    public void Close() {
        isOpen = false;
        spriteRenderer.sprite = closeSpr;
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
