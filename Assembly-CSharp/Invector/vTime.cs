using System;
using UnityEngine;

namespace Invector
{
	// Token: 0x02000498 RID: 1176
	public static class vTime
	{
		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x06001CE7 RID: 7399 RVA: 0x000B2B9D File Offset: 0x000B0D9D
		private static bool unscaledTime
		{
			get
			{
				return Time.timeScale <= 0f && vTime.useUnscaledTime;
			}
		}

		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x06001CE8 RID: 7400 RVA: 0x000B2BB2 File Offset: 0x000B0DB2
		public static float deltaTime
		{
			get
			{
				if (vTime.unscaledTime)
				{
					return Time.unscaledDeltaTime;
				}
				return Time.deltaTime;
			}
		}

		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x06001CE9 RID: 7401 RVA: 0x000B2BC6 File Offset: 0x000B0DC6
		public static float fixedDeltaTime
		{
			get
			{
				if (vTime.unscaledTime)
				{
					return Time.fixedUnscaledDeltaTime;
				}
				return Time.fixedDeltaTime;
			}
		}

		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x06001CEA RID: 7402 RVA: 0x000B2BDA File Offset: 0x000B0DDA
		public static float time
		{
			get
			{
				if (vTime.unscaledTime)
				{
					return Time.unscaledTime;
				}
				return Time.time;
			}
		}

		// Token: 0x06001CEB RID: 7403 RVA: 0x000B2BF0 File Offset: 0x000B0DF0
		public static float GetNormalizedTime(this Animator animator, int layer, int round = 2)
		{
			return (float)Math.Round((double)((animator.IsInTransition(layer) ? animator.GetNextAnimatorStateInfo(layer).normalizedTime : animator.GetCurrentAnimatorStateInfo(layer).normalizedTime) % 1f), round);
		}

		// Token: 0x04001E0A RID: 7690
		public static bool useUnscaledTime;
	}
}
