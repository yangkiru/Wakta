using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PortraitManager : MonoSingleton<PortraitManager> {
    public Image[] images;
    public TextMeshProUGUI[] names;

    public void Start() {
        Init();
    }

    public void Init() {
        for (int i = 0; i < images.Length; i++) {
            images[i].gameObject.SetActive(false);
            names[i].enabled = false;
        }
    }

    public void SetPanzee(int idx, Panzee panzee) {
        images[idx].gameObject.SetActive(true);
        names[idx].text = panzee.name;
        names[idx].enabled = true;
    }

    public void RemovePanzee(int idx) {
        images[idx].gameObject.SetActive(false);
        names[idx].text = "";
        names[idx].enabled = false;
    }
}
