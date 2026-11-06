using System;
using System.Collections.Generic;
using Features.CielOS.UI;
using Features.Localization;
using ProjectExhibitionist.PoiTravel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Missions
{
	// Token: 0x02000F7E RID: 3966
	[RequireComponent(typeof(RectTransform))]
	public sealed class CompletedMissionsPanel : MonoBehaviour
	{
		// Token: 0x06007778 RID: 30584 RVA: 0x0030A10C File Offset: 0x0030830C
		public void Initialize(TMP_FontAsset font)
		{
			if (this._content != null)
			{
				return;
			}
			this._font = ((font != null) ? font : TMP_Settings.defaultFontAsset);
			Image image = base.gameObject.AddComponent<Image>();
			image.color = new Color32(34, 17, 49, byte.MaxValue);
			image.raycastTarget = false;
			RawImage rawImage = CompletedMissionsPanel.Rect("Project Exhibitionist Pattern", base.transform).gameObject.AddComponent<RawImage>();
			CompletedMissionsPanel.Stretch(rawImage.rectTransform);
			rawImage.color = new Color(1f, 1f, 1f, 0.12f);
			rawImage.raycastTarget = false;
			PoiPortalPresentation.ConfigurePattern(rawImage);
			TMP_Text tmp_Text = this.Label("Run Archive", base.transform, "RUN ARCHIVE", 16f, CompletedMissionsPanel.Pale);
			MissionLocalization.Bind(tmp_Text, "RUN ARCHIVE", true);
			CompletedMissionsPanel.Place(tmp_Text.rectTransform, 22f, 20f, 400f, 26f);
			tmp_Text.characterSpacing = 4f;
			this._summary = this.Label("Summary", base.transform, "", 25f, Color.white);
			CompletedMissionsPanel.Place(this._summary.rectTransform, 22f, 53f, 400f, 42f);
			TMP_Text tmp_Text2 = this.Label("Scope", base.transform, "THIS RUN  /  NEWEST FIRST", 13f, CompletedMissionsPanel.Pale);
			MissionLocalization.Bind(tmp_Text2, "THIS RUN  /  NEWEST FIRST", true);
			CompletedMissionsPanel.Place(tmp_Text2.rectTransform, 22f, 101f, 400f, 24f);
			RectTransform rectTransform = CompletedMissionsPanel.Rect("Viewport", base.transform);
			CompletedMissionsPanel.Stretch(rectTransform);
			rectTransform.offsetMin = new Vector2(18f, 18f);
			rectTransform.offsetMax = new Vector2(-18f, -148f);
			rectTransform.gameObject.AddComponent<RectMask2D>();
			rectTransform.gameObject.AddComponent<Image>().color = Color.clear;
			this._scroll = rectTransform.gameObject.AddComponent<ScrollRect>();
			this._scroll.horizontal = false;
			this._scroll.movementType = ScrollRect.MovementType.Clamped;
			rectTransform.gameObject.AddComponent<CielOSSmoothWheelScroll>().Bind(this._scroll);
			this._scroll.viewport = rectTransform;
			this._content = CompletedMissionsPanel.Rect("Completed Cards", rectTransform);
			this._content.anchorMin = new Vector2(0f, 1f);
			this._content.anchorMax = Vector2.one;
			this._content.pivot = new Vector2(0.5f, 1f);
			this._scroll.content = this._content;
			this._empty = this.Label("Empty History", rectTransform, "YOUR NEXT MISSION STARTS HERE\n\nComplete an active mission to fill your archive and earn XP.", 21f, CompletedMissionsPanel.Pale);
			MissionLocalization.Bind(this._empty, "YOUR NEXT MISSION STARTS HERE\n\nComplete an active mission to fill your archive and earn XP.", true);
			CompletedMissionsPanel.Stretch(this._empty.rectTransform);
			this._empty.rectTransform.offsetMin = new Vector2(24f, 60f);
			this._empty.rectTransform.offsetMax = new Vector2(-24f, -60f);
			this._empty.alignment = TextAlignmentOptions.Center;
			this.Refresh();
		}

		// Token: 0x06007779 RID: 30585 RVA: 0x0030A450 File Offset: 0x00308650
		private void OnEnable()
		{
			MissionService.Changed += this.Refresh;
			MissionService.Completed += this.OnCompleted;
			GameLocalization.LocaleChanged += this.Refresh;
			this.Refresh();
		}

		// Token: 0x0600777A RID: 30586 RVA: 0x0030A48B File Offset: 0x0030868B
		private void OnDisable()
		{
			MissionService.Changed -= this.Refresh;
			MissionService.Completed -= this.OnCompleted;
			GameLocalization.LocaleChanged -= this.Refresh;
		}

		// Token: 0x0600777B RID: 30587 RVA: 0x0030A4C0 File Offset: 0x003086C0
		private void OnCompleted(MissionCompletion completion)
		{
			this.Refresh();
		}

		// Token: 0x0600777C RID: 30588 RVA: 0x0030A4C8 File Offset: 0x003086C8
		private void Refresh()
		{
			if (this._content == null)
			{
				return;
			}
			IReadOnlyList<MissionCompletion> completedRunMissions = MissionService.CompletedRunMissions;
			MissionState missionState = ((completedRunMissions.Count > 0) ? completedRunMissions[completedRunMissions.Count - 1].Mission : null);
			if (string.Equals(this._lastLocale, GameLocalization.CurrentCode, StringComparison.Ordinal) && this._count == completedRunMissions.Count && this._latestMission == missionState)
			{
				return;
			}
			this._latestMission = missionState;
			this._lastLocale = GameLocalization.CurrentCode;
			int num = 0;
			foreach (MissionCompletion missionCompletion in completedRunMissions)
			{
				num += Mathf.RoundToInt(missionCompletion.XpOutcome.xpGained);
			}
			string text = ((completedRunMissions.Count == 1) ? MissionLocalization.Format("{0} mission  /  +{1:N0} XP", new object[] { completedRunMissions.Count, num }) : MissionLocalization.Format("{0} missions  /  +{1:N0} XP", new object[] { completedRunMissions.Count, num }));
			MissionLocalization.Set(this._summary, text);
			this._empty.gameObject.SetActive(completedRunMissions.Count == 0);
			float num2 = 0f;
			for (int i = 0; i < completedRunMissions.Count; i++)
			{
				MissionCompletion missionCompletion2 = completedRunMissions[completedRunMissions.Count - 1 - i];
				GameObject gameObject = ((i < this._rows.Count) ? this._rows[i] : this.CreateRow());
				gameObject.SetActive(true);
				TMP_Text component = gameObject.transform.Find("Description").GetComponent<TMP_Text>();
				string text2 = ((missionCompletion2.Mission != null) ? MissionLocalization.Full(missionCompletion2.Mission) : MissionLocalization.Text("Mission completed"));
				MissionLocalization.Set(component, text2);
				float num3 = Mathf.Max(48f, component.GetPreferredValues(component.text, 360f, 0f).y);
				float num4 = num3 + 92f;
				CompletedMissionsPanel.Place((RectTransform)gameObject.transform, 0f, num2, 408f, num4);
				CompletedMissionsPanel.Place(component.rectTransform, 22f, 51f, 360f, num3);
				MissionLocalization.Set(gameObject.transform.Find("Number").GetComponent<TMP_Text>(), MissionLocalization.Format("MISSION {0:00}", new object[] { completedRunMissions.Count - i }));
				MissionLocalization.Set(gameObject.transform.Find("Reward").GetComponent<TMP_Text>(), MissionLocalization.Format("+{0:N0} XP", new object[] { Mathf.RoundToInt(missionCompletion2.XpOutcome.xpGained) }));
				CompletedMissionsPanel.Place(gameObject.transform.Find("Status").GetComponent<TMP_Text>().rectTransform, 22f, num4 - 30f, 360f, 20f);
				num2 += num4 + 14f;
			}
			for (int j = completedRunMissions.Count; j < this._rows.Count; j++)
			{
				this._rows[j].SetActive(false);
			}
			this._content.sizeDelta = new Vector2(0f, Mathf.Max(0f, num2 - 14f));
			if (this._count != completedRunMissions.Count)
			{
				this._scroll.verticalNormalizedPosition = 1f;
			}
			this._count = completedRunMissions.Count;
		}

		// Token: 0x0600777D RID: 30589 RVA: 0x0030A878 File Offset: 0x00308A78
		private GameObject CreateRow()
		{
			RectTransform rectTransform = CompletedMissionsPanel.Rect("Completed Mission " + (this._rows.Count + 1).ToString(), this._content);
			rectTransform.gameObject.AddComponent<Image>().color = new Color32(56, 31, 75, 247);
			RectTransform rectTransform2 = CompletedMissionsPanel.Rect("Accent", rectTransform);
			CompletedMissionsPanel.Stretch(rectTransform2);
			rectTransform2.anchorMax = new Vector2(0f, 1f);
			rectTransform2.sizeDelta = new Vector2(4f, 0f);
			rectTransform2.gameObject.AddComponent<Image>().color = CompletedMissionsPanel.Violet;
			CompletedMissionsPanel.Place(this.Label("Number", rectTransform, "", 14f, CompletedMissionsPanel.Pale).rectTransform, 22f, 17f, 200f, 24f);
			TMP_Text tmp_Text = this.Label("Reward", rectTransform, "", 21f, Color.white);
			tmp_Text.alignment = TextAlignmentOptions.TopRight;
			CompletedMissionsPanel.Place(tmp_Text.rectTransform, 225f, 13f, 160f, 32f);
			this.Label("Description", rectTransform, "", 20f, Color.white);
			MissionLocalization.Bind(this.Label("Status", rectTransform, "COMPLETED  /  REWARD EARNED", 12f, CompletedMissionsPanel.Pale), "COMPLETED  /  REWARD EARNED", true);
			this._rows.Add(rectTransform.gameObject);
			return rectTransform.gameObject;
		}

		// Token: 0x0600777E RID: 30590 RVA: 0x0030A9FC File Offset: 0x00308BFC
		private TMP_Text Label(string name, Transform parent, string value, float size, Color color)
		{
			TextMeshProUGUI textMeshProUGUI = CompletedMissionsPanel.Rect(name, parent).gameObject.AddComponent<TextMeshProUGUI>();
			textMeshProUGUI.font = this._font;
			textMeshProUGUI.fontSize = size;
			textMeshProUGUI.fontStyle = FontStyles.Bold;
			textMeshProUGUI.color = color;
			textMeshProUGUI.text = value;
			textMeshProUGUI.isOrthographic = true;
			textMeshProUGUI.richText = false;
			textMeshProUGUI.enableWordWrapping = true;
			textMeshProUGUI.overflowMode = TextOverflowModes.Overflow;
			textMeshProUGUI.raycastTarget = false;
			return textMeshProUGUI;
		}

		// Token: 0x0600777F RID: 30591 RVA: 0x0030AA68 File Offset: 0x00308C68
		private static RectTransform Rect(string name, Transform parent)
		{
			GameObject gameObject = new GameObject(name, new Type[] { typeof(RectTransform) });
			gameObject.layer = parent.gameObject.layer;
			gameObject.transform.SetParent(parent, false);
			return (RectTransform)gameObject.transform;
		}

		// Token: 0x06007780 RID: 30592 RVA: 0x0030AAB8 File Offset: 0x00308CB8
		private static void Stretch(RectTransform rect)
		{
			rect.anchorMin = Vector2.zero;
			rect.anchorMax = Vector2.one;
			rect.offsetMin = (rect.offsetMax = Vector2.zero);
		}

		// Token: 0x06007781 RID: 30593 RVA: 0x0030AAF0 File Offset: 0x00308CF0
		private static void Place(RectTransform rect, float left, float top, float width, float height)
		{
			Vector2 vector = new Vector2(0f, 1f);
			rect.anchorMax = vector;
			rect.anchorMin = vector;
			rect.pivot = new Vector2(0f, 1f);
			rect.anchoredPosition = new Vector2(left, -top);
			rect.sizeDelta = new Vector2(width, height);
		}

		// Token: 0x04007AB8 RID: 31416
		private static readonly Color Violet = new Color32(169, 96, 201, byte.MaxValue);

		// Token: 0x04007AB9 RID: 31417
		private static readonly Color Pale = new Color32(230, 205, 250, byte.MaxValue);

		// Token: 0x04007ABA RID: 31418
		private readonly List<GameObject> _rows = new List<GameObject>();

		// Token: 0x04007ABB RID: 31419
		private TMP_FontAsset _font;

		// Token: 0x04007ABC RID: 31420
		private TMP_Text _summary;

		// Token: 0x04007ABD RID: 31421
		private TMP_Text _empty;

		// Token: 0x04007ABE RID: 31422
		private RectTransform _content;

		// Token: 0x04007ABF RID: 31423
		private ScrollRect _scroll;

		// Token: 0x04007AC0 RID: 31424
		private int _count = -1;

		// Token: 0x04007AC1 RID: 31425
		private MissionState _latestMission;

		// Token: 0x04007AC2 RID: 31426
		private string _lastLocale;
	}
}
