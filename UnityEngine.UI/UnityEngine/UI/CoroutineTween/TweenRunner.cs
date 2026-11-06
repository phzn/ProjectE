using System;
using System.Collections;

namespace UnityEngine.UI.CoroutineTween
{
	// Token: 0x0200004A RID: 74
	internal class TweenRunner<T> where T : struct, ITweenValue
	{
		// Token: 0x060004F9 RID: 1273 RVA: 0x000174AD File Offset: 0x000156AD
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

		// Token: 0x060004FA RID: 1274 RVA: 0x000174BC File Offset: 0x000156BC
		public void Init(MonoBehaviour coroutineContainer)
		{
			this.m_CoroutineContainer = coroutineContainer;
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x000174C8 File Offset: 0x000156C8
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

		// Token: 0x060004FC RID: 1276 RVA: 0x00017537 File Offset: 0x00015737
		public void StopTween()
		{
			if (this.m_Tween != null)
			{
				this.m_CoroutineContainer.StopCoroutine(this.m_Tween);
				this.m_Tween = null;
			}
		}

		// Token: 0x040001A3 RID: 419
		protected MonoBehaviour m_CoroutineContainer;

		// Token: 0x040001A4 RID: 420
		protected IEnumerator m_Tween;
	}
}
