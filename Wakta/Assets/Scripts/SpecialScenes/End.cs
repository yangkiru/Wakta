using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class End : MonoBehaviour {
    private WaitForSeconds wait4 = new WaitForSeconds(4);
    public Panzee yangkiru;
    public CanvasGroup cg;
    public RectTransform rt;

    private String[] talk = {
        "형 갑자기 노래가 바뀌었지?",
        "이제 끝이야",
        "이것 저것 계속 바꾸느라 늦었어",
        "늦게 제출해서 미안해",
        "형 뱅송 할 시간이 되어가서",
        "일단 급하게 엔딩부터 만들어봤어",
        "더 만들고 싶었는데",
        "산타가 주제인 게임이다보니",
        "점점 약발 떨어질 것 같기도 하고ㅋㅋ",
        "또 아쉬운 모습만 보여준 것 같네..",
        "2021년 한 해 동안 수고했고",
        "올해도 재밌는 방송 부탁해",
        "해피 뉴 이어!!",
        "새해 복 많이 받아!",
        "팬치 여러분도 새해 복 많이 받으세요!!"
    };
    private IEnumerator Start() {
        GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>().mute = true;
        PanzeeManager.Instance.AddPanzee("yangkiru", yangkiru);
        for (int i = 0; i < talk.Length; i++) {
            yangkiru.SetText(talk[i]);
            yield return wait4;
        }
        Vector3 pos = rt.position;
        for (float t = 0; t < 50; t += Time.deltaTime) {
            pos.y += Time.deltaTime * 20;
            cg.alpha += Time.deltaTime * 0.1f;
            rt.position = pos;
            yield return null;
        }

        for (; cg.alpha >= 0; cg.alpha -= Time.deltaTime) {
            yield return null;
        }

    }
}