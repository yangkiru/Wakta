using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanzeeManager : MonoSingleton<PanzeeManager>
{
    public ObjectPool panzeePool;
    public GameObject panzeeObj;
    public Dictionary<string, Panzee> panzeeDict = new Dictionary<string, Panzee>();
    public Dictionary<string, bool> banDict = new Dictionary<string, bool>();
    public Panzee[] panzeeArray = new Panzee[6];
    public Sprite[] itemSprite
    public int maxPanzee = 5;
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
        Vector2 wakPos = Wakta.Instance.tf.position;
		Vector3 pos = wakPos;
		pos.y += 1;
		pos.z = -1;
		panzee.tf.position = pos;
        panzee.name = name;
        panzee.hpParent.SetActive(false);

        AddPanzee(name, panzee);
        panzee.gameObject.SetActive(true);
        panzee.SetText(greeting);
    }

    public void AddPanzee(string name, Panzee panzee) {
	    panzeeDict.Add(name, panzee);
	    for (int i = 0; i < panzeeArray.Length-1; i++) {
		    if (panzeeArray[i] == null) {
			    panzeeArray[i] = panzee;
			    panzee.panzeeIdx = i;
			    panzee.keyButton.text = (i+1).ToString();
			    PortraitManager.Instance.SetPanzee(i, panzee);
			    break;
		    }
	    }
	    CameraManager.Instance.cineGroup.AddMember(panzee.tf, 1f, 3f);
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