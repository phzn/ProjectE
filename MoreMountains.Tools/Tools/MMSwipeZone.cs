using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace MoreMountains.Tools
{
	// Token: 0x0200018A RID: 394
	[RequireComponent(typeof(RectTransform))]
	[AddComponentMenu("More Mountains/Tools/Controls/MMSwipeZone")]
	public class MMSwipeZone : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler
	{
		// Token: 0x0600093D RID: 2365 RVA: 0x0002AB0C File Offset: 0x00028D0C
		protected virtual void Swipe()
		{
			float num = this._swipeEndedAt - this._swipeStartedAt;
			MMSwipeEvent mmswipeEvent = new MMSwipeEvent(this._swipeDirection, this._angle, this._length, this._firstTouchPosition, this._destination, num);
			MMEventManager.TriggerEvent<MMSwipeEvent>(mmswipeEvent);
			if (this.ZoneSwiped != null)
			{
				this.ZoneSwiped.Invoke(mmswipeEvent);
			}
		}

		// Token: 0x0600093E RID: 2366 RVA: 0x0002AB67 File Offset: 0x00028D67
		protected virtual void Press()
		{
			if (this.ZonePressed != null)
			{
				this.ZonePressed.Invoke();
			}
		}

		// Token: 0x0600093F RID: 2367 RVA: 0x0002AB7C File Offset: 0x00028D7C
		public virtual void OnPointerDown(PointerEventData data)
		{
			this._firstTouchPosition = Input.mousePosition;
			this._swipeStartedAt = Time.unscaledTime;
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x0002AB9C File Offset: 0x00028D9C
		public virtual void OnPointerUp(PointerEventData data)
		{
			if ((float)Time.frameCount == this._lastPointerUpAt)
			{
				return;
			}
			this._destination = Input.mousePosition;
			this._deltaSwipe = this._destination - this._firstTouchPosition;
			this._length = this._deltaSwipe.magnitude;
			if (this._length > this.MinimalSwipeLength)
			{
				this._angle = MMMaths.AngleBetween(this._deltaSwipe, Vector2.right);
				this._swipeDirection = this.AngleToSwipeDirection(this._angle);
				this._swipeEndedAt = Time.unscaledTime;
				this.Swipe();
			}
			if (this._deltaSwipe.magnitude < this.MaximumPressLength)
			{
				this.Press();
			}
			this._lastPointerUpAt = (float)Time.frameCount;
		}

		// Token: 0x06000941 RID: 2369 RVA: 0x0002AC5C File Offset: 0x00028E5C
		public virtual void OnPointerEnter(PointerEventData data)
		{
			if (!this.MouseMode)
			{
				this.OnPointerDown(data);
			}
		}

		// Token: 0x06000942 RID: 2370 RVA: 0x0002AC6D File Offset: 0x00028E6D
		public virtual void OnPointerExit(PointerEventData data)
		{
			if (!this.MouseMode)
			{
				this.OnPointerUp(data);
			}
		}

		// Token: 0x06000943 RID: 2371 RVA: 0x0002AC80 File Offset: 0x00028E80
		protected virtual MMPossibleSwipeDirections AngleToSwipeDirection(float angle)
		{
			if (angle < 45f || angle >= 315f)
			{
				return MMPossibleSwipeDirections.Right;
			}
			if (angle >= 45f && angle < 135f)
			{
				return MMPossibleSwipeDirections.Up;
			}
			if (angle >= 135f && angle < 225f)
			{
				return MMPossibleSwipeDirections.Left;
			}
			if (angle >= 225f && angle < 315f)
			{
				return MMPossibleSwipeDirections.Down;
			}
			return MMPossibleSwipeDirections.Right;
		}

		// Token: 0x04000ACE RID: 2766
		[Tooltip("the minimal length of a swipe")]
		public float MinimalSwipeLength = 50f;

		// Token: 0x04000ACF RID: 2767
		[Tooltip("the maximum press length of a swipe")]
		public float MaximumPressLength = 10f;

		// Token: 0x04000AD0 RID: 2768
		[Tooltip("The method(s) to call when the zone is swiped")]
		public SwipeEvent ZoneSwiped;

		// Token: 0x04000AD1 RID: 2769
		[Tooltip("The method(s) to call while the zone is being pressed")]
		public UnityEvent ZonePressed;

		// Token: 0x04000AD2 RID: 2770
		[Header("Mouse Mode")]
		[MMInformation("If you set this to true, you'll need to actually press the button for it to be triggered, otherwise a simple hover will trigger it (better for touch input).", MMInformationAttribute.InformationType.Info, false)]
		[Tooltip("If you set this to true, you'll need to actually press the button for it to be triggered, otherwise a simple hover will trigger it (better for touch input).")]
		public bool MouseMode;

		// Token: 0x04000AD3 RID: 2771
		protected Vector2 _firstTouchPosition;

		// Token: 0x04000AD4 RID: 2772
		protected float _angle;

		// Token: 0x04000AD5 RID: 2773
		protected float _length;

		// Token: 0x04000AD6 RID: 2774
		protected Vector2 _destination;

		// Token: 0x04000AD7 RID: 2775
		protected Vector2 _deltaSwipe;

		// Token: 0x04000AD8 RID: 2776
		protected MMPossibleSwipeDirections _swipeDirection;

		// Token: 0x04000AD9 RID: 2777
		protected float _lastPointerUpAt;

		// Token: 0x04000ADA RID: 2778
		protected float _swipeStartedAt;

		// Token: 0x04000ADB RID: 2779
		protected float _swipeEndedAt;
	}
}
