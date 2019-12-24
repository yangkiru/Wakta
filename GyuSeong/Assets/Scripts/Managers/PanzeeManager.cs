using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanzeeManager : MonoSingleton<PanzeeManager>
{
    public ObjectPool panzeePool;
    public GameObject panzeeObj;
    public Dictionary<string, Panzee> panzeeDict = new Dictionary<string, Panzee>();
    public List<Panzee> panzeeList = new List<Panzee>();
    public int maxPanzee = 5;


    #region DEBUG
    private int unknownCount = 0;
    #endregion

    private void Awake() {
        for (int i = 0; i < 10; i++) {
            panzeePool.EnqueueObjectPool(Instantiate(panzeeObj));
        }
    }

    [ContextMenu("SpawnPanzee")]
    public void SpawnPanzee() {
        SpawnPanzee(string.Format("UNKNOWN{0}", unknownCount++), "");
    }

    public void SpawnPanzee(string name, string greeting) {
        Panzee panzee = panzeePool.DequeueObjectPool().GetComponent<Panzee>();
        panzeeDict.Add(name, panzee);
        panzeeList.Add(panzee);
        Vector3 pos = Wakta.Instance.Tf.position;
        pos.x += Random.Range(1, 3);
        pos.z = -1;
        panzee.Tf.position = pos;
        panzee.name = name;

        panzee.gameObject.SetActive(true);
        panzee.SetText(greeting);
        CameraManager.Instance.cineGroup.AddMember(panzee.Tf, 1f, 3f);
    }
}
