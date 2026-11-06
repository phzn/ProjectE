using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Profiling;
using UnityEngine.UIElements.UIR.Implementation;

namespace UnityEngine.UIElements.UIR
{
	// Token: 0x02000455 RID: 1109
	internal class RenderChain : IDisposable
	{
		// Token: 0x170007E2 RID: 2018
		// (get) Token: 0x060022A3 RID: 8867 RVA: 0x00085750 File Offset: 0x00083950
		internal RenderChainCommand firstCommand
		{
			get
			{
				return this.m_FirstCommand;
			}
		}

		// Token: 0x170007E3 RID: 2019
		// (get) Token: 0x060022A4 RID: 8868 RVA: 0x00085768 File Offset: 0x00083968
		// (set) Token: 0x060022A5 RID: 8869 RVA: 0x00085770 File Offset: 0x00083970
		public OpacityIdAccelerator opacityIdAccelerator { get; private set; }

		// Token: 0x060022A6 RID: 8870 RVA: 0x0008577C File Offset: 0x0008397C
		static RenderChain()
		{
			Utility.RegisterIntermediateRenderers += RenderChain.OnRegisterIntermediateRenderers;
			Utility.RenderNodeExecute += RenderChain.OnRenderNodeExecute;
		}

		// Token: 0x060022A7 RID: 8871 RVA: 0x00085820 File Offset: 0x00083A20
		public RenderChain(BaseVisualElementPanel panel)
		{
			this.Constructor(panel, new UIRenderDevice(0U, 0U), panel.atlas, new VectorImageManager(panel.atlas));
		}

		// Token: 0x060022A8 RID: 8872 RVA: 0x000858DC File Offset: 0x00083ADC
		protected RenderChain(BaseVisualElementPanel panel, UIRenderDevice device, AtlasBase atlas, VectorImageManager vectorImageManager)
		{
			this.Constructor(panel, device, atlas, vectorImageManager);
		}

		// Token: 0x060022A9 RID: 8873 RVA: 0x00085984 File Offset: 0x00083B84
		private void Constructor(BaseVisualElementPanel panelObj, UIRenderDevice deviceObj, AtlasBase atlas, VectorImageManager vectorImageMan)
		{
			bool disposed = this.disposed;
			if (disposed)
			{
				DisposeHelper.NotifyDisposedUsed(this);
			}
			this.m_DirtyTracker.heads = new List<VisualElement>(8);
			this.m_DirtyTracker.tails = new List<VisualElement>(8);
			this.m_DirtyTracker.minDepths = new int[5];
			this.m_DirtyTracker.maxDepths = new int[5];
			this.m_DirtyTracker.Reset();
			bool flag = this.m_RenderNodesData.Count < 1;
			if (flag)
			{
				this.m_RenderNodesData.Add(new RenderChain.RenderNodeData
				{
					matPropBlock = new MaterialPropertyBlock()
				});
			}
			this.panel = panelObj;
			this.device = deviceObj;
			this.atlas = atlas;
			this.vectorImageManager = vectorImageMan;
			this.vertsPool = new TempAllocator<Vertex>(8192, 2048, 65536);
			this.indicesPool = new TempAllocator<ushort>(16384, 4096, 131072);
			this.jobManager = new JobManager();
			this.shaderInfoAllocator.Construct();
			this.opacityIdAccelerator = new OpacityIdAccelerator();
			this.painter = new UIRStylePainter(this);
			BaseRuntimePanel baseRuntimePanel = this.panel as BaseRuntimePanel;
			bool flag2 = baseRuntimePanel != null && baseRuntimePanel.drawToCameras;
			if (flag2)
			{
				this.drawInCameras = true;
				this.m_StaticIndex = RenderChain.RenderChainStaticIndexAllocator.AllocateIndex(this);
			}
		}

		// Token: 0x060022AA RID: 8874 RVA: 0x00085AE4 File Offset: 0x00083CE4
		private void Destructor()
		{
			bool flag = this.m_StaticIndex >= 0;
			if (flag)
			{
				RenderChain.RenderChainStaticIndexAllocator.FreeIndex(this.m_StaticIndex);
			}
			this.m_StaticIndex = -1;
			RenderChainCommand firstCommand = this.m_FirstCommand;
			for (VisualElement visualElement = RenderChain.GetFirstElementInPanel((firstCommand != null) ? firstCommand.owner : null); visualElement != null; visualElement = visualElement.renderChainData.next)
			{
				this.ResetTextures(visualElement);
			}
			UIRUtility.Destroy(this.m_DefaultMat);
			UIRUtility.Destroy(this.m_DefaultWorldSpaceMat);
			this.m_DefaultMat = (this.m_DefaultWorldSpaceMat = null);
			this.vertsPool.Dispose();
			this.indicesPool.Dispose();
			this.jobManager.Dispose();
			VectorImageManager vectorImageManager = this.vectorImageManager;
			if (vectorImageManager != null)
			{
				vectorImageManager.Dispose();
			}
			this.shaderInfoAllocator.Dispose();
			UIRenderDevice device = this.device;
			if (device != null)
			{
				device.Dispose();
			}
			OpacityIdAccelerator opacityIdAccelerator = this.opacityIdAccelerator;
			if (opacityIdAccelerator != null)
			{
				opacityIdAccelerator.Dispose();
			}
			bool flag2 = this.painter != null;
			if (flag2)
			{
				bool hasPainter2D = this.painter.meshGenerationContext.hasPainter2D;
				if (hasPainter2D)
				{
					this.painter.meshGenerationContext.painter2D.Dispose();
				}
				this.painter = null;
			}
			this.atlas = null;
			this.shaderInfoAllocator = default(UIRVEShaderInfoAllocator);
			this.device = null;
			this.m_ActiveRenderNodes = 0;
			this.m_RenderNodesData.Clear();
		}

		// Token: 0x170007E4 RID: 2020
		// (get) Token: 0x060022AB RID: 8875 RVA: 0x00085C51 File Offset: 0x00083E51
		// (set) Token: 0x060022AC RID: 8876 RVA: 0x00085C59 File Offset: 0x00083E59
		private protected bool disposed { protected get; private set; }

		// Token: 0x060022AD RID: 8877 RVA: 0x00085C62 File Offset: 0x00083E62
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060022AE RID: 8878 RVA: 0x00085C74 File Offset: 0x00083E74
		protected void Dispose(bool disposing)
		{
			bool disposed = this.disposed;
			if (!disposed)
			{
				if (disposing)
				{
					this.Destructor();
				}
				this.disposed = true;
			}
		}

		// Token: 0x170007E5 RID: 2021
		// (get) Token: 0x060022AF RID: 8879 RVA: 0x00085CA4 File Offset: 0x00083EA4
		internal ChainBuilderStats stats
		{
			get
			{
				return this.m_Stats;
			}
		}

		// Token: 0x060022B0 RID: 8880 RVA: 0x00085CBC File Offset: 0x00083EBC
		public void ProcessChanges()
		{
			this.m_Stats = default(ChainBuilderStats);
			this.m_Stats.elementsAdded = this.m_Stats.elementsAdded + this.m_StatsElementsAdded;
			this.m_Stats.elementsRemoved = this.m_Stats.elementsRemoved + this.m_StatsElementsRemoved;
			this.m_StatsElementsAdded = (this.m_StatsElementsRemoved = 0U);
			this.m_DirtyTracker.dirtyID = this.m_DirtyTracker.dirtyID + 1U;
			int num = 0;
			RenderDataDirtyTypes renderDataDirtyTypes = RenderDataDirtyTypes.Clipping | RenderDataDirtyTypes.ClippingHierarchy;
			RenderDataDirtyTypes renderDataDirtyTypes2 = ~renderDataDirtyTypes;
			for (int i = this.m_DirtyTracker.minDepths[num]; i <= this.m_DirtyTracker.maxDepths[num]; i++)
			{
				VisualElement visualElement = this.m_DirtyTracker.heads[i];
				while (visualElement != null)
				{
					VisualElement nextDirty = visualElement.renderChainData.nextDirty;
					bool flag = (visualElement.renderChainData.dirtiedValues & renderDataDirtyTypes) > RenderDataDirtyTypes.None;
					if (flag)
					{
						bool flag2 = visualElement.renderChainData.isInChain && visualElement.renderChainData.dirtyID != this.m_DirtyTracker.dirtyID;
						if (flag2)
						{
							RenderEvents.ProcessOnClippingChanged(this, visualElement, this.m_DirtyTracker.dirtyID, ref this.m_Stats);
						}
						this.m_DirtyTracker.ClearDirty(visualElement, renderDataDirtyTypes2);
					}
					visualElement = nextDirty;
					this.m_Stats.dirtyProcessed = this.m_Stats.dirtyProcessed + 1U;
				}
			}
			this.m_DirtyTracker.dirtyID = this.m_DirtyTracker.dirtyID + 1U;
			num = 1;
			renderDataDirtyTypes = RenderDataDirtyTypes.Opacity | RenderDataDirtyTypes.OpacityHierarchy;
			renderDataDirtyTypes2 = ~renderDataDirtyTypes;
			for (int j = this.m_DirtyTracker.minDepths[num]; j <= this.m_DirtyTracker.maxDepths[num]; j++)
			{
				VisualElement visualElement2 = this.m_DirtyTracker.heads[j];
				while (visualElement2 != null)
				{
					VisualElement nextDirty2 = visualElement2.renderChainData.nextDirty;
					bool flag3 = (visualElement2.renderChainData.dirtiedValues & renderDataDirtyTypes) > RenderDataDirtyTypes.None;
					if (flag3)
					{
						bool flag4 = visualElement2.renderChainData.isInChain && visualElement2.renderChainData.dirtyID != this.m_DirtyTracker.dirtyID;
						if (flag4)
						{
							RenderEvents.ProcessOnOpacityChanged(this, visualElement2, this.m_DirtyTracker.dirtyID, ref this.m_Stats);
						}
						this.m_DirtyTracker.ClearDirty(visualElement2, renderDataDirtyTypes2);
					}
					visualElement2 = nextDirty2;
					this.m_Stats.dirtyProcessed = this.m_Stats.dirtyProcessed + 1U;
				}
			}
			this.m_DirtyTracker.dirtyID = this.m_DirtyTracker.dirtyID + 1U;
			num = 2;
			renderDataDirtyTypes = RenderDataDirtyTypes.Color;
			renderDataDirtyTypes2 = ~renderDataDirtyTypes;
			for (int k = this.m_DirtyTracker.minDepths[num]; k <= this.m_DirtyTracker.maxDepths[num]; k++)
			{
				VisualElement visualElement3 = this.m_DirtyTracker.heads[k];
				while (visualElement3 != null)
				{
					VisualElement nextDirty3 = visualElement3.renderChainData.nextDirty;
					bool flag5 = (visualElement3.renderChainData.dirtiedValues & renderDataDirtyTypes) > RenderDataDirtyTypes.None;
					if (flag5)
					{
						bool flag6 = visualElement3.renderChainData.isInChain && visualElement3.renderChainData.dirtyID != this.m_DirtyTracker.dirtyID;
						if (flag6)
						{
							RenderEvents.ProcessOnColorChanged(this, visualElement3, this.m_DirtyTracker.dirtyID, ref this.m_Stats);
						}
						this.m_DirtyTracker.ClearDirty(visualElement3, renderDataDirtyTypes2);
					}
					visualElement3 = nextDirty3;
					this.m_Stats.dirtyProcessed = this.m_Stats.dirtyProcessed + 1U;
				}
			}
			this.m_DirtyTracker.dirtyID = this.m_DirtyTracker.dirtyID + 1U;
			num = 3;
			renderDataDirtyTypes = RenderDataDirtyTypes.Transform | RenderDataDirtyTypes.ClipRectSize;
			renderDataDirtyTypes2 = ~renderDataDirtyTypes;
			for (int l = this.m_DirtyTracker.minDepths[num]; l <= this.m_DirtyTracker.maxDepths[num]; l++)
			{
				VisualElement visualElement4 = this.m_DirtyTracker.heads[l];
				while (visualElement4 != null)
				{
					VisualElement nextDirty4 = visualElement4.renderChainData.nextDirty;
					bool flag7 = (visualElement4.renderChainData.dirtiedValues & renderDataDirtyTypes) > RenderDataDirtyTypes.None;
					if (flag7)
					{
						bool flag8 = visualElement4.renderChainData.isInChain && visualElement4.renderChainData.dirtyID != this.m_DirtyTracker.dirtyID;
						if (flag8)
						{
							RenderEvents.ProcessOnTransformOrSizeChanged(this, visualElement4, this.m_DirtyTracker.dirtyID, ref this.m_Stats);
						}
						this.m_DirtyTracker.ClearDirty(visualElement4, renderDataDirtyTypes2);
					}
					visualElement4 = nextDirty4;
					this.m_Stats.dirtyProcessed = this.m_Stats.dirtyProcessed + 1U;
				}
			}
			this.jobManager.CompleteNudgeJobs();
			this.m_BlockDirtyRegistration = true;
			this.m_DirtyTracker.dirtyID = this.m_DirtyTracker.dirtyID + 1U;
			num = 4;
			renderDataDirtyTypes = RenderDataDirtyTypes.AllVisuals;
			renderDataDirtyTypes2 = ~renderDataDirtyTypes;
			for (int m = this.m_DirtyTracker.minDepths[num]; m <= this.m_DirtyTracker.maxDepths[num]; m++)
			{
				VisualElement visualElement5 = this.m_DirtyTracker.heads[m];
				while (visualElement5 != null)
				{
					VisualElement nextDirty5 = visualElement5.renderChainData.nextDirty;
					bool flag9 = (visualElement5.renderChainData.dirtiedValues & renderDataDirtyTypes) > RenderDataDirtyTypes.None;
					if (flag9)
					{
						bool flag10 = visualElement5.renderChainData.isInChain && visualElement5.renderChainData.dirtyID != this.m_DirtyTracker.dirtyID;
						if (flag10)
						{
							RenderEvents.ProcessOnVisualsChanged(this, visualElement5, this.m_DirtyTracker.dirtyID, ref this.m_Stats);
						}
						this.m_DirtyTracker.ClearDirty(visualElement5, renderDataDirtyTypes2);
					}
					visualElement5 = nextDirty5;
					this.m_Stats.dirtyProcessed = this.m_Stats.dirtyProcessed + 1U;
				}
			}
			this.jobManager.CompleteConvertMeshJobs();
			this.jobManager.CompleteClosingMeshJobs();
			this.opacityIdAccelerator.CompleteJobs();
			this.m_BlockDirtyRegistration = false;
			this.vertsPool.Reset();
			this.indicesPool.Reset();
			this.m_DirtyTracker.Reset();
			AtlasBase atlas = this.atlas;
			if (atlas != null)
			{
				atlas.InvokeUpdateDynamicTextures(this.panel);
			}
			VectorImageManager vectorImageManager = this.vectorImageManager;
			if (vectorImageManager != null)
			{
				vectorImageManager.Commit();
			}
			this.shaderInfoAllocator.IssuePendingStorageChanges();
			UIRenderDevice device = this.device;
			if (device != null)
			{
				device.OnFrameRenderingBegin();
			}
		}

		// Token: 0x060022B1 RID: 8881 RVA: 0x00086314 File Offset: 0x00084514
		public void Render()
		{
			Material standardMaterial = this.GetStandardMaterial();
			this.panel.InvokeUpdateMaterial(standardMaterial);
			Exception ex = null;
			bool flag = this.m_FirstCommand != null;
			if (flag)
			{
				bool flag2 = !this.drawInCameras;
				if (flag2)
				{
					Rect layout = this.panel.visualTree.layout;
					if (standardMaterial != null)
					{
						standardMaterial.SetPass(0);
					}
					Matrix4x4 matrix4x = ProjectionUtils.Ortho(layout.xMin, layout.xMax, layout.yMax, layout.yMin, -0.001f, 1.001f);
					GL.LoadProjectionMatrix(matrix4x);
					GL.modelview = Matrix4x4.identity;
					UIRenderDevice device = this.device;
					RenderChainCommand firstCommand = this.m_FirstCommand;
					Material material = standardMaterial;
					Material material2 = standardMaterial;
					VectorImageManager vectorImageManager = this.vectorImageManager;
					device.EvaluateChain(firstCommand, material, material2, (vectorImageManager != null) ? vectorImageManager.atlas : null, this.shaderInfoAllocator.atlas, this.panel.scaledPixelsPerPoint, this.shaderInfoAllocator.transformConstants, this.shaderInfoAllocator.clipRectConstants, this.m_RenderNodesData[0].matPropBlock, true, ref ex);
				}
			}
			bool flag3 = ex != null;
			if (!flag3)
			{
				bool drawStats = this.drawStats;
				if (drawStats)
				{
					this.DrawStats();
				}
				return;
			}
			bool flag4 = GUIUtility.IsExitGUIException(ex);
			if (flag4)
			{
				throw ex;
			}
			throw new ImmediateModeException(ex);
		}

		// Token: 0x060022B2 RID: 8882 RVA: 0x00086454 File Offset: 0x00084654
		public void UIEOnChildAdded(VisualElement ve)
		{
			VisualElement parent = ve.hierarchy.parent;
			int num = ((parent != null) ? parent.hierarchy.IndexOf(ve) : 0);
			bool blockDirtyRegistration = this.m_BlockDirtyRegistration;
			if (blockDirtyRegistration)
			{
				throw new InvalidOperationException("VisualElements cannot be added to an active visual tree during generateVisualContent callback execution nor during visual tree rendering");
			}
			bool flag = parent != null && !parent.renderChainData.isInChain;
			if (!flag)
			{
				uint num2 = RenderEvents.DepthFirstOnChildAdded(this, parent, ve, num, true);
				Debug.Assert(ve.renderChainData.isInChain);
				Debug.Assert(ve.panel == this.panel);
				this.UIEOnClippingChanged(ve, true);
				this.UIEOnOpacityChanged(ve, false);
				this.UIEOnVisualsChanged(ve, true);
				this.m_StatsElementsAdded += num2;
			}
		}

		// Token: 0x060022B3 RID: 8883 RVA: 0x00086514 File Offset: 0x00084714
		public void UIEOnChildrenReordered(VisualElement ve)
		{
			bool blockDirtyRegistration = this.m_BlockDirtyRegistration;
			if (blockDirtyRegistration)
			{
				throw new InvalidOperationException("VisualElements cannot be moved under an active visual tree during generateVisualContent callback execution nor during visual tree rendering");
			}
			int childCount = ve.hierarchy.childCount;
			for (int i = 0; i < childCount; i++)
			{
				RenderEvents.DepthFirstOnChildRemoving(this, ve.hierarchy[i]);
			}
			for (int j = 0; j < childCount; j++)
			{
				RenderEvents.DepthFirstOnChildAdded(this, ve, ve.hierarchy[j], j, false);
			}
			this.UIEOnClippingChanged(ve, true);
			this.UIEOnOpacityChanged(ve, true);
			this.UIEOnVisualsChanged(ve, true);
		}

		// Token: 0x060022B4 RID: 8884 RVA: 0x000865BC File Offset: 0x000847BC
		public void UIEOnChildRemoving(VisualElement ve)
		{
			bool blockDirtyRegistration = this.m_BlockDirtyRegistration;
			if (blockDirtyRegistration)
			{
				throw new InvalidOperationException("VisualElements cannot be removed from an active visual tree during generateVisualContent callback execution nor during visual tree rendering");
			}
			this.m_StatsElementsRemoved += RenderEvents.DepthFirstOnChildRemoving(this, ve);
			Debug.Assert(!ve.renderChainData.isInChain);
		}

		// Token: 0x060022B5 RID: 8885 RVA: 0x00086608 File Offset: 0x00084808
		public void UIEOnRenderHintsChanged(VisualElement ve)
		{
			bool isInChain = ve.renderChainData.isInChain;
			if (isInChain)
			{
				bool blockDirtyRegistration = this.m_BlockDirtyRegistration;
				if (blockDirtyRegistration)
				{
					throw new InvalidOperationException("Render Hints cannot change under an active visual tree during generateVisualContent callback execution nor during visual tree rendering");
				}
				bool flag = (ve.renderHints & RenderHints.DirtyAll) == RenderHints.DirtyDynamicColor;
				bool flag2 = flag;
				if (flag2)
				{
					this.UIEOnVisualsChanged(ve, false);
				}
				else
				{
					this.UIEOnChildRemoving(ve);
					this.UIEOnChildAdded(ve);
				}
				ve.MarkRenderHintsClean();
			}
		}

		// Token: 0x060022B6 RID: 8886 RVA: 0x0008667C File Offset: 0x0008487C
		public void UIEOnClippingChanged(VisualElement ve, bool hierarchical)
		{
			bool isInChain = ve.renderChainData.isInChain;
			if (isInChain)
			{
				bool blockDirtyRegistration = this.m_BlockDirtyRegistration;
				if (blockDirtyRegistration)
				{
					throw new InvalidOperationException("VisualElements cannot change clipping state under an active visual tree during generateVisualContent callback execution nor during visual tree rendering");
				}
				this.m_DirtyTracker.RegisterDirty(ve, RenderDataDirtyTypes.Clipping | (hierarchical ? RenderDataDirtyTypes.ClippingHierarchy : RenderDataDirtyTypes.None), RenderDataDirtyTypeClasses.Clipping);
			}
		}

		// Token: 0x060022B7 RID: 8887 RVA: 0x000866C8 File Offset: 0x000848C8
		public void UIEOnOpacityChanged(VisualElement ve, bool hierarchical = false)
		{
			bool isInChain = ve.renderChainData.isInChain;
			if (isInChain)
			{
				bool blockDirtyRegistration = this.m_BlockDirtyRegistration;
				if (blockDirtyRegistration)
				{
					throw new InvalidOperationException("VisualElements cannot change opacity under an active visual tree during generateVisualContent callback execution nor during visual tree rendering");
				}
				this.m_DirtyTracker.RegisterDirty(ve, RenderDataDirtyTypes.Opacity | (hierarchical ? RenderDataDirtyTypes.OpacityHierarchy : RenderDataDirtyTypes.None), RenderDataDirtyTypeClasses.Opacity);
			}
		}

		// Token: 0x060022B8 RID: 8888 RVA: 0x0008671C File Offset: 0x0008491C
		public void UIEOnColorChanged(VisualElement ve)
		{
			bool isInChain = ve.renderChainData.isInChain;
			if (isInChain)
			{
				bool blockDirtyRegistration = this.m_BlockDirtyRegistration;
				if (blockDirtyRegistration)
				{
					throw new InvalidOperationException("VisualElements cannot change background color under an active visual tree during generateVisualContent callback execution nor during visual tree rendering");
				}
				this.m_DirtyTracker.RegisterDirty(ve, RenderDataDirtyTypes.Color, RenderDataDirtyTypeClasses.Color);
			}
		}

		// Token: 0x060022B9 RID: 8889 RVA: 0x00086764 File Offset: 0x00084964
		public void UIEOnTransformOrSizeChanged(VisualElement ve, bool transformChanged, bool clipRectSizeChanged)
		{
			bool isInChain = ve.renderChainData.isInChain;
			if (isInChain)
			{
				bool blockDirtyRegistration = this.m_BlockDirtyRegistration;
				if (blockDirtyRegistration)
				{
					throw new InvalidOperationException("VisualElements cannot change size or transform under an active visual tree during generateVisualContent callback execution nor during visual tree rendering");
				}
				RenderDataDirtyTypes renderDataDirtyTypes = (transformChanged ? RenderDataDirtyTypes.Transform : RenderDataDirtyTypes.None) | (clipRectSizeChanged ? RenderDataDirtyTypes.ClipRectSize : RenderDataDirtyTypes.None);
				this.m_DirtyTracker.RegisterDirty(ve, renderDataDirtyTypes, RenderDataDirtyTypeClasses.TransformSize);
			}
		}

		// Token: 0x060022BA RID: 8890 RVA: 0x000867B8 File Offset: 0x000849B8
		public void UIEOnVisualsChanged(VisualElement ve, bool hierarchical)
		{
			bool isInChain = ve.renderChainData.isInChain;
			if (isInChain)
			{
				bool blockDirtyRegistration = this.m_BlockDirtyRegistration;
				if (blockDirtyRegistration)
				{
					throw new InvalidOperationException("VisualElements cannot be marked for dirty repaint under an active visual tree during generateVisualContent callback execution nor during visual tree rendering");
				}
				this.m_DirtyTracker.RegisterDirty(ve, RenderDataDirtyTypes.Visuals | (hierarchical ? RenderDataDirtyTypes.VisualsHierarchy : RenderDataDirtyTypes.None), RenderDataDirtyTypeClasses.Visuals);
			}
		}

		// Token: 0x060022BB RID: 8891 RVA: 0x00086808 File Offset: 0x00084A08
		public void UIEOnOpacityIdChanged(VisualElement ve)
		{
			bool isInChain = ve.renderChainData.isInChain;
			if (isInChain)
			{
				bool blockDirtyRegistration = this.m_BlockDirtyRegistration;
				if (blockDirtyRegistration)
				{
					throw new InvalidOperationException("VisualElements cannot for opacity id change under an active visual tree during generateVisualContent callback execution nor during visual tree rendering");
				}
				this.m_DirtyTracker.RegisterDirty(ve, RenderDataDirtyTypes.VisualsOpacityId, RenderDataDirtyTypeClasses.Visuals);
			}
		}

		// Token: 0x170007E6 RID: 2022
		// (get) Token: 0x060022BC RID: 8892 RVA: 0x0008684C File Offset: 0x00084A4C
		// (set) Token: 0x060022BD RID: 8893 RVA: 0x00086854 File Offset: 0x00084A54
		internal BaseVisualElementPanel panel { get; private set; }

		// Token: 0x170007E7 RID: 2023
		// (get) Token: 0x060022BE RID: 8894 RVA: 0x0008685D File Offset: 0x00084A5D
		// (set) Token: 0x060022BF RID: 8895 RVA: 0x00086865 File Offset: 0x00084A65
		internal UIRenderDevice device { get; private set; }

		// Token: 0x170007E8 RID: 2024
		// (get) Token: 0x060022C0 RID: 8896 RVA: 0x0008686E File Offset: 0x00084A6E
		// (set) Token: 0x060022C1 RID: 8897 RVA: 0x00086876 File Offset: 0x00084A76
		internal AtlasBase atlas { get; private set; }

		// Token: 0x170007E9 RID: 2025
		// (get) Token: 0x060022C2 RID: 8898 RVA: 0x0008687F File Offset: 0x00084A7F
		// (set) Token: 0x060022C3 RID: 8899 RVA: 0x00086887 File Offset: 0x00084A87
		internal VectorImageManager vectorImageManager { get; private set; }

		// Token: 0x170007EA RID: 2026
		// (get) Token: 0x060022C4 RID: 8900 RVA: 0x00086890 File Offset: 0x00084A90
		// (set) Token: 0x060022C5 RID: 8901 RVA: 0x00086898 File Offset: 0x00084A98
		internal TempAllocator<Vertex> vertsPool { get; private set; }

		// Token: 0x170007EB RID: 2027
		// (get) Token: 0x060022C6 RID: 8902 RVA: 0x000868A1 File Offset: 0x00084AA1
		// (set) Token: 0x060022C7 RID: 8903 RVA: 0x000868A9 File Offset: 0x00084AA9
		internal TempAllocator<ushort> indicesPool { get; private set; }

		// Token: 0x170007EC RID: 2028
		// (get) Token: 0x060022C8 RID: 8904 RVA: 0x000868B2 File Offset: 0x00084AB2
		// (set) Token: 0x060022C9 RID: 8905 RVA: 0x000868BA File Offset: 0x00084ABA
		internal JobManager jobManager { get; private set; }

		// Token: 0x170007ED RID: 2029
		// (get) Token: 0x060022CA RID: 8906 RVA: 0x000868C3 File Offset: 0x00084AC3
		// (set) Token: 0x060022CB RID: 8907 RVA: 0x000868CB File Offset: 0x00084ACB
		internal UIRStylePainter painter { get; private set; }

		// Token: 0x170007EE RID: 2030
		// (get) Token: 0x060022CC RID: 8908 RVA: 0x000868D4 File Offset: 0x00084AD4
		// (set) Token: 0x060022CD RID: 8909 RVA: 0x000868DC File Offset: 0x00084ADC
		internal bool drawStats { get; set; }

		// Token: 0x170007EF RID: 2031
		// (get) Token: 0x060022CE RID: 8910 RVA: 0x000868E5 File Offset: 0x00084AE5
		// (set) Token: 0x060022CF RID: 8911 RVA: 0x000868ED File Offset: 0x00084AED
		internal bool drawInCameras { get; private set; }

		// Token: 0x170007F0 RID: 2032
		// (get) Token: 0x060022D0 RID: 8912 RVA: 0x000868F8 File Offset: 0x00084AF8
		// (set) Token: 0x060022D1 RID: 8913 RVA: 0x00086910 File Offset: 0x00084B10
		internal Shader defaultShader
		{
			get
			{
				return this.m_DefaultShader;
			}
			set
			{
				bool flag = this.m_DefaultShader == value;
				if (!flag)
				{
					this.m_DefaultShader = value;
					UIRUtility.Destroy(this.m_DefaultMat);
					this.m_DefaultMat = null;
				}
			}
		}

		// Token: 0x170007F1 RID: 2033
		// (get) Token: 0x060022D2 RID: 8914 RVA: 0x0008694C File Offset: 0x00084B4C
		// (set) Token: 0x060022D3 RID: 8915 RVA: 0x00086964 File Offset: 0x00084B64
		internal Shader defaultWorldSpaceShader
		{
			get
			{
				return this.m_DefaultWorldSpaceShader;
			}
			set
			{
				bool flag = this.m_DefaultWorldSpaceShader == value;
				if (!flag)
				{
					this.m_DefaultWorldSpaceShader = value;
					UIRUtility.Destroy(this.m_DefaultWorldSpaceMat);
					this.m_DefaultWorldSpaceMat = null;
				}
			}
		}

		// Token: 0x060022D4 RID: 8916 RVA: 0x000869A0 File Offset: 0x00084BA0
		internal Material GetStandardMaterial()
		{
			bool flag = this.m_DefaultMat == null && this.m_DefaultShader != null;
			if (flag)
			{
				this.m_DefaultMat = new Material(this.m_DefaultShader);
				this.m_DefaultMat.hideFlags |= HideFlags.DontSaveInEditor;
			}
			return this.m_DefaultMat;
		}

		// Token: 0x060022D5 RID: 8917 RVA: 0x00086A00 File Offset: 0x00084C00
		internal Material GetStandardWorldSpaceMaterial()
		{
			bool flag = this.m_DefaultWorldSpaceMat == null && this.m_DefaultWorldSpaceShader != null;
			if (flag)
			{
				this.m_DefaultWorldSpaceMat = new Material(this.m_DefaultWorldSpaceShader);
				this.m_DefaultWorldSpaceMat.hideFlags |= HideFlags.DontSaveInEditor;
			}
			return this.m_DefaultWorldSpaceMat;
		}

		// Token: 0x060022D6 RID: 8918 RVA: 0x00086A60 File Offset: 0x00084C60
		internal void EnsureFitsDepth(int depth)
		{
			this.m_DirtyTracker.EnsureFits(depth);
		}

		// Token: 0x060022D7 RID: 8919 RVA: 0x00086A70 File Offset: 0x00084C70
		internal void ChildWillBeRemoved(VisualElement ve)
		{
			bool flag = ve.renderChainData.dirtiedValues > RenderDataDirtyTypes.None;
			if (flag)
			{
				this.m_DirtyTracker.ClearDirty(ve, ~ve.renderChainData.dirtiedValues);
			}
			Debug.Assert(ve.renderChainData.dirtiedValues == RenderDataDirtyTypes.None);
			Debug.Assert(ve.renderChainData.prevDirty == null);
			Debug.Assert(ve.renderChainData.nextDirty == null);
		}

		// Token: 0x060022D8 RID: 8920 RVA: 0x00086AE8 File Offset: 0x00084CE8
		internal RenderChainCommand AllocCommand()
		{
			RenderChainCommand renderChainCommand = this.m_CommandPool.Get();
			renderChainCommand.Reset();
			return renderChainCommand;
		}

		// Token: 0x060022D9 RID: 8921 RVA: 0x00086B10 File Offset: 0x00084D10
		internal void FreeCommand(RenderChainCommand cmd)
		{
			bool flag = cmd.state.material != null;
			if (flag)
			{
				this.m_CustomMaterialCommands--;
			}
			cmd.Reset();
			this.m_CommandPool.Return(cmd);
		}

		// Token: 0x060022DA RID: 8922 RVA: 0x00086B58 File Offset: 0x00084D58
		internal void OnRenderCommandAdded(RenderChainCommand command)
		{
			bool flag = command.prev == null;
			if (flag)
			{
				this.m_FirstCommand = command;
			}
			bool flag2 = command.state.material != null;
			if (flag2)
			{
				this.m_CustomMaterialCommands++;
			}
		}

		// Token: 0x060022DB RID: 8923 RVA: 0x00086BA0 File Offset: 0x00084DA0
		internal void OnRenderCommandsRemoved(RenderChainCommand firstCommand, RenderChainCommand lastCommand)
		{
			bool flag = firstCommand.prev == null;
			if (flag)
			{
				this.m_FirstCommand = lastCommand.next;
			}
		}

		// Token: 0x060022DC RID: 8924 RVA: 0x00086BC8 File Offset: 0x00084DC8
		private unsafe static RenderChain.RenderNodeData AccessRenderNodeData(IntPtr obj)
		{
			int* ptr = (int*)obj.ToPointer();
			RenderChain renderChain = RenderChain.RenderChainStaticIndexAllocator.AccessIndex(*ptr);
			return renderChain.m_RenderNodesData[ptr[1]];
		}

		// Token: 0x060022DD RID: 8925 RVA: 0x00086BFC File Offset: 0x00084DFC
		private static void OnRenderNodeExecute(IntPtr obj)
		{
			RenderChain.RenderNodeData renderNodeData = RenderChain.AccessRenderNodeData(obj);
			Exception ex = null;
			renderNodeData.device.EvaluateChain(renderNodeData.firstCommand, renderNodeData.initialMaterial, renderNodeData.standardMaterial, renderNodeData.vectorAtlas, renderNodeData.shaderInfoAtlas, renderNodeData.dpiScale, renderNodeData.transformConstants, renderNodeData.clipRectConstants, renderNodeData.matPropBlock, false, ref ex);
		}

		// Token: 0x060022DE RID: 8926 RVA: 0x00086C58 File Offset: 0x00084E58
		private static void OnRegisterIntermediateRenderers(Camera camera)
		{
			int num = 0;
			Dictionary<int, Panel>.Enumerator panelsIterator = UIElementsUtility.GetPanelsIterator();
			while (panelsIterator.MoveNext())
			{
				KeyValuePair<int, Panel> keyValuePair = panelsIterator.Current;
				Panel value = keyValuePair.Value;
				UIRRepaintUpdater uirrepaintUpdater = value.GetUpdater(VisualTreeUpdatePhase.Repaint) as UIRRepaintUpdater;
				RenderChain renderChain = ((uirrepaintUpdater != null) ? uirrepaintUpdater.renderChain : null);
				bool flag = renderChain == null || renderChain.m_StaticIndex < 0 || renderChain.m_FirstCommand == null;
				if (!flag)
				{
					BaseRuntimePanel baseRuntimePanel = (BaseRuntimePanel)value;
					Material standardWorldSpaceMaterial = renderChain.GetStandardWorldSpaceMaterial();
					RenderChain.RenderNodeData renderNodeData = default(RenderChain.RenderNodeData);
					renderNodeData.device = renderChain.device;
					renderNodeData.standardMaterial = standardWorldSpaceMaterial;
					VectorImageManager vectorImageManager = renderChain.vectorImageManager;
					renderNodeData.vectorAtlas = ((vectorImageManager != null) ? vectorImageManager.atlas : null);
					renderNodeData.shaderInfoAtlas = renderChain.shaderInfoAllocator.atlas;
					renderNodeData.dpiScale = baseRuntimePanel.scaledPixelsPerPoint;
					renderNodeData.transformConstants = renderChain.shaderInfoAllocator.transformConstants;
					renderNodeData.clipRectConstants = renderChain.shaderInfoAllocator.clipRectConstants;
					bool flag2 = renderChain.m_CustomMaterialCommands == 0;
					if (flag2)
					{
						renderNodeData.initialMaterial = standardWorldSpaceMaterial;
						renderNodeData.firstCommand = renderChain.m_FirstCommand;
						RenderChain.OnRegisterIntermediateRendererMat(baseRuntimePanel, renderChain, ref renderNodeData, camera, num++);
					}
					else
					{
						Material material = null;
						RenderChainCommand renderChainCommand = renderChain.m_FirstCommand;
						RenderChainCommand renderChainCommand2 = renderChainCommand;
						while (renderChainCommand != null)
						{
							bool flag3 = renderChainCommand.type > CommandType.Draw;
							if (flag3)
							{
								renderChainCommand = renderChainCommand.next;
							}
							else
							{
								Material material2 = ((renderChainCommand.state.material == null) ? standardWorldSpaceMaterial : renderChainCommand.state.material);
								bool flag4 = material2 != material;
								if (flag4)
								{
									bool flag5 = material != null;
									if (flag5)
									{
										renderNodeData.initialMaterial = material;
										renderNodeData.firstCommand = renderChainCommand2;
										RenderChain.OnRegisterIntermediateRendererMat(baseRuntimePanel, renderChain, ref renderNodeData, camera, num++);
										renderChainCommand2 = renderChainCommand;
									}
									material = material2;
								}
								renderChainCommand = renderChainCommand.next;
							}
						}
						bool flag6 = renderChainCommand2 != null;
						if (flag6)
						{
							renderNodeData.initialMaterial = material;
							renderNodeData.firstCommand = renderChainCommand2;
							RenderChain.OnRegisterIntermediateRendererMat(baseRuntimePanel, renderChain, ref renderNodeData, camera, num++);
						}
					}
				}
			}
		}

		// Token: 0x060022DF RID: 8927 RVA: 0x00086E84 File Offset: 0x00085084
		private unsafe static void OnRegisterIntermediateRendererMat(BaseRuntimePanel rtp, RenderChain renderChain, ref RenderChain.RenderNodeData rnd, Camera camera, int sameDistanceSortPriority)
		{
			int activeRenderNodes = renderChain.m_ActiveRenderNodes;
			renderChain.m_ActiveRenderNodes = activeRenderNodes + 1;
			int num = activeRenderNodes;
			bool flag = num < renderChain.m_RenderNodesData.Count;
			if (flag)
			{
				RenderChain.RenderNodeData renderNodeData = renderChain.m_RenderNodesData[num];
				rnd.matPropBlock = renderNodeData.matPropBlock;
				renderChain.m_RenderNodesData[num] = rnd;
			}
			else
			{
				rnd.matPropBlock = new MaterialPropertyBlock();
				num = renderChain.m_RenderNodesData.Count;
				renderChain.m_RenderNodesData.Add(rnd);
			}
			int* ptr = stackalloc int[(UIntPtr)8];
			*ptr = renderChain.m_StaticIndex;
			ptr[1] = num;
			Utility.RegisterIntermediateRenderer(camera, rnd.initialMaterial, rtp.panelToWorld, new Bounds(Vector3.zero, new Vector3(float.MaxValue, float.MaxValue, float.MaxValue)), 3, 0, false, sameDistanceSortPriority, (ulong)((long)camera.cullingMask), 2, new IntPtr((void*)ptr), 8);
		}

		// Token: 0x060022E0 RID: 8928 RVA: 0x00086F6C File Offset: 0x0008516C
		internal void RepaintTexturedElements()
		{
			RenderChainCommand firstCommand = this.m_FirstCommand;
			for (VisualElement visualElement = RenderChain.GetFirstElementInPanel((firstCommand != null) ? firstCommand.owner : null); visualElement != null; visualElement = visualElement.renderChainData.next)
			{
				bool flag = visualElement.renderChainData.textures != null;
				if (flag)
				{
					this.UIEOnVisualsChanged(visualElement, false);
				}
			}
			this.UIEOnOpacityChanged(this.panel.visualTree, false);
		}

		// Token: 0x060022E1 RID: 8929 RVA: 0x00086FD8 File Offset: 0x000851D8
		public void InsertTexture(VisualElement ve, Texture src, TextureId id, bool isAtlas)
		{
			BasicNode<TextureEntry> basicNode = this.m_TexturePool.Get();
			basicNode.data.source = src;
			basicNode.data.actual = id;
			basicNode.data.replaced = isAtlas;
			basicNode.InsertFirst(ref ve.renderChainData.textures);
		}

		// Token: 0x060022E2 RID: 8930 RVA: 0x0008702C File Offset: 0x0008522C
		public void ResetTextures(VisualElement ve)
		{
			AtlasBase atlas = this.atlas;
			TextureRegistry textureRegistry = this.m_TextureRegistry;
			BasicNodePool<TextureEntry> texturePool = this.m_TexturePool;
			BasicNode<TextureEntry> basicNode = ve.renderChainData.textures;
			ve.renderChainData.textures = null;
			while (basicNode != null)
			{
				BasicNode<TextureEntry> next = basicNode.next;
				bool replaced = basicNode.data.replaced;
				if (replaced)
				{
					atlas.ReturnAtlas(ve, basicNode.data.source as Texture2D, basicNode.data.actual);
				}
				else
				{
					textureRegistry.Release(basicNode.data.actual);
				}
				texturePool.Return(basicNode);
				basicNode = next;
			}
		}

		// Token: 0x060022E3 RID: 8931 RVA: 0x000870D0 File Offset: 0x000852D0
		private void DrawStats()
		{
			bool flag = this.device != null;
			float num = 12f;
			Rect rect = new Rect(30f, 60f, 1000f, 100f);
			GUI.Box(new Rect(20f, 40f, 200f, (float)(flag ? 380 : 256)), "UI Toolkit Draw Stats");
			GUI.Label(rect, "Elements added\t: " + this.m_Stats.elementsAdded.ToString());
			rect.y += num;
			GUI.Label(rect, "Elements removed\t: " + this.m_Stats.elementsRemoved.ToString());
			rect.y += num;
			GUI.Label(rect, "Mesh allocs allocated\t: " + this.m_Stats.newMeshAllocations.ToString());
			rect.y += num;
			GUI.Label(rect, "Mesh allocs updated\t: " + this.m_Stats.updatedMeshAllocations.ToString());
			rect.y += num;
			GUI.Label(rect, "Clip update roots\t: " + this.m_Stats.recursiveClipUpdates.ToString());
			rect.y += num;
			GUI.Label(rect, "Clip update total\t: " + this.m_Stats.recursiveClipUpdatesExpanded.ToString());
			rect.y += num;
			GUI.Label(rect, "Opacity update roots\t: " + this.m_Stats.recursiveOpacityUpdates.ToString());
			rect.y += num;
			GUI.Label(rect, "Opacity update total\t: " + this.m_Stats.recursiveOpacityUpdatesExpanded.ToString());
			rect.y += num;
			GUI.Label(rect, "Opacity ID update\t: " + this.m_Stats.opacityIdUpdates.ToString());
			rect.y += num;
			GUI.Label(rect, "Xform update roots\t: " + this.m_Stats.recursiveTransformUpdates.ToString());
			rect.y += num;
			GUI.Label(rect, "Xform update total\t: " + this.m_Stats.recursiveTransformUpdatesExpanded.ToString());
			rect.y += num;
			GUI.Label(rect, "Xformed by bone\t: " + this.m_Stats.boneTransformed.ToString());
			rect.y += num;
			GUI.Label(rect, "Xformed by skipping\t: " + this.m_Stats.skipTransformed.ToString());
			rect.y += num;
			GUI.Label(rect, "Xformed by nudging\t: " + this.m_Stats.nudgeTransformed.ToString());
			rect.y += num;
			GUI.Label(rect, "Xformed by repaint\t: " + this.m_Stats.visualUpdateTransformed.ToString());
			rect.y += num;
			GUI.Label(rect, "Visual update roots\t: " + this.m_Stats.recursiveVisualUpdates.ToString());
			rect.y += num;
			GUI.Label(rect, "Visual update total\t: " + this.m_Stats.recursiveVisualUpdatesExpanded.ToString());
			rect.y += num;
			GUI.Label(rect, "Visual update flats\t: " + this.m_Stats.nonRecursiveVisualUpdates.ToString());
			rect.y += num;
			GUI.Label(rect, "Dirty processed\t: " + this.m_Stats.dirtyProcessed.ToString());
			rect.y += num;
			GUI.Label(rect, "Group-xform updates\t: " + this.m_Stats.groupTransformElementsChanged.ToString());
			rect.y += num;
			bool flag2 = !flag;
			if (!flag2)
			{
				rect.y += num;
				UIRenderDevice.DrawStatistics drawStatistics = this.device.GatherDrawStatistics();
				GUI.Label(rect, "Frame index\t: " + drawStatistics.currentFrameIndex.ToString());
				rect.y += num;
				GUI.Label(rect, "Command count\t: " + drawStatistics.commandCount.ToString());
				rect.y += num;
				GUI.Label(rect, "Draw commands\t: " + drawStatistics.drawCommandCount.ToString());
				rect.y += num;
				GUI.Label(rect, "Draw ranges\t: " + drawStatistics.drawRangeCount.ToString());
				rect.y += num;
				GUI.Label(rect, "Draw range calls\t: " + drawStatistics.drawRangeCallCount.ToString());
				rect.y += num;
				GUI.Label(rect, "Material sets\t: " + drawStatistics.materialSetCount.ToString());
				rect.y += num;
				GUI.Label(rect, "Stencil changes\t: " + drawStatistics.stencilRefChanges.ToString());
				rect.y += num;
				GUI.Label(rect, "Immediate draws\t: " + drawStatistics.immediateDraws.ToString());
				rect.y += num;
				GUI.Label(rect, "Total triangles\t: " + (drawStatistics.totalIndices / 3U).ToString());
				rect.y += num;
			}
		}

		// Token: 0x060022E4 RID: 8932 RVA: 0x000876D4 File Offset: 0x000858D4
		private static VisualElement GetFirstElementInPanel(VisualElement ve)
		{
			for (;;)
			{
				bool flag;
				if (ve != null)
				{
					VisualElement prev = ve.renderChainData.prev;
					flag = prev != null && prev.renderChainData.isInChain;
				}
				else
				{
					flag = false;
				}
				if (!flag)
				{
					break;
				}
				ve = ve.renderChainData.prev;
			}
			return ve;
		}

		// Token: 0x04000F9D RID: 3997
		private RenderChainCommand m_FirstCommand;

		// Token: 0x04000F9E RID: 3998
		private RenderChain.DepthOrderedDirtyTracking m_DirtyTracker;

		// Token: 0x04000F9F RID: 3999
		private LinkedPool<RenderChainCommand> m_CommandPool = new LinkedPool<RenderChainCommand>(() => new RenderChainCommand(), delegate(RenderChainCommand cmd)
		{
		}, 10000);

		// Token: 0x04000FA0 RID: 4000
		private BasicNodePool<TextureEntry> m_TexturePool = new BasicNodePool<TextureEntry>();

		// Token: 0x04000FA1 RID: 4001
		private List<RenderChain.RenderNodeData> m_RenderNodesData = new List<RenderChain.RenderNodeData>();

		// Token: 0x04000FA2 RID: 4002
		private Shader m_DefaultShader;

		// Token: 0x04000FA3 RID: 4003
		private Shader m_DefaultWorldSpaceShader;

		// Token: 0x04000FA4 RID: 4004
		private Material m_DefaultMat;

		// Token: 0x04000FA5 RID: 4005
		private Material m_DefaultWorldSpaceMat;

		// Token: 0x04000FA6 RID: 4006
		private bool m_BlockDirtyRegistration;

		// Token: 0x04000FA7 RID: 4007
		private int m_StaticIndex = -1;

		// Token: 0x04000FA8 RID: 4008
		private int m_ActiveRenderNodes = 0;

		// Token: 0x04000FA9 RID: 4009
		private int m_CustomMaterialCommands = 0;

		// Token: 0x04000FAA RID: 4010
		private ChainBuilderStats m_Stats;

		// Token: 0x04000FAB RID: 4011
		private uint m_StatsElementsAdded;

		// Token: 0x04000FAC RID: 4012
		private uint m_StatsElementsRemoved;

		// Token: 0x04000FAD RID: 4013
		private TextureRegistry m_TextureRegistry = TextureRegistry.instance;

		// Token: 0x04000FAF RID: 4015
		private static ProfilerMarker s_MarkerProcess = new ProfilerMarker("RenderChain.Process");

		// Token: 0x04000FB0 RID: 4016
		private static ProfilerMarker s_MarkerClipProcessing = new ProfilerMarker("RenderChain.UpdateClips");

		// Token: 0x04000FB1 RID: 4017
		private static ProfilerMarker s_MarkerOpacityProcessing = new ProfilerMarker("RenderChain.UpdateOpacity");

		// Token: 0x04000FB2 RID: 4018
		private static ProfilerMarker s_MarkerColorsProcessing = new ProfilerMarker("RenderChain.UpdateColors");

		// Token: 0x04000FB3 RID: 4019
		private static ProfilerMarker s_MarkerTransformProcessing = new ProfilerMarker("RenderChain.UpdateTransforms");

		// Token: 0x04000FB4 RID: 4020
		private static ProfilerMarker s_MarkerVisualsProcessing = new ProfilerMarker("RenderChain.UpdateVisuals");

		// Token: 0x04000FB5 RID: 4021
		private static ProfilerMarker s_MarkerTextRegen = new ProfilerMarker("RenderChain.RegenText");

		// Token: 0x04000FB7 RID: 4023
		internal static Action OnPreRender = null;

		// Token: 0x04000FBF RID: 4031
		internal UIRVEShaderInfoAllocator shaderInfoAllocator;

		// Token: 0x02000456 RID: 1110
		private struct DepthOrderedDirtyTracking
		{
			// Token: 0x060022E5 RID: 8933 RVA: 0x0008771C File Offset: 0x0008591C
			public void EnsureFits(int maxDepth)
			{
				while (this.heads.Count <= maxDepth)
				{
					this.heads.Add(null);
					this.tails.Add(null);
				}
			}

			// Token: 0x060022E6 RID: 8934 RVA: 0x00087760 File Offset: 0x00085960
			public void RegisterDirty(VisualElement ve, RenderDataDirtyTypes dirtyTypes, RenderDataDirtyTypeClasses dirtyTypeClass)
			{
				Debug.Assert(dirtyTypes > RenderDataDirtyTypes.None);
				int hierarchyDepth = ve.renderChainData.hierarchyDepth;
				this.minDepths[(int)dirtyTypeClass] = ((hierarchyDepth < this.minDepths[(int)dirtyTypeClass]) ? hierarchyDepth : this.minDepths[(int)dirtyTypeClass]);
				this.maxDepths[(int)dirtyTypeClass] = ((hierarchyDepth > this.maxDepths[(int)dirtyTypeClass]) ? hierarchyDepth : this.maxDepths[(int)dirtyTypeClass]);
				bool flag = ve.renderChainData.dirtiedValues > RenderDataDirtyTypes.None;
				if (flag)
				{
					ve.renderChainData.dirtiedValues = ve.renderChainData.dirtiedValues | dirtyTypes;
				}
				else
				{
					ve.renderChainData.dirtiedValues = dirtyTypes;
					bool flag2 = this.tails[hierarchyDepth] != null;
					if (flag2)
					{
						this.tails[hierarchyDepth].renderChainData.nextDirty = ve;
						ve.renderChainData.prevDirty = this.tails[hierarchyDepth];
						this.tails[hierarchyDepth] = ve;
					}
					else
					{
						List<VisualElement> list = this.heads;
						int num = hierarchyDepth;
						this.tails[hierarchyDepth] = ve;
						list[num] = ve;
					}
				}
			}

			// Token: 0x060022E7 RID: 8935 RVA: 0x00087868 File Offset: 0x00085A68
			public void ClearDirty(VisualElement ve, RenderDataDirtyTypes dirtyTypesInverse)
			{
				Debug.Assert(ve.renderChainData.dirtiedValues > RenderDataDirtyTypes.None);
				ve.renderChainData.dirtiedValues = ve.renderChainData.dirtiedValues & dirtyTypesInverse;
				bool flag = ve.renderChainData.dirtiedValues == RenderDataDirtyTypes.None;
				if (flag)
				{
					bool flag2 = ve.renderChainData.prevDirty != null;
					if (flag2)
					{
						ve.renderChainData.prevDirty.renderChainData.nextDirty = ve.renderChainData.nextDirty;
					}
					bool flag3 = ve.renderChainData.nextDirty != null;
					if (flag3)
					{
						ve.renderChainData.nextDirty.renderChainData.prevDirty = ve.renderChainData.prevDirty;
					}
					bool flag4 = this.tails[ve.renderChainData.hierarchyDepth] == ve;
					if (flag4)
					{
						Debug.Assert(ve.renderChainData.nextDirty == null);
						this.tails[ve.renderChainData.hierarchyDepth] = ve.renderChainData.prevDirty;
					}
					bool flag5 = this.heads[ve.renderChainData.hierarchyDepth] == ve;
					if (flag5)
					{
						Debug.Assert(ve.renderChainData.prevDirty == null);
						this.heads[ve.renderChainData.hierarchyDepth] = ve.renderChainData.nextDirty;
					}
					ve.renderChainData.prevDirty = (ve.renderChainData.nextDirty = null);
				}
			}

			// Token: 0x060022E8 RID: 8936 RVA: 0x000879E0 File Offset: 0x00085BE0
			public void Reset()
			{
				for (int i = 0; i < this.minDepths.Length; i++)
				{
					this.minDepths[i] = int.MaxValue;
					this.maxDepths[i] = int.MinValue;
				}
			}

			// Token: 0x04000FC3 RID: 4035
			public List<VisualElement> heads;

			// Token: 0x04000FC4 RID: 4036
			public List<VisualElement> tails;

			// Token: 0x04000FC5 RID: 4037
			public int[] minDepths;

			// Token: 0x04000FC6 RID: 4038
			public int[] maxDepths;

			// Token: 0x04000FC7 RID: 4039
			public uint dirtyID;
		}

		// Token: 0x02000457 RID: 1111
		private struct RenderChainStaticIndexAllocator
		{
			// Token: 0x060022E9 RID: 8937 RVA: 0x00087A24 File Offset: 0x00085C24
			public static int AllocateIndex(RenderChain renderChain)
			{
				int num = RenderChain.RenderChainStaticIndexAllocator.renderChains.IndexOf(null);
				bool flag = num >= 0;
				if (flag)
				{
					RenderChain.RenderChainStaticIndexAllocator.renderChains[num] = renderChain;
				}
				else
				{
					num = RenderChain.RenderChainStaticIndexAllocator.renderChains.Count;
					RenderChain.RenderChainStaticIndexAllocator.renderChains.Add(renderChain);
				}
				return num;
			}

			// Token: 0x060022EA RID: 8938 RVA: 0x00087A76 File Offset: 0x00085C76
			public static void FreeIndex(int index)
			{
				RenderChain.RenderChainStaticIndexAllocator.renderChains[index] = null;
			}

			// Token: 0x060022EB RID: 8939 RVA: 0x00087A88 File Offset: 0x00085C88
			public static RenderChain AccessIndex(int index)
			{
				return RenderChain.RenderChainStaticIndexAllocator.renderChains[index];
			}

			// Token: 0x04000FC8 RID: 4040
			private static List<RenderChain> renderChains = new List<RenderChain>(4);
		}

		// Token: 0x02000458 RID: 1112
		private struct RenderNodeData
		{
			// Token: 0x04000FC9 RID: 4041
			public Material standardMaterial;

			// Token: 0x04000FCA RID: 4042
			public Material initialMaterial;

			// Token: 0x04000FCB RID: 4043
			public MaterialPropertyBlock matPropBlock;

			// Token: 0x04000FCC RID: 4044
			public RenderChainCommand firstCommand;

			// Token: 0x04000FCD RID: 4045
			public UIRenderDevice device;

			// Token: 0x04000FCE RID: 4046
			public Texture vectorAtlas;

			// Token: 0x04000FCF RID: 4047
			public Texture shaderInfoAtlas;

			// Token: 0x04000FD0 RID: 4048
			public float dpiScale;

			// Token: 0x04000FD1 RID: 4049
			public NativeSlice<Transform3x4> transformConstants;

			// Token: 0x04000FD2 RID: 4050
			public NativeSlice<Vector4> clipRectConstants;
		}
	}
}
