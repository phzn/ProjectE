using System;
using System.Collections;
using Features.CensorBars;
using Features.RunModes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Features.Inventory.UI
{
	// Token: 0x02001152 RID: 4434
	public class ClothSlotUI : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x1700155B RID: 5467
		// (get) Token: 0x060086E2 RID: 34530 RVA: 0x0037234D File Offset: 0x0037054D
		// (set) Token: 0x060086E3 RID: 34531 RVA: 0x00372355 File Offset: 0x00370555
		public int SlotIndex { get; private set; } = -1;

		// Token: 0x060086E4 RID: 34532 RVA: 0x00372360 File Offset: 0x00370560
		public void Init(int slotIndex, ClothHUDController controller = null)
		{
			this.SlotIndex = slotIndex;
			this._controller = controller;
			this._baseScale = base.transform.localScale;
			if (this.numberLabel != null)
			{
				this.numberLabel.text = (slotIndex + 1).ToString();
			}
			if (this.radialBorder != null)
			{
				this.radialBorder.color = this.radialColor;
			}
			this.Clear();
		}

		// Token: 0x060086E5 RID: 34533 RVA: 0x003723D5 File Offset: 0x003705D5
		public void Bind(Image icon, Image radialBorder, Image selectionFrame, TextMeshProUGUI numberLabel, CanvasGroup group)
		{
			this.icon = icon;
			this.radialBorder = radialBorder;
			this.selectionFrame = selectionFrame;
			this.numberLabel = numberLabel;
			this.canvasGroup = group;
		}

		// Token: 0x060086E6 RID: 34534 RVA: 0x003723FC File Offset: 0x003705FC
		public void SetCloth(InventoryItemSO cloth, float radial01)
		{
			this._cloth = cloth;
			bool flag = cloth != null;
			if (this.icon != null)
			{
				PlayerInventoryManager instance = PlayerInventoryManager.Instance;
				int num = ((this._controller != null) ? this._controller.ResolveClothIndexForUiSlot(this) : this.SlotIndex);
				int num2 = ((flag && instance != null && num >= 0) ? instance.GetClothVariantAt(num) : (-1));
				Sprite sprite = (flag ? ItemVariantLook.ResolveIcon(cloth, num2) : null);
				this.icon.sprite = sprite;
				StreamerCensorImage.BindItem(this.icon, cloth);
				this.icon.enabled = flag && sprite != null;
				RarityUiHelper.ApplyOutline(this.icon, cloth, (flag && instance != null && num >= 0) ? instance.GetClothRarityAt(num) : (flag ? cloth.rarity : ItemRarity.Common));
			}
			if (this.radialBorder != null)
			{
				this.radialBorder.enabled = flag && !RunModeService.IsLongWayHome;
				this.radialBorder.fillAmount = Mathf.Clamp01(radial01);
			}
			if (this.canvasGroup != null)
			{
				this.canvasGroup.alpha = (flag ? this.filledAlpha : this.emptyAlpha);
			}
		}

		// Token: 0x060086E7 RID: 34535 RVA: 0x00372544 File Offset: 0x00370744
		public void Clear()
		{
			this._cloth = null;
			if (this.icon != null)
			{
				this.icon.enabled = false;
			}
			if (this.radialBorder != null)
			{
				this.radialBorder.fillAmount = 1f;
				this.radialBorder.enabled = false;
			}
			if (this.canvasGroup != null)
			{
				this.canvasGroup.alpha = this.emptyAlpha;
			}
			this.SetSelected(false);
		}

		// Token: 0x060086E8 RID: 34536 RVA: 0x003725C4 File Offset: 0x003707C4
		public void SetRadial(float normalized01)
		{
			if (this.radialBorder != null)
			{
				this.radialBorder.enabled = this._cloth != null && !RunModeService.IsLongWayHome;
				this.radialBorder.fillAmount = Mathf.Clamp01(normalized01);
			}
		}

		// Token: 0x060086E9 RID: 34537 RVA: 0x00372614 File Offset: 0x00370814
		public void SetSelected(bool selected)
		{
			bool flag = this.selectionFrame != null && this.selectionFrame.enabled;
			if (this.selectionFrame != null)
			{
				this.selectionFrame.enabled = selected;
			}
			if (selected && !flag && base.isActiveAndEnabled)
			{
				this.PlayPop();
			}
		}

		// Token: 0x1700155C RID: 5468
		// (get) Token: 0x060086EA RID: 34538 RVA: 0x0037266C File Offset: 0x0037086C
		public bool HasCloth
		{
			get
			{
				return this._cloth != null;
			}
		}

		// Token: 0x060086EB RID: 34539 RVA: 0x0037267A File Offset: 0x0037087A
		private void OnDisable()
		{
			if (this._popCo != null)
			{
				base.StopCoroutine(this._popCo);
			}
			this._popCo = null;
			if (this.SlotIndex >= 0)
			{
				base.transform.localScale = this._baseScale;
			}
		}

		// Token: 0x060086EC RID: 34540 RVA: 0x003726B4 File Offset: 0x003708B4
		private void PlayPop()
		{
			if (this.selectPopDuration <= 0f || this.selectPopScale <= 1f)
			{
				return;
			}
			if (this._popCo != null)
			{
				base.StopCoroutine(this._popCo);
			}
			this._popCo = base.StartCoroutine(this.PopRoutine());
		}

		// Token: 0x060086ED RID: 34541 RVA: 0x00372702 File Offset: 0x00370902
		private IEnumerator PopRoutine()
		{
			float t = 0f;
			float dur = Mathf.Max(0.01f, this.selectPopDuration);
			while (t < dur)
			{
				t += Time.unscaledDeltaTime;
				float num = Mathf.Clamp01(t / dur);
				float num2 = 1f + (this.selectPopScale - 1f) * Mathf.Sin(num * 3.1415927f);
				base.transform.localScale = this._baseScale * num2;
				yield return null;
			}
			base.transform.localScale = this._baseScale;
			this._popCo = null;
			yield break;
		}

		// Token: 0x060086EE RID: 34542 RVA: 0x00372714 File Offset: 0x00370914
		public void OnPointerClick(PointerEventData eventData)
		{
			if (this._cloth == null)
			{
				return;
			}
			int num = ((this._controller != null) ? this._controller.ResolveClothIndexForUiSlot(this) : this.SlotIndex);
			if (num >= 0)
			{
				PlayerInventoryManager instance = PlayerInventoryManager.Instance;
				if (instance == null)
				{
					return;
				}
				instance.SelectCloth(num);
			}
		}

		// Token: 0x0400898E RID: 35214
		[Header("References")]
		[SerializeField]
		private Image icon;

		// Token: 0x0400898F RID: 35215
		[Tooltip("Filled / Radial360 border image. fillAmount = remaining wear duration (0..1).")]
		[SerializeField]
		private Image radialBorder;

		// Token: 0x04008990 RID: 35216
		[Tooltip("Highlight shown when this is the interactable (focused) slot.")]
		[SerializeField]
		private Image selectionFrame;

		// Token: 0x04008991 RID: 35217
		[SerializeField]
		private TextMeshProUGUI numberLabel;

		// Token: 0x04008992 RID: 35218
		[Tooltip("Optional CanvasGroup used to dim empty slots.")]
		[SerializeField]
		private CanvasGroup canvasGroup;

		// Token: 0x04008993 RID: 35219
		[Header("Settings")]
		[SerializeField]
		private float emptyAlpha = 0.35f;

		// Token: 0x04008994 RID: 35220
		[SerializeField]
		private float filledAlpha = 1f;

		// Token: 0x04008995 RID: 35221
		[SerializeField]
		private Color radialColor = Color.white;

		// Token: 0x04008996 RID: 35222
		[Header("Animation")]
		[Tooltip("Scale 'pop' played when this slot becomes the focused/interactable one.")]
		[SerializeField]
		private float selectPopScale = 1.18f;

		// Token: 0x04008997 RID: 35223
		[SerializeField]
		private float selectPopDuration = 0.18f;

		// Token: 0x04008999 RID: 35225
		private InventoryItemSO _cloth;

		// Token: 0x0400899A RID: 35226
		private Coroutine _popCo;

		// Token: 0x0400899B RID: 35227
		private Vector3 _baseScale = Vector3.one;

		// Token: 0x0400899C RID: 35228
		private ClothHUDController _controller;
	}
}
