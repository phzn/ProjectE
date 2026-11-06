using System;
using Features.Detection;
using Features.Flash;
using Features.HideFlash;
using Features.InsertableToys;
using Features.Inventory;
using Features.PhoneCompanion;
using Features.PlayerEmotes;
using Features.Police;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using UnityEngine;

namespace PoolWaterTools
{
	// Token: 0x02000727 RID: 1831
	public static class PoolWaterEntryPreparation
	{
		// Token: 0x06002F6B RID: 12139 RVA: 0x0010C19C File Offset: 0x0010A39C
		public static bool CanReleaseOwner(Object owner, Transform player)
		{
			Component component = owner as Component;
			return component != null && !(player == null) && (!(component.transform != player) || component.transform.IsChildOf(player)) && (owner is HideFlashController || owner is GameplayEmoteController || owner is ArmsFlashController || owner is ClothFullFlashController);
		}

		// Token: 0x06002F6C RID: 12140 RVA: 0x0010C200 File Offset: 0x0010A400
		public static bool IsSwimming(Transform player)
		{
			if (player == null)
			{
				return false;
			}
			vSwimming vSwimming = player.GetComponentInParent<vSwimming>() ?? player.GetComponentInChildren<vSwimming>(true);
			return vSwimming != null && (vSwimming.inTheWater || vSwimming.isSwimming);
		}

		// Token: 0x06002F6D RID: 12141 RVA: 0x0010C248 File Offset: 0x0010A448
		public static void Prepare(Transform player)
		{
			if (player == null)
			{
				return;
			}
			vThirdPersonController vThirdPersonController = player.GetComponentInParent<vThirdPersonController>() ?? player.GetComponentInChildren<vThirdPersonController>(true);
			if (vThirdPersonController != null)
			{
				player = vThirdPersonController.transform;
			}
			PlayerStunExecutor[] componentsInChildren = player.GetComponentsInChildren<PlayerStunExecutor>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].CancelForWaterEntry();
			}
			SmartphoneController.InterruptForDetection();
			foreach (GameplayEmoteController gameplayEmoteController in player.GetComponentsInChildren<GameplayEmoteController>(true))
			{
				if (gameplayEmoteController.CurrentState != GameplayEmoteController.EmoteState.Idle)
				{
					gameplayEmoteController.ForceEndEmoteImmediate();
				}
			}
			FlashGateRegistry.InterruptForEncounter(player);
			foreach (CoverPoseController coverPoseController in player.GetComponentsInChildren<CoverPoseController>(true))
			{
				if (coverPoseController.IsCovering)
				{
					coverPoseController.DeactivateCoverPose();
				}
			}
			foreach (PlayerActionAnimator playerActionAnimator in player.GetComponentsInChildren<PlayerActionAnimator>(true))
			{
				if (playerActionAnimator.GenericAction != null && playerActionAnimator.GenericAction.doingAction)
				{
					playerActionAnimator.CancelCurrentAction();
				}
			}
			InsertableToyController[] componentsInChildren5 = player.GetComponentsInChildren<InsertableToyController>(true);
			for (int i = 0; i < componentsInChildren5.Length; i++)
			{
				componentsInChildren5[i].PrepareForWaterEntry();
			}
			PlayerInventoryManager[] componentsInChildren6 = player.GetComponentsInChildren<PlayerInventoryManager>(true);
			for (int i = 0; i < componentsInChildren6.Length; i++)
			{
				componentsInChildren6[i].StowHeldItemForInteraction();
			}
			if (vThirdPersonController != null)
			{
				vThirdPersonController.isCrouching = false;
			}
		}
	}
}
