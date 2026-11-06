using System;
using System.Collections.Generic;
using Features.CielOS.UI;
using Features.Localization;
using Features.OxGram;
using Features.PhoneCompanion;
using Features.RunModes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Missions
{
	// Token: 0x02000FB5 RID: 4021
	[DisallowMultipleComponent]
	[RequireComponent(typeof(RectTransform))]
	public sealed class MissionsPhoneWidget : MonoBehaviour
	{
		// Token: 0x06007967 RID: 31079 RVA: 0x00319190 File Offset: 0x00317390
		internal void Initialize(TMP_FontAsset font)
		{
			if (this._content != null)
			{
				return;
			}
			foreach (object obj in base.transform)
			{
				((Transform)obj).gameObject.SetActive(false);
			}
			Image component = base.GetComponent<Image>();
			if (component != null)
			{
				component.sprite = PhoneUiSpriteFactory.Widget;
				component.type = Image.Type.Simple;
				component.color = MissionsPhoneWidget.Background;
			}
			this._content = OxGramUI.Rect("Mission Summary", base.transform);
			this._content.anchorMin = (this._content.anchorMax = Vector2.one * 0.5f);
			this._content.sizeDelta = Vector2.one * 140f;
			MissionLocalization.Bind(this.Label("Heading", "MISSIONS", 12f, MissionsPhoneWidget.Mint, font, 14f, 13f, 96f, 19f), "MISSIONS", true);
			Image image = OxGramUI.Image("Notebook", this._content, MissionsPhoneWidget.Mint, CielOSIconLibrary.GetSprite(CielOSIconId.Notebook, 64));
			image.type = Image.Type.Simple;
			image.preserveAspect = true;
			image.raycastTarget = false;
			MissionsPhoneWidget.Position(image.rectTransform, 108f, 14f, 18f, 18f);
			this._count = this.Label("Completed Count", "0", 34f, Color.white, font, 14f, 32f, 112f, 48f);
			this._status = this.Label("Status", "completed this run", 10f, MissionsPhoneWidget.Secondary, font, 14f, 80f, 112f, 16f);
			this._reward = this.Label("XP Earned", "0 XP earned", 11f, MissionsPhoneWidget.Mint, font, 14f, 97f, 112f, 17f);
			Image image2 = OxGramUI.Image("Divider", this._content, new Color(MissionsPhoneWidget.Mint.r, MissionsPhoneWidget.Mint.g, MissionsPhoneWidget.Mint.b, 0.2f), null);
			image2.raycastTarget = false;
			MissionsPhoneWidget.Position(image2.rectTransform, 14f, 117f, 112f, 1f);
			this._action = this.Label("Open Missions", "VIEW MISSIONS  >", 10f, Color.white, font, 14f, 120f, 112f, 15f);
			this.FitContent();
			this.Refresh();
		}

		// Token: 0x06007968 RID: 31080 RVA: 0x0031944C File Offset: 0x0031764C
		private void OnEnable()
		{
			MissionService.Changed += this.Refresh;
			RunModeService.Changed += this.Refresh;
			GameLocalization.LocaleChanged += this.Refresh;
			this.FitContent();
			this.Refresh();
		}

		// Token: 0x06007969 RID: 31081 RVA: 0x00319498 File Offset: 0x00317698
		private void OnDisable()
		{
			MissionService.Changed -= this.Refresh;
			RunModeService.Changed -= this.Refresh;
			GameLocalization.LocaleChanged -= this.Refresh;
		}

		// Token: 0x0600796A RID: 31082 RVA: 0x003194CD File Offset: 0x003176CD
		private void OnRectTransformDimensionsChange()
		{
			this.FitContent();
		}

		// Token: 0x0600796B RID: 31083 RVA: 0x003194D8 File Offset: 0x003176D8
		private void FitContent()
		{
			if (this._content == null)
			{
				return;
			}
			Vector2 size = ((RectTransform)base.transform).rect.size;
			float num = Mathf.Min(size.x, size.y) / 140f;
			this._content.localScale = new Vector3(num, num, 1f);
		}

		// Token: 0x0600796C RID: 31084 RVA: 0x0031953C File Offset: 0x0031773C
		private void Refresh()
		{
			if (this._count == null)
			{
				return;
			}
			bool areRunMissionsEnabled = MissionService.AreRunMissionsEnabled;
			IReadOnlyList<MissionCompletion> completedRunMissions = MissionService.CompletedRunMissions;
			int num = (areRunMissionsEnabled ? completedRunMissions.Count : 0);
			int num2 = 0;
			if (areRunMissionsEnabled)
			{
				for (int i = 0; i < completedRunMissions.Count; i++)
				{
					num2 += Mathf.RoundToInt(completedRunMissions[i].XpOutcome.xpGained);
				}
			}
			MissionLocalization.Set(this._count, MissionLocalization.Text(areRunMissionsEnabled ? num.ToString() : "--"));
			MissionLocalization.Set(this._status, MissionLocalization.Text(areRunMissionsEnabled ? "completed this run" : "no active missions"));
			string text = (areRunMissionsEnabled ? MissionLocalization.Format("{0:N0} XP earned", new object[] { num2 }) : MissionLocalization.Text(RunModeService.IsLongWayHome ? "Return home" : "Start a new run"));
			MissionLocalization.Set(this._reward, text);
			MissionLocalization.Set(this._action, MissionLocalization.Text((areRunMissionsEnabled && num == 0) ? "VIEW OBJECTIVES  >" : "VIEW MISSIONS  >"));
		}

		// Token: 0x0600796D RID: 31085 RVA: 0x0031964C File Offset: 0x0031784C
		private TMP_Text Label(string name, string value, float size, Color color, TMP_FontAsset font, float left, float top, float width, float height)
		{
			TMP_Text tmp_Text = OxGramUI.Text(name, this._content, value, font, size, color, TextAlignmentOptions.MidlineLeft);
			tmp_Text.fontStyle = FontStyles.Bold;
			tmp_Text.enableWordWrapping = false;
			tmp_Text.overflowMode = TextOverflowModes.Ellipsis;
			MissionsPhoneWidget.Position(tmp_Text.rectTransform, left, top, width, height);
			MissionLocalization.Bind(tmp_Text, value, true);
			return tmp_Text;
		}

		// Token: 0x0600796E RID: 31086 RVA: 0x003196A4 File Offset: 0x003178A4
		private static void Position(RectTransform rect, float left, float top, float width, float height)
		{
			Vector2 vector = new Vector2(0f, 1f);
			rect.anchorMax = vector;
			rect.anchorMin = vector;
			rect.pivot = new Vector2(0f, 1f);
			rect.anchoredPosition = new Vector2(left, -top);
			rect.sizeDelta = new Vector2(width, height);
		}

		// Token: 0x04007CB8 RID: 31928
		private static readonly Color Background = new Color(0.045f, 0.205f, 0.17f, 1f);

		// Token: 0x04007CB9 RID: 31929
		private static readonly Color Mint = new Color(0.58f, 0.98f, 0.79f, 1f);

		// Token: 0x04007CBA RID: 31930
		private static readonly Color Secondary = new Color(0.73f, 0.85f, 0.79f, 1f);

		// Token: 0x04007CBB RID: 31931
		private RectTransform _content;

		// Token: 0x04007CBC RID: 31932
		private TMP_Text _count;

		// Token: 0x04007CBD RID: 31933
		private TMP_Text _status;

		// Token: 0x04007CBE RID: 31934
		private TMP_Text _reward;

		// Token: 0x04007CBF RID: 31935
		private TMP_Text _action;
	}
}
