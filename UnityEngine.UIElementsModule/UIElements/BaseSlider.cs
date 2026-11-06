using System;
using System.Collections.Generic;
using System.Globalization;

namespace UnityEngine.UIElements
{
	// Token: 0x02000062 RID: 98
	public abstract class BaseSlider<TValueType> : BaseField<TValueType>, IValueField<TValueType> where TValueType : IComparable<TValueType>
	{
		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000404 RID: 1028 RVA: 0x0000F992 File Offset: 0x0000DB92
		// (set) Token: 0x06000405 RID: 1029 RVA: 0x0000F99A File Offset: 0x0000DB9A
		internal VisualElement dragContainer { get; private set; }

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000406 RID: 1030 RVA: 0x0000F9A3 File Offset: 0x0000DBA3
		// (set) Token: 0x06000407 RID: 1031 RVA: 0x0000F9AB File Offset: 0x0000DBAB
		internal VisualElement dragElement { get; private set; }

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000408 RID: 1032 RVA: 0x0000F9B4 File Offset: 0x0000DBB4
		// (set) Token: 0x06000409 RID: 1033 RVA: 0x0000F9BC File Offset: 0x0000DBBC
		internal VisualElement trackElement { get; private set; }

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x0600040A RID: 1034 RVA: 0x0000F9C5 File Offset: 0x0000DBC5
		// (set) Token: 0x0600040B RID: 1035 RVA: 0x0000F9CD File Offset: 0x0000DBCD
		internal VisualElement dragBorderElement { get; private set; }

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x0600040C RID: 1036 RVA: 0x0000F9D6 File Offset: 0x0000DBD6
		// (set) Token: 0x0600040D RID: 1037 RVA: 0x0000F9DE File Offset: 0x0000DBDE
		internal TextField inputTextField { get; private set; }

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x0600040E RID: 1038 RVA: 0x0000F9E8 File Offset: 0x0000DBE8
		private protected override bool canSwitchToMixedValue
		{
			get
			{
				bool flag = this.inputTextField == null;
				return flag || !this.inputTextField.textInputBase.textElement.hasFocus || (this.inputTextField.textInputBase.textElement.hasFocus && this.focusController != null && this.focusController.IsPendingFocus(this));
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x0600040F RID: 1039 RVA: 0x0000FA54 File Offset: 0x0000DC54
		// (set) Token: 0x06000410 RID: 1040 RVA: 0x0000FA6C File Offset: 0x0000DC6C
		public TValueType lowValue
		{
			get
			{
				return this.m_LowValue;
			}
			set
			{
				bool flag = !EqualityComparer<TValueType>.Default.Equals(this.m_LowValue, value);
				if (flag)
				{
					this.m_LowValue = value;
					this.ClampValue();
					this.UpdateDragElementPosition();
					base.SaveViewData();
				}
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000411 RID: 1041 RVA: 0x0000FAB0 File Offset: 0x0000DCB0
		// (set) Token: 0x06000412 RID: 1042 RVA: 0x0000FAC8 File Offset: 0x0000DCC8
		public TValueType highValue
		{
			get
			{
				return this.m_HighValue;
			}
			set
			{
				bool flag = !EqualityComparer<TValueType>.Default.Equals(this.m_HighValue, value);
				if (flag)
				{
					this.m_HighValue = value;
					this.ClampValue();
					this.UpdateDragElementPosition();
					base.SaveViewData();
				}
			}
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x0000FB0C File Offset: 0x0000DD0C
		internal void SetHighValueWithoutNotify(TValueType newHighValue)
		{
			this.m_HighValue = newHighValue;
			TValueType tvalueType = (this.clamped ? this.GetClampedValue(this.value) : this.value);
			this.SetValueWithoutNotify(tvalueType);
			this.UpdateDragElementPosition();
			base.SaveViewData();
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000414 RID: 1044 RVA: 0x0000FB54 File Offset: 0x0000DD54
		public TValueType range
		{
			get
			{
				return this.SliderRange();
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000415 RID: 1045 RVA: 0x0000FB6C File Offset: 0x0000DD6C
		// (set) Token: 0x06000416 RID: 1046 RVA: 0x0000FB84 File Offset: 0x0000DD84
		public virtual float pageSize
		{
			get
			{
				return this.m_PageSize;
			}
			set
			{
				this.m_PageSize = value;
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000417 RID: 1047 RVA: 0x0000FB90 File Offset: 0x0000DD90
		// (set) Token: 0x06000418 RID: 1048 RVA: 0x0000FBA8 File Offset: 0x0000DDA8
		public virtual bool showInputField
		{
			get
			{
				return this.m_ShowInputField;
			}
			set
			{
				bool flag = this.m_ShowInputField != value;
				if (flag)
				{
					this.m_ShowInputField = value;
					this.UpdateTextFieldVisibility();
				}
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000419 RID: 1049 RVA: 0x0000FBD6 File Offset: 0x0000DDD6
		// (set) Token: 0x0600041A RID: 1050 RVA: 0x0000FBDE File Offset: 0x0000DDDE
		internal bool clamped { get; set; } = true;

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x0600041B RID: 1051 RVA: 0x0000FBE7 File Offset: 0x0000DDE7
		// (set) Token: 0x0600041C RID: 1052 RVA: 0x0000FBEF File Offset: 0x0000DDEF
		internal ClampedDragger<TValueType> clampedDragger { get; private set; }

		// Token: 0x0600041D RID: 1053 RVA: 0x0000FBF8 File Offset: 0x0000DDF8
		private TValueType Clamp(TValueType value, TValueType lowBound, TValueType highBound)
		{
			TValueType tvalueType = value;
			bool flag = lowBound.CompareTo(value) > 0;
			if (flag)
			{
				tvalueType = lowBound;
			}
			else
			{
				bool flag2 = highBound.CompareTo(value) < 0;
				if (flag2)
				{
					tvalueType = highBound;
				}
			}
			return tvalueType;
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x0000FC44 File Offset: 0x0000DE44
		private TValueType GetClampedValue(TValueType newValue)
		{
			TValueType tvalueType = this.lowValue;
			TValueType tvalueType2 = this.highValue;
			bool flag = tvalueType.CompareTo(tvalueType2) > 0;
			if (flag)
			{
				TValueType tvalueType3 = tvalueType;
				tvalueType = tvalueType2;
				tvalueType2 = tvalueType3;
			}
			return this.Clamp(newValue, tvalueType, tvalueType2);
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x0600041F RID: 1055 RVA: 0x0000FC8C File Offset: 0x0000DE8C
		// (set) Token: 0x06000420 RID: 1056 RVA: 0x0000FCA4 File Offset: 0x0000DEA4
		public override TValueType value
		{
			get
			{
				return base.value;
			}
			set
			{
				TValueType tvalueType = (this.clamped ? this.GetClampedValue(value) : value);
				base.value = tvalueType;
			}
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x00003CD2 File Offset: 0x00001ED2
		public virtual void ApplyInputDeviceDelta(Vector3 delta, DeltaSpeed speed, TValueType startValue)
		{
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x00003CD2 File Offset: 0x00001ED2
		void IValueField<TValueType>.StartDragging()
		{
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x00003CD2 File Offset: 0x00001ED2
		void IValueField<TValueType>.StopDragging()
		{
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x0000FCD0 File Offset: 0x0000DED0
		public override void SetValueWithoutNotify(TValueType newValue)
		{
			TValueType tvalueType = (this.clamped ? this.GetClampedValue(newValue) : newValue);
			base.SetValueWithoutNotify(tvalueType);
			this.UpdateDragElementPosition();
			this.UpdateTextFieldValue();
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000425 RID: 1061 RVA: 0x0000FD08 File Offset: 0x0000DF08
		// (set) Token: 0x06000426 RID: 1062 RVA: 0x0000FD20 File Offset: 0x0000DF20
		public SliderDirection direction
		{
			get
			{
				return this.m_Direction;
			}
			set
			{
				this.m_Direction = value;
				bool flag = this.m_Direction == SliderDirection.Horizontal;
				if (flag)
				{
					base.RemoveFromClassList(BaseSlider<TValueType>.verticalVariantUssClassName);
					base.AddToClassList(BaseSlider<TValueType>.horizontalVariantUssClassName);
				}
				else
				{
					base.RemoveFromClassList(BaseSlider<TValueType>.horizontalVariantUssClassName);
					base.AddToClassList(BaseSlider<TValueType>.verticalVariantUssClassName);
				}
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000427 RID: 1063 RVA: 0x0000FD78 File Offset: 0x0000DF78
		// (set) Token: 0x06000428 RID: 1064 RVA: 0x0000FD90 File Offset: 0x0000DF90
		public bool inverted
		{
			get
			{
				return this.m_Inverted;
			}
			set
			{
				bool flag = this.m_Inverted != value;
				if (flag)
				{
					this.m_Inverted = value;
					this.UpdateDragElementPosition();
				}
			}
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x0000FDC0 File Offset: 0x0000DFC0
		internal BaseSlider(string label, TValueType start, TValueType end, SliderDirection direction = SliderDirection.Horizontal, float pageSize = 0f)
			: base(label, null)
		{
			base.AddToClassList(BaseSlider<TValueType>.ussClassName);
			base.labelElement.AddToClassList(BaseSlider<TValueType>.labelUssClassName);
			base.visualInput.AddToClassList(BaseSlider<TValueType>.inputUssClassName);
			this.direction = direction;
			this.pageSize = pageSize;
			this.lowValue = start;
			this.highValue = end;
			base.pickingMode = PickingMode.Ignore;
			this.dragContainer = new VisualElement
			{
				name = "unity-drag-container"
			};
			this.dragContainer.AddToClassList(BaseSlider<TValueType>.dragContainerUssClassName);
			this.dragContainer.RegisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.UpdateDragElementPosition), TrickleDown.NoTrickleDown);
			base.visualInput.Add(this.dragContainer);
			this.trackElement = new VisualElement
			{
				name = "unity-tracker",
				usageHints = UsageHints.DynamicColor
			};
			this.trackElement.AddToClassList(BaseSlider<TValueType>.trackerUssClassName);
			this.dragContainer.Add(this.trackElement);
			this.dragBorderElement = new VisualElement
			{
				name = "unity-dragger-border"
			};
			this.dragBorderElement.AddToClassList(BaseSlider<TValueType>.draggerBorderUssClassName);
			this.dragContainer.Add(this.dragBorderElement);
			this.dragElement = new VisualElement
			{
				name = "unity-dragger",
				usageHints = UsageHints.DynamicTransform
			};
			this.dragElement.RegisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.UpdateDragElementPosition), TrickleDown.NoTrickleDown);
			this.dragElement.AddToClassList(BaseSlider<TValueType>.draggerUssClassName);
			this.dragContainer.Add(this.dragElement);
			this.clampedDragger = new ClampedDragger<TValueType>(this, new Action(this.SetSliderValueFromClick), new Action(this.SetSliderValueFromDrag));
			this.dragContainer.pickingMode = PickingMode.Position;
			this.dragContainer.AddManipulator(this.clampedDragger);
			base.RegisterCallback<KeyDownEvent>(new EventCallback<KeyDownEvent>(this.OnKeyDown), TrickleDown.NoTrickleDown);
			base.RegisterCallback<NavigationMoveEvent>(new EventCallback<NavigationMoveEvent>(this.OnNavigationMove), TrickleDown.NoTrickleDown);
			this.UpdateTextFieldVisibility();
			FieldMouseDragger<TValueType> fieldMouseDragger = new FieldMouseDragger<TValueType>(this);
			fieldMouseDragger.SetDragZone(base.labelElement);
			base.labelElement.AddToClassList(BaseField<TValueType>.labelDraggerVariantUssClassName);
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x00010018 File Offset: 0x0000E218
		protected static float GetClosestPowerOfTen(float positiveNumber)
		{
			bool flag = positiveNumber <= 0f;
			float num;
			if (flag)
			{
				num = 1f;
			}
			else
			{
				num = Mathf.Pow(10f, (float)Mathf.RoundToInt(Mathf.Log10(positiveNumber)));
			}
			return num;
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x00010058 File Offset: 0x0000E258
		protected static float RoundToMultipleOf(float value, float roundingValue)
		{
			bool flag = roundingValue == 0f;
			float num;
			if (flag)
			{
				num = value;
			}
			else
			{
				num = Mathf.Round(value / roundingValue) * roundingValue;
			}
			return num;
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x00010084 File Offset: 0x0000E284
		private void ClampValue()
		{
			this.value = base.rawValue;
		}

		// Token: 0x0600042D RID: 1069
		internal abstract TValueType SliderLerpUnclamped(TValueType a, TValueType b, float interpolant);

		// Token: 0x0600042E RID: 1070
		internal abstract float SliderNormalizeValue(TValueType currentValue, TValueType lowerValue, TValueType higherValue);

		// Token: 0x0600042F RID: 1071
		internal abstract TValueType SliderRange();

		// Token: 0x06000430 RID: 1072
		internal abstract TValueType ParseStringToValue(string previousValue, string newValue);

		// Token: 0x06000431 RID: 1073
		internal abstract void ComputeValueFromKey(BaseSlider<TValueType>.SliderKey sliderKey, bool isShift);

		// Token: 0x06000432 RID: 1074 RVA: 0x00010094 File Offset: 0x0000E294
		private TValueType SliderLerpDirectionalUnclamped(TValueType a, TValueType b, float positionInterpolant)
		{
			float num = ((this.direction == SliderDirection.Vertical) ? (1f - positionInterpolant) : positionInterpolant);
			bool inverted = this.inverted;
			TValueType tvalueType;
			if (inverted)
			{
				tvalueType = this.SliderLerpUnclamped(b, a, num);
			}
			else
			{
				tvalueType = this.SliderLerpUnclamped(a, b, num);
			}
			return tvalueType;
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x000100DC File Offset: 0x0000E2DC
		private void SetSliderValueFromDrag()
		{
			bool flag = this.clampedDragger.dragDirection != ClampedDragger<TValueType>.DragDirection.Free;
			if (!flag)
			{
				Vector2 delta = this.clampedDragger.delta;
				bool flag2 = this.direction == SliderDirection.Horizontal;
				if (flag2)
				{
					this.ComputeValueAndDirectionFromDrag(this.dragContainer.resolvedStyle.width, this.dragElement.resolvedStyle.width, this.m_DragElementStartPos.x + delta.x);
				}
				else
				{
					this.ComputeValueAndDirectionFromDrag(this.dragContainer.resolvedStyle.height, this.dragElement.resolvedStyle.height, this.m_DragElementStartPos.y + delta.y);
				}
			}
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x00010194 File Offset: 0x0000E394
		private void ComputeValueAndDirectionFromDrag(float sliderLength, float dragElementLength, float dragElementPos)
		{
			float num = sliderLength - dragElementLength;
			bool flag = Mathf.Abs(num) < 1E-30f;
			if (!flag)
			{
				bool clamped = this.clamped;
				float num2;
				if (clamped)
				{
					num2 = Mathf.Max(0f, Mathf.Min(dragElementPos, num)) / num;
				}
				else
				{
					num2 = dragElementPos / num;
				}
				this.value = this.SliderLerpDirectionalUnclamped(this.lowValue, this.highValue, num2);
			}
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x000101F8 File Offset: 0x0000E3F8
		private void SetSliderValueFromClick()
		{
			bool flag = this.clampedDragger.dragDirection == ClampedDragger<TValueType>.DragDirection.Free;
			if (!flag)
			{
				bool flag2 = this.clampedDragger.dragDirection == ClampedDragger<TValueType>.DragDirection.None;
				if (flag2)
				{
					bool flag3 = Mathf.Approximately(this.pageSize, 0f);
					if (flag3)
					{
						bool flag4 = this.direction == SliderDirection.Horizontal;
						float num;
						float num2;
						float num5;
						float num6;
						float num7;
						if (flag4)
						{
							num = this.dragContainer.resolvedStyle.width;
							num2 = this.dragElement.resolvedStyle.width;
							float num3 = num - num2;
							float num4 = this.clampedDragger.startMousePosition.x - num2 / 2f;
							num5 = Mathf.Max(0f, Mathf.Min(num4, num3));
							num6 = this.dragElement.transform.position.y;
							num7 = num5;
						}
						else
						{
							num = this.dragContainer.resolvedStyle.height;
							num2 = this.dragElement.resolvedStyle.height;
							float num8 = num - num2;
							float num9 = this.clampedDragger.startMousePosition.y - num2 / 2f;
							num5 = this.dragElement.transform.position.x;
							num6 = Mathf.Max(0f, Mathf.Min(num9, num8));
							num7 = num6;
						}
						Vector3 vector = new Vector3(num5, num6, 0f);
						this.dragElement.transform.position = vector;
						this.dragBorderElement.transform.position = vector;
						this.m_DragElementStartPos = new Rect(num5, num6, this.dragElement.resolvedStyle.width, this.dragElement.resolvedStyle.height);
						this.clampedDragger.dragDirection = ClampedDragger<TValueType>.DragDirection.Free;
						this.ComputeValueAndDirectionFromDrag(num, num2, num7);
						return;
					}
					this.m_DragElementStartPos = new Rect(this.dragElement.transform.position.x, this.dragElement.transform.position.y, this.dragElement.resolvedStyle.width, this.dragElement.resolvedStyle.height);
				}
				bool flag5 = this.direction == SliderDirection.Horizontal;
				if (flag5)
				{
					this.ComputeValueAndDirectionFromClick(this.dragContainer.resolvedStyle.width, this.dragElement.resolvedStyle.width, this.dragElement.transform.position.x, this.clampedDragger.lastMousePosition.x);
				}
				else
				{
					this.ComputeValueAndDirectionFromClick(this.dragContainer.resolvedStyle.height, this.dragElement.resolvedStyle.height, this.dragElement.transform.position.y, this.clampedDragger.lastMousePosition.y);
				}
			}
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x000104CC File Offset: 0x0000E6CC
		private void OnKeyDown(KeyDownEvent evt)
		{
			BaseSlider<TValueType>.SliderKey sliderKey = BaseSlider<TValueType>.SliderKey.None;
			bool flag = this.direction == SliderDirection.Horizontal;
			bool flag2 = (flag && evt.keyCode == KeyCode.Home) || (!flag && evt.keyCode == KeyCode.End);
			if (flag2)
			{
				sliderKey = (this.inverted ? BaseSlider<TValueType>.SliderKey.Highest : BaseSlider<TValueType>.SliderKey.Lowest);
			}
			else
			{
				bool flag3 = (flag && evt.keyCode == KeyCode.End) || (!flag && evt.keyCode == KeyCode.Home);
				if (flag3)
				{
					sliderKey = (this.inverted ? BaseSlider<TValueType>.SliderKey.Lowest : BaseSlider<TValueType>.SliderKey.Highest);
				}
				else
				{
					bool flag4 = (flag && evt.keyCode == KeyCode.PageUp) || (!flag && evt.keyCode == KeyCode.PageDown);
					if (flag4)
					{
						sliderKey = (this.inverted ? BaseSlider<TValueType>.SliderKey.HigherPage : BaseSlider<TValueType>.SliderKey.LowerPage);
					}
					else
					{
						bool flag5 = (flag && evt.keyCode == KeyCode.PageDown) || (!flag && evt.keyCode == KeyCode.PageUp);
						if (flag5)
						{
							sliderKey = (this.inverted ? BaseSlider<TValueType>.SliderKey.LowerPage : BaseSlider<TValueType>.SliderKey.HigherPage);
						}
					}
				}
			}
			bool flag6 = sliderKey == BaseSlider<TValueType>.SliderKey.None;
			if (!flag6)
			{
				this.ComputeValueFromKey(sliderKey, evt.shiftKey);
				evt.StopPropagation();
			}
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x000105F0 File Offset: 0x0000E7F0
		private void OnNavigationMove(NavigationMoveEvent evt)
		{
			BaseSlider<TValueType>.SliderKey sliderKey = BaseSlider<TValueType>.SliderKey.None;
			bool flag = this.direction == SliderDirection.Horizontal;
			bool flag2 = evt.direction == (flag ? NavigationMoveEvent.Direction.Left : NavigationMoveEvent.Direction.Down);
			if (flag2)
			{
				sliderKey = (this.inverted ? BaseSlider<TValueType>.SliderKey.Higher : BaseSlider<TValueType>.SliderKey.Lower);
			}
			else
			{
				bool flag3 = evt.direction == (flag ? NavigationMoveEvent.Direction.Right : NavigationMoveEvent.Direction.Up);
				if (flag3)
				{
					sliderKey = (this.inverted ? BaseSlider<TValueType>.SliderKey.Lower : BaseSlider<TValueType>.SliderKey.Higher);
				}
			}
			bool flag4 = sliderKey == BaseSlider<TValueType>.SliderKey.None;
			if (!flag4)
			{
				this.ComputeValueFromKey(sliderKey, evt.shiftKey);
				evt.StopPropagation();
				evt.PreventDefault();
			}
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x00010674 File Offset: 0x0000E874
		internal virtual void ComputeValueAndDirectionFromClick(float sliderLength, float dragElementLength, float dragElementPos, float dragElementLastPos)
		{
			float num = sliderLength - dragElementLength;
			bool flag = Mathf.Abs(num) < 1E-30f;
			if (!flag)
			{
				bool flag2 = dragElementLastPos < dragElementPos;
				bool flag3 = dragElementLastPos > dragElementPos + dragElementLength;
				bool flag4 = (this.inverted ? flag3 : flag2);
				bool flag5 = (this.inverted ? flag2 : flag3);
				this.m_AdjustedPageSizeFromClick = (this.inverted ? (this.m_AdjustedPageSizeFromClick - this.pageSize) : (this.m_AdjustedPageSizeFromClick + this.pageSize));
				bool flag6 = flag4 && this.clampedDragger.dragDirection != ClampedDragger<TValueType>.DragDirection.LowToHigh;
				if (flag6)
				{
					this.clampedDragger.dragDirection = ClampedDragger<TValueType>.DragDirection.HighToLow;
					float num2 = Mathf.Max(0f, Mathf.Min(dragElementPos - this.m_AdjustedPageSizeFromClick, num)) / num;
					this.value = this.SliderLerpDirectionalUnclamped(this.lowValue, this.highValue, num2);
				}
				else
				{
					bool flag7 = flag5 && this.clampedDragger.dragDirection != ClampedDragger<TValueType>.DragDirection.HighToLow;
					if (flag7)
					{
						this.clampedDragger.dragDirection = ClampedDragger<TValueType>.DragDirection.LowToHigh;
						float num3 = Mathf.Max(0f, Mathf.Min(dragElementPos + this.m_AdjustedPageSizeFromClick, num)) / num;
						this.value = this.SliderLerpDirectionalUnclamped(this.lowValue, this.highValue, num3);
					}
				}
			}
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x000107BC File Offset: 0x0000E9BC
		public void AdjustDragElement(float factor)
		{
			bool flag = factor < 1f;
			this.dragElement.visible = flag;
			bool flag2 = flag;
			if (flag2)
			{
				IStyle style = this.dragElement.style;
				this.dragElement.style.visibility = StyleKeyword.Null;
				bool flag3 = this.direction == SliderDirection.Horizontal;
				if (flag3)
				{
					float num = ((base.resolvedStyle.minWidth == StyleKeyword.Auto) ? 0f : base.resolvedStyle.minWidth.value);
					style.width = Mathf.Round(Mathf.Max(this.dragContainer.layout.width * factor, num));
				}
				else
				{
					float num2 = ((base.resolvedStyle.minHeight == StyleKeyword.Auto) ? 0f : base.resolvedStyle.minHeight.value);
					style.height = Mathf.Round(Mathf.Max(this.dragContainer.layout.height * factor, num2));
				}
			}
			this.dragBorderElement.visible = this.dragElement.visible;
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x00010900 File Offset: 0x0000EB00
		private void UpdateDragElementPosition(GeometryChangedEvent evt)
		{
			bool flag = evt.oldRect.size == evt.newRect.size;
			if (!flag)
			{
				this.UpdateDragElementPosition();
			}
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x0001093D File Offset: 0x0000EB3D
		internal override void OnViewDataReady()
		{
			base.OnViewDataReady();
			this.UpdateDragElementPosition();
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x00010950 File Offset: 0x0000EB50
		private bool SameValues(float a, float b, float epsilon)
		{
			return Mathf.Abs(b - a) < epsilon;
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x00010970 File Offset: 0x0000EB70
		private void UpdateDragElementPosition()
		{
			bool flag = base.panel == null;
			if (!flag)
			{
				float num = this.SliderNormalizeValue(this.value, this.lowValue, this.highValue);
				float num2 = (this.inverted ? (1f - num) : num);
				float num3 = base.scaledPixelsPerPoint * 0.5f;
				bool flag2 = this.direction == SliderDirection.Horizontal;
				if (flag2)
				{
					float width = this.dragElement.resolvedStyle.width;
					float num4 = -this.dragElement.resolvedStyle.marginLeft - this.dragElement.resolvedStyle.marginRight;
					float num5 = this.dragContainer.layout.width - width + num4;
					float num6 = num2 * num5;
					bool flag3 = float.IsNaN(num6);
					if (!flag3)
					{
						float x = this.dragElement.transform.position.x;
						bool flag4 = !this.SameValues(x, num6, num3);
						if (flag4)
						{
							Vector3 vector = new Vector3(num6, 0f, 0f);
							this.dragElement.transform.position = vector;
							this.dragBorderElement.transform.position = vector;
							this.m_AdjustedPageSizeFromClick = 0f;
						}
					}
				}
				else
				{
					float height = this.dragElement.resolvedStyle.height;
					float num7 = this.dragContainer.resolvedStyle.height - height;
					float num8 = (1f - num2) * num7;
					bool flag5 = float.IsNaN(num8);
					if (!flag5)
					{
						float y = this.dragElement.transform.position.y;
						bool flag6 = !this.SameValues(y, num8, num3);
						if (flag6)
						{
							Vector3 vector2 = new Vector3(0f, num8, 0f);
							this.dragElement.transform.position = vector2;
							this.dragBorderElement.transform.position = vector2;
							this.m_AdjustedPageSizeFromClick = 0f;
						}
					}
				}
			}
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x00010B74 File Offset: 0x0000ED74
		[EventInterest(new Type[] { typeof(GeometryChangedEvent) })]
		protected override void ExecuteDefaultAction(EventBase evt)
		{
			base.ExecuteDefaultAction(evt);
			bool flag = evt == null;
			if (!flag)
			{
				bool flag2 = evt.eventTypeId == EventBase<GeometryChangedEvent>.TypeId();
				if (flag2)
				{
					this.UpdateDragElementPosition((GeometryChangedEvent)evt);
				}
			}
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x00010BB8 File Offset: 0x0000EDB8
		private void UpdateTextFieldVisibility()
		{
			bool showInputField = this.showInputField;
			if (showInputField)
			{
				bool flag = this.inputTextField == null;
				if (flag)
				{
					this.inputTextField = new TextField
					{
						name = "unity-text-field"
					};
					this.inputTextField.AddToClassList(BaseSlider<TValueType>.textFieldClassName);
					this.inputTextField.RegisterCallback<NavigationMoveEvent>(new EventCallback<NavigationMoveEvent>(this.OnInputNavigationMoveEvent), TrickleDown.TrickleDown);
					this.inputTextField.RegisterValueChangedCallback<string>(new EventCallback<ChangeEvent<string>>(this.OnTextFieldValueChange));
					this.inputTextField.RegisterCallback<FocusInEvent>(new EventCallback<FocusInEvent>(this.OnTextFieldFocusIn), TrickleDown.NoTrickleDown);
					this.inputTextField.RegisterCallback<FocusOutEvent>(new EventCallback<FocusOutEvent>(this.OnTextFieldFocusOut), TrickleDown.NoTrickleDown);
					base.visualInput.Add(this.inputTextField);
					this.UpdateTextFieldValue();
				}
			}
			else
			{
				bool flag2 = this.inputTextField != null && this.inputTextField.panel != null;
				if (flag2)
				{
					bool flag3 = this.inputTextField.panel != null;
					if (flag3)
					{
						this.inputTextField.RemoveFromHierarchy();
					}
					this.inputTextField.UnregisterCallback<NavigationMoveEvent>(new EventCallback<NavigationMoveEvent>(this.OnInputNavigationMoveEvent), TrickleDown.NoTrickleDown);
					this.inputTextField.UnregisterValueChangedCallback<string>(new EventCallback<ChangeEvent<string>>(this.OnTextFieldValueChange));
					this.inputTextField.UnregisterCallback<FocusInEvent>(new EventCallback<FocusInEvent>(this.OnTextFieldFocusIn), TrickleDown.NoTrickleDown);
					this.inputTextField.UnregisterCallback<FocusOutEvent>(new EventCallback<FocusOutEvent>(this.OnTextFieldFocusOut), TrickleDown.NoTrickleDown);
					this.inputTextField = null;
				}
			}
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x00010D3C File Offset: 0x0000EF3C
		private void UpdateTextFieldValue()
		{
			bool flag = this.inputTextField == null || this.m_IsEditingTextField;
			if (!flag)
			{
				this.inputTextField.SetValueWithoutNotify(string.Format(CultureInfo.InvariantCulture, "{0:g7}", this.value));
			}
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x00010D87 File Offset: 0x0000EF87
		private void OnTextFieldFocusIn(FocusInEvent evt)
		{
			this.m_IsEditingTextField = true;
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x00010D91 File Offset: 0x0000EF91
		private void OnTextFieldFocusOut(FocusOutEvent evt)
		{
			this.m_IsEditingTextField = false;
			this.UpdateTextFieldValue();
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x00010DA2 File Offset: 0x0000EFA2
		private void OnInputNavigationMoveEvent(NavigationMoveEvent evt)
		{
			evt.StopPropagation();
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x00010DAC File Offset: 0x0000EFAC
		private void OnTextFieldValueChange(ChangeEvent<string> evt)
		{
			TValueType clampedValue = this.GetClampedValue(this.ParseStringToValue(evt.previousValue, evt.newValue));
			bool flag = !EqualityComparer<TValueType>.Default.Equals(clampedValue, this.value);
			if (flag)
			{
				this.value = clampedValue;
				evt.StopPropagation();
				bool flag2 = base.elementPanel != null;
				if (flag2)
				{
					this.OnViewDataReady();
				}
			}
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x00010E10 File Offset: 0x0000F010
		protected override void UpdateMixedValueContent()
		{
			bool showMixedValue = base.showMixedValue;
			if (showMixedValue)
			{
				VisualElement dragElement = this.dragElement;
				if (dragElement != null)
				{
					dragElement.RemoveFromHierarchy();
				}
				bool flag = this.inputTextField != null;
				if (flag)
				{
					this.inputTextField.showMixedValue = true;
				}
			}
			else
			{
				this.dragContainer.Add(this.dragElement);
				bool flag2 = this.inputTextField != null;
				if (flag2)
				{
					this.inputTextField.showMixedValue = false;
				}
			}
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x00010E8C File Offset: 0x0000F08C
		internal override void RegisterEditingCallbacks()
		{
			base.labelElement.RegisterCallback<PointerDownEvent>(new EventCallback<PointerDownEvent>(base.StartEditing), TrickleDown.TrickleDown);
			this.dragContainer.RegisterCallback<PointerDownEvent>(new EventCallback<PointerDownEvent>(base.StartEditing), TrickleDown.TrickleDown);
			this.dragContainer.RegisterCallback<PointerUpEvent>(new EventCallback<PointerUpEvent>(base.EndEditing), TrickleDown.NoTrickleDown);
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x00010EE8 File Offset: 0x0000F0E8
		internal override void UnregisterEditingCallbacks()
		{
			base.labelElement.UnregisterCallback<PointerDownEvent>(new EventCallback<PointerDownEvent>(base.StartEditing), TrickleDown.TrickleDown);
			this.dragContainer.UnregisterCallback<PointerDownEvent>(new EventCallback<PointerDownEvent>(base.StartEditing), TrickleDown.TrickleDown);
			this.dragContainer.UnregisterCallback<PointerUpEvent>(new EventCallback<PointerUpEvent>(base.EndEditing), TrickleDown.NoTrickleDown);
		}

		// Token: 0x04000175 RID: 373
		private float m_AdjustedPageSizeFromClick = 0f;

		// Token: 0x0400017B RID: 379
		private bool m_IsEditingTextField;

		// Token: 0x0400017C RID: 380
		[SerializeField]
		private TValueType m_LowValue;

		// Token: 0x0400017D RID: 381
		[SerializeField]
		private TValueType m_HighValue;

		// Token: 0x0400017E RID: 382
		private float m_PageSize;

		// Token: 0x0400017F RID: 383
		private bool m_ShowInputField = false;

		// Token: 0x04000182 RID: 386
		private Rect m_DragElementStartPos;

		// Token: 0x04000183 RID: 387
		private SliderDirection m_Direction;

		// Token: 0x04000184 RID: 388
		private bool m_Inverted = false;

		// Token: 0x04000185 RID: 389
		internal const float kDefaultPageSize = 0f;

		// Token: 0x04000186 RID: 390
		internal const bool kDefaultShowInputField = false;

		// Token: 0x04000187 RID: 391
		internal const bool kDefaultInverted = false;

		// Token: 0x04000188 RID: 392
		public new static readonly string ussClassName = "unity-base-slider";

		// Token: 0x04000189 RID: 393
		public new static readonly string labelUssClassName = BaseSlider<TValueType>.ussClassName + "__label";

		// Token: 0x0400018A RID: 394
		public new static readonly string inputUssClassName = BaseSlider<TValueType>.ussClassName + "__input";

		// Token: 0x0400018B RID: 395
		public static readonly string horizontalVariantUssClassName = BaseSlider<TValueType>.ussClassName + "--horizontal";

		// Token: 0x0400018C RID: 396
		public static readonly string verticalVariantUssClassName = BaseSlider<TValueType>.ussClassName + "--vertical";

		// Token: 0x0400018D RID: 397
		public static readonly string dragContainerUssClassName = BaseSlider<TValueType>.ussClassName + "__drag-container";

		// Token: 0x0400018E RID: 398
		public static readonly string trackerUssClassName = BaseSlider<TValueType>.ussClassName + "__tracker";

		// Token: 0x0400018F RID: 399
		public static readonly string draggerUssClassName = BaseSlider<TValueType>.ussClassName + "__dragger";

		// Token: 0x04000190 RID: 400
		public static readonly string draggerBorderUssClassName = BaseSlider<TValueType>.ussClassName + "__dragger-border";

		// Token: 0x04000191 RID: 401
		public static readonly string textFieldClassName = BaseSlider<TValueType>.ussClassName + "__text-field";

		// Token: 0x02000063 RID: 99
		public new class UxmlTraits : BaseField<TValueType>.UxmlTraits
		{
			// Token: 0x06000449 RID: 1097 RVA: 0x0001100F File Offset: 0x0000F20F
			public UxmlTraits()
			{
				this.m_PickingMode.defaultValue = PickingMode.Ignore;
			}
		}

		// Token: 0x02000064 RID: 100
		internal enum SliderKey
		{
			// Token: 0x04000193 RID: 403
			None,
			// Token: 0x04000194 RID: 404
			Lowest,
			// Token: 0x04000195 RID: 405
			LowerPage,
			// Token: 0x04000196 RID: 406
			Lower,
			// Token: 0x04000197 RID: 407
			Higher,
			// Token: 0x04000198 RID: 408
			HigherPage,
			// Token: 0x04000199 RID: 409
			Highest
		}
	}
}
