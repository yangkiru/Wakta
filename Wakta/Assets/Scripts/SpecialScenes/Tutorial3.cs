using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial3 : MonoBehaviour {
    private WaitForSeconds wait3 = new WaitForSeconds(3);
    public Panzee yangkiru;

    private IEnumerator Start() {
        if (PanzeeManager.Instance.IsSpawnable) {
            Destroy(yangkiru.gameObject);
            yield break;
        }
        yield return null;
        PanzeeManager.Instance.AddPanzee("yangkiru", yangkiru);
        yangkiru.SetText("팬치들은 !퇴장 을 입력해 포기 할 수 있어");
        yield return wait3;
        yangkiru.SetText("!퇴장 뒤에 입력한 말을 유언으로 남길 수 있어");
        yield return wait3;
        yangkiru.SetText("형 일단 만들긴 했는데");
        yield return wait3;
        yangkiru.SetText("재밌게 해!");
        yield return wait3;
        yangkiru.SetText("형 이제 난 가볼게..");
        yield return wait3;
        yangkiru.Suicide("너무 힘들다.. 난 좀 쉴께..");
        PanzeeManager.Instance.SetSpawnable(true);
    }
}