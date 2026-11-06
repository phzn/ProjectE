using System;
using System.Collections.Generic;
using Features.CielOS.UI;
using Features.Localization;
using Features.Rank;
using Features.RunModes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Missions
{
	// Token: 0x02000F7F RID: 3967
	public sealed class GlobalMissionsPanel : MonoBehaviour
	{
		// Token: 0x06007784 RID: 30596 RVA: 0x0030ABB8 File Offset: 0x00308DB8
		public void InitializeForPauseMenu(TMP_FontAsset font)
		{
			this._font = font;
		}

		// Token: 0x06007785 RID: 30597 RVA: 0x0030ABC4 File Offset: 0x00308DC4
		private void OnEnable()
		{
			MissionService.Changed += this.Refresh;
			MissionService.Progressed += this.HandleProgress;
			RankProgressionService.Changed += this.Refresh;
			RunModeService.Changed += this.Refresh;
			GameLocalization.LocaleChanged += this.Refresh;
			if (this._built)
			{
				this.Refresh();
			}
		}

		// Token: 0x06007786 RID: 30598 RVA: 0x0030AC34 File Offset: 0x00308E34
		private void Start()
		{
			MissionService.EnsureAccountMissions();
			this.Build();
			this.Refresh();
		}

		// Token: 0x06007787 RID: 30599 RVA: 0x0030AC48 File Offset: 0x00308E48
		private void OnDisable()
		{
			MissionService.Changed -= this.Refresh;
			MissionService.Progressed -= this.HandleProgress;
			RankProgressionService.Changed -= this.Refresh;
			RunModeService.Changed -= this.Refresh;
			GameLocalization.LocaleChanged -= this.Refresh;
		}

		// Token: 0x06007788 RID: 30600 RVA: 0x0030ACAC File Offset: 0x00308EAC
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
			this._summary = this.CreateText("Catalog Summary", rectTransform, string.Empty, 12f, FontStyles.Normal, MissionPanelStyle.Muted, TextAlignmentOptions.TopLeft);
			GlobalMissionsPanel.SetRect(this._summary.rectTransform, new Vector2(0f, 1f), Vector2.one, new Vector2(6f, -48f), new Vector2(-18f, 0f));
			RectTransform rectTransform2 = GlobalMissionsPanel.CreateRect("Mission Viewport", rectTransform);
			GlobalMissionsPanel.Stretch(rectTransform2);
			rectTransform2.offsetMax = new Vector2(-18f, -54f);
			Image image = rectTransform2.gameObject.AddComponent<Image>();
			image.color = new Color(0f, 0f, 0f, 0f);
			image.raycastTarget = true;
			rectTransform2.gameObject.AddComponent<RectMask2D>();
			RectTransform rectTransform3 = GlobalMissionsPanel.CreateRect("Mission List", rectTransform2);
			rectTransform3.anchorMin = new Vector2(0f, 1f);
			rectTransform3.anchorMax = Vector2.one;
			rectTransform3.pivot = new Vector2(0.5f, 1f);
			rectTransform3.anchoredPosition = Vector2.zero;
			this._content = rectTransform3;
			rectTransform3.sizeDelta = Vector2.zero;
			Scrollbar scrollbar = GlobalMissionsPanel.BuildScrollbar(rectTransform);
			ScrollRect scrollRect = base.gameObject.AddComponent<ScrollRect>();
			scrollRect.viewport = rectTransform2;
			scrollRect.content = rectTransform3;
			scrollRect.horizontal = false;
			scrollRect.vertical = true;
			scrollRect.movementType = ScrollRect.MovementType.Clamped;
			scrollRect.inertia = true;
			scrollRect.decelerationRate = 0.12f;
			base.gameObject.AddComponent<CielOSSmoothWheelScroll>().Bind(scrollRect);
			scrollRect.verticalScrollbar = scrollbar;
			scrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.Permanent;
			scrollRect.verticalNormalizedPosition = 1f;
			this.Refresh();
		}

		// Token: 0x06007789 RID: 30601 RVA: 0x0030AE90 File Offset: 0x00309090
		private void Update()
		{
			if (!this._built)
			{
				return;
			}
			float num = 2.8f * Time.unscaledDeltaTime;
			for (int i = 0; i < this._rows.Count; i++)
			{
				GlobalMissionsPanel.Row row = this._rows[i];
				if (row != null && !(row.Fill == null) && !Mathf.Approximately(row.DisplayedFill, row.TargetFill))
				{
					row.DisplayedFill = Mathf.MoveTowards(row.DisplayedFill, row.TargetFill, num);
					GlobalMissionsPanel.SetHorizontalFill(row.Fill, row.DisplayedFill);
				}
			}
		}

		// Token: 0x0600778A RID: 30602 RVA: 0x0030AF24 File Offset: 0x00309124
		private void LateUpdate()
		{
			if (this._built && ((RectTransform)base.transform).rect.size != this._lastSize)
			{
				this.LayoutRows();
			}
		}

		// Token: 0x0600778B RID: 30603 RVA: 0x0030AF64 File Offset: 0x00309164
		private void LayoutRows()
		{
			this._lastSize = ((RectTransform)base.transform).rect.size;
			float num = Mathf.Max(64f, this._lastSize.x - 18f);
			float num2 = 0f;
			MissionCategory? missionCategory = null;
			foreach (GlobalMissionsPanel.Row row in this._rows)
			{
				if (row.Root.activeSelf)
				{
					MissionCategory? missionCategory2 = missionCategory;
					MissionCategory category = row.Category;
					if (!((missionCategory2.GetValueOrDefault() == category) & (missionCategory2 != null)))
					{
						missionCategory = new MissionCategory?(row.Category);
						this._categoryHeaders[row.Category].rectTransform.anchoredPosition = new Vector2(6f, -num2);
						num2 += 34f;
					}
					float num3 = Mathf.Ceil(row.Reward.GetPreferredValues(row.Reward.text, float.PositiveInfinity, float.PositiveInfinity).x);
					float num4 = num3 + 32f;
					float num5 = Mathf.Ceil(row.Description.GetPreferredValues(row.Description.text, Mathf.Max(1f, num * 0.91f - num4), float.PositiveInfinity).y);
					RectTransform rectTransform = (RectTransform)row.Root.transform;
					float num6 = Mathf.Max(62f, num5 + 60f);
					rectTransform.anchoredPosition = new Vector2(0f, -num2);
					rectTransform.sizeDelta = new Vector2(0f, num6);
					GlobalMissionsPanel.SetRect(row.Description.rectTransform, new Vector2(0.09f, 1f), Vector2.one, new Vector2(0f, -8f - num5), new Vector2(-num4, -8f));
					GlobalMissionsPanel.SetRect(row.Reward.rectTransform, Vector2.one, Vector2.one, new Vector2(-20f - num3, -32f), new Vector2(-20f, -8f));
					GlobalMissionsPanel.SetRect(row.Progress.rectTransform, new Vector2(0.09f, 0f), new Vector2(0.38f, 0f), new Vector2(0f, 5f), new Vector2(0f, 25f));
					GlobalMissionsPanel.SetRect(row.Availability.rectTransform, new Vector2(0.09f, 0f), new Vector2(1f, 0f), new Vector2(0f, 28f), new Vector2(-20f, 48f));
					num2 += num6 + 14f;
				}
			}
			this._content.sizeDelta = new Vector2(0f, Mathf.Max(0f, num2 - 14f));
		}

		// Token: 0x0600778C RID: 30604 RVA: 0x0030B284 File Offset: 0x00309484
		private GlobalMissionsPanel.Row BuildRow(RectTransform parent, int index, float y, float height)
		{
			RectTransform rectTransform = GlobalMissionsPanel.CreateRect("Account Mission " + (index + 1).ToString(), parent);
			rectTransform.anchorMin = new Vector2(0f, 1f);
			rectTransform.anchorMax = Vector2.one;
			rectTransform.pivot = new Vector2(0.5f, 1f);
			rectTransform.anchoredPosition = new Vector2(0f, y);
			rectTransform.sizeDelta = new Vector2(0f, height);
			Color card = MissionPanelStyle.Card;
			Image image = rectTransform.gameObject.AddComponent<Image>();
			image.color = card;
			image.raycastTarget = true;
			GlobalMissionsPanel.Round(rectTransform.gameObject, 14f);
			Color accent = MissionPanelStyle.Accent;
			accent.a = 0f;
			Image image2 = GlobalMissionsPanel.CreateImage("Hover Accent", rectTransform, accent);
			GlobalMissionsPanel.SetRect(image2.rectTransform, Vector2.zero, new Vector2(0f, 1f), new Vector2(0f, 8f), new Vector2(5f, -8f));
			GlobalMissionsPanel.Round(image2.gameObject, 3f);
			GlobalMissionsPanel.SetRect(this.CreateText("Number", rectTransform, (index + 1).ToString("00"), 13f, FontStyles.Bold, MissionPanelStyle.Accent, TextAlignmentOptions.Center).rectTransform, Vector2.zero, new Vector2(0.075f, 1f), Vector2.zero, Vector2.zero);
			TextMeshProUGUI textMeshProUGUI = this.CreateText("Description", rectTransform, string.Empty, 16f, FontStyles.Bold, MissionPanelStyle.Text, TextAlignmentOptions.MidlineLeft);
			textMeshProUGUI.enableWordWrapping = true;
			textMeshProUGUI.enableAutoSizing = false;
			textMeshProUGUI.fontSizeMin = 16f;
			textMeshProUGUI.fontSizeMax = 16f;
			textMeshProUGUI.overflowMode = TextOverflowModes.Overflow;
			textMeshProUGUI.richText = false;
			GlobalMissionsPanel.SetRect(textMeshProUGUI.rectTransform, new Vector2(0.09f, 0.38f), new Vector2(0.77f, 1f), Vector2.zero, new Vector2(-8f, -2f));
			TextMeshProUGUI textMeshProUGUI2 = this.CreateText("Progress", rectTransform, string.Empty, 12f, FontStyles.Bold, MissionPanelStyle.Muted, TextAlignmentOptions.MidlineLeft);
			GlobalMissionsPanel.SetRect(textMeshProUGUI2.rectTransform, new Vector2(0.09f, 0f), new Vector2(0.38f, 0.42f), Vector2.zero, new Vector2(0f, -2f));
			TextMeshProUGUI textMeshProUGUI3 = this.CreateText("Reward", rectTransform, string.Empty, 14f, FontStyles.Bold, MissionPanelStyle.Accent, TextAlignmentOptions.MidlineRight);
			GlobalMissionsPanel.SetRect(textMeshProUGUI3.rectTransform, new Vector2(0.76f, 0.38f), Vector2.one, Vector2.zero, new Vector2(-20f, -2f));
			Image image3 = GlobalMissionsPanel.CreateImage("Track", rectTransform, MissionPanelStyle.Track);
			RectTransform rectTransform2 = image3.rectTransform;
			rectTransform2.anchorMin = new Vector2(0.38f, 0f);
			rectTransform2.anchorMax = new Vector2(1f, 0f);
			rectTransform2.pivot = new Vector2(0.5f, 0f);
			rectTransform2.anchoredPosition = new Vector2(-10f, 14f);
			rectTransform2.sizeDelta = new Vector2(-20f, 5f);
			GlobalMissionsPanel.Round(image3.gameObject, 3f);
			Image image4 = GlobalMissionsPanel.CreateImage("Fill", rectTransform2, MissionPanelStyle.Accent);
			image4.type = Image.Type.Filled;
			image4.fillMethod = Image.FillMethod.Horizontal;
			GlobalMissionsPanel.Stretch(image4.rectTransform);
			GlobalMissionsPanel.SetHorizontalFill(image4, 0f);
			MissionRowFx missionRowFx = rectTransform.gameObject.AddComponent<MissionRowFx>();
			missionRowFx.Configure(image, image2, card, MissionPanelStyle.CardHover);
			missionRowFx.PlayEntrance((float)Mathf.Min(index, 10) * 0.035f);
			return new GlobalMissionsPanel.Row
			{
				Root = rectTransform.gameObject,
				Description = textMeshProUGUI,
				Progress = textMeshProUGUI2,
				Reward = textMeshProUGUI3,
				Availability = this.CreateText("Availability", rectTransform, string.Empty, 12f, FontStyles.Normal, MissionPanelStyle.Muted, TextAlignmentOptions.MidlineLeft),
				Fill = image4,
				Fx = missionRowFx
			};
		}

		// Token: 0x0600778D RID: 30605 RVA: 0x0030B6B0 File Offset: 0x003098B0
		private static Scrollbar BuildScrollbar(RectTransform parent)
		{
			Image image = GlobalMissionsPanel.CreateImage("Scrollbar", parent, MissionPanelStyle.ScrollTrack);
			RectTransform rectTransform = image.rectTransform;
			rectTransform.anchorMin = new Vector2(1f, 0f);
			rectTransform.anchorMax = Vector2.one;
			rectTransform.pivot = new Vector2(1f, 0.5f);
			rectTransform.offsetMin = new Vector2(-7f, 0f);
			rectTransform.offsetMax = Vector2.zero;
			GlobalMissionsPanel.Round(image.gameObject, 4f);
			RectTransform rectTransform2 = GlobalMissionsPanel.CreateRect("Sliding Area", rectTransform);
			GlobalMissionsPanel.Stretch(rectTransform2);
			RectTransform rectTransform3 = GlobalMissionsPanel.CreateRect("Handle", rectTransform2);
			GlobalMissionsPanel.Stretch(rectTransform3);
			Image image2 = rectTransform3.gameObject.AddComponent<Image>();
			image2.color = MissionPanelStyle.Accent;
			GlobalMissionsPanel.Round(image2.gameObject, 4f);
			Scrollbar scrollbar = image.gameObject.AddComponent<Scrollbar>();
			scrollbar.handleRect = rectTransform3;
			scrollbar.targetGraphic = image2;
			scrollbar.direction = Scrollbar.Direction.BottomToTop;
			return scrollbar;
		}

		// Token: 0x0600778E RID: 30606 RVA: 0x0030B7A3 File Offset: 0x003099A3
		private void HandleProgress(MissionState mission)
		{
			if (mission.scope == MissionScope.Global)
			{
				this.Refresh();
			}
		}

		// Token: 0x0600778F RID: 30607 RVA: 0x0030B7B4 File Offset: 0x003099B4
		private void Refresh()
		{
			if (!this._built)
			{
				return;
			}
			IReadOnlyList<MissionState> readOnlyList = AccountMissionPresentation.Catalog();
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			foreach (MissionState missionState in readOnlyList)
			{
				if (missionState.IsComplete)
				{
					num++;
				}
				if (missionState.secret)
				{
					num2++;
					if (missionState.IsComplete)
					{
						num3++;
					}
				}
			}
			MissionLocalization.Set(this._summary, MissionLocalization.Format("ACCOUNT MISSIONS  |  {0} / {1} completed\nOne-time XP rewards. Scroll to browse every objective.", new object[] { num, readOnlyList.Count }));
			while (this._rows.Count < readOnlyList.Count)
			{
				this._rows.Add(this.BuildRow(this._content, this._rows.Count, 0f, 62f));
			}
			foreach (TextMeshProUGUI textMeshProUGUI in this._categoryHeaders.Values)
			{
				textMeshProUGUI.gameObject.SetActive(false);
			}
			MissionCategory? missionCategory = null;
			for (int i = 0; i < this._rows.Count; i++)
			{
				bool flag = i < readOnlyList.Count && readOnlyList[i] != null;
				this._rows[i].Root.SetActive(flag);
				if (flag)
				{
					MissionState missionState2 = readOnlyList[i];
					MissionCategory? missionCategory2 = missionCategory;
					MissionCategory category = missionState2.category;
					if (!((missionCategory2.GetValueOrDefault() == category) & (missionCategory2 != null)))
					{
						missionCategory = new MissionCategory?(missionState2.category);
						TextMeshProUGUI textMeshProUGUI2;
						if (!this._categoryHeaders.TryGetValue(missionState2.category, out textMeshProUGUI2))
						{
							textMeshProUGUI2 = this.CreateText("Category " + missionState2.category.ToString(), this._content, string.Empty, 12f, FontStyles.Bold, MissionPanelStyle.Accent, TextAlignmentOptions.MidlineLeft);
							textMeshProUGUI2.rectTransform.anchorMin = new Vector2(0f, 1f);
							textMeshProUGUI2.rectTransform.anchorMax = Vector2.one;
							textMeshProUGUI2.rectTransform.pivot = new Vector2(0.5f, 1f);
							textMeshProUGUI2.rectTransform.sizeDelta = new Vector2(-12f, 34f);
							MissionLocalization.Bind(textMeshProUGUI2, MissionRankPolicy.CategoryName(missionState2.category), true);
							this._categoryHeaders.Add(missionState2.category, textMeshProUGUI2);
						}
						textMeshProUGUI2.gameObject.SetActive(true);
						if (missionState2.category == MissionCategory.Secret)
						{
							MissionLocalization.Set(textMeshProUGUI2, MissionLocalization.Format("SECRET MISSIONS  {0} / {1}", new object[] { num3, num2 }));
						}
					}
					this._rows[i].Category = missionState2.category;
					object obj = !string.IsNullOrEmpty(this._rows[i].MissionId) && this._rows[i].MissionId != missionState2.id;
					bool flag2 = AccountMissionPresentation.IsConcealed(missionState2);
					float num4 = (flag2 ? 0f : missionState2.NormalizedProgress);
					bool flag3 = this._rows[i].MissionId == missionState2.id && num4 > this._rows[i].TargetFill + 0.0001f;
					MissionLocalization.Set(this._rows[i].Description, flag2 ? AccountMissionPresentation.SecretTitle(missionState2) : MissionLocalization.Full(missionState2));
					MissionLocalization.Set(this._rows[i].Availability, AccountMissionPresentation.Availability(missionState2));
					if (missionState2.IsComplete)
					{
						MissionLocalization.Bind(this._rows[i].Progress, "COMPLETED", true);
					}
					else if (flag2)
					{
						MissionLocalization.Set(this._rows[i].Progress, "???");
					}
					else
					{
						MissionLocalization.Set(this._rows[i].Progress, GlobalMissionsPanel.FormatProgress(missionState2));
					}
					this._rows[i].Reward.text = "+" + missionState2.xpReward.ToString("N0") + " XP";
					this._rows[i].MissionId = missionState2.id;
					object obj2 = obj;
					if (obj2 != null)
					{
						this._rows[i].DisplayedFill = 0f;
					}
					this._rows[i].TargetFill = num4;
					if ((obj2 | flag3) != null)
					{
						this._rows[i].Fx.PulseProgress();
					}
				}
			}
			this.LayoutRows();
		}

		// Token: 0x06007790 RID: 30608 RVA: 0x0030BCBC File Offset: 0x00309EBC
		private static string FormatProgress(MissionState mission)
		{
			string text = (MissionRankPolicy.IsMeters(mission.kind) ? " m" : (MissionRankPolicy.IsSeconds(mission.kind) ? " s" : string.Empty));
			return Mathf.FloorToInt(mission.progress).ToString("N0") + " / " + Mathf.RoundToInt(mission.target).ToString("N0") + text;
		}

		// Token: 0x06007791 RID: 30609 RVA: 0x00290F05 File Offset: 0x0028F105
		private static RectTransform CreateRect(string name, Transform parent)
		{
			RectTransform component = new GameObject(name, new Type[] { typeof(RectTransform) }).GetComponent<RectTransform>();
			component.SetParent(parent, false);
			return component;
		}

		// Token: 0x06007792 RID: 30610 RVA: 0x0030BD32 File Offset: 0x00309F32
		private static Image CreateImage(string name, Transform parent, Color color)
		{
			Image image = GlobalMissionsPanel.CreateRect(name, parent).gameObject.AddComponent<Image>();
			image.color = color;
			image.raycastTarget = false;
			return image;
		}

		// Token: 0x06007793 RID: 30611 RVA: 0x0030BD54 File Offset: 0x00309F54
		private TextMeshProUGUI CreateText(string name, Transform parent, string value, float size, FontStyles style, Color color, TextAlignmentOptions alignment)
		{
			TextMeshProUGUI textMeshProUGUI = GlobalMissionsPanel.CreateRect(name, parent).gameObject.AddComponent<TextMeshProUGUI>();
			textMeshProUGUI.text = value;
			textMeshProUGUI.fontSize = size;
			textMeshProUGUI.fontStyle = style;
			textMeshProUGUI.color = color;
			textMeshProUGUI.alignment = alignment;
			textMeshProUGUI.raycastTarget = false;
			textMeshProUGUI.isOrthographic = true;
			textMeshProUGUI.font = ((this._font != null) ? this._font : TMP_Settings.defaultFontAsset);
			return textMeshProUGUI;
		}

		// Token: 0x06007794 RID: 30612 RVA: 0x0030BDC8 File Offset: 0x00309FC8
		private static void Round(GameObject target, float radius)
		{
			MissionPanelStyle.Round(target, radius);
		}

		// Token: 0x06007795 RID: 30613 RVA: 0x0018F85B File Offset: 0x0018DA5B
		private static void SetRect(RectTransform rect, Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin, Vector2 offsetMax)
		{
			rect.anchorMin = anchorMin;
			rect.anchorMax = anchorMax;
			rect.offsetMin = offsetMin;
			rect.offsetMax = offsetMax;
		}

		// Token: 0x06007796 RID: 30614 RVA: 0x001A38AD File Offset: 0x001A1AAD
		private static void Stretch(RectTransform rect)
		{
			rect.anchorMin = Vector2.zero;
			rect.anchorMax = Vector2.one;
			rect.offsetMin = Vector2.zero;
			rect.offsetMax = Vector2.zero;
		}

		// Token: 0x06007797 RID: 30615 RVA: 0x0030BDD4 File Offset: 0x00309FD4
		private static void SetHorizontalFill(Image image, float amount)
		{
			if (image == null)
			{
				return;
			}
			RectTransform rectTransform = image.rectTransform;
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = new Vector2(Mathf.Clamp01(amount), 1f);
			rectTransform.offsetMin = Vector2.zero;
			rectTransform.offsetMax = Vector2.zero;
		}

		// Token: 0x04007AC3 RID: 31427
		private const float RowHeight = 62f;

		// Token: 0x04007AC4 RID: 31428
		private const float CategoryHeight = 34f;

		// Token: 0x04007AC5 RID: 31429
		private readonly List<GlobalMissionsPanel.Row> _rows = new List<GlobalMissionsPanel.Row>();

		// Token: 0x04007AC6 RID: 31430
		private readonly Dictionary<MissionCategory, TextMeshProUGUI> _categoryHeaders = new Dictionary<MissionCategory, TextMeshProUGUI>();

		// Token: 0x04007AC7 RID: 31431
		private RectTransform _content;

		// Token: 0x04007AC8 RID: 31432
		private bool _built;

		// Token: 0x04007AC9 RID: 31433
		private TMP_FontAsset _font;

		// Token: 0x04007ACA RID: 31434
		private Vector2 _lastSize;

		// Token: 0x04007ACB RID: 31435
		private TextMeshProUGUI _summary;

		// Token: 0x02000F80 RID: 3968
		private sealed class Row
		{
			// Token: 0x04007ACC RID: 31436
			public GameObject Root;

			// Token: 0x04007ACD RID: 31437
			public TextMeshProUGUI Description;

			// Token: 0x04007ACE RID: 31438
			public TextMeshProUGUI Progress;

			// Token: 0x04007ACF RID: 31439
			public TextMeshProUGUI Reward;

			// Token: 0x04007AD0 RID: 31440
			public TextMeshProUGUI Availability;

			// Token: 0x04007AD1 RID: 31441
			public Image Fill;

			// Token: 0x04007AD2 RID: 31442
			public MissionRowFx Fx;

			// Token: 0x04007AD3 RID: 31443
			public string MissionId;

			// Token: 0x04007AD4 RID: 31444
			public MissionCategory Category;

			// Token: 0x04007AD5 RID: 31445
			public float DisplayedFill;

			// Token: 0x04007AD6 RID: 31446
			public float TargetFill;
		}
	}
}
