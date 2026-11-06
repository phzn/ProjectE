using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Profiling;

namespace UnityEngine.UIElements
{
	// Token: 0x02000298 RID: 664
	internal class Panel : BaseVisualElementPanel
	{
		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x060012CC RID: 4812 RVA: 0x0004201C File Offset: 0x0004021C
		public sealed override VisualElement visualTree
		{
			get
			{
				return this.m_RootContainer;
			}
		}

		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x060012CD RID: 4813 RVA: 0x00042034 File Offset: 0x00040234
		// (set) Token: 0x060012CE RID: 4814 RVA: 0x0004203C File Offset: 0x0004023C
		public sealed override EventDispatcher dispatcher { get; set; }

		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x060012CF RID: 4815 RVA: 0x00042048 File Offset: 0x00040248
		public TimerEventScheduler timerEventScheduler
		{
			get
			{
				TimerEventScheduler timerEventScheduler;
				if ((timerEventScheduler = this.m_Scheduler) == null)
				{
					timerEventScheduler = (this.m_Scheduler = new TimerEventScheduler());
				}
				return timerEventScheduler;
			}
		}

		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x060012D0 RID: 4816 RVA: 0x00042074 File Offset: 0x00040274
		internal override IScheduler scheduler
		{
			get
			{
				return this.timerEventScheduler;
			}
		}

		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x060012D1 RID: 4817 RVA: 0x0004208C File Offset: 0x0004028C
		internal VisualTreeUpdater visualTreeUpdater
		{
			get
			{
				return this.m_VisualTreeUpdater;
			}
		}

		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x060012D2 RID: 4818 RVA: 0x000420A4 File Offset: 0x000402A4
		// (set) Token: 0x060012D3 RID: 4819 RVA: 0x000420AC File Offset: 0x000402AC
		internal override IStylePropertyAnimationSystem styleAnimationSystem
		{
			get
			{
				return this.m_StylePropertyAnimationSystem;
			}
			set
			{
				bool flag = this.m_StylePropertyAnimationSystem == value;
				if (!flag)
				{
					IStylePropertyAnimationSystem stylePropertyAnimationSystem = this.m_StylePropertyAnimationSystem;
					if (stylePropertyAnimationSystem != null)
					{
						stylePropertyAnimationSystem.CancelAllAnimations();
					}
					this.m_StylePropertyAnimationSystem = value;
				}
			}
		}

		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x060012D4 RID: 4820 RVA: 0x000420E2 File Offset: 0x000402E2
		// (set) Token: 0x060012D5 RID: 4821 RVA: 0x000420EA File Offset: 0x000402EA
		public override ScriptableObject ownerObject { get; protected set; }

		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x060012D6 RID: 4822 RVA: 0x000420F3 File Offset: 0x000402F3
		// (set) Token: 0x060012D7 RID: 4823 RVA: 0x000420FB File Offset: 0x000402FB
		public override ContextType contextType { get; protected set; }

		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x060012D8 RID: 4824 RVA: 0x00042104 File Offset: 0x00040304
		// (set) Token: 0x060012D9 RID: 4825 RVA: 0x0004210C File Offset: 0x0004030C
		public override SavePersistentViewData saveViewData { get; set; }

		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x060012DA RID: 4826 RVA: 0x00042115 File Offset: 0x00040315
		// (set) Token: 0x060012DB RID: 4827 RVA: 0x0004211D File Offset: 0x0004031D
		public override GetViewDataDictionary getViewDataDictionary { get; set; }

		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x060012DC RID: 4828 RVA: 0x00042126 File Offset: 0x00040326
		// (set) Token: 0x060012DD RID: 4829 RVA: 0x0004212E File Offset: 0x0004032E
		public sealed override FocusController focusController { get; set; }

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x060012DE RID: 4830 RVA: 0x00042137 File Offset: 0x00040337
		// (set) Token: 0x060012DF RID: 4831 RVA: 0x0004213F File Offset: 0x0004033F
		public override EventInterests IMGUIEventInterests { get; set; }

		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x060012E0 RID: 4832 RVA: 0x00042148 File Offset: 0x00040348
		// (set) Token: 0x060012E1 RID: 4833 RVA: 0x0004214F File Offset: 0x0004034F
		internal static LoadResourceFunction loadResourceFunc { private get; set; }

		// Token: 0x060012E2 RID: 4834 RVA: 0x00042158 File Offset: 0x00040358
		internal static Object LoadResource(string pathName, Type type, float dpiScaling)
		{
			bool flag = Panel.loadResourceFunc != null;
			Object @object;
			if (flag)
			{
				@object = Panel.loadResourceFunc(pathName, type, dpiScaling);
			}
			else
			{
				@object = Resources.Load(pathName, type);
			}
			return @object;
		}

		// Token: 0x060012E3 RID: 4835 RVA: 0x00042195 File Offset: 0x00040395
		internal void Focus()
		{
			this.m_JustReceivedFocus = true;
		}

		// Token: 0x060012E4 RID: 4836 RVA: 0x0004219F File Offset: 0x0004039F
		internal void Blur()
		{
			FocusController focusController = this.focusController;
			if (focusController != null)
			{
				focusController.BlurLastFocusedElement();
			}
		}

		// Token: 0x060012E5 RID: 4837 RVA: 0x000421B4 File Offset: 0x000403B4
		public override void ValidateFocus()
		{
			bool justReceivedFocus = this.m_JustReceivedFocus;
			if (justReceivedFocus)
			{
				this.m_JustReceivedFocus = false;
				FocusController focusController = this.focusController;
				if (focusController != null)
				{
					focusController.SetFocusToLastFocusedElement();
				}
			}
		}

		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x060012E6 RID: 4838 RVA: 0x000421E8 File Offset: 0x000403E8
		// (set) Token: 0x060012E7 RID: 4839 RVA: 0x00042200 File Offset: 0x00040400
		internal string name
		{
			get
			{
				return this.m_PanelName;
			}
			set
			{
				this.m_PanelName = value;
				this.CreateMarkers();
			}
		}

		// Token: 0x060012E8 RID: 4840 RVA: 0x00042214 File Offset: 0x00040414
		private void CreateMarkers()
		{
			bool flag = !string.IsNullOrEmpty(this.m_PanelName);
			if (flag)
			{
				this.m_MarkerBeforeUpdate = new ProfilerMarker("Panel.BeforeUpdate." + this.m_PanelName);
				this.m_MarkerUpdate = new ProfilerMarker("Panel.Update." + this.m_PanelName);
				this.m_MarkerLayout = new ProfilerMarker("Panel.Layout." + this.m_PanelName);
				this.m_MarkerBindings = new ProfilerMarker("Panel.Bindings." + this.m_PanelName);
				this.m_MarkerAnimations = new ProfilerMarker("Panel.Animations." + this.m_PanelName);
			}
			else
			{
				this.m_MarkerBeforeUpdate = new ProfilerMarker("Panel.BeforeUpdate");
				this.m_MarkerUpdate = new ProfilerMarker("Panel.Update");
				this.m_MarkerLayout = new ProfilerMarker("Panel.Layout");
				this.m_MarkerBindings = new ProfilerMarker("Panel.Bindings");
				this.m_MarkerAnimations = new ProfilerMarker("Panel.Animations");
			}
		}

		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x060012E9 RID: 4841 RVA: 0x00042314 File Offset: 0x00040514
		// (set) Token: 0x060012EA RID: 4842 RVA: 0x0004231B File Offset: 0x0004051B
		internal static TimeMsFunction TimeSinceStartup { private get; set; }

		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x060012EB RID: 4843 RVA: 0x00042323 File Offset: 0x00040523
		// (set) Token: 0x060012EC RID: 4844 RVA: 0x0004232B File Offset: 0x0004052B
		public override int IMGUIContainersCount { get; set; }

		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x060012ED RID: 4845 RVA: 0x00042334 File Offset: 0x00040534
		// (set) Token: 0x060012EE RID: 4846 RVA: 0x0004233C File Offset: 0x0004053C
		public override IMGUIContainer rootIMGUIContainer { get; set; }

		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x060012EF RID: 4847 RVA: 0x00042345 File Offset: 0x00040545
		internal override uint version
		{
			get
			{
				return this.m_Version;
			}
		}

		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x060012F0 RID: 4848 RVA: 0x0004234D File Offset: 0x0004054D
		internal override uint repaintVersion
		{
			get
			{
				return this.m_RepaintVersion;
			}
		}

		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x060012F1 RID: 4849 RVA: 0x00042355 File Offset: 0x00040555
		internal override uint hierarchyVersion
		{
			get
			{
				return this.m_HierarchyVersion;
			}
		}

		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x060012F2 RID: 4850 RVA: 0x00042360 File Offset: 0x00040560
		// (set) Token: 0x060012F3 RID: 4851 RVA: 0x00042378 File Offset: 0x00040578
		internal override Shader standardShader
		{
			get
			{
				return this.m_StandardShader;
			}
			set
			{
				bool flag = this.m_StandardShader != value;
				if (flag)
				{
					this.m_StandardShader = value;
					base.InvokeStandardShaderChanged();
				}
			}
		}

		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x060012F4 RID: 4852 RVA: 0x000423A8 File Offset: 0x000405A8
		// (set) Token: 0x060012F5 RID: 4853 RVA: 0x000423C0 File Offset: 0x000405C0
		public override AtlasBase atlas
		{
			get
			{
				return this.m_Atlas;
			}
			set
			{
				bool flag = this.m_Atlas != value;
				if (flag)
				{
					AtlasBase atlas = this.m_Atlas;
					if (atlas != null)
					{
						atlas.InvokeRemovedFromPanel(this);
					}
					this.m_Atlas = value;
					base.InvokeAtlasChanged();
					AtlasBase atlas2 = this.m_Atlas;
					if (atlas2 != null)
					{
						atlas2.InvokeAssignedToPanel(this);
					}
				}
			}
		}

		// Token: 0x060012F6 RID: 4854 RVA: 0x00042414 File Offset: 0x00040614
		internal static Panel CreateEditorPanel(ScriptableObject ownerObject)
		{
			return new Panel(ownerObject, ContextType.Editor, EventDispatcher.CreateDefault());
		}

		// Token: 0x060012F7 RID: 4855 RVA: 0x00042434 File Offset: 0x00040634
		public Panel(ScriptableObject ownerObject, ContextType contextType, EventDispatcher dispatcher)
		{
			this.ownerObject = ownerObject;
			this.contextType = contextType;
			this.dispatcher = dispatcher;
			this.repaintData = new RepaintData();
			this.cursorManager = new CursorManager();
			base.contextualMenuManager = null;
			this.m_VisualTreeUpdater = new VisualTreeUpdater(this);
			this.m_RootContainer = new VisualElement
			{
				name = VisualElementUtils.GetUniqueName("unity-panel-container"),
				viewDataKey = "PanelContainer",
				pickingMode = ((contextType == ContextType.Editor) ? PickingMode.Position : PickingMode.Ignore),
				eventCallbackCategories = int.MinValue
			};
			this.visualTree.SetPanel(this);
			this.focusController = new FocusController(new VisualElementFocusRing(this.visualTree, VisualElementFocusRing.DefaultFocusOrder.ChildOrder));
			this.styleAnimationSystem = new StylePropertyAnimationSystem();
			this.CreateMarkers();
			base.InvokeHierarchyChanged(this.visualTree, HierarchyChangeType.Add);
			this.atlas = new DynamicAtlas();
		}

		// Token: 0x060012F8 RID: 4856 RVA: 0x00042540 File Offset: 0x00040740
		protected override void Dispose(bool disposing)
		{
			bool disposed = base.disposed;
			if (!disposed)
			{
				if (disposing)
				{
					this.atlas = null;
					this.m_VisualTreeUpdater.Dispose();
				}
				base.Dispose(disposing);
			}
		}

		// Token: 0x060012F9 RID: 4857 RVA: 0x00042580 File Offset: 0x00040780
		public static long TimeSinceStartupMs()
		{
			TimeMsFunction timeSinceStartup = Panel.TimeSinceStartup;
			return (timeSinceStartup != null) ? timeSinceStartup() : Panel.DefaultTimeSinceStartupMs();
		}

		// Token: 0x060012FA RID: 4858 RVA: 0x000425A8 File Offset: 0x000407A8
		internal static long DefaultTimeSinceStartupMs()
		{
			return (long)(Time.realtimeSinceStartup * 1000f);
		}

		// Token: 0x060012FB RID: 4859 RVA: 0x000425C8 File Offset: 0x000407C8
		internal static VisualElement PickAllWithoutValidatingLayout(VisualElement root, Vector2 point)
		{
			return Panel.PickAll(root, point, null, false);
		}

		// Token: 0x060012FC RID: 4860 RVA: 0x000425E4 File Offset: 0x000407E4
		private static VisualElement PickAll(VisualElement root, Vector2 point, List<VisualElement> picked = null, bool includeIgnoredElement = false)
		{
			return Panel.PerformPick(root, point, picked, includeIgnoredElement);
		}

		// Token: 0x060012FD RID: 4861 RVA: 0x00042604 File Offset: 0x00040804
		private static VisualElement PerformPick(VisualElement root, Vector2 point, List<VisualElement> picked = null, bool includeIgnoredElement = false)
		{
			bool flag = root.resolvedStyle.display == DisplayStyle.None;
			VisualElement visualElement;
			if (flag)
			{
				visualElement = null;
			}
			else
			{
				bool flag2 = root.pickingMode == PickingMode.Ignore && root.hierarchy.childCount == 0 && !includeIgnoredElement;
				if (flag2)
				{
					visualElement = null;
				}
				else
				{
					bool flag3 = !root.worldBoundingBox.Contains(point);
					if (flag3)
					{
						visualElement = null;
					}
					else
					{
						Vector2 vector = root.WorldToLocal(point);
						bool flag4 = root.ContainsPoint(vector);
						bool flag5 = !flag4 && root.ShouldClip();
						if (flag5)
						{
							visualElement = null;
						}
						else
						{
							VisualElement visualElement2 = null;
							int childCount = root.hierarchy.childCount;
							for (int i = childCount - 1; i >= 0; i--)
							{
								VisualElement visualElement3 = root.hierarchy[i];
								VisualElement visualElement4 = Panel.PerformPick(visualElement3, point, picked, includeIgnoredElement);
								bool flag6 = visualElement2 == null && visualElement4 != null;
								if (flag6)
								{
									bool flag7 = picked == null;
									if (flag7)
									{
										return visualElement4;
									}
									visualElement2 = visualElement4;
								}
							}
							bool flag8 = root.visible && (root.pickingMode == PickingMode.Position || includeIgnoredElement) && flag4;
							if (flag8)
							{
								if (picked != null)
								{
									picked.Add(root);
								}
								bool flag9 = visualElement2 == null;
								if (flag9)
								{
									visualElement2 = root;
								}
							}
							visualElement = visualElement2;
						}
					}
				}
			}
			return visualElement;
		}

		// Token: 0x060012FE RID: 4862 RVA: 0x00042760 File Offset: 0x00040960
		public override VisualElement PickAll(Vector2 point, List<VisualElement> picked)
		{
			this.ValidateLayout();
			bool flag = picked != null;
			if (flag)
			{
				picked.Clear();
			}
			return Panel.PickAll(this.visualTree, point, picked, false);
		}

		// Token: 0x060012FF RID: 4863 RVA: 0x00042798 File Offset: 0x00040998
		public override VisualElement Pick(Vector2 point)
		{
			this.ValidateLayout();
			Vector2 vector;
			bool flag;
			VisualElement topElementUnderPointer = this.m_TopElementUnderPointers.GetTopElementUnderPointer(PointerId.mousePointerId, out vector, out flag);
			bool flag2 = !flag && Panel.<Pick>g__PixelOf|101_0(vector) == Panel.<Pick>g__PixelOf|101_0(point);
			VisualElement visualElement;
			if (flag2)
			{
				visualElement = topElementUnderPointer;
			}
			else
			{
				visualElement = Panel.PickAll(this.visualTree, point, null, false);
			}
			return visualElement;
		}

		// Token: 0x06001300 RID: 4864 RVA: 0x000427F8 File Offset: 0x000409F8
		public override void ValidateLayout()
		{
			bool flag = !this.m_ValidatingLayout;
			if (flag)
			{
				this.m_ValidatingLayout = true;
				this.m_VisualTreeUpdater.UpdateVisualTreePhase(VisualTreeUpdatePhase.Styles);
				this.m_VisualTreeUpdater.UpdateVisualTreePhase(VisualTreeUpdatePhase.Layout);
				this.m_VisualTreeUpdater.UpdateVisualTreePhase(VisualTreeUpdatePhase.TransformClip);
				this.m_ValidatingLayout = false;
			}
		}

		// Token: 0x06001301 RID: 4865 RVA: 0x0004284A File Offset: 0x00040A4A
		public override void UpdateAnimations()
		{
			this.m_VisualTreeUpdater.UpdateVisualTreePhase(VisualTreeUpdatePhase.Animation);
		}

		// Token: 0x06001302 RID: 4866 RVA: 0x0004285A File Offset: 0x00040A5A
		public override void UpdateBindings()
		{
			this.m_VisualTreeUpdater.UpdateVisualTreePhase(VisualTreeUpdatePhase.Bindings);
		}

		// Token: 0x06001303 RID: 4867 RVA: 0x0004286A File Offset: 0x00040A6A
		public override void ApplyStyles()
		{
			this.m_VisualTreeUpdater.UpdateVisualTreePhase(VisualTreeUpdatePhase.Styles);
		}

		// Token: 0x06001304 RID: 4868 RVA: 0x0004287C File Offset: 0x00040A7C
		private void UpdateForRepaint()
		{
			this.m_VisualTreeUpdater.UpdateVisualTreePhase(VisualTreeUpdatePhase.ViewData);
			this.m_VisualTreeUpdater.UpdateVisualTreePhase(VisualTreeUpdatePhase.Styles);
			this.m_VisualTreeUpdater.UpdateVisualTreePhase(VisualTreeUpdatePhase.Layout);
			this.m_VisualTreeUpdater.UpdateVisualTreePhase(VisualTreeUpdatePhase.TransformClip);
			this.m_VisualTreeUpdater.UpdateVisualTreePhase(VisualTreeUpdatePhase.Repaint);
		}

		// Token: 0x06001305 RID: 4869 RVA: 0x000428CC File Offset: 0x00040ACC
		internal void UpdateWithoutRepaint()
		{
			this.m_VisualTreeUpdater.UpdateVisualTreePhase(VisualTreeUpdatePhase.ViewData);
			this.m_VisualTreeUpdater.UpdateVisualTreePhase(VisualTreeUpdatePhase.Bindings);
			this.m_VisualTreeUpdater.UpdateVisualTreePhase(VisualTreeUpdatePhase.Animation);
			this.m_VisualTreeUpdater.UpdateVisualTreePhase(VisualTreeUpdatePhase.Styles);
			this.m_VisualTreeUpdater.UpdateVisualTreePhase(VisualTreeUpdatePhase.Layout);
		}

		// Token: 0x1400003C RID: 60
		// (add) Token: 0x06001306 RID: 4870 RVA: 0x0004291C File Offset: 0x00040B1C
		// (remove) Token: 0x06001307 RID: 4871 RVA: 0x00042950 File Offset: 0x00040B50
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal static event Action<Panel> beforeAnyRepaint;

		// Token: 0x06001308 RID: 4872 RVA: 0x00042984 File Offset: 0x00040B84
		public override void Repaint(Event e)
		{
			this.m_RepaintVersion = this.version;
			bool flag = this.contextType == ContextType.Editor;
			if (flag)
			{
				base.pixelsPerPoint = GUIUtility.pixelsPerPoint;
			}
			this.repaintData.repaintEvent = e;
			using (this.m_MarkerBeforeUpdate.Auto())
			{
				base.InvokeBeforeUpdate();
			}
			Action<Panel> action = Panel.beforeAnyRepaint;
			if (action != null)
			{
				action(this);
			}
			using (this.m_MarkerUpdate.Auto())
			{
				this.UpdateForRepaint();
			}
		}

		// Token: 0x06001309 RID: 4873 RVA: 0x00042A40 File Offset: 0x00040C40
		internal override void OnVersionChanged(VisualElement ve, VersionChangeType versionChangeType)
		{
			this.m_Version += 1U;
			this.m_VisualTreeUpdater.OnVersionChanged(ve, versionChangeType);
			bool flag = (versionChangeType & VersionChangeType.Hierarchy) == VersionChangeType.Hierarchy;
			if (flag)
			{
				this.m_HierarchyVersion += 1U;
			}
		}

		// Token: 0x0600130A RID: 4874 RVA: 0x00042A82 File Offset: 0x00040C82
		internal override void SetUpdater(IVisualTreeUpdater updater, VisualTreeUpdatePhase phase)
		{
			this.m_VisualTreeUpdater.SetUpdater(updater, phase);
		}

		// Token: 0x0600130B RID: 4875 RVA: 0x00042A94 File Offset: 0x00040C94
		internal override IVisualTreeUpdater GetUpdater(VisualTreeUpdatePhase phase)
		{
			return this.m_VisualTreeUpdater.GetUpdater(phase);
		}

		// Token: 0x0600130D RID: 4877 RVA: 0x00042AC3 File Offset: 0x00040CC3
		[CompilerGenerated]
		internal static Vector2Int <Pick>g__PixelOf|101_0(Vector2 p)
		{
			return Vector2Int.FloorToInt(p);
		}

		// Token: 0x04000889 RID: 2185
		private VisualElement m_RootContainer;

		// Token: 0x0400088A RID: 2186
		private VisualTreeUpdater m_VisualTreeUpdater;

		// Token: 0x0400088B RID: 2187
		private IStylePropertyAnimationSystem m_StylePropertyAnimationSystem;

		// Token: 0x0400088C RID: 2188
		private string m_PanelName;

		// Token: 0x0400088D RID: 2189
		private uint m_Version = 0U;

		// Token: 0x0400088E RID: 2190
		private uint m_RepaintVersion = 0U;

		// Token: 0x0400088F RID: 2191
		private uint m_HierarchyVersion = 0U;

		// Token: 0x04000890 RID: 2192
		private ProfilerMarker m_MarkerBeforeUpdate;

		// Token: 0x04000891 RID: 2193
		private ProfilerMarker m_MarkerUpdate;

		// Token: 0x04000892 RID: 2194
		private ProfilerMarker m_MarkerLayout;

		// Token: 0x04000893 RID: 2195
		private ProfilerMarker m_MarkerBindings;

		// Token: 0x04000894 RID: 2196
		private ProfilerMarker m_MarkerAnimations;

		// Token: 0x04000895 RID: 2197
		private static ProfilerMarker s_MarkerPickAll = new ProfilerMarker("Panel.PickAll");

		// Token: 0x04000897 RID: 2199
		private TimerEventScheduler m_Scheduler;

		// Token: 0x0400089F RID: 2207
		private bool m_JustReceivedFocus;

		// Token: 0x040008A3 RID: 2211
		private Shader m_StandardShader;

		// Token: 0x040008A4 RID: 2212
		private AtlasBase m_Atlas;

		// Token: 0x040008A5 RID: 2213
		private bool m_ValidatingLayout = false;
	}
}
