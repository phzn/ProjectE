using System;
using UnityEngine.UIElements.StyleSheets;

namespace UnityEngine.UIElements
{
	// Token: 0x02000124 RID: 292
	public class ScrollView : VisualElement
	{
		// Token: 0x170001CF RID: 463
		// (get) Token: 0x060009A4 RID: 2468 RVA: 0x000251C4 File Offset: 0x000233C4
		// (set) Token: 0x060009A5 RID: 2469 RVA: 0x000251DC File Offset: 0x000233DC
		public ScrollerVisibility horizontalScrollerVisibility
		{
			get
			{
				return this.m_HorizontalScrollerVisibility;
			}
			set
			{
				this.m_HorizontalScrollerVisibility = value;
				this.UpdateScrollers(this.needsHorizontal, this.needsVertical);
			}
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x060009A6 RID: 2470 RVA: 0x000251FC File Offset: 0x000233FC
		// (set) Token: 0x060009A7 RID: 2471 RVA: 0x00025214 File Offset: 0x00023414
		public ScrollerVisibility verticalScrollerVisibility
		{
			get
			{
				return this.m_VerticalScrollerVisibility;
			}
			set
			{
				this.m_VerticalScrollerVisibility = value;
				this.UpdateScrollers(this.needsHorizontal, this.needsVertical);
			}
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x060009A8 RID: 2472 RVA: 0x00025231 File Offset: 0x00023431
		// (set) Token: 0x060009A9 RID: 2473 RVA: 0x0002523C File Offset: 0x0002343C
		[Obsolete("showHorizontal is obsolete. Use horizontalScrollerVisibility instead")]
		public bool showHorizontal
		{
			get
			{
				return this.horizontalScrollerVisibility == ScrollerVisibility.AlwaysVisible;
			}
			set
			{
				this.m_HorizontalScrollerVisibility = (value ? ScrollerVisibility.AlwaysVisible : ScrollerVisibility.Auto);
			}
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x060009AA RID: 2474 RVA: 0x0002524B File Offset: 0x0002344B
		// (set) Token: 0x060009AB RID: 2475 RVA: 0x00025256 File Offset: 0x00023456
		[Obsolete("showVertical is obsolete. Use verticalScrollerVisibility instead")]
		public bool showVertical
		{
			get
			{
				return this.verticalScrollerVisibility == ScrollerVisibility.AlwaysVisible;
			}
			set
			{
				this.m_VerticalScrollerVisibility = (value ? ScrollerVisibility.AlwaysVisible : ScrollerVisibility.Auto);
			}
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x060009AC RID: 2476 RVA: 0x00025265 File Offset: 0x00023465
		internal bool needsHorizontal
		{
			get
			{
				return (this.mode != ScrollViewMode.Vertical && this.horizontalScrollerVisibility == ScrollerVisibility.AlwaysVisible) || (this.horizontalScrollerVisibility == ScrollerVisibility.Auto && this.scrollableWidth > 0.001f);
			}
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x060009AD RID: 2477 RVA: 0x00025293 File Offset: 0x00023493
		internal bool needsVertical
		{
			get
			{
				return (this.mode != ScrollViewMode.Horizontal && this.verticalScrollerVisibility == ScrollerVisibility.AlwaysVisible) || (this.verticalScrollerVisibility == ScrollerVisibility.Auto && this.scrollableHeight > 0.001f);
			}
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x060009AE RID: 2478 RVA: 0x000252C4 File Offset: 0x000234C4
		internal bool isVerticalScrollDisplayed
		{
			get
			{
				return this.verticalScroller.resolvedStyle.display == DisplayStyle.Flex;
			}
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x060009AF RID: 2479 RVA: 0x000252EC File Offset: 0x000234EC
		internal bool isHorizontalScrollDisplayed
		{
			get
			{
				return this.horizontalScroller.resolvedStyle.display == DisplayStyle.Flex;
			}
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x060009B0 RID: 2480 RVA: 0x00025314 File Offset: 0x00023514
		// (set) Token: 0x060009B1 RID: 2481 RVA: 0x00025344 File Offset: 0x00023544
		public Vector2 scrollOffset
		{
			get
			{
				return new Vector2(this.horizontalScroller.value, this.verticalScroller.value);
			}
			set
			{
				bool flag = value != this.scrollOffset;
				if (flag)
				{
					this.horizontalScroller.value = value.x;
					this.verticalScroller.value = value.y;
					bool flag2 = base.panel != null;
					if (flag2)
					{
						this.UpdateScrollers(this.needsHorizontal, this.needsVertical);
						this.UpdateContentViewTransform();
					}
				}
			}
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x060009B2 RID: 2482 RVA: 0x000253B4 File Offset: 0x000235B4
		// (set) Token: 0x060009B3 RID: 2483 RVA: 0x000253CC File Offset: 0x000235CC
		public float horizontalPageSize
		{
			get
			{
				return this.m_HorizontalPageSize;
			}
			set
			{
				this.m_HorizontalPageSize = value;
				this.UpdateHorizontalSliderPageSize();
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x060009B4 RID: 2484 RVA: 0x000253E0 File Offset: 0x000235E0
		// (set) Token: 0x060009B5 RID: 2485 RVA: 0x000253F8 File Offset: 0x000235F8
		public float verticalPageSize
		{
			get
			{
				return this.m_VerticalPageSize;
			}
			set
			{
				this.m_VerticalPageSize = value;
				this.UpdateVerticalSliderPageSize();
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x060009B6 RID: 2486 RVA: 0x0002540C File Offset: 0x0002360C
		// (set) Token: 0x060009B7 RID: 2487 RVA: 0x00025424 File Offset: 0x00023624
		public float mouseWheelScrollSize
		{
			get
			{
				return this.m_MouseWheelScrollSize;
			}
			set
			{
				float mouseWheelScrollSize = this.m_MouseWheelScrollSize;
				bool flag = Math.Abs(this.m_MouseWheelScrollSize - value) > float.Epsilon;
				if (flag)
				{
					this.m_MouseWheelScrollSizeIsInline = true;
					this.m_MouseWheelScrollSize = value;
				}
			}
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x060009B8 RID: 2488 RVA: 0x00025464 File Offset: 0x00023664
		internal float scrollableWidth
		{
			get
			{
				return this.contentContainer.boundingBox.width - this.contentViewport.layout.width;
			}
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x060009B9 RID: 2489 RVA: 0x000254A0 File Offset: 0x000236A0
		internal float scrollableHeight
		{
			get
			{
				return this.contentContainer.boundingBox.height - this.contentViewport.layout.height;
			}
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x060009BA RID: 2490 RVA: 0x000254D9 File Offset: 0x000236D9
		private bool hasInertia
		{
			get
			{
				return this.scrollDecelerationRate > 0f;
			}
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x060009BB RID: 2491 RVA: 0x000254E8 File Offset: 0x000236E8
		// (set) Token: 0x060009BC RID: 2492 RVA: 0x00025500 File Offset: 0x00023700
		public float scrollDecelerationRate
		{
			get
			{
				return this.m_ScrollDecelerationRate;
			}
			set
			{
				this.m_ScrollDecelerationRate = Mathf.Max(0f, value);
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x060009BD RID: 2493 RVA: 0x00025514 File Offset: 0x00023714
		// (set) Token: 0x060009BE RID: 2494 RVA: 0x0002552C File Offset: 0x0002372C
		public float elasticity
		{
			get
			{
				return this.m_Elasticity;
			}
			set
			{
				this.m_Elasticity = Mathf.Max(0f, value);
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x060009BF RID: 2495 RVA: 0x00025540 File Offset: 0x00023740
		// (set) Token: 0x060009C0 RID: 2496 RVA: 0x00025558 File Offset: 0x00023758
		public ScrollView.TouchScrollBehavior touchScrollBehavior
		{
			get
			{
				return this.m_TouchScrollBehavior;
			}
			set
			{
				this.m_TouchScrollBehavior = value;
				bool flag = this.m_TouchScrollBehavior == ScrollView.TouchScrollBehavior.Clamped;
				if (flag)
				{
					this.horizontalScroller.slider.clamped = true;
					this.verticalScroller.slider.clamped = true;
				}
				else
				{
					this.horizontalScroller.slider.clamped = false;
					this.verticalScroller.slider.clamped = false;
				}
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x060009C1 RID: 2497 RVA: 0x000255C8 File Offset: 0x000237C8
		// (set) Token: 0x060009C2 RID: 2498 RVA: 0x000255D0 File Offset: 0x000237D0
		public ScrollView.NestedInteractionKind nestedInteractionKind
		{
			get
			{
				return this.m_NestedInteractionKind;
			}
			set
			{
				this.m_NestedInteractionKind = value;
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x060009C3 RID: 2499 RVA: 0x000255DC File Offset: 0x000237DC
		// (set) Token: 0x060009C4 RID: 2500 RVA: 0x000255F4 File Offset: 0x000237F4
		public long elasticAnimationIntervalMs
		{
			get
			{
				return this.m_ElasticAnimationIntervalMs;
			}
			set
			{
				long elasticAnimationIntervalMs = this.m_ElasticAnimationIntervalMs;
				this.m_ElasticAnimationIntervalMs = value;
				bool flag = elasticAnimationIntervalMs != this.m_ElasticAnimationIntervalMs;
				if (flag)
				{
					this.m_PostPointerUpAnimation = base.schedule.Execute(new Action(this.PostPointerUpAnimation)).Every(this.m_ElasticAnimationIntervalMs);
				}
			}
		}

		// Token: 0x060009C5 RID: 2501 RVA: 0x0002564C File Offset: 0x0002384C
		private void OnHorizontalScrollDragElementChanged(GeometryChangedEvent evt)
		{
			bool flag = evt.oldRect.size == evt.newRect.size;
			if (!flag)
			{
				this.UpdateHorizontalSliderPageSize();
			}
		}

		// Token: 0x060009C6 RID: 2502 RVA: 0x0002568C File Offset: 0x0002388C
		private void OnVerticalScrollDragElementChanged(GeometryChangedEvent evt)
		{
			bool flag = evt.oldRect.size == evt.newRect.size;
			if (!flag)
			{
				this.UpdateVerticalSliderPageSize();
			}
		}

		// Token: 0x060009C7 RID: 2503 RVA: 0x000256CC File Offset: 0x000238CC
		private void UpdateHorizontalSliderPageSize()
		{
			float width = this.horizontalScroller.resolvedStyle.width;
			float num = this.m_HorizontalPageSize;
			bool flag = width > 0f;
			if (flag)
			{
				bool flag2 = Mathf.Approximately(this.m_HorizontalPageSize, -1f);
				if (flag2)
				{
					float width2 = this.horizontalScroller.slider.dragElement.resolvedStyle.width;
					num = width2 * 0.9f;
				}
			}
			bool flag3 = num >= 0f;
			if (flag3)
			{
				this.horizontalScroller.slider.pageSize = num;
			}
		}

		// Token: 0x060009C8 RID: 2504 RVA: 0x00025760 File Offset: 0x00023960
		private void UpdateVerticalSliderPageSize()
		{
			float height = this.verticalScroller.resolvedStyle.height;
			float num = this.m_VerticalPageSize;
			bool flag = height > 0f;
			if (flag)
			{
				bool flag2 = Mathf.Approximately(this.m_VerticalPageSize, -1f);
				if (flag2)
				{
					float height2 = this.verticalScroller.slider.dragElement.resolvedStyle.height;
					num = height2 * 0.9f;
				}
			}
			bool flag3 = num >= 0f;
			if (flag3)
			{
				this.verticalScroller.slider.pageSize = num;
			}
		}

		// Token: 0x060009C9 RID: 2505 RVA: 0x000257F4 File Offset: 0x000239F4
		internal void UpdateContentViewTransform()
		{
			Vector3 position = this.contentContainer.transform.position;
			Vector2 scrollOffset = this.scrollOffset;
			bool needsVertical = this.needsVertical;
			if (needsVertical)
			{
				scrollOffset.y += this.contentContainer.resolvedStyle.top;
			}
			position.x = GUIUtility.RoundToPixelGrid(-scrollOffset.x);
			position.y = GUIUtility.RoundToPixelGrid(-scrollOffset.y);
			this.contentContainer.transform.position = position;
			base.IncrementVersion(VersionChangeType.Repaint);
		}

		// Token: 0x060009CA RID: 2506 RVA: 0x00025884 File Offset: 0x00023A84
		public void ScrollTo(VisualElement child)
		{
			bool flag = child == null;
			if (flag)
			{
				throw new ArgumentNullException("child");
			}
			bool flag2 = !this.contentContainer.Contains(child);
			if (flag2)
			{
				throw new ArgumentException("Cannot scroll to a VisualElement that's not a child of the ScrollView content-container.");
			}
			this.m_Velocity = Vector2.zero;
			float num = 0f;
			float num2 = 0f;
			bool flag3 = this.scrollableHeight > 0f;
			if (flag3)
			{
				num = this.GetYDeltaOffset(child);
				this.verticalScroller.value = this.scrollOffset.y + num;
			}
			bool flag4 = this.scrollableWidth > 0f;
			if (flag4)
			{
				num2 = this.GetXDeltaOffset(child);
				this.horizontalScroller.value = this.scrollOffset.x + num2;
			}
			bool flag5 = num == 0f && num2 == 0f;
			if (!flag5)
			{
				this.UpdateContentViewTransform();
			}
		}

		// Token: 0x060009CB RID: 2507 RVA: 0x00025968 File Offset: 0x00023B68
		private float GetXDeltaOffset(VisualElement child)
		{
			float num = this.contentContainer.transform.position.x * -1f;
			Rect worldBound = this.contentViewport.worldBound;
			float num2 = worldBound.xMin + num;
			float num3 = worldBound.xMax + num;
			Rect worldBound2 = child.worldBound;
			float num4 = worldBound2.xMin + num;
			float num5 = worldBound2.xMax + num;
			bool flag = (num4 >= num2 && num5 <= num3) || float.IsNaN(num4) || float.IsNaN(num5);
			float num6;
			if (flag)
			{
				num6 = 0f;
			}
			else
			{
				float deltaDistance = this.GetDeltaDistance(num2, num3, num4, num5);
				num6 = deltaDistance * this.horizontalScroller.highValue / this.scrollableWidth;
			}
			return num6;
		}

		// Token: 0x060009CC RID: 2508 RVA: 0x00025A28 File Offset: 0x00023C28
		private float GetYDeltaOffset(VisualElement child)
		{
			float num = this.contentContainer.transform.position.y * -1f;
			Rect worldBound = this.contentViewport.worldBound;
			float num2 = worldBound.yMin + num;
			float num3 = worldBound.yMax + num;
			Rect worldBound2 = child.worldBound;
			float num4 = worldBound2.yMin + num;
			float num5 = worldBound2.yMax + num;
			bool flag = (num4 >= num2 && num5 <= num3) || float.IsNaN(num4) || float.IsNaN(num5);
			float num6;
			if (flag)
			{
				num6 = 0f;
			}
			else
			{
				float deltaDistance = this.GetDeltaDistance(num2, num3, num4, num5);
				num6 = deltaDistance * this.verticalScroller.highValue / this.scrollableHeight;
			}
			return num6;
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x00025AE8 File Offset: 0x00023CE8
		private float GetDeltaDistance(float viewMin, float viewMax, float childBoundaryMin, float childBoundaryMax)
		{
			float num = viewMax - viewMin;
			float num2 = childBoundaryMax - childBoundaryMin;
			bool flag = num2 > num;
			float num3;
			if (flag)
			{
				bool flag2 = viewMin > childBoundaryMin && childBoundaryMax > viewMax;
				if (flag2)
				{
					num3 = 0f;
				}
				else
				{
					num3 = ((childBoundaryMin > viewMin) ? (childBoundaryMin - viewMin) : (childBoundaryMax - viewMax));
				}
			}
			else
			{
				float num4 = childBoundaryMax - viewMax;
				bool flag3 = num4 < -1f;
				if (flag3)
				{
					num4 = childBoundaryMin - viewMin;
				}
				num3 = num4;
			}
			return num3;
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x060009CE RID: 2510 RVA: 0x00025B54 File Offset: 0x00023D54
		public VisualElement contentViewport { get; }

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x060009CF RID: 2511 RVA: 0x00025B5C File Offset: 0x00023D5C
		public Scroller horizontalScroller { get; }

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x060009D0 RID: 2512 RVA: 0x00025B64 File Offset: 0x00023D64
		public Scroller verticalScroller { get; }

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x060009D1 RID: 2513 RVA: 0x00025B6C File Offset: 0x00023D6C
		public override VisualElement contentContainer
		{
			get
			{
				return this.m_ContentContainer;
			}
		}

		// Token: 0x060009D2 RID: 2514 RVA: 0x00025B84 File Offset: 0x00023D84
		public ScrollView()
			: this(ScrollViewMode.Vertical)
		{
		}

		// Token: 0x060009D3 RID: 2515 RVA: 0x00025B90 File Offset: 0x00023D90
		public ScrollView(ScrollViewMode scrollViewMode)
		{
			base.AddToClassList(ScrollView.ussClassName);
			this.m_ContentAndVerticalScrollContainer = new VisualElement
			{
				name = "unity-content-and-vertical-scroll-container"
			};
			this.m_ContentAndVerticalScrollContainer.AddToClassList(ScrollView.contentAndVerticalScrollUssClassName);
			base.hierarchy.Add(this.m_ContentAndVerticalScrollContainer);
			this.contentViewport = new VisualElement
			{
				name = "unity-content-viewport"
			};
			this.contentViewport.AddToClassList(ScrollView.viewportUssClassName);
			this.contentViewport.RegisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.OnGeometryChanged), TrickleDown.NoTrickleDown);
			this.contentViewport.pickingMode = PickingMode.Ignore;
			this.m_ContentAndVerticalScrollContainer.RegisterCallback<AttachToPanelEvent>(new EventCallback<AttachToPanelEvent>(this.OnAttachToPanel), TrickleDown.NoTrickleDown);
			this.m_ContentAndVerticalScrollContainer.RegisterCallback<DetachFromPanelEvent>(new EventCallback<DetachFromPanelEvent>(this.OnDetachFromPanel), TrickleDown.NoTrickleDown);
			this.m_ContentAndVerticalScrollContainer.Add(this.contentViewport);
			this.m_ContentContainer = new VisualElement
			{
				name = "unity-content-container"
			};
			this.m_ContentContainer.disableClipping = true;
			this.m_ContentContainer.RegisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.OnGeometryChanged), TrickleDown.NoTrickleDown);
			this.m_ContentContainer.AddToClassList(ScrollView.contentUssClassName);
			this.m_ContentContainer.usageHints = UsageHints.GroupTransform;
			this.contentViewport.Add(this.m_ContentContainer);
			this.SetScrollViewMode(scrollViewMode);
			this.horizontalScroller = new Scroller(0f, 2.1474836E+09f, delegate(float value)
			{
				this.scrollOffset = new Vector2(value, this.scrollOffset.y);
				this.UpdateContentViewTransform();
			}, SliderDirection.Horizontal)
			{
				viewDataKey = "HorizontalScroller"
			};
			this.horizontalScroller.AddToClassList(ScrollView.hScrollerUssClassName);
			this.horizontalScroller.style.display = DisplayStyle.None;
			base.hierarchy.Add(this.horizontalScroller);
			this.verticalScroller = new Scroller(0f, 2.1474836E+09f, delegate(float value)
			{
				this.scrollOffset = new Vector2(this.scrollOffset.x, value);
				this.UpdateContentViewTransform();
			}, SliderDirection.Vertical)
			{
				viewDataKey = "VerticalScroller"
			};
			this.horizontalScroller.slider.clampedDragger.draggingEnded += this.UpdateElasticBehaviour;
			this.verticalScroller.slider.clampedDragger.draggingEnded += this.UpdateElasticBehaviour;
			this.horizontalScroller.slider.clampedDragger.acceptClicksIfDisabled = true;
			this.verticalScroller.slider.clampedDragger.acceptClicksIfDisabled = true;
			this.verticalScroller.highButton.acceptClicksIfDisabled = true;
			this.verticalScroller.lowButton.acceptClicksIfDisabled = true;
			this.horizontalScroller.highButton.acceptClicksIfDisabled = true;
			this.horizontalScroller.lowButton.acceptClicksIfDisabled = true;
			this.horizontalScroller.lowButton.AddAction(new Action(this.UpdateElasticBehaviour));
			this.horizontalScroller.highButton.AddAction(new Action(this.UpdateElasticBehaviour));
			this.verticalScroller.lowButton.AddAction(new Action(this.UpdateElasticBehaviour));
			this.verticalScroller.highButton.AddAction(new Action(this.UpdateElasticBehaviour));
			this.verticalScroller.AddToClassList(ScrollView.vScrollerUssClassName);
			this.verticalScroller.style.display = DisplayStyle.None;
			this.m_ContentAndVerticalScrollContainer.Add(this.verticalScroller);
			this.touchScrollBehavior = ScrollView.TouchScrollBehavior.Clamped;
			base.RegisterCallback<WheelEvent>(new EventCallback<WheelEvent>(this.OnScrollWheel), InvokePolicy.IncludeDisabled, TrickleDown.NoTrickleDown);
			this.verticalScroller.RegisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.OnScrollersGeometryChanged), TrickleDown.NoTrickleDown);
			this.horizontalScroller.RegisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.OnScrollersGeometryChanged), TrickleDown.NoTrickleDown);
			this.horizontalPageSize = -1f;
			this.verticalPageSize = -1f;
			this.horizontalScroller.slider.dragElement.RegisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.OnHorizontalScrollDragElementChanged), TrickleDown.NoTrickleDown);
			this.verticalScroller.slider.dragElement.RegisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.OnVerticalScrollDragElementChanged), TrickleDown.NoTrickleDown);
			this.m_CapturedTargetPointerMoveCallback = new EventCallback<PointerMoveEvent>(this.OnPointerMove);
			this.m_CapturedTargetPointerUpCallback = new EventCallback<PointerUpEvent>(this.OnPointerUp);
			this.scrollOffset = Vector2.zero;
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x060009D4 RID: 2516 RVA: 0x00026053 File Offset: 0x00024253
		// (set) Token: 0x060009D5 RID: 2517 RVA: 0x0002605C File Offset: 0x0002425C
		public ScrollViewMode mode
		{
			get
			{
				return this.m_Mode;
			}
			set
			{
				bool flag = this.m_Mode == value;
				if (!flag)
				{
					this.SetScrollViewMode(value);
				}
			}
		}

		// Token: 0x060009D6 RID: 2518 RVA: 0x00026084 File Offset: 0x00024284
		private void SetScrollViewMode(ScrollViewMode mode)
		{
			this.m_Mode = mode;
			base.RemoveFromClassList(ScrollView.verticalVariantUssClassName);
			base.RemoveFromClassList(ScrollView.horizontalVariantUssClassName);
			base.RemoveFromClassList(ScrollView.verticalHorizontalVariantUssClassName);
			base.RemoveFromClassList(ScrollView.scrollVariantUssClassName);
			this.contentContainer.RemoveFromClassList(ScrollView.verticalVariantContentUssClassName);
			this.contentContainer.RemoveFromClassList(ScrollView.horizontalVariantContentUssClassName);
			this.contentContainer.RemoveFromClassList(ScrollView.verticalHorizontalVariantContentUssClassName);
			this.contentViewport.RemoveFromClassList(ScrollView.verticalVariantViewportUssClassName);
			this.contentViewport.RemoveFromClassList(ScrollView.horizontalVariantViewportUssClassName);
			this.contentViewport.RemoveFromClassList(ScrollView.verticalHorizontalVariantViewportUssClassName);
			switch (mode)
			{
			case ScrollViewMode.Vertical:
				base.AddToClassList(ScrollView.scrollVariantUssClassName);
				base.AddToClassList(ScrollView.verticalVariantUssClassName);
				this.contentViewport.AddToClassList(ScrollView.verticalVariantViewportUssClassName);
				this.contentContainer.AddToClassList(ScrollView.verticalVariantContentUssClassName);
				break;
			case ScrollViewMode.Horizontal:
				base.AddToClassList(ScrollView.scrollVariantUssClassName);
				base.AddToClassList(ScrollView.horizontalVariantUssClassName);
				this.contentViewport.AddToClassList(ScrollView.horizontalVariantViewportUssClassName);
				this.contentContainer.AddToClassList(ScrollView.horizontalVariantContentUssClassName);
				break;
			case ScrollViewMode.VerticalAndHorizontal:
				base.AddToClassList(ScrollView.scrollVariantUssClassName);
				base.AddToClassList(ScrollView.verticalHorizontalVariantUssClassName);
				this.contentViewport.AddToClassList(ScrollView.verticalHorizontalVariantViewportUssClassName);
				this.contentContainer.AddToClassList(ScrollView.verticalHorizontalVariantContentUssClassName);
				break;
			}
		}

		// Token: 0x060009D7 RID: 2519 RVA: 0x00026200 File Offset: 0x00024400
		private void OnAttachToPanel(AttachToPanelEvent evt)
		{
			bool flag = evt.destinationPanel == null;
			if (!flag)
			{
				this.m_AttachedRootVisualContainer = base.GetRootVisualContainer();
				VisualElement attachedRootVisualContainer = this.m_AttachedRootVisualContainer;
				if (attachedRootVisualContainer != null)
				{
					attachedRootVisualContainer.RegisterCallback<CustomStyleResolvedEvent>(new EventCallback<CustomStyleResolvedEvent>(this.OnRootCustomStyleResolved), TrickleDown.NoTrickleDown);
				}
				this.ReadSingleLineHeight();
				bool flag2 = evt.destinationPanel.contextType == ContextType.Player;
				if (flag2)
				{
					this.m_ContentAndVerticalScrollContainer.RegisterCallback<PointerMoveEvent>(new EventCallback<PointerMoveEvent>(this.OnPointerMove), TrickleDown.NoTrickleDown);
					this.contentContainer.RegisterCallback<PointerDownEvent>(new EventCallback<PointerDownEvent>(this.OnPointerDown), TrickleDown.TrickleDown);
					this.contentContainer.RegisterCallback<PointerCancelEvent>(new EventCallback<PointerCancelEvent>(this.OnPointerCancel), TrickleDown.NoTrickleDown);
					this.contentContainer.RegisterCallback<PointerUpEvent>(new EventCallback<PointerUpEvent>(this.OnPointerUp), TrickleDown.TrickleDown);
					this.contentContainer.RegisterCallback<PointerCaptureEvent>(new EventCallback<PointerCaptureEvent>(this.OnPointerCapture), TrickleDown.NoTrickleDown);
					this.contentContainer.RegisterCallback<PointerCaptureOutEvent>(new EventCallback<PointerCaptureOutEvent>(this.OnPointerCaptureOut), TrickleDown.NoTrickleDown);
					evt.destinationPanel.visualTree.RegisterCallback<PointerUpEvent>(new EventCallback<PointerUpEvent>(this.OnRootPointerUp), TrickleDown.TrickleDown);
				}
			}
		}

		// Token: 0x060009D8 RID: 2520 RVA: 0x00026320 File Offset: 0x00024520
		private void OnDetachFromPanel(DetachFromPanelEvent evt)
		{
			IVisualElementScheduledItem scheduledLayoutPassResetItem = this.m_ScheduledLayoutPassResetItem;
			if (scheduledLayoutPassResetItem != null)
			{
				scheduledLayoutPassResetItem.Pause();
			}
			this.ResetLayoutPass();
			bool flag = evt.originPanel == null;
			if (!flag)
			{
				VisualElement attachedRootVisualContainer = this.m_AttachedRootVisualContainer;
				if (attachedRootVisualContainer != null)
				{
					attachedRootVisualContainer.UnregisterCallback<CustomStyleResolvedEvent>(new EventCallback<CustomStyleResolvedEvent>(this.OnRootCustomStyleResolved), TrickleDown.NoTrickleDown);
				}
				this.m_AttachedRootVisualContainer = null;
				bool flag2 = evt.originPanel.contextType == ContextType.Player;
				if (flag2)
				{
					this.m_ContentAndVerticalScrollContainer.UnregisterCallback<PointerMoveEvent>(new EventCallback<PointerMoveEvent>(this.OnPointerMove), TrickleDown.NoTrickleDown);
					this.contentContainer.UnregisterCallback<PointerDownEvent>(new EventCallback<PointerDownEvent>(this.OnPointerDown), TrickleDown.TrickleDown);
					this.contentContainer.UnregisterCallback<PointerCancelEvent>(new EventCallback<PointerCancelEvent>(this.OnPointerCancel), TrickleDown.NoTrickleDown);
					this.contentContainer.UnregisterCallback<PointerUpEvent>(new EventCallback<PointerUpEvent>(this.OnPointerUp), TrickleDown.TrickleDown);
					this.contentContainer.UnregisterCallback<PointerCaptureEvent>(new EventCallback<PointerCaptureEvent>(this.OnPointerCapture), TrickleDown.NoTrickleDown);
					this.contentContainer.UnregisterCallback<PointerCaptureOutEvent>(new EventCallback<PointerCaptureOutEvent>(this.OnPointerCaptureOut), TrickleDown.NoTrickleDown);
					evt.originPanel.visualTree.UnregisterCallback<PointerUpEvent>(new EventCallback<PointerUpEvent>(this.OnRootPointerUp), TrickleDown.TrickleDown);
				}
			}
		}

		// Token: 0x060009D9 RID: 2521 RVA: 0x0002644C File Offset: 0x0002464C
		private void OnPointerCapture(PointerCaptureEvent evt)
		{
			this.m_CapturedTarget = evt.target as VisualElement;
			bool flag = this.m_CapturedTarget == null;
			if (!flag)
			{
				this.m_TouchPointerMoveAllowed = true;
				this.m_CapturedTarget.RegisterCallback<PointerMoveEvent>(this.m_CapturedTargetPointerMoveCallback, TrickleDown.NoTrickleDown);
				this.m_CapturedTarget.RegisterCallback<PointerUpEvent>(this.m_CapturedTargetPointerUpCallback, TrickleDown.NoTrickleDown);
			}
		}

		// Token: 0x060009DA RID: 2522 RVA: 0x000264A8 File Offset: 0x000246A8
		private void OnPointerCaptureOut(PointerCaptureOutEvent evt)
		{
			this.ReleaseScrolling(evt.pointerId, evt.target);
			bool flag = this.m_CapturedTarget == null;
			if (!flag)
			{
				this.m_CapturedTarget.UnregisterCallback<PointerMoveEvent>(this.m_CapturedTargetPointerMoveCallback, TrickleDown.NoTrickleDown);
				this.m_CapturedTarget.UnregisterCallback<PointerUpEvent>(this.m_CapturedTargetPointerUpCallback, TrickleDown.NoTrickleDown);
				this.m_CapturedTarget = null;
			}
		}

		// Token: 0x060009DB RID: 2523 RVA: 0x00026508 File Offset: 0x00024708
		private void OnGeometryChanged(GeometryChangedEvent evt)
		{
			bool flag = evt.oldRect.size == evt.newRect.size;
			if (!flag)
			{
				bool flag2 = this.needsVertical;
				bool flag3 = this.needsHorizontal;
				bool flag4 = this.m_FirstLayoutPass == -1;
				if (flag4)
				{
					this.m_FirstLayoutPass = evt.layoutPass;
				}
				else
				{
					bool flag5 = evt.layoutPass - this.m_FirstLayoutPass > 5;
					if (flag5)
					{
						flag2 = flag2 || this.isVerticalScrollDisplayed;
						flag3 = flag3 || this.isHorizontalScrollDisplayed;
					}
				}
				this.UpdateScrollers(flag3, flag2);
				this.UpdateContentViewTransform();
				this.ScheduleResetLayoutPass();
			}
		}

		// Token: 0x060009DC RID: 2524 RVA: 0x000265B4 File Offset: 0x000247B4
		private void ScheduleResetLayoutPass()
		{
			bool flag = this.m_ScheduledLayoutPassResetItem == null;
			if (flag)
			{
				this.m_ScheduledLayoutPassResetItem = base.schedule.Execute(new Action(this.ResetLayoutPass));
			}
			else
			{
				this.m_ScheduledLayoutPassResetItem.Pause();
				this.m_ScheduledLayoutPassResetItem.Resume();
			}
		}

		// Token: 0x060009DD RID: 2525 RVA: 0x0002660A File Offset: 0x0002480A
		private void ResetLayoutPass()
		{
			this.m_FirstLayoutPass = -1;
		}

		// Token: 0x060009DE RID: 2526 RVA: 0x00026614 File Offset: 0x00024814
		private static float ComputeElasticOffset(float deltaPointer, float initialScrollOffset, float lowLimit, float hardLowLimit, float highLimit, float hardHighLimit)
		{
			initialScrollOffset = Mathf.Max(initialScrollOffset, hardLowLimit * 0.95f);
			initialScrollOffset = Mathf.Min(initialScrollOffset, hardHighLimit * 0.95f);
			bool flag = initialScrollOffset < lowLimit && hardLowLimit < lowLimit;
			float num;
			float num3;
			if (flag)
			{
				num = lowLimit - hardLowLimit;
				float num2 = (lowLimit - initialScrollOffset) / num;
				num3 = num2 * num / (1f - num2);
				num3 += deltaPointer;
				initialScrollOffset = lowLimit;
			}
			else
			{
				bool flag2 = initialScrollOffset > highLimit && hardHighLimit > highLimit;
				if (flag2)
				{
					num = hardHighLimit - highLimit;
					float num4 = (initialScrollOffset - highLimit) / num;
					num3 = -1f * num4 * num / (1f - num4);
					num3 += deltaPointer;
					initialScrollOffset = highLimit;
				}
				else
				{
					num3 = deltaPointer;
				}
			}
			float num5 = initialScrollOffset - num3;
			bool flag3 = num5 < lowLimit;
			float num6;
			if (flag3)
			{
				num3 = lowLimit - num5;
				initialScrollOffset = lowLimit;
				num = lowLimit - hardLowLimit;
				num6 = 1f;
			}
			else
			{
				bool flag4 = num5 <= highLimit;
				if (flag4)
				{
					return num5;
				}
				num3 = num5 - highLimit;
				initialScrollOffset = highLimit;
				num = hardHighLimit - highLimit;
				num6 = -1f;
			}
			bool flag5 = Mathf.Abs(num3) < 1E-30f;
			float num7;
			if (flag5)
			{
				num7 = initialScrollOffset;
			}
			else
			{
				float num8 = num3 / (num3 + num);
				num8 *= num;
				num8 *= num6;
				num5 = initialScrollOffset - num8;
				num7 = num5;
			}
			return num7;
		}

		// Token: 0x060009DF RID: 2527 RVA: 0x00026744 File Offset: 0x00024944
		private void ComputeInitialSpringBackVelocity()
		{
			bool flag = this.touchScrollBehavior != ScrollView.TouchScrollBehavior.Elastic;
			if (flag)
			{
				this.m_SpringBackVelocity = Vector2.zero;
			}
			else
			{
				bool flag2 = this.scrollOffset.x < this.m_LowBounds.x;
				if (flag2)
				{
					this.m_SpringBackVelocity.x = this.m_LowBounds.x - this.scrollOffset.x;
				}
				else
				{
					bool flag3 = this.scrollOffset.x > this.m_HighBounds.x;
					if (flag3)
					{
						this.m_SpringBackVelocity.x = this.m_HighBounds.x - this.scrollOffset.x;
					}
					else
					{
						this.m_SpringBackVelocity.x = 0f;
					}
				}
				bool flag4 = this.scrollOffset.y < this.m_LowBounds.y;
				if (flag4)
				{
					this.m_SpringBackVelocity.y = this.m_LowBounds.y - this.scrollOffset.y;
				}
				else
				{
					bool flag5 = this.scrollOffset.y > this.m_HighBounds.y;
					if (flag5)
					{
						this.m_SpringBackVelocity.y = this.m_HighBounds.y - this.scrollOffset.y;
					}
					else
					{
						this.m_SpringBackVelocity.y = 0f;
					}
				}
			}
		}

		// Token: 0x060009E0 RID: 2528 RVA: 0x000268A4 File Offset: 0x00024AA4
		private void SpringBack()
		{
			bool flag = this.touchScrollBehavior != ScrollView.TouchScrollBehavior.Elastic;
			if (flag)
			{
				this.m_SpringBackVelocity = Vector2.zero;
			}
			else
			{
				Vector2 scrollOffset = this.scrollOffset;
				bool flag2 = scrollOffset.x < this.m_LowBounds.x;
				if (flag2)
				{
					scrollOffset.x = Mathf.SmoothDamp(scrollOffset.x, this.m_LowBounds.x, ref this.m_SpringBackVelocity.x, this.elasticity, float.PositiveInfinity, this.elapsedTimeSinceLastHorizontalTouchScroll);
					bool flag3 = Mathf.Abs(this.m_SpringBackVelocity.x) < base.scaledPixelsPerPoint;
					if (flag3)
					{
						this.m_SpringBackVelocity.x = 0f;
					}
				}
				else
				{
					bool flag4 = scrollOffset.x > this.m_HighBounds.x;
					if (flag4)
					{
						scrollOffset.x = Mathf.SmoothDamp(scrollOffset.x, this.m_HighBounds.x, ref this.m_SpringBackVelocity.x, this.elasticity, float.PositiveInfinity, this.elapsedTimeSinceLastHorizontalTouchScroll);
						bool flag5 = Mathf.Abs(this.m_SpringBackVelocity.x) < base.scaledPixelsPerPoint;
						if (flag5)
						{
							this.m_SpringBackVelocity.x = 0f;
						}
					}
					else
					{
						this.m_SpringBackVelocity.x = 0f;
					}
				}
				bool flag6 = scrollOffset.y < this.m_LowBounds.y;
				if (flag6)
				{
					scrollOffset.y = Mathf.SmoothDamp(scrollOffset.y, this.m_LowBounds.y, ref this.m_SpringBackVelocity.y, this.elasticity, float.PositiveInfinity, this.elapsedTimeSinceLastVerticalTouchScroll);
					bool flag7 = Mathf.Abs(this.m_SpringBackVelocity.y) < base.scaledPixelsPerPoint;
					if (flag7)
					{
						this.m_SpringBackVelocity.y = 0f;
					}
				}
				else
				{
					bool flag8 = scrollOffset.y > this.m_HighBounds.y;
					if (flag8)
					{
						scrollOffset.y = Mathf.SmoothDamp(scrollOffset.y, this.m_HighBounds.y, ref this.m_SpringBackVelocity.y, this.elasticity, float.PositiveInfinity, this.elapsedTimeSinceLastVerticalTouchScroll);
						bool flag9 = Mathf.Abs(this.m_SpringBackVelocity.y) < base.scaledPixelsPerPoint;
						if (flag9)
						{
							this.m_SpringBackVelocity.y = 0f;
						}
					}
					else
					{
						this.m_SpringBackVelocity.y = 0f;
					}
				}
				this.scrollOffset = scrollOffset;
			}
		}

		// Token: 0x060009E1 RID: 2529 RVA: 0x00026B20 File Offset: 0x00024D20
		internal void ApplyScrollInertia()
		{
			bool flag = this.hasInertia && this.m_Velocity != Vector2.zero;
			if (flag)
			{
				Vector2 vector = Vector2.zero;
				float num = 0f;
				while (num < this.elapsedTimeSinceLastVerticalTouchScroll)
				{
					this.m_Velocity *= Mathf.Pow(this.scrollDecelerationRate, this.k_TouchScrollInertiaBaseTimeInterval);
					num += this.k_TouchScrollInertiaBaseTimeInterval;
					vector += this.m_Velocity * this.k_TouchScrollInertiaBaseTimeInterval;
				}
				float num2 = this.elapsedTimeSinceLastVerticalTouchScroll - num;
				bool flag2 = num2 > 0f && num2 < this.k_TouchScrollInertiaBaseTimeInterval;
				if (flag2)
				{
					this.m_Velocity *= Mathf.Pow(this.scrollDecelerationRate, num2);
					vector += this.m_Velocity * num2;
				}
				float num3 = base.scaledPixelsPerPoint * this.k_ScaledPixelsPerPointMultiplier;
				bool flag3 = Mathf.Abs(this.m_Velocity.x) <= num3 || (this.touchScrollBehavior == ScrollView.TouchScrollBehavior.Elastic && (this.scrollOffset.x < this.m_LowBounds.x || this.scrollOffset.x > this.m_HighBounds.x));
				if (flag3)
				{
					this.m_Velocity.x = 0f;
				}
				bool flag4 = Mathf.Abs(this.m_Velocity.y) <= num3 || (this.touchScrollBehavior == ScrollView.TouchScrollBehavior.Elastic && (this.scrollOffset.y < this.m_LowBounds.y || this.scrollOffset.y > this.m_HighBounds.y));
				if (flag4)
				{
					this.m_Velocity.y = 0f;
				}
				this.scrollOffset += vector;
			}
			else
			{
				this.m_Velocity = Vector2.zero;
			}
		}

		// Token: 0x060009E2 RID: 2530 RVA: 0x00026D18 File Offset: 0x00024F18
		private void PostPointerUpAnimation()
		{
			this.elapsedTimeSinceLastVerticalTouchScroll = Time.unscaledTime - this.previousVerticalTouchScrollTimeStamp;
			this.previousVerticalTouchScrollTimeStamp = Time.unscaledTime;
			this.elapsedTimeSinceLastHorizontalTouchScroll = Time.unscaledTime - this.previousHorizontalTouchScrollTimeStamp;
			this.previousHorizontalTouchScrollTimeStamp = Time.unscaledTime;
			this.ApplyScrollInertia();
			this.SpringBack();
			bool flag = this.m_SpringBackVelocity == Vector2.zero && this.m_Velocity == Vector2.zero;
			if (flag)
			{
				this.m_PostPointerUpAnimation.Pause();
				this.elapsedTimeSinceLastVerticalTouchScroll = 0f;
				this.elapsedTimeSinceLastHorizontalTouchScroll = 0f;
				this.previousVerticalTouchScrollTimeStamp = 0f;
				this.previousHorizontalTouchScrollTimeStamp = 0f;
			}
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x00026DD4 File Offset: 0x00024FD4
		private void OnPointerDown(PointerDownEvent evt)
		{
			bool flag = evt.pointerType == PointerType.mouse || !evt.isPrimary;
			if (!flag)
			{
				bool flag2 = evt.pointerId != PointerId.invalidPointerId;
				if (flag2)
				{
					this.ReleaseScrolling(evt.pointerId, evt.target);
				}
				IVisualElementScheduledItem postPointerUpAnimation = this.m_PostPointerUpAnimation;
				if (postPointerUpAnimation != null)
				{
					postPointerUpAnimation.Pause();
				}
				bool flag3 = Mathf.Abs(this.m_Velocity.x) > 10f || Mathf.Abs(this.m_Velocity.y) > 10f;
				this.m_TouchPointerMoveAllowed = true;
				this.m_StartedMoving = false;
				this.InitTouchScrolling(evt.position);
				bool flag4 = flag3;
				if (flag4)
				{
					this.contentContainer.CapturePointer(evt.pointerId);
					this.contentContainer.panel.PreventCompatibilityMouseEvents(evt.pointerId);
					evt.StopPropagation();
					this.m_TouchStoppedVelocity = true;
				}
			}
		}

		// Token: 0x060009E4 RID: 2532 RVA: 0x00026ED4 File Offset: 0x000250D4
		private void OnPointerMove(PointerMoveEvent evt)
		{
			bool flag = evt.pointerType == PointerType.mouse || !evt.isPrimary || !this.m_TouchPointerMoveAllowed;
			if (!flag)
			{
				bool isHandledByDraggable = evt.isHandledByDraggable;
				if (isHandledByDraggable)
				{
					this.m_PointerStartPosition = evt.position;
					this.m_StartPosition = this.scrollOffset;
				}
				else
				{
					Vector2 vector = evt.position;
					Vector2 vector2 = vector - this.m_PointerStartPosition;
					bool flag2 = this.mode == ScrollViewMode.Horizontal;
					if (flag2)
					{
						vector2.y = 0f;
					}
					else
					{
						bool flag3 = this.mode == ScrollViewMode.Vertical;
						if (flag3)
						{
							vector2.x = 0f;
						}
					}
					bool flag4 = !this.m_TouchStoppedVelocity && !this.m_StartedMoving && vector2.sqrMagnitude < 100f;
					if (!flag4)
					{
						ScrollView.TouchScrollingResult touchScrollingResult = this.ComputeTouchScrolling(evt.position);
						bool flag5 = touchScrollingResult != ScrollView.TouchScrollingResult.Forward;
						if (flag5)
						{
							evt.isHandledByDraggable = true;
							evt.StopPropagation();
							bool flag6 = !this.contentContainer.HasPointerCapture(evt.pointerId);
							if (flag6)
							{
								this.contentContainer.CapturePointer(evt.pointerId);
							}
						}
						else
						{
							this.m_Velocity = Vector2.zero;
						}
					}
				}
			}
		}

		// Token: 0x060009E5 RID: 2533 RVA: 0x00027025 File Offset: 0x00025225
		private void OnPointerCancel(PointerCancelEvent evt)
		{
			this.ReleaseScrolling(evt.pointerId, evt.target);
		}

		// Token: 0x060009E6 RID: 2534 RVA: 0x0002703C File Offset: 0x0002523C
		private void OnPointerUp(PointerUpEvent evt)
		{
			bool flag = this.ReleaseScrolling(evt.pointerId, evt.target);
			if (flag)
			{
				this.contentContainer.panel.PreventCompatibilityMouseEvents(evt.pointerId);
				evt.StopPropagation();
			}
		}

		// Token: 0x060009E7 RID: 2535 RVA: 0x00027080 File Offset: 0x00025280
		internal void InitTouchScrolling(Vector2 position)
		{
			this.m_PointerStartPosition = position;
			this.m_StartPosition = this.scrollOffset;
			this.m_Velocity = Vector2.zero;
			this.m_SpringBackVelocity = Vector2.zero;
			this.m_LowBounds = new Vector2(Mathf.Min(this.horizontalScroller.lowValue, this.horizontalScroller.highValue), Mathf.Min(this.verticalScroller.lowValue, this.verticalScroller.highValue));
			this.m_HighBounds = new Vector2(Mathf.Max(this.horizontalScroller.lowValue, this.horizontalScroller.highValue), Mathf.Max(this.verticalScroller.lowValue, this.verticalScroller.highValue));
		}

		// Token: 0x060009E8 RID: 2536 RVA: 0x0002713C File Offset: 0x0002533C
		internal ScrollView.TouchScrollingResult ComputeTouchScrolling(Vector2 position)
		{
			bool flag = this.touchScrollBehavior == ScrollView.TouchScrollBehavior.Clamped;
			Vector2 vector;
			if (flag)
			{
				vector = this.m_StartPosition - (position - this.m_PointerStartPosition);
				vector = Vector2.Max(vector, this.m_LowBounds);
				vector = Vector2.Min(vector, this.m_HighBounds);
			}
			else
			{
				bool flag2 = this.touchScrollBehavior == ScrollView.TouchScrollBehavior.Elastic;
				if (flag2)
				{
					Vector2 vector2 = position - this.m_PointerStartPosition;
					vector.x = ScrollView.ComputeElasticOffset(vector2.x, this.m_StartPosition.x, this.m_LowBounds.x, this.m_LowBounds.x - this.contentViewport.resolvedStyle.width, this.m_HighBounds.x, this.m_HighBounds.x + this.contentViewport.resolvedStyle.width);
					vector.y = ScrollView.ComputeElasticOffset(vector2.y, this.m_StartPosition.y, this.m_LowBounds.y, this.m_LowBounds.y - this.contentViewport.resolvedStyle.height, this.m_HighBounds.y, this.m_HighBounds.y + this.contentViewport.resolvedStyle.height);
					this.previousVerticalTouchScrollTimeStamp = Time.unscaledTime;
					this.previousHorizontalTouchScrollTimeStamp = Time.unscaledTime;
				}
				else
				{
					vector = this.m_StartPosition - (position - this.m_PointerStartPosition);
				}
			}
			bool flag3 = this.mode == ScrollViewMode.Vertical;
			if (flag3)
			{
				vector.x = this.m_LowBounds.x;
			}
			else
			{
				bool flag4 = this.mode == ScrollViewMode.Horizontal;
				if (flag4)
				{
					vector.y = this.m_LowBounds.y;
				}
			}
			bool flag5 = this.scrollOffset != vector;
			bool flag6 = flag5;
			ScrollView.TouchScrollingResult touchScrollingResult;
			if (flag6)
			{
				touchScrollingResult = (this.ApplyTouchScrolling(vector) ? ScrollView.TouchScrollingResult.Apply : ScrollView.TouchScrollingResult.Forward);
			}
			else
			{
				touchScrollingResult = ((this.m_StartedMoving && this.nestedInteractionKind != ScrollView.NestedInteractionKind.ForwardScrolling) ? ScrollView.TouchScrollingResult.Block : ScrollView.TouchScrollingResult.Forward);
			}
			return touchScrollingResult;
		}

		// Token: 0x060009E9 RID: 2537 RVA: 0x0002734C File Offset: 0x0002554C
		private bool ApplyTouchScrolling(Vector2 newScrollOffset)
		{
			this.m_StartedMoving = true;
			bool hasInertia = this.hasInertia;
			if (hasInertia)
			{
				bool flag = newScrollOffset == this.m_LowBounds || newScrollOffset == this.m_HighBounds;
				if (flag)
				{
					this.m_Velocity = Vector2.zero;
					this.scrollOffset = newScrollOffset;
					return false;
				}
				bool flag2 = this.m_LastVelocityLerpTime > 0f;
				if (flag2)
				{
					float num = Time.unscaledTime - this.m_LastVelocityLerpTime;
					this.m_Velocity = Vector2.Lerp(this.m_Velocity, Vector2.zero, num * 10f);
				}
				this.m_LastVelocityLerpTime = Time.unscaledTime;
				float num2 = this.k_TouchScrollInertiaBaseTimeInterval;
				Vector2 vector = (newScrollOffset - this.scrollOffset) / num2;
				this.m_Velocity = Vector2.Lerp(this.m_Velocity, vector, num2 * 10f);
			}
			bool flag3 = this.scrollOffset != newScrollOffset;
			this.scrollOffset = newScrollOffset;
			return flag3;
		}

		// Token: 0x060009EA RID: 2538 RVA: 0x0002744C File Offset: 0x0002564C
		private bool ReleaseScrolling(int pointerId, IEventHandler target)
		{
			this.m_TouchStoppedVelocity = false;
			this.m_StartedMoving = false;
			this.m_TouchPointerMoveAllowed = false;
			bool flag = target != this.contentContainer || !this.contentContainer.HasPointerCapture(pointerId);
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				this.previousVerticalTouchScrollTimeStamp = Time.unscaledTime;
				this.previousHorizontalTouchScrollTimeStamp = Time.unscaledTime;
				bool flag3 = this.touchScrollBehavior == ScrollView.TouchScrollBehavior.Elastic || this.hasInertia;
				if (flag3)
				{
					this.ExecuteElasticSpringAnimation();
				}
				this.contentContainer.ReleasePointer(pointerId);
				flag2 = true;
			}
			return flag2;
		}

		// Token: 0x060009EB RID: 2539 RVA: 0x000274DC File Offset: 0x000256DC
		private void ExecuteElasticSpringAnimation()
		{
			this.ComputeInitialSpringBackVelocity();
			bool flag = this.m_PostPointerUpAnimation == null;
			if (flag)
			{
				this.m_PostPointerUpAnimation = base.schedule.Execute(new Action(this.PostPointerUpAnimation)).Every(this.m_ElasticAnimationIntervalMs);
			}
			else
			{
				this.m_PostPointerUpAnimation.Pause();
				this.m_PostPointerUpAnimation.Resume();
			}
		}

		// Token: 0x060009EC RID: 2540 RVA: 0x00027544 File Offset: 0x00025744
		private void AdjustScrollers()
		{
			float num = ((this.contentContainer.boundingBox.width > 1E-30f) ? (this.contentViewport.layout.width / this.contentContainer.boundingBox.width) : 1f);
			float num2 = ((this.contentContainer.boundingBox.height > 1E-30f) ? (this.contentViewport.layout.height / this.contentContainer.boundingBox.height) : 1f);
			this.horizontalScroller.Adjust(num);
			this.verticalScroller.Adjust(num2);
		}

		// Token: 0x060009ED RID: 2541 RVA: 0x00027600 File Offset: 0x00025800
		internal void UpdateScrollers(bool displayHorizontal, bool displayVertical)
		{
			this.AdjustScrollers();
			this.horizontalScroller.SetEnabled(this.contentContainer.boundingBox.width - this.contentViewport.layout.width > 0f);
			this.verticalScroller.SetEnabled(this.contentContainer.boundingBox.height - this.contentViewport.layout.height > 0f);
			bool flag = displayHorizontal && this.m_HorizontalScrollerVisibility != ScrollerVisibility.Hidden;
			bool flag2 = displayVertical && this.m_VerticalScrollerVisibility != ScrollerVisibility.Hidden;
			DisplayStyle displayStyle = (flag ? DisplayStyle.Flex : DisplayStyle.None);
			DisplayStyle displayStyle2 = (flag2 ? DisplayStyle.Flex : DisplayStyle.None);
			bool flag3 = displayStyle != this.horizontalScroller.style.display;
			if (flag3)
			{
				this.horizontalScroller.style.display = displayStyle;
			}
			bool flag4 = displayStyle2 != this.verticalScroller.style.display;
			if (flag4)
			{
				this.verticalScroller.style.display = displayStyle2;
			}
			this.verticalScroller.lowValue = 0f;
			this.verticalScroller.highValue = this.scrollableHeight;
			this.horizontalScroller.lowValue = 0f;
			this.horizontalScroller.highValue = this.scrollableWidth;
		}

		// Token: 0x060009EE RID: 2542 RVA: 0x00027780 File Offset: 0x00025980
		private void OnScrollersGeometryChanged(GeometryChangedEvent evt)
		{
			bool flag = evt.oldRect.size == evt.newRect.size;
			if (!flag)
			{
				bool flag2 = this.needsHorizontal && this.m_HorizontalScrollerVisibility != ScrollerVisibility.Hidden;
				bool flag3 = flag2;
				if (flag3)
				{
					this.horizontalScroller.style.marginRight = this.verticalScroller.layout.width;
				}
				this.AdjustScrollers();
			}
		}

		// Token: 0x060009EF RID: 2543 RVA: 0x00027808 File Offset: 0x00025A08
		private void OnScrollWheel(WheelEvent evt)
		{
			bool flag = false;
			bool flag2 = this.mode != ScrollViewMode.Horizontal && this.contentContainer.boundingBox.height - base.layout.height > 0f;
			bool flag3 = this.mode != ScrollViewMode.Vertical && this.contentContainer.boundingBox.width - base.layout.width > 0f;
			float num = ((flag3 && !flag2) ? evt.delta.y : evt.delta.x);
			float num2 = (this.m_MouseWheelScrollSizeIsInline ? this.mouseWheelScrollSize : this.m_SingleLineHeight);
			bool flag4 = flag2;
			if (flag4)
			{
				float value = this.verticalScroller.value;
				this.verticalScroller.value += evt.delta.y * ((this.verticalScroller.lowValue < this.verticalScroller.highValue) ? 1f : (-1f)) * num2;
				bool flag5 = this.nestedInteractionKind == ScrollView.NestedInteractionKind.StopScrolling || !Mathf.Approximately(this.verticalScroller.value, value);
				if (flag5)
				{
					evt.StopPropagation();
					flag = true;
				}
			}
			bool flag6 = flag3;
			if (flag6)
			{
				float value2 = this.horizontalScroller.value;
				this.horizontalScroller.value += num * ((this.horizontalScroller.lowValue < this.horizontalScroller.highValue) ? 1f : (-1f)) * num2;
				bool flag7 = this.nestedInteractionKind == ScrollView.NestedInteractionKind.StopScrolling || !Mathf.Approximately(this.horizontalScroller.value, value2);
				if (flag7)
				{
					evt.StopPropagation();
					flag = true;
				}
			}
			bool flag8 = flag;
			if (flag8)
			{
				this.UpdateElasticBehaviour();
				this.UpdateContentViewTransform();
			}
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x000279EA File Offset: 0x00025BEA
		private void OnRootCustomStyleResolved(CustomStyleResolvedEvent evt)
		{
			this.ReadSingleLineHeight();
		}

		// Token: 0x060009F1 RID: 2545 RVA: 0x000279F4 File Offset: 0x00025BF4
		private void OnRootPointerUp(PointerUpEvent evt)
		{
			this.m_TouchPointerMoveAllowed = false;
		}

		// Token: 0x060009F2 RID: 2546 RVA: 0x00027A00 File Offset: 0x00025C00
		private void ReadSingleLineHeight()
		{
			VisualElement attachedRootVisualContainer = this.m_AttachedRootVisualContainer;
			StylePropertyValue stylePropertyValue;
			bool flag = ((attachedRootVisualContainer != null) ? attachedRootVisualContainer.computedStyle.customProperties : null) != null && this.m_AttachedRootVisualContainer.computedStyle.customProperties.TryGetValue("--unity-metrics-single_line-height", out stylePropertyValue);
			if (flag)
			{
				Dimension dimension;
				bool flag2 = stylePropertyValue.sheet.TryReadDimension(stylePropertyValue.handle, out dimension);
				if (flag2)
				{
					this.m_SingleLineHeight = dimension.value;
				}
			}
			else
			{
				this.m_SingleLineHeight = UIElementsUtility.singleLineHeight;
			}
		}

		// Token: 0x060009F3 RID: 2547 RVA: 0x00027A80 File Offset: 0x00025C80
		private void UpdateElasticBehaviour()
		{
			bool flag = this.touchScrollBehavior == ScrollView.TouchScrollBehavior.Elastic;
			if (flag)
			{
				this.m_LowBounds = new Vector2(Mathf.Min(this.horizontalScroller.lowValue, this.horizontalScroller.highValue), Mathf.Min(this.verticalScroller.lowValue, this.verticalScroller.highValue));
				this.m_HighBounds = new Vector2(Mathf.Max(this.horizontalScroller.lowValue, this.horizontalScroller.highValue), Mathf.Max(this.verticalScroller.lowValue, this.verticalScroller.highValue));
				this.ExecuteElasticSpringAnimation();
			}
		}

		// Token: 0x04000468 RID: 1128
		private const int k_MaxLocalLayoutPassCount = 5;

		// Token: 0x04000469 RID: 1129
		private int m_FirstLayoutPass = -1;

		// Token: 0x0400046A RID: 1130
		private ScrollerVisibility m_HorizontalScrollerVisibility;

		// Token: 0x0400046B RID: 1131
		private ScrollerVisibility m_VerticalScrollerVisibility;

		// Token: 0x0400046C RID: 1132
		private const float k_SizeThreshold = 0.001f;

		// Token: 0x0400046D RID: 1133
		private VisualElement m_AttachedRootVisualContainer;

		// Token: 0x0400046E RID: 1134
		private float m_SingleLineHeight = UIElementsUtility.singleLineHeight;

		// Token: 0x0400046F RID: 1135
		private const string k_SingleLineHeightPropertyName = "--unity-metrics-single_line-height";

		// Token: 0x04000470 RID: 1136
		private const float k_ScrollPageOverlapFactor = 0.1f;

		// Token: 0x04000471 RID: 1137
		internal const float k_UnsetPageSizeValue = -1f;

		// Token: 0x04000472 RID: 1138
		internal const float k_MouseWheelScrollSizeDefaultValue = 18f;

		// Token: 0x04000473 RID: 1139
		internal const float k_MouseWheelScrollSizeUnset = -1f;

		// Token: 0x04000474 RID: 1140
		internal bool m_MouseWheelScrollSizeIsInline;

		// Token: 0x04000475 RID: 1141
		private float m_HorizontalPageSize;

		// Token: 0x04000476 RID: 1142
		private float m_VerticalPageSize;

		// Token: 0x04000477 RID: 1143
		private float m_MouseWheelScrollSize = 18f;

		// Token: 0x04000478 RID: 1144
		private static readonly float k_DefaultScrollDecelerationRate = 0.135f;

		// Token: 0x04000479 RID: 1145
		private float m_ScrollDecelerationRate = ScrollView.k_DefaultScrollDecelerationRate;

		// Token: 0x0400047A RID: 1146
		private float k_ScaledPixelsPerPointMultiplier = 10f;

		// Token: 0x0400047B RID: 1147
		private float k_TouchScrollInertiaBaseTimeInterval = 0.004167f;

		// Token: 0x0400047C RID: 1148
		private static readonly float k_DefaultElasticity = 0.1f;

		// Token: 0x0400047D RID: 1149
		private float m_Elasticity = ScrollView.k_DefaultElasticity;

		// Token: 0x0400047E RID: 1150
		private ScrollView.TouchScrollBehavior m_TouchScrollBehavior;

		// Token: 0x0400047F RID: 1151
		private ScrollView.NestedInteractionKind m_NestedInteractionKind;

		// Token: 0x04000480 RID: 1152
		private static readonly long k_DefaultElasticAnimationInterval = 16L;

		// Token: 0x04000481 RID: 1153
		private long m_ElasticAnimationIntervalMs = ScrollView.k_DefaultElasticAnimationInterval;

		// Token: 0x04000485 RID: 1157
		private VisualElement m_ContentContainer;

		// Token: 0x04000486 RID: 1158
		private VisualElement m_ContentAndVerticalScrollContainer;

		// Token: 0x04000487 RID: 1159
		private float previousVerticalTouchScrollTimeStamp = 0f;

		// Token: 0x04000488 RID: 1160
		private float previousHorizontalTouchScrollTimeStamp = 0f;

		// Token: 0x04000489 RID: 1161
		private float elapsedTimeSinceLastVerticalTouchScroll = 0f;

		// Token: 0x0400048A RID: 1162
		private float elapsedTimeSinceLastHorizontalTouchScroll = 0f;

		// Token: 0x0400048B RID: 1163
		public static readonly string ussClassName = "unity-scroll-view";

		// Token: 0x0400048C RID: 1164
		public static readonly string viewportUssClassName = ScrollView.ussClassName + "__content-viewport";

		// Token: 0x0400048D RID: 1165
		public static readonly string horizontalVariantViewportUssClassName = ScrollView.viewportUssClassName + "--horizontal";

		// Token: 0x0400048E RID: 1166
		public static readonly string verticalVariantViewportUssClassName = ScrollView.viewportUssClassName + "--vertical";

		// Token: 0x0400048F RID: 1167
		public static readonly string verticalHorizontalVariantViewportUssClassName = ScrollView.viewportUssClassName + "--vertical-horizontal";

		// Token: 0x04000490 RID: 1168
		public static readonly string contentAndVerticalScrollUssClassName = ScrollView.ussClassName + "__content-and-vertical-scroll-container";

		// Token: 0x04000491 RID: 1169
		public static readonly string contentUssClassName = ScrollView.ussClassName + "__content-container";

		// Token: 0x04000492 RID: 1170
		public static readonly string horizontalVariantContentUssClassName = ScrollView.contentUssClassName + "--horizontal";

		// Token: 0x04000493 RID: 1171
		public static readonly string verticalVariantContentUssClassName = ScrollView.contentUssClassName + "--vertical";

		// Token: 0x04000494 RID: 1172
		public static readonly string verticalHorizontalVariantContentUssClassName = ScrollView.contentUssClassName + "--vertical-horizontal";

		// Token: 0x04000495 RID: 1173
		public static readonly string hScrollerUssClassName = ScrollView.ussClassName + "__horizontal-scroller";

		// Token: 0x04000496 RID: 1174
		public static readonly string vScrollerUssClassName = ScrollView.ussClassName + "__vertical-scroller";

		// Token: 0x04000497 RID: 1175
		public static readonly string horizontalVariantUssClassName = ScrollView.ussClassName + "--horizontal";

		// Token: 0x04000498 RID: 1176
		public static readonly string verticalVariantUssClassName = ScrollView.ussClassName + "--vertical";

		// Token: 0x04000499 RID: 1177
		public static readonly string verticalHorizontalVariantUssClassName = ScrollView.ussClassName + "--vertical-horizontal";

		// Token: 0x0400049A RID: 1178
		public static readonly string scrollVariantUssClassName = ScrollView.ussClassName + "--scroll";

		// Token: 0x0400049B RID: 1179
		private ScrollViewMode m_Mode;

		// Token: 0x0400049C RID: 1180
		private IVisualElementScheduledItem m_ScheduledLayoutPassResetItem;

		// Token: 0x0400049D RID: 1181
		private const float k_VelocityLerpTimeFactor = 10f;

		// Token: 0x0400049E RID: 1182
		internal const float ScrollThresholdSquared = 100f;

		// Token: 0x0400049F RID: 1183
		private Vector2 m_StartPosition;

		// Token: 0x040004A0 RID: 1184
		private Vector2 m_PointerStartPosition;

		// Token: 0x040004A1 RID: 1185
		private Vector2 m_Velocity;

		// Token: 0x040004A2 RID: 1186
		private Vector2 m_SpringBackVelocity;

		// Token: 0x040004A3 RID: 1187
		private Vector2 m_LowBounds;

		// Token: 0x040004A4 RID: 1188
		private Vector2 m_HighBounds;

		// Token: 0x040004A5 RID: 1189
		private float m_LastVelocityLerpTime;

		// Token: 0x040004A6 RID: 1190
		private bool m_StartedMoving;

		// Token: 0x040004A7 RID: 1191
		private bool m_TouchPointerMoveAllowed;

		// Token: 0x040004A8 RID: 1192
		private bool m_TouchStoppedVelocity;

		// Token: 0x040004A9 RID: 1193
		private VisualElement m_CapturedTarget;

		// Token: 0x040004AA RID: 1194
		private EventCallback<PointerMoveEvent> m_CapturedTargetPointerMoveCallback;

		// Token: 0x040004AB RID: 1195
		private EventCallback<PointerUpEvent> m_CapturedTargetPointerUpCallback;

		// Token: 0x040004AC RID: 1196
		internal IVisualElementScheduledItem m_PostPointerUpAnimation;

		// Token: 0x02000125 RID: 293
		public new class UxmlFactory : UxmlFactory<ScrollView, ScrollView.UxmlTraits>
		{
		}

		// Token: 0x02000126 RID: 294
		public new class UxmlTraits : VisualElement.UxmlTraits
		{
			// Token: 0x060009F8 RID: 2552 RVA: 0x00027CD8 File Offset: 0x00025ED8
			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ve, bag, cc);
				ScrollView scrollView = (ScrollView)ve;
				scrollView.mode = this.m_ScrollViewMode.GetValueFromBag(bag, cc);
				ScrollerVisibility scrollerVisibility = ScrollerVisibility.Auto;
				bool flag = this.m_HorizontalScrollerVisibility.TryGetValueFromBag(bag, cc, ref scrollerVisibility);
				if (flag)
				{
					scrollView.horizontalScrollerVisibility = scrollerVisibility;
				}
				else
				{
					scrollView.showHorizontal = this.m_ShowHorizontal.GetValueFromBag(bag, cc);
				}
				ScrollerVisibility scrollerVisibility2 = ScrollerVisibility.Auto;
				bool flag2 = this.m_VerticalScrollerVisibility.TryGetValueFromBag(bag, cc, ref scrollerVisibility2);
				if (flag2)
				{
					scrollView.verticalScrollerVisibility = scrollerVisibility2;
				}
				else
				{
					scrollView.showVertical = this.m_ShowVertical.GetValueFromBag(bag, cc);
				}
				scrollView.nestedInteractionKind = this.m_NestedInteractionKind.GetValueFromBag(bag, cc);
				scrollView.horizontalPageSize = this.m_HorizontalPageSize.GetValueFromBag(bag, cc);
				scrollView.verticalPageSize = this.m_VerticalPageSize.GetValueFromBag(bag, cc);
				scrollView.mouseWheelScrollSize = this.m_MouseWheelScrollSize.GetValueFromBag(bag, cc);
				scrollView.scrollDecelerationRate = this.m_ScrollDecelerationRate.GetValueFromBag(bag, cc);
				scrollView.touchScrollBehavior = this.m_TouchScrollBehavior.GetValueFromBag(bag, cc);
				scrollView.elasticity = this.m_Elasticity.GetValueFromBag(bag, cc);
				scrollView.elasticAnimationIntervalMs = this.m_ElasticAnimationIntervalMs.GetValueFromBag(bag, cc);
			}

			// Token: 0x040004AD RID: 1197
			private UxmlEnumAttributeDescription<ScrollViewMode> m_ScrollViewMode = new UxmlEnumAttributeDescription<ScrollViewMode>
			{
				name = "mode",
				defaultValue = ScrollViewMode.Vertical
			};

			// Token: 0x040004AE RID: 1198
			private UxmlEnumAttributeDescription<ScrollView.NestedInteractionKind> m_NestedInteractionKind = new UxmlEnumAttributeDescription<ScrollView.NestedInteractionKind>
			{
				name = "nested-interaction-kind",
				defaultValue = ScrollView.NestedInteractionKind.Default
			};

			// Token: 0x040004AF RID: 1199
			private UxmlBoolAttributeDescription m_ShowHorizontal = new UxmlBoolAttributeDescription
			{
				name = "show-horizontal-scroller"
			};

			// Token: 0x040004B0 RID: 1200
			private UxmlBoolAttributeDescription m_ShowVertical = new UxmlBoolAttributeDescription
			{
				name = "show-vertical-scroller"
			};

			// Token: 0x040004B1 RID: 1201
			private UxmlEnumAttributeDescription<ScrollerVisibility> m_HorizontalScrollerVisibility = new UxmlEnumAttributeDescription<ScrollerVisibility>
			{
				name = "horizontal-scroller-visibility"
			};

			// Token: 0x040004B2 RID: 1202
			private UxmlEnumAttributeDescription<ScrollerVisibility> m_VerticalScrollerVisibility = new UxmlEnumAttributeDescription<ScrollerVisibility>
			{
				name = "vertical-scroller-visibility"
			};

			// Token: 0x040004B3 RID: 1203
			private UxmlFloatAttributeDescription m_HorizontalPageSize = new UxmlFloatAttributeDescription
			{
				name = "horizontal-page-size",
				defaultValue = -1f
			};

			// Token: 0x040004B4 RID: 1204
			private UxmlFloatAttributeDescription m_VerticalPageSize = new UxmlFloatAttributeDescription
			{
				name = "vertical-page-size",
				defaultValue = -1f
			};

			// Token: 0x040004B5 RID: 1205
			private UxmlFloatAttributeDescription m_MouseWheelScrollSize = new UxmlFloatAttributeDescription
			{
				name = "mouse-wheel-scroll-size",
				defaultValue = 18f
			};

			// Token: 0x040004B6 RID: 1206
			private UxmlEnumAttributeDescription<ScrollView.TouchScrollBehavior> m_TouchScrollBehavior = new UxmlEnumAttributeDescription<ScrollView.TouchScrollBehavior>
			{
				name = "touch-scroll-type",
				defaultValue = ScrollView.TouchScrollBehavior.Clamped
			};

			// Token: 0x040004B7 RID: 1207
			private UxmlFloatAttributeDescription m_ScrollDecelerationRate = new UxmlFloatAttributeDescription
			{
				name = "scroll-deceleration-rate",
				defaultValue = ScrollView.k_DefaultScrollDecelerationRate
			};

			// Token: 0x040004B8 RID: 1208
			private UxmlFloatAttributeDescription m_Elasticity = new UxmlFloatAttributeDescription
			{
				name = "elasticity",
				defaultValue = ScrollView.k_DefaultElasticity
			};

			// Token: 0x040004B9 RID: 1209
			private UxmlLongAttributeDescription m_ElasticAnimationIntervalMs = new UxmlLongAttributeDescription
			{
				name = "elastic-animation-interval-ms",
				defaultValue = ScrollView.k_DefaultElasticAnimationInterval
			};
		}

		// Token: 0x02000127 RID: 295
		public enum TouchScrollBehavior
		{
			// Token: 0x040004BB RID: 1211
			Unrestricted,
			// Token: 0x040004BC RID: 1212
			Elastic,
			// Token: 0x040004BD RID: 1213
			Clamped
		}

		// Token: 0x02000128 RID: 296
		public enum NestedInteractionKind
		{
			// Token: 0x040004BF RID: 1215
			Default,
			// Token: 0x040004C0 RID: 1216
			StopScrolling,
			// Token: 0x040004C1 RID: 1217
			ForwardScrolling
		}

		// Token: 0x02000129 RID: 297
		internal enum TouchScrollingResult
		{
			// Token: 0x040004C3 RID: 1219
			Apply,
			// Token: 0x040004C4 RID: 1220
			Forward,
			// Token: 0x040004C5 RID: 1221
			Block
		}
	}
}
