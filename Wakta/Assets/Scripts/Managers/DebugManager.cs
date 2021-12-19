using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugManager : MonoSingleton<DebugManager>
{
    public bool isDebug = false;
	public string sceneName = string.Empty;

	private void Awake()
	{
		if (sceneName.CompareTo(string.Empty) != 0) GameManager.LastScene = sceneName;
	}

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
	        (Wakta.Instance.selected as MonoBehaviour).GetComponent<Panzee>().cmdTimer = 9999;
            if (Input.GetKeyDown(KeyCode.A))
                (Wakta.Instance.selected as MonoBehaviour).GetComponent<Panzee>().SetCommand(!Input.GetKey(KeyCode.RightShift) ? Panzee.Command.Left : Panzee.Command.LeftRun);
            else if (Input.GetKeyDown(KeyCode.D))
                (Wakta.Instance.selected as MonoBehaviour).GetComponent<Panzee>().SetCommand(!Input.GetKey(KeyCode.RightShift) ? Panzee.Command.Right : Panzee.Command.RightRun);
            else if (Input.GetKeyDown(KeyCode.W))
            {
	            Panzee panzee = (Wakta.Instance.selected as MonoBehaviour).GetComponent<Panzee>();
	            panzee.SetCommand(Panzee.Command.Jump);
	            if (Input.GetKey(KeyCode.RightShift)) {
		            panzee.cmdTimer = 0;
		            panzee.isJumpAuto = true;
		            panzee.jumpTimer = 0;
		            panzee.jumpTimerSet = 0.5f;
	            }
            }
            else if (Input.GetKeyDown(KeyCode.S))
                (Wakta.Instance.selected as MonoBehaviour).GetComponent<Panzee>().SetCommand(Panzee.Command.Stop);
            else if (Input.GetKeyDown(KeyCode.Q))
	            (Wakta.Instance.selected as MonoBehaviour).GetComponent<Panzee>().SetCommand(!Input.GetKey(KeyCode.RightShift) ? Panzee.Command.LeftJump : Panzee.Command.LeftJumpRun);
            else if (Input.GetKeyDown(KeyCode.E))
	            (Wakta.Instance.selected as MonoBehaviour).GetComponent<Panzee>().SetCommand(!Input.GetKey(KeyCode.RightShift) ? Panzee.Command.RightJump : Panzee.Command.RightJumpRun);
			if (Input.GetKeyDown(KeyCode.T))
				(Wakta.Instance.selected as MonoBehaviour).GetComponent<Panzee>().SetText("채팅 테스트");
        }
        
    }

	public void Enter(string scene)
	{
		GameManager.LastScene = scene;
		FadeManager.Instance.FadeOut(1);
		Invoke("LoadScene", 1);
	}

	public void LoadScene()
	{
		SceneManager.LoadScene("Loading");
	}
}
