using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Invector
{
	// Token: 0x020004C5 RID: 1221
	public class vBlindGhostVFX : MonoBehaviour
	{
		// Token: 0x06001D86 RID: 7558 RVA: 0x000B5792 File Offset: 0x000B3992
		public void TriggerGoBlind(vIEffect effect)
		{
			if (effect != null)
			{
				this.TriggerGoBlind(effect.EffectDuration);
			}
		}

		// Token: 0x06001D87 RID: 7559 RVA: 0x000B57A3 File Offset: 0x000B39A3
		public void TriggerGoBlind(float duration)
		{
			base.StopAllCoroutines();
			base.StartCoroutine(this.GhostEffectRoutine(duration));
		}

		// Token: 0x06001D88 RID: 7560 RVA: 0x000B57B9 File Offset: 0x000B39B9
		private IEnumerator GhostEffectRoutine(float duration)
		{
			yield return new WaitForEndOfFrame();
			Texture2D texture2D = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
			texture2D.ReadPixels(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), 0, 0);
			texture2D.Apply();
			if (this.imageToDisplay == null)
			{
				Debug.LogWarning("Please assign the Image component");
			}
			this.imageToDisplay.texture = texture2D;
			this.StartGhostFX();
			yield return new WaitForSeconds(duration);
			this.FinishGhostFX();
			yield break;
		}

		// Token: 0x06001D89 RID: 7561 RVA: 0x000B57CF File Offset: 0x000B39CF
		private void StartGhostFX()
		{
			this.onStartGhostFX.Invoke();
		}

		// Token: 0x06001D8A RID: 7562 RVA: 0x000B57DC File Offset: 0x000B39DC
		private void FinishGhostFX()
		{
			this.onFinishGhostFX.Invoke();
		}

		// Token: 0x04001EB1 RID: 7857
		[SerializeField]
		private RawImage imageToDisplay;

		// Token: 0x04001EB2 RID: 7858
		public UnityEvent onStartGhostFX;

		// Token: 0x04001EB3 RID: 7859
		public UnityEvent onFinishGhostFX;
	}
}
