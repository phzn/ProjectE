using System;
using System.Collections.Generic;
using Features.CielOS.Desktop;
using Features.CielOS.Panel;
using Features.CielOS.WindowManager;
using Features.Localization;
using Features.PhoneCompanion;
using Features.Rank;
using Features.RunModes;
using ProjectExhibitionist.Core.Rendering;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Features.CielOS
{
	// Token: 0x02001376 RID: 4982
	public sealed class CielOSAppUnlockNotice : MonoBehaviour
	{
		// Token: 0x17001892 RID: 6290
		// (get) Token: 0x06009A33 RID: 39475 RVA: 0x003F1958 File Offset: 0x003EFB58
		public bool IsNoticeVisible
		{
			get
			{
				return this._notice != null && this._notice.gameObject.activeInHierarchy;
			}
		}

		// Token: 0x17001893 RID: 6291
		// (get) Token: 0x06009A34 RID: 39476 RVA: 0x003F197C File Offset: 0x003EFB7C
		public static bool HasPendingNotice
		{
			get
			{
				if (!RankProgressionService.IsAdminRankActive)
				{
					RankDefinitionSO currentRank = RankProgressionService.CurrentRank;
					if (((currentRank != null) ? currentRank.rankId : null) == "hornyslut_declared")
					{
						return !RankProgressionService.GetStoryProgressionData().pcAdvancedAppsNoticeSeen || !RankProgressionService.GetStoryProgressionData().personalDiaryNoticeSeen;
					}
				}
				return false;
			}
		}

		// Token: 0x06009A35 RID: 39477 RVA: 0x003F19CA File Offset: 0x003EFBCA
		private void Awake()
		{
			this._controller = base.GetComponent<CielOSController>();
			this._availabilityKey = CielOSAppUnlockNotice.AvailabilityKey;
		}

		// Token: 0x17001894 RID: 6292
		// (get) Token: 0x06009A36 RID: 39478 RVA: 0x003F19E4 File Offset: 0x003EFBE4
		private static string AvailabilityKey
		{
			get
			{
				RankDefinitionSO currentRank = RankProgressionService.CurrentRank;
				return ((currentRank != null) ? currentRank.rankId : null) + "|" + RankedLeaderboardService.IsSupported.ToString();
			}
		}

		// Token: 0x06009A37 RID: 39479 RVA: 0x003F1A19 File Offset: 0x003EFC19
		private void OnEnable()
		{
			RankProgressionService.Changed += this.OnProgressionChanged;
			RankedLeaderboardService.AvailabilityChanged += this.OnProgressionChanged;
		}

		// Token: 0x06009A38 RID: 39480 RVA: 0x003F1A3D File Offset: 0x003EFC3D
		private void OnDisable()
		{
			RankProgressionService.Changed -= this.OnProgressionChanged;
			RankedLeaderboardService.AvailabilityChanged -= this.OnProgressionChanged;
			this.Hide();
		}

		// Token: 0x06009A39 RID: 39481 RVA: 0x003F1A67 File Offset: 0x003EFC67
		private void OnProgressionChanged()
		{
			this._refreshPending = true;
		}

		// Token: 0x06009A3A RID: 39482 RVA: 0x003F1A70 File Offset: 0x003EFC70
		public void OnPcOpened()
		{
			this._refreshPending = true;
			this.RefreshAvailability();
		}

		// Token: 0x06009A3B RID: 39483 RVA: 0x003F1A7F File Offset: 0x003EFC7F
		public void Hide()
		{
			if (this._notice != null)
			{
				this._notice.gameObject.SetActive(false);
			}
		}

		// Token: 0x06009A3C RID: 39484 RVA: 0x003F1AA0 File Offset: 0x003EFCA0
		private void Update()
		{
			if (!Application.isPlaying)
			{
				this.Hide();
				return;
			}
			this.TickNotice(this._controller != null && this._controller.IsOpen, SceneManager.GetActiveScene().name == "CielHome");
		}

		// Token: 0x06009A3D RID: 39485 RVA: 0x003F1AF4 File Offset: 0x003EFCF4
		private void TickNotice(bool pcOpen, bool atHome)
		{
			if (!pcOpen)
			{
				this.Hide();
				return;
			}
			if (this._refreshPending)
			{
				this.RefreshAvailability();
			}
			if (!atHome || GraphicsSettingsRuntimeUI.AnyOpen || PhotoPolaroidViewer.IsOpen)
			{
				return;
			}
			if (this.IsNoticeVisible)
			{
				if (Time.frameCount <= this._shownFrame)
				{
					return;
				}
				this.MarkSeenIfVisible(pcOpen);
				return;
			}
			else
			{
				if (!CielOSAppUnlockNotice.HasPendingNotice || RankStoryProgression.HasPendingChapter)
				{
					return;
				}
				this.ShowNotice();
				return;
			}
		}

		// Token: 0x06009A3E RID: 39486 RVA: 0x003F1B60 File Offset: 0x003EFD60
		private void RefreshAvailability()
		{
			this._refreshPending = false;
			if (this._windows == null)
			{
				this._windows = base.GetComponentInChildren<CielOSWindowManager>(true);
			}
			string availabilityKey = CielOSAppUnlockNotice.AvailabilityKey;
			if (this._availabilityKey == availabilityKey)
			{
				return;
			}
			this._availabilityKey = availabilityKey;
			if (this._windows != null)
			{
				foreach (CielOSWindow cielOSWindow in new List<CielOSWindow>(this._windows.Windows))
				{
					CielOSAppDefinition cielOSAppDefinition;
					if (!(cielOSWindow != null))
					{
						cielOSAppDefinition = null;
					}
					else
					{
						CielOSAppCatalogSO catalog = this._windows.Catalog;
						cielOSAppDefinition = ((catalog != null) ? catalog.FindById(cielOSWindow.AppId) : null);
					}
					CielOSAppDefinition cielOSAppDefinition2 = cielOSAppDefinition;
					if (cielOSAppDefinition2 != null && !cielOSAppDefinition2.IsAvailable)
					{
						cielOSWindow.Close();
					}
				}
			}
			CielOSDesktop[] componentsInChildren = base.GetComponentsInChildren<CielOSDesktop>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].RefreshAvailability();
			}
			CielOSDock[] componentsInChildren2 = base.GetComponentsInChildren<CielOSDock>(true);
			for (int i = 0; i < componentsInChildren2.Length; i++)
			{
				componentsInChildren2[i].RefreshAvailability();
			}
			CielOSAppMenu[] componentsInChildren3 = base.GetComponentsInChildren<CielOSAppMenu>(true);
			for (int i = 0; i < componentsInChildren3.Length; i++)
			{
				componentsInChildren3[i].RefreshAvailability();
			}
			CielOSQuickTools[] componentsInChildren4 = base.GetComponentsInChildren<CielOSQuickTools>(true);
			for (int i = 0; i < componentsInChildren4.Length; i++)
			{
				componentsInChildren4[i].Rebuild();
			}
			RankDefinitionSO currentRank = RankProgressionService.CurrentRank;
			if (((currentRank != null) ? currentRank.rankId : null) != "hornyslut_declared")
			{
				this.Hide();
			}
		}

		// Token: 0x06009A3F RID: 39487 RVA: 0x003F1D00 File Offset: 0x003EFF00
		private bool IsActuallyVisible()
		{
			if (!this.IsNoticeVisible)
			{
				return false;
			}
			foreach (CanvasGroup canvasGroup in this._notice.GetComponentsInParent<CanvasGroup>(true))
			{
				if (canvasGroup.alpha < 0.99f || !canvasGroup.interactable || !canvasGroup.blocksRaycasts)
				{
					return false;
				}
			}
			Canvas[] componentsInParent2 = this._notice.GetComponentsInParent<Canvas>(true);
			if (componentsInParent2.Length == 0)
			{
				return false;
			}
			Canvas[] array = componentsInParent2;
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i].isActiveAndEnabled)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06009A40 RID: 39488 RVA: 0x003F1D88 File Offset: 0x003EFF88
		private void MarkSeenIfVisible(bool pcOpen)
		{
			if (!pcOpen || !this.IsActuallyVisible() || GraphicsSettingsRuntimeUI.AnyOpen || PhotoPolaroidViewer.IsOpen)
			{
				return;
			}
			RankProgressionData storyProgressionData = RankProgressionService.GetStoryProgressionData();
			if (storyProgressionData.pcAdvancedAppsNoticeSeen && storyProgressionData.personalDiaryNoticeSeen)
			{
				return;
			}
			storyProgressionData.pcAdvancedAppsNoticeSeen = true;
			storyProgressionData.personalDiaryNoticeSeen = true;
			RankProgressionService.Save();
		}

		// Token: 0x06009A41 RID: 39489 RVA: 0x003F1DD9 File Offset: 0x003EFFD9
		private void Dismiss()
		{
			this.MarkSeenIfVisible(this._controller != null && this._controller.IsOpen);
			this.Hide();
		}

		// Token: 0x06009A42 RID: 39490 RVA: 0x003F1E04 File Offset: 0x003F0004
		private void ShowNotice()
		{
			if (this._notice == null)
			{
				this._notice = new GameObject("NewAppsNotice", new Type[]
				{
					typeof(RectTransform),
					typeof(Image)
				}).GetComponent<RectTransform>();
				this._notice.SetParent(base.transform, false);
				this._notice.anchorMin = (this._notice.anchorMax = Vector2.one);
				this._notice.pivot = Vector2.one;
				this._notice.anchoredPosition = new Vector2(-28f, -60f);
				this._notice.sizeDelta = new Vector2(520f, 218f);
				Image component = this._notice.GetComponent<Image>();
				component.color = new Color(0.13f, 0.09f, 0.19f, 0.98f);
				CielOSTheme.Round(component);
				CielOSTheme.AddBorder(this._notice, new Color?(new Color(0.76f, 0.46f, 1f)), 1f);
				this.AddLabel("New features unlocked", new Vector2(20f, -15f), new Vector2(430f, 32f), 25f);
				this.AddLabel("Maps and Dialogues are ready. Continue your personal diary in Story with your own pages, words and photos.", new Vector2(20f, -51f), new Vector2(476f, 78f), 18f);
				this.AddButton("Maps", "maps", new Vector2(20f, -141f), new Vector2(148f, 57f));
				this.AddButton("Dialogues", "dialoghi", new Vector2(184f, -141f), new Vector2(148f, 57f));
				this.AddButton("Story", "story", new Vector2(348f, -141f), new Vector2(148f, 57f));
				this.AddButton("x", null, new Vector2(474f, -10f), new Vector2(32f, 32f)).onClick.AddListener(new UnityAction(this.Dismiss));
			}
			this._notice.SetAsLastSibling();
			this._notice.gameObject.SetActive(true);
			this._shownFrame = Time.frameCount;
		}

		// Token: 0x06009A43 RID: 39491 RVA: 0x003F2078 File Offset: 0x003F0278
		private TMP_Text AddLabel(string text, Vector2 position, Vector2 size, float fontSize)
		{
			TextMeshProUGUI component = new GameObject(text, new Type[]
			{
				typeof(RectTransform),
				typeof(TextMeshProUGUI)
			}).GetComponent<TextMeshProUGUI>();
			CielOSAppUnlockNotice.Place((RectTransform)component.transform, this._notice, position, size);
			component.text = text;
			component.fontSize = fontSize;
			component.color = Color.white;
			component.raycastTarget = false;
			if (CielOSTheme.Active != null && CielOSTheme.Active.uiFont != null)
			{
				component.font = CielOSTheme.Active.uiFont;
			}
			LocalizedGameText.BindSource(component, "cielos_shell", text, null).FitTranslationToRectangle(false);
			return component;
		}

		// Token: 0x06009A44 RID: 39492 RVA: 0x003F2130 File Offset: 0x003F0330
		private Button AddButton(string label, string appId, Vector2 position, Vector2 size)
		{
			RectTransform component = new GameObject(label, new Type[]
			{
				typeof(RectTransform),
				typeof(Image),
				typeof(Button)
			}).GetComponent<RectTransform>();
			CielOSAppUnlockNotice.Place(component, this._notice, position, size);
			Image component2 = component.GetComponent<Image>();
			component2.color = new Color(0.38f, 0.22f, 0.49f);
			CielOSTheme.Round(component2);
			Button component3 = component.GetComponent<Button>();
			component3.targetGraphic = component2;
			TMP_Text tmp_Text = this.AddLabel(label, Vector2.zero, size, 21f);
			CielOSAppUnlockNotice.Place((RectTransform)tmp_Text.transform, component, Vector2.zero, size);
			tmp_Text.alignment = TextAlignmentOptions.Center;
			if (appId != null)
			{
				component3.onClick.AddListener(delegate
				{
					if (this._controller == null || !this._controller.IsOpen || !this.IsActuallyVisible())
					{
						return;
					}
					this.Dismiss();
					CielOSWindowManager windows = this._windows;
					if (windows == null)
					{
						return;
					}
					windows.LaunchApp(appId);
				});
			}
			return component3;
		}

		// Token: 0x06009A45 RID: 39493 RVA: 0x003F2224 File Offset: 0x003F0424
		private static void Place(RectTransform rect, Transform parent, Vector2 position, Vector2 size)
		{
			rect.SetParent(parent, false);
			Vector2 vector = new Vector2(0f, 1f);
			rect.pivot = vector;
			rect.anchorMin = (rect.anchorMax = vector);
			rect.anchoredPosition = position;
			rect.sizeDelta = size;
		}

		// Token: 0x04009F2B RID: 40747
		private CielOSController _controller;

		// Token: 0x04009F2C RID: 40748
		private CielOSWindowManager _windows;

		// Token: 0x04009F2D RID: 40749
		private string _availabilityKey;

		// Token: 0x04009F2E RID: 40750
		private bool _refreshPending = true;

		// Token: 0x04009F2F RID: 40751
		private RectTransform _notice;

		// Token: 0x04009F30 RID: 40752
		private int _shownFrame;
	}
}
