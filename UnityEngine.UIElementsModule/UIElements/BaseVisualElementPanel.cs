using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.Yoga;

namespace UnityEngine.UIElements
{
	// Token: 0x02000293 RID: 659
	internal abstract class BaseVisualElementPanel : IPanel, IDisposable, IGroupBox
	{
		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x06001259 RID: 4697
		// (set) Token: 0x0600125A RID: 4698
		public abstract EventInterests IMGUIEventInterests { get; set; }

		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x0600125B RID: 4699
		// (set) Token: 0x0600125C RID: 4700
		public abstract ScriptableObject ownerObject { get; protected set; }

		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x0600125D RID: 4701
		// (set) Token: 0x0600125E RID: 4702
		public abstract SavePersistentViewData saveViewData { get; set; }

		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x0600125F RID: 4703
		// (set) Token: 0x06001260 RID: 4704
		public abstract GetViewDataDictionary getViewDataDictionary { get; set; }

		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x06001261 RID: 4705
		// (set) Token: 0x06001262 RID: 4706
		public abstract int IMGUIContainersCount { get; set; }

		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x06001263 RID: 4707
		// (set) Token: 0x06001264 RID: 4708
		public abstract FocusController focusController { get; set; }

		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x06001265 RID: 4709
		// (set) Token: 0x06001266 RID: 4710
		public abstract IMGUIContainer rootIMGUIContainer { get; set; }

		// Token: 0x14000035 RID: 53
		// (add) Token: 0x06001267 RID: 4711 RVA: 0x00041734 File Offset: 0x0003F934
		// (remove) Token: 0x06001268 RID: 4712 RVA: 0x0004176C File Offset: 0x0003F96C
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal event Action<BaseVisualElementPanel> panelDisposed;

		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x06001269 RID: 4713 RVA: 0x000417A4 File Offset: 0x0003F9A4
		// (set) Token: 0x0600126A RID: 4714 RVA: 0x000417D4 File Offset: 0x0003F9D4
		internal UIElementsBridge uiElementsBridge
		{
			get
			{
				bool flag = this.m_UIElementsBridge != null;
				if (flag)
				{
					return this.m_UIElementsBridge;
				}
				throw new Exception("Panel has no UIElementsBridge.");
			}
			set
			{
				this.m_UIElementsBridge = value;
			}
		}

		// Token: 0x0600126B RID: 4715 RVA: 0x000417E0 File Offset: 0x0003F9E0
		protected BaseVisualElementPanel()
		{
			this.yogaConfig = new YogaConfig();
			this.yogaConfig.UseWebDefaults = YogaConfig.Default.UseWebDefaults;
			this.m_UIElementsBridge = new RuntimeUIElementsBridge();
		}

		// Token: 0x0600126C RID: 4716 RVA: 0x00041878 File Offset: 0x0003FA78
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600126D RID: 4717 RVA: 0x0004188C File Offset: 0x0003FA8C
		protected virtual void Dispose(bool disposing)
		{
			bool disposed = this.disposed;
			if (!disposed)
			{
				if (disposing)
				{
					bool flag = this.ownerObject != null;
					if (flag)
					{
						UIElementsUtility.RemoveCachedPanel(this.ownerObject.GetInstanceID());
					}
					PointerDeviceState.RemovePanelData(this);
				}
				Action<BaseVisualElementPanel> action = this.panelDisposed;
				if (action != null)
				{
					action(this);
				}
				this.yogaConfig = null;
				this.disposed = true;
			}
		}

		// Token: 0x0600126E RID: 4718
		public abstract void Repaint(Event e);

		// Token: 0x0600126F RID: 4719
		public abstract void ValidateFocus();

		// Token: 0x06001270 RID: 4720
		public abstract void ValidateLayout();

		// Token: 0x06001271 RID: 4721
		public abstract void UpdateAnimations();

		// Token: 0x06001272 RID: 4722
		public abstract void UpdateBindings();

		// Token: 0x06001273 RID: 4723
		public abstract void ApplyStyles();

		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x06001274 RID: 4724 RVA: 0x000418FC File Offset: 0x0003FAFC
		// (set) Token: 0x06001275 RID: 4725 RVA: 0x00041914 File Offset: 0x0003FB14
		internal float scale
		{
			get
			{
				return this.m_Scale;
			}
			set
			{
				bool flag = !Mathf.Approximately(this.m_Scale, value);
				if (flag)
				{
					this.m_Scale = value;
					this.visualTree.IncrementVersion(VersionChangeType.Layout);
					this.yogaConfig.PointScaleFactor = this.scaledPixelsPerPoint;
					this.visualTree.IncrementVersion(VersionChangeType.StyleSheet);
				}
			}
		}

		// Token: 0x170003DA RID: 986
		// (get) Token: 0x06001276 RID: 4726 RVA: 0x0004196C File Offset: 0x0003FB6C
		// (set) Token: 0x06001277 RID: 4727 RVA: 0x00041984 File Offset: 0x0003FB84
		internal float pixelsPerPoint
		{
			get
			{
				return this.m_PixelsPerPoint;
			}
			set
			{
				bool flag = !Mathf.Approximately(this.m_PixelsPerPoint, value);
				if (flag)
				{
					this.m_PixelsPerPoint = value;
					this.visualTree.IncrementVersion(VersionChangeType.Layout);
					this.yogaConfig.PointScaleFactor = this.scaledPixelsPerPoint;
					this.visualTree.IncrementVersion(VersionChangeType.StyleSheet);
				}
			}
		}

		// Token: 0x170003DB RID: 987
		// (get) Token: 0x06001278 RID: 4728 RVA: 0x000419DC File Offset: 0x0003FBDC
		public float scaledPixelsPerPoint
		{
			get
			{
				return this.m_PixelsPerPoint * this.m_Scale;
			}
		}

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x06001279 RID: 4729 RVA: 0x000419FB File Offset: 0x0003FBFB
		// (set) Token: 0x0600127A RID: 4730 RVA: 0x00041A03 File Offset: 0x0003FC03
		public float referenceSpritePixelsPerUnit { get; set; } = 100f;

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x0600127B RID: 4731 RVA: 0x00041A0C File Offset: 0x0003FC0C
		// (set) Token: 0x0600127C RID: 4732 RVA: 0x00041A4C File Offset: 0x0003FC4C
		public PanelClearFlags clearFlags
		{
			get
			{
				PanelClearFlags panelClearFlags = PanelClearFlags.None;
				bool clearColor = this.clearSettings.clearColor;
				if (clearColor)
				{
					panelClearFlags |= PanelClearFlags.Color;
				}
				bool clearDepthStencil = this.clearSettings.clearDepthStencil;
				if (clearDepthStencil)
				{
					panelClearFlags |= PanelClearFlags.Depth;
				}
				return panelClearFlags;
			}
			set
			{
				PanelClearSettings clearSettings = this.clearSettings;
				clearSettings.clearColor = (value & PanelClearFlags.Color) == PanelClearFlags.Color;
				clearSettings.clearDepthStencil = (value & PanelClearFlags.Depth) == PanelClearFlags.Depth;
				this.clearSettings = clearSettings;
			}
		}

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x0600127D RID: 4733 RVA: 0x00041A83 File Offset: 0x0003FC83
		// (set) Token: 0x0600127E RID: 4734 RVA: 0x00041A8B File Offset: 0x0003FC8B
		internal PanelClearSettings clearSettings { get; set; } = new PanelClearSettings
		{
			clearDepthStencil = true,
			clearColor = true,
			color = Color.clear
		};

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x0600127F RID: 4735 RVA: 0x00041A94 File Offset: 0x0003FC94
		// (set) Token: 0x06001280 RID: 4736 RVA: 0x00041A9C File Offset: 0x0003FC9C
		internal bool duringLayoutPhase { get; set; }

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x06001281 RID: 4737 RVA: 0x00041AA8 File Offset: 0x0003FCA8
		public bool isDirty
		{
			get
			{
				return this.version != this.repaintVersion;
			}
		}

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x06001282 RID: 4738
		internal abstract uint version { get; }

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x06001283 RID: 4739
		internal abstract uint repaintVersion { get; }

		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x06001284 RID: 4740
		internal abstract uint hierarchyVersion { get; }

		// Token: 0x06001285 RID: 4741
		internal abstract void OnVersionChanged(VisualElement ele, VersionChangeType changeTypeFlag);

		// Token: 0x06001286 RID: 4742
		internal abstract void SetUpdater(IVisualTreeUpdater updater, VisualTreeUpdatePhase phase);

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x06001287 RID: 4743 RVA: 0x00041ACB File Offset: 0x0003FCCB
		// (set) Token: 0x06001288 RID: 4744 RVA: 0x00041AD3 File Offset: 0x0003FCD3
		internal virtual RepaintData repaintData { get; set; }

		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x06001289 RID: 4745 RVA: 0x00041ADC File Offset: 0x0003FCDC
		// (set) Token: 0x0600128A RID: 4746 RVA: 0x00041AE4 File Offset: 0x0003FCE4
		internal virtual ICursorManager cursorManager { get; set; }

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x0600128B RID: 4747 RVA: 0x00041AED File Offset: 0x0003FCED
		// (set) Token: 0x0600128C RID: 4748 RVA: 0x00041AF5 File Offset: 0x0003FCF5
		public ContextualMenuManager contextualMenuManager { get; internal set; }

		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x0600128D RID: 4749
		public abstract VisualElement visualTree { get; }

		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x0600128E RID: 4750
		// (set) Token: 0x0600128F RID: 4751
		public abstract EventDispatcher dispatcher { get; set; }

		// Token: 0x06001290 RID: 4752 RVA: 0x00041AFE File Offset: 0x0003FCFE
		internal void SendEvent(EventBase e, DispatchMode dispatchMode = DispatchMode.Default)
		{
			Debug.Assert(this.dispatcher != null);
			EventDispatcher dispatcher = this.dispatcher;
			if (dispatcher != null)
			{
				dispatcher.Dispatch(e, this, dispatchMode);
			}
		}

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x06001291 RID: 4753
		internal abstract IScheduler scheduler { get; }

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x06001292 RID: 4754
		// (set) Token: 0x06001293 RID: 4755
		internal abstract IStylePropertyAnimationSystem styleAnimationSystem { get; set; }

		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x06001294 RID: 4756
		// (set) Token: 0x06001295 RID: 4757
		public abstract ContextType contextType { get; protected set; }

		// Token: 0x06001296 RID: 4758
		public abstract VisualElement Pick(Vector2 point);

		// Token: 0x06001297 RID: 4759
		public abstract VisualElement PickAll(Vector2 point, List<VisualElement> picked);

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x06001298 RID: 4760 RVA: 0x00041B25 File Offset: 0x0003FD25
		// (set) Token: 0x06001299 RID: 4761 RVA: 0x00041B2D File Offset: 0x0003FD2D
		internal bool disposed { get; private set; }

		// Token: 0x0600129A RID: 4762
		internal abstract IVisualTreeUpdater GetUpdater(VisualTreeUpdatePhase phase);

		// Token: 0x0600129B RID: 4763 RVA: 0x00041B38 File Offset: 0x0003FD38
		internal VisualElement GetTopElementUnderPointer(int pointerId)
		{
			return this.m_TopElementUnderPointers.GetTopElementUnderPointer(pointerId);
		}

		// Token: 0x0600129C RID: 4764 RVA: 0x00041B58 File Offset: 0x0003FD58
		internal VisualElement RecomputeTopElementUnderPointer(int pointerId, Vector2 pointerPos, EventBase triggerEvent)
		{
			VisualElement visualElement = null;
			bool flag = PointerDeviceState.GetPanel(pointerId, this.contextType) == this && !PointerDeviceState.HasLocationFlag(pointerId, this.contextType, PointerDeviceState.LocationFlag.OutsidePanel);
			if (flag)
			{
				visualElement = this.Pick(pointerPos);
			}
			this.m_TopElementUnderPointers.SetElementUnderPointer(visualElement, pointerId, triggerEvent);
			return visualElement;
		}

		// Token: 0x0600129D RID: 4765 RVA: 0x00041BAC File Offset: 0x0003FDAC
		internal void ClearCachedElementUnderPointer(int pointerId, EventBase triggerEvent)
		{
			this.m_TopElementUnderPointers.SetTemporaryElementUnderPointer(null, pointerId, triggerEvent);
		}

		// Token: 0x0600129E RID: 4766 RVA: 0x00041BBE File Offset: 0x0003FDBE
		internal void CommitElementUnderPointers()
		{
			this.m_TopElementUnderPointers.CommitElementUnderPointers(this.dispatcher, this.contextType);
		}

		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x0600129F RID: 4767
		// (set) Token: 0x060012A0 RID: 4768
		internal abstract Shader standardShader { get; set; }

		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x060012A1 RID: 4769 RVA: 0x00041BDC File Offset: 0x0003FDDC
		// (set) Token: 0x060012A2 RID: 4770 RVA: 0x00003CD2 File Offset: 0x00001ED2
		internal virtual Shader standardWorldSpaceShader
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x14000036 RID: 54
		// (add) Token: 0x060012A3 RID: 4771 RVA: 0x00041BF0 File Offset: 0x0003FDF0
		// (remove) Token: 0x060012A4 RID: 4772 RVA: 0x00041C28 File Offset: 0x0003FE28
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal event Action standardShaderChanged;

		// Token: 0x14000037 RID: 55
		// (add) Token: 0x060012A5 RID: 4773 RVA: 0x00041C60 File Offset: 0x0003FE60
		// (remove) Token: 0x060012A6 RID: 4774 RVA: 0x00041C98 File Offset: 0x0003FE98
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal event Action standardWorldSpaceShaderChanged;

		// Token: 0x060012A7 RID: 4775 RVA: 0x00041CD0 File Offset: 0x0003FED0
		protected void InvokeStandardShaderChanged()
		{
			bool flag = this.standardShaderChanged != null;
			if (flag)
			{
				this.standardShaderChanged();
			}
		}

		// Token: 0x060012A8 RID: 4776 RVA: 0x00041CF8 File Offset: 0x0003FEF8
		protected void InvokeStandardWorldSpaceShaderChanged()
		{
			bool flag = this.standardWorldSpaceShaderChanged != null;
			if (flag)
			{
				this.standardWorldSpaceShaderChanged();
			}
		}

		// Token: 0x14000038 RID: 56
		// (add) Token: 0x060012A9 RID: 4777 RVA: 0x00041D20 File Offset: 0x0003FF20
		// (remove) Token: 0x060012AA RID: 4778 RVA: 0x00041D58 File Offset: 0x0003FF58
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal event Action atlasChanged;

		// Token: 0x060012AB RID: 4779 RVA: 0x00041D8D File Offset: 0x0003FF8D
		protected void InvokeAtlasChanged()
		{
			Action action = this.atlasChanged;
			if (action != null)
			{
				action();
			}
		}

		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x060012AC RID: 4780
		// (set) Token: 0x060012AD RID: 4781
		public abstract AtlasBase atlas { get; set; }

		// Token: 0x14000039 RID: 57
		// (add) Token: 0x060012AE RID: 4782 RVA: 0x00041DA4 File Offset: 0x0003FFA4
		// (remove) Token: 0x060012AF RID: 4783 RVA: 0x00041DDC File Offset: 0x0003FFDC
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal event Action<Material> updateMaterial;

		// Token: 0x060012B0 RID: 4784 RVA: 0x00041E11 File Offset: 0x00040011
		internal void InvokeUpdateMaterial(Material mat)
		{
			Action<Material> action = this.updateMaterial;
			if (action != null)
			{
				action(mat);
			}
		}

		// Token: 0x1400003A RID: 58
		// (add) Token: 0x060012B1 RID: 4785 RVA: 0x00041E28 File Offset: 0x00040028
		// (remove) Token: 0x060012B2 RID: 4786 RVA: 0x00041E60 File Offset: 0x00040060
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal event HierarchyEvent hierarchyChanged;

		// Token: 0x060012B3 RID: 4787 RVA: 0x00041E98 File Offset: 0x00040098
		internal void InvokeHierarchyChanged(VisualElement ve, HierarchyChangeType changeType)
		{
			bool flag = this.hierarchyChanged != null;
			if (flag)
			{
				this.hierarchyChanged(ve, changeType);
			}
		}

		// Token: 0x1400003B RID: 59
		// (add) Token: 0x060012B4 RID: 4788 RVA: 0x00041EC4 File Offset: 0x000400C4
		// (remove) Token: 0x060012B5 RID: 4789 RVA: 0x00041EFC File Offset: 0x000400FC
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal event Action<IPanel> beforeUpdate;

		// Token: 0x060012B6 RID: 4790 RVA: 0x00041F31 File Offset: 0x00040131
		internal void InvokeBeforeUpdate()
		{
			Action<IPanel> action = this.beforeUpdate;
			if (action != null)
			{
				action(this);
			}
		}

		// Token: 0x060012B7 RID: 4791 RVA: 0x00041F48 File Offset: 0x00040148
		internal void UpdateElementUnderPointers()
		{
			foreach (int num in PointerId.hoveringPointers)
			{
				bool flag = PointerDeviceState.GetPanel(num, this.contextType) != this || PointerDeviceState.HasLocationFlag(num, this.contextType, PointerDeviceState.LocationFlag.OutsidePanel);
				if (flag)
				{
					this.m_TopElementUnderPointers.SetElementUnderPointer(null, num, new Vector2(float.MinValue, float.MinValue));
				}
				else
				{
					Vector2 pointerPosition = PointerDeviceState.GetPointerPosition(num, this.contextType);
					VisualElement visualElement = this.PickAll(pointerPosition, null);
					this.m_TopElementUnderPointers.SetElementUnderPointer(visualElement, num, pointerPosition);
				}
			}
			this.CommitElementUnderPointers();
		}

		// Token: 0x060012B8 RID: 4792 RVA: 0x00003CD2 File Offset: 0x00001ED2
		void IGroupBox.OnOptionAdded(IGroupBoxOption option)
		{
		}

		// Token: 0x060012B9 RID: 4793 RVA: 0x00003CD2 File Offset: 0x00001ED2
		void IGroupBox.OnOptionRemoved(IGroupBoxOption option)
		{
		}

		// Token: 0x060012BA RID: 4794 RVA: 0x00041FE8 File Offset: 0x000401E8
		internal virtual IGenericMenu CreateMenu()
		{
			return new GenericDropdownMenu();
		}

		// Token: 0x060012BB RID: 4795 RVA: 0x00041FEF File Offset: 0x000401EF
		public virtual void Update()
		{
			this.scheduler.UpdateScheduledEvents();
			this.ValidateFocus();
			this.ValidateLayout();
			this.UpdateAnimations();
			this.UpdateBindings();
		}

		// Token: 0x04000877 RID: 2167
		private UIElementsBridge m_UIElementsBridge;

		// Token: 0x04000878 RID: 2168
		private float m_Scale = 1f;

		// Token: 0x04000879 RID: 2169
		internal YogaConfig yogaConfig;

		// Token: 0x0400087A RID: 2170
		private float m_PixelsPerPoint = 1f;

		// Token: 0x04000882 RID: 2178
		internal ElementUnderPointer m_TopElementUnderPointers = new ElementUnderPointer();
	}
}
