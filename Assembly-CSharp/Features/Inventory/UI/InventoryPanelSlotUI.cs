using System;
using System.Collections;
using Features.CensorBars;
using Features.Inventory.Wearable;
using Features.RunModes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Features.Inventory.UI
{
	// Token: 0x0200115C RID: 4444
	public class InventoryPanelSlotUI : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
	{
		// Token: 0x17001565 RID: 5477
		// (get) Token: 0x06008756 RID: 34646 RVA: 0x0037670A File Offset: 0x0037490A
		// (set) Token: 0x06008757 RID: 34647 RVA: 0x00376712 File Offset: 0x00374912
		public InventoryPanelSlotUI.Collection Kind { get; private set; }

		// Token: 0x17001566 RID: 5478
		// (get) Token: 0x06008758 RID: 34648 RVA: 0x0037671B File Offset: 0x0037491B
		// (set) Token: 0x06008759 RID: 34649 RVA: 0x00376723 File Offset: 0x00374923
		public int SlotIndex { get; private set; } = -1;

		// Token: 0x17001567 RID: 5479
		// (get) Token: 0x0600875A RID: 34650 RVA: 0x0037672C File Offset: 0x0037492C
		// (set) Token: 0x0600875B RID: 34651 RVA: 0x00376734 File Offset: 0x00374934
		public InventoryItemSO Item { get; private set; }

		// Token: 0x0600875C RID: 34652 RVA: 0x0037673D File Offset: 0x0037493D
		public void Bind(Image icon, Image selectionFrame, Image durabilityBar, Image dropHighlight = null)
		{
			this.icon = icon;
			this.selectionFrame = selectionFrame;
			this.durabilityBar = durabilityBar;
			this.dropHighlight = dropHighlight;
		}

		// Token: 0x0600875D RID: 34653 RVA: 0x0037675C File Offset: 0x0037495C
		public void Init(InventoryDialogController dialog, InventoryPanelSlotUI.Collection kind, int slotIndex)
		{
			this._dialog = dialog;
			this.Kind = kind;
			this.SlotIndex = slotIndex;
			this.StabilizeSlotLayout();
			this.SetItem(null, 1f);
			this.SetSelected(false);
			this.SetDropHighlight(false);
			this.SetDragSource(false);
		}

		// Token: 0x0600875E RID: 34654 RVA: 0x0037679C File Offset: 0x0037499C
		private void StabilizeSlotLayout()
		{
			RectTransform rectTransform = base.transform as RectTransform;
			RectTransform rectTransform2 = ((this.icon != null) ? (this.icon.transform.parent as RectTransform) : null);
			if (rectTransform == null || rectTransform2 == null || rectTransform2 == rectTransform || rectTransform2.parent != rectTransform || rectTransform.childCount != 1 || rectTransform2.anchorMin != rectTransform2.anchorMax)
			{
				return;
			}
			Vector2 size = rectTransform2.rect.size;
			if (size.x <= 0f || size.y <= 0f)
			{
				return;
			}
			LayoutGroup component = base.GetComponent<LayoutGroup>();
			if (component != null)
			{
				component.enabled = false;
			}
			ContentSizeFitter component2 = base.GetComponent<ContentSizeFitter>();
			if (component2 != null)
			{
				component2.enabled = false;
			}
			rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
			rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
			RectTransform rectTransform3 = rectTransform2;
			RectTransform rectTransform4 = rectTransform2;
			Vector2 vector = new Vector2(0.5f, 0.5f);
			rectTransform4.anchorMax = vector;
			rectTransform3.anchorMin = vector;
			rectTransform2.anchoredPosition = Vector2.zero;
			LayoutElement layoutElement = base.GetComponent<LayoutElement>();
			if (layoutElement == null)
			{
				layoutElement = base.gameObject.AddComponent<LayoutElement>();
			}
			layoutElement.minWidth = (layoutElement.preferredWidth = size.x);
			layoutElement.minHeight = (layoutElement.preferredHeight = size.y);
		}

		// Token: 0x0600875F RID: 34655 RVA: 0x00376918 File Offset: 0x00374B18
		public void SetItem(InventoryItemSO item, float durability01)
		{
			this.Item = item;
			bool flag = item != null;
			if (this.icon != null)
			{
				PlayerInventoryManager instance = PlayerInventoryManager.Instance;
				int num = ((!flag || instance == null) ? (-1) : ((this.Kind == InventoryPanelSlotUI.Collection.Clothing) ? instance.GetClothVariantAt(this.SlotIndex) : instance.GetItemVariantAt(this.SlotIndex)));
				Sprite sprite = (flag ? ItemVariantLook.ResolveIcon(item, num) : null);
				this.icon.sprite = sprite;
				StreamerCensorImage.BindItem(this.icon, item);
				this.icon.preserveAspect = true;
				InventoryPanelSlotUI.SetGoActive(this.icon, flag && sprite != null);
				ItemRarity itemRarity = ((!flag) ? ItemRarity.Common : ((instance == null) ? item.rarity : ((this.Kind == InventoryPanelSlotUI.Collection.Clothing) ? instance.GetClothRarityAt(this.SlotIndex) : instance.GetItemRarityAt(this.SlotIndex))));
				RarityUiHelper.ApplyOutline(this.icon, item, itemRarity);
			}
			if (this.durabilityBar != null)
			{
				bool flag2;
				if (flag)
				{
					if (!item.hasDurability)
					{
						WearableItemSO wearableItemSO = item as WearableItemSO;
						flag2 = wearableItemSO != null && wearableItemSO.hasWearDuration && wearableItemSO.wearDuration > 0f;
					}
					else
					{
						flag2 = true;
					}
				}
				else
				{
					flag2 = false;
				}
				bool flag3 = flag2;
				if (this.Kind == InventoryPanelSlotUI.Collection.Clothing && RunModeService.IsLongWayHome)
				{
					flag3 = false;
				}
				if (flag3)
				{
					this.durabilityBar.fillAmount = Mathf.Clamp01(durability01);
				}
				InventoryPanelSlotUI.SetGoActive(this.durabilityBar, flag3);
			}
			this.SetStackCount((flag && this.Kind == InventoryPanelSlotUI.Collection.Item && PlayerInventoryManager.Instance != null) ? PlayerInventoryManager.Instance.GetItemStackCount(this.SlotIndex) : 0);
		}

		// Token: 0x06008760 RID: 34656 RVA: 0x00376AC0 File Offset: 0x00374CC0
		public void SetStackCount(int quantity)
		{
			this.EnsureStackCountText();
			if (this.stackCountText == null)
			{
				return;
			}
			bool flag = this.Item != null && this.Item.isStackable && quantity > 1;
			this.stackCountText.text = (flag ? ("x" + quantity.ToString()) : string.Empty);
			this.stackCountText.gameObject.SetActive(flag);
		}

		// Token: 0x06008761 RID: 34657 RVA: 0x00376B3C File Offset: 0x00374D3C
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
			rectTransform.anchoredPosition = new Vector2(-4f, -3f);
			rectTransform.sizeDelta = new Vector2(38f, 20f);
			rectTransform.localScale = Vector3.one;
			this.stackCountText.enableAutoSizing = false;
			this.stackCountText.fontSize = 15f;
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

		// Token: 0x06008762 RID: 34658 RVA: 0x00376CE0 File Offset: 0x00374EE0
		public void SetSelected(bool selected)
		{
			bool flag = this.selectionFrame != null && this.selectionFrame.gameObject.activeSelf;
			if (this.selectionFrame != null)
			{
				InventoryPanelSlotUI.SetGoActive(this.selectionFrame, selected);
			}
			if (selected && !flag && base.isActiveAndEnabled)
			{
				this.PlayPop();
			}
		}

		// Token: 0x06008763 RID: 34659 RVA: 0x00376D3D File Offset: 0x00374F3D
		private static void SetGoActive(Image img, bool on)
		{
			if (img == null)
			{
				return;
			}
			img.enabled = on;
			if (img.gameObject.activeSelf != on)
			{
				img.gameObject.SetActive(on);
			}
		}

		// Token: 0x06008764 RID: 34660 RVA: 0x00376D6A File Offset: 0x00374F6A
		private void OnDisable()
		{
			if (this._popCo != null)
			{
				base.StopCoroutine(this._popCo);
			}
			this._popCo = null;
			if (this._baseScaleCaptured)
			{
				base.transform.localScale = this._baseScale;
			}
			this.SetDragSource(false);
		}

		// Token: 0x06008765 RID: 34661 RVA: 0x00376DA8 File Offset: 0x00374FA8
		private void PlayPop()
		{
			if (!this._baseScaleCaptured)
			{
				this._baseScale = base.transform.localScale;
				this._baseScaleCaptured = true;
			}
			if (this._popCo != null)
			{
				base.StopCoroutine(this._popCo);
			}
			this._popCo = base.StartCoroutine(this.PopRoutine());
		}

		// Token: 0x06008766 RID: 34662 RVA: 0x00376DFB File Offset: 0x00374FFB
		private IEnumerator PopRoutine()
		{
			float t = 0f;
			float dur = 0.16f;
			while (t < dur)
			{
				t += Time.unscaledDeltaTime;
				float num = Mathf.Clamp01(t / dur);
				base.transform.localScale = this._baseScale * (1f + 0.12f * Mathf.Sin(num * 3.1415927f));
				yield return null;
			}
			base.transform.localScale = this._baseScale;
			this._popCo = null;
			yield break;
		}

		// Token: 0x17001568 RID: 5480
		// (get) Token: 0x06008767 RID: 34663 RVA: 0x00376E0A File Offset: 0x0037500A
		public bool HasItem
		{
			get
			{
				return this.Item != null;
			}
		}

		// Token: 0x06008768 RID: 34664 RVA: 0x00376E18 File Offset: 0x00375018
		public void SetDropHighlight(bool on)
		{
			InventoryPanelSlotUI.SetGoActive(this.dropHighlight, on);
		}

		// Token: 0x06008769 RID: 34665 RVA: 0x00376E28 File Offset: 0x00375028
		public void SetDragSource(bool dragging)
		{
			if (this.icon == null)
			{
				return;
			}
			Color color = this.icon.color;
			color.a = (dragging ? 0.35f : 1f);
			this.icon.color = color;
		}

		// Token: 0x0600876A RID: 34666 RVA: 0x00376E72 File Offset: 0x00375072
		public void OnPointerClick(PointerEventData eventData)
		{
			if (eventData == null || eventData.button != PointerEventData.InputButton.Left || eventData.dragging)
			{
				return;
			}
			InventoryDialogController dialog = this._dialog;
			if (dialog == null)
			{
				return;
			}
			dialog.OnSlotClicked(this);
		}

		// Token: 0x0600876B RID: 34667 RVA: 0x00376E99 File Offset: 0x00375099
		public void OnBeginDrag(PointerEventData eventData)
		{
			if (!this.HasItem)
			{
				return;
			}
			InventoryDialogController dialog = this._dialog;
			if (dialog == null)
			{
				return;
			}
			dialog.BeginSlotDrag(this, eventData);
		}

		// Token: 0x0600876C RID: 34668 RVA: 0x00376EB6 File Offset: 0x003750B6
		public void OnDrag(PointerEventData eventData)
		{
			InventoryDialogController dialog = this._dialog;
			if (dialog == null)
			{
				return;
			}
			dialog.UpdateSlotDrag(eventData);
		}

		// Token: 0x0600876D RID: 34669 RVA: 0x00376EC9 File Offset: 0x003750C9
		public void OnEndDrag(PointerEventData eventData)
		{
			InventoryDialogController dialog = this._dialog;
			if (dialog == null)
			{
				return;
			}
			dialog.EndSlotDrag(this, eventData);
		}

		// Token: 0x04008A01 RID: 35329
		[Header("References")]
		[SerializeField]
		private Image icon;

		// Token: 0x04008A02 RID: 35330
		[SerializeField]
		private Image selectionFrame;

		// Token: 0x04008A03 RID: 35331
		[SerializeField]
		private Image durabilityBar;

		// Token: 0x04008A04 RID: 35332
		[SerializeField]
		private TextMeshProUGUI stackCountText;

		// Token: 0x04008A05 RID: 35333
		[Tooltip("Optional frame tinted/shown while a dragged slot hovers over this one (drop target).")]
		[SerializeField]
		private Image dropHighlight;

		// Token: 0x04008A09 RID: 35337
		private InventoryDialogController _dialog;

		// Token: 0x04008A0A RID: 35338
		private Coroutine _popCo;

		// Token: 0x04008A0B RID: 35339
		private Vector3 _baseScale = Vector3.one;

		// Token: 0x04008A0C RID: 35340
		private bool _baseScaleCaptured;

		// Token: 0x0200115D RID: 4445
		public enum Collection
		{
			// Token: 0x04008A0E RID: 35342
			Item,
			// Token: 0x04008A0F RID: 35343
			Clothing
		}
	}
}
