using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public bool IsOpen { get { return IsOpen; } }
    [SerializeField]
    private bool isOpen;
    private Animator anim;

    private void Awake() {
        anim = GetComponent<Animator>();
        if (isOpen) Open();
        else Close();
    }
    public void Open() {
        anim.SetTrigger("open");
        isOpen = true;
    }

    public void Close() {
        anim.SetTrigger("close");
        isOpen = false;
    }

    public void Toggle() {
        if (isOpen) Close();
        else Open();
    }

    public void Enter() {

    }
}
