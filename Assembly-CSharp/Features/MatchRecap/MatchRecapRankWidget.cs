using System;
using System.Collections;
using DA_Assets.CR;
using Features.Localization;
using Features.MainMenu;
using Features.Rank;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.MatchRecap
{
	// Token: 0x02000FD4 RID: 4052
	internal sealed class MatchRecapRankWidget : MonoBehaviour
	{
		// Token: 0x06007A68 RID: 31336 RVA: 0x00320040 File Offset: 0x0031E240
		public static MatchRecapRankWidget Create(Transform parent, TMP_Text styleSource, Image themeSource, AudioClip sound, float soundVolume)
		{
			GameObject gameObject = new GameObject("Rank Progress Widget", new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(Image),
				typeof(CanvasGroup),
				typeof(RectMask2D)
			});
			gameObject.layer = parent.gameObject.layer;
			RectTransform component = gameObject.GetComponent<RectTransform>();
			component.SetParent(parent, false);
			component.anchorMin = (component.anchorMax = (component.pivot = Vector2.one));
			component.sizeDelta = new Vector2(470f, 118f);
			component.anchoredPosition = new Vector2(-42f, -42f);
			component.SetAsLastSibling();
			MatchRecapRankWidget matchRecapRankWidget = gameObject.AddComponent<MatchRecapRankWidget>();
			matchRecapRankWidget.Build(styleSource, themeSource, sound, soundVolume);
			return matchRecapRankWidget;
		}

		// Token: 0x06007A69 RID: 31337 RVA: 0x0032011C File Offset: 0x0031E31C
		private void Build(TMP_Text styleSource, Image themeSource, AudioClip sound, float soundVolume)
		{
			this._rect = (RectTransform)base.transform;
			this._finalPosition = this._rect.anchoredPosition;
			this._group = base.GetComponent<CanvasGroup>();
			this._group.interactable = false;
			this._group.blocksRaycasts = false;
			this._panel = base.GetComponent<Image>();
			this._panel.color = MatchRecapRankWidget.PanelColor;
			this._panel.raycastTarget = false;
			base.gameObject.AddComponent<CornerRounder>().SetRadii(new Vector4(14f, 14f, 14f, 14f));
			Outline outline = base.gameObject.AddComponent<Outline>();
			outline.effectColor = new Color(0.15f, 0.045f, 0.19f, 0.88f);
			outline.effectDistance = new Vector2(3f, -3f);
			if (themeSource != null && themeSource.sprite != null)
			{
				Image image = MatchRecapRankWidget.CreateImage("Match Recap Pattern", this._rect, Color.white);
				image.sprite = themeSource.sprite;
				image.type = Image.Type.Simple;
				image.preserveAspect = true;
				image.color = new Color(1f, 1f, 1f, 0.32f);
				RectTransform rectTransform = image.rectTransform;
				RectTransform rectTransform2 = image.rectTransform;
				RectTransform rectTransform3 = image.rectTransform;
				Vector2 vector = new Vector2(0.5f, 0.5f);
				rectTransform3.pivot = vector;
				rectTransform.anchorMin = (rectTransform2.anchorMax = vector);
				image.rectTransform.anchoredPosition = new Vector2(0f, -55f);
				image.rectTransform.sizeDelta = new Vector2(510f, 510f);
				image.transform.SetAsFirstSibling();
			}
			TMP_FontAsset tmp_FontAsset = ((styleSource != null) ? LocalizedGameText.SourceFont(styleSource) : TMP_Settings.defaultFontAsset);
			this._accent = MatchRecapRankWidget.CreateImage("Accent", this._rect, MatchRecapRankWidget.AccentColor);
			this._accent.rectTransform.anchorMin = Vector2.zero;
			this._accent.rectTransform.anchorMax = new Vector2(0f, 1f);
			this._accent.rectTransform.offsetMin = new Vector2(0f, 9f);
			this._accent.rectTransform.offsetMax = new Vector2(6f, -9f);
			this._accent.gameObject.AddComponent<CornerRounder>().SetRadii(new Vector4(3f, 3f, 3f, 3f));
			this._xpText = MatchRecapRankWidget.CreateText("XP Gained", this._rect, tmp_FontAsset, 29f, FontStyles.Bold, TextAlignmentOptions.Left, Color.white);
			MatchRecapRankWidget.Place(this._xpText.rectTransform, new Vector2(20f, -9f), new Vector2(205f, 40f), new Vector2(0f, 1f));
			this._rankText = MatchRecapRankWidget.CreateText("Rank", this._rect, tmp_FontAsset, 13f, FontStyles.Bold, TextAlignmentOptions.TopRight, new Color(1f, 0.79f, 0.88f, 1f));
			MatchRecapRankWidget.Place(this._rankText.rectTransform, new Vector2(-18f, -14f), new Vector2(245f, 25f), Vector2.one);
			this._levelText = MatchRecapRankWidget.CreateText("Level", this._rect, tmp_FontAsset, 15f, FontStyles.Bold, TextAlignmentOptions.Left, Color.white);
			MatchRecapRankWidget.Place(this._levelText.rectTransform, new Vector2(20f, -53f), new Vector2(110f, 24f), new Vector2(0f, 1f));
			this._levelProgressText = MatchRecapRankWidget.CreateText("Level Progress", this._rect, tmp_FontAsset, 13f, FontStyles.Bold, TextAlignmentOptions.Right, new Color(1f, 1f, 1f, 0.78f));
			MatchRecapRankWidget.Place(this._levelProgressText.rectTransform, new Vector2(-18f, -53f), new Vector2(260f, 24f), Vector2.one);
			Image image2 = MatchRecapRankWidget.CreateImage("Level Track", this._rect, MatchRecapRankWidget.TrackColor);
			RectTransform rectTransform4 = image2.rectTransform;
			rectTransform4.anchorMin = new Vector2(0f, 0f);
			rectTransform4.anchorMax = new Vector2(1f, 0f);
			rectTransform4.offsetMin = new Vector2(20f, 14f);
			rectTransform4.offsetMax = new Vector2(-18f, 27f);
			image2.gameObject.AddComponent<CornerRounder>().SetRadii(new Vector4(7f, 7f, 7f, 7f));
			Image image3 = MatchRecapRankWidget.CreateImage("Fill", rectTransform4, MatchRecapRankWidget.AccentColor);
			this._progressFillRect = image3.rectTransform;
			this._progressFillRect.anchorMin = Vector2.zero;
			this._progressFillRect.anchorMax = new Vector2(0f, 1f);
			this._progressFillRect.offsetMin = Vector2.zero;
			this._progressFillRect.offsetMax = Vector2.zero;
			this._sound = sound;
			this._soundVolume = Mathf.Clamp01(soundVolume);
			this._audio = base.gameObject.AddComponent<AudioSource>();
			this._audio.playOnAwake = false;
			this._audio.loop = false;
			this._audio.spatialBlend = 0f;
			this._audio.ignoreListenerPause = true;
			GameAudioRouter.Route(this._audio, GameAudioRouter.Channel.Sfx);
			this.ResetVisuals();
		}

		// Token: 0x06007A6A RID: 31338 RVA: 0x003206C4 File Offset: 0x0031E8C4
		public void ResetVisuals()
		{
			if (this._group == null)
			{
				return;
			}
			this._group.alpha = 0f;
			this._rect.anchoredPosition = this._finalPosition + Vector2.right * 55f;
			this._rect.localScale = Vector3.one * 0.94f;
			this._rect.localRotation = Quaternion.identity;
			this._panel.color = MatchRecapRankWidget.PanelColor;
			this._accent.color = MatchRecapRankWidget.AccentColor;
			LocalizedGameText.BindFormat(this._xpText, "rank_ui", "+{0} XP", new object[] { 0 });
			this.SetProgress(0f);
		}

		// Token: 0x06007A6B RID: 31339 RVA: 0x0032078F File Offset: 0x0031E98F
		public IEnumerator Reveal(RunOutcome outcome)
		{
			this.ConfigureContent(outcome, false);
			this.ResetVisuals();
			float elapsed = 0f;
			while (elapsed < 0.3f)
			{
				elapsed += Time.unscaledDeltaTime;
				float num = MatchRecapRankWidget.EaseOutCubic(Mathf.Clamp01(elapsed / 0.3f));
				this._group.alpha = num;
				this._rect.anchoredPosition = Vector2.LerpUnclamped(this._finalPosition + Vector2.right * 55f, this._finalPosition, num);
				this._rect.localScale = Vector3.one * Mathf.LerpUnclamped(0.94f, 1f, num);
				yield return null;
			}
			this.PlaySound(0.96f, 0.8f);
			yield return this.CountXp(outcome.xpGained, 0.55f);
			yield return this.AnimateLevelProgress(outcome);
			if (outcome.rankChanged)
			{
				yield return this.CelebrateRankUp(outcome.newRankIndex);
			}
			else
			{
				this.SetRankLabel(outcome.newRankIndex, false);
			}
			LocalizedGameText.BindFormat(this._xpText, "rank_ui", "+{0} XP", new object[] { Mathf.RoundToInt(outcome.xpGained) });
			this._rect.localScale = Vector3.one;
			this._rect.localRotation = Quaternion.identity;
			this._panel.color = MatchRecapRankWidget.PanelColor;
			this._accent.color = MatchRecapRankWidget.AccentColor;
			yield break;
		}

		// Token: 0x06007A6C RID: 31340 RVA: 0x003207A8 File Offset: 0x0031E9A8
		public void ShowImmediate(RunOutcome outcome)
		{
			this.ConfigureContent(outcome, true);
			this._group.alpha = 1f;
			this._rect.anchoredPosition = this._finalPosition;
			this._rect.localScale = Vector3.one;
			this._rect.localRotation = Quaternion.identity;
			this._panel.color = MatchRecapRankWidget.PanelColor;
			this._accent.color = MatchRecapRankWidget.AccentColor;
			LocalizedGameText.BindFormat(this._xpText, "rank_ui", "+{0} XP", new object[] { Mathf.RoundToInt(outcome.xpGained) });
			float num = MatchRecapRankWidget.CurrentLevelCost(outcome.newLevel);
			this.SetProgress((num > 0f) ? (outcome.totalXpAfter / num) : 0f);
			this.UpdateLevelProgressText(outcome.newLevel, outcome.totalXpAfter, num);
		}

		// Token: 0x06007A6D RID: 31341 RVA: 0x00320889 File Offset: 0x0031EA89
		private IEnumerator CountXp(float target, float duration)
		{
			float elapsed = 0f;
			while (elapsed < duration)
			{
				elapsed += Time.unscaledDeltaTime;
				float num = Mathf.Clamp01(elapsed / duration);
				float num2 = MatchRecapRankWidget.EaseOutCubic(num);
				LocalizedGameText.BindFormat(this._xpText, "rank_ui", "+{0} XP", new object[] { Mathf.RoundToInt(target * num2) });
				this.PlayCountTick(num, 0.96f + num * 0.15f);
				yield return null;
			}
			LocalizedGameText.BindFormat(this._xpText, "rank_ui", "+{0} XP", new object[] { Mathf.RoundToInt(target) });
			this.PlaySound(1.12f, 0.9f);
			yield break;
		}

		// Token: 0x06007A6E RID: 31342 RVA: 0x003208A6 File Offset: 0x0031EAA6
		private IEnumerator AnimateLevelProgress(RunOutcome outcome)
		{
			int num = Mathf.Max(0, outcome.previousLevel);
			int newLevel = Mathf.Max(num, outcome.newLevel);
			float num2 = MatchRecapRankWidget.CurrentLevelCost(num);
			float num3 = ((num2 > 0f) ? (outcome.previousXp / num2) : 0f);
			int firstAnimatedLevel = num;
			if (newLevel - num > 3)
			{
				firstAnimatedLevel = newLevel - 3;
				num3 = 0f;
			}
			LocalizedGameText.BindFormat(this._levelText, "rank_ui", "LV {0}", new object[] { firstAnimatedLevel });
			this.SetProgress(num3);
			int num4;
			for (int level = firstAnimatedLevel; level < newLevel; level = num4 + 1)
			{
				yield return this.AnimateProgress(num3, 1f, 0.24f);
				this.PlaySound(1.22f + (float)(level - firstAnimatedLevel) * 0.05f, 1f);
				yield return this.PunchLevel(level + 1);
				num3 = 0f;
				this.SetProgress(0f);
				num4 = level;
			}
			float finalCost = MatchRecapRankWidget.CurrentLevelCost(newLevel);
			float num5 = ((finalCost > 0f) ? (outcome.totalXpAfter / finalCost) : 0f);
			LocalizedGameText.BindFormat(this._levelText, "rank_ui", "LV {0}", new object[] { newLevel });
			yield return this.AnimateProgress(num3, num5, 0.38f);
			this.UpdateLevelProgressText(newLevel, outcome.totalXpAfter, finalCost);
			yield break;
		}

		// Token: 0x06007A6F RID: 31343 RVA: 0x003208BC File Offset: 0x0031EABC
		private IEnumerator PunchLevel(int level)
		{
			LocalizedGameText.BindFormat(this._levelText, "rank_ui", "LV {0}", new object[] { level });
			float elapsed = 0f;
			while (elapsed < 0.32f)
			{
				elapsed += Time.unscaledDeltaTime;
				float num = Mathf.Sin(Mathf.Clamp01(elapsed / 0.32f) * 3.1415927f);
				this._levelText.rectTransform.localScale = Vector3.one * (1f + num * 0.27f);
				this._levelText.color = Color.Lerp(Color.white, MatchRecapRankWidget.CelebrationColor, num);
				this._accent.color = Color.Lerp(MatchRecapRankWidget.AccentColor, MatchRecapRankWidget.CelebrationColor, num);
				yield return null;
			}
			this._levelText.rectTransform.localScale = Vector3.one;
			this._levelText.color = Color.white;
			this._accent.color = MatchRecapRankWidget.AccentColor;
			yield break;
		}

		// Token: 0x06007A70 RID: 31344 RVA: 0x003208D2 File Offset: 0x0031EAD2
		private IEnumerator CelebrateRankUp(int rankIndex)
		{
			this.SetRankLabel(rankIndex, true);
			this.PlaySound(1.38f, 1f);
			float elapsed = 0f;
			while (elapsed < 0.55f)
			{
				elapsed += Time.unscaledDeltaTime;
				float num = Mathf.Clamp01(elapsed / 0.55f);
				float num2 = Mathf.Sin(num * 3.1415927f * 2f) * (1f - num);
				this._rankText.rectTransform.localScale = Vector3.one * (1f + Mathf.Abs(num2) * 0.16f);
				this._rect.localRotation = Quaternion.Euler(0f, 0f, num2 * 1.25f);
				yield return null;
			}
			this._rankText.rectTransform.localScale = Vector3.one;
			this._rect.localRotation = Quaternion.identity;
			yield break;
		}

		// Token: 0x06007A71 RID: 31345 RVA: 0x003208E8 File Offset: 0x0031EAE8
		private IEnumerator AnimateProgress(float from, float to, float duration)
		{
			float elapsed = 0f;
			while (elapsed < duration)
			{
				elapsed += Time.unscaledDeltaTime;
				float num = MatchRecapRankWidget.EaseOutCubic(Mathf.Clamp01(elapsed / duration));
				this.SetProgress(Mathf.Lerp(from, to, num));
				yield return null;
			}
			this.SetProgress(to);
			yield break;
		}

		// Token: 0x06007A72 RID: 31346 RVA: 0x0032090C File Offset: 0x0031EB0C
		private void ConfigureContent(RunOutcome outcome, bool finalState)
		{
			int num = (finalState ? outcome.newLevel : outcome.previousLevel);
			float num2 = (finalState ? outcome.totalXpAfter : outcome.previousXp);
			float num3 = MatchRecapRankWidget.CurrentLevelCost(num);
			this.SetRankLabel(outcome.newRankIndex, finalState && outcome.rankChanged);
			LocalizedGameText.BindFormat(this._levelText, "rank_ui", "LV {0}", new object[] { Mathf.Max(0, num) });
			this.UpdateLevelProgressText(num, num2, num3);
		}

		// Token: 0x06007A73 RID: 31347 RVA: 0x00320990 File Offset: 0x0031EB90
		private static string BuildRankText(int rankIndex)
		{
			string text = MatchRecapRankWidget.ResolveRankName(rankIndex).ToUpperInvariant();
			RankProgressionConfigSO config = RankProgressionService.Config;
			if (config != null && config.ranksInOrder != null && rankIndex + 1 >= 0 && rankIndex + 1 < config.ranksInOrder.Count && config.ranksInOrder[rankIndex + 1] != null)
			{
				string source = GameLocalization.GetSource("rank", config.ranksInOrder[rankIndex + 1].displayName);
				return text + "  >  " + source.ToUpperInvariant();
			}
			return GameLocalization.FormatSource("rank_ui", "{0}  -  MAX", new object[] { text });
		}

		// Token: 0x06007A74 RID: 31348 RVA: 0x00320A38 File Offset: 0x0031EC38
		private static string ResolveRankName(int rankIndex)
		{
			RankProgressionConfigSO config = RankProgressionService.Config;
			if (config != null && config.ranksInOrder != null && rankIndex >= 0 && rankIndex < config.ranksInOrder.Count && config.ranksInOrder[rankIndex] != null && !string.IsNullOrWhiteSpace(config.ranksInOrder[rankIndex].displayName))
			{
				return GameLocalization.GetSource("rank", config.ranksInOrder[rankIndex].displayName);
			}
			return GameLocalization.FormatSource("rank_ui", "RANK {0}", new object[] { rankIndex });
		}

		// Token: 0x06007A75 RID: 31349 RVA: 0x00320AD4 File Offset: 0x0031ECD4
		private void SetRankLabel(int rankIndex, bool promoted)
		{
			LocalizedGameText.BindComputed(this._rankText, delegate
			{
				if (!promoted)
				{
					return MatchRecapRankWidget.BuildRankText(rankIndex);
				}
				return GameLocalization.FormatSource("rank_ui", "RANK UP  -  {0}", new object[] { MatchRecapRankWidget.ResolveRankName(rankIndex).ToUpperInvariant() });
			});
		}

		// Token: 0x06007A76 RID: 31350 RVA: 0x00320B10 File Offset: 0x0031ED10
		private static float CurrentLevelCost(int level)
		{
			RankProgressionConfigSO config = RankProgressionService.Config;
			if (!(config != null))
			{
				return 1f;
			}
			return config.GetLevelCost(Mathf.Max(0, level));
		}

		// Token: 0x06007A77 RID: 31351 RVA: 0x00320B40 File Offset: 0x0031ED40
		private void UpdateLevelProgressText(int level, float xp, float cost)
		{
			float num = Mathf.Max(0f, Mathf.Max(1f, cost) - Mathf.Max(0f, xp));
			LocalizedGameText.BindFormat(this._levelProgressText, "rank_ui", "{0} XP TO LVL {1}", new object[]
			{
				Mathf.CeilToInt(num),
				Mathf.Max(0, level) + 1
			});
		}

		// Token: 0x06007A78 RID: 31352 RVA: 0x00320BAC File Offset: 0x0031EDAC
		private void SetProgress(float value)
		{
			if (this._progressFillRect == null)
			{
				return;
			}
			Vector2 anchorMax = this._progressFillRect.anchorMax;
			anchorMax.x = Mathf.Clamp01(value);
			this._progressFillRect.anchorMax = anchorMax;
		}

		// Token: 0x06007A79 RID: 31353 RVA: 0x00320BED File Offset: 0x0031EDED
		private void PlayCountTick(float progress, float pitch)
		{
			if (Time.unscaledTime < this._nextTickTime)
			{
				return;
			}
			this._nextTickTime = Time.unscaledTime + Mathf.Lerp(0.085f, 0.045f, progress);
			this.PlaySound(pitch, 0.28f);
		}

		// Token: 0x06007A7A RID: 31354 RVA: 0x00320C28 File Offset: 0x0031EE28
		private void PlaySound(float pitch, float volumeScale)
		{
			if (this._audio == null || this._sound == null)
			{
				return;
			}
			this._audio.pitch = Mathf.Clamp(pitch, 0.5f, 2f);
			this._audio.PlayOneShot(this._sound, Mathf.Clamp01(this._soundVolume * volumeScale));
		}

		// Token: 0x06007A7B RID: 31355 RVA: 0x00320C8C File Offset: 0x0031EE8C
		private static Image CreateImage(string name, RectTransform parent, Color color)
		{
			GameObject gameObject = new GameObject(name, new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(Image)
			});
			gameObject.layer = parent.gameObject.layer;
			gameObject.GetComponent<RectTransform>().SetParent(parent, false);
			Image component = gameObject.GetComponent<Image>();
			component.color = color;
			component.raycastTarget = false;
			return component;
		}

		// Token: 0x06007A7C RID: 31356 RVA: 0x00320D00 File Offset: 0x0031EF00
		private static TMP_Text CreateText(string name, RectTransform parent, TMP_FontAsset font, float fontSize, FontStyles style, TextAlignmentOptions alignment, Color color)
		{
			GameObject gameObject = new GameObject(name, new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(TextMeshProUGUI)
			});
			gameObject.layer = parent.gameObject.layer;
			gameObject.GetComponent<RectTransform>().SetParent(parent, false);
			TextMeshProUGUI component = gameObject.GetComponent<TextMeshProUGUI>();
			component.font = font;
			component.fontSize = fontSize;
			component.fontStyle = style;
			component.alignment = alignment;
			component.color = color;
			component.enableWordWrapping = false;
			component.overflowMode = TextOverflowModes.Ellipsis;
			component.raycastTarget = false;
			LocalizedGameText.BindFontOnly(component).FitTranslationToRectangle(false);
			return component;
		}

		// Token: 0x06007A7D RID: 31357 RVA: 0x00320DAC File Offset: 0x0031EFAC
		private static void Place(RectTransform rect, Vector2 position, Vector2 size, Vector2 pivot)
		{
			rect.anchorMax = pivot;
			rect.anchorMin = pivot;
			rect.pivot = pivot;
			rect.anchoredPosition = position;
			rect.sizeDelta = size;
		}

		// Token: 0x06007A7E RID: 31358 RVA: 0x00320DE0 File Offset: 0x0031EFE0
		private static float EaseOutCubic(float t)
		{
			float num = 1f - t;
			return 1f - num * num * num;
		}

		// Token: 0x04007DB6 RID: 32182
		private static readonly Color PanelColor = new Color(0.43f, 0.2f, 0.51f, 0.98f);

		// Token: 0x04007DB7 RID: 32183
		private static readonly Color AccentColor = new Color(0.96f, 0.18f, 0.43f, 1f);

		// Token: 0x04007DB8 RID: 32184
		private static readonly Color CelebrationColor = new Color(1f, 0.77f, 0.25f, 1f);

		// Token: 0x04007DB9 RID: 32185
		private static readonly Color TrackColor = new Color(0.16f, 0.055f, 0.24f, 0.78f);

		// Token: 0x04007DBA RID: 32186
		private RectTransform _rect;

		// Token: 0x04007DBB RID: 32187
		private CanvasGroup _group;

		// Token: 0x04007DBC RID: 32188
		private Image _panel;

		// Token: 0x04007DBD RID: 32189
		private Image _accent;

		// Token: 0x04007DBE RID: 32190
		private TMP_Text _rankText;

		// Token: 0x04007DBF RID: 32191
		private TMP_Text _xpText;

		// Token: 0x04007DC0 RID: 32192
		private TMP_Text _levelText;

		// Token: 0x04007DC1 RID: 32193
		private TMP_Text _levelProgressText;

		// Token: 0x04007DC2 RID: 32194
		private RectTransform _progressFillRect;

		// Token: 0x04007DC3 RID: 32195
		private AudioSource _audio;

		// Token: 0x04007DC4 RID: 32196
		private AudioClip _sound;

		// Token: 0x04007DC5 RID: 32197
		private float _soundVolume;

		// Token: 0x04007DC6 RID: 32198
		private Vector2 _finalPosition;

		// Token: 0x04007DC7 RID: 32199
		private float _nextTickTime;
	}
}
