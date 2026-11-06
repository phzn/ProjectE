using System;
using System.Collections;
using ProjectExhibitionist.Core;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectExhibitionist.Subway
{
	// Token: 0x0200093C RID: 2364
	public class SubwayScreenFade : MonoBehaviour
	{
		// Token: 0x06004052 RID: 16466 RVA: 0x0017DFB8 File Offset: 0x0017C1B8
		private static SubwayScreenFade Ensure()
		{
			if (SubwayScreenFade._instance != null)
			{
				return SubwayScreenFade._instance;
			}
			GameObject gameObject = new GameObject("SubwayScreenFade");
			SubwayScreenFade._instance = gameObject.AddComponent<SubwayScreenFade>();
			Canvas canvas = gameObject.AddComponent<Canvas>();
			canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			canvas.sortingOrder = 30000;
			GameplayHudVisibility.RegisterCanvas(canvas);
			GameObject gameObject2 = new GameObject("Black");
			gameObject2.transform.SetParent(gameObject.transform, false);
			Image image = gameObject2.AddComponent<Image>();
			image.color = Color.black;
			image.raycastTarget = false;
			RectTransform rectTransform = image.rectTransform;
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.offsetMin = Vector2.zero;
			rectTransform.offsetMax = Vector2.zero;
			SubwayScreenFade._instance._group = gameObject.AddComponent<CanvasGroup>();
			SubwayScreenFade._instance._group.alpha = 0f;
			SubwayScreenFade._instance._group.blocksRaycasts = false;
			SubwayScreenFade._instance._group.interactable = false;
			return SubwayScreenFade._instance;
		}

		// Token: 0x06004053 RID: 16467 RVA: 0x0017E0B8 File Offset: 0x0017C2B8
		public static Coroutine Fade(float targetAlpha, float seconds)
		{
			SubwayScreenFade subwayScreenFade = SubwayScreenFade.Ensure();
			if (subwayScreenFade._routine != null)
			{
				subwayScreenFade.StopCoroutine(subwayScreenFade._routine);
			}
			subwayScreenFade._routine = subwayScreenFade.StartCoroutine(subwayScreenFade.FadeRoutine(Mathf.Clamp01(targetAlpha), seconds));
			return subwayScreenFade._routine;
		}

		// Token: 0x06004054 RID: 16468 RVA: 0x0017E100 File Offset: 0x0017C300
		public static void ClearImmediately()
		{
			if (SubwayScreenFade._instance == null)
			{
				return;
			}
			if (SubwayScreenFade._instance._routine != null)
			{
				SubwayScreenFade._instance.StopCoroutine(SubwayScreenFade._instance._routine);
			}
			SubwayScreenFade._instance._routine = null;
			if (SubwayScreenFade._instance._group != null)
			{
				SubwayScreenFade._instance._group.alpha = 0f;
			}
		}

		// Token: 0x06004055 RID: 16469 RVA: 0x0017E16C File Offset: 0x0017C36C
		private IEnumerator FadeRoutine(float target, float seconds)
		{
			float from = this._group.alpha;
			float t = 0f;
			while (t < seconds)
			{
				t += Time.deltaTime;
				this._group.alpha = Mathf.Lerp(from, target, Mathf.Clamp01(t / Mathf.Max(0.01f, seconds)));
				yield return null;
			}
			this._group.alpha = target;
			this._routine = null;
			yield break;
		}

		// Token: 0x04004027 RID: 16423
		private static SubwayScreenFade _instance;

		// Token: 0x04004028 RID: 16424
		private CanvasGroup _group;

		// Token: 0x04004029 RID: 16425
		private Coroutine _routine;
	}
}
