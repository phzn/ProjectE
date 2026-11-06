using System;
using Features.Flash;
using Features.Inventory;
using Features.PhoneCompanion;
using ProjectExhibitionist.Core;
using UnityEngine;

namespace ProjectExhibitionist.Interactions
{
	// Token: 0x02000908 RID: 2312
	public static class FullBodyHideActivityContext
	{
		// Token: 0x17000A7B RID: 2683
		// (get) Token: 0x06003E7D RID: 15997 RVA: 0x00173351 File Offset: 0x00171551
		public static bool CanAcquire
		{
			get
			{
				return FullBodyHideActivityContext.CanAcquireProfile(false);
			}
		}

		// Token: 0x17000A7C RID: 2684
		// (get) Token: 0x06003E7E RID: 15998 RVA: 0x00173359 File Offset: 0x00171559
		public static bool CanAcquireWithTripodCamera
		{
			get
			{
				return FullBodyHideActivityContext.CanAcquireProfile(true);
			}
		}

		// Token: 0x06003E7F RID: 15999 RVA: 0x00173361 File Offset: 0x00171561
		private static bool CanAcquireProfile(bool allowTripodCamera)
		{
			return !ClothActionGate.IsBusy && !FullBodyHideActivityContext.HasHeldInventoryItem && PlayerActivityGate.Allows(PlayerActivity.Interact) && !PlayerActivityGate.IsResourceOwned(FullBodyHideActivityContext.ResolveResources(allowTripodCamera));
		}

		// Token: 0x06003E80 RID: 16000 RVA: 0x0017338A File Offset: 0x0017158A
		private static PlayerContextResource ResolveResources(bool allowTripodCamera)
		{
			if (!allowTripodCamera || !SmartphoneController.IsStableTripodCameraOwner)
			{
				return PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.Camera | PlayerContextResource.FullBodyAnimation | PlayerContextResource.HeldItem;
			}
			return PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.FullBodyAnimation | PlayerContextResource.HeldItem;
		}

		// Token: 0x06003E81 RID: 16001 RVA: 0x0017339B File Offset: 0x0017159B
		public static bool IsUsingTripodCamera(PlayerActivityLease lease)
		{
			return lease != null && lease.IsActive && lease.Resources == (PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.FullBodyAnimation | PlayerContextResource.HeldItem);
		}

		// Token: 0x17000A7D RID: 2685
		// (get) Token: 0x06003E82 RID: 16002 RVA: 0x001733B4 File Offset: 0x001715B4
		private static bool HasHeldInventoryItem
		{
			get
			{
				return PlayerInventoryManager.Instance != null && PlayerInventoryManager.Instance.GetActiveItem() != null;
			}
		}

		// Token: 0x06003E83 RID: 16003 RVA: 0x001733D8 File Offset: 0x001715D8
		public static bool TryAcquire(string reason, Object owner, ref PlayerActivityLease lease, PlayerActivity blockedActivities = PlayerActivity.AllPlayerActions, bool allowsUtilityModals = true, bool allowTripodCamera = false)
		{
			if (lease != null)
			{
				if (lease.IsActive)
				{
					return true;
				}
				lease = null;
			}
			if (ClothActionGate.IsBusy || FullBodyHideActivityContext.HasHeldInventoryItem)
			{
				return false;
			}
			if (!PlayerActivityGate.TryAcquire(reason, owner, PlayerActivity.Interact, FullBodyHideActivityContext.ResolveResources(allowTripodCamera), blockedActivities, out lease, allowsUtilityModals, null))
			{
				return false;
			}
			InteractionCandidateRegistry.MarkInteract();
			return true;
		}

		// Token: 0x06003E84 RID: 16004 RVA: 0x00173426 File Offset: 0x00171626
		public static bool SetPhotoModeAvailable(PlayerActivityLease lease, bool available)
		{
			return lease != null && lease.IsActive && PlayerActivityGate.TryUpdate(lease, PlayerActivity.None, lease.Resources, available ? (PlayerActivity.Emote | PlayerActivity.Cover | PlayerActivity.Flash | PlayerActivity.RapidHUD | PlayerActivity.Interact | PlayerActivity.StanceSwitch | PlayerActivity.Undress | PlayerActivity.LingerieOpen | PlayerActivity.Inventory | PlayerActivity.Phone | PlayerActivity.Crouch | PlayerActivity.ArmsPose) : PlayerActivity.AllPlayerActions);
		}

		// Token: 0x06003E85 RID: 16005 RVA: 0x00173451 File Offset: 0x00171651
		public static void Release(ref PlayerActivityLease lease)
		{
			if (lease == null)
			{
				return;
			}
			lease.Dispose();
			lease = null;
		}

		// Token: 0x04003E98 RID: 16024
		public const PlayerContextResource Resources = PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.Camera | PlayerContextResource.FullBodyAnimation | PlayerContextResource.HeldItem;

		// Token: 0x04003E99 RID: 16025
		public const PlayerContextResource TripodCompanionResources = PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.FullBodyAnimation | PlayerContextResource.HeldItem;
	}
}
