using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace Features.MainMenu
{
	// Token: 0x020010B1 RID: 4273
	[DisallowMultipleComponent]
	[RequireComponent(typeof(ScrollRect))]
	public sealed class SmoothCollectionScroll : MonoBehaviour, IScrollHandler, IEventSystemHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler
	{
		// Token: 0x17001491 RID: 5265
		// (get) Token: 0x0600816A RID: 33130 RVA: 0x0034E3C6 File Offset: 0x0034C5C6
		public float PixelsPerStep
		{
			get
			{
				return this.pixelsPerStep;
			}
		}

		// Token: 0x17001492 RID: 5266
		// (get) Token: 0x0600816B RID: 33131 RVA: 0x0034E3CE File Offset: 0x0034C5CE
		public float TargetNormalizedPosition
		{
			get
			{
				return this._targetNormalizedPosition;
			}
		}

		// Token: 0x0600816C RID: 33132 RVA: 0x0034E3D8 File Offset: 0x0034C5D8
		public void Configure(float stepPixels, float easingTime)
		{
			this.pixelsPerStep = Mathf.Max(8f, stepPixels);
			this.smoothTime = Mathf.Clamp(easingTime, 0.05f, 0.4f);
			if (this._scrollRect == null)
			{
				this._scrollRect = base.GetComponent<ScrollRect>();
			}
			if (this._scrollRect != null)
			{
				this._scrollRect.scrollSensitivity = 0f;
			}
			this.SyncTargetToCurrentPosition();
		}

		// Token: 0x0600816D RID: 33133 RVA: 0x0034E44A File Offset: 0x0034C64A
		private void Awake()
		{
			this._scrollRect = base.GetComponent<ScrollRect>();
			this._scrollRect.scrollSensitivity = 0f;
		}

		// Token: 0x0600816E RID: 33134 RVA: 0x0034E468 File Offset: 0x0034C668
		private void OnEnable()
		{
			this.SyncTargetToCurrentPosition();
		}

		// Token: 0x0600816F RID: 33135 RVA: 0x0034E470 File Offset: 0x0034C670
		private void OnDisable()
		{
			this._smoothing = false;
			this._draggingContent = false;
			this._smoothVelocity = 0f;
		}

		// Token: 0x06008170 RID: 33136 RVA: 0x0034E48C File Offset: 0x0034C68C
		public void OnScroll(PointerEventData eventData)
		{
			if (this._scrollRect == null || !this._scrollRect.IsActive() || this._scrollRect.content == null || this._scrollRect.viewport == null)
			{
				return;
			}
			float num = Mathf.Max(0f, this._scrollRect.content.rect.height - this._scrollRect.viewport.rect.height);
			if (num <= 0.01f)
			{
				return;
			}
			float num2 = eventData.scrollDelta.y;
			if (EventSystem.current != null)
			{
				InputSystemUIInputModule inputSystemUIInputModule = EventSystem.current.currentInputModule as InputSystemUIInputModule;
				if (inputSystemUIInputModule != null)
				{
					float num3 = Mathf.Abs(inputSystemUIInputModule.scrollDeltaPerTick);
					if (num3 <= Mathf.Epsilon)
					{
						return;
					}
					num2 /= num3;
				}
			}
			if (Mathf.Abs(num2) < 0.001f)
			{
				return;
			}
			if (this.IsScrollbarSelected())
			{
				EventSystem.current.SetSelectedGameObject(null);
			}
			float num4 = ((Mathf.Abs(num2) >= 1f) ? Mathf.Sign(num2) : num2);
			if (!this._smoothing)
			{
				this._targetNormalizedPosition = this._scrollRect.verticalNormalizedPosition;
				this._smoothVelocity = 0f;
			}
			this._targetNormalizedPosition = Mathf.Clamp01(this._targetNormalizedPosition + num4 * this.pixelsPerStep / num);
			this._smoothing = true;
			this._scrollRect.StopMovement();
			eventData.Use();
		}

		// Token: 0x06008171 RID: 33137 RVA: 0x0034E5FA File Offset: 0x0034C7FA
		public void OnPointerDown(PointerEventData eventData)
		{
			this.CancelSmoothing();
		}

		// Token: 0x06008172 RID: 33138 RVA: 0x0034E602 File Offset: 0x0034C802
		public void OnBeginDrag(PointerEventData eventData)
		{
			this._draggingContent = true;
			this.CancelSmoothing();
		}

		// Token: 0x06008173 RID: 33139 RVA: 0x0034E611 File Offset: 0x0034C811
		public void OnEndDrag(PointerEventData eventData)
		{
			this._draggingContent = false;
			this.SyncTargetToCurrentPosition();
		}

		// Token: 0x06008174 RID: 33140 RVA: 0x0034E620 File Offset: 0x0034C820
		private void Update()
		{
			if (this._scrollRect == null || this._scrollRect.content == null)
			{
				return;
			}
			if (this._draggingContent || this.IsScrollbarSelected())
			{
				this.CancelSmoothing();
				return;
			}
			if (!this._smoothing)
			{
				this._targetNormalizedPosition = this._scrollRect.verticalNormalizedPosition;
				return;
			}
			float num = Mathf.SmoothDamp(this._scrollRect.verticalNormalizedPosition, this._targetNormalizedPosition, ref this._smoothVelocity, this.smoothTime, float.PositiveInfinity, Time.unscaledDeltaTime);
			this._scrollRect.verticalNormalizedPosition = num;
			if (Mathf.Abs(num - this._targetNormalizedPosition) <= 0.0001f && Mathf.Abs(this._smoothVelocity) <= 0.001f)
			{
				this._scrollRect.verticalNormalizedPosition = this._targetNormalizedPosition;
				this._smoothing = false;
				this._smoothVelocity = 0f;
			}
		}

		// Token: 0x06008175 RID: 33141 RVA: 0x0034E704 File Offset: 0x0034C904
		private bool IsScrollbarSelected()
		{
			if (EventSystem.current == null || EventSystem.current.currentSelectedGameObject == null || this._scrollRect.verticalScrollbar == null)
			{
				return false;
			}
			Transform transform = EventSystem.current.currentSelectedGameObject.transform;
			Transform transform2 = this._scrollRect.verticalScrollbar.transform;
			return transform == transform2 || transform.IsChildOf(transform2);
		}

		// Token: 0x06008176 RID: 33142 RVA: 0x0034E778 File Offset: 0x0034C978
		private void CancelSmoothing()
		{
			this._smoothing = false;
			this._smoothVelocity = 0f;
			this.SyncTargetToCurrentPosition();
		}

		// Token: 0x06008177 RID: 33143 RVA: 0x0034E792 File Offset: 0x0034C992
		private void SyncTargetToCurrentPosition()
		{
			if (this._scrollRect != null)
			{
				this._targetNormalizedPosition = this._scrollRect.verticalNormalizedPosition;
			}
		}

		// Token: 0x040084AE RID: 33966
		[SerializeField]
		[Min(8f)]
		private float pixelsPerStep = 72f;

		// Token: 0x040084AF RID: 33967
		[SerializeField]
		[Range(0.05f, 0.4f)]
		private float smoothTime = 0.16f;

		// Token: 0x040084B0 RID: 33968
		private ScrollRect _scrollRect;

		// Token: 0x040084B1 RID: 33969
		private float _targetNormalizedPosition = 1f;

		// Token: 0x040084B2 RID: 33970
		private float _smoothVelocity;

		// Token: 0x040084B3 RID: 33971
		private bool _smoothing;

		// Token: 0x040084B4 RID: 33972
		private bool _draggingContent;
	}
}
