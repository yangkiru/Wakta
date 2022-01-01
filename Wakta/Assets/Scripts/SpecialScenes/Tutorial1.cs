using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial1 : MonoBehaviour {
    private WaitForSeconds wait1 = new WaitForSeconds(1);
    private WaitForSeconds wait2 = new WaitForSeconds(2);
    private WaitForSeconds wait3 = new WaitForSeconds(3);
    public Panzee yangkiru;
    public Button btn;
    public House house;

    private String[] tutorialTalk = {
        "!입장을 치면 밴 당합니다.", "자연스럽게 채팅을 치다보면 입장됩니다.", "지금은 튜토리얼이라 입장이 제한됩니다.", "튜토리얼이 끝나고부터 밴 때리니까 맘껏 치세요..",
        "형 이제 튜토리얼을 시작할께", "+키와 -키로 카메라를 축소 및 확대할 수 있어, 한 번 눌러봐"
    };
    private IEnumerator Start() {
        PanzeeManager.Instance.AddPanzee("yangkiru", yangkiru);
        CameraManager.Instance.cineGroup.AddMember(house.transform, 1f, 3f); // 튜토리얼 끝날 때 빼줘야함
        CameraManager.Instance.cineGroup.AddMember(btn.transform, 1f, 3f); // 튜토리얼 끝날 때 빼줘야함
        yangkiru.SetCommand(Panzee.Command.LeftJump);
        yield return wait1;
        yangkiru.SetCommand(Panzee.Command.Stop);
        yangkiru.SetText("팬돌프들 집중!!");
        for (int i = 0; i < tutorialTalk.Length; i++) {
            yield return wait3;
            yangkiru.SetText(tutorialTalk[i]);
        }
        
        while (!(Input.GetKeyDown(KeyCode.KeypadMinus) || Input.GetKeyDown(KeyCode.KeypadPlus) || Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.Equals))) {
            yield return null;
        }
        yangkiru.SetText("이제 내 밑에 있는 숫자키 눌러볼래?");
        yangkiru.keyButton.transform.parent.gameObject.SetActive(true);
        while (!Input.GetKeyDown(KeyCode.Alpha6)) {
            yield return null;
        }
        yangkiru.SetText("이렇게 팬치 밑에 있는 숫자 버튼을 누르면");
        yield return wait3;
        yangkiru.SetText("해당 팬치를 포커싱 할 수 있어");
        yield return wait3;
        yangkiru.SetText("이제 포커싱한 채로 왼쪽 쉬프트 키를 눌러볼래?");
        while (!Input.GetKeyDown(KeyCode.LeftShift) || Wakta.Instance.selected == null) {
            yield return null;
        }
        yield return wait1;
        yangkiru.SetText("왁타는 팬치를 포커싱하고 왼쪽 쉬프트를 누르면");
        yield return wait3;
        yangkiru.SetText("팬돌프와 연결되어서 이동할 수 있어");
        yield return wait3;
        
        CameraManager.Instance.Focus(house.transform);
        yangkiru.SetText("집의 문이 열렸을 때 왁타가 집에 도착하면");
        yield return wait3;
        yangkiru.SetText("다음 스테이지로 넘어갈 수 있어");
        yield return wait3;
        yangkiru.SetText("그리고 누군가가 버튼을 누르면 문이 열려");
        CameraManager.Instance.Focus(btn.transform);
        yield return wait3;
        yangkiru.SetText("팬돌프들은 !a !d로 좌우로 움직일 수 있어");
        yangkiru.SetCommand(Panzee.Command.Left);
        while (!btn.IsPush) {
            yield return null;
        }
        CameraManager.Instance.FocusOut();
        yangkiru.SetText("형을 집으로 데려가줄게!");
        yield return wait2;
        yangkiru.SetCommand(Panzee.Command.RightRun);
        yangkiru.SetText("팬돌프들은 !A !D로 빨리 갈 수 있어");
        CameraManager.Instance.cineGroup.RemoveMember(house.transform);
        CameraManager.Instance.cineGroup.RemoveMember(btn.transform);
    }
}