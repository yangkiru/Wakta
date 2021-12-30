using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial2 : MonoBehaviour {
    private WaitForSeconds wait1 = new WaitForSeconds(1);
    private WaitForSeconds wait3 = new WaitForSeconds(3);
    private WaitForSeconds wait4 = new WaitForSeconds(4);
    public Panzee yangkiru;

    private IEnumerator Start() {
        yield return null;
        PanzeeManager.Instance.AddPanzee("yangkiru", yangkiru);
        yangkiru.keyButton.text = "Dev";
        yangkiru.SetText("이번엔 팬치들의 다양한 동작을 알려줄게");
        yield return wait3;
        yangkiru.SetCommand(Panzee.Command.Jump);
        yangkiru.SetText("!w를 입력하면 점프할 수 있고");
        yield return wait4;
        yangkiru.SetCommand(Panzee.Command.JumpAuto);
        yangkiru.SetText("!W를 입력하면 계속 점프할 수 있어");
        yield return wait4;
        yangkiru.SetCommand(Panzee.Command.Stop);
        yield return wait3;
        yangkiru.cmdTimer = 9999;
        yangkiru.SetCommand(Panzee.Command.RightJump);
        yangkiru.SetText("!q나 !e를 입력하면 좌우로 점프 할 수 있어");
        yield return wait4;
        yangkiru.SetText("물론 대문자로 입력하면 더 빠르게 갈 수 있어");
        yield return wait3;
        yangkiru.SetCommand(Panzee.Command.Stop);
        yangkiru.SetText("!s를 입력하면 멈출 수 있어");
        yield return wait4;
        yangkiru.cmdTimer = 2.5f;
        yangkiru.SetCommand(Panzee.Command.Left);
        yangkiru.SetText("!a 2.5 같이 숫자와 함께 이동 명령을 내리면");
        yield return wait4;
        yangkiru.SetText("그만큼만 움직이고 멈춰");
        yield return wait3;
        yangkiru.cmdTimer = 3;
        yangkiru.SetCommand(Panzee.Command.Jump);
        yangkiru.SetText("!w 3 같이 숫자와 함께 점프 명령을 내리면");
        yield return wait4;
        yangkiru.SetText("그만큼 기다렸다가 점프를 해");
        yield return wait3;
        yangkiru.cmdTimer = 3f;
        yangkiru.SetCommand(Panzee.Command.JumpAuto);
        yangkiru.SetText("!W 3 같이 숫자와 함께 점프 반복 명령을 내리면");
        yield return wait4;
        yangkiru.SetText("그만큼의 공백을 갖고 점프를 해");
        yield return wait3;
        yangkiru.SetText("!q나 !e를 숫자와 함께 명령을 내리면");
        yangkiru.cmdTimer = 3f;
        yangkiru.SetCommand(Panzee.Command.LeftJump);
        yield return wait4;
        yangkiru.SetText("이동으로 간주해서, 그만큼 이동하고 멈춰");
        yield return wait4;
        yangkiru.SetText("형, 이제 팬치를 조련하는 법을 알려줄게");
        yield return wait3;
        yangkiru.SetText("날 포커싱하고 연결해봐");
        yangkiru.keyButton.transform.parent.gameObject.SetActive(true);
        while (!yangkiru.joint.enabled) {
            yield return null;
        }
        yield return wait1;
        yangkiru.SetText("연결한 상태에서 스페이스 바를 눌러봐");
        while (!Input.GetKeyDown(KeyCode.Space)) {
            yield return null;
        }
        yield return wait3;
        yangkiru.SetText("연결 안 해도 때릴 수 있긴 해");
        yield return wait3;
        yangkiru.SetText("팬돌프는 최대 5번까지 맞을 수 있고");
        yield return wait4;
        yangkiru.SetText("형은 A와 D를 눌러서 팬치가 날아갈 방향을 정할 수 있어");
        yield return wait4;
        yangkiru.SetText("날 때려서 반대편으로 날려봐");
    }
}