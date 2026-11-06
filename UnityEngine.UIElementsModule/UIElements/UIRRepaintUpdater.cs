using System;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine.UIElements.UIR;

namespace UnityEngine.UIElements
{
	// Token: 0x020002B3 RID: 691
	internal class UIRRepaintUpdater : BaseVisualTreeUpdater
	{
		// Token: 0x060013F7 RID: 5111 RVA: 0x00047036 File Offset: 0x00045236
		public UIRRepaintUpdater()
		{
			base.panelChanged += this.OnPanelChanged;
		}

		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x060013F8 RID: 5112 RVA: 0x00047053 File Offset: 0x00045253
		public override ProfilerMarker profilerMarker
		{
			get
			{
				return UIRRepaintUpdater.s_ProfilerMarker;
			}
		}

		// Token: 0x17000439 RID: 1081
		// (get) Token: 0x060013F9 RID: 5113 RVA: 0x0004705A File Offset: 0x0004525A
		// (set) Token: 0x060013FA RID: 5114 RVA: 0x00047062 File Offset: 0x00045262
		public bool drawStats { get; set; }

		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x060013FB RID: 5115 RVA: 0x0004706B File Offset: 0x0004526B
		// (set) Token: 0x060013FC RID: 5116 RVA: 0x00047073 File Offset: 0x00045273
		public bool breakBatches { get; set; }

		// Token: 0x060013FD RID: 5117 RVA: 0x0004707C File Offset: 0x0004527C
		public override void OnVersionChanged(VisualElement ve, VersionChangeType versionChangeType)
		{
			bool flag = this.renderChain == null;
			if (!flag)
			{
				bool flag2 = (versionChangeType & VersionChangeType.Transform) > (VersionChangeType)0;
				bool flag3 = (versionChangeType & VersionChangeType.Size) > (VersionChangeType)0;
				bool flag4 = (versionChangeType & VersionChangeType.Overflow) > (VersionChangeType)0;
				bool flag5 = (versionChangeType & VersionChangeType.BorderRadius) > (VersionChangeType)0;
				bool flag6 = (versionChangeType & VersionChangeType.BorderWidth) > (VersionChangeType)0;
				bool flag7 = (versionChangeType & VersionChangeType.RenderHints) > (VersionChangeType)0;
				bool flag8 = flag7;
				if (flag8)
				{
					this.renderChain.UIEOnRenderHintsChanged(ve);
				}
				bool flag9 = flag2 || flag3 || flag6;
				if (flag9)
				{
					this.renderChain.UIEOnTransformOrSizeChanged(ve, flag2, flag3 || flag6);
				}
				bool flag10 = flag4 || flag5;
				if (flag10)
				{
					this.renderChain.UIEOnClippingChanged(ve, false);
				}
				bool flag11 = (versionChangeType & VersionChangeType.Opacity) > (VersionChangeType)0;
				if (flag11)
				{
					this.renderChain.UIEOnOpacityChanged(ve, false);
				}
				bool flag12 = (versionChangeType & VersionChangeType.Color) > (VersionChangeType)0;
				if (flag12)
				{
					this.renderChain.UIEOnColorChanged(ve);
				}
				bool flag13 = (versionChangeType & VersionChangeType.Repaint) > (VersionChangeType)0;
				if (flag13)
				{
					this.renderChain.UIEOnVisualsChanged(ve, false);
				}
			}
		}

		// Token: 0x060013FE RID: 5118 RVA: 0x00047184 File Offset: 0x00045384
		public override void Update()
		{
			bool flag = this.renderChain == null;
			if (flag)
			{
				this.InitRenderChain();
			}
			bool flag2 = this.renderChain == null || this.renderChain.device == null;
			if (!flag2)
			{
				this.renderChain.ProcessChanges();
				PanelClearSettings clearSettings = base.panel.clearSettings;
				bool flag3 = clearSettings.clearColor || clearSettings.clearDepthStencil;
				if (flag3)
				{
					Color color = clearSettings.color;
					color = color.RGBMultiplied(color.a);
					GL.Clear(clearSettings.clearDepthStencil, clearSettings.clearColor, color, 0.99f);
				}
				this.renderChain.drawStats = this.drawStats;
				this.renderChain.device.breakBatches = this.breakBatches;
				this.renderChain.Render();
			}
		}

		// Token: 0x060013FF RID: 5119 RVA: 0x00047260 File Offset: 0x00045460
		protected virtual RenderChain CreateRenderChain()
		{
			return new RenderChain(base.panel);
		}

		// Token: 0x06001400 RID: 5120 RVA: 0x0004727D File Offset: 0x0004547D
		static UIRRepaintUpdater()
		{
			Utility.GraphicsResourcesRecreate += UIRRepaintUpdater.OnGraphicsResourcesRecreate;
		}

		// Token: 0x06001401 RID: 5121 RVA: 0x000472AC File Offset: 0x000454AC
		private static void OnGraphicsResourcesRecreate(bool recreate)
		{
			bool flag = !recreate;
			if (flag)
			{
				UIRenderDevice.PrepareForGfxDeviceRecreate();
			}
			Dictionary<int, Panel>.Enumerator panelsIterator = UIElementsUtility.GetPanelsIterator();
			while (panelsIterator.MoveNext())
			{
				if (recreate)
				{
					KeyValuePair<int, Panel> keyValuePair = panelsIterator.Current;
					AtlasBase atlas = keyValuePair.Value.atlas;
					if (atlas != null)
					{
						atlas.Reset();
					}
				}
				else
				{
					KeyValuePair<int, Panel> keyValuePair = panelsIterator.Current;
					UIRRepaintUpdater uirrepaintUpdater = keyValuePair.Value.GetUpdater(VisualTreeUpdatePhase.Repaint) as UIRRepaintUpdater;
					if (uirrepaintUpdater != null)
					{
						uirrepaintUpdater.DestroyRenderChain();
					}
				}
			}
			bool flag2 = !recreate;
			if (flag2)
			{
				UIRenderDevice.FlushAllPendingDeviceDisposes();
			}
			else
			{
				UIRenderDevice.WrapUpGfxDeviceRecreate();
			}
		}

		// Token: 0x06001402 RID: 5122 RVA: 0x00047342 File Offset: 0x00045542
		private void OnPanelChanged(BaseVisualElementPanel obj)
		{
			this.DetachFromPanel();
			this.AttachToPanel();
		}

		// Token: 0x06001403 RID: 5123 RVA: 0x00047354 File Offset: 0x00045554
		private void AttachToPanel()
		{
			Debug.Assert(this.attachedPanel == null);
			bool flag = base.panel == null;
			if (!flag)
			{
				this.attachedPanel = base.panel;
				this.attachedPanel.atlasChanged += this.OnPanelAtlasChanged;
				this.attachedPanel.standardShaderChanged += this.OnPanelStandardShaderChanged;
				this.attachedPanel.standardWorldSpaceShaderChanged += this.OnPanelStandardWorldSpaceShaderChanged;
				this.attachedPanel.hierarchyChanged += this.OnPanelHierarchyChanged;
			}
		}

		// Token: 0x06001404 RID: 5124 RVA: 0x000473EC File Offset: 0x000455EC
		private void DetachFromPanel()
		{
			bool flag = this.attachedPanel == null;
			if (!flag)
			{
				this.DestroyRenderChain();
				this.attachedPanel.atlasChanged -= this.OnPanelAtlasChanged;
				this.attachedPanel.standardShaderChanged -= this.OnPanelStandardShaderChanged;
				this.attachedPanel.standardWorldSpaceShaderChanged -= this.OnPanelStandardWorldSpaceShaderChanged;
				this.attachedPanel.hierarchyChanged -= this.OnPanelHierarchyChanged;
				this.attachedPanel = null;
			}
		}

		// Token: 0x06001405 RID: 5125 RVA: 0x00047478 File Offset: 0x00045678
		private void InitRenderChain()
		{
			this.renderChain = this.CreateRenderChain();
			BaseVisualElementPanel baseVisualElementPanel = this.attachedPanel;
			bool flag = ((baseVisualElementPanel != null) ? baseVisualElementPanel.visualTree : null) != null;
			if (flag)
			{
				this.renderChain.UIEOnChildAdded(this.attachedPanel.visualTree);
			}
			this.OnPanelStandardShaderChanged();
			bool flag2 = base.panel.contextType == ContextType.Player;
			if (flag2)
			{
				this.OnPanelStandardWorldSpaceShaderChanged();
			}
		}

		// Token: 0x06001406 RID: 5126 RVA: 0x000474E4 File Offset: 0x000456E4
		internal void DestroyRenderChain()
		{
			bool flag = this.renderChain == null;
			if (!flag)
			{
				this.renderChain.Dispose();
				this.renderChain = null;
				this.ResetAllElementsDataRecursive(this.attachedPanel.visualTree);
			}
		}

		// Token: 0x06001407 RID: 5127 RVA: 0x00047526 File Offset: 0x00045726
		private void OnPanelAtlasChanged()
		{
			this.DestroyRenderChain();
		}

		// Token: 0x06001408 RID: 5128 RVA: 0x00047530 File Offset: 0x00045730
		private void OnPanelHierarchyChanged(VisualElement ve, HierarchyChangeType changeType)
		{
			bool flag = this.renderChain == null;
			if (!flag)
			{
				switch (changeType)
				{
				case HierarchyChangeType.Add:
					this.renderChain.UIEOnChildAdded(ve);
					break;
				case HierarchyChangeType.Remove:
					this.renderChain.UIEOnChildRemoving(ve);
					break;
				case HierarchyChangeType.Move:
					this.renderChain.UIEOnChildrenReordered(ve);
					break;
				}
			}
		}

		// Token: 0x06001409 RID: 5129 RVA: 0x00047594 File Offset: 0x00045794
		private void OnPanelStandardShaderChanged()
		{
			bool flag = this.renderChain == null;
			if (!flag)
			{
				Shader shader = base.panel.standardShader;
				bool flag2 = shader == null;
				if (flag2)
				{
					shader = Shader.Find(UIRUtility.k_DefaultShaderName);
					Debug.Assert(shader != null, "Failed to load UIElements default shader");
					bool flag3 = shader != null;
					if (flag3)
					{
						shader.hideFlags |= HideFlags.DontSaveInEditor;
					}
				}
				this.renderChain.defaultShader = shader;
			}
		}

		// Token: 0x0600140A RID: 5130 RVA: 0x00047610 File Offset: 0x00045810
		private void OnPanelStandardWorldSpaceShaderChanged()
		{
			bool flag = this.renderChain == null;
			if (!flag)
			{
				Shader shader = base.panel.standardWorldSpaceShader;
				bool flag2 = shader == null;
				if (flag2)
				{
					shader = Shader.Find(UIRUtility.k_DefaultWorldSpaceShaderName);
					Debug.Assert(shader != null, "Failed to load UIElements default world-space shader");
					bool flag3 = shader != null;
					if (flag3)
					{
						shader.hideFlags |= HideFlags.DontSaveInEditor;
					}
				}
				this.renderChain.defaultWorldSpaceShader = shader;
			}
		}

		// Token: 0x0600140B RID: 5131 RVA: 0x0004768C File Offset: 0x0004588C
		private void ResetAllElementsDataRecursive(VisualElement ve)
		{
			ve.renderChainData = default(RenderChainVEData);
			int i = ve.hierarchy.childCount - 1;
			while (i >= 0)
			{
				this.ResetAllElementsDataRecursive(ve.hierarchy[i--]);
			}
		}

		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x0600140C RID: 5132 RVA: 0x000476DE File Offset: 0x000458DE
		// (set) Token: 0x0600140D RID: 5133 RVA: 0x000476E6 File Offset: 0x000458E6
		private protected bool disposed { protected get; private set; }

		// Token: 0x0600140E RID: 5134 RVA: 0x000476F0 File Offset: 0x000458F0
		protected override void Dispose(bool disposing)
		{
			bool disposed = this.disposed;
			if (!disposed)
			{
				if (disposing)
				{
					this.DetachFromPanel();
				}
				this.disposed = true;
			}
		}

		// Token: 0x0400094B RID: 2379
		private BaseVisualElementPanel attachedPanel;

		// Token: 0x0400094C RID: 2380
		internal RenderChain renderChain;

		// Token: 0x0400094D RID: 2381
		private static readonly string s_Description = "Update Rendering";

		// Token: 0x0400094E RID: 2382
		private static readonly ProfilerMarker s_ProfilerMarker = new ProfilerMarker(UIRRepaintUpdater.s_Description);
	}
}
