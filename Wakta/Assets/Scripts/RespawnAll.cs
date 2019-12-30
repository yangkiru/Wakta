using UnityEngine;
using System.Collections;

public class RespawnAll : MonoBehaviour
{
	// Use this for initialization
	void Start()
	{
		Wakta.Instance.Respawn();
		Panzee.RespawnAll();
	}
}
