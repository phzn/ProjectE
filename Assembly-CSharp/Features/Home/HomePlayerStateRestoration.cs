using System;
using Dinopunch.LivingMud;
using Features.BodyPaintRain;
using Features.BodyPaintStudio;
using Features.Detection;
using Features.Flash;
using Features.FlexiPenis;
using Features.Inventory;
using Features.Inventory.Actions;
using Features.Inventory.Placement;
using Features.Pencil;
using Features.PlayerEmotes;
using Features.PoolDive;
using Features.TintBodyPaint;
using Invector.vCharacterController;
using Invector.vCover;
using PoolWaterTools;
using ProjectExhibitionist;
using ProjectExhibitionist.CharacterCustomization.Core.Presets;
using ProjectExhibitionist.CharacterCustomization.Core.Runtime;
using UnityEngine;

namespace Features.Home
{
	// Token: 0x0200119C RID: 4508
	public static class HomePlayerStateRestoration
	{
		// Token: 0x06008A04 RID: 35332 RVA: 0x00386570 File Offset: 0x00384770
		public static void Restore(Transform player, CustomizationPresetData preset)
		{
			if (player == null)
			{
				return;
			}
			DildoPlacementSystem[] componentsInChildren = player.GetComponentsInChildren<DildoPlacementSystem>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].InterruptForEncounter();
			}
			FlexiWallHeldActions[] componentsInChildren2 = player.GetComponentsInChildren<FlexiWallHeldActions>(true);
			for (int i = 0; i < componentsInChildren2.Length; i++)
			{
				componentsInChildren2[i].InterruptForEncounter();
			}
			FlashGateRegistry.InterruptForEncounter(player);
			vBasicCoverController componentInChildren = player.GetComponentInChildren<vBasicCoverController>(true);
			if (componentInChildren != null)
			{
				componentInChildren.InterruptForEncounter();
			}
			foreach (BodyPaintModeController bodyPaintModeController in player.GetComponentsInChildren<BodyPaintModeController>(true))
			{
				if (bodyPaintModeController.IsActive)
				{
					bodyPaintModeController.ExitMode();
				}
			}
			DildoActionExecutor[] componentsInChildren4 = player.GetComponentsInChildren<DildoActionExecutor>(true);
			for (int i = 0; i < componentsInChildren4.Length; i++)
			{
				componentsInChildren4[i].CancelAction();
			}
			foreach (GameplayEmoteController gameplayEmoteController in player.GetComponentsInChildren<GameplayEmoteController>(true))
			{
				if (gameplayEmoteController.CurrentState != GameplayEmoteController.EmoteState.Idle)
				{
					gameplayEmoteController.ForceEndEmoteImmediate();
				}
			}
			PlayerActionAnimator componentInChildren2 = player.GetComponentInChildren<PlayerActionAnimator>(true);
			if (componentInChildren2 != null)
			{
				componentInChildren2.CancelCurrentAction();
			}
			PoolPlayerStateRestoration.ResetForHomeReturn(player);
			CoverPoseController componentInChildren3 = player.GetComponentInChildren<CoverPoseController>(true);
			if (componentInChildren3 != null)
			{
				componentInChildren3.CancelPoseImmediate();
			}
			MudBodyPaintRuntimeController[] componentsInChildren6 = player.GetComponentsInChildren<MudBodyPaintRuntimeController>(true);
			for (int i = 0; i < componentsInChildren6.Length; i++)
			{
				componentsInChildren6[i].RemoveBodyPaintImmediate();
			}
			TintBodyPaintRuntimeController[] componentsInChildren7 = player.GetComponentsInChildren<TintBodyPaintRuntimeController>(true);
			for (int i = 0; i < componentsInChildren7.Length; i++)
			{
				componentsInChildren7[i].RemoveBodyPaintImmediate();
			}
			PencilBodyPaintUseController[] componentsInChildren8 = player.GetComponentsInChildren<PencilBodyPaintUseController>(true);
			for (int i = 0; i < componentsInChildren8.Length; i++)
			{
				componentsInChildren8[i].ResetForHomeReturn();
			}
			BodyPaintRainWashController[] componentsInChildren9 = player.GetComponentsInChildren<BodyPaintRainWashController>(true);
			for (int i = 0; i < componentsInChildren9.Length; i++)
			{
				componentsInChildren9[i].ResetForHomeReturn();
			}
			PoolWetOnExit[] componentsInChildren10 = player.GetComponentsInChildren<PoolWetOnExit>(true);
			for (int i = 0; i < componentsInChildren10.Length; i++)
			{
				componentsInChildren10[i].ResetForHomeReturn();
			}
			BodyPaintCanvasService[] componentsInChildren11 = player.GetComponentsInChildren<BodyPaintCanvasService>(true);
			for (int i = 0; i < componentsInChildren11.Length; i++)
			{
				componentsInChildren11[i].ClearAllPaint();
			}
			MudFootprintTrailController[] componentsInChildren12 = player.GetComponentsInChildren<MudFootprintTrailController>(true);
			for (int i = 0; i < componentsInChildren12.Length; i++)
			{
				componentsInChildren12[i].ClearResidueImmediate();
			}
			CielWetController[] componentsInChildren13 = player.GetComponentsInChildren<CielWetController>(true);
			for (int i = 0; i < componentsInChildren13.Length; i++)
			{
				componentsInChildren13[i].ResetForHomeReturn();
			}
			ShowerBubbleSpawner[] componentsInChildren14 = player.GetComponentsInChildren<ShowerBubbleSpawner>(true);
			for (int i = 0; i < componentsInChildren14.Length; i++)
			{
				componentsInChildren14[i].HideBubbles();
			}
			PlayerInventoryManager componentInChildren4 = player.GetComponentInChildren<PlayerInventoryManager>(true);
			if (componentInChildren4 != null)
			{
				componentInChildren4.ClearRunInventory();
			}
			HomePlayerStateRestoration.RestoreStandingState(player);
			MainCharacterAppearanceApplier componentInChildren5 = player.GetComponentInChildren<MainCharacterAppearanceApplier>(true);
			if (componentInChildren5 != null)
			{
				componentInChildren5.RestoreAuthoredMorphsForHome();
				MainCharacterAppearanceApplier mainCharacterAppearanceApplier = componentInChildren5;
				CustomizationPresetData customizationPresetData = preset;
				if (preset == null)
				{
					(customizationPresetData = new CustomizationPresetData()).isSystemPreset = true;
				}
				mainCharacterAppearanceApplier.ApplyPreset(customizationPresetData);
			}
			ClothInventorySyncer componentInChildren6 = player.GetComponentInChildren<ClothInventorySyncer>(true);
			if (componentInChildren6 == null)
			{
				return;
			}
			componentInChildren6.Resync();
		}

		// Token: 0x06008A05 RID: 35333 RVA: 0x00386804 File Offset: 0x00384A04
		private static void RestoreStandingState(Transform player)
		{
			vThirdPersonController componentInChildren = player.GetComponentInChildren<vThirdPersonController>(true);
			if (componentInChildren == null)
			{
				return;
			}
			componentInChildren.isCrouching = false;
			componentInChildren.customAction = false;
			componentInChildren.input = Vector2.zero;
			componentInChildren.isSprinting = false;
			Animator animator = componentInChildren.animator;
			if (componentInChildren._rigidbody != null && animator != null && animator.runtimeAnimatorController != null && animator.isInitialized)
			{
				componentInChildren.StopCharacter();
				foreach (AnimatorControllerParameter animatorControllerParameter in animator.parameters)
				{
					if (animatorControllerParameter.name == "IsCrouching" && animatorControllerParameter.type == AnimatorControllerParameterType.Bool)
					{
						animator.SetBool(animatorControllerParameter.nameHash, false);
					}
					else if ((animatorControllerParameter.name == "InputHorizontal" || animatorControllerParameter.name == "InputVertical" || animatorControllerParameter.name == "InputMagnitude") && animatorControllerParameter.type == AnimatorControllerParameterType.Float)
					{
						animator.SetFloat(animatorControllerParameter.nameHash, 0f);
					}
				}
			}
		}
	}
}
