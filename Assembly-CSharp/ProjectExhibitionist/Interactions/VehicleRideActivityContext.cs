using System;
using Features.Flash;
using Features.HideFlash;
using Features.InsertableToys;
using Features.Inventory;
using Features.Rank;
using Invector.vCharacterController.vActions;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Minimap;
using UnityEngine;

namespace ProjectExhibitionist.Interactions
{
	// Token: 0x0200091A RID: 2330
	public static class VehicleRideActivityContext
	{
		// Token: 0x17000A9D RID: 2717
		// (get) Token: 0x06003F1E RID: 16158 RVA: 0x00176A40 File Offset: 0x00174C40
		public static bool BlocksDrivingInput
		{
			get
			{
				if (!PlayerActivityGate.IsResourceOwned(PlayerContextResource.ModalUI))
				{
					return false;
				}
				ProceduralMiniMapController resourceOwner = PlayerActivityGate.GetResourceOwner<ProceduralMiniMapController>(PlayerContextResource.ModalUI);
				return resourceOwner == null || !resourceOwner.IsExpanded || ControllerInput.IsActive;
			}
		}

		// Token: 0x06003F1F RID: 16159 RVA: 0x00176A78 File Offset: 0x00174C78
		public static bool IsCompatibleWithRider(GameObject rider, vVehicleController.VehicleType vehicleType)
		{
			if (rider == null || HideFlashController.HasHeldCoverItem || VehicleRideActivityContext.HasHeldInventoryItem)
			{
				return false;
			}
			if (vehicleType != vVehicleController.VehicleType.Bicycle && vehicleType != vVehicleController.VehicleType.Unicycle)
			{
				return true;
			}
			InsertableToyController componentInChildren = rider.GetComponentInChildren<InsertableToyController>(true);
			return componentInChildren == null || !componentInChildren.IsInserted;
		}

		// Token: 0x17000A9E RID: 2718
		// (get) Token: 0x06003F20 RID: 16160 RVA: 0x00176AC3 File Offset: 0x00174CC3
		public static bool CanAcquire
		{
			get
			{
				return !GuidedRankGameplayPolicy.RideableVehiclesBlocked && !ClothActionGate.IsBusy && !VehicleRideActivityContext.HasHeldInventoryItem && PlayerActivityGate.Allows(PlayerActivity.Interact) && !PlayerActivityGate.IsResourceOwned(PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.FullBodyAnimation | PlayerContextResource.Rider | PlayerContextResource.HeldItem);
			}
		}

		// Token: 0x17000A9F RID: 2719
		// (get) Token: 0x06003F21 RID: 16161 RVA: 0x001733B4 File Offset: 0x001715B4
		private static bool HasHeldInventoryItem
		{
			get
			{
				return PlayerInventoryManager.Instance != null && PlayerInventoryManager.Instance.GetActiveItem() != null;
			}
		}

		// Token: 0x06003F22 RID: 16162 RVA: 0x00176AF0 File Offset: 0x00174CF0
		public static bool TryAcquire(string reason, Object owner, ref PlayerActivityLease lease)
		{
			if (GuidedRankGameplayPolicy.RideableVehiclesBlocked)
			{
				return false;
			}
			if (lease != null)
			{
				if (lease.IsActive)
				{
					return true;
				}
				lease = null;
			}
			return !ClothActionGate.IsBusy && !VehicleRideActivityContext.HasHeldInventoryItem && PlayerActivityGate.TryAcquire(reason, owner, PlayerActivity.Interact, PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.FullBodyAnimation | PlayerContextResource.Rider | PlayerContextResource.HeldItem, PlayerActivity.Flash | PlayerActivity.Interact, out lease, true, null);
		}

		// Token: 0x06003F23 RID: 16163 RVA: 0x00176B3D File Offset: 0x00174D3D
		public static void Release(ref PlayerActivityLease lease)
		{
			PlayerActivityLease playerActivityLease = lease;
			lease = null;
			if (playerActivityLease == null)
			{
				return;
			}
			playerActivityLease.Dispose();
		}

		// Token: 0x04003F04 RID: 16132
		public const PlayerContextResource Resources = PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.FullBodyAnimation | PlayerContextResource.Rider | PlayerContextResource.HeldItem;

		// Token: 0x04003F05 RID: 16133
		public const PlayerActivity BlockedActivities = PlayerActivity.Flash | PlayerActivity.Interact;
	}
}
