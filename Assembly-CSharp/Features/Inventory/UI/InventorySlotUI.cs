using System;
using Features.CensorBars;
using Features.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Inventory.UI
{
	// Token: 0x02001150 RID: 4432
	public class InventorySlotUI : MonoBehaviour
	{
		// Token: 0x060086BD RID: 34493 RVA: 0x00370E81 File Offset: 0x0036F081
		public void Init(int index)
		{
			this.slotIndex = index;
		}

		// Token: 0x060086BE RID: 34494 RVA: 0x00370E8C File Offset: 0x0036F08C
		public void RefreshInputDevice(bool controller, bool showContextBadge)
		{
			if (this._contextBadge == null)
			{
				Transform transform = base.transform.Find("Keyboard Input");
				if (transform == null)
				{
					return;
				}
				this._contextBadge = new InventoryContextBadge(transform, showContextBadge ? "D-Pad Right" : null, new Vector2(0f, 14f));
			}
			this._contextBadge.Refresh(controller);
		}

		// Token: 0x060086BF RID: 34495 RVA: 0x00370EF0 File Offset: 0x0036F0F0
		public void SetItem(InventoryItemSO item)
		{
			this._item = item;
			if (this.icon != null)
			{
				PlayerInventoryManager instance = PlayerInventoryManager.Instance;
				int num = ((instance != null && item != null) ? instance.GetItemVariantAt(this.slotIndex) : (-1));
				Sprite sprite = ItemVariantLook.ResolveIcon(item, num);
				this.icon.sprite = sprite;
				StreamerCensorImage.BindItem(this.icon, item);
				this.icon.preserveAspect = true;
				this.icon.gameObject.SetActive(item != null && sprite != null);
				RarityUiHelper.ApplyOutline(this.icon, item, (instance != null && item != null) ? instance.GetItemRarityAt(this.slotIndex) : ((item != null) ? item.rarity : ItemRarity.Common));
			}
			if (this.itemNameText != null)
			{
				LocalizedGameText.BindSource(this.itemNameText, "items", (item != null) ? item.itemName : string.Empty, null);
			}
			this.SetStackCount((item != null && PlayerInventoryManager.Instance != null) ? PlayerInventoryManager.Instance.GetItemStackCount(this.slotIndex) : 0);
			this.UpdateDurability(1f);
		}

		// Token: 0x060086C0 RID: 34496 RVA: 0x0037103C File Offset: 0x0036F23C
		public void ClearSlot()
		{
			this._item = null;
			if (this.icon != null)
			{
				this.icon.gameObject.SetActive(false);
			}
			if (this.itemNameText != null)
			{
				LocalizedGameText.BindSource(this.itemNameText, "items", string.Empty, null);
			}
			this.SetStackCount(0);
			if (this.durabilityBar != null)
			{
				this.durabilityBar.gameObject.SetActive(false);
			}
			this.SetSelected(false);
		}

		// Token: 0x060086C1 RID: 34497 RVA: 0x003710C4 File Offset: 0x0036F2C4
		public void UpdateDurability(float normalizedValue)
		{
			if (this.durabilityBar != null)
			{
				bool flag = this._item != null && this._item.hasDurability;
				this.durabilityBar.gameObject.SetActive(flag);
				if (flag)
				{
					this.durabilityBar.fillAmount = normalizedValue;
				}
			}
		}

		// Token: 0x060086C2 RID: 34498 RVA: 0x0037111C File Offset: 0x0036F31C
		public void SetSelected(bool isSelected)
		{
			if (this.selectionFrame != null)
			{
				this.selectionFrame.gameObject.SetActive(isSelected);
				this.selectionFrame.color = (isSelected ? this.activeColor : this.inactiveColor);
			}
		}

		// Token: 0x060086C3 RID: 34499 RVA: 0x0037115C File Offset: 0x0036F35C
		public void SetStackCount(int quantity)
		{
			this.EnsureStackCountText();
			if (this.stackCountText == null)
			{
				return;
			}
			bool flag = this._item != null && this._item.isStackable && quantity > 1;
			this.stackCountText.text = (flag ? ("x" + quantity.ToString()) : string.Empty);
			this.stackCountText.gameObject.SetActive(flag);
		}

		// Token: 0x060086C4 RID: 34500 RVA: 0x003711D8 File Offset: 0x0036F3D8
		private void EnsureStackCountText()
		{
			if (this.stackCountText == null)
			{
				this.stackCountText = new GameObject("StackCount", new Type[]
				{
					typeof(RectTransform),
					typeof(TextMeshProUGUI)
				})
				{
					layer = base.gameObject.layer
				}.GetComponent<TextMeshProUGUI>();
			}
			RectTransform rectTransform = this.stackCountText.rectTransform;
			Transform transform = ((this.icon != null) ? this.icon.transform : base.transform);
			if (rectTransform.parent != transform)
			{
				rectTransform.SetParent(transform, false);
			}
			RectTransform rectTransform2 = rectTransform;
			RectTransform rectTransform3 = rectTransform;
			RectTransform rectTransform4 = rectTransform;
			Vector2 vector = new Vector2(1f, 1f);
			rectTransform4.pivot = vector;
			rectTransform2.anchorMin = (rectTransform3.anchorMax = vector);
			rectTransform.anchoredPosition = new Vector2(-3f, -2f);
			rectTransform.sizeDelta = new Vector2(34f, 18f);
			rectTransform.localScale = Vector3.one;
			if (this.itemNameText != null)
			{
				this.stackCountText.font = this.itemNameText.font;
			}
			this.stackCountText.enableAutoSizing = false;
			this.stackCountText.fontSize = 14f;
			this.stackCountText.fontStyle = FontStyles.Bold;
			this.stackCountText.alignment = TextAlignmentOptions.TopRight;
			this.stackCountText.color = Color.white;
			this.stackCountText.outlineColor = new Color32(65, 16, 52, byte.MaxValue);
			this.stackCountText.outlineWidth = 0.16f;
			this.stackCountText.enableWordWrapping = false;
			this.stackCountText.overflowMode = TextOverflowModes.Overflow;
			this.stackCountText.raycastTarget = false;
			rectTransform.SetAsLastSibling();
		}

		// Token: 0x060086C5 RID: 34501 RVA: 0x0037139F File Offset: 0x0036F59F
		public void OnClick()
		{
			if (PlayerInventoryManager.Instance != null)
			{
				PlayerInventoryManager.Instance.SelectSlot(this.slotIndex);
			}
		}

		// Token: 0x0400896E RID: 35182
		[Header("References")]
		[SerializeField]
		private Image icon;

		// Token: 0x0400896F RID: 35183
		[SerializeField]
		private Image selectionFrame;

		// Token: 0x04008970 RID: 35184
		[SerializeField]
		private TextMeshProUGUI itemNameText;

		// Token: 0x04008971 RID: 35185
		[SerializeField]
		private TextMeshProUGUI stackCountText;

		// Token: 0x04008972 RID: 35186
		[SerializeField]
		private Image durabilityBar;

		// Token: 0x04008973 RID: 35187
		[SerializeField]
		private int slotIndex;

		// Token: 0x04008974 RID: 35188
		[Header("Settings")]
		[SerializeField]
		private Color activeColor = Color.white;

		// Token: 0x04008975 RID: 35189
		[SerializeField]
		private Color inactiveColor = new Color(1f, 1f, 1f, 0.5f);

		// Token: 0x04008976 RID: 35190
		private InventoryItemSO _item;

		// Token: 0x04008977 RID: 35191
		private InventoryContextBadge _contextBadge;
	}
}
