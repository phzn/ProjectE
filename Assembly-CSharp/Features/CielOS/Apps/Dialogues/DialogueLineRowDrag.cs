using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Features.CielOS.Apps.Dialogues
{
	// Token: 0x020014A9 RID: 5289
	public class DialogueLineRowDrag : MonoBehaviour, IBeginDragHandler, IEventSystemHandler, IDragHandler, IEndDragHandler
	{
		// Token: 0x17001915 RID: 6421
		// (get) Token: 0x0600A2D5 RID: 41685 RVA: 0x00439982 File Offset: 0x00437B82
		// (set) Token: 0x0600A2D6 RID: 41686 RVA: 0x0043998A File Offset: 0x00437B8A
		public int OriginalIndex { get; private set; }

		// Token: 0x17001916 RID: 6422
		// (get) Token: 0x0600A2D7 RID: 41687 RVA: 0x00439993 File Offset: 0x00437B93
		public RectTransform Row
		{
			get
			{
				return this._row;
			}
		}

		// Token: 0x0600A2D8 RID: 41688 RVA: 0x0043999C File Offset: 0x00437B9C
		public void MoveWithController(int direction)
		{
			if (this._row == null || this._row.parent == null || direction == 0)
			{
				return;
			}
			int siblingIndex = this._row.GetSiblingIndex();
			int num = Mathf.Clamp(siblingIndex + ((direction > 0) ? 1 : (-1)), 0, this._row.parent.childCount - 1);
			if (siblingIndex == num)
			{
				return;
			}
			this._row.SetSiblingIndex(num);
			Action onReordered = this._onReordered;
			if (onReordered == null)
			{
				return;
			}
			onReordered();
		}

		// Token: 0x0600A2D9 RID: 41689 RVA: 0x00439A1C File Offset: 0x00437C1C
		public void Init(int originalIndex, RectTransform row, Action onReordered)
		{
			this.OriginalIndex = originalIndex;
			this._row = ((row != null) ? row : ((RectTransform)base.transform));
			this._onReordered = onReordered;
			this._rowCg = this._row.GetComponent<CanvasGroup>();
			if (this._rowCg == null)
			{
				this._rowCg = this._row.gameObject.AddComponent<CanvasGroup>();
			}
		}

		// Token: 0x0600A2DA RID: 41690 RVA: 0x00439A8C File Offset: 0x00437C8C
		public void OnBeginDrag(PointerEventData e)
		{
			this._parent = this._row.parent as RectTransform;
			if (this._rowCg != null)
			{
				this._rowCg.blocksRaycasts = false;
				this._rowCg.alpha = 0.7f;
			}
		}

		// Token: 0x0600A2DB RID: 41691 RVA: 0x00439ADC File Offset: 0x00437CDC
		public void OnDrag(PointerEventData e)
		{
			if (this._parent == null)
			{
				return;
			}
			int i = 0;
			while (i < this._parent.childCount)
			{
				RectTransform rectTransform = (RectTransform)this._parent.GetChild(i);
				if (!(rectTransform == this._row) && rectTransform.gameObject.activeSelf && RectTransformUtility.RectangleContainsScreenPoint(rectTransform, e.position, e.pressEventCamera))
				{
					if (this._row.GetSiblingIndex() != i)
					{
						this._row.SetSiblingIndex(i);
						return;
					}
					break;
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x0600A2DC RID: 41692 RVA: 0x00439B6A File Offset: 0x00437D6A
		public void OnEndDrag(PointerEventData e)
		{
			if (this._rowCg != null)
			{
				this._rowCg.blocksRaycasts = true;
				this._rowCg.alpha = 1f;
			}
			Action onReordered = this._onReordered;
			if (onReordered == null)
			{
				return;
			}
			onReordered();
		}

		// Token: 0x0400A693 RID: 42643
		private RectTransform _row;

		// Token: 0x0400A694 RID: 42644
		private RectTransform _parent;

		// Token: 0x0400A695 RID: 42645
		private CanvasGroup _rowCg;

		// Token: 0x0400A696 RID: 42646
		private Action _onReordered;
	}
}
