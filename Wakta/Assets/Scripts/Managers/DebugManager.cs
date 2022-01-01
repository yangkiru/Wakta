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
		if (isDebug && sceneName.CompareTo(string.Empty) != 0) GameManager.LastScene = sceneName;
			
	}

	private void Update() {
        if (!isDebug) return;

        if (Input.GetKeyDown(KeyCode.P)) {
            if (!Input.GetKey(KeyCode.LeftShift)) {
                Debug.Log("DEBUG:Spawn UNKNOWN Panzee");
                for (int i = 0; i < PanzeeManager.Instance.panzeeArray.Length-1; i++) {
	                Panzee panzee = PanzeeManager.Instance.panzeeArray[i];
	                if (panzee == null) {
		                PanzeeManager.Instance.SpawnPanzee();
		                break;
	                }
                }
            } else {
                Debug.Log("DEBUG:Remove Last Panzee");
                for (int i = PanzeeManager.Instance.panzeeArray.Length-1; i >= 0; i--) {
	                if (PanzeeManager.Instance.panzeeArray[i] != null) {
		                PanzeeManager.Instance.panzeeArray[i].Damage(9999);
		                break;
	                }
                }
            }
        }
        if(Wakta.Instance.selected != null && !Wakta.Instance.selected.Equals(Wakta.Instance)) {
	        Panzee panzee = (Wakta.Instance.selected as MonoBehaviour).GetComponent<Panzee>();
	        
	        panzee.cmdTimer = 9999;
	        if (Input.GetKeyDown(KeyCode.A))
	            panzee.SetCommand(!Input.GetKey(KeyCode.RightShift) ? Panzee.Command.Left : Panzee.Command.LeftRun);
            else if (Input.GetKeyDown(KeyCode.D))
	            panzee.SetCommand(!Input.GetKey(KeyCode.RightShift) ? Panzee.Command.Right : Panzee.Command.RightRun);
            else if (Input.GetKeyDown(KeyCode.W)) {
		        if (Input.GetKey(KeyCode.RightShift)) {
		            panzee.jumpTimerSet = 0.5f;
		            panzee.SetCommand(Panzee.Command.JumpAuto);
	            }
		        else {
			        panzee.jumpTimer = 0;
			        panzee.jumpTimerSet = 9999;
			        panzee.SetCommand(Panzee.Command.Jump);
		        }
            }
            else if (Input.GetKeyDown(KeyCode.S)) {
		        if(Input.GetKey(KeyCode.RightShift))
					panzee.cmdTimer = 1;
		        panzee.SetCommand(Panzee.Command.Stop);
	        }
	        else if (Input.GetKeyDown(KeyCode.Q)) {
		        panzee.cmdTimer = 9999;
		        panzee.SetCommand(!Input.GetKey(KeyCode.RightShift)
			        ? Panzee.Command.LeftJump
			        : Panzee.Command.LeftJumpRun);
	        }
	        else if (Input.GetKeyDown(KeyCode.E)) {
		        panzee.cmdTimer = 9999;
		        panzee.SetCommand(!Input.GetKey(KeyCode.RightShift)
			        ? Panzee.Command.RightJump
			        : Panzee.Command.RightJumpRun);
	        }

	        if (Input.GetKeyDown(KeyCode.T))
				panzee.SetText("채팅 테스트");
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
