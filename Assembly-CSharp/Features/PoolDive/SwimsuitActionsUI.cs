using System;
using System.Collections.Generic;
using Features.Flash;
using Features.Slip;
using Features.Vehicles.Traffic;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.CoverWall;
using ProjectExhibitionist.Interactions;
using UnityEngine;

namespace Features.PoolDive
{
	// Token: 0x02000DDD RID: 3549
	public class SwimsuitActionsUI : MonoBehaviour
	{
		// Token: 0x0600690E RID: 26894 RVA: 0x002A0798 File Offset: 0x0029E998
		private void Awake()
		{
			if (this.flashController == null)
			{
				foreach (ArmsFlashController armsFlashController in base.GetComponents<ArmsFlashController>())
				{
					if (armsFlashController.Config != null && armsFlashController.Config.flashLayerName == this.flashLayerName)
					{
						this.flashController = armsFlashController;
						break;
					}
				}
			}
			if (this.slipController == null)
			{
				this.slipController = base.GetComponent<ClothSlipController>();
			}
			if (this.diveController == null)
			{
				this.diveController = base.GetComponent<PoolDiveController>();
			}
			if (this.pickupController == null)
			{
				this.pickupController = base.GetComponent<SwimsuitPickupController>();
			}
			if (this.swimsuitBridge == null)
			{
				this.swimsuitBridge = base.GetComponent<SwimsuitClothBridge>();
			}
		}

		// Token: 0x0600690F RID: 26895 RVA: 0x002A0860 File Offset: 0x0029EA60
		private void OnDisable()
		{
			this._mountedPrompt.Hide();
			this.SetDivePrompt(null, null);
			if (this._shown != null && InteractionHUDController.Instance != null)
			{
				InteractionHUDController.Instance.HideIfCurrent(this._shown);
			}
			this._shown = null;
		}

		// Token: 0x06006910 RID: 26896 RVA: 0x002A08B4 File Offset: 0x0029EAB4
		private void OnDestroy()
		{
			this._mountedPrompt.Dispose();
			if (this._diveOnlyConfig == null)
			{
				return;
			}
			if (Application.isPlaying)
			{
				Object.Destroy(this._diveOnlyConfig);
			}
			else
			{
				Object.DestroyImmediate(this._diveOnlyConfig);
			}
			this._diveOnlyConfig = null;
		}

		// Token: 0x06006911 RID: 26897 RVA: 0x002A0904 File Offset: 0x0029EB04
		private void LateUpdate()
		{
			InteractionHUDController instance = InteractionHUDController.Instance;
			if (instance == null)
			{
				return;
			}
			if (PlayerActivityGate.IsResourceOwned(PlayerContextResource.Rider) && !CarRoofCarrier.IsCarSurfing(base.transform))
			{
				this.SetDivePrompt(null, null);
				instance.HideIfCurrent(this._shown);
				this._shown = null;
				bool flag = (this.swimsuitBridge == null || this.swimsuitBridge.AreSwimsuitActionsAvailable) && this.slipController != null && this.slipController.CanSlipLand(null);
				this._mountedPrompt.Show(this, instance, this.wornConfig, "Drop Swimsuit", flag);
				return;
			}
			this._mountedPrompt.Hide();
			InteractionConfigSO interactionConfigSO = this.ComputeDesiredConfig();
			bool flag2 = interactionConfigSO != null && (interactionConfigSO == this.diveConfig || interactionConfigSO == this._diveOnlyConfig);
			this.SetDivePrompt(flag2 ? instance : null, flag2 ? interactionConfigSO : null);
			if (flag2)
			{
				instance.HideIfCurrent(this._shown);
				this._shown = null;
				return;
			}
			if (interactionConfigSO == this._shown)
			{
				if (interactionConfigSO != null && instance.CurrentConfig == null)
				{
					instance.Show(interactionConfigSO);
				}
				return;
			}
			if (interactionConfigSO == null)
			{
				if (this._shown != null)
				{
					instance.HideIfCurrent(this._shown);
				}
			}
			else
			{
				instance.Show(interactionConfigSO);
			}
			this._shown = interactionConfigSO;
		}

		// Token: 0x06006912 RID: 26898 RVA: 0x002A0A70 File Offset: 0x0029EC70
		private void SetDivePrompt(InteractionHUDController hud, InteractionConfigSO config)
		{
			if (this._diveHud == hud && this._shownDive == config)
			{
				return;
			}
			if (this._diveHud != null)
			{
				this._diveHud.HideSupplemental(this);
			}
			this._diveHud = hud;
			this._shownDive = config;
			if (this._diveHud != null && config != null)
			{
				this._diveHud.ShowSupplemental(this, config);
			}
		}

		// Token: 0x06006913 RID: 26899 RVA: 0x002A0AE8 File Offset: 0x0029ECE8
		private InteractionConfigSO ComputeDesiredConfig()
		{
			if (WallCoverActivityLock.ActiveController != null)
			{
				return null;
			}
			if (this.slipController != null && this.slipController.IsSlipping)
			{
				return null;
			}
			if (this.diveController != null && this.diveController.IsDiving)
			{
				return null;
			}
			bool flag = this.swimsuitBridge == null || this.swimsuitBridge.AreSwimsuitActionsAvailable;
			if (flag && this.flashController != null && (this.flashController.IsOpen || this.flashController.IsBusy))
			{
				return this.openConfig;
			}
			if (flag && this.pickupController != null && this.pickupController.CanPickUp)
			{
				return this.pickupConfig;
			}
			if (this.diveController != null && this.diveController.CanDive)
			{
				return this.ResolveDiveConfig();
			}
			if (flag && this.flashController != null && this.flashController.IsWorn)
			{
				return this.wornConfig;
			}
			return null;
		}

		// Token: 0x06006914 RID: 26900 RVA: 0x002A0BFC File Offset: 0x0029EDFC
		private InteractionConfigSO ResolveDiveConfig()
		{
			if (this.diveConfig == null || this.diveController == null || this.diveController.CanStripOnDive)
			{
				return this.diveConfig;
			}
			if (this._diveOnlyConfig == null)
			{
				this._diveOnlyConfig = SwimsuitActionsUI.BuildDiveOnlyConfig(this.diveConfig);
			}
			return this._diveOnlyConfig;
		}

		// Token: 0x06006915 RID: 26901 RVA: 0x002A0C60 File Offset: 0x0029EE60
		private static InteractionConfigSO BuildDiveOnlyConfig(InteractionConfigSO source)
		{
			if (source == null)
			{
				return null;
			}
			InteractionConfigSO interactionConfigSO = ScriptableObject.CreateInstance<InteractionConfigSO>();
			interactionConfigSO.name = source.name + "_DiveOnly_Runtime";
			interactionConfigSO.hideFlags = HideFlags.HideAndDontSave;
			interactionConfigSO.objectName = source.objectName;
			interactionConfigSO.objectNameColor = source.objectNameColor;
			interactionConfigSO.mainActionName = source.mainActionName;
			interactionConfigSO.mainKeyDisplayLabel = source.mainKeyDisplayLabel;
			interactionConfigSO.secondaryActionName = source.secondaryActionName;
			interactionConfigSO.secondaryKeyDisplayLabel = source.secondaryKeyDisplayLabel;
			interactionConfigSO.usableActions = new List<UsableAction>();
			UsableAction usableAction = null;
			if (source.usableActions != null)
			{
				for (int i = 0; i < source.usableActions.Count; i++)
				{
					UsableAction usableAction2 = source.usableActions[i];
					if (usableAction2 != null && string.Equals(usableAction2.keyDisplayLabel, "F", StringComparison.OrdinalIgnoreCase))
					{
						usableAction = usableAction2;
						break;
					}
				}
			}
			List<UsableAction> usableActions = interactionConfigSO.usableActions;
			UsableAction usableAction4;
			if (usableAction == null)
			{
				UsableAction usableAction3 = new UsableAction();
				usableAction3.actionName = "Dive";
				usableAction3.keyDisplayLabel = "F";
				usableAction4 = usableAction3;
				usableAction3.description = string.Empty;
			}
			else
			{
				UsableAction usableAction5 = new UsableAction();
				usableAction5.actionName = usableAction.actionName;
				usableAction5.keyDisplayLabel = usableAction.keyDisplayLabel;
				usableAction4 = usableAction5;
				usableAction5.description = usableAction.description;
			}
			usableActions.Add(usableAction4);
			return interactionConfigSO;
		}

		// Token: 0x04006BAE RID: 27566
		[Header("Controllers (auto-found if empty)")]
		[Tooltip("Generic arms-flash controller for the swimsuit (config.flashLayerName == flashLayerName). Auto-found.")]
		[SerializeField]
		private ArmsFlashController flashController;

		// Token: 0x04006BAF RID: 27567
		[SerializeField]
		private string flashLayerName = "SwimsuitFlash";

		// Token: 0x04006BB0 RID: 27568
		[SerializeField]
		private ClothSlipController slipController;

		// Token: 0x04006BB1 RID: 27569
		[SerializeField]
		private PoolDiveController diveController;

		// Token: 0x04006BB2 RID: 27570
		[SerializeField]
		private SwimsuitPickupController pickupController;

		// Token: 0x04006BB3 RID: 27571
		[SerializeField]
		private SwimsuitClothBridge swimsuitBridge;

		// Token: 0x04006BB4 RID: 27572
		[Header("HUD Configs")]
		[Tooltip("Shown while standing in a dive zone: Dive / Dive & Strip.")]
		[SerializeField]
		private InteractionConfigSO diveConfig;

		// Token: 0x04006BB5 RID: 27573
		[Tooltip("Shown while the suit is worn (out of water): Flash / Drop Swimsuit.")]
		[SerializeField]
		private InteractionConfigSO wornConfig;

		// Token: 0x04006BB6 RID: 27574
		[Tooltip("Shown while flashing: Cover Up.")]
		[SerializeField]
		private InteractionConfigSO openConfig;

		// Token: 0x04006BB7 RID: 27575
		[Tooltip("Shown near the fallen swimsuit: Pick Up Swimsuit.")]
		[SerializeField]
		private InteractionConfigSO pickupConfig;

		// Token: 0x04006BB8 RID: 27576
		private InteractionConfigSO _shown;

		// Token: 0x04006BB9 RID: 27577
		private InteractionConfigSO _diveOnlyConfig;

		// Token: 0x04006BBA RID: 27578
		private InteractionHUDController _diveHud;

		// Token: 0x04006BBB RID: 27579
		private InteractionConfigSO _shownDive;

		// Token: 0x04006BBC RID: 27580
		private readonly MountedClothSlipPrompt _mountedPrompt = new MountedClothSlipPrompt();
	}
}
