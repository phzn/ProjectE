using System;
using System.Collections;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001AC RID: 428
	public static class MMFeedbacksCoroutine
	{
		// Token: 0x06000A52 RID: 2642 RVA: 0x0002F36B File Offset: 0x0002D56B
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

		// Token: 0x06000A53 RID: 2643 RVA: 0x0002F37A File Offset: 0x0002D57A
		public static IEnumerator WaitFor(float seconds)
		{
			for (float timer = 0f; timer < seconds; timer += Time.deltaTime)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x06000A54 RID: 2644 RVA: 0x0002F389 File Offset: 0x0002D589
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
