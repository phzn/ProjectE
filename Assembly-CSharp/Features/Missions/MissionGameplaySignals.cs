using System;
using Features.Detection;
using Features.Inventory;
using Features.Inventory.Wearable;

namespace Features.Missions
{
	// Token: 0x02000F81 RID: 3969
	public static class MissionGameplaySignals
	{
		// Token: 0x140000CB RID: 203
		// (add) Token: 0x0600779A RID: 30618 RVA: 0x0030BE48 File Offset: 0x0030A048
		// (remove) Token: 0x0600779B RID: 30619 RVA: 0x0030BE7C File Offset: 0x0030A07C
		public static event Action<InventoryItemSO, ItemRarity> ItemCollected;

		// Token: 0x140000CC RID: 204
		// (add) Token: 0x0600779C RID: 30620 RVA: 0x0030BEB0 File Offset: 0x0030A0B0
		// (remove) Token: 0x0600779D RID: 30621 RVA: 0x0030BEE4 File Offset: 0x0030A0E4
		public static event Action<int> TrashcanLooted;

		// Token: 0x140000CD RID: 205
		// (add) Token: 0x0600779E RID: 30622 RVA: 0x0030BF18 File Offset: 0x0030A118
		// (remove) Token: 0x0600779F RID: 30623 RVA: 0x0030BF4C File Offset: 0x0030A14C
		public static event Action<int> NpcPhotosCommitted;

		// Token: 0x140000CE RID: 206
		// (add) Token: 0x060077A0 RID: 30624 RVA: 0x0030BF80 File Offset: 0x0030A180
		// (remove) Token: 0x060077A1 RID: 30625 RVA: 0x0030BFB4 File Offset: 0x0030A1B4
		public static event Action<WearableItemSO, string, string> WearableActionCompleted;

		// Token: 0x140000CF RID: 207
		// (add) Token: 0x060077A2 RID: 30626 RVA: 0x0030BFE8 File Offset: 0x0030A1E8
		// (remove) Token: 0x060077A3 RID: 30627 RVA: 0x0030C01C File Offset: 0x0030A21C
		public static event Action<WearableItemSO> ClothingBroken;

		// Token: 0x140000D0 RID: 208
		// (add) Token: 0x060077A4 RID: 30628 RVA: 0x0030C050 File Offset: 0x0030A250
		// (remove) Token: 0x060077A5 RID: 30629 RVA: 0x0030C084 File Offset: 0x0030A284
		public static event Action<NPCSenses> PlayerSeen;

		// Token: 0x140000D1 RID: 209
		// (add) Token: 0x060077A6 RID: 30630 RVA: 0x0030C0B8 File Offset: 0x0030A2B8
		// (remove) Token: 0x060077A7 RID: 30631 RVA: 0x0030C0EC File Offset: 0x0030A2EC
		public static event Action FirstFlight;

		// Token: 0x060077A8 RID: 30632 RVA: 0x0030C11F File Offset: 0x0030A31F
		public static void NotifyFirstFlight()
		{
			Action firstFlight = MissionGameplaySignals.FirstFlight;
			if (firstFlight == null)
			{
				return;
			}
			firstFlight();
		}

		// Token: 0x060077A9 RID: 30633 RVA: 0x0030C130 File Offset: 0x0030A330
		public static void NotifyPlayerSeen(NPCSenses npc)
		{
			if (npc != null)
			{
				Action<NPCSenses> playerSeen = MissionGameplaySignals.PlayerSeen;
				if (playerSeen == null)
				{
					return;
				}
				playerSeen(npc);
			}
		}

		// Token: 0x060077AA RID: 30634 RVA: 0x0030C14B File Offset: 0x0030A34B
		public static void NotifyItemCollected(InventoryItemSO item, ItemRarity rarity)
		{
			if (item != null)
			{
				Action<InventoryItemSO, ItemRarity> itemCollected = MissionGameplaySignals.ItemCollected;
				if (itemCollected == null)
				{
					return;
				}
				itemCollected(item, rarity);
			}
		}

		// Token: 0x060077AB RID: 30635 RVA: 0x0030C167 File Offset: 0x0030A367
		public static void NotifyTrashcanLooted(int itemCount)
		{
			if (itemCount > 0)
			{
				Action<int> trashcanLooted = MissionGameplaySignals.TrashcanLooted;
				if (trashcanLooted == null)
				{
					return;
				}
				trashcanLooted(itemCount);
			}
		}

		// Token: 0x060077AC RID: 30636 RVA: 0x0030C17D File Offset: 0x0030A37D
		public static void NotifyNpcPhotosCommitted(int photoCount)
		{
			if (photoCount > 0)
			{
				Action<int> npcPhotosCommitted = MissionGameplaySignals.NpcPhotosCommitted;
				if (npcPhotosCommitted == null)
				{
					return;
				}
				npcPhotosCommitted(photoCount);
			}
		}

		// Token: 0x060077AD RID: 30637 RVA: 0x0030C193 File Offset: 0x0030A393
		public static void NotifyWearableAction(WearableItemSO item, string actionId, string actionName)
		{
			if (item == null || string.IsNullOrWhiteSpace(actionId))
			{
				return;
			}
			Action<WearableItemSO, string, string> wearableActionCompleted = MissionGameplaySignals.WearableActionCompleted;
			if (wearableActionCompleted == null)
			{
				return;
			}
			wearableActionCompleted(item, actionId, string.IsNullOrWhiteSpace(actionName) ? actionId : actionName);
		}

		// Token: 0x060077AE RID: 30638 RVA: 0x0030C1C4 File Offset: 0x0030A3C4
		public static void NotifyClothingBroken(WearableItemSO item)
		{
			if (item != null)
			{
				Action<WearableItemSO> clothingBroken = MissionGameplaySignals.ClothingBroken;
				if (clothingBroken == null)
				{
					return;
				}
				clothingBroken(item);
			}
		}
	}
}
