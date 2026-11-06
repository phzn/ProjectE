using System;
using System.Collections.Generic;
using Features.CielOS.Apps.Story;
using Features.Localization;
using TMPro;
using UnityEngine;

namespace Features.CielOS.WindowManager
{
	// Token: 0x020013A0 RID: 5024
	public class CielOSWindowManager : MonoBehaviour
	{
		// Token: 0x170018C6 RID: 6342
		// (get) Token: 0x06009B9C RID: 39836 RVA: 0x003F941D File Offset: 0x003F761D
		public CielOSTheme Theme
		{
			get
			{
				return this.theme;
			}
		}

		// Token: 0x170018C7 RID: 6343
		// (get) Token: 0x06009B9D RID: 39837 RVA: 0x003F9425 File Offset: 0x003F7625
		public CielOSAppCatalogSO Catalog
		{
			get
			{
				return this.catalog;
			}
		}

		// Token: 0x14000117 RID: 279
		// (add) Token: 0x06009B9E RID: 39838 RVA: 0x003F9430 File Offset: 0x003F7630
		// (remove) Token: 0x06009B9F RID: 39839 RVA: 0x003F9468 File Offset: 0x003F7668
		public event Action<CielOSWindow> WindowOpened;

		// Token: 0x14000118 RID: 280
		// (add) Token: 0x06009BA0 RID: 39840 RVA: 0x003F94A0 File Offset: 0x003F76A0
		// (remove) Token: 0x06009BA1 RID: 39841 RVA: 0x003F94D8 File Offset: 0x003F76D8
		public event Action<CielOSWindow> WindowClosed;

		// Token: 0x06009BA2 RID: 39842 RVA: 0x003F9510 File Offset: 0x003F7710
		public CielOSWindow LaunchApp(string appId)
		{
			if (this.catalog == null)
			{
				Debug.LogWarning("[CielOSWindowManager] No catalog assigned.", this);
				return null;
			}
			CielOSAppDefinition cielOSAppDefinition = this.catalog.FindById(appId);
			if (cielOSAppDefinition == null)
			{
				Debug.LogWarning("[CielOSWindowManager] No app with id '" + appId + "'.", this);
				return null;
			}
			return this.LaunchApp(cielOSAppDefinition);
		}

		// Token: 0x06009BA3 RID: 39843 RVA: 0x003F9570 File Offset: 0x003F7770
		public CielOSWindow LaunchApp(CielOSAppDefinition def)
		{
			if (def == null)
			{
				return null;
			}
			if (!def.IsPlatformSupported && def.IsUnlocked)
			{
				this.ShowSteamRequirement();
				return null;
			}
			if (!CielOSStoryFlow.AllowsAppLaunch(def.appId))
			{
				return null;
			}
			if (!def.IsAvailable)
			{
				Debug.LogWarning("[CielOSWindowManager] App '" + def.appId + "' is unavailable on this platform or progression-locked.", this);
				return null;
			}
			if (def.launchesExternalScene)
			{
				if (CielOSAppLauncher.Instance != null)
				{
					CielOSAppLauncher.Instance.LaunchExternalScene(def.appId);
				}
				else
				{
					Debug.LogWarning("[CielOSWindowManager] App '" + def.appId + "' è external-launch ma CielOSAppLauncher.Instance è null.", this);
				}
				return null;
			}
			CielOSWindow cielOSWindow;
			if (def.singleInstance && this._singleInstanceByApp.TryGetValue(def.appId, out cielOSWindow) && cielOSWindow != null)
			{
				if (cielOSWindow.IsMinimized)
				{
					cielOSWindow.Restore();
				}
				else
				{
					cielOSWindow.BringToFront();
				}
				return cielOSWindow;
			}
			if (this.windowPrefab == null || this.windowLayer == null)
			{
				Debug.LogWarning("[CielOSWindowManager] windowPrefab or windowLayer not assigned.", this);
				return null;
			}
			CielOSWindow cielOSWindow2 = Object.Instantiate<CielOSWindow>(this.windowPrefab, this.windowLayer);
			cielOSWindow2.Rect.sizeDelta = def.defaultWindowSize;
			cielOSWindow2.Rect.anchoredPosition = this.firstSpawnPos + this.cascadeStep * (float)(this._spawnCount % 6);
			this._spawnCount++;
			cielOSWindow2.Initialize(this, def.displayName, def.icon, def.appId, def.allowResize, def.minWindowSize, def.openMaximized, true);
			if (def.windowContentPrefab != null && cielOSWindow2.Content != null)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(def.windowContentPrefab, cielOSWindow2.Content);
				CielOSWindowManager.StretchToParent(gameObject.transform as RectTransform);
				if (!gameObject.activeSelf)
				{
					gameObject.SetActive(true);
				}
			}
			cielOSWindow2.Closed += this.OnWindowClosed;
			this._windows.Add(cielOSWindow2);
			if (def.singleInstance)
			{
				this._singleInstanceByApp[def.appId] = cielOSWindow2;
			}
			cielOSWindow2.Open();
			Action<CielOSWindow> windowOpened = this.WindowOpened;
			if (windowOpened != null)
			{
				windowOpened(cielOSWindow2);
			}
			return cielOSWindow2;
		}

		// Token: 0x06009BA4 RID: 39844 RVA: 0x003F97A4 File Offset: 0x003F79A4
		public CielOSWindow LaunchTransientWindow(string title, Sprite icon, Vector2 size, string appId = null)
		{
			if (this.windowPrefab == null || this.windowLayer == null)
			{
				Debug.LogWarning("[CielOSWindowManager] windowPrefab or windowLayer not assigned.", this);
				return null;
			}
			CielOSWindow cielOSWindow = Object.Instantiate<CielOSWindow>(this.windowPrefab, this.windowLayer);
			if (size.x > 1f && size.y > 1f)
			{
				cielOSWindow.Rect.sizeDelta = size;
			}
			cielOSWindow.Rect.anchoredPosition = this.firstSpawnPos + this.cascadeStep * (float)(this._spawnCount % 6);
			this._spawnCount++;
			cielOSWindow.Initialize(this, title, icon, appId, true, default(Vector2), false, false);
			cielOSWindow.Closed += this.OnWindowClosed;
			this._windows.Add(cielOSWindow);
			cielOSWindow.Open();
			Action<CielOSWindow> windowOpened = this.WindowOpened;
			if (windowOpened != null)
			{
				windowOpened(cielOSWindow);
			}
			return cielOSWindow;
		}

		// Token: 0x06009BA5 RID: 39845 RVA: 0x003F9898 File Offset: 0x003F7A98
		public void NotifyFocused(CielOSWindow window)
		{
			if (this._focused == window)
			{
				return;
			}
			if (this._focused != null)
			{
				this._focused.SetFocusedVisual(false);
			}
			this._focused = window;
			if (this._focused != null)
			{
				this._focused.SetFocusedVisual(true);
			}
		}

		// Token: 0x06009BA6 RID: 39846 RVA: 0x003F98F0 File Offset: 0x003F7AF0
		private void ShowSteamRequirement()
		{
			if (this._steamNotice != null)
			{
				if (this._steamNotice.IsMinimized)
				{
					this._steamNotice.Restore();
				}
				this._steamNotice.BringToFront();
				return;
			}
			if (this.windowPrefab == null || this.windowLayer == null)
			{
				return;
			}
			this._steamNotice = this.LaunchTransientWindow(GameLocalization.GetSource("leaderboard_ui", "Steam required"), null, new Vector2(480f, 200f), "leaderboard_steam_notice");
			if (this._steamNotice == null || this._steamNotice.Content == null)
			{
				return;
			}
			GameObject gameObject = new GameObject("SteamRequirement", new Type[]
			{
				typeof(RectTransform),
				typeof(TextMeshProUGUI)
			});
			gameObject.transform.SetParent(this._steamNotice.Content, false);
			RectTransform rectTransform = (RectTransform)gameObject.transform;
			CielOSWindowManager.StretchToParent(rectTransform);
			rectTransform.offsetMin = new Vector2(24f, 16f);
			rectTransform.offsetMax = new Vector2(-24f, -16f);
			TextMeshProUGUI component = gameObject.GetComponent<TextMeshProUGUI>();
			component.fontSize = 20f;
			component.color = Color.white;
			component.alignment = TextAlignmentOptions.Center;
			component.enableWordWrapping = true;
			component.raycastTarget = false;
			LocalizedGameText.BindSource(component, "leaderboard_ui", "Leaderboards are only available when playing on Steam.", null).FitTranslationToRectangle(false);
		}

		// Token: 0x170018C8 RID: 6344
		// (get) Token: 0x06009BA7 RID: 39847 RVA: 0x003F9A63 File Offset: 0x003F7C63
		public IReadOnlyList<CielOSWindow> Windows
		{
			get
			{
				return this._windows;
			}
		}

		// Token: 0x170018C9 RID: 6345
		// (get) Token: 0x06009BA8 RID: 39848 RVA: 0x003F9A6C File Offset: 0x003F7C6C
		public CielOSWindow FocusedWindow
		{
			get
			{
				if (this._focused != null && this._focused.IsOpen && !this._focused.IsMinimized && this._focused.gameObject.activeInHierarchy)
				{
					return this._focused;
				}
				CielOSWindow cielOSWindow = null;
				foreach (CielOSWindow cielOSWindow2 in this._windows)
				{
					if (cielOSWindow2 != null && cielOSWindow2.IsOpen && !cielOSWindow2.IsMinimized && cielOSWindow2.gameObject.activeInHierarchy && (cielOSWindow == null || cielOSWindow2.transform.GetSiblingIndex() > cielOSWindow.transform.GetSiblingIndex()))
					{
						cielOSWindow = cielOSWindow2;
					}
				}
				return cielOSWindow;
			}
		}

		// Token: 0x06009BA9 RID: 39849 RVA: 0x003F9B48 File Offset: 0x003F7D48
		private void OnWindowClosed(CielOSWindow window)
		{
			window.Closed -= this.OnWindowClosed;
			this._windows.Remove(window);
			string text = null;
			foreach (KeyValuePair<string, CielOSWindow> keyValuePair in this._singleInstanceByApp)
			{
				if (keyValuePair.Value == window)
				{
					text = keyValuePair.Key;
					break;
				}
			}
			if (text != null)
			{
				this._singleInstanceByApp.Remove(text);
			}
			if (this._focused == window)
			{
				this._focused = null;
			}
			Action<CielOSWindow> windowClosed = this.WindowClosed;
			if (windowClosed == null)
			{
				return;
			}
			windowClosed(window);
		}

		// Token: 0x06009BAA RID: 39850 RVA: 0x003F9C04 File Offset: 0x003F7E04
		public bool CanCloseAll()
		{
			foreach (CielOSWindow cielOSWindow in this._windows)
			{
				if (cielOSWindow != null && !cielOSWindow.CanClose())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06009BAB RID: 39851 RVA: 0x003F9C68 File Offset: 0x003F7E68
		public void CloseAll()
		{
			foreach (CielOSWindow cielOSWindow in new List<CielOSWindow>(this._windows))
			{
				if (cielOSWindow != null)
				{
					cielOSWindow.Close();
				}
			}
			this._windows.RemoveAll((CielOSWindow window) => window == null);
			if (this._windows.Count == 0)
			{
				this._singleInstanceByApp.Clear();
				this._focused = null;
				this._spawnCount = 0;
			}
		}

		// Token: 0x06009BAC RID: 39852 RVA: 0x003F9D1C File Offset: 0x003F7F1C
		private static void StretchToParent(RectTransform rt)
		{
			if (rt == null)
			{
				return;
			}
			rt.anchorMin = Vector2.zero;
			rt.anchorMax = Vector2.one;
			rt.offsetMin = Vector2.zero;
			rt.offsetMax = Vector2.zero;
			rt.localScale = Vector3.one;
		}

		// Token: 0x0400A05D RID: 41053
		[Header("References")]
		[Tooltip("Theme used for window focus visuals.")]
		[SerializeField]
		private CielOSTheme theme;

		// Token: 0x0400A05E RID: 41054
		[Tooltip("Catalog of installed apps.")]
		[SerializeField]
		private CielOSAppCatalogSO catalog;

		// Token: 0x0400A05F RID: 41055
		[Tooltip("Prefab of an empty CielOSWindow (chrome only); app content is mounted inside.")]
		[SerializeField]
		private CielOSWindow windowPrefab;

		// Token: 0x0400A060 RID: 41056
		[Tooltip("Parent under which windows are instantiated (the window layer).")]
		[SerializeField]
		private RectTransform windowLayer;

		// Token: 0x0400A061 RID: 41057
		[Header("Spawn")]
		[Tooltip("Offset each new window by this many pixels so they cascade.")]
		[SerializeField]
		private Vector2 cascadeStep = new Vector2(36f, -36f);

		// Token: 0x0400A062 RID: 41058
		[Tooltip("Anchored position of the first window.")]
		[SerializeField]
		private Vector2 firstSpawnPos = new Vector2(80f, -80f);

		// Token: 0x0400A065 RID: 41061
		private readonly List<CielOSWindow> _windows = new List<CielOSWindow>();

		// Token: 0x0400A066 RID: 41062
		private readonly Dictionary<string, CielOSWindow> _singleInstanceByApp = new Dictionary<string, CielOSWindow>();

		// Token: 0x0400A067 RID: 41063
		private CielOSWindow _focused;

		// Token: 0x0400A068 RID: 41064
		private int _spawnCount;

		// Token: 0x0400A069 RID: 41065
		private CielOSWindow _steamNotice;
	}
}
