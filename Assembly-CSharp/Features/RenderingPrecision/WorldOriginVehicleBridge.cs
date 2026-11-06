using System;
using Features.PhoneCompanion;
using Features.Vehicles.Spawning;
using Features.Vehicles.Traffic;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using ProjectExhibitionist.Core;
using UnityEngine;
using VehicleSpawnPlugin;

namespace Features.RenderingPrecision
{
	// Token: 0x02000D84 RID: 3460
	public static class WorldOriginVehicleBridge
	{
		// Token: 0x060065E2 RID: 26082 RVA: 0x0028ACF8 File Offset: 0x00288EF8
		public static bool IsGroupingTransform(Transform candidate)
		{
			if (candidate == null)
			{
				return false;
			}
			if (candidate.GetComponent<VehicleSpawnManager>() != null || candidate.GetComponent<VehicleSpawner>() != null)
			{
				return true;
			}
			VehicleSpawner vehicleSpawner = ((candidate.parent != null) ? candidate.parent.GetComponent<VehicleSpawner>() : null);
			return vehicleSpawner != null && vehicleSpawner.PoolRoot == candidate;
		}

		// Token: 0x060065E3 RID: 26083 RVA: 0x0028AD64 File Offset: 0x00288F64
		public static bool CanShift(vThirdPersonController rider)
		{
			if (rider != null && rider.customAction)
			{
				return false;
			}
			bool flag = WorldOriginVehicleBridge.IsStableRide(rider);
			if (rider != null && rider.lockAnimMovement && !flag)
			{
				return false;
			}
			if (flag)
			{
				return true;
			}
			return PlayerActivityGate.CanAcquireAfterReleasing(PlayerActivity.None, PlayerContextResource.WorldInteraction | PlayerContextResource.FullBodyAnimation, delegate(Object owner)
			{
				SmartphoneController smartphoneController = owner as SmartphoneController;
				return smartphoneController != null && smartphoneController.State == SmartphoneController.PhoneState.PhoneHeld;
			});
		}

		// Token: 0x060065E4 RID: 26084 RVA: 0x0028ADD0 File Offset: 0x00288FD0
		public static bool IsStableRide(vThirdPersonController rider)
		{
			if (rider == null || !PlayerActivityGate.IsResourceOwned(PlayerContextResource.Rider))
			{
				return false;
			}
			bool foundRide = false;
			bool flag = PlayerActivityGate.CanAcquireAfterReleasing(PlayerActivity.None, PlayerContextResource.WorldInteraction | PlayerContextResource.FullBodyAnimation, delegate(Object owner)
			{
				bool flag2 = WorldOriginVehicleBridge.IsStableOwner(owner, rider);
				foundRide = foundRide || flag2;
				return flag2;
			});
			return foundRide && flag;
		}

		// Token: 0x060065E5 RID: 26085 RVA: 0x0028AE28 File Offset: 0x00289028
		private static bool IsStableOwner(Object owner, vThirdPersonController rider)
		{
			vVehicleInteractable vVehicleInteractable = owner as vVehicleInteractable;
			if (vVehicleInteractable != null && vVehicleInteractable.isActiveAndEnabled && vVehicleInteractable.IsBeingUsed && vVehicleInteractable.CurrentRider == rider)
			{
				vVehicleController component = vVehicleInteractable.GetComponent<vVehicleController>();
				return component != null && !component.IsTransitioning && component.MountStabilizationRemaining <= 0f;
			}
			vBikeInteractable vBikeInteractable = owner as vBikeInteractable;
			if (vBikeInteractable != null)
			{
				return vBikeInteractable.isActiveAndEnabled && vBikeInteractable.IsBeingUsed && vBikeInteractable.CurrentRider == rider;
			}
			SimpleRollerSkatesInteractable simpleRollerSkatesInteractable = owner as SimpleRollerSkatesInteractable;
			if (simpleRollerSkatesInteractable != null)
			{
				return simpleRollerSkatesInteractable.isActiveAndEnabled && simpleRollerSkatesInteractable.IsBeingWorn && !simpleRollerSkatesInteractable.IsTransitioning && simpleRollerSkatesInteractable.CurrentRider == rider;
			}
			SkateboardInteractable skateboardInteractable = owner as SkateboardInteractable;
			return skateboardInteractable != null && (skateboardInteractable.isActiveAndEnabled && skateboardInteractable.IsBeingWorn && !skateboardInteractable.IsTransitioning && !skateboardInteractable.TrickActive) && skateboardInteractable.CurrentRider == rider;
		}

		// Token: 0x060065E6 RID: 26086 RVA: 0x0028AF20 File Offset: 0x00289120
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void Install()
		{
			WorldOrigin.Shifted -= WorldOriginVehicleBridge.ApplyShift;
			WorldOrigin.Shifted += WorldOriginVehicleBridge.ApplyShift;
		}

		// Token: 0x060065E7 RID: 26087 RVA: 0x0028AF44 File Offset: 0x00289144
		private static void ApplyShift(Vector3 delta)
		{
			VehicleAI.ApplyWorldOriginShiftToAll(delta);
			CarRoofCarrier.ApplyWorldOriginShiftToAll(delta);
			CarSurfShowController instance = CarSurfShowController.Instance;
			if (instance != null)
			{
				instance.ApplyWorldOriginShift(delta);
			}
			CarClimbBridge.ApplyWorldOriginShiftToAll(delta);
		}

		// Token: 0x0400686A RID: 26730
		private const PlayerContextResource PoseResources = PlayerContextResource.WorldInteraction | PlayerContextResource.FullBodyAnimation;
	}
}
