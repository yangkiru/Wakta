using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanzeeManager : MonoSingleton<PanzeeManager>
{
    public ObjectPool panzeePool;
    public GameObject panzeeObj;
    public Dictionary<string, Panzee> panzeeDict = new Dictionary<string, Panzee>();
    public List<Panzee> panzeeList = new List<Panzee>();
	public Dictionary<int, Panzee> panzeeDictInOrder = new Dictionary<int, Panzee>();
    public int maxPanzee = 5;
    [SerializeField]
    private bool isSpawnable = false;


    #region DEBUG
    private int unknownCount = 0;
    #endregion
    private void Awake() {
        for (int i = 0; i < 10; i++) {
            panzeePool.EnqueueObjectPool(Instantiate(panzeeObj));
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
        panzeeDict.Add(name, panzee);
        panzeeList.Add(panzee);
		for(int i = 1; i <= maxPanzee; i++) {
			Panzee temp;
			panzeeDictInOrder.TryGetValue(i, out temp);
			if (temp == null) {
				panzeeDictInOrder[i] = panzee;
				KeyUpdate(i);
				break;
			}
		}
		Vector2 wakPos = Wakta.Instance.tf.position;
		Vector3 pos = wakPos;
		pos.z = -1;
		panzee.tf.position = pos;
        panzee.name = name;

        panzee.gameObject.SetActive(true);
        panzee.SetText(greeting);
        CameraManager.Instance.cineGroup.AddMember(panzee.tf, 1f, 3f);
	}

	public void KeyUpdate(int i)
	{
		panzeeDictInOrder[i].keyButton.text = (i).ToString();
	}
}