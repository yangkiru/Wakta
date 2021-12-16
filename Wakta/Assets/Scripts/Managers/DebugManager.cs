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
            if (Input.GetKeyDown(KeyCode.A))
                (Wakta.Instance.selected as MonoBehaviour).GetComponent<Panzee>().SetCommand(!Input.GetKey(KeyCode.RightShift) ? Panzee.Command.Left : Panzee.Command.LeftRun);
            else if (Input.GetKeyDown(KeyCode.D))
                (Wakta.Instance.selected as MonoBehaviour).GetComponent<Panzee>().SetCommand(!Input.GetKey(KeyCode.RightShift) ? Panzee.Command.Right : Panzee.Command.RightRun);
            else if (Input.GetKeyDown(KeyCode.W))
                (Wakta.Instance.selected as MonoBehaviour).GetComponent<Panzee>().SetCommand(!Input.GetKey(KeyCode.RightShift) ? Panzee.Command.Jump : Panzee.Command.JumpAuto);
            else if (Input.GetKeyDown(KeyCode.S))
                (Wakta.Instance.selected as MonoBehaviour).GetComponent<Panzee>().SetCommand(Panzee.Command.Stop);
			if (Input.GetKeyDown(KeyCode.T))
				(Wakta.Instance.selected as MonoBehaviour).GetComponent<Panzee>().SetText("채팅 테스트");
        }
        
    }

	public void Command(ISelectable selectable, string str)
	{
		bool isWakta = selectable.Equals(Wakta.Instance);
		if (isWakta) {
			switch (str) {
				case "respawn":
					Wakta.Instance.Respawn();
					break;
				case "reload":
					SceneManager.LoadScene("Loading");
					Wakta.Instance.Respawn();
					Panzee.RespawnAll();
					break;
			}
		} else {
			switch (str) {
				case "a":
					(selectable as MonoBehaviour).GetComponent<Panzee>().SetCommand(Panzee.Command.Left);
					break;
				case "A":
					(selectable as MonoBehaviour).GetComponent<Panzee>().SetCommand(Panzee.Command.LeftRun);
					break;
				case "d":
					(selectable as MonoBehaviour).GetComponent<Panzee>().SetCommand(Panzee.Command.Right);
					break;
				case "D":
					(selectable as MonoBehaviour).GetComponent<Panzee>().SetCommand(Panzee.Command.RightRun);
					break;
				case "s":
					(selectable as MonoBehaviour).GetComponent<Panzee>().SetCommand(Panzee.Command.Wait);
					break;
				case "S":
					(selectable as MonoBehaviour).GetComponent<Panzee>().SetCommand(Panzee.Command.Stop);
					break;
				case "respawn":
					(selectable as MonoBehaviour).GetComponent<Panzee>().Respawn();
					break;
				case "kill":
					(selectable as MonoBehaviour).GetComponent<Panzee>().Damage(3);
					break;

			}
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
