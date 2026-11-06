using System;
using BoZo.CharacterCreator.MC.StyleSystem;
using Features.BodyPaintStudio;
using Features.Inventory;
using ProjectExhibitionist.CharacterCustomization.Core.Runtime;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Interactions;
using UnityEngine;

namespace Features.NeckSigns
{
	// Token: 0x02000F72 RID: 3954
	[DisallowMultipleComponent]
	public sealed class NeckSignActionsUI : MonoBehaviour
	{
		// Token: 0x06007703 RID: 30467 RVA: 0x0030630F File Offset: 0x0030450F
		public void Configure(MCStyleCustomizationService service)
		{
			this.styleService = service;
			this.ResolveOwner();
		}

		// Token: 0x06007704 RID: 30468 RVA: 0x0030631E File Offset: 0x0030451E
		private void Awake()
		{
			if (this.styleService == null)
			{
				this.styleService = base.GetComponent<MCStyleCustomizationService>();
			}
			this.ResolveOwner();
		}

		// Token: 0x06007705 RID: 30469 RVA: 0x00306340 File Offset: 0x00304540
		private void ResolveOwner()
		{
			if (this.styleService == null)
			{
				return;
			}
			this._appearance = this.styleService.GetComponentInParent<MainCharacterAppearanceApplier>();
			if (this._appearance != null)
			{
				this._mode = this._appearance.GetComponentInChildren<BodyPaintModeController>(true);
			}
		}

		// Token: 0x06007706 RID: 30470 RVA: 0x00306390 File Offset: 0x00304590
		public static bool ShouldShow(MCStyleCustomizationService service, string partId)
		{
			if (service == null || service.IsPreviewClothingHidden || !service.IsSlotEquipped(partId))
			{
				return false;
			}
			GameObject spawnedRoot = service.GetSpawnedRoot(partId);
			if (spawnedRoot == null || !spawnedRoot.activeInHierarchy)
			{
				return false;
			}
			NeckSignSurface componentInChildren = spawnedRoot.GetComponentInChildren<NeckSignSurface>();
			return componentInChildren != null && componentInChildren.isActiveAndEnabled && componentInChildren.TargetRenderer != null && componentInChildren.TargetRenderer.enabled && componentInChildren.TargetRenderer.gameObject.activeInHierarchy;
		}

		// Token: 0x06007707 RID: 30471 RVA: 0x0030641C File Offset: 0x0030461C
		private bool CanOfferActions()
		{
			if (this._mode == null || this._appearance == null)
			{
				this.ResolveOwner();
			}
			if (this._appearance == null || !this._appearance.UsesSavedMainCharacterPreset || this._mode == null || !this._mode.CanEnterMode())
			{
				return false;
			}
			PlayerInventoryManager instance = PlayerInventoryManager.Instance;
			return instance == null || instance.GetActiveItem() == null;
		}

		// Token: 0x06007708 RID: 30472 RVA: 0x003064A0 File Offset: 0x003046A0
		private void LateUpdate()
		{
			InteractionHUDController instance = InteractionHUDController.Instance;
			int num = 0;
			if (instance != null && this.CanOfferActions())
			{
				if (NeckSignActionsUI.ShouldShow(this.styleService, "Accessory.NeckTag"))
				{
					num |= 1;
				}
				if (NeckSignActionsUI.ShouldShow(this.styleService, "Accessory.NeckSign"))
				{
					num |= 2;
				}
			}
			if (this._hud != null && (this._hud != instance || num == 0))
			{
				this._hud.HideSupplemental(this);
				this._hud = null;
			}
			if (num == 0)
			{
				this._visibleMask = 0;
				return;
			}
			if (this._config == null)
			{
				this._config = ScriptableObject.CreateInstance<InteractionConfigSO>();
				this._config.name = "Neck sign paint actions";
				this._config.hideFlags = HideFlags.HideAndDontSave;
				ControllerContextBindings.SetLabel(this._config, "J", ControllerBindings.Label(KeyCode.J), false);
				ControllerContextBindings.SetLabel(this._config, "H", ControllerBindings.Label(KeyCode.H), false);
			}
			if (num != this._visibleMask || this._hud != instance)
			{
				this._config.usableActions.Clear();
				if ((num & 1) != 0)
				{
					this._config.usableActions.Add(new UsableAction
					{
						keyDisplayLabel = "J",
						actionName = "Paint neck tag"
					});
				}
				if ((num & 2) != 0)
				{
					this._config.usableActions.Add(new UsableAction
					{
						keyDisplayLabel = "H",
						actionName = "Paint hanging sign"
					});
				}
				this._visibleMask = num;
				this._hud = instance;
				this._hud.ShowSupplemental(this, this._config);
			}
			if (this._hud == null || !this._hud.IsShowing || ControllerInput.UiInputConsumedThisFrame)
			{
				return;
			}
			if ((num & 1) != 0 && this._hud.IsActionVisible(this._config, "J") && NeckSignActionsUI.Pressed(KeyCode.J))
			{
				this.TryPaint("Accessory.NeckTag");
				return;
			}
			if ((num & 2) != 0 && this._hud.IsActionVisible(this._config, "H") && NeckSignActionsUI.Pressed(KeyCode.H))
			{
				this.TryPaint("Accessory.NeckSign");
			}
		}

		// Token: 0x06007709 RID: 30473 RVA: 0x003066C8 File Offset: 0x003048C8
		public bool TryPaint(string partId)
		{
			if (!this.CanOfferActions() || !NeckSignActionsUI.ShouldShow(this.styleService, partId))
			{
				return false;
			}
			NeckSignSurface componentInChildren = this.styleService.GetSpawnedRoot(partId).GetComponentInChildren<NeckSignSurface>();
			if (!this._mode.TryEnterItemMode(componentInChildren))
			{
				return false;
			}
			if (ControllerInput.IsActive)
			{
				ControllerInput.ConsumeUIInput();
			}
			if (this._hud != null)
			{
				this._hud.HideSupplemental(this);
			}
			this._hud = null;
			this._visibleMask = 0;
			return true;
		}

		// Token: 0x0600770A RID: 30474 RVA: 0x00306744 File Offset: 0x00304944
		private static bool Pressed(KeyCode key)
		{
			if (!ControllerInput.IsActive)
			{
				return Input.GetKeyDown(key);
			}
			return ControllerGameplayBridge.AllowsAxes && ControllerInput.GetKeyDown(key);
		}

		// Token: 0x0600770B RID: 30475 RVA: 0x00306763 File Offset: 0x00304963
		private void OnDisable()
		{
			if (this._hud != null)
			{
				this._hud.HideSupplemental(this);
			}
			this._hud = null;
			this._visibleMask = -1;
		}

		// Token: 0x0600770C RID: 30476 RVA: 0x0030678D File Offset: 0x0030498D
		private void OnDestroy()
		{
			if (this._config == null)
			{
				return;
			}
			if (Application.isPlaying)
			{
				Object.Destroy(this._config);
				return;
			}
			Object.DestroyImmediate(this._config);
		}

		// Token: 0x04007A48 RID: 31304
		public const string TagPartId = "Accessory.NeckTag";

		// Token: 0x04007A49 RID: 31305
		public const string SignPartId = "Accessory.NeckSign";

		// Token: 0x04007A4A RID: 31306
		[SerializeField]
		private MCStyleCustomizationService styleService;

		// Token: 0x04007A4B RID: 31307
		private BodyPaintModeController _mode;

		// Token: 0x04007A4C RID: 31308
		private MainCharacterAppearanceApplier _appearance;

		// Token: 0x04007A4D RID: 31309
		private InteractionConfigSO _config;

		// Token: 0x04007A4E RID: 31310
		private InteractionHUDController _hud;

		// Token: 0x04007A4F RID: 31311
		private int _visibleMask = -1;
	}
}
