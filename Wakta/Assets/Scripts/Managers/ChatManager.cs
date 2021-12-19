using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatManager : MonoSingleton<ChatManager> {
    [SerializeField]
    private List<TextMeshProUGUI> TMPList = new List<TextMeshProUGUI>();

    [SerializeField]
    private List<string> textList = new List<string>();

    public void AddText(string text) {
        if (textList.Count >= TMPList.Count) textList.RemoveAt(textList.Count-1);
        textList.Insert(0, text);
        for (int i = 0; i < textList.Count; i++) {
            TMPList[i].text = textList[i];
        }
    }
}
