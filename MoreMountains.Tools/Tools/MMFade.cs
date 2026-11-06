using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MoreMountains.Tools
{
	// Token: 0x020000FB RID: 251
	public static class MMFade
	{
		// Token: 0x0600056B RID: 1387 RVA: 0x0001AEC0 File Offset: 0x000190C0
		public static IEnumerator FadeImage(Image target, float duration, Color color)
		{
			if (target == null)
			{
				yield break;
			}
			float alpha = target.color.a;
			for (float t = 0f; t < 1f; t += Time.deltaTime / duration)
			{
				if (target == null)
				{
					yield break;
				}
				Color color2 = new Color(color.r, color.g, color.b, Mathf.SmoothStep(alpha, color.a, t));
				target.color = color2;
				yield return null;
			}
			target.color = color;
			yield break;
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x0001AEDD File Offset: 0x000190DD
		public static IEnumerator FadeText(Text target, float duration, Color color)
		{
			if (target == null)
			{
				yield break;
			}
			float alpha = target.color.a;
			for (float t = 0f; t < 1f; t += Time.deltaTime / duration)
			{
				if (target == null)
				{
					yield break;
				}
				Color color2 = new Color(color.r, color.g, color.b, Mathf.SmoothStep(alpha, color.a, t));
				target.color = color2;
				yield return null;
			}
			target.color = color;
			yield break;
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x0001AEFA File Offset: 0x000190FA
		public static IEnumerator FadeSprite(SpriteRenderer target, float duration, Color color)
		{
			if (target == null)
			{
				yield break;
			}
			float alpha = target.material.color.a;
			float t = 0f;
			while (t < 1f)
			{
				if (target == null)
				{
					yield break;
				}
				Color color2 = new Color(color.r, color.g, color.b, Mathf.SmoothStep(alpha, color.a, t));
				target.material.color = color2;
				t += Time.deltaTime / duration;
				yield return null;
			}
			Color color3 = new Color(color.r, color.g, color.b, Mathf.SmoothStep(alpha, color.a, t));
			if (target != null)
			{
				target.material.color = color3;
			}
			yield break;
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x0001AF17 File Offset: 0x00019117
		public static IEnumerator FadeCanvasGroup(CanvasGroup target, float duration, float targetAlpha, bool unscaled = true)
		{
			if (target == null)
			{
				yield break;
			}
			float currentAlpha = target.alpha;
			float t = 0f;
			while (t < 1f)
			{
				if (target == null)
				{
					yield break;
				}
				float num = Mathf.SmoothStep(currentAlpha, targetAlpha, t);
				target.alpha = num;
				if (unscaled)
				{
					t += Time.unscaledDeltaTime / duration;
				}
				else
				{
					t += Time.deltaTime / duration;
				}
				yield return null;
			}
			target.alpha = targetAlpha;
			yield break;
		}
	}
}
