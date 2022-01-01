using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class PanzeeManager : MonoSingleton<PanzeeManager>
{
    public ObjectPool panzeePool;
    public GameObject panzeeObj;
    public Dictionary<string, Panzee> panzeeDict = new Dictionary<string, Panzee>();
    public Dictionary<string, bool> banDict = new Dictionary<string, bool>();
    public Panzee[] panzeeArray = new Panzee[6];
    public Sprite[] itemSprites = new Sprite[6];
    public int maxPanzee = 5;
    public const String devName = "yangkiru";
    public bool IsSpawnable {
	    get { return isSpawnable; }
    }
    [SerializeField]
    private bool isSpawnable = false;


    #region DEBUG
    private int unknownCount = 0;
    #endregion
    private void Awake() {
        for (int i = 0; i < 10; i++) {
            panzeePool.EnqueueObjectPool(Instantiate(panzeeObj));
        }
        for (int i = 0; i < 5; i++) {
	        panzeeArray[i] = null;
        }
    }

    public void SetSpawnable(bool value) {
	    isSpawnable = value;
    }
    public void SpawnPanzee() {
        SpawnPanzee(string.Format("UNKNOWN{0}", unknownCount++), "");
    }

    public void SpawnPanzee(string name, string greeting) {
	    if (!isSpawnable) return;
        Panzee panzee = panzeePool.DequeueObjectPool().GetComponent<Panzee>();
        panzee.name = name;
        panzee.hpParent.SetActive(false);
		if(Wakta.Instance.selected != null) panzee.SetAlpha(0.3f);
        AddPanzee(name, panzee);
        panzee.gameObject.SetActive(true);
        panzee.SetText(greeting);
        panzee.Respawn();
    }

    public void AddPanzee(string name, Panzee panzee) {
	    panzeeDict.Add(name, panzee);
	    if (name.Equals(devName)) {
		    int idx = panzeeArray.Length-1;
		    panzeeArray[idx] = panzee;
		    panzee.panzeeIdx = idx;
		    panzee.keyButton.text = (idx+1).ToString();
		    panzee.chatTag = "Dev";
		    panzee.itemRenderer.sprite = itemSprites[idx];
		    CameraManager.Instance.cineGroup.AddMember(panzee.tf, 1f, 3f);
		    return;
	    }
	    for (int i = 0; i < panzeeArray.Length-1; i++) {
		    if (panzeeArray[i] == null) {
			    panzeeArray[i] = panzee;
			    panzee.panzeeIdx = i;
			    panzee.chatTag = (i+1).ToString();
			    panzee.keyButton.text = (i+1).ToString();
			    panzee.itemRenderer.sprite = itemSprites[i];
			    PortraitManager.Instance.SetPanzee(i, panzee);
			    CameraManager.Instance.cineGroup.AddMember(panzee.tf, 1f, 3f);
			    break;
		    }
	    }
    }

    public void SetAlphaFocusPanzee(Panzee panzee) {
	    for (int i = 0; i < panzeeArray.Length; i++) {
		    if (panzeeArray[i] != null && panzeeArray[i] != panzee)
				panzeeArray[i].SetAlpha(0.3f);
	    }
	    panzee.SetAlpha(1);
    }
    
    public void SetAlphaFocusPanzee(bool isOne = true) {
	    float a = isOne ? 1 : 0.3f;
	    for (int i = 0; i < panzeeArray.Length; i++) {
		    if (panzeeArray[i] != null)
				panzeeArray[i].SetAlpha(a);
	    }
    }
}