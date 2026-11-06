using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x02000142 RID: 322
	public class TwoPaneSplitView : VisualElement
	{
		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06000A8F RID: 2703 RVA: 0x0002A128 File Offset: 0x00028328
		public VisualElement fixedPane
		{
			get
			{
				return this.m_FixedPane;
			}
		}

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000A90 RID: 2704 RVA: 0x0002A130 File Offset: 0x00028330
		public VisualElement flexedPane
		{
			get
			{
				return this.m_FlexedPane;
			}
		}

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06000A91 RID: 2705 RVA: 0x0002A138 File Offset: 0x00028338
		internal VisualElement dragLine
		{
			get
			{
				return this.m_DragLine;
			}
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06000A92 RID: 2706 RVA: 0x0002A140 File Offset: 0x00028340
		// (set) Token: 0x06000A93 RID: 2707 RVA: 0x0002A148 File Offset: 0x00028348
		public int fixedPaneIndex
		{
			get
			{
				return this.m_FixedPaneIndex;
			}
			set
			{
				bool flag = value == this.m_FixedPaneIndex;
				if (!flag)
				{
					this.Init(value, this.m_FixedPaneInitialDimension, this.m_Orientation);
				}
			}
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06000A94 RID: 2708 RVA: 0x0002A179 File Offset: 0x00028379
		// (set) Token: 0x06000A95 RID: 2709 RVA: 0x0002A184 File Offset: 0x00028384
		public float fixedPaneInitialDimension
		{
			get
			{
				return this.m_FixedPaneInitialDimension;
			}
			set
			{
				bool flag = value == this.m_FixedPaneInitialDimension;
				if (!flag)
				{
					this.Init(this.m_FixedPaneIndex, value, this.m_Orientation);
				}
			}
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x06000A96 RID: 2710 RVA: 0x0002A1B5 File Offset: 0x000283B5
		// (set) Token: 0x06000A97 RID: 2711 RVA: 0x0002A1C0 File Offset: 0x000283C0
		public TwoPaneSplitViewOrientation orientation
		{
			get
			{
				return this.m_Orientation;
			}
			set
			{
				bool flag = value == this.m_Orientation;
				if (!flag)
				{
					this.Init(this.m_FixedPaneIndex, this.m_FixedPaneInitialDimension, value);
				}
			}
		}

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x06000A98 RID: 2712 RVA: 0x0002A1F1 File Offset: 0x000283F1
		// (set) Token: 0x06000A99 RID: 2713 RVA: 0x0002A210 File Offset: 0x00028410
		internal float fixedPaneDimension
		{
			get
			{
				return string.IsNullOrEmpty(base.viewDataKey) ? this.m_FixedPaneInitialDimension : this.m_FixedPaneDimension;
			}
			set
			{
				bool flag = value == this.m_FixedPaneDimension;
				if (!flag)
				{
					this.m_FixedPaneDimension = value;
					base.SaveViewData();
				}
			}
		}

		// Token: 0x06000A9A RID: 2714 RVA: 0x0002A23C File Offset: 0x0002843C
		public TwoPaneSplitView()
		{
			base.AddToClassList(TwoPaneSplitView.s_UssClassName);
			this.m_Content = new VisualElement();
			this.m_Content.name = "unity-content-container";
			this.m_Content.AddToClassList(TwoPaneSplitView.s_ContentContainerClassName);
			base.hierarchy.Add(this.m_Content);
			this.m_DragLineAnchor = new VisualElement();
			this.m_DragLineAnchor.name = "unity-dragline-anchor";
			this.m_DragLineAnchor.AddToClassList(TwoPaneSplitView.s_HandleDragLineAnchorClassName);
			base.hierarchy.Add(this.m_DragLineAnchor);
			this.m_DragLine = new VisualElement();
			this.m_DragLine.name = "unity-dragline";
			this.m_DragLine.AddToClassList(TwoPaneSplitView.s_HandleDragLineClassName);
			this.m_DragLineAnchor.Add(this.m_DragLine);
		}

		// Token: 0x06000A9B RID: 2715 RVA: 0x0002A332 File Offset: 0x00028532
		public TwoPaneSplitView(int fixedPaneIndex, float fixedPaneStartDimension, TwoPaneSplitViewOrientation orientation)
			: this()
		{
			this.Init(fixedPaneIndex, fixedPaneStartDimension, orientation);
		}

		// Token: 0x06000A9C RID: 2716 RVA: 0x0002A348 File Offset: 0x00028548
		public void CollapseChild(int index)
		{
			bool flag = index != 0 && index != 1;
			if (!flag)
			{
				bool flag2 = this.m_LeftPane == null;
				if (flag2)
				{
					this.m_CollapseChildCalledBeforeSetupComplete = true;
					this.m_CollapsedChildIndex = index;
				}
				else
				{
					this.m_DragLine.style.display = DisplayStyle.None;
					this.m_DragLineAnchor.style.display = DisplayStyle.None;
					bool flag3 = index == 0;
					if (flag3)
					{
						this.m_RightPane.style.width = StyleKeyword.Initial;
						this.m_RightPane.style.height = StyleKeyword.Initial;
						this.m_RightPane.style.flexGrow = 1f;
						this.m_LeftPane.style.display = DisplayStyle.None;
					}
					else
					{
						this.m_LeftPane.style.width = StyleKeyword.Initial;
						this.m_LeftPane.style.height = StyleKeyword.Initial;
						this.m_LeftPane.style.flexGrow = 1f;
						this.m_RightPane.style.display = DisplayStyle.None;
					}
					this.m_CollapseMode = true;
				}
			}
		}

		// Token: 0x06000A9D RID: 2717 RVA: 0x0002A490 File Offset: 0x00028690
		public void UnCollapse()
		{
			bool flag = this.m_LeftPane == null;
			if (!flag)
			{
				VisualElement visualElement = null;
				bool flag2 = this.m_LeftPane.style.display == DisplayStyle.None;
				if (flag2)
				{
					visualElement = this.m_LeftPane;
				}
				else
				{
					bool flag3 = this.m_RightPane.style.display == DisplayStyle.None;
					if (flag3)
					{
						visualElement = this.m_RightPane;
					}
				}
				bool flag4 = visualElement == null;
				if (!flag4)
				{
					this.m_LeftPane.style.display = DisplayStyle.Flex;
					this.m_RightPane.style.display = DisplayStyle.Flex;
					this.m_DragLine.style.display = DisplayStyle.Flex;
					this.m_DragLineAnchor.style.display = DisplayStyle.Flex;
					this.m_LeftPane.style.flexGrow = 0f;
					this.m_RightPane.style.flexGrow = 0f;
					this.m_CollapseMode = false;
					this.m_CollapseChildCalledBeforeSetupComplete = false;
					this.m_CollapsedChildIndex = -1;
					this.Init(this.m_FixedPaneIndex, this.m_FixedPaneInitialDimension, this.m_Orientation);
					visualElement.RegisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.OnUncollapsedPaneResized), TrickleDown.NoTrickleDown);
				}
			}
		}

		// Token: 0x06000A9E RID: 2718 RVA: 0x0002A5E3 File Offset: 0x000287E3
		private void OnUncollapsedPaneResized(GeometryChangedEvent evt)
		{
			this.UpdateDragLineAnchorOffset();
			(evt.target as VisualElement).UnregisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.OnUncollapsedPaneResized), TrickleDown.NoTrickleDown);
		}

		// Token: 0x06000A9F RID: 2719 RVA: 0x0002A60C File Offset: 0x0002880C
		internal void Init(int fixedPaneIndex, float fixedPaneInitialDimension, TwoPaneSplitViewOrientation orientation)
		{
			this.m_Orientation = orientation;
			this.m_FixedPaneIndex = fixedPaneIndex;
			this.m_FixedPaneInitialDimension = fixedPaneInitialDimension;
			this.m_Content.RemoveFromClassList(TwoPaneSplitView.s_HorizontalClassName);
			this.m_Content.RemoveFromClassList(TwoPaneSplitView.s_VerticalClassName);
			bool flag = this.m_Orientation == TwoPaneSplitViewOrientation.Horizontal;
			if (flag)
			{
				this.m_Content.AddToClassList(TwoPaneSplitView.s_HorizontalClassName);
			}
			else
			{
				this.m_Content.AddToClassList(TwoPaneSplitView.s_VerticalClassName);
			}
			this.m_DragLineAnchor.RemoveFromClassList(TwoPaneSplitView.s_HandleDragLineAnchorHorizontalClassName);
			this.m_DragLineAnchor.RemoveFromClassList(TwoPaneSplitView.s_HandleDragLineAnchorVerticalClassName);
			bool flag2 = this.m_Orientation == TwoPaneSplitViewOrientation.Horizontal;
			if (flag2)
			{
				this.m_DragLineAnchor.AddToClassList(TwoPaneSplitView.s_HandleDragLineAnchorHorizontalClassName);
			}
			else
			{
				this.m_DragLineAnchor.AddToClassList(TwoPaneSplitView.s_HandleDragLineAnchorVerticalClassName);
			}
			this.m_DragLine.RemoveFromClassList(TwoPaneSplitView.s_HandleDragLineHorizontalClassName);
			this.m_DragLine.RemoveFromClassList(TwoPaneSplitView.s_HandleDragLineVerticalClassName);
			bool flag3 = this.m_Orientation == TwoPaneSplitViewOrientation.Horizontal;
			if (flag3)
			{
				this.m_DragLine.AddToClassList(TwoPaneSplitView.s_HandleDragLineHorizontalClassName);
			}
			else
			{
				this.m_DragLine.AddToClassList(TwoPaneSplitView.s_HandleDragLineVerticalClassName);
			}
			bool flag4 = this.m_Resizer != null;
			if (flag4)
			{
				this.m_DragLineAnchor.RemoveManipulator(this.m_Resizer);
				this.m_Resizer = null;
			}
			bool flag5 = this.m_Content.childCount != 2;
			if (flag5)
			{
				base.RegisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.OnPostDisplaySetup), TrickleDown.NoTrickleDown);
			}
			else
			{
				this.PostDisplaySetup();
			}
			this.m_DragLineAnchor.RegisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.OnAnchorPostDisplaySetup), TrickleDown.NoTrickleDown);
		}

		// Token: 0x06000AA0 RID: 2720 RVA: 0x0002A7A0 File Offset: 0x000289A0
		private void OnPostDisplaySetup(GeometryChangedEvent evt)
		{
			bool flag = this.m_Content.childCount != 2;
			if (flag)
			{
				Debug.LogError("TwoPaneSplitView needs exactly 2 children.");
			}
			else
			{
				this.PostDisplaySetup();
				bool collapseChildCalledBeforeSetupComplete = this.m_CollapseChildCalledBeforeSetupComplete;
				if (collapseChildCalledBeforeSetupComplete)
				{
					this.CollapseChild(this.m_CollapsedChildIndex);
					this.m_CollapseChildCalledBeforeSetupComplete = false;
				}
				base.UnregisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.OnPostDisplaySetup), TrickleDown.NoTrickleDown);
				this.ReplacePanesBasedOnAnchor();
			}
		}

		// Token: 0x06000AA1 RID: 2721 RVA: 0x0002A814 File Offset: 0x00028A14
		private void ReplacePanesBasedOnAnchor()
		{
			bool flag = this.m_Orientation == TwoPaneSplitViewOrientation.Horizontal;
			if (flag)
			{
				this.m_RightPane.style.left = this.m_DragLineAnchor.worldBound.width;
			}
			else
			{
				this.m_RightPane.style.top = this.m_DragLineAnchor.worldBound.height;
			}
		}

		// Token: 0x06000AA2 RID: 2722 RVA: 0x0002A884 File Offset: 0x00028A84
		private void OnAnchorPostDisplaySetup(GeometryChangedEvent evt)
		{
			bool flag = Mathf.Approximately(evt.newRect.width, evt.oldRect.width) && Mathf.Approximately(evt.newRect.height, evt.oldRect.height);
			if (!flag)
			{
				this.IdentifyLeftAndRightPane();
				this.ReplacePanesBasedOnAnchor();
			}
		}

		// Token: 0x06000AA3 RID: 2723 RVA: 0x0002A8F0 File Offset: 0x00028AF0
		private void IdentifyLeftAndRightPane()
		{
			this.m_LeftPane = this.m_Content[0];
			bool flag = this.m_FixedPaneIndex == 0;
			if (flag)
			{
				this.m_FixedPane = this.m_LeftPane;
			}
			else
			{
				this.m_FlexedPane = this.m_LeftPane;
			}
			this.m_RightPane = this.m_Content[1];
			bool flag2 = this.m_FixedPaneIndex == 1;
			if (flag2)
			{
				this.m_FixedPane = this.m_RightPane;
			}
			else
			{
				this.m_FlexedPane = this.m_RightPane;
			}
		}

		// Token: 0x06000AA4 RID: 2724 RVA: 0x0002A970 File Offset: 0x00028B70
		private void PostDisplaySetup()
		{
			bool flag = this.m_Content.childCount != 2;
			if (flag)
			{
				Debug.LogError("TwoPaneSplitView needs exactly 2 children.");
			}
			else
			{
				this.m_DragLineAnchor.UnregisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.OnAnchorPostDisplaySetup), TrickleDown.NoTrickleDown);
				bool flag2 = this.fixedPaneDimension < 0f;
				if (flag2)
				{
					this.fixedPaneDimension = this.m_FixedPaneInitialDimension;
				}
				float fixedPaneDimension = this.fixedPaneDimension;
				this.IdentifyLeftAndRightPane();
				this.m_FixedPane.style.flexBasis = StyleKeyword.Null;
				this.m_FixedPane.style.flexShrink = StyleKeyword.Null;
				this.m_FixedPane.style.flexGrow = StyleKeyword.Null;
				this.m_FlexedPane.style.flexGrow = StyleKeyword.Null;
				this.m_FlexedPane.style.flexShrink = StyleKeyword.Null;
				this.m_FlexedPane.style.flexBasis = StyleKeyword.Null;
				this.m_FixedPane.style.width = StyleKeyword.Null;
				this.m_FixedPane.style.height = StyleKeyword.Null;
				this.m_FlexedPane.style.width = StyleKeyword.Null;
				this.m_FlexedPane.style.height = StyleKeyword.Null;
				bool flag3 = this.m_Orientation == TwoPaneSplitViewOrientation.Horizontal;
				if (flag3)
				{
					this.m_FixedPane.style.width = fixedPaneDimension;
					this.m_FixedPane.style.height = StyleKeyword.Null;
				}
				else
				{
					this.m_FixedPane.style.width = StyleKeyword.Null;
					this.m_FixedPane.style.height = fixedPaneDimension;
				}
				this.m_FixedPane.style.flexShrink = 0f;
				this.m_FixedPane.style.flexGrow = 0f;
				this.m_FlexedPane.style.flexGrow = 1f;
				this.m_FlexedPane.style.flexShrink = 0f;
				this.m_FlexedPane.style.flexBasis = 0f;
				this.m_DragLineAnchor.style.left = 0f;
				this.m_DragLineAnchor.style.top = 0f;
				bool flag4 = this.m_Orientation == TwoPaneSplitViewOrientation.Horizontal;
				if (flag4)
				{
					float num = this.m_FixedPane.resolvedStyle.marginLeft + this.m_FixedPane.resolvedStyle.marginRight;
					bool flag5 = this.m_FixedPaneIndex == 0;
					if (flag5)
					{
						this.m_DragLineAnchor.style.left = num + fixedPaneDimension;
					}
					else
					{
						this.m_DragLineAnchor.style.left = base.resolvedStyle.width - num - fixedPaneDimension - this.m_DragLineAnchor.resolvedStyle.width;
					}
				}
				else
				{
					float num2 = this.m_FixedPane.resolvedStyle.marginTop + this.m_FixedPane.resolvedStyle.marginBottom;
					bool flag6 = this.m_FixedPaneIndex == 0;
					if (flag6)
					{
						this.m_DragLineAnchor.style.top = num2 + fixedPaneDimension;
					}
					else
					{
						this.m_DragLineAnchor.style.top = base.resolvedStyle.height - num2 - fixedPaneDimension - this.m_DragLineAnchor.resolvedStyle.height;
					}
				}
				bool flag7 = this.m_FixedPaneIndex == 0;
				int num3;
				if (flag7)
				{
					num3 = 1;
				}
				else
				{
					num3 = -1;
				}
				bool flag8 = this.m_Resizer != null;
				if (flag8)
				{
					this.m_DragLineAnchor.RemoveManipulator(this.m_Resizer);
				}
				this.m_Resizer = new TwoPaneSplitViewResizer(this, num3);
				this.m_DragLineAnchor.AddManipulator(this.m_Resizer);
				base.RegisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.OnSizeChange), TrickleDown.NoTrickleDown);
				this.m_DragLineAnchor.RegisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.OnAnchorPostDisplaySetup), TrickleDown.NoTrickleDown);
			}
		}

		// Token: 0x06000AA5 RID: 2725 RVA: 0x0002ADA6 File Offset: 0x00028FA6
		private void OnSizeChange(GeometryChangedEvent evt)
		{
			this.UpdateLayout(true, true);
		}

		// Token: 0x06000AA6 RID: 2726 RVA: 0x0002ADB2 File Offset: 0x00028FB2
		private void UpdateDragLineAnchorOffset()
		{
			this.UpdateLayout(false, true);
		}

		// Token: 0x06000AA7 RID: 2727 RVA: 0x0002ADC0 File Offset: 0x00028FC0
		private void UpdateLayout(bool updateFixedPane, bool updateDragLine)
		{
			bool collapseMode = this.m_CollapseMode;
			if (!collapseMode)
			{
				bool flag = base.resolvedStyle.display == DisplayStyle.None || base.resolvedStyle.visibility == Visibility.Hidden;
				if (!flag)
				{
					float num = base.resolvedStyle.width;
					float num2 = this.m_FixedPane.resolvedStyle.width;
					float num3 = this.m_FixedPane.resolvedStyle.marginLeft + this.m_FixedPane.resolvedStyle.marginRight;
					float num4 = this.m_FixedPane.resolvedStyle.minWidth.value;
					float num5 = this.m_FlexedPane.resolvedStyle.marginLeft + this.m_FlexedPane.resolvedStyle.marginRight;
					float num6 = this.m_FlexedPane.resolvedStyle.minWidth.value;
					bool flag2 = this.m_Orientation == TwoPaneSplitViewOrientation.Vertical;
					if (flag2)
					{
						num = base.resolvedStyle.height;
						num2 = this.m_FixedPane.resolvedStyle.height;
						num3 = this.m_FixedPane.resolvedStyle.marginTop + this.m_FixedPane.resolvedStyle.marginBottom;
						num4 = this.m_FixedPane.resolvedStyle.minHeight.value;
						num5 = this.m_FlexedPane.resolvedStyle.marginTop + this.m_FlexedPane.resolvedStyle.marginBottom;
						num6 = this.m_FlexedPane.resolvedStyle.minHeight.value;
					}
					bool flag3 = num >= num2 + num3 + num6 + num5;
					if (flag3)
					{
						if (updateDragLine)
						{
							this.SetDragLineOffset((this.m_FixedPaneIndex == 0) ? (num2 + num3) : (num - num2 - num3));
						}
					}
					else
					{
						bool flag4 = num >= num4 + num3 + num6 + num5;
						if (flag4)
						{
							float num7 = num - num6 - num5 - num3;
							float num8 = ((this.m_Orientation == TwoPaneSplitViewOrientation.Horizontal) ? Math.Abs(this.m_DragLineAnchor.worldBound.width - (this.m_DragLine.resolvedStyle.width - Math.Abs(this.m_DragLine.resolvedStyle.left))) : Math.Abs(this.m_DragLineAnchor.worldBound.height - (this.m_DragLine.resolvedStyle.height - Math.Abs(this.m_DragLine.resolvedStyle.top))));
							num7 -= num8;
							if (updateFixedPane)
							{
								this.SetFixedPaneDimension(num7);
							}
							if (updateDragLine)
							{
								this.SetDragLineOffset((this.m_FixedPaneIndex == 0) ? (num7 + num3 + num8) : (num6 + num5));
							}
						}
						else
						{
							if (updateFixedPane)
							{
								this.SetFixedPaneDimension(num4);
							}
							if (updateDragLine)
							{
								this.SetDragLineOffset((this.m_FixedPaneIndex == 0) ? (num4 + num3) : (num6 + num5));
							}
						}
					}
				}
			}
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x06000AA8 RID: 2728 RVA: 0x0002B0B0 File Offset: 0x000292B0
		public override VisualElement contentContainer
		{
			get
			{
				return this.m_Content;
			}
		}

		// Token: 0x06000AA9 RID: 2729 RVA: 0x0002B0C8 File Offset: 0x000292C8
		internal override void OnViewDataReady()
		{
			base.OnViewDataReady();
			string fullHierarchicalViewDataKey = base.GetFullHierarchicalViewDataKey();
			base.OverwriteFromViewData(this, fullHierarchicalViewDataKey);
			this.PostDisplaySetup();
		}

		// Token: 0x06000AAA RID: 2730 RVA: 0x0002B0F4 File Offset: 0x000292F4
		private void SetDragLineOffset(float offset)
		{
			bool flag = this.m_Orientation == TwoPaneSplitViewOrientation.Horizontal;
			if (flag)
			{
				this.m_DragLineAnchor.style.left = offset;
			}
			else
			{
				this.m_DragLineAnchor.style.top = offset;
			}
		}

		// Token: 0x06000AAB RID: 2731 RVA: 0x0002B140 File Offset: 0x00029340
		private void SetFixedPaneDimension(float dimension)
		{
			bool flag = this.m_Orientation == TwoPaneSplitViewOrientation.Horizontal;
			if (flag)
			{
				this.m_FixedPane.style.width = dimension;
			}
			else
			{
				this.m_FixedPane.style.height = dimension;
			}
		}

		// Token: 0x04000508 RID: 1288
		private static readonly string s_UssClassName = "unity-two-pane-split-view";

		// Token: 0x04000509 RID: 1289
		private static readonly string s_ContentContainerClassName = "unity-two-pane-split-view__content-container";

		// Token: 0x0400050A RID: 1290
		private static readonly string s_HandleDragLineClassName = "unity-two-pane-split-view__dragline";

		// Token: 0x0400050B RID: 1291
		private static readonly string s_HandleDragLineVerticalClassName = TwoPaneSplitView.s_HandleDragLineClassName + "--vertical";

		// Token: 0x0400050C RID: 1292
		private static readonly string s_HandleDragLineHorizontalClassName = TwoPaneSplitView.s_HandleDragLineClassName + "--horizontal";

		// Token: 0x0400050D RID: 1293
		private static readonly string s_HandleDragLineAnchorClassName = "unity-two-pane-split-view__dragline-anchor";

		// Token: 0x0400050E RID: 1294
		private static readonly string s_HandleDragLineAnchorVerticalClassName = TwoPaneSplitView.s_HandleDragLineAnchorClassName + "--vertical";

		// Token: 0x0400050F RID: 1295
		private static readonly string s_HandleDragLineAnchorHorizontalClassName = TwoPaneSplitView.s_HandleDragLineAnchorClassName + "--horizontal";

		// Token: 0x04000510 RID: 1296
		private static readonly string s_VerticalClassName = "unity-two-pane-split-view--vertical";

		// Token: 0x04000511 RID: 1297
		private static readonly string s_HorizontalClassName = "unity-two-pane-split-view--horizontal";

		// Token: 0x04000512 RID: 1298
		private VisualElement m_LeftPane;

		// Token: 0x04000513 RID: 1299
		private VisualElement m_RightPane;

		// Token: 0x04000514 RID: 1300
		private VisualElement m_FixedPane;

		// Token: 0x04000515 RID: 1301
		private VisualElement m_FlexedPane;

		// Token: 0x04000516 RID: 1302
		[SerializeField]
		private float m_FixedPaneDimension = -1f;

		// Token: 0x04000517 RID: 1303
		private VisualElement m_DragLine;

		// Token: 0x04000518 RID: 1304
		private VisualElement m_DragLineAnchor;

		// Token: 0x04000519 RID: 1305
		private bool m_CollapseMode;

		// Token: 0x0400051A RID: 1306
		private bool m_CollapseChildCalledBeforeSetupComplete;

		// Token: 0x0400051B RID: 1307
		private int m_CollapsedChildIndex = -1;

		// Token: 0x0400051C RID: 1308
		private VisualElement m_Content;

		// Token: 0x0400051D RID: 1309
		private TwoPaneSplitViewOrientation m_Orientation;

		// Token: 0x0400051E RID: 1310
		private int m_FixedPaneIndex;

		// Token: 0x0400051F RID: 1311
		private float m_FixedPaneInitialDimension;

		// Token: 0x04000520 RID: 1312
		internal TwoPaneSplitViewResizer m_Resizer;

		// Token: 0x02000143 RID: 323
		public new class UxmlFactory : UxmlFactory<TwoPaneSplitView, TwoPaneSplitView.UxmlTraits>
		{
		}

		// Token: 0x02000144 RID: 324
		public new class UxmlTraits : VisualElement.UxmlTraits
		{
			// Token: 0x1700020C RID: 524
			// (get) Token: 0x06000AAE RID: 2734 RVA: 0x0002B230 File Offset: 0x00029430
			public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
			{
				get
				{
					yield break;
				}
			}

			// Token: 0x06000AAF RID: 2735 RVA: 0x0002B250 File Offset: 0x00029450
			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ve, bag, cc);
				int valueFromBag = this.m_FixedPaneIndex.GetValueFromBag(bag, cc);
				int valueFromBag2 = this.m_FixedPaneInitialDimension.GetValueFromBag(bag, cc);
				TwoPaneSplitViewOrientation valueFromBag3 = this.m_Orientation.GetValueFromBag(bag, cc);
				((TwoPaneSplitView)ve).Init(valueFromBag, (float)valueFromBag2, valueFromBag3);
			}

			// Token: 0x04000521 RID: 1313
			private UxmlIntAttributeDescription m_FixedPaneIndex = new UxmlIntAttributeDescription
			{
				name = "fixed-pane-index",
				defaultValue = 0
			};

			// Token: 0x04000522 RID: 1314
			private UxmlIntAttributeDescription m_FixedPaneInitialDimension = new UxmlIntAttributeDescription
			{
				name = "fixed-pane-initial-dimension",
				defaultValue = 100
			};

			// Token: 0x04000523 RID: 1315
			private UxmlEnumAttributeDescription<TwoPaneSplitViewOrientation> m_Orientation = new UxmlEnumAttributeDescription<TwoPaneSplitViewOrientation>
			{
				name = "orientation",
				defaultValue = TwoPaneSplitViewOrientation.Horizontal
			};
		}
	}
}
