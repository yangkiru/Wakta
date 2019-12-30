using UnityEngine;
using System.Collections;

public class Music : MonoSingleton<Music>
{
	private void Awake()
	{
		Instance = this;
	}
}
