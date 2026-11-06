using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000147 RID: 327
	internal class TwoPaneSplitViewResizer : PointerManipulator
	{
		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06000AB9 RID: 2745 RVA: 0x0002B3AF File Offset: 0x000295AF
		private TwoPaneSplitViewOrientation orientation
		{
			get
			{
				return this.m_SplitView.orientation;
			}
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06000ABA RID: 2746 RVA: 0x0002B3BC File Offset: 0x000295BC
		private VisualElement fixedPane
		{
			get
			{
				return this.m_SplitView.fixedPane;
			}
		}

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06000ABB RID: 2747 RVA: 0x0002B3C9 File Offset: 0x000295C9
		private VisualElement flexedPane
		{
			get
			{
				return this.m_SplitView.flexedPane;
			}
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06000ABC RID: 2748 RVA: 0x0002B3D8 File Offset: 0x000295D8
		private float fixedPaneMinDimension
		{
			get
			{
				bool flag = this.orientation == TwoPaneSplitViewOrientation.Horizontal;
				float num;
				if (flag)
				{
					num = this.fixedPane.resolvedStyle.minWidth.value;
				}
				else
				{
					num = this.fixedPane.resolvedStyle.minHeight.value;
				}
				return num;
			}
		}

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06000ABD RID: 2749 RVA: 0x0002B42C File Offset: 0x0002962C
		private float fixedPaneMargins
		{
			get
			{
				bool flag = this.orientation == TwoPaneSplitViewOrientation.Horizontal;
				float num;
				if (flag)
				{
					num = this.fixedPane.resolvedStyle.marginLeft + this.fixedPane.resolvedStyle.marginRight;
				}
				else
				{
					num = this.fixedPane.resolvedStyle.marginTop + this.fixedPane.resolvedStyle.marginBottom;
				}
				return num;
			}
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06000ABE RID: 2750 RVA: 0x0002B490 File Offset: 0x00029690
		private float flexedPaneMinDimension
		{
			get
			{
				bool flag = this.orientation == TwoPaneSplitViewOrientation.Horizontal;
				float num;
				if (flag)
				{
					num = this.flexedPane.resolvedStyle.minWidth.value;
				}
				else
				{
					num = this.flexedPane.resolvedStyle.minHeight.value;
				}
				return num;
			}
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06000ABF RID: 2751 RVA: 0x0002B4E4 File Offset: 0x000296E4
		private float flexedPaneMargin
		{
			get
			{
				bool flag = this.orientation == TwoPaneSplitViewOrientation.Horizontal;
				float num;
				if (flag)
				{
					num = this.flexedPane.resolvedStyle.marginLeft + this.flexedPane.resolvedStyle.marginRight;
				}
				else
				{
					num = this.flexedPane.resolvedStyle.marginTop + this.flexedPane.resolvedStyle.marginBottom;
				}
				return num;
			}
		}

		// Token: 0x06000AC0 RID: 2752 RVA: 0x0002B548 File Offset: 0x00029748
		public TwoPaneSplitViewResizer(TwoPaneSplitView splitView, int dir)
		{
			this.m_SplitView = splitView;
			this.m_Direction = dir;
			base.activators.Add(new ManipulatorActivationFilter
			{
				button = MouseButton.LeftMouse
			});
			this.m_Active = false;
		}

		// Token: 0x06000AC1 RID: 2753 RVA: 0x0002B590 File Offset: 0x00029790
		protected override void RegisterCallbacksOnTarget()
		{
			base.target.RegisterCallback<PointerDownEvent>(new EventCallback<PointerDownEvent>(this.OnPointerDown), TrickleDown.NoTrickleDown);
			base.target.RegisterCallback<PointerMoveEvent>(new EventCallback<PointerMoveEvent>(this.OnPointerMove), TrickleDown.NoTrickleDown);
			base.target.RegisterCallback<PointerUpEvent>(new EventCallback<PointerUpEvent>(this.OnPointerUp), TrickleDown.NoTrickleDown);
		}

		// Token: 0x06000AC2 RID: 2754 RVA: 0x0002B5EC File Offset: 0x000297EC
		protected override void UnregisterCallbacksFromTarget()
		{
			base.target.UnregisterCallback<PointerDownEvent>(new EventCallback<PointerDownEvent>(this.OnPointerDown), TrickleDown.NoTrickleDown);
			base.target.UnregisterCallback<PointerMoveEvent>(new EventCallback<PointerMoveEvent>(this.OnPointerMove), TrickleDown.NoTrickleDown);
			base.target.UnregisterCallback<PointerUpEvent>(new EventCallback<PointerUpEvent>(this.OnPointerUp), TrickleDown.NoTrickleDown);
		}

		// Token: 0x06000AC3 RID: 2755 RVA: 0x0002B648 File Offset: 0x00029848
		public void ApplyDelta(float delta)
		{
			float num = ((this.orientation == TwoPaneSplitViewOrientation.Horizontal) ? this.fixedPane.resolvedStyle.width : this.fixedPane.resolvedStyle.height);
			float num2 = num + delta;
			float num3 = this.fixedPaneMinDimension;
			bool flag = this.m_SplitView.fixedPaneIndex == 1;
			if (flag)
			{
				num3 += ((this.orientation == TwoPaneSplitViewOrientation.Horizontal) ? (base.target.worldBound.width + Math.Abs(this.m_SplitView.dragLine.resolvedStyle.left)) : (base.target.worldBound.height + Math.Abs(this.m_SplitView.dragLine.resolvedStyle.top)));
			}
			bool flag2 = num2 < num && num2 < num3;
			if (flag2)
			{
				num2 = num3;
			}
			float num4 = ((this.orientation == TwoPaneSplitViewOrientation.Horizontal) ? this.m_SplitView.resolvedStyle.width : this.m_SplitView.resolvedStyle.height);
			num4 -= this.flexedPaneMinDimension + this.flexedPaneMargin + this.fixedPaneMargins;
			bool flag3 = this.m_SplitView.fixedPaneIndex == 0;
			if (flag3)
			{
				num4 -= ((this.orientation == TwoPaneSplitViewOrientation.Horizontal) ? Math.Abs(base.target.worldBound.width - (this.m_SplitView.dragLine.resolvedStyle.width - Math.Abs(this.m_SplitView.dragLine.resolvedStyle.left))) : Math.Abs(base.target.worldBound.height - (this.m_SplitView.dragLine.resolvedStyle.height - Math.Abs(this.m_SplitView.dragLine.resolvedStyle.top))));
			}
			bool flag4 = num2 > num && num2 > num4;
			if (flag4)
			{
				num2 = num4;
			}
			bool flag5 = this.orientation == TwoPaneSplitViewOrientation.Horizontal;
			if (flag5)
			{
				this.fixedPane.style.width = num2;
				bool flag6 = this.m_SplitView.fixedPaneIndex == 0;
				if (flag6)
				{
					base.target.style.left = num2 + this.fixedPaneMargins;
				}
				else
				{
					float num5 = this.m_SplitView.resolvedStyle.width - num2 - this.fixedPaneMargins;
					bool flag7 = num5 >= this.flexedPaneMinDimension + this.flexedPaneMargin;
					if (flag7)
					{
						base.target.style.left = num5;
					}
				}
			}
			else
			{
				this.fixedPane.style.height = num2;
				bool flag8 = this.m_SplitView.fixedPaneIndex == 0;
				if (flag8)
				{
					base.target.style.top = num2 + this.fixedPaneMargins;
				}
				else
				{
					float num6 = this.m_SplitView.resolvedStyle.height - num2 - this.fixedPaneMargins;
					bool flag9 = num6 >= this.flexedPaneMinDimension + this.flexedPaneMargin;
					if (flag9)
					{
						base.target.style.top = num6;
					}
				}
			}
			this.m_SplitView.fixedPaneDimension = num2;
		}

		// Token: 0x06000AC4 RID: 2756 RVA: 0x0002B988 File Offset: 0x00029B88
		protected void OnPointerDown(PointerDownEvent e)
		{
			bool active = this.m_Active;
			if (active)
			{
				e.StopImmediatePropagation();
			}
			else
			{
				bool flag = base.CanStartManipulation(e);
				if (flag)
				{
					this.m_Start = e.localPosition;
					this.m_Active = true;
					base.target.CapturePointer(e.pointerId);
					e.StopPropagation();
				}
			}
		}

		// Token: 0x06000AC5 RID: 2757 RVA: 0x0002B9E4 File Offset: 0x00029BE4
		protected void OnPointerMove(PointerMoveEvent e)
		{
			bool flag = !this.m_Active || !base.target.HasPointerCapture(e.pointerId);
			if (!flag)
			{
				bool flag2 = ((this.orientation == TwoPaneSplitViewOrientation.Horizontal) ? (this.m_SplitView.dragLine.worldBound.x < base.target.worldBound.x) : (this.m_SplitView.dragLine.worldBound.y < base.target.worldBound.y));
				float num = ((this.orientation == TwoPaneSplitViewOrientation.Horizontal) ? Math.Abs(base.target.worldBound.x - this.m_SplitView.dragLine.worldBound.x) : Math.Abs(base.target.worldBound.y - this.m_SplitView.dragLine.worldBound.y));
				float num2 = ((this.orientation == TwoPaneSplitViewOrientation.Horizontal) ? this.m_SplitView.dragLine.resolvedStyle.left : this.m_SplitView.dragLine.resolvedStyle.top);
				bool flag3 = flag2 && Math.Abs(num2) + 1f <= num;
				if (flag3)
				{
					this.InterruptPointerMove(e);
				}
				else
				{
					Vector2 vector = e.localPosition - this.m_Start;
					float num3 = vector.x;
					bool flag4 = this.orientation == TwoPaneSplitViewOrientation.Vertical;
					if (flag4)
					{
						num3 = vector.y;
					}
					float num4 = (float)this.m_Direction * num3;
					this.ApplyDelta(num4);
					e.StopPropagation();
				}
			}
		}

		// Token: 0x06000AC6 RID: 2758 RVA: 0x0002BBA8 File Offset: 0x00029DA8
		protected void OnPointerUp(PointerUpEvent e)
		{
			bool flag = !this.m_Active || !base.target.HasPointerCapture(e.pointerId) || !base.CanStopManipulation(e);
			if (!flag)
			{
				this.m_Active = false;
				base.target.ReleasePointer(e.pointerId);
				e.StopPropagation();
			}
		}

		// Token: 0x06000AC7 RID: 2759 RVA: 0x0002BC04 File Offset: 0x00029E04
		protected void InterruptPointerMove(PointerMoveEvent e)
		{
			bool flag = !base.CanStopManipulation(e);
			if (!flag)
			{
				this.m_Active = false;
				base.target.ReleasePointer(e.pointerId);
				e.StopPropagation();
			}
		}

		// Token: 0x0400052B RID: 1323
		private const float k_DragLineTolerance = 1f;

		// Token: 0x0400052C RID: 1324
		private Vector3 m_Start;

		// Token: 0x0400052D RID: 1325
		protected bool m_Active;

		// Token: 0x0400052E RID: 1326
		private TwoPaneSplitView m_SplitView;

		// Token: 0x0400052F RID: 1327
		private int m_Direction;
	}
}
