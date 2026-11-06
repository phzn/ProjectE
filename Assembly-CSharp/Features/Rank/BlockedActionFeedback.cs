using System;
using System.Collections;
using Features.GameOver;
using Features.Localization;
using ProjectExhibitionist.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Rank
{
	// Token: 0x02000D97 RID: 3479
	public class BlockedActionFeedback : MonoBehaviour
	{
		// Token: 0x06006676 RID: 26230 RVA: 0x0028F6F5 File Offset: 0x0028D8F5
		public static void Show(string message = null)
		{
			BlockedActionFeedback.ShowPresentation(message, null);
		}

		// Token: 0x06006677 RID: 26231 RVA: 0x0028F6FE File Offset: 0x0028D8FE
		public static void ShowComputed(Func<string> resolver)
		{
			BlockedActionFeedback.ShowPresentation(null, resolver);
		}

		// Token: 0x06006678 RID: 26232 RVA: 0x0028F708 File Offset: 0x0028D908
		private static void ShowPresentation(string message, Func<string> resolver)
		{
			if (TextInputFocus.BlocksGameplayInput)
			{
				return;
			}
			if (GameOverManager.Instance != null && GameOverManager.Instance.IsGameOver)
			{
				return;
			}
			BlockedActionFeedback blockedActionFeedback = BlockedActionFeedback.EnsureInstance();
			if (blockedActionFeedback == null)
			{
				Debug.Log("[BlockedActionFeedback] (no host) " + message);
				return;
			}
			blockedActionFeedback.ShowInternal(message, resolver);
		}

		// Token: 0x06006679 RID: 26233 RVA: 0x0028F760 File Offset: 0x0028D960
		private static BlockedActionFeedback EnsureInstance()
		{
			if (BlockedActionFeedback._instance != null && BlockedActionFeedback._instance.isActiveAndEnabled)
			{
				return BlockedActionFeedback._instance;
			}
			BlockedActionFeedback blockedActionFeedback = Object.FindObjectOfType<BlockedActionFeedback>();
			if (blockedActionFeedback != null && blockedActionFeedback.isActiveAndEnabled)
			{
				BlockedActionFeedback._instance = blockedActionFeedback;
				return BlockedActionFeedback._instance;
			}
			BlockedActionFeedback._instance = new GameObject("[BlockedActionFeedback]").AddComponent<BlockedActionFeedback>();
			return BlockedActionFeedback._instance;
		}

		// Token: 0x0600667A RID: 26234 RVA: 0x0028F7C8 File Offset: 0x0028D9C8
		private void Awake()
		{
			if (BlockedActionFeedback._instance != null && BlockedActionFeedback._instance != this)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			BlockedActionFeedback._instance = this;
		}

		// Token: 0x0600667B RID: 26235 RVA: 0x0028F7F6 File Offset: 0x0028D9F6
		private void OnDestroy()
		{
			if (BlockedActionFeedback._instance == this)
			{
				BlockedActionFeedback._instance = null;
			}
		}

		// Token: 0x0600667C RID: 26236 RVA: 0x0028F80B File Offset: 0x0028DA0B
		private void ShowInternal(string message, Func<string> resolver)
		{
			this.SetText(message, resolver);
			if (this._activeRoutine != null)
			{
				base.StopCoroutine(this._activeRoutine);
			}
			this._activeRoutine = base.StartCoroutine(this.RunFeedback());
		}

		// Token: 0x0600667D RID: 26237 RVA: 0x0028F83C File Offset: 0x0028DA3C
		private void SetText(string message, Func<string> resolver)
		{
			this.EnsureUiBuilt();
			string text = ((!string.IsNullOrEmpty(message)) ? message : ((RankProgressionService.Config != null && !string.IsNullOrEmpty(RankProgressionService.Config.blockedActionMessage)) ? RankProgressionService.Config.blockedActionMessage : "Action locked"));
			if (resolver != null)
			{
				LocalizedGameText.BindComputed(this._text, resolver);
				return;
			}
			LocalizedGameText.BindSource(this._text, "gameplay_prompts", text, null);
		}

		// Token: 0x0600667E RID: 26238 RVA: 0x0028F8AE File Offset: 0x0028DAAE
		private IEnumerator RunFeedback()
		{
			this._overlayCanvas.gameObject.SetActive(true);
			yield return this.Animate(this.popFromScale, 1f, 0f, 1f, this.fadeInSeconds);
			float t = 0f;
			while (t < this.holdSeconds)
			{
				t += Time.unscaledDeltaTime;
				yield return null;
			}
			yield return this.Animate(1f, 1f, 1f, 0f, this.fadeOutSeconds);
			this._overlayCanvas.gameObject.SetActive(false);
			this._activeRoutine = null;
			yield break;
		}

		// Token: 0x0600667F RID: 26239 RVA: 0x0028F8BD File Offset: 0x0028DABD
		private IEnumerator Animate(float scaleFrom, float scaleTo, float alphaFrom, float alphaTo, float seconds)
		{
			if (seconds <= 0f)
			{
				this._panelRect.localScale = Vector3.one * scaleTo;
				this._panelGroup.alpha = alphaTo;
				yield break;
			}
			float t = 0f;
			while (t < seconds)
			{
				t += Time.unscaledDeltaTime;
				float num = Mathf.Clamp01(t / seconds);
				float num2 = 1f - Mathf.Pow(1f - num, 3f);
				this._panelRect.localScale = Vector3.one * Mathf.LerpUnclamped(scaleFrom, scaleTo, num2);
				this._panelGroup.alpha = Mathf.Lerp(alphaFrom, alphaTo, num);
				yield return null;
			}
			this._panelRect.localScale = Vector3.one * scaleTo;
			this._panelGroup.alpha = alphaTo;
			yield break;
		}

		// Token: 0x06006680 RID: 26240 RVA: 0x0028F8F4 File Offset: 0x0028DAF4
		private void EnsureUiBuilt()
		{
			if (this._overlayCanvas != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("BlockedActionOverlayCanvas", new Type[]
			{
				typeof(Canvas),
				typeof(CanvasScaler),
				typeof(GraphicRaycaster)
			});
			gameObject.transform.SetParent(base.transform, false);
			gameObject.layer = LayerMask.NameToLayer("UI");
			this._overlayCanvas = gameObject.GetComponent<Canvas>();
			this._overlayCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
			this._overlayCanvas.sortingOrder = 32050;
			GameplayHudVisibility.RegisterCanvas(this._overlayCanvas);
			CanvasScaler component = gameObject.GetComponent<CanvasScaler>();
			component.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			component.referenceResolution = new Vector2(1920f, 1080f);
			component.matchWidthOrHeight = 0.5f;
			GameObject gameObject2 = new GameObject("BlockedActionPanel", new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasGroup),
				typeof(ContentSizeFitter),
				typeof(HorizontalLayoutGroup),
				typeof(Image)
			});
			gameObject2.transform.SetParent(gameObject.transform, false);
			this._panelRect = gameObject2.GetComponent<RectTransform>();
			this._panelRect.anchorMin = new Vector2(0.5f, 0.5f);
			this._panelRect.anchorMax = new Vector2(0.5f, 0.5f);
			this._panelRect.pivot = new Vector2(0.5f, 0.5f);
			this._panelRect.anchoredPosition = new Vector2(0f, 120f);
			gameObject2.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.6f);
			HorizontalLayoutGroup component2 = gameObject2.GetComponent<HorizontalLayoutGroup>();
			component2.padding = new RectOffset(28, 28, 14, 14);
			component2.childAlignment = TextAnchor.MiddleCenter;
			ContentSizeFitter component3 = gameObject2.GetComponent<ContentSizeFitter>();
			component3.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
			component3.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			this._panelGroup = gameObject2.GetComponent<CanvasGroup>();
			this._panelGroup.alpha = 0f;
			this._panelGroup.interactable = false;
			this._panelGroup.blocksRaycasts = false;
			GameObject gameObject3 = new GameObject("Message", new Type[]
			{
				typeof(RectTransform),
				typeof(TextMeshProUGUI)
			});
			gameObject3.transform.SetParent(gameObject2.transform, false);
			this._text = gameObject3.GetComponent<TextMeshProUGUI>();
			this._text.fontSize = 42f;
			this._text.fontStyle = FontStyles.Bold;
			this._text.color = Color.white;
			this._text.alignment = TextAlignmentOptions.Center;
			this._text.enableWordWrapping = false;
			this._text.overflowMode = TextOverflowModes.Overflow;
			this._overlayCanvas.gameObject.SetActive(false);
		}

		// Token: 0x040068FE RID: 26878
		private static BlockedActionFeedback _instance;

		// Token: 0x040068FF RID: 26879
		[SerializeField]
		private float fadeInSeconds = 0.12f;

		// Token: 0x04006900 RID: 26880
		[SerializeField]
		private float holdSeconds = 0.9f;

		// Token: 0x04006901 RID: 26881
		[SerializeField]
		private float fadeOutSeconds = 0.35f;

		// Token: 0x04006902 RID: 26882
		[SerializeField]
		private float popFromScale = 0.7f;

		// Token: 0x04006903 RID: 26883
		private Canvas _overlayCanvas;

		// Token: 0x04006904 RID: 26884
		private RectTransform _panelRect;

		// Token: 0x04006905 RID: 26885
		private CanvasGroup _panelGroup;

		// Token: 0x04006906 RID: 26886
		private TextMeshProUGUI _text;

		// Token: 0x04006907 RID: 26887
		private Coroutine _activeRoutine;
	}
}
