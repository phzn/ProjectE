using System;
using System.Collections.Generic;
using System.Globalization;
using Features.CielOS.UI;
using Features.CielOS.WindowManager;
using Features.Localization;
using Features.Missions;
using Features.Rank;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.CielOS.Apps.Rank
{
	// Token: 0x0200145F RID: 5215
	public class CielOSRankApp : MonoBehaviour
	{
		// Token: 0x0600A0F0 RID: 41200 RVA: 0x0042A1AC File Offset: 0x004283AC
		private void Awake()
		{
			foreach (TMP_Text tmp_Text in base.GetComponentsInChildren<TMP_Text>(true))
			{
				LocalizedGameText.BindSource(tmp_Text, "rank_ui", tmp_Text.text, null).FitTranslationToRectangle(false);
			}
			this.HidePrefabTemplates();
			if (this.rankPanel != null)
			{
				CielOSRankApp.PrioritizePromotion(this.rankPanel.transform);
				(this.rankPanel.GetComponent<CielOSRankResponsiveLayout>() ?? this.rankPanel.AddComponent<CielOSRankResponsiveLayout>()).Initialize();
			}
			this.HookTabs();
			if (this.missionsListParent != null)
			{
				ScrollRect componentInParent = this.missionsListParent.GetComponentInParent<ScrollRect>(true);
				if (componentInParent != null)
				{
					(componentInParent.GetComponent<CielOSSmoothWheelScroll>() ?? componentInParent.gameObject.AddComponent<CielOSSmoothWheelScroll>()).Bind(componentInParent);
				}
			}
		}

		// Token: 0x0600A0F1 RID: 41201 RVA: 0x0042A274 File Offset: 0x00428474
		private static void PrioritizePromotion(Transform panel)
		{
			Transform transform = panel.Find("Block_Perks");
			Transform transform2 = panel.Find("Block_Promotion");
			Transform transform3 = panel.Find("Block_MaxRank");
			if (transform == null || transform2 == null || transform2.GetSiblingIndex() < transform.GetSiblingIndex())
			{
				return;
			}
			int siblingIndex = transform.GetSiblingIndex();
			transform2.SetSiblingIndex(siblingIndex);
			if (transform3 != null && transform3.GetSiblingIndex() > transform.GetSiblingIndex())
			{
				transform3.SetSiblingIndex(siblingIndex + 1);
			}
		}

		// Token: 0x0600A0F2 RID: 41202 RVA: 0x0042A2F4 File Offset: 0x004284F4
		private void Start()
		{
			this.MaximizeWindow();
			this.PopulateMissions();
			this.SetTab(CielOSRankApp.Tab.Rank);
		}

		// Token: 0x0600A0F3 RID: 41203 RVA: 0x0042A30C File Offset: 0x0042850C
		private void OnEnable()
		{
			if (!this._subscribed)
			{
				RankProgressionService.Changed += this.Refresh;
				GameLocalization.LocaleChanged += this.Refresh;
				MissionService.Changed += this.RefreshMissions;
				this._subscribed = true;
			}
			MissionService.EnsureAccountMissions();
			this.Refresh();
			this.RefreshMissions();
		}

		// Token: 0x0600A0F4 RID: 41204 RVA: 0x0042A36C File Offset: 0x0042856C
		private void OnDisable()
		{
			if (this._subscribed)
			{
				RankProgressionService.Changed -= this.Refresh;
				GameLocalization.LocaleChanged -= this.Refresh;
				MissionService.Changed -= this.RefreshMissions;
				this._subscribed = false;
			}
		}

		// Token: 0x0600A0F5 RID: 41205 RVA: 0x0042A3BC File Offset: 0x004285BC
		private void HidePrefabTemplates()
		{
			if (this.rankRowTemplate != null)
			{
				this.rankRowTemplate.SetActive(false);
			}
			if (this.missionCardTemplate != null)
			{
				this.missionCardTemplate.SetActive(false);
			}
			if (this.missionSectionTemplate != null)
			{
				this.missionSectionTemplate.SetActive(false);
			}
			if (this.perksCurrentRowTemplate != null)
			{
				this.perksCurrentRowTemplate.SetActive(false);
			}
			if (this.perksNextRowTemplate != null)
			{
				this.perksNextRowTemplate.SetActive(false);
			}
		}

		// Token: 0x0600A0F6 RID: 41206 RVA: 0x0042A44C File Offset: 0x0042864C
		private void HookTabs()
		{
			if (this.rankTabButton != null)
			{
				this.rankTabButton.onClick.RemoveAllListeners();
				this.rankTabButton.onClick.AddListener(delegate
				{
					this.SetTab(CielOSRankApp.Tab.Rank);
				});
			}
			if (this.missionsTabButton != null)
			{
				this.missionsTabButton.onClick.RemoveAllListeners();
				this.missionsTabButton.onClick.AddListener(delegate
				{
					this.SetTab(CielOSRankApp.Tab.Missions);
				});
			}
		}

		// Token: 0x0600A0F7 RID: 41207 RVA: 0x0042A4D0 File Offset: 0x004286D0
		private void SetTab(CielOSRankApp.Tab tab)
		{
			bool flag = tab == CielOSRankApp.Tab.Rank;
			if (this.rankPanel != null)
			{
				this.rankPanel.SetActive(flag);
			}
			if (this.missionsPanel != null)
			{
				this.missionsPanel.SetActive(!flag);
			}
			if (this.rankTabUnderline != null)
			{
				this.rankTabUnderline.SetActive(flag);
			}
			if (this.missionsTabUnderline != null)
			{
				this.missionsTabUnderline.SetActive(!flag);
			}
			if (this.missionsEmptyState != null)
			{
				this.missionsEmptyState.SetActive(false);
			}
		}

		// Token: 0x0600A0F8 RID: 41208 RVA: 0x0042A56C File Offset: 0x0042876C
		private void Refresh()
		{
			if (this == null || !base.isActiveAndEnabled)
			{
				return;
			}
			RankProgressionConfigSO config = RankProgressionService.Config;
			RankDefinitionSO currentRank = RankProgressionService.CurrentRank;
			int rankIndex = RankProgressionService.RankIndex;
			int level = RankProgressionService.Level;
			float xp = RankProgressionService.Xp;
			if (currentRank != null)
			{
				if (this.currentRankLabel != null)
				{
					LocalizedGameText.BindSource(this.currentRankLabel, "rank", currentRank.displayName, (string value) => value.ToUpperInvariant());
					this.currentRankLabel.color = currentRank.badgeColor;
				}
				LocalizedGameText.BindSource(this.taglineLabel, "rank_ui", currentRank.tagline, null);
			}
			LocalizedGameText.BindFormat(this.levelLabel, "rank_ui", "LEVEL {0}", new object[] { level });
			if (this.totalXpLabel != null)
			{
				this.totalXpLabel.text = CielOSRankApp.N0(RankProgressionService.TotalXpEarned);
				LocalizedGameText.BindFontOnly(this.totalXpLabel);
			}
			float num = ((config != null) ? config.GetLevelCost(level) : 0f);
			float num2 = ((num > 0f) ? Mathf.Clamp01(xp / num) : 0f);
			CielOSRankApp.SetFill(this.levelProgressFill, num2);
			LocalizedGameText.BindFormat(this.levelProgressText, "rank_ui", "{0} / {1} XP", new object[]
			{
				CielOSRankApp.N0(xp),
				CielOSRankApp.N0(num)
			});
			LocalizedGameText.BindFormat(this.nextLevelText, "rank_ui", "Next Level: {0}", new object[] { level + 1 });
			LocalizedGameText.BindFormat(this.xpToGoText, "rank_ui", "{0} XP to go", new object[] { CielOSRankApp.N0(Mathf.Max(0f, num - xp)) });
			this.RebuildTimeline(config, rankIndex);
			RankDefinitionSO rankDefinitionSO = ((config != null && config.ranksInOrder != null && rankIndex + 1 < config.ranksInOrder.Count) ? config.ranksInOrder[rankIndex + 1] : null);
			foreach (GameObject gameObject in this._perkRows)
			{
				if (gameObject != null)
				{
					gameObject.SetActive(false);
					Object.Destroy(gameObject);
				}
			}
			this._perkRows.Clear();
			if (currentRank != null)
			{
				if (this.perksCurrentName != null)
				{
					LocalizedGameText.BindSource(this.perksCurrentName, "rank", currentRank.displayName, (string value) => value.ToUpperInvariant());
					this.perksCurrentName.color = currentRank.badgeColor;
				}
				this.SpawnPerkRows(this.perksCurrentRowTemplate, currentRank.perkLines);
			}
			if (this.perksNextBox != null)
			{
				this.perksNextBox.SetActive(rankDefinitionSO != null);
			}
			if (rankDefinitionSO != null)
			{
				if (this.perksNextName != null)
				{
					LocalizedGameText.BindSource(this.perksNextName, "rank", rankDefinitionSO.displayName, (string value) => value.ToUpperInvariant());
					this.perksNextName.color = rankDefinitionSO.badgeColor;
				}
				this.SpawnPerkRows(this.perksNextRowTemplate, rankDefinitionSO.perkLines);
			}
			bool flag = rankDefinitionSO == null;
			if (this.promotionBox != null)
			{
				this.promotionBox.SetActive(!flag);
			}
			if (this.maxRankBox != null)
			{
				this.maxRankBox.SetActive(flag);
			}
			if (!flag)
			{
				float bestQualifyingDistance = RankProgressionService.BestQualifyingDistance;
				LocalizedGameText.BindFormat(this.reqLevelTitle, "rank_ui", "Reach Level {0}", new object[] { rankDefinitionSO.minLevel });
				LocalizedGameText.BindFormat(this.reqLevelText, "rank_ui", "{0} / {1}", new object[] { level, rankDefinitionSO.minLevel });
				CielOSRankApp.SetFill(this.reqLevelFill, (rankDefinitionSO.minLevel > 0) ? Mathf.Clamp01((float)level / (float)rankDefinitionSO.minLevel) : 1f);
				LocalizedGameText.BindFormat(this.reqDistanceTitle, "rank_ui", "Achieve a peak distance of {0} m in a single run", new object[] { CielOSRankApp.DistanceN0(rankDefinitionSO.promotionRunDistance) });
				LocalizedGameText.BindFormat(this.reqDistanceText, "rank_ui", "{0} / {1} m", new object[]
				{
					CielOSRankApp.DistanceN0(bestQualifyingDistance),
					CielOSRankApp.DistanceN0(rankDefinitionSO.promotionRunDistance)
				});
				CielOSRankApp.SetFill(this.reqDistanceFill, (rankDefinitionSO.promotionRunDistance > 0f) ? Mathf.Clamp01(bestQualifyingDistance / rankDefinitionSO.promotionRunDistance) : 1f);
			}
		}

		// Token: 0x0600A0F9 RID: 41209 RVA: 0x0042AA54 File Offset: 0x00428C54
		private void RebuildTimeline(RankProgressionConfigSO cfg, int rankIndex)
		{
			if (this.progressionRowsParent == null || this.rankRowTemplate == null)
			{
				return;
			}
			foreach (GameObject gameObject in this._rankRows)
			{
				if (gameObject != null)
				{
					gameObject.SetActive(false);
					Object.Destroy(gameObject);
				}
			}
			this._rankRows.Clear();
			List<RankDefinitionSO> list = ((cfg != null) ? cfg.ranksInOrder : null);
			if (list == null || list.Count == 0)
			{
				return;
			}
			for (int i = 0; i < list.Count; i++)
			{
				RankDefinitionSO rankDefinitionSO = list[i];
				if (!(rankDefinitionSO == null))
				{
					GameObject gameObject2 = Object.Instantiate<GameObject>(this.rankRowTemplate, this.progressionRowsParent);
					gameObject2.name = string.Format("RankRow_{0}", i);
					gameObject2.SetActive(true);
					this._rankRows.Add(gameObject2);
					bool flag = i == rankIndex;
					bool flag2 = i < rankIndex;
					Transform transform = gameObject2.transform.Find("Name");
					TextMeshProUGUI textMeshProUGUI = ((transform != null) ? transform.GetComponent<TextMeshProUGUI>() : null);
					if (textMeshProUGUI != null)
					{
						LocalizedGameText.BindSource(textMeshProUGUI, "rank", rankDefinitionSO.displayName, (string value) => value.ToUpperInvariant());
						textMeshProUGUI.color = (flag ? rankDefinitionSO.badgeColor : (flag2 ? CielOSRankApp.TextPrimary : CielOSRankApp.TextLocked));
						textMeshProUGUI.fontStyle = (flag ? FontStyles.Bold : FontStyles.Normal);
					}
					Transform transform2 = gameObject2.transform.Find("Range");
					TextMeshProUGUI textMeshProUGUI2 = ((transform2 != null) ? transform2.GetComponent<TextMeshProUGUI>() : null);
					if (textMeshProUGUI2 != null)
					{
						if (i == list.Count - 1)
						{
							LocalizedGameText.BindFormat(textMeshProUGUI2, "rank_ui", "LV {0}+", new object[] { rankDefinitionSO.minLevel });
						}
						else
						{
							LocalizedGameText.BindFormat(textMeshProUGUI2, "rank_ui", "LV {0}–{1}", new object[]
							{
								rankDefinitionSO.minLevel,
								list[i + 1].minLevel - 1
							});
						}
						textMeshProUGUI2.color = (flag ? CielOSRankApp.TextMuted : CielOSRankApp.TextLocked);
					}
					Transform transform3 = gameObject2.transform.Find("DotWrap/Dot");
					Image image = ((transform3 != null) ? transform3.GetComponent<Image>() : null);
					if (image != null)
					{
						image.color = (flag ? rankDefinitionSO.badgeColor : (flag2 ? CielOSRankApp.Accent : CielOSRankApp.DotLocked));
						image.rectTransform.sizeDelta = (flag ? new Vector2(16f, 16f) : new Vector2(10f, 10f));
					}
				}
			}
			if (this.progressionTrackFill != null && list.Count > 1)
			{
				CielOSRankApp.SetFill(this.progressionTrackFill, (float)rankIndex / (float)(list.Count - 1));
			}
		}

		// Token: 0x0600A0FA RID: 41210 RVA: 0x0042AD68 File Offset: 0x00428F68
		private void PopulateMissions()
		{
			if (this._accountCatalog != null || this.missionsPanel == null)
			{
				return;
			}
			foreach (object obj in this.missionsPanel.transform)
			{
				((Transform)obj).gameObject.SetActive(false);
			}
			GameObject gameObject = new GameObject("Account Mission Catalog", new Type[] { typeof(RectTransform) });
			gameObject.SetActive(false);
			RectTransform rectTransform = (RectTransform)gameObject.transform;
			rectTransform.SetParent(this.missionsPanel.transform, false);
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.offsetMin = new Vector2(24f, 20f);
			rectTransform.offsetMax = new Vector2(-24f, -20f);
			gameObject.AddComponent<LayoutElement>().ignoreLayout = true;
			this._accountCatalog = gameObject.AddComponent<GlobalMissionsPanel>();
			this._accountCatalog.InitializeForPauseMenu((this.currentRankLabel != null) ? LocalizedGameText.SourceFont(this.currentRankLabel) : TMP_Settings.defaultFontAsset);
			gameObject.SetActive(true);
		}

		// Token: 0x0600A0FB RID: 41211 RVA: 0x0042AEB4 File Offset: 0x004290B4
		private void RefreshMissions()
		{
			if (this == null || !base.isActiveAndEnabled)
			{
				return;
			}
			this.PopulateMissions();
		}

		// Token: 0x0600A0FC RID: 41212 RVA: 0x0042AECE File Offset: 0x004290CE
		private static void SetFill(Image fill, float t)
		{
			if (fill == null)
			{
				return;
			}
			fill.rectTransform.anchorMax = new Vector2(Mathf.Clamp01(t), 1f);
		}

		// Token: 0x0600A0FD RID: 41213 RVA: 0x0042AEF5 File Offset: 0x004290F5
		private static void SetText(GameObject root, string path, string value)
		{
			Transform transform = root.transform.Find(path);
			LocalizedGameText localizedGameText = LocalizedGameText.BindSource((transform != null) ? transform.GetComponent<TextMeshProUGUI>() : null, "rank_ui", value, null);
			if (localizedGameText == null)
			{
				return;
			}
			localizedGameText.FitTranslationToRectangle(false);
		}

		// Token: 0x0600A0FE RID: 41214 RVA: 0x0042AF28 File Offset: 0x00429128
		private void SpawnPerkRows(GameObject template, string[] lines)
		{
			if (template == null || lines == null)
			{
				return;
			}
			foreach (string text in lines)
			{
				if (!string.IsNullOrWhiteSpace(text))
				{
					GameObject gameObject = Object.Instantiate<GameObject>(template, template.transform.parent);
					gameObject.name = "PerkRow";
					gameObject.SetActive(true);
					this._perkRows.Add(gameObject);
					CielOSRankApp.SetText(gameObject, "Label", text.Trim());
				}
			}
		}

		// Token: 0x0600A0FF RID: 41215 RVA: 0x0042AFA0 File Offset: 0x004291A0
		private static string N0(float v)
		{
			return Mathf.RoundToInt(v).ToString("N0", CultureInfo.InvariantCulture);
		}

		// Token: 0x0600A100 RID: 41216 RVA: 0x0042AFC8 File Offset: 0x004291C8
		private static string DistanceN0(float meters)
		{
			return Math.Floor((double)meters).ToString("N0", CultureInfo.InvariantCulture);
		}

		// Token: 0x0600A101 RID: 41217 RVA: 0x0042AFF0 File Offset: 0x004291F0
		private void MaximizeWindow()
		{
			CielOSWindow componentInParent = base.GetComponentInParent<CielOSWindow>();
			if (componentInParent == null || componentInParent.Rect == null)
			{
				return;
			}
			RectTransform rect = componentInParent.Rect;
			rect.anchorMin = Vector2.zero;
			rect.anchorMax = Vector2.one;
			rect.pivot = new Vector2(0.5f, 0.5f);
			rect.offsetMin = Vector2.zero;
			rect.offsetMax = Vector2.zero;
			CielOSWindowResizeHandle[] componentsInChildren = componentInParent.GetComponentsInChildren<CielOSWindowResizeHandle>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].gameObject.SetActive(false);
			}
			CielOSWindowDragHandle[] componentsInChildren2 = componentInParent.GetComponentsInChildren<CielOSWindowDragHandle>(true);
			for (int i = 0; i < componentsInChildren2.Length; i++)
			{
				componentsInChildren2[i].enabled = false;
			}
		}

		// Token: 0x0400A49E RID: 42142
		[Header("Tabs")]
		[Tooltip("Button of the 'Rank' tab (top tab bar).")]
		[SerializeField]
		private Button rankTabButton;

		// Token: 0x0400A49F RID: 42143
		[Tooltip("Button of the 'Missions' tab (top tab bar).")]
		[SerializeField]
		private Button missionsTabButton;

		// Token: 0x0400A4A0 RID: 42144
		[Tooltip("Underline shown under the active 'Rank' tab.")]
		[SerializeField]
		private GameObject rankTabUnderline;

		// Token: 0x0400A4A1 RID: 42145
		[Tooltip("Underline shown under the active 'Missions' tab.")]
		[SerializeField]
		private GameObject missionsTabUnderline;

		// Token: 0x0400A4A2 RID: 42146
		[Tooltip("Rank tab content (shown when the Rank tab is active).")]
		[SerializeField]
		private GameObject rankPanel;

		// Token: 0x0400A4A3 RID: 42147
		[Tooltip("Missions tab content (shown when the Missions tab is active).")]
		[SerializeField]
		private GameObject missionsPanel;

		// Token: 0x0400A4A4 RID: 42148
		[Tooltip("The empty-state inside the Missions panel (shown only when there are no mission cards).")]
		[SerializeField]
		private GameObject missionsEmptyState;

		// Token: 0x0400A4A5 RID: 42149
		[Header("A. Rank overview")]
		[SerializeField]
		private TextMeshProUGUI currentRankLabel;

		// Token: 0x0400A4A6 RID: 42150
		[SerializeField]
		private TextMeshProUGUI levelLabel;

		// Token: 0x0400A4A7 RID: 42151
		[SerializeField]
		private TextMeshProUGUI taglineLabel;

		// Token: 0x0400A4A8 RID: 42152
		[SerializeField]
		private TextMeshProUGUI totalXpLabel;

		// Token: 0x0400A4A9 RID: 42153
		[Tooltip("Fill image of the level progress bar (anchorMax.x driven).")]
		[SerializeField]
		private Image levelProgressFill;

		// Token: 0x0400A4AA RID: 42154
		[SerializeField]
		private TextMeshProUGUI levelProgressText;

		// Token: 0x0400A4AB RID: 42155
		[SerializeField]
		private TextMeshProUGUI nextLevelText;

		// Token: 0x0400A4AC RID: 42156
		[SerializeField]
		private TextMeshProUGUI xpToGoText;

		// Token: 0x0400A4AD RID: 42157
		[Header("B. Rank progression timeline")]
		[Tooltip("Parent (HBox) of the per-rank widgets cloned from rankRowTemplate.")]
		[SerializeField]
		private Transform progressionRowsParent;

		// Token: 0x0400A4AE RID: 42158
		[Tooltip("Disabled per-rank template cloned once per rank.")]
		[SerializeField]
		private GameObject rankRowTemplate;

		// Token: 0x0400A4AF RID: 42159
		[Tooltip("Optional track fill (line filled up to the current rank).")]
		[SerializeField]
		private Image progressionTrackFill;

		// Token: 0x0400A4B0 RID: 42160
		[Header("C. Rank perks")]
		[SerializeField]
		private TextMeshProUGUI perksCurrentName;

		// Token: 0x0400A4B1 RID: 42161
		[Tooltip("Disabled pill-row template (icon + label) cloned once per CURRENT rank perk line.")]
		[SerializeField]
		private GameObject perksCurrentRowTemplate;

		// Token: 0x0400A4B2 RID: 42162
		[Tooltip("Whole 'next rank' preview card (hidden at the final rank).")]
		[SerializeField]
		private GameObject perksNextBox;

		// Token: 0x0400A4B3 RID: 42163
		[SerializeField]
		private TextMeshProUGUI perksNextName;

		// Token: 0x0400A4B4 RID: 42164
		[Tooltip("Disabled pill-row template (icon + label) cloned once per NEXT rank perk line.")]
		[SerializeField]
		private GameObject perksNextRowTemplate;

		// Token: 0x0400A4B5 RID: 42165
		[Header("D. Promotion requirement")]
		[Tooltip("Whole promotion box (hidden at the final rank).")]
		[SerializeField]
		private GameObject promotionBox;

		// Token: 0x0400A4B6 RID: 42166
		[Tooltip("'Max rank reached' message (shown only at the final rank).")]
		[SerializeField]
		private GameObject maxRankBox;

		// Token: 0x0400A4B7 RID: 42167
		[SerializeField]
		private TextMeshProUGUI reqLevelTitle;

		// Token: 0x0400A4B8 RID: 42168
		[SerializeField]
		private TextMeshProUGUI reqLevelText;

		// Token: 0x0400A4B9 RID: 42169
		[SerializeField]
		private Image reqLevelFill;

		// Token: 0x0400A4BA RID: 42170
		[SerializeField]
		private TextMeshProUGUI reqDistanceTitle;

		// Token: 0x0400A4BB RID: 42171
		[SerializeField]
		private TextMeshProUGUI reqDistanceText;

		// Token: 0x0400A4BC RID: 42172
		[SerializeField]
		private Image reqDistanceFill;

		// Token: 0x0400A4BD RID: 42173
		[Header("E. Missions")]
		[Tooltip("Parent (scroll content VBox) of the mission cards cloned from missionCardTemplate.")]
		[SerializeField]
		private Transform missionsListParent;

		// Token: 0x0400A4BE RID: 42174
		[Tooltip("Disabled mission-card template cloned once per mission.")]
		[SerializeField]
		private GameObject missionCardTemplate;

		// Token: 0x0400A4BF RID: 42175
		[Tooltip("Disabled section-header template cloned once per mission category.")]
		[SerializeField]
		private GameObject missionSectionTemplate;

		// Token: 0x0400A4C0 RID: 42176
		private static readonly Color TextPrimary = new Color(0.9411765f, 0.91764706f, 0.972549f);

		// Token: 0x0400A4C1 RID: 42177
		private static readonly Color TextMuted = new Color(0.6901961f, 0.627451f, 0.76862746f);

		// Token: 0x0400A4C2 RID: 42178
		private static readonly Color TextLocked = new Color(0.41568628f, 0.37254903f, 0.49411765f);

		// Token: 0x0400A4C3 RID: 42179
		private static readonly Color Accent = new Color(0.65882355f, 0.38039216f, 0.8117647f);

		// Token: 0x0400A4C4 RID: 42180
		private static readonly Color DotLocked = new Color(0.22745098f, 0.1882353f, 0.28235295f);

		// Token: 0x0400A4C5 RID: 42181
		private bool _subscribed;

		// Token: 0x0400A4C6 RID: 42182
		private GlobalMissionsPanel _accountCatalog;

		// Token: 0x0400A4C7 RID: 42183
		private readonly List<GameObject> _rankRows = new List<GameObject>();

		// Token: 0x0400A4C8 RID: 42184
		private readonly List<GameObject> _perkRows = new List<GameObject>();

		// Token: 0x02001460 RID: 5216
		private enum Tab
		{
			// Token: 0x0400A4CA RID: 42186
			Rank,
			// Token: 0x0400A4CB RID: 42187
			Missions
		}
	}
}
