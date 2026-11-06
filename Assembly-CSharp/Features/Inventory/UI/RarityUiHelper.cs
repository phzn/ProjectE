using System;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Inventory.UI
{
	// Token: 0x0200115F RID: 4447
	public static class RarityUiHelper
	{
		// Token: 0x06008775 RID: 34677 RVA: 0x00376FE7 File Offset: 0x003751E7
		public static void ApplyOutline(Graphic icon, InventoryItemSO item)
		{
			RarityUiHelper.ApplyOutline(icon, item, (item != null) ? item.rarity : ItemRarity.Common);
		}

		// Token: 0x06008776 RID: 34678 RVA: 0x00377004 File Offset: 0x00375204
		public static void ApplyOutline(Graphic icon, InventoryItemSO item, ItemRarity instanceRarity)
		{
			if (icon == null)
			{
				return;
			}
			RaritySettingsSO instance = RaritySettingsSO.Instance;
			bool flag = item != null && instance != null && instance.ShowOutline(instanceRarity);
			Outline outline = icon.GetComponent<Outline>();
			if (!flag)
			{
				if (outline != null)
				{
					outline.enabled = false;
				}
				return;
			}
			if (outline == null)
			{
				outline = icon.gameObject.AddComponent<Outline>();
			}
			outline.enabled = true;
			outline.effectColor = instance.GetColor(instanceRarity);
			outline.effectDistance = new Vector2(instance.OutlineWidth, instance.OutlineWidth);
			outline.useGraphicAlpha = true;
		}
	}
}
