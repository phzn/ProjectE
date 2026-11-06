using System;
using System.Collections;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x020000FF RID: 255
	public class MMImage : MonoBehaviour
	{
		// Token: 0x06000576 RID: 1398 RVA: 0x0001B2BA File Offset: 0x000194BA
		public static IEnumerator Flicker(Renderer renderer, Color initialColor, Color flickerColor, float flickerSpeed, float flickerDuration)
		{
			if (renderer == null)
			{
				yield break;
			}
			if (!renderer.material.HasProperty("_Color"))
			{
				yield break;
			}
			if (initialColor == flickerColor)
			{
				yield break;
			}
			float flickerStop = Time.time + flickerDuration;
			while (Time.time < flickerStop)
			{
				renderer.material.color = flickerColor;
				yield return MMCoroutine.WaitFor(flickerSpeed);
				renderer.material.color = initialColor;
				yield return MMCoroutine.WaitFor(flickerSpeed);
			}
			renderer.material.color = initialColor;
			yield break;
		}
	}
}
