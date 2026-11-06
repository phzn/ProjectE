using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000245 RID: 581
	public abstract class Focusable : CallbackEventHandler
	{
		// Token: 0x0600107F RID: 4223 RVA: 0x0003BE4D File Offset: 0x0003A04D
		protected Focusable()
		{
			this.focusable = true;
			this.tabIndex = 0;
		}

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x06001080 RID: 4224
		public abstract FocusController focusController { get; }

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x06001081 RID: 4225 RVA: 0x0003BE67 File Offset: 0x0003A067
		// (set) Token: 0x06001082 RID: 4226 RVA: 0x0003BE6F File Offset: 0x0003A06F
		public bool focusable { get; set; }

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x06001083 RID: 4227 RVA: 0x0003BE78 File Offset: 0x0003A078
		// (set) Token: 0x06001084 RID: 4228 RVA: 0x0003BE80 File Offset: 0x0003A080
		public int tabIndex { get; set; }

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x06001085 RID: 4229 RVA: 0x0003BE8C File Offset: 0x0003A08C
		// (set) Token: 0x06001086 RID: 4230 RVA: 0x0003BEA4 File Offset: 0x0003A0A4
		public bool delegatesFocus
		{
			get
			{
				return this.m_DelegatesFocus;
			}
			set
			{
				this.m_DelegatesFocus = value;
			}
		}

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x06001087 RID: 4231 RVA: 0x0003BEB0 File Offset: 0x0003A0B0
		// (set) Token: 0x06001088 RID: 4232 RVA: 0x0003BEC8 File Offset: 0x0003A0C8
		internal bool excludeFromFocusRing
		{
			get
			{
				return this.m_ExcludeFromFocusRing;
			}
			set
			{
				bool flag = !((VisualElement)this).isCompositeRoot;
				if (flag)
				{
					throw new InvalidOperationException("excludeFromFocusRing should only be set on composite roots.");
				}
				this.m_ExcludeFromFocusRing = value;
			}
		}

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x06001089 RID: 4233 RVA: 0x0003BEFB File Offset: 0x0003A0FB
		public virtual bool canGrabFocus
		{
			get
			{
				return this.focusable;
			}
		}

		// Token: 0x0600108A RID: 4234 RVA: 0x0003BF04 File Offset: 0x0003A104
		public virtual void Focus()
		{
			bool flag = this.focusController != null;
			if (flag)
			{
				bool canGrabFocus = this.canGrabFocus;
				if (canGrabFocus)
				{
					Focusable focusDelegate = this.GetFocusDelegate();
					this.focusController.SwitchFocus(focusDelegate, this != focusDelegate, DispatchMode.Default);
				}
				else
				{
					this.focusController.SwitchFocus(null, false, DispatchMode.Default);
				}
			}
		}

		// Token: 0x0600108B RID: 4235 RVA: 0x0003BF5C File Offset: 0x0003A15C
		public virtual void Blur()
		{
			FocusController focusController = this.focusController;
			if (focusController != null)
			{
				focusController.Blur(this, false, DispatchMode.Default);
			}
		}

		// Token: 0x0600108C RID: 4236 RVA: 0x0003BF74 File Offset: 0x0003A174
		internal void BlurImmediately()
		{
			FocusController focusController = this.focusController;
			if (focusController != null)
			{
				focusController.Blur(this, false, DispatchMode.Immediate);
			}
		}

		// Token: 0x0600108D RID: 4237 RVA: 0x0003BF8C File Offset: 0x0003A18C
		private Focusable GetFocusDelegate()
		{
			Focusable focusable = this;
			while (focusable != null && focusable.delegatesFocus)
			{
				focusable = Focusable.GetFirstFocusableChild(focusable as VisualElement);
			}
			return focusable;
		}

		// Token: 0x0600108E RID: 4238 RVA: 0x0003BFC4 File Offset: 0x0003A1C4
		private static Focusable GetFirstFocusableChild(VisualElement ve)
		{
			int childCount = ve.hierarchy.childCount;
			int i = 0;
			while (i < childCount)
			{
				VisualElement visualElement = ve.hierarchy[i];
				bool flag = visualElement.canGrabFocus && visualElement.tabIndex >= 0;
				if (!flag)
				{
					bool flag2 = visualElement.hierarchy.parent != null && visualElement == visualElement.hierarchy.parent.contentContainer;
					bool flag3 = !visualElement.isCompositeRoot && !flag2;
					if (flag3)
					{
						Focusable firstFocusableChild = Focusable.GetFirstFocusableChild(visualElement);
						bool flag4 = firstFocusableChild != null;
						if (flag4)
						{
							return firstFocusableChild;
						}
					}
					i++;
					continue;
				}
				return visualElement;
			}
			return null;
		}

		// Token: 0x0600108F RID: 4239 RVA: 0x0003C092 File Offset: 0x0003A292
		[EventInterest(new Type[]
		{
			typeof(PointerDownEvent),
			typeof(NavigationMoveEvent)
		})]
		protected override void ExecuteDefaultAction(EventBase evt)
		{
			base.ExecuteDefaultAction(evt);
			this.ProcessEvent(evt);
		}

		// Token: 0x06001090 RID: 4240 RVA: 0x0003C0A5 File Offset: 0x0003A2A5
		[EventInterest(new Type[]
		{
			typeof(PointerDownEvent),
			typeof(NavigationMoveEvent)
		})]
		internal override void ExecuteDefaultActionDisabled(EventBase evt)
		{
			base.ExecuteDefaultActionDisabled(evt);
			this.ProcessEvent(evt);
		}

		// Token: 0x06001091 RID: 4241 RVA: 0x0003C0B8 File Offset: 0x0003A2B8
		private void ProcessEvent(EventBase evt)
		{
			bool flag = evt != null && evt.target == evt.leafTarget;
			if (flag)
			{
				FocusController focusController = this.focusController;
				if (focusController != null)
				{
					focusController.SwitchFocusOnEvent(evt);
				}
			}
		}

		// Token: 0x04000746 RID: 1862
		private bool m_DelegatesFocus;

		// Token: 0x04000747 RID: 1863
		private bool m_ExcludeFromFocusRing;
	}
}
