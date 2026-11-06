using System;
using System.Collections;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x020000F8 RID: 248
	public static class MMCoroutine
	{
		// Token: 0x06000536 RID: 1334 RVA: 0x000192C3 File Offset: 0x000174C3
		public static IEnumerator WaitForFrames(int frameCount)
		{
			while (frameCount > 0)
			{
				int num = frameCount;
				frameCount = num - 1;
				yield return null;
			}
			yield break;
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x000192D2 File Offset: 0x000174D2
		public static IEnumerator WaitFor(float seconds)
		{
			for (float timer = 0f; timer < seconds; timer += Time.deltaTime)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x000192E1 File Offset: 0x000174E1
		public static IEnumerator WaitForUnscaled(float seconds)
		{
			for (float timer = 0f; timer < seconds; timer += Time.unscaledDeltaTime)
			{
				yield return null;
			}
			yield break;
		}
	}
}
