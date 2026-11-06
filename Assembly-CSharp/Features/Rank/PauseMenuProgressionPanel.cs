using System;
using DA_Assets.CR;
using Features.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Rank
{
	// Token: 0x02000D9E RID: 3486
	[DisallowMultipleComponent]
	public sealed class PauseMenuProgressionPanel : MonoBehaviour
	{
		// Token: 0x060066A7 RID: 26279 RVA: 0x002905CA File Offset: 0x0028E7CA
		public void Initialize(TMP_FontAsset font)
		{
			this._font = font;
			this.Build();
			this.Refresh();
		}

		// Token: 0x060066A8 RID: 26280 RVA: 0x002905DF File Offset: 0x0028E7DF
		private void OnEnable()
		{
			RankProgressionService.Changed += this.Refresh;
			if (this._built)
			{
				this.Refresh();
			}
		}

		// Token: 0x060066A9 RID: 26281 RVA: 0x00290600 File Offset: 0x0028E800
		private void OnDisable()
		{
			RankProgressionService.Changed -= this.Refresh;
		}

		// Token: 0x060066AA RID: 26282 RVA: 0x00290614 File Offset: 0x0028E814
		private void Build()
		{
			if (this._built)
			{
				return;
			}
			RectTransform rectTransform = base.transform as RectTransform;
			if (rectTransform == null)
			{
				return;
			}
			this._built = true;
			Image image = base.gameObject.AddComponent<Image>();
			image.color = PauseMenuProgressionPanel.Surface;
			image.raycastTarget = false;
			PauseMenuProgressionPanel.Round(base.gameObject, 14f);
			Image image2 = PauseMenuProgressionPanel.CreateImage("Accent", rectTransform, PauseMenuProgressionPanel.Pink);
			PauseMenuProgressionPanel.SetRect(image2.rectTransform, Vector2.zero, new Vector2(0f, 1f), new Vector2(0f, 10f), new Vector2(6f, -10f));
			PauseMenuProgressionPanel.Round(image2.gameObject, 3f);
			this._level = this.CreateText("Level", rectTransform, "LV 0", 27f, FontStyles.Bold, PauseMenuProgressionPanel.White, TextAlignmentOptions.MidlineLeft);
			PauseMenuProgressionPanel.SetRect(this._level.rectTransform, new Vector2(0f, 0.7f), new Vector2(0.42f, 1f), new Vector2(22f, 0f), Vector2.zero);
			this._rank = this.CreateText("Rank", rectTransform, string.Empty, 14f, FontStyles.Bold, PauseMenuProgressionPanel.Pink, TextAlignmentOptions.MidlineRight);
			this._rank.enableAutoSizing = true;
			this._rank.fontSizeMin = 10f;
			this._rank.fontSizeMax = 14f;
			this._rank.enableWordWrapping = false;
			PauseMenuProgressionPanel.SetRect(this._rank.rectTransform, new Vector2(0.38f, 0.72f), Vector2.one, Vector2.zero, new Vector2(-18f, 0f));
			this._xp = this.CreateText("XP Progress", rectTransform, "0 / 0 XP", 13f, FontStyles.Bold, PauseMenuProgressionPanel.Muted, TextAlignmentOptions.MidlineRight);
			PauseMenuProgressionPanel.SetRect(this._xp.rectTransform, new Vector2(0f, 0.52f), new Vector2(1f, 0.71f), new Vector2(22f, 0f), new Vector2(-18f, 0f));
			Image image3 = PauseMenuProgressionPanel.CreateImage("XP Track", rectTransform, PauseMenuProgressionPanel.Track);
			PauseMenuProgressionPanel.SetRect(image3.rectTransform, new Vector2(0f, 0f), new Vector2(1f, 0f), new Vector2(22f, 79f), new Vector2(-18f, 87f));
			PauseMenuProgressionPanel.Round(image3.gameObject, 4f);
			this._fill = PauseMenuProgressionPanel.CreateImage("Fill", image3.rectTransform, PauseMenuProgressionPanel.Pink);
			this._fill.rectTransform.anchorMin = Vector2.zero;
			this._fill.rectTransform.anchorMax = Vector2.one;
			this._fill.rectTransform.offsetMin = Vector2.zero;
			this._fill.rectTransform.offsetMax = Vector2.zero;
			PauseMenuProgressionPanel.Round(this._fill.gameObject, 4f);
			RectTransform rectTransform2 = PauseMenuProgressionPanel.CreateRect("Next Rank Distance", rectTransform);
			this._distanceGroup = rectTransform2.gameObject;
			PauseMenuProgressionPanel.SetRect(rectTransform2, Vector2.zero, new Vector2(1f, 0f), new Vector2(22f, 11f), new Vector2(-18f, 72f));
			this._distanceTitle = this.CreateText("Distance Title", rectTransform2, string.Empty, 12f, FontStyles.Bold, PauseMenuProgressionPanel.Pink, TextAlignmentOptions.MidlineLeft);
			this._distanceTitle.enableAutoSizing = true;
			this._distanceTitle.fontSizeMin = 9f;
			this._distanceTitle.fontSizeMax = 12f;
			this._distanceTitle.enableWordWrapping = false;
			PauseMenuProgressionPanel.SetRect(this._distanceTitle.rectTransform, new Vector2(0f, 0.66f), Vector2.one, Vector2.zero, Vector2.zero);
			this._distanceProgress = this.CreateText("Distance Progress", rectTransform2, string.Empty, 12f, FontStyles.Bold, PauseMenuProgressionPanel.Muted, TextAlignmentOptions.MidlineRight);
			this._distanceProgress.enableAutoSizing = true;
			this._distanceProgress.fontSizeMin = 9f;
			this._distanceProgress.fontSizeMax = 12f;
			this._distanceProgress.enableWordWrapping = false;
			PauseMenuProgressionPanel.SetRect(this._distanceProgress.rectTransform, new Vector2(0f, 0.29f), new Vector2(1f, 0.66f), Vector2.zero, Vector2.zero);
			Image image4 = PauseMenuProgressionPanel.CreateImage("Distance Track", rectTransform2, PauseMenuProgressionPanel.Track);
			PauseMenuProgressionPanel.SetRect(image4.rectTransform, Vector2.zero, new Vector2(1f, 0f), Vector2.zero, new Vector2(0f, 8f));
			PauseMenuProgressionPanel.Round(image4.gameObject, 4f);
			this._distanceFill = PauseMenuProgressionPanel.CreateImage("Distance Fill", image4.rectTransform, PauseMenuProgressionPanel.Pink);
			PauseMenuProgressionPanel.SetRect(this._distanceFill.rectTransform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
			PauseMenuProgressionPanel.Round(this._distanceFill.gameObject, 4f);
		}

		// Token: 0x060066AB RID: 26283 RVA: 0x00290B4C File Offset: 0x0028ED4C
		private void Refresh()
		{
			if (!this._built)
			{
				return;
			}
			int level = RankProgressionService.Level;
			float num = Mathf.Max(0f, RankProgressionService.Xp);
			RankProgressionConfigSO config = RankProgressionService.Config;
			float num2 = ((config != null) ? config.GetLevelCost(level) : 1f);
			RankDefinitionSO currentRank = RankProgressionService.CurrentRank;
			LocalizedGameText.BindFormat(this._level, "rank_ui", "LV {0}", new object[] { level });
			if (currentRank != null && !string.IsNullOrWhiteSpace(currentRank.displayName))
			{
				LocalizedGameText.BindSource(this._rank, "rank", currentRank.displayName, (string value) => value.ToUpperInvariant());
			}
			else
			{
				LocalizedGameText.BindSource(this._rank, "settings", "UNRANKED", null);
			}
			LocalizedGameText.BindFormat(this._xp, "rank_ui", "{0} / {1} XP", new object[]
			{
				Mathf.FloorToInt(num).ToString("N0"),
				Mathf.CeilToInt(num2).ToString("N0")
			});
			RectTransform rectTransform = this._fill.rectTransform;
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = new Vector2(Mathf.Clamp01(num / Mathf.Max(1f, num2)), 1f);
			rectTransform.offsetMin = Vector2.zero;
			rectTransform.offsetMax = Vector2.zero;
			RankDefinitionSO rankDefinitionSO = ((config != null && config.ranksInOrder != null && RankProgressionService.RankIndex + 1 < config.ranksInOrder.Count) ? config.ranksInOrder[RankProgressionService.RankIndex + 1] : null);
			bool flag = rankDefinitionSO != null && rankDefinitionSO.promotionRunDistance > 0f;
			this._distanceGroup.SetActive(flag);
			if (!flag)
			{
				return;
			}
			float num3 = Mathf.Max(0f, RankProgressionService.BestQualifyingDistance);
			RankRuntimeController rankRuntimeController = RankRuntimeController.Current;
			float num4 = ((rankRuntimeController != null && rankRuntimeController.HasPendingQualifyingDistance && rankRuntimeController.Tracker != null) ? Mathf.Max(0f, rankRuntimeController.Tracker.PeakDistance) : 0f);
			bool flag2 = num4 > num3;
			float num5 = Mathf.Max(num3, num4);
			float promotionRunDistance = rankDefinitionSO.promotionRunDistance;
			LocalizedGameText.BindSource(this._distanceTitle, "rank_ui", flag2 ? "NEXT RANK · RUN PEAK (PENDING)" : "NEXT RANK · BEST RUN PEAK", null);
			LocalizedGameText.BindFormat(this._distanceProgress, "rank_ui", "{0} / {1} m · {2} m to go", new object[]
			{
				Mathf.FloorToInt(num5).ToString("N0"),
				Mathf.CeilToInt(promotionRunDistance).ToString("N0"),
				Mathf.CeilToInt(Mathf.Max(0f, promotionRunDistance - num5)).ToString("N0")
			});
			RectTransform rectTransform2 = this._distanceFill.rectTransform;
			rectTransform2.anchorMin = Vector2.zero;
			rectTransform2.anchorMax = new Vector2(Mathf.Clamp01(num5 / promotionRunDistance), 1f);
			rectTransform2.offsetMin = Vector2.zero;
			rectTransform2.offsetMax = Vector2.zero;
		}

		// Token: 0x060066AC RID: 26284 RVA: 0x00290E7C File Offset: 0x0028F07C
		private TextMeshProUGUI CreateText(string name, Transform parent, string value, float size, FontStyles style, Color color, TextAlignmentOptions alignment)
		{
			TextMeshProUGUI textMeshProUGUI = PauseMenuProgressionPanel.CreateRect(name, parent).gameObject.AddComponent<TextMeshProUGUI>();
			textMeshProUGUI.text = value;
			textMeshProUGUI.fontSize = size;
			textMeshProUGUI.fontStyle = style;
			textMeshProUGUI.color = color;
			textMeshProUGUI.alignment = alignment;
			textMeshProUGUI.raycastTarget = false;
			if (this._font != null)
			{
				textMeshProUGUI.font = this._font;
			}
			return textMeshProUGUI;
		}

		// Token: 0x060066AD RID: 26285 RVA: 0x00290EE4 File Offset: 0x0028F0E4
		private static Image CreateImage(string name, Transform parent, Color color)
		{
			Image image = PauseMenuProgressionPanel.CreateRect(name, parent).gameObject.AddComponent<Image>();
			image.color = color;
			image.raycastTarget = false;
			return image;
		}

		// Token: 0x060066AE RID: 26286 RVA: 0x00290F05 File Offset: 0x0028F105
		private static RectTransform CreateRect(string name, Transform parent)
		{
			RectTransform component = new GameObject(name, new Type[] { typeof(RectTransform) }).GetComponent<RectTransform>();
			component.SetParent(parent, false);
			return component;
		}

		// Token: 0x060066AF RID: 26287 RVA: 0x0018F85B File Offset: 0x0018DA5B
		private static void SetRect(RectTransform rect, Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin, Vector2 offsetMax)
		{
			rect.anchorMin = anchorMin;
			rect.anchorMax = anchorMax;
			rect.offsetMin = offsetMin;
			rect.offsetMax = offsetMax;
		}

		// Token: 0x060066B0 RID: 26288 RVA: 0x001A3764 File Offset: 0x001A1964
		private static void Round(GameObject target, float radius)
		{
			target.AddComponent<CornerRounder>().SetRadii(new Vector4(radius, radius, radius, radius));
		}

		// Token: 0x0400691F RID: 26911
		private static readonly Color Pink = new Color(1f, 0.31f, 0.61f, 1f);

		// Token: 0x04006920 RID: 26912
		private static readonly Color White = new Color(1f, 0.97f, 1f, 1f);

		// Token: 0x04006921 RID: 26913
		private static readonly Color Muted = new Color(0.78f, 0.67f, 0.76f, 1f);

		// Token: 0x04006922 RID: 26914
		private static readonly Color Surface = new Color(0.165f, 0.118f, 0.2f, 0.94f);

		// Token: 0x04006923 RID: 26915
		private static readonly Color Track = new Color(0.035f, 0.015f, 0.035f, 1f);

		// Token: 0x04006924 RID: 26916
		private TMP_FontAsset _font;

		// Token: 0x04006925 RID: 26917
		private TextMeshProUGUI _level;

		// Token: 0x04006926 RID: 26918
		private TextMeshProUGUI _rank;

		// Token: 0x04006927 RID: 26919
		private TextMeshProUGUI _xp;

		// Token: 0x04006928 RID: 26920
		private Image _fill;

		// Token: 0x04006929 RID: 26921
		private GameObject _distanceGroup;

		// Token: 0x0400692A RID: 26922
		private TextMeshProUGUI _distanceTitle;

		// Token: 0x0400692B RID: 26923
		private TextMeshProUGUI _distanceProgress;

		// Token: 0x0400692C RID: 26924
		private Image _distanceFill;

		// Token: 0x0400692D RID: 26925
		private bool _built;
	}
}
