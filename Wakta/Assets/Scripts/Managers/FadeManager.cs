using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoSingleton<FadeManager>
{
	public RawImage image;

	private void Awake()
	{
		Instance = this;
	}

	public void FadeIn(float t, bool ignoreTimescale = true)
	{
		image.canvasRenderer.SetAlpha(1);
		image.CrossFadeAlpha(0, t, ignoreTimescale);
	}

	public void FadeOut(float t, bool ignoreTimescale = true)
	{
		image.canvasRenderer.SetAlpha(0);
		image.CrossFadeAlpha(1, t, ignoreTimescale);
	}
}