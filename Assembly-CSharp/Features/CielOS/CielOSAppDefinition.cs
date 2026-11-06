using System;
using Features.Rank;
using Features.RunModes;
using UnityEngine;

namespace Features.CielOS
{
	// Token: 0x02001371 RID: 4977
	[CreateAssetMenu(fileName = "CielOSApp", menuName = "CielOS/App Definition", order = 0)]
	public class CielOSAppDefinition : ScriptableObject
	{
		// Token: 0x1700188D RID: 6285
		// (get) Token: 0x06009A22 RID: 39458 RVA: 0x003F1563 File Offset: 0x003EF763
		public bool IsPlatformSupported
		{
			get
			{
				return !this.requiresSteamLeaderboard || RankedLeaderboardService.IsSupported;
			}
		}

		// Token: 0x1700188E RID: 6286
		// (get) Token: 0x06009A23 RID: 39459 RVA: 0x003F1574 File Offset: 0x003EF774
		public bool IsAvailable
		{
			get
			{
				return this.IsPlatformSupported && this.IsUnlocked;
			}
		}

		// Token: 0x1700188F RID: 6287
		// (get) Token: 0x06009A24 RID: 39460 RVA: 0x003F1586 File Offset: 0x003EF786
		public bool IsVisible
		{
			get
			{
				return this.IsUnlocked;
			}
		}

		// Token: 0x17001890 RID: 6288
		// (get) Token: 0x06009A25 RID: 39461 RVA: 0x003F1590 File Offset: 0x003EF790
		public bool IsUnlocked
		{
			get
			{
				if (string.IsNullOrWhiteSpace(this.minimumRankId) || RankProgressionService.IsAdminRankActive)
				{
					return true;
				}
				RankProgressionConfigSO config = RankProgressionService.Config;
				if (config == null || config.ranksInOrder == null)
				{
					return true;
				}
				int num = -1;
				for (int i = 0; i < config.ranksInOrder.Count; i++)
				{
					RankDefinitionSO rankDefinitionSO = config.ranksInOrder[i];
					if (rankDefinitionSO != null && rankDefinitionSO.rankId == this.minimumRankId)
					{
						num = i;
						break;
					}
				}
				return num < 0 || RankProgressionService.RankIndex >= num;
			}
		}

		// Token: 0x04009F0B RID: 40715
		[Header("Identity")]
		[Tooltip("Stable unique id, e.g. 'terminal', 'customize_ciel'.")]
		public string appId = "app_id";

		// Token: 0x04009F0C RID: 40716
		[Tooltip("Window title and menu/desktop label.")]
		public string displayName = "App";

		// Token: 0x04009F0D RID: 40717
		[Tooltip("Icon for the desktop, app menu, taskbar and window title bar.")]
		public Sprite icon;

		// Token: 0x04009F0E RID: 40718
		[Tooltip("Marks first-party game apps (Customization, Run Modes, etc.) rather than simulated system utilities. It controls coloured icon styling and the default Quick Tools set; desktop visibility is controlled independently by showOnDesktop.")]
		public bool isCustomApp;

		// Token: 0x04009F0F RID: 40719
		[Header("Progression")]
		[Tooltip("Optional minimum RankDefinitionSO.rankId required before this app is installed/visible. Unknown ids fail open so a configuration typo cannot permanently hide an app.")]
		public string minimumRankId = string.Empty;

		// Token: 0x04009F10 RID: 40720
		[Header("Platform")]
		[Tooltip("Lock this app when the Steam leaderboard integration is absent. A disconnected Steam session can still open the app to see its offline state.")]
		public bool requiresSteamLeaderboard;

		// Token: 0x04009F11 RID: 40721
		[Header("External Launch")]
		[Tooltip("Se true, lanciare quest'app NON apre una finestra: invoca direttamente CielOSAppLauncher.LaunchExternalScene(appId) (es. app 'Start Run' che avvia la partita, o app di customization che caricano una scena esterna). Richiede una ExternalSceneBinding con lo stesso appId sul CielOSAppLauncher di scena.")]
		public bool launchesExternalScene;

		// Token: 0x04009F12 RID: 40722
		[Header("Window")]
		[Tooltip("Prefab instantiated into the window content area when launched. Root may carry the app's controller component.")]
		public GameObject windowContentPrefab;

		// Token: 0x04009F13 RID: 40723
		[Tooltip("Initial window size in reference-resolution pixels.")]
		public Vector2 defaultWindowSize = new Vector2(720f, 460f);

		// Token: 0x04009F14 RID: 40724
		[Tooltip("Smallest size the adaptive app supports when resized.")]
		public Vector2 minWindowSize = new Vector2(320f, 200f);

		// Token: 0x04009F15 RID: 40725
		[Tooltip("Allow the player to resize this app window from its bottom-right grip.")]
		public bool allowResize = true;

		// Token: 0x04009F16 RID: 40726
		[Tooltip("Open at the full WindowLayer size. Resizable apps can still be restored by dragging the grip.")]
		public bool openMaximized;

		// Token: 0x04009F17 RID: 40727
		[Tooltip("Allow only one window of this app at a time (focus the existing one instead of opening a second).")]
		public bool singleInstance = true;

		// Token: 0x04009F18 RID: 40728
		[Header("Placement")]
		[Tooltip("Show an icon for this app on the desktop.")]
		public bool showOnDesktop = true;

		// Token: 0x04009F19 RID: 40729
		[Tooltip("Show this app in the panel application menu.")]
		public bool showInAppMenu = true;

		// Token: 0x04009F1A RID: 40730
		[Tooltip("Hide everywhere (work in progress).")]
		public bool hidden;

		// Token: 0x04009F1B RID: 40731
		[Tooltip("DEFAULT di fabbrica per la presenza nei Quick Tools. Il player può comunque pinnare/spinnare le app a runtime: la scelta effettiva è salvata in PlayerPrefs (vedi CielOSQuickToolsPrefs) e ha la precedenza su questo flag. Se il player non ha mai toccato un'app, vale questo default.")]
		public bool showInQuickTools;
	}
}
