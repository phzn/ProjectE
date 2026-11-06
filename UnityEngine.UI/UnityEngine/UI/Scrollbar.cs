using System;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
	// Token: 0x02000033 RID: 51
	[AddComponentMenu("UI/Scrollbar", 36)]
	[ExecuteAlways]
	[RequireComponent(typeof(RectTransform))]
	public class Scrollbar : Selectable, IBeginDragHandler, IEventSystemHandler, IDragHandler, IInitializePotentialDragHandler, ICanvasElement
	{
		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000336 RID: 822 RVA: 0x00010E2F File Offset: 0x0000F02F
		// (set) Token: 0x06000337 RID: 823 RVA: 0x00010E37 File Offset: 0x0000F037
		public RectTransform handleRect
		{
			get
			{
				return this.m_HandleRect;
			}
			set
			{
				if (SetPropertyUtility.SetClass<RectTransform>(ref this.m_HandleRect, value))
				{
					this.UpdateCachedReferences();
					this.UpdateVisuals();
				}
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000338 RID: 824 RVA: 0x00010E53 File Offset: 0x0000F053
		// (set) Token: 0x06000339 RID: 825 RVA: 0x00010E5B File Offset: 0x0000F05B
		public Scrollbar.Direction direction
		{
			get
			{
				return this.m_Direction;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<Scrollbar.Direction>(ref this.m_Direction, value))
				{
					this.UpdateVisuals();
				}
			}
		}

		// Token: 0x0600033A RID: 826 RVA: 0x00010E71 File Offset: 0x0000F071
		protected Scrollbar()
		{
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x0600033B RID: 827 RVA: 0x00010E9C File Offset: 0x0000F09C
		// (set) Token: 0x0600033C RID: 828 RVA: 0x00010ED5 File Offset: 0x0000F0D5
		public float value
		{
			get
			{
				float num = this.m_Value;
				if (this.m_NumberOfSteps > 1)
				{
					num = Mathf.Round(num * (float)(this.m_NumberOfSteps - 1)) / (float)(this.m_NumberOfSteps - 1);
				}
				return num;
			}
			set
			{
				this.Set(value, true);
			}
		}

		// Token: 0x0600033D RID: 829 RVA: 0x00010EDF File Offset: 0x0000F0DF
		public virtual void SetValueWithoutNotify(float input)
		{
			this.Set(input, false);
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x0600033E RID: 830 RVA: 0x00010EE9 File Offset: 0x0000F0E9
		// (set) Token: 0x0600033F RID: 831 RVA: 0x00010EF1 File Offset: 0x0000F0F1
		public float size
		{
			get
			{
				return this.m_Size;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<float>(ref this.m_Size, Mathf.Clamp01(value)))
				{
					this.UpdateVisuals();
				}
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000340 RID: 832 RVA: 0x00010F0C File Offset: 0x0000F10C
		// (set) Token: 0x06000341 RID: 833 RVA: 0x00010F14 File Offset: 0x0000F114
		public int numberOfSteps
		{
			get
			{
				return this.m_NumberOfSteps;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<int>(ref this.m_NumberOfSteps, value))
				{
					this.Set(this.m_Value, true);
					this.UpdateVisuals();
				}
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000342 RID: 834 RVA: 0x00010F37 File Offset: 0x0000F137
		// (set) Token: 0x06000343 RID: 835 RVA: 0x00010F3F File Offset: 0x0000F13F
		public Scrollbar.ScrollEvent onValueChanged
		{
			get
			{
				return this.m_OnValueChanged;
			}
			set
			{
				this.m_OnValueChanged = value;
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000344 RID: 836 RVA: 0x00010F48 File Offset: 0x0000F148
		private float stepSize
		{
			get
			{
				if (this.m_NumberOfSteps <= 1)
				{
					return 0.1f;
				}
				return 1f / (float)(this.m_NumberOfSteps - 1);
			}
		}

		// Token: 0x06000345 RID: 837 RVA: 0x00010F68 File Offset: 0x0000F168
		public virtual void Rebuild(CanvasUpdate executing)
		{
		}

		// Token: 0x06000346 RID: 838 RVA: 0x00010F6A File Offset: 0x0000F16A
		public virtual void LayoutComplete()
		{
		}

		// Token: 0x06000347 RID: 839 RVA: 0x00010F6C File Offset: 0x0000F16C
		public virtual void GraphicUpdateComplete()
		{
		}

		// Token: 0x06000348 RID: 840 RVA: 0x00010F6E File Offset: 0x0000F16E
		protected override void OnEnable()
		{
			base.OnEnable();
			this.UpdateCachedReferences();
			this.Set(this.m_Value, false);
			this.UpdateVisuals();
		}

		// Token: 0x06000349 RID: 841 RVA: 0x00010F8F File Offset: 0x0000F18F
		protected override void OnDisable()
		{
			this.m_Tracker.Clear();
			base.OnDisable();
		}

		// Token: 0x0600034A RID: 842 RVA: 0x00010FA2 File Offset: 0x0000F1A2
		protected virtual void Update()
		{
			if (this.m_DelayedUpdateVisuals)
			{
				this.m_DelayedUpdateVisuals = false;
				this.UpdateVisuals();
			}
		}

		// Token: 0x0600034B RID: 843 RVA: 0x00010FB9 File Offset: 0x0000F1B9
		private void UpdateCachedReferences()
		{
			if (this.m_HandleRect && this.m_HandleRect.parent != null)
			{
				this.m_ContainerRect = this.m_HandleRect.parent.GetComponent<RectTransform>();
				return;
			}
			this.m_ContainerRect = null;
		}

		// Token: 0x0600034C RID: 844 RVA: 0x00010FF9 File Offset: 0x0000F1F9
		private void Set(float input, bool sendCallback = true)
		{
			float value = this.m_Value;
			this.m_Value = input;
			if (value == this.value)
			{
				return;
			}
			this.UpdateVisuals();
			if (sendCallback)
			{
				UISystemProfilerApi.AddMarker("Scrollbar.value", this);
				this.m_OnValueChanged.Invoke(this.value);
			}
		}

		// Token: 0x0600034D RID: 845 RVA: 0x00011036 File Offset: 0x0000F236
		protected override void OnRectTransformDimensionsChange()
		{
			base.OnRectTransformDimensionsChange();
			if (!this.IsActive())
			{
				return;
			}
			this.UpdateVisuals();
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x0600034E RID: 846 RVA: 0x0001104D File Offset: 0x0000F24D
		private Scrollbar.Axis axis
		{
			get
			{
				if (this.m_Direction != Scrollbar.Direction.LeftToRight && this.m_Direction != Scrollbar.Direction.RightToLeft)
				{
					return Scrollbar.Axis.Vertical;
				}
				return Scrollbar.Axis.Horizontal;
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x0600034F RID: 847 RVA: 0x00011063 File Offset: 0x0000F263
		private bool reverseValue
		{
			get
			{
				return this.m_Direction == Scrollbar.Direction.RightToLeft || this.m_Direction == Scrollbar.Direction.TopToBottom;
			}
		}

		// Token: 0x06000350 RID: 848 RVA: 0x0001107C File Offset: 0x0000F27C
		private void UpdateVisuals()
		{
			this.m_Tracker.Clear();
			if (this.m_ContainerRect != null)
			{
				this.m_Tracker.Add(this, this.m_HandleRect, DrivenTransformProperties.Anchors);
				Vector2 zero = Vector2.zero;
				Vector2 one = Vector2.one;
				float num = Mathf.Clamp01(this.value) * (1f - this.size);
				if (this.reverseValue)
				{
					zero[(int)this.axis] = 1f - num - this.size;
					one[(int)this.axis] = 1f - num;
				}
				else
				{
					zero[(int)this.axis] = num;
					one[(int)this.axis] = num + this.size;
				}
				this.m_HandleRect.anchorMin = zero;
				this.m_HandleRect.anchorMax = one;
			}
		}

		// Token: 0x06000351 RID: 849 RVA: 0x00011158 File Offset: 0x0000F358
		private void UpdateDrag(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			if (this.m_ContainerRect == null)
			{
				return;
			}
			Vector2 zero = Vector2.zero;
			if (!MultipleDisplayUtilities.GetRelativeMousePositionForDrag(eventData, ref zero))
			{
				return;
			}
			Vector2 vector;
			if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(this.m_ContainerRect, zero, eventData.pressEventCamera, out vector))
			{
				return;
			}
			Vector2 vector2 = vector - this.m_Offset - this.m_ContainerRect.rect.position - (this.m_HandleRect.rect.size - this.m_HandleRect.sizeDelta) * 0.5f;
			float num = ((this.axis == Scrollbar.Axis.Horizontal) ? this.m_ContainerRect.rect.width : this.m_ContainerRect.rect.height) * (1f - this.size);
			if (num <= 0f)
			{
				return;
			}
			this.DoUpdateDrag(vector2, num);
		}

		// Token: 0x06000352 RID: 850 RVA: 0x00011250 File Offset: 0x0000F450
		private void DoUpdateDrag(Vector2 handleCorner, float remainingSize)
		{
			switch (this.m_Direction)
			{
			case Scrollbar.Direction.LeftToRight:
				this.Set(Mathf.Clamp01(handleCorner.x / remainingSize), true);
				return;
			case Scrollbar.Direction.RightToLeft:
				this.Set(Mathf.Clamp01(1f - handleCorner.x / remainingSize), true);
				return;
			case Scrollbar.Direction.BottomToTop:
				this.Set(Mathf.Clamp01(handleCorner.y / remainingSize), true);
				return;
			case Scrollbar.Direction.TopToBottom:
				this.Set(Mathf.Clamp01(1f - handleCorner.y / remainingSize), true);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000353 RID: 851 RVA: 0x000112DA File Offset: 0x0000F4DA
		private bool MayDrag(PointerEventData eventData)
		{
			return this.IsActive() && this.IsInteractable() && eventData.button == PointerEventData.InputButton.Left;
		}

		// Token: 0x06000354 RID: 852 RVA: 0x000112F8 File Offset: 0x0000F4F8
		public virtual void OnBeginDrag(PointerEventData eventData)
		{
			this.isPointerDownAndNotDragging = false;
			if (!this.MayDrag(eventData))
			{
				return;
			}
			if (this.m_ContainerRect == null)
			{
				return;
			}
			this.m_Offset = Vector2.zero;
			Vector2 vector;
			if (RectTransformUtility.RectangleContainsScreenPoint(this.m_HandleRect, eventData.pointerPressRaycast.screenPosition, eventData.enterEventCamera) && RectTransformUtility.ScreenPointToLocalPointInRectangle(this.m_HandleRect, eventData.pointerPressRaycast.screenPosition, eventData.pressEventCamera, out vector))
			{
				this.m_Offset = vector - this.m_HandleRect.rect.center;
			}
		}

		// Token: 0x06000355 RID: 853 RVA: 0x0001138D File Offset: 0x0000F58D
		public virtual void OnDrag(PointerEventData eventData)
		{
			if (!this.MayDrag(eventData))
			{
				return;
			}
			if (this.m_ContainerRect != null)
			{
				this.UpdateDrag(eventData);
			}
		}

		// Token: 0x06000356 RID: 854 RVA: 0x000113AE File Offset: 0x0000F5AE
		public override void OnPointerDown(PointerEventData eventData)
		{
			if (!this.MayDrag(eventData))
			{
				return;
			}
			base.OnPointerDown(eventData);
			this.isPointerDownAndNotDragging = true;
			this.m_PointerDownRepeat = base.StartCoroutine(this.ClickRepeat(eventData.pointerPressRaycast.screenPosition, eventData.enterEventCamera));
		}

		// Token: 0x06000357 RID: 855 RVA: 0x000113EB File Offset: 0x0000F5EB
		protected IEnumerator ClickRepeat(PointerEventData eventData)
		{
			return this.ClickRepeat(eventData.pointerPressRaycast.screenPosition, eventData.enterEventCamera);
		}

		// Token: 0x06000358 RID: 856 RVA: 0x00011404 File Offset: 0x0000F604
		protected IEnumerator ClickRepeat(Vector2 screenPosition, Camera camera)
		{
			while (this.isPointerDownAndNotDragging)
			{
				Vector2 vector;
				if (!RectTransformUtility.RectangleContainsScreenPoint(this.m_HandleRect, screenPosition, camera) && RectTransformUtility.ScreenPointToLocalPointInRectangle(this.m_HandleRect, screenPosition, camera, out vector))
				{
					float num = ((((this.axis == Scrollbar.Axis.Horizontal) ? vector.x : vector.y) < 0f) ? this.size : (-this.size));
					this.value += (this.reverseValue ? num : (-num));
					this.value = Mathf.Clamp01(this.value);
					this.value = Mathf.Round(this.value * 10000f) / 10000f;
				}
				yield return new WaitForEndOfFrame();
			}
			base.StopCoroutine(this.m_PointerDownRepeat);
			yield break;
		}

		// Token: 0x06000359 RID: 857 RVA: 0x00011421 File Offset: 0x0000F621
		public override void OnPointerUp(PointerEventData eventData)
		{
			base.OnPointerUp(eventData);
			this.isPointerDownAndNotDragging = false;
		}

		// Token: 0x0600035A RID: 858 RVA: 0x00011434 File Offset: 0x0000F634
		public override void OnMove(AxisEventData eventData)
		{
			if (!this.IsActive() || !this.IsInteractable())
			{
				base.OnMove(eventData);
				return;
			}
			switch (eventData.moveDir)
			{
			case MoveDirection.Left:
				if (this.axis == Scrollbar.Axis.Horizontal && this.FindSelectableOnLeft() == null)
				{
					this.Set(Mathf.Clamp01(this.reverseValue ? (this.value + this.stepSize) : (this.value - this.stepSize)), true);
					return;
				}
				base.OnMove(eventData);
				return;
			case MoveDirection.Up:
				if (this.axis == Scrollbar.Axis.Vertical && this.FindSelectableOnUp() == null)
				{
					this.Set(Mathf.Clamp01(this.reverseValue ? (this.value - this.stepSize) : (this.value + this.stepSize)), true);
					return;
				}
				base.OnMove(eventData);
				return;
			case MoveDirection.Right:
				if (this.axis == Scrollbar.Axis.Horizontal && this.FindSelectableOnRight() == null)
				{
					this.Set(Mathf.Clamp01(this.reverseValue ? (this.value - this.stepSize) : (this.value + this.stepSize)), true);
					return;
				}
				base.OnMove(eventData);
				return;
			case MoveDirection.Down:
				if (this.axis == Scrollbar.Axis.Vertical && this.FindSelectableOnDown() == null)
				{
					this.Set(Mathf.Clamp01(this.reverseValue ? (this.value + this.stepSize) : (this.value - this.stepSize)), true);
					return;
				}
				base.OnMove(eventData);
				return;
			default:
				return;
			}
		}

		// Token: 0x0600035B RID: 859 RVA: 0x000115B4 File Offset: 0x0000F7B4
		public override Selectable FindSelectableOnLeft()
		{
			if (base.navigation.mode == Navigation.Mode.Automatic && this.axis == Scrollbar.Axis.Horizontal)
			{
				return null;
			}
			return base.FindSelectableOnLeft();
		}

		// Token: 0x0600035C RID: 860 RVA: 0x000115E4 File Offset: 0x0000F7E4
		public override Selectable FindSelectableOnRight()
		{
			if (base.navigation.mode == Navigation.Mode.Automatic && this.axis == Scrollbar.Axis.Horizontal)
			{
				return null;
			}
			return base.FindSelectableOnRight();
		}

		// Token: 0x0600035D RID: 861 RVA: 0x00011614 File Offset: 0x0000F814
		public override Selectable FindSelectableOnUp()
		{
			if (base.navigation.mode == Navigation.Mode.Automatic && this.axis == Scrollbar.Axis.Vertical)
			{
				return null;
			}
			return base.FindSelectableOnUp();
		}

		// Token: 0x0600035E RID: 862 RVA: 0x00011644 File Offset: 0x0000F844
		public override Selectable FindSelectableOnDown()
		{
			if (base.navigation.mode == Navigation.Mode.Automatic && this.axis == Scrollbar.Axis.Vertical)
			{
				return null;
			}
			return base.FindSelectableOnDown();
		}

		// Token: 0x0600035F RID: 863 RVA: 0x00011673 File Offset: 0x0000F873
		public virtual void OnInitializePotentialDrag(PointerEventData eventData)
		{
			eventData.useDragThreshold = false;
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0001167C File Offset: 0x0000F87C
		public void SetDirection(Scrollbar.Direction direction, bool includeRectLayouts)
		{
			Scrollbar.Axis axis = this.axis;
			bool reverseValue = this.reverseValue;
			this.direction = direction;
			if (!includeRectLayouts)
			{
				return;
			}
			if (this.axis != axis)
			{
				RectTransformUtility.FlipLayoutAxes(base.transform as RectTransform, true, true);
			}
			if (this.reverseValue != reverseValue)
			{
				RectTransformUtility.FlipLayoutOnAxis(base.transform as RectTransform, (int)this.axis, true, true);
			}
		}

		// Token: 0x06000361 RID: 865 RVA: 0x000116DE File Offset: 0x0000F8DE
		Transform ICanvasElement.get_transform()
		{
			return base.transform;
		}

		// Token: 0x04000119 RID: 281
		[SerializeField]
		private RectTransform m_HandleRect;

		// Token: 0x0400011A RID: 282
		[SerializeField]
		private Scrollbar.Direction m_Direction;

		// Token: 0x0400011B RID: 283
		[Range(0f, 1f)]
		[SerializeField]
		private float m_Value;

		// Token: 0x0400011C RID: 284
		[Range(0f, 1f)]
		[SerializeField]
		private float m_Size = 0.2f;

		// Token: 0x0400011D RID: 285
		[Range(0f, 11f)]
		[SerializeField]
		private int m_NumberOfSteps;

		// Token: 0x0400011E RID: 286
		[Space(6f)]
		[SerializeField]
		private Scrollbar.ScrollEvent m_OnValueChanged = new Scrollbar.ScrollEvent();

		// Token: 0x0400011F RID: 287
		private RectTransform m_ContainerRect;

		// Token: 0x04000120 RID: 288
		private Vector2 m_Offset = Vector2.zero;

		// Token: 0x04000121 RID: 289
		private DrivenRectTransformTracker m_Tracker;

		// Token: 0x04000122 RID: 290
		private Coroutine m_PointerDownRepeat;

		// Token: 0x04000123 RID: 291
		private bool isPointerDownAndNotDragging;

		// Token: 0x04000124 RID: 292
		private bool m_DelayedUpdateVisuals;

		// Token: 0x020000A5 RID: 165
		public enum Direction
		{
			// Token: 0x040002EF RID: 751
			LeftToRight,
			// Token: 0x040002F0 RID: 752
			RightToLeft,
			// Token: 0x040002F1 RID: 753
			BottomToTop,
			// Token: 0x040002F2 RID: 754
			TopToBottom
		}

		// Token: 0x020000A6 RID: 166
		[Serializable]
		public class ScrollEvent : UnityEvent<float>
		{
		}

		// Token: 0x020000A7 RID: 167
		private enum Axis
		{
			// Token: 0x040002F4 RID: 756
			Horizontal,
			// Token: 0x040002F5 RID: 757
			Vertical
		}
	}
}
