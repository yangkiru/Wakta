using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoSingleton<DebugManager>
{
    public bool isDebug = false;

    private void Update() {
        if (!isDebug) return;

        if (Input.GetKeyDown(KeyCode.P)) {
            if (!Input.GetKey(KeyCode.LeftShift)) {
                Debug.Log("DEBUG:Spawn UNKNOWN Panzee");
                PanzeeManager.Instance.SpawnPanzee();
            } else {
                Debug.Log("DEBUG:Remove Last Panzee");
                PanzeeManager.Instance.panzeeList[PanzeeManager.Instance.panzeeList.Count - 1].gameObject.SetActive(false);
            }
        }

        if(Wakta.Instance.selected != null && !Wakta.Instance.selected.Equals(Wakta.Instance)) {
            if (Input.GetKeyDown(KeyCode.A))
                (Wakta.Instance.selected as MonoBehaviour).GetComponent<Panzee>().SetCommand(Panzee.Command.Left);
            else if (Input.GetKeyDown(KeyCode.D))
                (Wakta.Instance.selected as MonoBehaviour).GetComponent<Panzee>().SetCommand(Panzee.Command.Right);
            else if (Input.GetKeyDown(KeyCode.W))
                (Wakta.Instance.selected as MonoBehaviour).GetComponent<Panzee>().SetCommand(Panzee.Command.Jump);
            else if (Input.GetKeyDown(KeyCode.S))
                (Wakta.Instance.selected as MonoBehaviour).GetComponent<Panzee>().SetCommand(Panzee.Command.Wait);
        }
        
    }
}
