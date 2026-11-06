using System;
using Features.Flash;
using Features.Slip;
using Features.Vehicles.Traffic;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.CoverWall;
using ProjectExhibitionist.Interactions;
using UnityEngine;

namespace Features.TowelSystem
{
	// Token: 0x02000C03 RID: 3075
	public class TowelActionsUI : MonoBehaviour
	{
		// Token: 0x060058CC RID: 22732 RVA: 0x0022BCE8 File Offset: 0x00229EE8
		private void Awake()
		{
			if (this.flashController == null)
			{
				this.flashController = base.GetComponent<ClothFullFlashController>();
			}
			if (this.slipController == null)
			{
				this.slipController = base.GetComponent<ClothSlipController>();
			}
			if (this.pickupController == null)
			{
				this.pickupController = base.GetComponent<TowelPickupController>();
			}
		}

		// Token: 0x060058CD RID: 22733 RVA: 0x0022BD43 File Offset: 0x00229F43
		private void OnDisable()
		{
			this._mountedPrompt.Hide();
			if (this._shown != null && InteractionHUDController.Instance != null)
			{
				InteractionHUDController.Instance.HideIfCurrent(this._shown);
			}
			this._shown = null;
		}

		// Token: 0x060058CE RID: 22734 RVA: 0x0022BD83 File Offset: 0x00229F83
		private void OnDestroy()
		{
			this._mountedPrompt.Dispose();
		}

		// Token: 0x060058CF RID: 22735 RVA: 0x0022BD90 File Offset: 0x00229F90
		private void LateUpdate()
		{
			InteractionHUDController instance = InteractionHUDController.Instance;
			if (instance == null)
			{
				return;
			}
			if (PlayerActivityGate.IsResourceOwned(PlayerContextResource.Rider) && !CarRoofCarrier.IsCarSurfing(base.transform))
			{
				instance.HideIfCurrent(this._shown);
				this._shown = null;
				this._mountedPrompt.Show(this, instance, this.wornConfig, "Drop Towel", this.slipController != null && this.slipController.CanSlipLand(this.wornPieceName));
				return;
			}
			this._mountedPrompt.Hide();
			InteractionConfigSO interactionConfigSO = this.ComputeDesiredConfig();
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

		// Token: 0x060058D0 RID: 22736 RVA: 0x0022BE88 File Offset: 0x0022A088
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
			if (!(this.slipController != null) || !this.slipController.HasSlippedPiece(this.wornPieceName))
			{
				if (this.flashController != null && this.flashController.IsOpen)
				{
					return this.openConfig;
				}
				if (this.flashController != null && this.flashController.IsWorn)
				{
					return this.wornConfig;
				}
				return null;
			}
			else
			{
				if (this.pickupController != null && this.pickupController.CanPickUp)
				{
					return this.pickupConfig;
				}
				return null;
			}
		}

		// Token: 0x04005959 RID: 22873
		[Header("Controllers (auto-found if empty)")]
		[Tooltip("Towel-flash full-body sul layer ClothFlashFull: lo stato open/worn si legge da qui.")]
		[SerializeField]
		private ClothFullFlashController flashController;

		// Token: 0x0400595A RID: 22874
		[SerializeField]
		private ClothSlipController slipController;

		// Token: 0x0400595B RID: 22875
		[Tooltip("Worn-mesh name della piece towel nel ClothSlipController.")]
		[SerializeField]
		private string wornPieceName = "Bath_Towel";

		// Token: 0x0400595C RID: 22876
		[SerializeField]
		private TowelPickupController pickupController;

		// Token: 0x0400595D RID: 22877
		[Header("HUD Configs")]
		[Tooltip("Shown while the towel is worn: Open Towel / Drop Towel.")]
		[SerializeField]
		private InteractionConfigSO wornConfig;

		// Token: 0x0400595E RID: 22878
		[Tooltip("Shown while flashing: Close Towel.")]
		[SerializeField]
		private InteractionConfigSO openConfig;

		// Token: 0x0400595F RID: 22879
		[Tooltip("Shown near the fallen towel: Pick Up Towel.")]
		[SerializeField]
		private InteractionConfigSO pickupConfig;

		// Token: 0x04005960 RID: 22880
		private InteractionConfigSO _shown;

		// Token: 0x04005961 RID: 22881
		private readonly MountedClothSlipPrompt _mountedPrompt = new MountedClothSlipPrompt();
	}
}
