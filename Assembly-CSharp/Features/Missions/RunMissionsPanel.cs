using System;
using System.Collections.Generic;
using Features.CielOS.UI;
using Features.Localization;
using Features.RunModes;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Features.Missions
{
	// Token: 0x02000FBE RID: 4030
	[RequireComponent(typeof(RectTransform))]
	public sealed class RunMissionsPanel : MonoBehaviour
	{
		// Token: 0x060079A7 RID: 31143 RVA: 0x0031AE18 File Offset: 0x00319018
		public void Initialize(TMP_FontAsset font)
		{
			this._font = ((font != null) ? font : TMP_Settings.defaultFontAsset);
			this.Build();
			foreach (TMP_Text tmp_Text in base.GetComponentsInChildren<TMP_Text>(true))
			{
				tmp_Text.font = this._font;
				LocalizedGameText component = tmp_Text.GetComponent<LocalizedGameText>();
				if (component != null)
				{
					component.SetSourceFont(this._font);
				}
			}
			this.Refresh();
			this.LayoutRows();
		}

		// Token: 0x060079A8 RID: 31144 RVA: 0x0031AE90 File Offset: 0x00319090
		private void OnEnable()
		{
			MissionService.Changed += this.Refresh;
			MissionService.Progressed += this.HandleProgress;
			RunModeService.Changed += this.Refresh;
			GameLocalization.LocaleChanged += this.Refresh;
			SceneManager.activeSceneChanged += this.HandleSceneChanged;
			this.Build();
			this.Refresh();
		}

		// Token: 0x060079A9 RID: 31145 RVA: 0x0031AF00 File Offset: 0x00319100
		private void OnDisable()
		{
			MissionService.Changed -= this.Refresh;
			MissionService.Progressed -= this.HandleProgress;
			RunModeService.Changed -= this.Refresh;
			GameLocalization.LocaleChanged -= this.Refresh;
			SceneManager.activeSceneChanged -= this.HandleSceneChanged;
		}

		// Token: 0x060079AA RID: 31146 RVA: 0x0031AF62 File Offset: 0x00319162
		private void HandleProgress(MissionState mission)
		{
			if (mission != null && mission.scope == MissionScope.Run)
			{
				this.Refresh();
			}
		}

		// Token: 0x060079AB RID: 31147 RVA: 0x0031AF76 File Offset: 0x00319176
		private void HandleSceneChanged(Scene previous, Scene next)
		{
			this.Refresh();
		}

		// Token: 0x060079AC RID: 31148 RVA: 0x0031AF80 File Offset: 0x00319180
		private void LateUpdate()
		{
			if (((RectTransform)base.transform).rect.size == this._lastSize)
			{
				return;
			}
			this.LayoutRows();
		}

		// Token: 0x060079AD RID: 31149 RVA: 0x0031AFBC File Offset: 0x003191BC
		private void Build()
		{
			if (this._scroll != null)
			{
				return;
			}
			if (this._font == null)
			{
				this._font = TMP_Settings.defaultFontAsset;
			}
			RectTransform rectTransform = (RectTransform)base.transform;
			RectTransform rectTransform2 = RunMissionsPanel.Rect("Run Badge", rectTransform);
			RunMissionsPanel.Top(rectTransform2, 0f, 0f, 0f, 26f);
			rectTransform2.anchorMax = new Vector2(0f, 1f);
			rectTransform2.sizeDelta = new Vector2(52f, 26f);
			rectTransform2.pivot = new Vector2(0f, 1f);
			rectTransform2.anchoredPosition = Vector2.zero;
			Image image = rectTransform2.gameObject.AddComponent<Image>();
			image.color = MissionPanelStyle.Card;
			image.raycastTarget = false;
			MissionPanelStyle.Round(rectTransform2.gameObject, 8f);
			TMP_Text tmp_Text = this.Text("Heading", rectTransform2, 13f, MissionPanelStyle.Accent);
			tmp_Text.fontStyle = FontStyles.Bold;
			tmp_Text.alignment = TextAlignmentOptions.Center;
			RunMissionsPanel.Stretch(tmp_Text.rectTransform, Vector2.zero, Vector2.zero);
			MissionLocalization.Bind(tmp_Text, "RUN", true);
			this._context = this.Text("Context", rectTransform, 13f, MissionPanelStyle.Muted);
			this._context.alignment = TextAlignmentOptions.MidlineLeft;
			RunMissionsPanel.Top(this._context.rectTransform, 64f, 0f, 18f, 26f);
			RectTransform rectTransform3 = RunMissionsPanel.Rect("Mission Viewport", rectTransform);
			RunMissionsPanel.Stretch(rectTransform3, Vector2.zero, new Vector2(-18f, -40f));
			rectTransform3.gameObject.AddComponent<Image>().color = Color.clear;
			rectTransform3.gameObject.AddComponent<RectMask2D>();
			this._content = RunMissionsPanel.Rect("Mission List", rectTransform3);
			this._content.anchorMin = new Vector2(0f, 1f);
			this._content.anchorMax = Vector2.one;
			this._content.pivot = new Vector2(0.5f, 1f);
			this._content.sizeDelta = Vector2.zero;
			this._empty = this.Text("Empty State", this._content, 16f, MissionPanelStyle.Muted);
			RunMissionsPanel.Top(this._empty.rectTransform, 8f, 8f, 8f, 160f);
			this._scroll = base.gameObject.AddComponent<ScrollRect>();
			this._scroll.viewport = rectTransform3;
			this._scroll.content = this._content;
			this._scroll.horizontal = false;
			this._scroll.vertical = true;
			this._scroll.movementType = ScrollRect.MovementType.Clamped;
			base.gameObject.AddComponent<CielOSSmoothWheelScroll>().Bind(this._scroll);
			this._scroll.decelerationRate = 0.12f;
			RectTransform rectTransform4 = RunMissionsPanel.Rect("Scrollbar", rectTransform);
			RunMissionsPanel.Stretch(rectTransform4, new Vector2(-7f, 0f), new Vector2(0f, -40f));
			rectTransform4.anchorMin = new Vector2(1f, 0f);
			rectTransform4.gameObject.AddComponent<Image>().color = MissionPanelStyle.ScrollTrack;
			MissionPanelStyle.Round(rectTransform4.gameObject, 4f);
			RectTransform rectTransform5 = RunMissionsPanel.Rect("Sliding Area", rectTransform4);
			RunMissionsPanel.Stretch(rectTransform5, Vector2.zero, Vector2.zero);
			RectTransform rectTransform6 = RunMissionsPanel.Rect("Handle", rectTransform5);
			RunMissionsPanel.Stretch(rectTransform6, Vector2.zero, Vector2.zero);
			Image image2 = rectTransform6.gameObject.AddComponent<Image>();
			image2.color = MissionPanelStyle.Accent;
			MissionPanelStyle.Round(rectTransform6.gameObject, 4f);
			Scrollbar scrollbar = rectTransform4.gameObject.AddComponent<Scrollbar>();
			scrollbar.handleRect = rectTransform6;
			scrollbar.targetGraphic = image2;
			scrollbar.direction = Scrollbar.Direction.BottomToTop;
			this._scroll.verticalScrollbar = scrollbar;
			this._scroll.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.Permanent;
		}

		// Token: 0x060079AE RID: 31150 RVA: 0x0031B3B0 File Offset: 0x003195B0
		private RunMissionsPanel.Row BuildRow(int index)
		{
			RectTransform rectTransform = RunMissionsPanel.Rect("Run Mission " + (index + 1).ToString(), this._content);
			Image image = rectTransform.gameObject.AddComponent<Image>();
			image.color = MissionPanelStyle.Card;
			MissionPanelStyle.Round(rectTransform.gameObject, 14f);
			Image image2 = RunMissionsPanel.Rect("Hover Accent", rectTransform).gameObject.AddComponent<Image>();
			image2.color = new Color(MissionPanelStyle.Accent.r, MissionPanelStyle.Accent.g, MissionPanelStyle.Accent.b, 0f);
			image2.raycastTarget = false;
			RunMissionsPanel.Stretch(image2.rectTransform, new Vector2(0f, 8f), new Vector2(5f, -8f));
			image2.rectTransform.anchorMax = new Vector2(0f, 1f);
			MissionPanelStyle.Round(image2.gameObject, 3f);
			RunMissionsPanel.Row row = new RunMissionsPanel.Row
			{
				Root = rectTransform,
				Number = this.Text("Number", rectTransform, 13f, MissionPanelStyle.Accent),
				Description = this.Text("Description", rectTransform, 16f, MissionPanelStyle.Text),
				Progress = this.Text("Progress", rectTransform, 12f, MissionPanelStyle.Muted),
				Reward = this.Text("Reward", rectTransform, 14f, MissionPanelStyle.Accent),
				Track = RunMissionsPanel.Rect("Progress Track", rectTransform)
			};
			row.Number.text = (index + 1).ToString("00");
			row.Number.fontStyle = FontStyles.Bold;
			row.Number.alignment = TextAlignmentOptions.Center;
			RunMissionsPanel.Stretch(row.Number.rectTransform, Vector2.zero, Vector2.zero);
			row.Number.rectTransform.anchorMax = new Vector2(0.075f, 1f);
			row.Description.fontStyle = FontStyles.Bold;
			row.Progress.fontStyle = FontStyles.Bold;
			row.Reward.fontStyle = FontStyles.Bold;
			row.Reward.alignment = TextAlignmentOptions.MidlineRight;
			row.Track.gameObject.AddComponent<Image>().color = MissionPanelStyle.Track;
			MissionPanelStyle.Round(row.Track.gameObject, 3f);
			row.Fill = RunMissionsPanel.Rect("Fill", row.Track);
			Image image3 = row.Fill.gameObject.AddComponent<Image>();
			image3.color = MissionPanelStyle.Accent;
			image3.type = Image.Type.Filled;
			image3.fillMethod = Image.FillMethod.Horizontal;
			row.Fx = rectTransform.gameObject.AddComponent<MissionRowFx>();
			row.Fx.Configure(image, image2, MissionPanelStyle.Card, MissionPanelStyle.CardHover);
			this._rows.Add(row);
			return row;
		}

		// Token: 0x060079AF RID: 31151 RVA: 0x0031B67C File Offset: 0x0031987C
		private void Refresh()
		{
			if (this._scroll == null)
			{
				return;
			}
			IReadOnlyList<MissionState> runMissions = MissionService.RunMissions;
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < runMissions.Count; i++)
			{
				RunMissionsPanel.Row row = ((i < this._rows.Count) ? this._rows[i] : this.BuildRow(i));
				MissionState missionState = runMissions[i];
				bool flag3 = row.MissionId == missionState.id && missionState.NormalizedProgress > row.PreviousProgress;
				flag |= row.MissionId != missionState.id;
				string text = row.Description.text;
				string text2 = MissionLocalization.Full(missionState);
				flag2 |= !row.Root.gameObject.activeSelf || text != text2;
				row.MissionId = missionState.id;
				row.Root.gameObject.SetActive(true);
				MissionLocalization.Set(row.Description, text2);
				row.Progress.text = RunMissionPresentation.FormatProgress(missionState);
				row.Reward.text = "+" + missionState.xpReward.ToString("N0") + " XP";
				RunMissionsPanel.Stretch(row.Fill, Vector2.zero, Vector2.zero);
				row.Fill.anchorMax = new Vector2(RunMissionPresentation.DisplayProgress01(missionState), 1f);
				row.PreviousProgress = missionState.NormalizedProgress;
				if (flag3 && Application.isPlaying)
				{
					row.Fx.PulseProgress();
				}
			}
			for (int j = runMissions.Count; j < this._rows.Count; j++)
			{
				flag |= this._rows[j].MissionId != null;
				flag2 |= this._rows[j].Root.gameObject.activeSelf;
				this._rows[j].MissionId = null;
				this._rows[j].Root.gameObject.SetActive(false);
			}
			string text3 = (MissionService.AreRunMissionsEnabled ? MissionService.RunContextName : (RunModeService.IsLongWayHome ? "LONG WAY HOME" : "NO ACTIVE RUN"));
			MissionLocalization.Set(this._context, MissionService.AreRunMissionsEnabled ? MissionLocalization.Context(text3) : MissionLocalization.Text(text3));
			this._empty.gameObject.SetActive(runMissions.Count == 0);
			string text4 = (RunModeService.IsLongWayHome ? "Your objective is to return home. Use the map to find your front door." : "Start a run to see its current objectives here.");
			MissionLocalization.Bind(this._empty, text4, true);
			if (flag2 || ((RectTransform)base.transform).rect.size != this._lastSize)
			{
				this.LayoutRows();
			}
			if (flag)
			{
				this._scroll.StopMovement();
				this._scroll.verticalNormalizedPosition = 1f;
			}
		}

		// Token: 0x060079B0 RID: 31152 RVA: 0x0031B97C File Offset: 0x00319B7C
		private void LayoutRows()
		{
			if (this._content == null)
			{
				return;
			}
			this._lastSize = ((RectTransform)base.transform).rect.size;
			float num = Mathf.Max(64f, this._lastSize.x - 18f);
			float num2 = 0f;
			for (int i = 0; i < this._rows.Count; i++)
			{
				RunMissionsPanel.Row row = this._rows[i];
				if (row.Root.gameObject.activeSelf)
				{
					float num3 = Mathf.Ceil(row.Description.GetPreferredValues(row.Description.text, Mathf.Max(1f, num * 0.68f - 8f), float.PositiveInfinity).y);
					float num4 = Mathf.Max(62f, num3 + 36f);
					RunMissionsPanel.Top(row.Root, 0f, num2, 0f, num4);
					RunMissionsPanel.Top(row.Description.rectTransform, num * 0.09f, 8f, num * 0.23f + 8f, num3);
					RunMissionsPanel.Top(row.Progress.rectTransform, 0f, num4 - 25f, 0f, 20f);
					row.Progress.rectTransform.anchorMin = new Vector2(0.09f, 1f);
					row.Progress.rectTransform.anchorMax = new Vector2(0.38f, 1f);
					RunMissionsPanel.Top(row.Reward.rectTransform, 0f, 8f, 20f, 24f);
					row.Reward.rectTransform.anchorMin = new Vector2(0.76f, 1f);
					RunMissionsPanel.Top(row.Track, 0f, num4 - 19f, 20f, 5f);
					row.Track.anchorMin = new Vector2(0.38f, 1f);
					num2 += num4 + 14f;
				}
			}
			this._content.sizeDelta = new Vector2(0f, Mathf.Max(176f, num2));
		}

		// Token: 0x060079B1 RID: 31153 RVA: 0x0031BBC8 File Offset: 0x00319DC8
		private TMP_Text Text(string name, Transform parent, float size, Color color)
		{
			TextMeshProUGUI textMeshProUGUI = RunMissionsPanel.Rect(name, parent).gameObject.AddComponent<TextMeshProUGUI>();
			textMeshProUGUI.font = this._font;
			textMeshProUGUI.fontSize = size;
			textMeshProUGUI.isOrthographic = true;
			textMeshProUGUI.color = color;
			textMeshProUGUI.alignment = TextAlignmentOptions.TopLeft;
			textMeshProUGUI.enableWordWrapping = true;
			textMeshProUGUI.enableAutoSizing = false;
			textMeshProUGUI.overflowMode = TextOverflowModes.Overflow;
			textMeshProUGUI.richText = false;
			textMeshProUGUI.raycastTarget = false;
			return textMeshProUGUI;
		}

		// Token: 0x060079B2 RID: 31154 RVA: 0x0031BC38 File Offset: 0x00319E38
		private static RectTransform Rect(string name, Transform parent)
		{
			GameObject gameObject = new GameObject(name, new Type[] { typeof(RectTransform) });
			gameObject.layer = parent.gameObject.layer;
			gameObject.transform.SetParent(parent, false);
			return (RectTransform)gameObject.transform;
		}

		// Token: 0x060079B3 RID: 31155 RVA: 0x0031BC88 File Offset: 0x00319E88
		private static void Top(RectTransform rect, float left, float top, float right, float height)
		{
			rect.anchorMin = new Vector2(0f, 1f);
			rect.anchorMax = Vector2.one;
			rect.pivot = new Vector2(0.5f, 1f);
			rect.offsetMin = new Vector2(left, -top - height);
			rect.offsetMax = new Vector2(-right, -top);
		}

		// Token: 0x060079B4 RID: 31156 RVA: 0x0013F62D File Offset: 0x0013D82D
		private static void Stretch(RectTransform rect, Vector2 min, Vector2 max)
		{
			rect.anchorMin = Vector2.zero;
			rect.anchorMax = Vector2.one;
			rect.offsetMin = min;
			rect.offsetMax = max;
		}

		// Token: 0x04007CED RID: 31981
		private readonly List<RunMissionsPanel.Row> _rows = new List<RunMissionsPanel.Row>();

		// Token: 0x04007CEE RID: 31982
		private TMP_FontAsset _font;

		// Token: 0x04007CEF RID: 31983
		private TMP_Text _context;

		// Token: 0x04007CF0 RID: 31984
		private TMP_Text _empty;

		// Token: 0x04007CF1 RID: 31985
		private ScrollRect _scroll;

		// Token: 0x04007CF2 RID: 31986
		private RectTransform _content;

		// Token: 0x04007CF3 RID: 31987
		private Vector2 _lastSize;

		// Token: 0x02000FBF RID: 4031
		private sealed class Row
		{
			// Token: 0x04007CF4 RID: 31988
			public RectTransform Root;

			// Token: 0x04007CF5 RID: 31989
			public TMP_Text Number;

			// Token: 0x04007CF6 RID: 31990
			public TMP_Text Description;

			// Token: 0x04007CF7 RID: 31991
			public TMP_Text Progress;

			// Token: 0x04007CF8 RID: 31992
			public TMP_Text Reward;

			// Token: 0x04007CF9 RID: 31993
			public RectTransform Track;

			// Token: 0x04007CFA RID: 31994
			public RectTransform Fill;

			// Token: 0x04007CFB RID: 31995
			public MissionRowFx Fx;

			// Token: 0x04007CFC RID: 31996
			public string MissionId;

			// Token: 0x04007CFD RID: 31997
			public float PreviousProgress;
		}
	}
}
