using System;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoSingleton<CameraManager>
{
    
    public CinemachineTargetGroup cineGroup;

    private CinemachineVirtualCamera vcam;
    private CinemachineFramingTransposer cineFramTrans;

    [SerializeField] private Vector2 zoomLimit = Vector2.zero;
    [SerializeField] private float zoomSpd = 0.1f;
    private void Awake()
	{
		cineGroup = FindObjectOfType<CinemachineTargetGroup>();
        vcam = FindObjectOfType<CinemachineVirtualCamera>();
        cineFramTrans = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
        //cineFramTrans.m_GroupFramingSize -= 0.1f;
    }

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

    public void Zoom(float amount)
    {
        cineFramTrans.m_GroupFramingSize = Math.Clamp(cineFramTrans.m_GroupFramingSize+amount, zoomLimit.x, zoomLimit.y);
        Debug.Log("zoom "+cineFramTrans.m_GroupFramingSize);
    }

    public void Update()
    {
        if(Input.GetKey(KeyCode.KeypadPlus) || Input.GetKey(KeyCode.Equals)) Zoom(zoomSpd * Time.deltaTime);
        else if (Input.GetKey(KeyCode.KeypadMinus) || Input.GetKey(KeyCode.Minus)) Zoom(-zoomSpd * Time.deltaTime);
    }
}
