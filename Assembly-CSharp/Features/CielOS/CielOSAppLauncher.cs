using System;
using System.Collections.Generic;
using Features.CielOS.Apps.Story;
using Features.Home;
using Features.RunModes;
using ProjectExhibitionist.UI;
using UnityEngine;

namespace Features.CielOS
{
	// Token: 0x02001372 RID: 4978
	public class CielOSAppLauncher : MonoBehaviour
	{
		// Token: 0x17001891 RID: 6289
		// (get) Token: 0x06009A27 RID: 39463 RVA: 0x003F169E File Offset: 0x003EF89E
		// (set) Token: 0x06009A28 RID: 39464 RVA: 0x003F16A5 File Offset: 0x003EF8A5
		public static CielOSAppLauncher Instance { get; private set; }

		// Token: 0x06009A29 RID: 39465 RVA: 0x003F16AD File Offset: 0x003EF8AD
		private void Awake()
		{
			if (CielOSAppLauncher.Instance != null && CielOSAppLauncher.Instance != this)
			{
				Object.Destroy(this);
				return;
			}
			CielOSAppLauncher.Instance = this;
		}

		// Token: 0x06009A2A RID: 39466 RVA: 0x003F16D6 File Offset: 0x003EF8D6
		private void OnDestroy()
		{
			if (CielOSAppLauncher.Instance == this)
			{
				CielOSAppLauncher.Instance = null;
			}
		}

		// Token: 0x06009A2B RID: 39467 RVA: 0x003F16EC File Offset: 0x003EF8EC
		public void LaunchExternalScene(string appId)
		{
			CielOSAppLauncher.ExternalSceneBinding externalSceneBinding = this.externalSceneBindings.Find((CielOSAppLauncher.ExternalSceneBinding b) => b != null && b.appId == appId);
			if (externalSceneBinding == null)
			{
				Debug.LogWarning("[CielOSAppLauncher] No external-scene binding for app id '" + appId + "'.", this);
				return;
			}
			if (this.homeManager == null)
			{
				Debug.LogWarning("[CielOSAppLauncher] HomeManager reference is missing; cannot open customization scene.", this);
				return;
			}
			if (externalSceneBinding.target == CielOSAppLauncher.ExternalSceneTarget.StartGame)
			{
				if (this._preparingRun)
				{
					return;
				}
				string text;
				if (!this.homeManager.TryStartGame(new Action(this.CloseDesktopForRun), out text) && !string.IsNullOrEmpty(text))
				{
					ToastService.Show(text, "Start run", null, -1f, "cielos_settings");
				}
				return;
			}
			else if (externalSceneBinding.target == CielOSAppLauncher.ExternalSceneTarget.InPlaceCustomization)
			{
				CielOSCustomizationTakeover cielOSCustomizationTakeover = ((this.customizationTakeover != null) ? this.customizationTakeover : Object.FindObjectOfType<CielOSCustomizationTakeover>(true));
				if (cielOSCustomizationTakeover == null)
				{
					Debug.LogWarning("[CielOSAppLauncher] Nessun CielOSCustomizationTakeover in scena.", this);
					return;
				}
				cielOSCustomizationTakeover.Open();
				return;
			}
			else
			{
				CielOSSession.MarkOpenBeforeSceneChange(appId);
				switch (externalSceneBinding.target)
				{
				case CielOSAppLauncher.ExternalSceneTarget.MainCharacterCustomization:
					this.homeManager.OpenMainCharacterCustomization();
					return;
				case CielOSAppLauncher.ExternalSceneTarget.CustomizationHub:
					this.homeManager.OpenCustomizationHub();
					return;
				case CielOSAppLauncher.ExternalSceneTarget.NpcCustomization:
					this.homeManager.OpenNpcCustomization(externalSceneBinding.npcCharacterId);
					return;
				default:
					return;
				}
			}
		}

		// Token: 0x06009A2C RID: 39468 RVA: 0x003F1835 File Offset: 0x003EFA35
		public bool StartRun(RunModeKind mode, FreeRunSettings settings, out string error)
		{
			return this.StartRun(mode, settings, ClassicRunKind.Normal, out error);
		}

		// Token: 0x06009A2D RID: 39469 RVA: 0x003F1844 File Offset: 0x003EFA44
		public bool StartRun(RunModeKind mode, FreeRunSettings settings, ClassicRunKind classicRunKind, out string error)
		{
			if (this._preparingRun)
			{
				error = "A run is already being prepared.";
				return false;
			}
			if (this.homeManager == null)
			{
				error = "HomeManager reference is missing.";
				return false;
			}
			if (!this.homeManager.CanStartGame(mode == RunModeKind.LongWayHome, out error))
			{
				return false;
			}
			if (!CielOSStoryFlow.BeforeStartRun(out error))
			{
				return false;
			}
			this._preparingRun = true;
			bool flag;
			try
			{
				if (!RunModeService.TryPrepareRun(mode, settings, classicRunKind, out error))
				{
					flag = false;
				}
				else
				{
					flag = this.homeManager.TryStartGame(new Action(this.CloseDesktopForRun), out error);
				}
			}
			finally
			{
				this._preparingRun = false;
			}
			return flag;
		}

		// Token: 0x06009A2E RID: 39470 RVA: 0x003F18E8 File Offset: 0x003EFAE8
		private void CloseDesktopForRun()
		{
			CielOSController cielOSController = base.GetComponent<CielOSController>();
			if (cielOSController == null)
			{
				cielOSController = Object.FindObjectOfType<CielOSController>();
			}
			if (cielOSController != null)
			{
				cielOSController.Close();
			}
		}

		// Token: 0x04009F1D RID: 40733
		[Header("Scene Entry Points")]
		[Tooltip("HomeManager that owns the customization scene entry points.")]
		[SerializeField]
		private HomeManager homeManager;

		// Token: 0x04009F1E RID: 40734
		[Header("External Scene Bindings")]
		[Tooltip("Maps an app id to the customization entry point it opens.")]
		[SerializeField]
		private List<CielOSAppLauncher.ExternalSceneBinding> externalSceneBindings = new List<CielOSAppLauncher.ExternalSceneBinding>();

		// Token: 0x04009F1F RID: 40735
		[Header("In-Place Customization")]
		[Tooltip("Takeover della customization in-place (target InPlaceCustomization). Se nullo, cercato in scena.")]
		[SerializeField]
		private CielOSCustomizationTakeover customizationTakeover;

		// Token: 0x04009F20 RID: 40736
		private bool _preparingRun;

		// Token: 0x02001373 RID: 4979
		public enum ExternalSceneTarget
		{
			// Token: 0x04009F22 RID: 40738
			MainCharacterCustomization,
			// Token: 0x04009F23 RID: 40739
			CustomizationHub,
			// Token: 0x04009F24 RID: 40740
			NpcCustomization,
			// Token: 0x04009F25 RID: 40741
			StartGame,
			// Token: 0x04009F26 RID: 40742
			InPlaceCustomization
		}

		// Token: 0x02001374 RID: 4980
		[Serializable]
		public class ExternalSceneBinding
		{
			// Token: 0x04009F27 RID: 40743
			[Tooltip("appId of the app this binding launches.")]
			public string appId;

			// Token: 0x04009F28 RID: 40744
			[Tooltip("Which customization entry point to open.")]
			public CielOSAppLauncher.ExternalSceneTarget target;

			// Token: 0x04009F29 RID: 40745
			[Tooltip("For NpcCustomization: the NPC character id to pass.")]
			public string npcCharacterId = string.Empty;
		}
	}
}
