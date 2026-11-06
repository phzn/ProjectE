using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace UnityEngine.UIElements
{
	// Token: 0x0200011D RID: 285
	public class Scroller : VisualElement
	{
		// Token: 0x14000034 RID: 52
		// (add) Token: 0x0600097D RID: 2429 RVA: 0x00024AF0 File Offset: 0x00022CF0
		// (remove) Token: 0x0600097E RID: 2430 RVA: 0x00024B28 File Offset: 0x00022D28
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<float> valueChanged;

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x0600097F RID: 2431 RVA: 0x00024B5D File Offset: 0x00022D5D
		// (set) Token: 0x06000980 RID: 2432 RVA: 0x00024B65 File Offset: 0x00022D65
		public Slider slider { get; private set; }

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06000981 RID: 2433 RVA: 0x00024B6E File Offset: 0x00022D6E
		// (set) Token: 0x06000982 RID: 2434 RVA: 0x00024B76 File Offset: 0x00022D76
		public RepeatButton lowButton { get; private set; }

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000983 RID: 2435 RVA: 0x00024B7F File Offset: 0x00022D7F
		// (set) Token: 0x06000984 RID: 2436 RVA: 0x00024B87 File Offset: 0x00022D87
		public RepeatButton highButton { get; private set; }

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000985 RID: 2437 RVA: 0x00024B90 File Offset: 0x00022D90
		// (set) Token: 0x06000986 RID: 2438 RVA: 0x00024BAD File Offset: 0x00022DAD
		public float value
		{
			get
			{
				return this.slider.value;
			}
			set
			{
				this.slider.value = value;
			}
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000987 RID: 2439 RVA: 0x00024BC0 File Offset: 0x00022DC0
		// (set) Token: 0x06000988 RID: 2440 RVA: 0x00024BDD File Offset: 0x00022DDD
		public float lowValue
		{
			get
			{
				return this.slider.lowValue;
			}
			set
			{
				this.slider.lowValue = value;
			}
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000989 RID: 2441 RVA: 0x00024BF0 File Offset: 0x00022DF0
		// (set) Token: 0x0600098A RID: 2442 RVA: 0x00024C0D File Offset: 0x00022E0D
		public float highValue
		{
			get
			{
				return this.slider.highValue;
			}
			set
			{
				this.slider.highValue = value;
			}
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x0600098B RID: 2443 RVA: 0x00024C20 File Offset: 0x00022E20
		// (set) Token: 0x0600098C RID: 2444 RVA: 0x00024C44 File Offset: 0x00022E44
		public SliderDirection direction
		{
			get
			{
				return (base.resolvedStyle.flexDirection == FlexDirection.Row) ? SliderDirection.Horizontal : SliderDirection.Vertical;
			}
			set
			{
				this.slider.direction = value;
				this.slider.inverted = value == SliderDirection.Vertical;
				bool flag = value == SliderDirection.Horizontal;
				if (flag)
				{
					base.style.flexDirection = FlexDirection.Row;
					base.AddToClassList(Scroller.horizontalVariantUssClassName);
					base.RemoveFromClassList(Scroller.verticalVariantUssClassName);
				}
				else
				{
					base.style.flexDirection = FlexDirection.Column;
					base.AddToClassList(Scroller.verticalVariantUssClassName);
					base.RemoveFromClassList(Scroller.horizontalVariantUssClassName);
				}
			}
		}

		// Token: 0x0600098D RID: 2445 RVA: 0x00024CD1 File Offset: 0x00022ED1
		public Scroller()
			: this(0f, 0f, null, SliderDirection.Vertical)
		{
		}

		// Token: 0x0600098E RID: 2446 RVA: 0x00024CE8 File Offset: 0x00022EE8
		public Scroller(float lowValue, float highValue, Action<float> valueChanged, SliderDirection direction = SliderDirection.Vertical)
		{
			base.AddToClassList(Scroller.ussClassName);
			this.slider = new Scroller.ScrollerSlider(lowValue, highValue, direction, 20f)
			{
				name = "unity-slider",
				viewDataKey = "Slider"
			};
			this.slider.AddToClassList(Scroller.sliderUssClassName);
			this.slider.RegisterValueChangedCallback<float>(new EventCallback<ChangeEvent<float>>(this.OnSliderValueChange));
			this.lowButton = new RepeatButton(new Action(this.ScrollPageUp), 250L, 30L)
			{
				name = "unity-low-button"
			};
			this.lowButton.AddToClassList(Scroller.lowButtonUssClassName);
			base.Add(this.lowButton);
			this.highButton = new RepeatButton(new Action(this.ScrollPageDown), 250L, 30L)
			{
				name = "unity-high-button"
			};
			this.highButton.AddToClassList(Scroller.highButtonUssClassName);
			base.Add(this.highButton);
			base.Add(this.slider);
			this.direction = direction;
			this.valueChanged = valueChanged;
		}

		// Token: 0x0600098F RID: 2447 RVA: 0x00024E12 File Offset: 0x00023012
		public void Adjust(float factor)
		{
			base.SetEnabled(factor < 1f);
			this.slider.AdjustDragElement(factor);
		}

		// Token: 0x06000990 RID: 2448 RVA: 0x00024E31 File Offset: 0x00023031
		private void OnSliderValueChange(ChangeEvent<float> evt)
		{
			this.value = evt.newValue;
			Action<float> action = this.valueChanged;
			if (action != null)
			{
				action(this.slider.value);
			}
			base.IncrementVersion(VersionChangeType.Repaint);
		}

		// Token: 0x06000991 RID: 2449 RVA: 0x00024E6A File Offset: 0x0002306A
		public void ScrollPageUp()
		{
			this.ScrollPageUp(1f);
		}

		// Token: 0x06000992 RID: 2450 RVA: 0x00024E79 File Offset: 0x00023079
		public void ScrollPageDown()
		{
			this.ScrollPageDown(1f);
		}

		// Token: 0x06000993 RID: 2451 RVA: 0x00024E88 File Offset: 0x00023088
		public void ScrollPageUp(float factor)
		{
			this.value -= factor * (this.slider.pageSize * ((this.slider.lowValue < this.slider.highValue) ? 1f : (-1f)));
		}

		// Token: 0x06000994 RID: 2452 RVA: 0x00024ED8 File Offset: 0x000230D8
		public void ScrollPageDown(float factor)
		{
			this.value += factor * (this.slider.pageSize * ((this.slider.lowValue < this.slider.highValue) ? 1f : (-1f)));
		}

		// Token: 0x04000451 RID: 1105
		internal const float kDefaultPageSize = 20f;

		// Token: 0x04000452 RID: 1106
		public static readonly string ussClassName = "unity-scroller";

		// Token: 0x04000453 RID: 1107
		public static readonly string horizontalVariantUssClassName = Scroller.ussClassName + "--horizontal";

		// Token: 0x04000454 RID: 1108
		public static readonly string verticalVariantUssClassName = Scroller.ussClassName + "--vertical";

		// Token: 0x04000455 RID: 1109
		public static readonly string sliderUssClassName = Scroller.ussClassName + "__slider";

		// Token: 0x04000456 RID: 1110
		public static readonly string lowButtonUssClassName = Scroller.ussClassName + "__low-button";

		// Token: 0x04000457 RID: 1111
		public static readonly string highButtonUssClassName = Scroller.ussClassName + "__high-button";

		// Token: 0x0200011E RID: 286
		private class ScrollerSlider : Slider
		{
			// Token: 0x06000996 RID: 2454 RVA: 0x00024FA3 File Offset: 0x000231A3
			public ScrollerSlider(float start, float end, SliderDirection direction, float pageSize)
				: base(start, end, direction, pageSize)
			{
			}

			// Token: 0x06000997 RID: 2455 RVA: 0x00024FB4 File Offset: 0x000231B4
			internal override float SliderNormalizeValue(float currentValue, float lowerValue, float higherValue)
			{
				return Mathf.Clamp(base.SliderNormalizeValue(currentValue, lowerValue, higherValue), 0f, 1f);
			}
		}

		// Token: 0x0200011F RID: 287
		public new class UxmlFactory : UxmlFactory<Scroller, Scroller.UxmlTraits>
		{
		}

		// Token: 0x02000120 RID: 288
		public new class UxmlTraits : VisualElement.UxmlTraits
		{
			// Token: 0x170001CC RID: 460
			// (get) Token: 0x06000999 RID: 2457 RVA: 0x00024FE8 File Offset: 0x000231E8
			public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
			{
				get
				{
					yield break;
				}
			}

			// Token: 0x0600099A RID: 2458 RVA: 0x00025008 File Offset: 0x00023208
			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ve, bag, cc);
				Scroller scroller = (Scroller)ve;
				scroller.slider.lowValue = this.m_LowValue.GetValueFromBag(bag, cc);
				scroller.slider.highValue = this.m_HighValue.GetValueFromBag(bag, cc);
				scroller.direction = this.m_Direction.GetValueFromBag(bag, cc);
				scroller.value = this.m_Value.GetValueFromBag(bag, cc);
			}

			// Token: 0x04000458 RID: 1112
			private UxmlFloatAttributeDescription m_LowValue = new UxmlFloatAttributeDescription
			{
				name = "low-value",
				obsoleteNames = new string[] { "lowValue" }
			};

			// Token: 0x04000459 RID: 1113
			private UxmlFloatAttributeDescription m_HighValue = new UxmlFloatAttributeDescription
			{
				name = "high-value",
				obsoleteNames = new string[] { "highValue" }
			};

			// Token: 0x0400045A RID: 1114
			private UxmlEnumAttributeDescription<SliderDirection> m_Direction = new UxmlEnumAttributeDescription<SliderDirection>
			{
				name = "direction",
				defaultValue = SliderDirection.Vertical
			};

			// Token: 0x0400045B RID: 1115
			private UxmlFloatAttributeDescription m_Value = new UxmlFloatAttributeDescription
			{
				name = "value"
			};
		}
	}
}
