using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Features.CielOS.Apps.Story
{
	// Token: 0x02001434 RID: 5172
	[DisallowMultipleComponent]
	public sealed class PersonalDiaryPhotoDrag : MonoBehaviour, IBeginDragHandler, IEventSystemHandler, IDragHandler, IEndDragHandler
	{
		// Token: 0x06009FC2 RID: 40898 RVA: 0x0041EAAC File Offset: 0x0041CCAC
		public static PersonalDiaryPhotoDrag Configure(RectTransform photoRoot, RectTransform document, ScrollRect scroll, Action<Vector2> onMove, Action onBegin = null, Action onEnd = null, float topPadding = 0f, float sidePadding = 0f)
		{
			if (photoRoot == null)
			{
				return null;
			}
			PersonalDiaryPhotoDrag personalDiaryPhotoDrag = photoRoot.GetComponent<PersonalDiaryPhotoDrag>();
			if (personalDiaryPhotoDrag == null)
			{
				personalDiaryPhotoDrag = photoRoot.gameObject.AddComponent<PersonalDiaryPhotoDrag>();
			}
			personalDiaryPhotoDrag.FinishDrag();
			personalDiaryPhotoDrag._photo = photoRoot;
			personalDiaryPhotoDrag._document = document;
			personalDiaryPhotoDrag._scroll = scroll;
			personalDiaryPhotoDrag._onMove = onMove;
			personalDiaryPhotoDrag._onBegin = onBegin;
			personalDiaryPhotoDrag._onEnd = onEnd;
			personalDiaryPhotoDrag._topPadding = topPadding;
			personalDiaryPhotoDrag._sidePadding = sidePadding;
			return personalDiaryPhotoDrag;
		}

		// Token: 0x06009FC3 RID: 40899 RVA: 0x0041EB24 File Offset: 0x0041CD24
		public void OnBeginDrag(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left || this._photo == null || this._document == null || this._onMove == null)
			{
				return;
			}
			this.FinishDrag();
			this._eventCamera = eventData.pressEventCamera;
			this._pointerPosition = eventData.position;
			Vector2 vector;
			if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(this._document, this._pointerPosition, this._eventCamera, out vector))
			{
				return;
			}
			this._pointerId = eventData.pointerId;
			Vector2 vector2 = this._document.InverseTransformPoint(this._photo.position);
			this._grabOffset = vector2 - vector;
			this._dragging = true;
			if (this._scroll != null)
			{
				this._scroll.StopMovement();
			}
			Action onBegin = this._onBegin;
			if (onBegin == null)
			{
				return;
			}
			onBegin();
		}

		// Token: 0x06009FC4 RID: 40900 RVA: 0x0041EBFD File Offset: 0x0041CDFD
		public void OnDrag(PointerEventData eventData)
		{
			if (!this._dragging || eventData.pointerId != this._pointerId)
			{
				return;
			}
			this._pointerPosition = eventData.position;
			this.MovePhotoToPointer();
		}

		// Token: 0x06009FC5 RID: 40901 RVA: 0x0041EC28 File Offset: 0x0041CE28
		public void OnEndDrag(PointerEventData eventData)
		{
			if (!this._dragging || eventData.pointerId != this._pointerId)
			{
				return;
			}
			this._pointerPosition = eventData.position;
			this.MovePhotoToPointer();
			this.FinishDrag();
		}

		// Token: 0x06009FC6 RID: 40902 RVA: 0x0041EC59 File Offset: 0x0041CE59
		private void Update()
		{
			if (!this._dragging || this._photo == null || this._document == null)
			{
				return;
			}
			this.AutoScroll();
			this.MovePhotoToPointer();
		}

		// Token: 0x06009FC7 RID: 40903 RVA: 0x0041EC8C File Offset: 0x0041CE8C
		private void MovePhotoToPointer()
		{
			if (!this._dragging || this._photo == null || this._document == null)
			{
				return;
			}
			Vector2 vector;
			if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(this._document, this._pointerPosition, this._eventCamera, out vector))
			{
				return;
			}
			Vector2 vector2 = vector + this._grabOffset;
			Vector2 vector3 = new Vector2(vector2.x - this._document.rect.xMin, this._document.rect.yMax - vector2.y);
			Vector2 vector4 = new Vector2(this._photo.rect.width * Mathf.Abs(this._photo.localScale.x), this._photo.rect.height * Mathf.Abs(this._photo.localScale.y));
			Vector2 vector5 = PersonalDiaryPhotoLayout.ClampPosition(vector3, vector4, this._photo.localEulerAngles.z, this._document.rect.width, this._topPadding, this._sidePadding, 100000f);
			Vector2 vector6 = new Vector2(vector5.x, -vector5.y);
			if ((this._photo.anchoredPosition - vector6).sqrMagnitude < 0.0001f)
			{
				return;
			}
			this._photo.anchoredPosition = vector6;
			Action<Vector2> onMove = this._onMove;
			if (onMove == null)
			{
				return;
			}
			onMove(vector5);
		}

		// Token: 0x06009FC8 RID: 40904 RVA: 0x0041EE10 File Offset: 0x0041D010
		private void AutoScroll()
		{
			if (this._scroll == null || this._scroll.viewport == null || this._scroll.content == null)
			{
				return;
			}
			Vector2 vector;
			if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(this._scroll.viewport, this._pointerPosition, this._eventCamera, out vector))
			{
				return;
			}
			Rect rect = this._scroll.viewport.rect;
			if (vector.x < rect.xMin || vector.x > rect.xMax)
			{
				return;
			}
			float num = Mathf.Min(72f, rect.height * 0.15f);
			float num2 = ((vector.y > rect.yMax - num) ? Mathf.Clamp01((vector.y - (rect.yMax - num)) / Mathf.Max(1f, num)) : ((vector.y < rect.yMin + num) ? (-Mathf.Clamp01((rect.yMin + num - vector.y) / Mathf.Max(1f, num))) : 0f));
			if (Mathf.Approximately(num2, 0f))
			{
				return;
			}
			this._scroll.content.GetWorldCorners(this._corners);
			float num3 = float.PositiveInfinity;
			float num4 = float.NegativeInfinity;
			foreach (Vector3 vector2 in this._corners)
			{
				float y = this._scroll.viewport.InverseTransformPoint(vector2).y;
				num3 = Mathf.Min(num3, y);
				num4 = Mathf.Max(num4, y);
			}
			float num5 = num4 - num3 - rect.height;
			if (num5 <= 1f)
			{
				return;
			}
			this._scroll.StopMovement();
			float num6 = Mathf.Max(240f, rect.height * 0.85f);
			this._scroll.verticalNormalizedPosition = Mathf.Clamp01(this._scroll.verticalNormalizedPosition + num2 * num6 * Time.unscaledDeltaTime / num5);
		}

		// Token: 0x06009FC9 RID: 40905 RVA: 0x0041F017 File Offset: 0x0041D217
		private void OnDisable()
		{
			this.FinishDrag();
		}

		// Token: 0x06009FCA RID: 40906 RVA: 0x0041F017 File Offset: 0x0041D217
		private void OnDestroy()
		{
			this.FinishDrag();
		}

		// Token: 0x06009FCB RID: 40907 RVA: 0x0041F01F File Offset: 0x0041D21F
		private void OnApplicationFocus(bool focused)
		{
			if (!focused)
			{
				this.FinishDrag();
			}
		}

		// Token: 0x06009FCC RID: 40908 RVA: 0x0041F02A File Offset: 0x0041D22A
		private void FinishDrag()
		{
			if (!this._dragging)
			{
				return;
			}
			this._dragging = false;
			Action onEnd = this._onEnd;
			if (onEnd == null)
			{
				return;
			}
			onEnd();
		}

		// Token: 0x0400A39F RID: 41887
		private RectTransform _photo;

		// Token: 0x0400A3A0 RID: 41888
		private RectTransform _document;

		// Token: 0x0400A3A1 RID: 41889
		private ScrollRect _scroll;

		// Token: 0x0400A3A2 RID: 41890
		private Action<Vector2> _onMove;

		// Token: 0x0400A3A3 RID: 41891
		private Action _onBegin;

		// Token: 0x0400A3A4 RID: 41892
		private Action _onEnd;

		// Token: 0x0400A3A5 RID: 41893
		private Camera _eventCamera;

		// Token: 0x0400A3A6 RID: 41894
		private Vector2 _pointerPosition;

		// Token: 0x0400A3A7 RID: 41895
		private Vector2 _grabOffset;

		// Token: 0x0400A3A8 RID: 41896
		private float _topPadding;

		// Token: 0x0400A3A9 RID: 41897
		private float _sidePadding;

		// Token: 0x0400A3AA RID: 41898
		private int _pointerId;

		// Token: 0x0400A3AB RID: 41899
		private bool _dragging;

		// Token: 0x0400A3AC RID: 41900
		private readonly Vector3[] _corners = new Vector3[4];
	}
}
