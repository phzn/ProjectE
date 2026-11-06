using System;
using Features.CarHide;
using Features.DumpsterHide;
using Features.Flash;
using Features.FlexiPenis;
using Features.GachaMachine;
using Features.Inventory;
using Features.Inventory.Actions;
using Features.Inventory.Placement;
using Features.Inventory.UI;
using Features.LockerHide;
using Features.PhoneCompanion;
using Features.PlayerEmotes;
using Features.PoolDive;
using Features.Stadium;
using Features.TrashcanLoot;
using Features.Vehicles.Traffic;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using Invector.vCover;
using PoolWaterTools;
using ProjectExhibitionist.BenchSeating;
using ProjectExhibitionist.Core.Rendering;
using ProjectExhibitionist.Core.SceneManagement;
using ProjectExhibitionist.Minimap;
using ProjectExhibitionist.PhotoBooth;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x02001273 RID: 4723
	public static class DetectionPlayerNormalizer
	{
		// Token: 0x060092DD RID: 37597 RVA: 0x003C4510 File Offset: 0x003C2710
		public static void InterruptGameplayOverlays()
		{
			foreach (GraphicsSettingsRuntimeUI graphicsSettingsRuntimeUI in Object.FindObjectsOfType<GraphicsSettingsRuntimeUI>(true))
			{
				if (graphicsSettingsRuntimeUI.IsOpen)
				{
					graphicsSettingsRuntimeUI.SetOpen(false);
				}
			}
			InventoryDialogController[] array2 = Object.FindObjectsOfType<InventoryDialogController>(true);
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].InterruptForEncounter();
			}
			ProceduralMiniMapController[] array3 = Object.FindObjectsOfType<ProceduralMiniMapController>(true);
			for (int i = 0; i < array3.Length; i++)
			{
				array3[i].InterruptForEncounter();
			}
			StadiumScreenZoomController[] array4 = Object.FindObjectsOfType<StadiumScreenZoomController>(true);
			for (int i = 0; i < array4.Length; i++)
			{
				array4[i].InterruptForEncounter();
			}
			PhotoBoothController[] array5 = Object.FindObjectsOfType<PhotoBoothController>(true);
			for (int i = 0; i < array5.Length; i++)
			{
				array5[i].InterruptForEncounter();
			}
		}

		// Token: 0x060092DE RID: 37598 RVA: 0x003C45C4 File Offset: 0x003C27C4
		public static bool PrepareForEncounter(NPCSenses detectingNpc)
		{
			DetectionPlayerNormalizer.InterruptGameplayOverlays();
			Transform player = PlayerRegistry.Player;
			vThirdPersonInput vThirdPersonInput = ((player != null) ? (player.GetComponent<vThirdPersonInput>() ?? player.GetComponentInChildren<vThirdPersonInput>(true)) : null);
			if (vThirdPersonInput == null)
			{
				vThirdPersonInput = Object.FindObjectOfType<vThirdPersonInput>(true);
			}
			if (vThirdPersonInput == null)
			{
				return true;
			}
			Transform transform = vThirdPersonInput.transform;
			StadiumParachuteController component = transform.GetComponent<StadiumParachuteController>();
			if (component != null && (component.IsFlying || component.IsLanding))
			{
				return false;
			}
			BenchPlayerSeating component2 = transform.GetComponent<BenchPlayerSeating>();
			if (component2 != null && !component2.TryPrepareForEncounter())
			{
				return false;
			}
			SmartphoneController.InterruptForDetection();
			DildoPlacementSystem[] array = Object.FindObjectsOfType<DildoPlacementSystem>(true);
			for (int i = 0; i < array.Length; i++)
			{
				array[i].InterruptForEncounter();
			}
			FlexiWallHeldActions[] array2 = Object.FindObjectsOfType<FlexiWallHeldActions>(true);
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].InterruptForEncounter();
			}
			DildoActionExecutor[] array3 = Object.FindObjectsOfType<DildoActionExecutor>(true);
			for (int i = 0; i < array3.Length; i++)
			{
				array3[i].CancelAction();
			}
			FlashGateRegistry.InterruptForEncounter((player != null) ? player : transform);
			vBasicCoverController componentInChildren = vThirdPersonInput.GetComponentInChildren<vBasicCoverController>(true);
			bool flag = componentInChildren != null && componentInChildren.InterruptForEncounter();
			TrashcanLootInteractable[] array4 = Object.FindObjectsOfType<TrashcanLootInteractable>(true);
			for (int i = 0; i < array4.Length; i++)
			{
				array4[i].InterruptForEncounter();
			}
			LockerHideInteractable[] array5 = Object.FindObjectsOfType<LockerHideInteractable>(true);
			for (int i = 0; i < array5.Length; i++)
			{
				array5[i].InterruptForEncounter();
			}
			DumpsterHideInteractable[] array6 = Object.FindObjectsOfType<DumpsterHideInteractable>(true);
			for (int i = 0; i < array6.Length; i++)
			{
				array6[i].InterruptForEncounter();
			}
			WasteCanHideInteractable[] array7 = Object.FindObjectsOfType<WasteCanHideInteractable>(true);
			for (int i = 0; i < array7.Length; i++)
			{
				array7[i].InterruptForEncounter();
			}
			GachaMachineController[] array8 = Object.FindObjectsOfType<GachaMachineController>(true);
			for (int i = 0; i < array8.Length; i++)
			{
				array8[i].InterruptForEncounter();
			}
			CarClimbBridge[] array9 = Object.FindObjectsOfType<CarClimbBridge>(true);
			for (int i = 0; i < array9.Length; i++)
			{
				array9[i].InterruptForEncounter();
			}
			GameplayEmoteController gameplayEmoteController = vThirdPersonInput.GetComponentInChildren<GameplayEmoteController>(true) ?? vThirdPersonInput.GetComponentInParent<GameplayEmoteController>();
			if (gameplayEmoteController != null && gameplayEmoteController.CurrentState != GameplayEmoteController.EmoteState.Idle)
			{
				gameplayEmoteController.ForceEndEmoteImmediate();
			}
			PlayerActionAnimator playerActionAnimator = vThirdPersonInput.GetComponentInChildren<PlayerActionAnimator>(true) ?? vThirdPersonInput.GetComponentInParent<PlayerActionAnimator>();
			if (playerActionAnimator != null)
			{
				playerActionAnimator.CancelCurrentAction();
			}
			CoverPoseController instance = CoverPoseController.Instance;
			if (instance != null && instance.IsCovering)
			{
				instance.DeactivateCoverPose();
			}
			CarHideUnderInteractable[] array10 = Object.FindObjectsOfType<CarHideUnderInteractable>(true);
			for (int j = 0; j < array10.Length; j++)
			{
				if (array10[j] != null && array10[j].IsBusy)
				{
					array10[j].ForceExitImmediate();
				}
			}
			DetectionPlayerNormalizer.ForceDismount(transform);
			PlayerInventoryManager playerInventoryManager = vThirdPersonInput.GetComponentInChildren<PlayerInventoryManager>(true) ?? vThirdPersonInput.GetComponentInParent<PlayerInventoryManager>();
			if (playerInventoryManager != null)
			{
				playerInventoryManager.StowHeldItemForInteraction();
			}
			if (!DetectionPlayerNormalizer.TryRecoverFromWater(vThirdPersonInput))
			{
				return false;
			}
			vThirdPersonController cc = vThirdPersonInput.cc;
			if (cc != null)
			{
				cc.isCrouching = false;
				cc.customAction = false;
				cc.input = Vector2.zero;
				cc.isSprinting = false;
				cc.StopCharacter();
				if (cc.animator != null)
				{
					cc.animator.SetBool(DetectionPlayerNormalizer.IsCrouchingHash, false);
					cc.animator.SetFloat("InputHorizontal", 0f);
					cc.animator.SetFloat("InputVertical", 0f);
					cc.animator.SetFloat("InputMagnitude", 0f);
				}
			}
			Vector3 vector = ((detectingNpc != null) ? (detectingNpc.transform.position - transform.position) : transform.forward);
			float num = (CarRoofCarrier.IsPlayerMounted ? 0.35f : (flag ? 0.9f : 0.8f));
			Vector3 vector2;
			if (DetectionEncounterPlacement.TryFindStandingPosition(transform, transform.position, vector, out vector2, num))
			{
				DetectionPlayerNormalizer.MovePlayer(transform, vector2);
			}
			if (detectingNpc != null)
			{
				Physics.SyncTransforms();
			}
			return true;
		}

		// Token: 0x060092DF RID: 37599 RVA: 0x003C49DC File Offset: 0x003C2BDC
		public static void ForceDismount(Transform player)
		{
			if (player == null)
			{
				return;
			}
			foreach (vVehicleController vVehicleController in Object.FindObjectsOfType<vVehicleController>(true))
			{
				if (vVehicleController != null && vVehicleController.IsBeingRidden && DetectionPlayerNormalizer.RiderMatches(vVehicleController.CurrentRider, player))
				{
					vVehicleController.ForceDismount();
				}
			}
			foreach (vBikeController vBikeController in Object.FindObjectsOfType<vBikeController>(true))
			{
				if (vBikeController != null && vBikeController.IsBeingRidden && DetectionPlayerNormalizer.RiderMatches(vBikeController.CurrentRider, player))
				{
					vBikeController.ForceDismount();
				}
			}
			foreach (SkateboardInteractable skateboardInteractable in Object.FindObjectsOfType<SkateboardInteractable>(true))
			{
				if (skateboardInteractable != null && skateboardInteractable.IsBeingWorn && DetectionPlayerNormalizer.RiderMatches(skateboardInteractable.CurrentRider, player))
				{
					skateboardInteractable.ForceCrashDismount();
				}
			}
			foreach (SimpleRollerSkatesInteractable simpleRollerSkatesInteractable in Object.FindObjectsOfType<SimpleRollerSkatesInteractable>(true))
			{
				if (simpleRollerSkatesInteractable != null && simpleRollerSkatesInteractable.IsBeingWorn && DetectionPlayerNormalizer.RiderMatches(simpleRollerSkatesInteractable.CurrentRider, player))
				{
					simpleRollerSkatesInteractable.ForceCrashDismount();
				}
			}
		}

		// Token: 0x060092E0 RID: 37600 RVA: 0x003C4B00 File Offset: 0x003C2D00
		private static bool RiderMatches(vThirdPersonController rider, Transform player)
		{
			if (rider == null || player == null)
			{
				return false;
			}
			Transform transform = rider.transform;
			return transform == player || transform.IsChildOf(player) || player.IsChildOf(transform);
		}

		// Token: 0x060092E1 RID: 37601 RVA: 0x003C4B44 File Offset: 0x003C2D44
		public static bool TryRecoverFromWater(vThirdPersonInput input)
		{
			if (input == null)
			{
				return true;
			}
			Transform transform = input.transform;
			vSwimming vSwimming = input.GetComponentInChildren<vSwimming>(true) ?? input.GetComponentInParent<vSwimming>();
			PoolDiveController poolDiveController = input.GetComponentInChildren<PoolDiveController>(true) ?? input.GetComponentInParent<PoolDiveController>();
			bool flag = poolDiveController != null && poolDiveController.IsWaterHandoffActive;
			bool flag2 = vSwimming != null && (vSwimming.isSwimming || vSwimming.inTheWater || vSwimming.isUnderWater);
			PoolEntryStateGuard[] componentsInChildren = input.GetComponentsInChildren<PoolEntryStateGuard>(true);
			bool flag3 = Array.Exists<PoolEntryStateGuard>(componentsInChildren, (PoolEntryStateGuard guard) => guard.HasPendingWaterRestore);
			Collider collider;
			bool flag4 = PoolWaterRecovery.TryFindWater(transform.position, out collider, flag ? 4f : 0.35f, true);
			if (!flag4 && !flag2 && !flag && !flag3)
			{
				return true;
			}
			Vector3 position = transform.position;
			if (flag4 && !PoolWaterRecovery.TryFindDryDeck(collider, transform.position, transform, out position, 0.8f) && !PoolWaterRecovery.TryFindDryDeck(collider, transform.position, transform, out position, 0.4f))
			{
				return false;
			}
			if (poolDiveController != null)
			{
				poolDiveController.InterruptForEncounter();
			}
			if (vSwimming != null)
			{
				vSwimming.ForceExitSwimming();
			}
			PoolEntryStateGuard[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].RestoreForEncounter();
			}
			SwimmingLayerSuppressor[] componentsInChildren2 = input.GetComponentsInChildren<SwimmingLayerSuppressor>(true);
			for (int i = 0; i < componentsInChildren2.Length; i++)
			{
				componentsInChildren2[i].RestoreForEncounter();
			}
			Animator animator = ((input.cc != null) ? input.cc.animator : input.GetComponent<Animator>());
			if (animator != null && animator.isActiveAndEnabled && animator.runtimeAnimatorController != null)
			{
				string[] array2 = new string[] { "Base Layer.Locomotion.Free Movement", "Base Layer.Free Movement", "Free Movement", "Locomotion", "Idle", "Grounded" };
				for (int i = 0; i < array2.Length; i++)
				{
					int num = Animator.StringToHash(array2[i]);
					if (animator.HasState(0, num))
					{
						animator.Play(num, 0, 0f);
						animator.Update(0f);
						break;
					}
				}
			}
			if (input.cc != null)
			{
				input.cc.customAction = false;
				input.cc.verticalVelocity = 0f;
				input.cc.input = Vector2.zero;
			}
			DetectionPlayerNormalizer.MovePlayer(transform, position);
			return true;
		}

		// Token: 0x060092E2 RID: 37602 RVA: 0x003C4DD0 File Offset: 0x003C2FD0
		private static void MovePlayer(Transform player, Vector3 position)
		{
			Rigidbody component = player.GetComponent<Rigidbody>();
			if (component != null)
			{
				if (!component.isKinematic)
				{
					component.velocity = Vector3.zero;
					component.angularVelocity = Vector3.zero;
				}
				component.position = position;
			}
			player.position = position;
			Physics.SyncTransforms();
		}

		// Token: 0x0400977E RID: 38782
		private static readonly int IsCrouchingHash = Animator.StringToHash("IsCrouching");
	}
}
