using System;
using System.Collections;
using System.Collections.Generic;
using DA_Assets.DAB;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Features.MainMenu
{
	// Token: 0x0200105D RID: 4189
	public sealed class MainMenuEntranceAnimator : MonoBehaviour
	{
		// Token: 0x17001466 RID: 5222
		// (get) Token: 0x06007F25 RID: 32549 RVA: 0x0033F7D4 File Offset: 0x0033D9D4
		// (set) Token: 0x06007F26 RID: 32550 RVA: 0x0033F7DC File Offset: 0x0033D9DC
		public bool IsComplete { get; private set; }

		// Token: 0x06007F27 RID: 32551 RVA: 0x0033F7E5 File Offset: 0x0033D9E5
		private void Awake()
		{
			this.CacheTargets();
			this.PrepareHidden();
		}

		// Token: 0x06007F28 RID: 32552 RVA: 0x0033F7F3 File Offset: 0x0033D9F3
		private IEnumerator Start()
		{
			yield return null;
			while (!this._playRequested && SceneManager.GetSceneByName("GameWarmUp").isLoaded)
			{
				yield return null;
			}
			if (!this._playRequested)
			{
				this.PlayEntrance();
			}
			yield break;
		}

		// Token: 0x06007F29 RID: 32553 RVA: 0x0033F802 File Offset: 0x0033DA02
		private void OnDisable()
		{
			if (this._entranceRoutine != null)
			{
				base.StopCoroutine(this._entranceRoutine);
				this._entranceRoutine = null;
			}
			this.RestoreFinalState();
		}

		// Token: 0x06007F2A RID: 32554 RVA: 0x0033F825 File Offset: 0x0033DA25
		public void PlayEntrance()
		{
			if (this._entranceRoutine != null || this.IsComplete)
			{
				return;
			}
			this._playRequested = true;
			this._entranceRoutine = base.StartCoroutine(this.AnimateEntrance());
		}

		// Token: 0x06007F2B RID: 32555 RVA: 0x0033F854 File Offset: 0x0033DA54
		public void RegisterButton(RectTransform rect)
		{
			if (rect == null)
			{
				return;
			}
			for (int i = 0; i < this._buttons.Count; i++)
			{
				if (this._buttons[i] != null && this._buttons[i].rect == rect)
				{
					return;
				}
			}
			CanvasGroup orAddCanvasGroup = MainMenuEntranceAnimator.GetOrAddCanvasGroup(rect.gameObject);
			this._buttons.Add(new MainMenuEntranceAnimator.ButtonState
			{
				rect = rect,
				group = orAddCanvasGroup,
				baseScale = rect.localScale,
				baseRotation = rect.localRotation
			});
			if (this.IsComplete)
			{
				orAddCanvasGroup.alpha = 1f;
				orAddCanvasGroup.blocksRaycasts = true;
				orAddCanvasGroup.interactable = true;
				return;
			}
			orAddCanvasGroup.alpha = 0f;
			orAddCanvasGroup.blocksRaycasts = false;
			orAddCanvasGroup.interactable = false;
		}

		// Token: 0x06007F2C RID: 32556 RVA: 0x0033F928 File Offset: 0x0033DB28
		private void CacheTargets()
		{
			GameObject gameObject = GameObject.Find("MainMenu/FigmaCanvas/Main Menu - v4/Logo/Image");
			if (gameObject != null)
			{
				this._logoRect = gameObject.transform as RectTransform;
				this._logoGroup = MainMenuEntranceAnimator.GetOrAddCanvasGroup(gameObject);
				this._logoBaseScale = this._logoRect.localScale;
				this._logoBaseRotation = this._logoRect.localRotation;
			}
			GameObject gameObject2 = GameObject.Find("MainMenu/FigmaCanvas/Main Menu - v4/MENU");
			if (gameObject2 == null)
			{
				return;
			}
			this._layout = gameObject2.GetComponent<VerticalLayoutGroup>();
			this._fitter = gameObject2.GetComponent<ContentSizeFitter>();
			this._layoutWasEnabled = this._layout != null && this._layout.enabled;
			this._fitterWasEnabled = this._fitter != null && this._fitter.enabled;
			for (int i = 0; i < gameObject2.transform.childCount; i++)
			{
				Transform child = gameObject2.transform.GetChild(i);
				if (child.gameObject.activeSelf && !(child.GetComponent<DAButton>() == null))
				{
					RectTransform rectTransform = child as RectTransform;
					if (!(rectTransform == null))
					{
						this._buttons.Add(new MainMenuEntranceAnimator.ButtonState
						{
							rect = rectTransform,
							group = MainMenuEntranceAnimator.GetOrAddCanvasGroup(child.gameObject),
							baseScale = rectTransform.localScale,
							baseRotation = rectTransform.localRotation
						});
					}
				}
			}
		}

		// Token: 0x06007F2D RID: 32557 RVA: 0x0033FA94 File Offset: 0x0033DC94
		private void PrepareHidden()
		{
			this.IsComplete = false;
			this.PruneInvalidButtons();
			if (this._logoGroup != null)
			{
				this._logoGroup.alpha = 0f;
				this._logoGroup.blocksRaycasts = false;
				this._logoGroup.interactable = false;
			}
			for (int i = 0; i < this._buttons.Count; i++)
			{
				CanvasGroup group = this._buttons[i].group;
				if (!(group == null))
				{
					group.alpha = 0f;
					group.blocksRaycasts = false;
					group.interactable = false;
				}
			}
		}

		// Token: 0x06007F2E RID: 32558 RVA: 0x0033FB2E File Offset: 0x0033DD2E
		private IEnumerator AnimateEntrance()
		{
			yield return null;
			Canvas.ForceUpdateCanvases();
			this.PruneInvalidButtons();
			this._buttons.Sort((MainMenuEntranceAnimator.ButtonState left, MainMenuEntranceAnimator.ButtonState right) => left.rect.GetSiblingIndex().CompareTo(right.rect.GetSiblingIndex()));
			if (this._logoRect != null)
			{
				this._logoBasePosition = this._logoRect.anchoredPosition;
				this._logoBaseScale = this._logoRect.localScale;
				this._logoBaseRotation = this._logoRect.localRotation;
				this._logoRect.anchoredPosition = this._logoBasePosition + new Vector2(0f, -28f);
				this._logoRect.localScale = this._logoBaseScale * 0.9f;
				this._logoRect.localRotation = this._logoBaseRotation * Quaternion.Euler(0f, 0f, -1.4f);
			}
			for (int i = 0; i < this._buttons.Count; i++)
			{
				MainMenuEntranceAnimator.ButtonState buttonState = this._buttons[i];
				if (MainMenuEntranceAnimator.IsButtonValid(buttonState))
				{
					buttonState.basePosition = buttonState.rect.anchoredPosition;
					buttonState.baseScale = buttonState.rect.localScale;
					buttonState.baseRotation = buttonState.rect.localRotation;
				}
			}
			this._basePositionsCaptured = true;
			if (this._layout != null)
			{
				this._layout.enabled = false;
			}
			if (this._fitter != null)
			{
				this._fitter.enabled = false;
			}
			for (int j = 0; j < this._buttons.Count; j++)
			{
				MainMenuEntranceAnimator.ButtonState buttonState2 = this._buttons[j];
				if (MainMenuEntranceAnimator.IsButtonValid(buttonState2))
				{
					buttonState2.rect.anchoredPosition = buttonState2.basePosition + new Vector2(-62f, 0f);
					buttonState2.rect.localScale = buttonState2.baseScale * 0.955f;
					buttonState2.rect.localRotation = buttonState2.baseRotation * Quaternion.Euler(0f, 0f, (j % 2 == 0) ? (-0.6f) : 0.6f);
				}
			}
			float totalDuration = Mathf.Max(0.78f, 0.18f + (float)Mathf.Max(0, this._buttons.Count - 1) * 0.085f + 0.46f);
			float elapsed = 0f;
			while (elapsed < totalDuration)
			{
				elapsed += Time.unscaledDeltaTime;
				float num = Mathf.Clamp01(elapsed / 0.78f);
				float num2 = MainMenuEntranceAnimator.EaseOutCubic(num);
				float num3 = MainMenuEntranceAnimator.EaseOutBack(num, 1.18f);
				if (this._logoRect != null)
				{
					this._logoRect.anchoredPosition = Vector2.LerpUnclamped(this._logoBasePosition + new Vector2(0f, -28f), this._logoBasePosition, num2);
					this._logoRect.localScale = Vector3.LerpUnclamped(this._logoBaseScale * 0.9f, this._logoBaseScale, num3);
					this._logoRect.localRotation = Quaternion.Slerp(this._logoBaseRotation * Quaternion.Euler(0f, 0f, -1.4f), this._logoBaseRotation, num2);
				}
				if (this._logoGroup != null)
				{
					this._logoGroup.alpha = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(num / 0.72f));
				}
				this.ApplyButtonAnimation(elapsed);
				yield return null;
			}
			this.RestoreFinalState();
			this.IsComplete = true;
			this._entranceRoutine = null;
			MainMenuLogoShimmer mainMenuLogoShimmer = ((this._logoRect != null) ? this._logoRect.GetComponent<MainMenuLogoShimmer>() : null);
			if (mainMenuLogoShimmer != null)
			{
				mainMenuLogoShimmer.PlayNow();
			}
			Debug.Log("[MainMenuEntrance] PASS logo and button entrance completed.");
			yield break;
		}

		// Token: 0x06007F2F RID: 32559 RVA: 0x0033FB40 File Offset: 0x0033DD40
		private void ApplyButtonAnimation(float elapsed)
		{
			for (int i = 0; i < this._buttons.Count; i++)
			{
				MainMenuEntranceAnimator.ButtonState buttonState = this._buttons[i];
				if (MainMenuEntranceAnimator.IsButtonValid(buttonState))
				{
					float num = Mathf.Clamp01((elapsed - 0.18f - (float)i * 0.085f) / 0.46f);
					float num2 = MainMenuEntranceAnimator.EaseOutCubic(num);
					float num3 = MainMenuEntranceAnimator.EaseOutBack(num, 1.08f);
					buttonState.group.alpha = Mathf.SmoothStep(0f, 1f, num);
					buttonState.rect.anchoredPosition = Vector2.LerpUnclamped(buttonState.basePosition + new Vector2(-62f, 0f), buttonState.basePosition, num2);
					buttonState.rect.localScale = Vector3.LerpUnclamped(buttonState.baseScale * 0.955f, buttonState.baseScale, num3);
					buttonState.rect.localRotation = Quaternion.Slerp(buttonState.baseRotation * Quaternion.Euler(0f, 0f, (i % 2 == 0) ? (-0.6f) : 0.6f), buttonState.baseRotation, num2);
				}
			}
		}

		// Token: 0x06007F30 RID: 32560 RVA: 0x0033FC68 File Offset: 0x0033DE68
		private void RestoreFinalState()
		{
			this.PruneInvalidButtons();
			if (this._logoRect != null)
			{
				if (this._basePositionsCaptured)
				{
					this._logoRect.anchoredPosition = this._logoBasePosition;
				}
				this._logoRect.localScale = this._logoBaseScale;
				this._logoRect.localRotation = this._logoBaseRotation;
			}
			if (this._logoGroup != null)
			{
				this._logoGroup.alpha = 1f;
				this._logoGroup.blocksRaycasts = false;
				this._logoGroup.interactable = false;
			}
			for (int i = 0; i < this._buttons.Count; i++)
			{
				MainMenuEntranceAnimator.ButtonState buttonState = this._buttons[i];
				if (MainMenuEntranceAnimator.IsButtonValid(buttonState))
				{
					if (this._basePositionsCaptured)
					{
						buttonState.rect.anchoredPosition = buttonState.basePosition;
					}
					buttonState.rect.localScale = buttonState.baseScale;
					buttonState.rect.localRotation = buttonState.baseRotation;
					buttonState.group.alpha = 1f;
					buttonState.group.blocksRaycasts = true;
					buttonState.group.interactable = true;
				}
			}
			if (this._layout != null)
			{
				this._layout.enabled = this._layoutWasEnabled;
			}
			if (this._fitter != null)
			{
				this._fitter.enabled = this._fitterWasEnabled;
			}
			Canvas.ForceUpdateCanvases();
		}

		// Token: 0x06007F31 RID: 32561 RVA: 0x0033FDD0 File Offset: 0x0033DFD0
		private void PruneInvalidButtons()
		{
			for (int i = this._buttons.Count - 1; i >= 0; i--)
			{
				if (!MainMenuEntranceAnimator.IsButtonValid(this._buttons[i]))
				{
					this._buttons.RemoveAt(i);
				}
			}
		}

		// Token: 0x06007F32 RID: 32562 RVA: 0x0033FE14 File Offset: 0x0033E014
		private static bool IsButtonValid(MainMenuEntranceAnimator.ButtonState button)
		{
			return button != null && button.rect != null && button.group != null;
		}

		// Token: 0x06007F33 RID: 32563 RVA: 0x0033FE38 File Offset: 0x0033E038
		private static CanvasGroup GetOrAddCanvasGroup(GameObject target)
		{
			CanvasGroup component = target.GetComponent<CanvasGroup>();
			if (!(component != null))
			{
				return target.AddComponent<CanvasGroup>();
			}
			return component;
		}

		// Token: 0x06007F34 RID: 32564 RVA: 0x0033FE60 File Offset: 0x0033E060
		private static float EaseOutCubic(float value)
		{
			float num = 1f - value;
			return 1f - num * num * num;
		}

		// Token: 0x06007F35 RID: 32565 RVA: 0x0033FE80 File Offset: 0x0033E080
		private static float EaseOutBack(float value, float overshoot)
		{
			float num = value - 1f;
			return 1f + (overshoot + 1f) * num * num * num + overshoot * num * num;
		}

		// Token: 0x040082A8 RID: 33448
		private const string MenuRootPath = "MainMenu/FigmaCanvas/Main Menu - v4/MENU";

		// Token: 0x040082A9 RID: 33449
		private const string LogoPath = "MainMenu/FigmaCanvas/Main Menu - v4/Logo/Image";

		// Token: 0x040082AA RID: 33450
		private const float ButtonStart = 0.18f;

		// Token: 0x040082AB RID: 33451
		private const float ButtonStagger = 0.085f;

		// Token: 0x040082AC RID: 33452
		private const float ButtonDuration = 0.46f;

		// Token: 0x040082AD RID: 33453
		private readonly List<MainMenuEntranceAnimator.ButtonState> _buttons = new List<MainMenuEntranceAnimator.ButtonState>();

		// Token: 0x040082AE RID: 33454
		private RectTransform _logoRect;

		// Token: 0x040082AF RID: 33455
		private CanvasGroup _logoGroup;

		// Token: 0x040082B0 RID: 33456
		private Vector2 _logoBasePosition;

		// Token: 0x040082B1 RID: 33457
		private Vector3 _logoBaseScale;

		// Token: 0x040082B2 RID: 33458
		private Quaternion _logoBaseRotation;

		// Token: 0x040082B3 RID: 33459
		private VerticalLayoutGroup _layout;

		// Token: 0x040082B4 RID: 33460
		private ContentSizeFitter _fitter;

		// Token: 0x040082B5 RID: 33461
		private bool _layoutWasEnabled;

		// Token: 0x040082B6 RID: 33462
		private bool _fitterWasEnabled;

		// Token: 0x040082B7 RID: 33463
		private Coroutine _entranceRoutine;

		// Token: 0x040082B8 RID: 33464
		private bool _playRequested;

		// Token: 0x040082B9 RID: 33465
		private bool _basePositionsCaptured;

		// Token: 0x0200105E RID: 4190
		private sealed class ButtonState
		{
			// Token: 0x040082BB RID: 33467
			public RectTransform rect;

			// Token: 0x040082BC RID: 33468
			public CanvasGroup group;

			// Token: 0x040082BD RID: 33469
			public Vector2 basePosition;

			// Token: 0x040082BE RID: 33470
			public Vector3 baseScale;

			// Token: 0x040082BF RID: 33471
			public Quaternion baseRotation;
		}
	}
}
