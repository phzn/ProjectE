using System;
using Features.CielOS.UI;
using Features.CielOS.VFS;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Features.CielOS.Apps
{
	// Token: 0x020013F1 RID: 5105
	public sealed class CielOSMediaTileDrag : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
	{
		// Token: 0x06009D9D RID: 40349 RVA: 0x0040C650 File Offset: 0x0040A850
		public void Bind(CielOSPhotosApp owner, CielOSFileSystem.Entry entry, ScrollRect scroll)
		{
			this._owner = owner;
			this._entry = entry;
			this._scroll = scroll;
			this._canvas = base.GetComponentInParent<Canvas>();
			this._sourceGroup = base.GetComponent<CanvasGroup>();
			if (this._sourceGroup == null)
			{
				this._sourceGroup = base.gameObject.AddComponent<CanvasGroup>();
			}
		}

		// Token: 0x06009D9E RID: 40350 RVA: 0x0040C6A9 File Offset: 0x0040A8A9
		public bool ConsumeClickSuppression()
		{
			bool suppressClick = this._suppressClick;
			this._suppressClick = false;
			return suppressClick;
		}

		// Token: 0x06009D9F RID: 40351 RVA: 0x0040C6B8 File Offset: 0x0040A8B8
		public void OnPointerDown(PointerEventData eventData)
		{
			if (eventData == null || eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			this._pressedAt = Time.unscaledTime;
			this._mediaDrag = false;
			this._forwardingScroll = false;
			ScrollRect scroll = this._scroll;
			if (scroll != null)
			{
				CielOSSmoothWheelScroll component = scroll.GetComponent<CielOSSmoothWheelScroll>();
				if (component != null)
				{
					component.ResetTarget();
				}
			}
			ScrollRect scroll2 = this._scroll;
			if (scroll2 == null)
			{
				return;
			}
			scroll2.OnInitializePotentialDrag(eventData);
		}

		// Token: 0x06009DA0 RID: 40352 RVA: 0x0040C717 File Offset: 0x0040A917
		public void OnPointerUp(PointerEventData eventData)
		{
			if (!this._mediaDrag && !this._forwardingScroll)
			{
				this._pressedAt = 0f;
			}
		}

		// Token: 0x06009DA1 RID: 40353 RVA: 0x0040C734 File Offset: 0x0040A934
		public void OnBeginDrag(PointerEventData eventData)
		{
			if (eventData == null || eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			if (Time.unscaledTime - this._pressedAt >= 0.24f)
			{
				this._mediaDrag = true;
				this._suppressClick = true;
				if (this._sourceGroup != null)
				{
					this._sourceGroup.alpha = 0.3f;
					this._sourceGroup.blocksRaycasts = false;
				}
				this.CreateGhost(eventData);
				CielOSPhotosApp owner = this._owner;
				if (owner == null)
				{
					return;
				}
				owner.BeginMediaDrag(this._entry);
				return;
			}
			else
			{
				this._forwardingScroll = true;
				ScrollRect scroll = this._scroll;
				if (scroll == null)
				{
					return;
				}
				scroll.OnBeginDrag(eventData);
				return;
			}
		}

		// Token: 0x06009DA2 RID: 40354 RVA: 0x0040C7D0 File Offset: 0x0040A9D0
		public void OnDrag(PointerEventData eventData)
		{
			if (!this._mediaDrag)
			{
				if (this._forwardingScroll)
				{
					ScrollRect scroll = this._scroll;
					if (scroll == null)
					{
						return;
					}
					scroll.OnDrag(eventData);
				}
				return;
			}
			this.MoveGhost(eventData);
			CielOSPhotosApp owner = this._owner;
			if (owner == null)
			{
				return;
			}
			owner.UpdateMediaDrag(this._entry, eventData.position, eventData.pressEventCamera);
		}

		// Token: 0x06009DA3 RID: 40355 RVA: 0x0040C828 File Offset: 0x0040AA28
		public void OnEndDrag(PointerEventData eventData)
		{
			if (this._mediaDrag)
			{
				CielOSPhotosApp owner = this._owner;
				if (owner != null)
				{
					owner.EndMediaDrag(this._entry, eventData.position, eventData.pressEventCamera);
				}
			}
			else if (this._forwardingScroll)
			{
				ScrollRect scroll = this._scroll;
				if (scroll != null)
				{
					scroll.OnEndDrag(eventData);
				}
			}
			this.RestoreSource();
			this._mediaDrag = false;
			this._forwardingScroll = false;
			this._pressedAt = 0f;
		}

		// Token: 0x06009DA4 RID: 40356 RVA: 0x0040C89C File Offset: 0x0040AA9C
		private void CreateGhost(PointerEventData eventData)
		{
			if (this._canvas == null)
			{
				return;
			}
			GameObject gameObject = Object.Instantiate<GameObject>(base.gameObject, this._canvas.transform, false);
			gameObject.name = base.name + " Drag Preview";
			this._ghost = gameObject.transform as RectTransform;
			if (this._ghost == null)
			{
				Object.Destroy(gameObject);
				return;
			}
			Selectable[] componentsInChildren = gameObject.GetComponentsInChildren<Selectable>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = false;
			}
			Graphic[] componentsInChildren2 = gameObject.GetComponentsInChildren<Graphic>(true);
			for (int i = 0; i < componentsInChildren2.Length; i++)
			{
				componentsInChildren2[i].raycastTarget = false;
			}
			CielOSMediaTileDrag[] componentsInChildren3 = gameObject.GetComponentsInChildren<CielOSMediaTileDrag>(true);
			for (int i = 0; i < componentsInChildren3.Length; i++)
			{
				componentsInChildren3[i].enabled = false;
			}
			CielOSClickable[] componentsInChildren4 = gameObject.GetComponentsInChildren<CielOSClickable>(true);
			for (int i = 0; i < componentsInChildren4.Length; i++)
			{
				componentsInChildren4[i].enabled = false;
			}
			CielOSRightClickable[] componentsInChildren5 = gameObject.GetComponentsInChildren<CielOSRightClickable>(true);
			for (int i = 0; i < componentsInChildren5.Length; i++)
			{
				componentsInChildren5[i].enabled = false;
			}
			CanvasGroup canvasGroup = gameObject.GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();
			canvasGroup.alpha = 0.86f;
			canvasGroup.blocksRaycasts = false;
			RectTransform ghost = this._ghost;
			RectTransform ghost2 = this._ghost;
			RectTransform ghost3 = this._ghost;
			Vector2 vector = new Vector2(0.5f, 0.5f);
			ghost3.pivot = vector;
			ghost.anchorMin = (ghost2.anchorMax = vector);
			this._ghost.SetAsLastSibling();
			this.MoveGhost(eventData);
		}

		// Token: 0x06009DA5 RID: 40357 RVA: 0x0040CA20 File Offset: 0x0040AC20
		private void MoveGhost(PointerEventData eventData)
		{
			if (this._ghost == null || this._canvas == null || eventData == null)
			{
				return;
			}
			RectTransform rectTransform = this._canvas.transform as RectTransform;
			Vector2 vector;
			if (rectTransform != null && RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out vector))
			{
				this._ghost.anchoredPosition = vector;
				return;
			}
			this._ghost.position = eventData.position;
		}

		// Token: 0x06009DA6 RID: 40358 RVA: 0x0040CAA0 File Offset: 0x0040ACA0
		private void RestoreSource()
		{
			if (this._sourceGroup != null)
			{
				this._sourceGroup.alpha = 1f;
				this._sourceGroup.blocksRaycasts = true;
			}
			if (this._ghost != null)
			{
				Object.Destroy(this._ghost.gameObject);
			}
			this._ghost = null;
		}

		// Token: 0x06009DA7 RID: 40359 RVA: 0x0040CAFC File Offset: 0x0040ACFC
		private void OnDisable()
		{
			this.RestoreSource();
			this._mediaDrag = false;
			this._forwardingScroll = false;
		}

		// Token: 0x0400A209 RID: 41481
		private const float HoldSeconds = 0.24f;

		// Token: 0x0400A20A RID: 41482
		private CielOSPhotosApp _owner;

		// Token: 0x0400A20B RID: 41483
		private CielOSFileSystem.Entry _entry;

		// Token: 0x0400A20C RID: 41484
		private ScrollRect _scroll;

		// Token: 0x0400A20D RID: 41485
		private Canvas _canvas;

		// Token: 0x0400A20E RID: 41486
		private CanvasGroup _sourceGroup;

		// Token: 0x0400A20F RID: 41487
		private RectTransform _ghost;

		// Token: 0x0400A210 RID: 41488
		private float _pressedAt;

		// Token: 0x0400A211 RID: 41489
		private bool _mediaDrag;

		// Token: 0x0400A212 RID: 41490
		private bool _forwardingScroll;

		// Token: 0x0400A213 RID: 41491
		private bool _suppressClick;
	}
}
