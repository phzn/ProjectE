using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Features.PhoneCompanion
{
	// Token: 0x02000E44 RID: 3652
	[RequireComponent(typeof(RectTransform))]
	public class PhoneHomeIconDrag : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
	{
		// Token: 0x06006D28 RID: 27944 RVA: 0x002BC83C File Offset: 0x002BAA3C
		public void Init(PhoneHomeGridReorder owner)
		{
			this._owner = owner;
			this._rect = (RectTransform)base.transform;
			this._gridRect = (RectTransform)owner.transform;
			this._canvas = base.GetComponentInParent<Canvas>();
			this._button = base.GetComponent<Button>();
			this._baseScale = this._rect.localScale;
		}

		// Token: 0x06006D29 RID: 27945 RVA: 0x002BC89C File Offset: 0x002BAA9C
		private void Awake()
		{
			if (this._rect == null)
			{
				this._rect = (RectTransform)base.transform;
			}
			if (this._baseScale == Vector3.zero)
			{
				this._baseScale = this._rect.localScale;
			}
		}

		// Token: 0x170011EE RID: 4590
		// (get) Token: 0x06006D2A RID: 27946 RVA: 0x002BC8EB File Offset: 0x002BAAEB
		private Camera EventCamera
		{
			get
			{
				if (this._canvas == null)
				{
					return null;
				}
				if (this._canvas.renderMode != RenderMode.ScreenSpaceOverlay)
				{
					return this._canvas.worldCamera;
				}
				return null;
			}
		}

		// Token: 0x06006D2B RID: 27947 RVA: 0x002BC917 File Offset: 0x002BAB17
		public void OnPointerDown(PointerEventData eventData)
		{
			if (this._owner == null)
			{
				return;
			}
			this._pointerDown = true;
			this._armed = false;
			this._downTime = Time.unscaledTime;
			this._downScreenPos = eventData.position;
		}

		// Token: 0x06006D2C RID: 27948 RVA: 0x002BC950 File Offset: 0x002BAB50
		public void OnPointerUp(PointerEventData eventData)
		{
			this._pointerDown = false;
			this._armed = false;
			if (this._inEditMode && !this._dragging)
			{
				this._inEditMode = false;
				this._rect.localScale = this._baseScale;
				this._owner.ExitEditMode(this._rect);
			}
		}

		// Token: 0x06006D2D RID: 27949 RVA: 0x002BC9A4 File Offset: 0x002BABA4
		private void Update()
		{
			if (this._pointerDown && !this._armed && !this._dragging && Time.unscaledTime - this._downTime >= this._owner.HoldToDrag)
			{
				this._armed = true;
				this._rect.localScale = this._baseScale * this._owner.DragScale;
				if (!this._inEditMode)
				{
					this._inEditMode = true;
					this._owner.EnterEditMode(null);
				}
			}
			if (!this._dragging)
			{
				return;
			}
			float num = ((this._owner.FollowLerp <= 0f) ? 1f : (1f - Mathf.Exp(-this._owner.FollowLerp * Time.unscaledDeltaTime)));
			this._rect.anchoredPosition = Vector2.Lerp(this._rect.anchoredPosition, this._targetAnchoredPos, num);
		}

		// Token: 0x06006D2E RID: 27950 RVA: 0x002BCA87 File Offset: 0x002BAC87
		public void OnBeginDrag(PointerEventData eventData)
		{
			if (this._owner == null || !this._armed)
			{
				return;
			}
			this.BeginActualDrag(eventData);
		}

		// Token: 0x06006D2F RID: 27951 RVA: 0x002BCAA8 File Offset: 0x002BACA8
		private void BeginActualDrag(PointerEventData eventData)
		{
			if (this._dragging)
			{
				return;
			}
			this._dragging = true;
			if (!this._inEditMode)
			{
				this._inEditMode = true;
				this._owner.EnterEditMode(this._rect);
			}
			else
			{
				this._owner.SetDraggedIcon(this._rect);
			}
			this._rect.sizeDelta = this._owner.Grid.cellSize;
			this._rect.SetAsLastSibling();
			this._lastSiblingIndex = this._rect.GetSiblingIndex();
			this._rect.localScale = this._baseScale * this._owner.DragScale;
			if (this._button != null)
			{
				this._button.interactable = false;
			}
			this._grabOffset = Vector2.zero;
			Vector2 vector;
			if (this.TryPointerToAnchored(eventData.position, out vector))
			{
				this._targetAnchoredPos = vector + this._grabOffset;
			}
			else
			{
				this._targetAnchoredPos = this._rect.anchoredPosition;
			}
			this._rect.anchoredPosition = this._targetAnchoredPos;
		}

		// Token: 0x06006D30 RID: 27952 RVA: 0x002BCBBC File Offset: 0x002BADBC
		public void OnDrag(PointerEventData eventData)
		{
			if (!this._dragging && this._armed)
			{
				this.BeginActualDrag(eventData);
			}
			if (!this._dragging)
			{
				return;
			}
			Vector2 vector;
			if (this.TryPointerToAnchored(eventData.position, out vector))
			{
				this._targetAnchoredPos = vector + this._grabOffset;
			}
			this.UpdateInsertionIndex(this._targetAnchoredPos);
		}

		// Token: 0x06006D31 RID: 27953 RVA: 0x002BCC18 File Offset: 0x002BAE18
		private bool TryPointerToAnchored(Vector2 screenPos, out Vector2 anchored)
		{
			anchored = default(Vector2);
			Vector2 vector;
			if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(this._gridRect, screenPos, this.EventCamera, out vector))
			{
				return false;
			}
			Rect rect = this._gridRect.rect;
			Vector2 anchorMin = this._rect.anchorMin;
			float num = rect.xMin + anchorMin.x * rect.width;
			float num2 = rect.yMin + anchorMin.y * rect.height;
			anchored = new Vector2(vector.x - num, vector.y - num2);
			return true;
		}

		// Token: 0x06006D32 RID: 27954 RVA: 0x002BCCA8 File Offset: 0x002BAEA8
		private void UpdateInsertionIndex(Vector2 anchoredPoint)
		{
			int childCount = this._owner.transform.childCount;
			if (childCount <= 1)
			{
				return;
			}
			int num = 0;
			float num2 = float.MaxValue;
			for (int i = 0; i < childCount; i++)
			{
				float sqrMagnitude = (this._owner.GetSlotPosition(i) - anchoredPoint).sqrMagnitude;
				if (sqrMagnitude < num2)
				{
					num2 = sqrMagnitude;
					num = i;
				}
			}
			if (num != this._rect.GetSiblingIndex())
			{
				this._rect.SetSiblingIndex(num);
				this._lastSiblingIndex = this._rect.GetSiblingIndex();
			}
		}

		// Token: 0x06006D33 RID: 27955 RVA: 0x002BCD34 File Offset: 0x002BAF34
		public void OnEndDrag(PointerEventData eventData)
		{
			this._pointerDown = false;
			this._armed = false;
			if (!this._dragging)
			{
				this._rect.localScale = this._baseScale;
				return;
			}
			this._dragging = false;
			this._rect.localScale = this._baseScale;
			if (this._inEditMode)
			{
				this._inEditMode = false;
				this._owner.ExitEditMode(this._rect);
			}
			this._owner.SaveOrder();
			if (this._button != null)
			{
				base.StartCoroutine(this.ReenableButtonAfterClick());
			}
		}

		// Token: 0x06006D34 RID: 27956 RVA: 0x002BCDC7 File Offset: 0x002BAFC7
		private IEnumerator ReenableButtonAfterClick()
		{
			yield return null;
			yield return null;
			yield return new WaitForEndOfFrame();
			if (this._button != null)
			{
				this._button.interactable = true;
			}
			yield break;
		}

		// Token: 0x04006FCC RID: 28620
		private PhoneHomeGridReorder _owner;

		// Token: 0x04006FCD RID: 28621
		private RectTransform _rect;

		// Token: 0x04006FCE RID: 28622
		private RectTransform _gridRect;

		// Token: 0x04006FCF RID: 28623
		private Canvas _canvas;

		// Token: 0x04006FD0 RID: 28624
		private Button _button;

		// Token: 0x04006FD1 RID: 28625
		private bool _pointerDown;

		// Token: 0x04006FD2 RID: 28626
		private bool _armed;

		// Token: 0x04006FD3 RID: 28627
		private bool _dragging;

		// Token: 0x04006FD4 RID: 28628
		private bool _inEditMode;

		// Token: 0x04006FD5 RID: 28629
		private float _downTime;

		// Token: 0x04006FD6 RID: 28630
		private Vector2 _downScreenPos;

		// Token: 0x04006FD7 RID: 28631
		private Vector3 _baseScale;

		// Token: 0x04006FD8 RID: 28632
		private Vector2 _grabOffset;

		// Token: 0x04006FD9 RID: 28633
		private Vector2 _targetAnchoredPos;

		// Token: 0x04006FDA RID: 28634
		private int _lastSiblingIndex;
	}
}
