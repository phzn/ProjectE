using System;
using System.Collections;
using UnityEngine;

namespace TMPro
{
	// Token: 0x0200002B RID: 43
	internal class TweenRunner<T> where T : struct, ITweenValue
	{
		// Token: 0x0600015C RID: 348 RVA: 0x00017A75 File Offset: 0x00015C75
		private static IEnumerator Start(T tweenInfo)
		{
			if (!tweenInfo.ValidTarget())
			{
				yield break;
			}
			float elapsedTime = 0f;
			while (elapsedTime < tweenInfo.duration)
			{
				elapsedTime += (tweenInfo.ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime);
				float num = Mathf.Clamp01(elapsedTime / tweenInfo.duration);
				tweenInfo.TweenValue(num);
				yield return null;
			}
			tweenInfo.TweenValue(1f);
			yield break;
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00017A84 File Offset: 0x00015C84
		public void Init(MonoBehaviour coroutineContainer)
		{
			this.m_CoroutineContainer = coroutineContainer;
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00017A90 File Offset: 0x00015C90
		public void StartTween(T info)
		{
			if (this.m_CoroutineContainer == null)
			{
				Debug.LogWarning("Coroutine container not configured... did you forget to call Init?");
				return;
			}
			this.StopTween();
			if (!this.m_CoroutineContainer.gameObject.activeInHierarchy)
			{
				info.TweenValue(1f);
				return;
			}
			this.m_Tween = TweenRunner<T>.Start(info);
			this.m_CoroutineContainer.StartCoroutine(this.m_Tween);
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00017AFF File Offset: 0x00015CFF
		public void StopTween()
		{
			if (this.m_Tween != null)
			{
				this.m_CoroutineContainer.StopCoroutine(this.m_Tween);
				this.m_Tween = null;
			}
		}

		// Token: 0x04000158 RID: 344
		protected MonoBehaviour m_CoroutineContainer;

		// Token: 0x04000159 RID: 345
		protected IEnumerator m_Tween;
	}
}
