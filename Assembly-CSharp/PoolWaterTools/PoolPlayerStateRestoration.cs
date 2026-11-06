using System;
using Features.GoldBikini;
using Features.PoolDive;
using Features.Slip;
using Invector.vCharacterController.vActions;
using UnityEngine;

namespace PoolWaterTools
{
	// Token: 0x02000726 RID: 1830
	public static class PoolPlayerStateRestoration
	{
		// Token: 0x06002F6A RID: 12138 RVA: 0x0010C0C8 File Offset: 0x0010A2C8
		public static void ResetForHomeReturn(Transform player)
		{
			if (player == null)
			{
				return;
			}
			PoolDiveController[] componentsInChildren = player.GetComponentsInChildren<PoolDiveController>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].ResetForHomeReturn();
			}
			BikiniSlipDiveController[] componentsInChildren2 = player.GetComponentsInChildren<BikiniSlipDiveController>(true);
			for (int i = 0; i < componentsInChildren2.Length; i++)
			{
				componentsInChildren2[i].ResetForHomeReturn();
			}
			vSwimming[] componentsInChildren3 = player.GetComponentsInChildren<vSwimming>(true);
			for (int i = 0; i < componentsInChildren3.Length; i++)
			{
				componentsInChildren3[i].ForceExitSwimming();
			}
			PoolEntryStateGuard[] componentsInChildren4 = player.GetComponentsInChildren<PoolEntryStateGuard>(true);
			for (int i = 0; i < componentsInChildren4.Length; i++)
			{
				componentsInChildren4[i].RestoreForEncounter();
			}
			SwimmingLayerSuppressor[] componentsInChildren5 = player.GetComponentsInChildren<SwimmingLayerSuppressor>(true);
			for (int i = 0; i < componentsInChildren5.Length; i++)
			{
				componentsInChildren5[i].RestoreForEncounter();
			}
			ClothSlipController[] componentsInChildren6 = player.GetComponentsInChildren<ClothSlipController>(true);
			for (int i = 0; i < componentsInChildren6.Length; i++)
			{
				componentsInChildren6[i].ResetForHomeReturn();
			}
		}
	}
}
