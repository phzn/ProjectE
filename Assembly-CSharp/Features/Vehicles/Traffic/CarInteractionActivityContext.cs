using System;
using Features.Flash;
using Features.HideFlash;
using Features.Inventory;
using ProjectExhibitionist.Core;
using UnityEngine;

namespace Features.Vehicles.Traffic
{
	// Token: 0x02000B91 RID: 2961
	public static class CarInteractionActivityContext
	{
		// Token: 0x17000E21 RID: 3617
		// (get) Token: 0x060054B8 RID: 21688 RVA: 0x0020E655 File Offset: 0x0020C855
		public static bool HasFreeHands
		{
			get
			{
				return !ClothActionGate.IsBusy && !HideFlashController.HasHeldCoverItem && (PlayerInventoryManager.Instance == null || PlayerInventoryManager.Instance.GetActiveItem() == null);
			}
		}

		// Token: 0x17000E22 RID: 3618
		// (get) Token: 0x060054B9 RID: 21689 RVA: 0x0020E686 File Offset: 0x0020C886
		public static bool CanEnter
		{
			get
			{
				return CarInteractionActivityContext.HasFreeHands && PlayerActivityGate.CanStartWorldInteraction && !PlayerActivityGate.IsResourceOwned(PlayerContextResource.Rider | PlayerContextResource.HeldItem);
			}
		}

		// Token: 0x060054BA RID: 21690 RVA: 0x0020E6A2 File Offset: 0x0020C8A2
		public static bool TryAcquire(string reason, Object owner, ref PlayerActivityLease lease)
		{
			if (lease != null && lease.IsActive)
			{
				return true;
			}
			lease = null;
			return CarInteractionActivityContext.CanEnter && PlayerActivityGate.TryAcquire(reason, owner, PlayerActivity.Interact, PlayerContextResource.Rider, PlayerActivity.None, out lease, true, null);
		}

		// Token: 0x060054BB RID: 21691 RVA: 0x00176B3D File Offset: 0x00174D3D
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
	}
}
