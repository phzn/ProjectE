using System;
using UnityEngine;
using VehicleSpawnPlugin;

namespace Features.Inventory.Placement
{
	// Token: 0x02001143 RID: 4419
	public static class MovingPlacementSupport
	{
		// Token: 0x06008624 RID: 34340 RVA: 0x0036BBF0 File Offset: 0x00369DF0
		public static Transform Resolve(Collider surface)
		{
			if (surface == null)
			{
				return null;
			}
			VehicleAI componentInParent = surface.GetComponentInParent<VehicleAI>();
			if (componentInParent != null)
			{
				return componentInParent.transform;
			}
			if (!(surface.attachedRigidbody != null))
			{
				return null;
			}
			return surface.attachedRigidbody.transform;
		}

		// Token: 0x06008625 RID: 34341 RVA: 0x0036BC3C File Offset: 0x00369E3C
		public static Transform Resolve(Transform surface)
		{
			if (surface == null)
			{
				return null;
			}
			VehicleAI componentInParent = surface.GetComponentInParent<VehicleAI>();
			if (componentInParent != null)
			{
				return componentInParent.transform;
			}
			Rigidbody componentInParent2 = surface.GetComponentInParent<Rigidbody>();
			if (!(componentInParent2 != null))
			{
				return null;
			}
			return componentInParent2.transform;
		}

		// Token: 0x06008626 RID: 34342 RVA: 0x0036BC83 File Offset: 0x00369E83
		public static bool Allows(InventoryItemSO item, Transform support)
		{
			return item != null && (support == null || (!item.requiresStaticPlacement && !item.isVehicleItem));
		}
	}
}
