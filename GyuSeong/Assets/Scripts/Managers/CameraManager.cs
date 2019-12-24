using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoSingleton<CameraManager>
{
//    public CinemachineVirtualCamera
    public CinemachineTargetGroup cineGroup;

    public void Focus(Transform target) {
        if (!target) FocusOut();

        int index = cineGroup.FindMember(target);
        for (int i = 0; i < cineGroup.m_Targets.Length; i++) {
            cineGroup.m_Targets[i].weight = i == index ? 1 : 0;
        }
    }

    public void FocusOut() {
        for (int i = 0; i < cineGroup.m_Targets.Length; i++) {
            cineGroup.m_Targets[i].weight = 1;
        }
    }

    public void Shake() {
        
    }
}
