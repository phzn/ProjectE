using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RendererUtils;

namespace UnityEngine.Experimental.Rendering.RenderGraphModule
{
	// Token: 0x02000015 RID: 21
	public class RenderGraph
	{
		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600008E RID: 142 RVA: 0x0000453B File Offset: 0x0000273B
		// (set) Token: 0x0600008F RID: 143 RVA: 0x00004543 File Offset: 0x00002743
		public string name { get; private set; } = "RenderGraph";

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000090 RID: 144 RVA: 0x0000454C File Offset: 0x0000274C
		// (set) Token: 0x06000091 RID: 145 RVA: 0x00004553 File Offset: 0x00002753
		internal static bool requireDebugData { get; set; } = false;

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000092 RID: 146 RVA: 0x0000455B File Offset: 0x0000275B
		public RenderGraphDefaultResources defaultResources
		{
			get
			{
				return this.m_DefaultResources;
			}
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00004564 File Offset: 0x00002764
		public RenderGraph(string name = "RenderGraph")
		{
			this.name = name;
			this.m_Resources = new RenderGraphResourceRegistry(this.m_DebugParameters, this.m_FrameInformationLogger);
			for (int i = 0; i < 2; i++)
			{
				this.m_CompiledResourcesInfos[i] = new DynamicArray<RenderGraph.CompiledResourceInfo>();
			}
			RenderGraph.s_RegisteredGraphs.Add(this);
			RenderGraph.OnGraphRegisteredDelegate onGraphRegisteredDelegate = RenderGraph.onGraphRegistered;
			if (onGraphRegisteredDelegate == null)
			{
				return;
			}
			onGraphRegisteredDelegate(this);
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00004669 File Offset: 0x00002869
		public void Cleanup()
		{
			this.m_Resources.Cleanup();
			this.m_DefaultResources.Cleanup();
			this.m_RenderGraphPool.Cleanup();
			RenderGraph.s_RegisteredGraphs.Remove(this);
			RenderGraph.OnGraphRegisteredDelegate onGraphRegisteredDelegate = RenderGraph.onGraphUnregistered;
			if (onGraphRegisteredDelegate == null)
			{
				return;
			}
			onGraphRegisteredDelegate(this);
		}

		// Token: 0x06000095 RID: 149 RVA: 0x000046A8 File Offset: 0x000028A8
		public void RegisterDebug(DebugUI.Panel panel = null)
		{
			this.m_DebugParameters.RegisterDebug(this.name, panel);
		}

		// Token: 0x06000096 RID: 150 RVA: 0x000046BC File Offset: 0x000028BC
		public void UnRegisterDebug()
		{
			this.m_DebugParameters.UnRegisterDebug(this.name);
		}

		// Token: 0x06000097 RID: 151 RVA: 0x000046CF File Offset: 0x000028CF
		public static List<RenderGraph> GetRegisteredRenderGraphs()
		{
			return RenderGraph.s_RegisteredGraphs;
		}

		// Token: 0x06000098 RID: 152 RVA: 0x000046D8 File Offset: 0x000028D8
		internal RenderGraphDebugData GetDebugData(string executionName)
		{
			RenderGraphDebugData renderGraphDebugData;
			if (this.m_DebugData.TryGetValue(executionName, out renderGraphDebugData))
			{
				return renderGraphDebugData;
			}
			return null;
		}

		// Token: 0x06000099 RID: 153 RVA: 0x000046F8 File Offset: 0x000028F8
		public void EndFrame()
		{
			this.m_Resources.PurgeUnusedGraphicsResources();
			if (this.m_DebugParameters.logFrameInformation)
			{
				Debug.Log(this.m_FrameInformationLogger.GetAllLogs());
				this.m_DebugParameters.logFrameInformation = false;
			}
			if (this.m_DebugParameters.logResources)
			{
				this.m_Resources.FlushLogs();
				this.m_DebugParameters.logResources = false;
			}
		}

		// Token: 0x0600009A RID: 154 RVA: 0x0000475D File Offset: 0x0000295D
		public TextureHandle ImportTexture(RTHandle rt)
		{
			return this.m_Resources.ImportTexture(rt);
		}

		// Token: 0x0600009B RID: 155 RVA: 0x0000476B File Offset: 0x0000296B
		public TextureHandle ImportBackbuffer(RenderTargetIdentifier rt)
		{
			return this.m_Resources.ImportBackbuffer(rt);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00004779 File Offset: 0x00002979
		public TextureHandle CreateTexture(in TextureDesc desc)
		{
			return this.m_Resources.CreateTexture(in desc, -1);
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00004788 File Offset: 0x00002988
		public TextureHandle CreateSharedTexture(in TextureDesc desc, bool explicitRelease = false)
		{
			if (this.m_HasRenderGraphBegun)
			{
				throw new InvalidOperationException("A shared texture can only be created outside of render graph execution.");
			}
			return this.m_Resources.CreateSharedTexture(in desc, explicitRelease);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x000047AA File Offset: 0x000029AA
		public void RefreshSharedTextureDesc(TextureHandle handle, in TextureDesc desc)
		{
			this.m_Resources.RefreshSharedTextureDesc(handle, in desc);
		}

		// Token: 0x0600009F RID: 159 RVA: 0x000047B9 File Offset: 0x000029B9
		public void ReleaseSharedTexture(TextureHandle texture)
		{
			if (this.m_HasRenderGraphBegun)
			{
				throw new InvalidOperationException("A shared texture can only be release outside of render graph execution.");
			}
			this.m_Resources.ReleaseSharedTexture(texture);
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x000047DC File Offset: 0x000029DC
		public TextureHandle CreateTexture(TextureHandle texture)
		{
			RenderGraphResourceRegistry resources = this.m_Resources;
			TextureDesc textureResourceDesc = this.m_Resources.GetTextureResourceDesc(in texture.handle);
			return resources.CreateTexture(in textureResourceDesc, -1);
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x0000480A File Offset: 0x00002A0A
		public void CreateTextureIfInvalid(in TextureDesc desc, ref TextureHandle texture)
		{
			if (!texture.IsValid())
			{
				texture = this.m_Resources.CreateTexture(in desc, -1);
			}
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00004827 File Offset: 0x00002A27
		public TextureDesc GetTextureDesc(TextureHandle texture)
		{
			return this.m_Resources.GetTextureResourceDesc(in texture.handle);
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x0000483B File Offset: 0x00002A3B
		public RendererListHandle CreateRendererList(in RendererListDesc desc)
		{
			return this.m_Resources.CreateRendererList(in desc);
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00004849 File Offset: 0x00002A49
		public ComputeBufferHandle ImportComputeBuffer(ComputeBuffer computeBuffer)
		{
			return this.m_Resources.ImportComputeBuffer(computeBuffer);
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00004857 File Offset: 0x00002A57
		public ComputeBufferHandle CreateComputeBuffer(in ComputeBufferDesc desc)
		{
			return this.m_Resources.CreateComputeBuffer(in desc, -1);
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00004868 File Offset: 0x00002A68
		public ComputeBufferHandle CreateComputeBuffer(in ComputeBufferHandle computeBuffer)
		{
			RenderGraphResourceRegistry resources = this.m_Resources;
			ComputeBufferDesc computeBufferResourceDesc = this.m_Resources.GetComputeBufferResourceDesc(in computeBuffer.handle);
			return resources.CreateComputeBuffer(in computeBufferResourceDesc, -1);
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00004895 File Offset: 0x00002A95
		public ComputeBufferDesc GetComputeBufferDesc(in ComputeBufferHandle computeBuffer)
		{
			return this.m_Resources.GetComputeBufferResourceDesc(in computeBuffer.handle);
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x000048A8 File Offset: 0x00002AA8
		public RenderGraphBuilder AddRenderPass<PassData>(string passName, out PassData passData, ProfilingSampler sampler) where PassData : class, new()
		{
			RenderGraphPass<PassData> renderGraphPass = this.m_RenderGraphPool.Get<RenderGraphPass<PassData>>();
			renderGraphPass.Initialize(this.m_RenderPasses.Count, this.m_RenderGraphPool.Get<PassData>(), passName, sampler);
			passData = renderGraphPass.data;
			this.m_RenderPasses.Add(renderGraphPass);
			return new RenderGraphBuilder(renderGraphPass, this.m_Resources, this);
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00004904 File Offset: 0x00002B04
		public RenderGraphBuilder AddRenderPass<PassData>(string passName, out PassData passData) where PassData : class, new()
		{
			return this.AddRenderPass<PassData>(passName, out passData, this.GetDefaultProfilingSampler(passName));
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00004918 File Offset: 0x00002B18
		public RenderGraphExecution RecordAndExecute(in RenderGraphParameters parameters)
		{
			this.m_CurrentFrameIndex = parameters.currentFrameIndex;
			this.m_CurrentExecutionName = ((parameters.executionName != null) ? parameters.executionName : "RenderGraphExecution");
			this.m_HasRenderGraphBegun = true;
			this.m_RendererListCulling = parameters.rendererListCulling;
			RenderGraphResourceRegistry resources = this.m_Resources;
			int executionCount = this.m_ExecutionCount;
			this.m_ExecutionCount = executionCount + 1;
			resources.BeginRenderGraph(executionCount);
			if (this.m_DebugParameters.enableLogging)
			{
				this.m_FrameInformationLogger.Initialize(this.m_CurrentExecutionName);
			}
			this.m_DefaultResources.InitializeForRendering(this);
			this.m_RenderGraphContext.cmd = parameters.commandBuffer;
			this.m_RenderGraphContext.renderContext = parameters.scriptableRenderContext;
			this.m_RenderGraphContext.renderGraphPool = this.m_RenderGraphPool;
			this.m_RenderGraphContext.defaultResources = this.m_DefaultResources;
			if (this.m_DebugParameters.immediateMode)
			{
				this.LogFrameInformation();
				this.m_CompiledPassInfos.Resize(this.m_CompiledPassInfos.capacity, false);
				this.m_CurrentImmediatePassIndex = 0;
				for (int i = 0; i < 2; i++)
				{
					if (this.m_ImmediateModeResourceList[i] == null)
					{
						this.m_ImmediateModeResourceList[i] = new List<int>();
					}
					this.m_ImmediateModeResourceList[i].Clear();
				}
				this.m_Resources.BeginExecute(this.m_CurrentFrameIndex);
			}
			return new RenderGraphExecution(this);
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00004A64 File Offset: 0x00002C64
		internal void Execute()
		{
			this.m_ExecutionExceptionWasRaised = false;
			try
			{
				if (this.m_RenderGraphContext.cmd == null)
				{
					throw new InvalidOperationException("RenderGraph.RecordAndExecute was not called before executing the render graph.");
				}
				if (!this.m_DebugParameters.immediateMode)
				{
					this.LogFrameInformation();
					this.CompileRenderGraph();
					this.m_Resources.BeginExecute(this.m_CurrentFrameIndex);
					this.ExecuteRenderGraph();
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("Render Graph Execution error");
				if (!this.m_ExecutionExceptionWasRaised)
				{
					Debug.LogException(ex);
				}
				this.m_ExecutionExceptionWasRaised = true;
			}
			finally
			{
				this.GenerateDebugData();
				if (this.m_DebugParameters.immediateMode)
				{
					this.ReleaseImmediateModeResources();
				}
				this.ClearCompiledGraph();
				this.m_Resources.EndExecute();
				this.InvalidateContext();
				this.m_HasRenderGraphBegun = false;
			}
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00004B38 File Offset: 0x00002D38
		public void BeginProfilingSampler(ProfilingSampler sampler)
		{
			RenderGraph.ProfilingScopePassData profilingScopePassData;
			using (RenderGraphBuilder renderGraphBuilder = this.AddRenderPass<RenderGraph.ProfilingScopePassData>("BeginProfile", out profilingScopePassData, null))
			{
				profilingScopePassData.sampler = sampler;
				renderGraphBuilder.AllowPassCulling(false);
				renderGraphBuilder.GenerateDebugData(false);
				renderGraphBuilder.SetRenderFunc<RenderGraph.ProfilingScopePassData>(delegate(RenderGraph.ProfilingScopePassData data, RenderGraphContext ctx)
				{
					data.sampler.Begin(ctx.cmd);
				});
			}
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00004BB4 File Offset: 0x00002DB4
		public void EndProfilingSampler(ProfilingSampler sampler)
		{
			RenderGraph.ProfilingScopePassData profilingScopePassData;
			using (RenderGraphBuilder renderGraphBuilder = this.AddRenderPass<RenderGraph.ProfilingScopePassData>("EndProfile", out profilingScopePassData, null))
			{
				profilingScopePassData.sampler = sampler;
				renderGraphBuilder.AllowPassCulling(false);
				renderGraphBuilder.GenerateDebugData(false);
				renderGraphBuilder.SetRenderFunc<RenderGraph.ProfilingScopePassData>(delegate(RenderGraph.ProfilingScopePassData data, RenderGraphContext ctx)
				{
					data.sampler.End(ctx.cmd);
				});
			}
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00004C30 File Offset: 0x00002E30
		internal DynamicArray<RenderGraph.CompiledPassInfo> GetCompiledPassInfos()
		{
			return this.m_CompiledPassInfos;
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00004C38 File Offset: 0x00002E38
		internal void ClearCompiledGraph()
		{
			this.ClearRenderPasses();
			this.m_Resources.Clear(this.m_ExecutionExceptionWasRaised);
			this.m_RendererLists.Clear();
			for (int i = 0; i < 2; i++)
			{
				this.m_CompiledResourcesInfos[i].Clear();
			}
			this.m_CompiledPassInfos.Clear();
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00004C8B File Offset: 0x00002E8B
		private void InvalidateContext()
		{
			this.m_RenderGraphContext.cmd = null;
			this.m_RenderGraphContext.renderGraphPool = null;
			this.m_RenderGraphContext.defaultResources = null;
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00004CB1 File Offset: 0x00002EB1
		internal void OnPassAdded(RenderGraphPass pass)
		{
			if (this.m_DebugParameters.immediateMode)
			{
				this.ExecutePassImmediately(pass);
			}
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060000B2 RID: 178 RVA: 0x00004CC8 File Offset: 0x00002EC8
		// (remove) Token: 0x060000B3 RID: 179 RVA: 0x00004CFC File Offset: 0x00002EFC
		internal static event RenderGraph.OnGraphRegisteredDelegate onGraphRegistered;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060000B4 RID: 180 RVA: 0x00004D30 File Offset: 0x00002F30
		// (remove) Token: 0x060000B5 RID: 181 RVA: 0x00004D64 File Offset: 0x00002F64
		internal static event RenderGraph.OnGraphRegisteredDelegate onGraphUnregistered;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060000B6 RID: 182 RVA: 0x00004D98 File Offset: 0x00002F98
		// (remove) Token: 0x060000B7 RID: 183 RVA: 0x00004DCC File Offset: 0x00002FCC
		internal static event RenderGraph.OnExecutionRegisteredDelegate onExecutionRegistered;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060000B8 RID: 184 RVA: 0x00004E00 File Offset: 0x00003000
		// (remove) Token: 0x060000B9 RID: 185 RVA: 0x00004E34 File Offset: 0x00003034
		internal static event RenderGraph.OnExecutionRegisteredDelegate onExecutionUnregistered;

		// Token: 0x060000BA RID: 186 RVA: 0x00004E68 File Offset: 0x00003068
		private void InitResourceInfosData(DynamicArray<RenderGraph.CompiledResourceInfo> resourceInfos, int count)
		{
			resourceInfos.Resize(count, false);
			for (int i = 0; i < resourceInfos.size; i++)
			{
				resourceInfos[i].Reset();
			}
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00004E9C File Offset: 0x0000309C
		private void InitializeCompilationData()
		{
			this.InitResourceInfosData(this.m_CompiledResourcesInfos[0], this.m_Resources.GetTextureResourceCount());
			this.InitResourceInfosData(this.m_CompiledResourcesInfos[1], this.m_Resources.GetComputeBufferResourceCount());
			this.m_CompiledPassInfos.Resize(this.m_RenderPasses.Count, false);
			for (int i = 0; i < this.m_CompiledPassInfos.size; i++)
			{
				this.m_CompiledPassInfos[i].Reset(this.m_RenderPasses[i]);
			}
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00004F28 File Offset: 0x00003128
		private void CountReferences()
		{
			for (int i = 0; i < this.m_CompiledPassInfos.size; i++)
			{
				ref RenderGraph.CompiledPassInfo ptr = ref this.m_CompiledPassInfos[i];
				for (int j = 0; j < 2; j++)
				{
					foreach (ResourceHandle resourceHandle in ptr.pass.resourceReadLists[j])
					{
						ref RenderGraph.CompiledResourceInfo ptr2 = ref this.m_CompiledResourcesInfos[j][resourceHandle];
						ptr2.imported = this.m_Resources.IsRenderGraphResourceImported(in resourceHandle);
						ptr2.consumers.Add(i);
						ptr2.refCount++;
					}
					foreach (ResourceHandle resourceHandle2 in ptr.pass.resourceWriteLists[j])
					{
						ref RenderGraph.CompiledResourceInfo ptr3 = ref this.m_CompiledResourcesInfos[j][resourceHandle2];
						ptr3.imported = this.m_Resources.IsRenderGraphResourceImported(in resourceHandle2);
						ptr3.producers.Add(i);
						ptr.hasSideEffect = ptr3.imported;
						ptr.refCount++;
					}
					foreach (ResourceHandle resourceHandle3 in ptr.pass.transientResourceList[j])
					{
						int num = resourceHandle3;
						ref RenderGraph.CompiledResourceInfo ptr4 = ref this.m_CompiledResourcesInfos[j][num];
						ptr4.refCount++;
						ptr4.consumers.Add(i);
						ptr4.producers.Add(i);
					}
				}
			}
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00005100 File Offset: 0x00003300
		private unsafe void CullUnusedPasses()
		{
			if (this.m_DebugParameters.disablePassCulling)
			{
				if (this.m_DebugParameters.enableLogging)
				{
					this.m_FrameInformationLogger.LogLine("- Pass Culling Disabled -\n", Array.Empty<object>());
				}
				return;
			}
			for (int i = 0; i < 2; i++)
			{
				DynamicArray<RenderGraph.CompiledResourceInfo> dynamicArray = this.m_CompiledResourcesInfos[i];
				this.m_CullingStack.Clear();
				for (int j = 0; j < dynamicArray.size; j++)
				{
					if (dynamicArray[j].refCount == 0)
					{
						this.m_CullingStack.Push(j);
					}
				}
				while (this.m_CullingStack.Count != 0)
				{
					foreach (int num in dynamicArray[this.m_CullingStack.Pop()]->producers)
					{
						ref RenderGraph.CompiledPassInfo ptr = ref this.m_CompiledPassInfos[num];
						ptr.refCount--;
						if (ptr.refCount == 0 && !ptr.hasSideEffect && ptr.allowPassCulling)
						{
							ptr.culled = true;
							foreach (ResourceHandle resourceHandle in ptr.pass.resourceReadLists[i])
							{
								ref RenderGraph.CompiledResourceInfo ptr2 = ref dynamicArray[resourceHandle];
								ptr2.refCount--;
								if (ptr2.refCount == 0)
								{
									this.m_CullingStack.Push(resourceHandle);
								}
							}
						}
					}
				}
			}
			this.LogCulledPasses();
		}

		// Token: 0x060000BE RID: 190 RVA: 0x000052BC File Offset: 0x000034BC
		private void UpdatePassSynchronization(ref RenderGraph.CompiledPassInfo currentPassInfo, ref RenderGraph.CompiledPassInfo producerPassInfo, int currentPassIndex, int lastProducer, ref int intLastSyncIndex)
		{
			currentPassInfo.syncToPassIndex = lastProducer;
			intLastSyncIndex = lastProducer;
			producerPassInfo.needGraphicsFence = true;
			if (producerPassInfo.syncFromPassIndex == -1)
			{
				producerPassInfo.syncFromPassIndex = currentPassIndex;
			}
		}

		// Token: 0x060000BF RID: 191 RVA: 0x000052E4 File Offset: 0x000034E4
		private void UpdateResourceSynchronization(ref int lastGraphicsPipeSync, ref int lastComputePipeSync, int currentPassIndex, in RenderGraph.CompiledResourceInfo resource)
		{
			int latestProducerIndex = this.GetLatestProducerIndex(currentPassIndex, in resource);
			if (latestProducerIndex != -1)
			{
				ref RenderGraph.CompiledPassInfo ptr = ref this.m_CompiledPassInfos[currentPassIndex];
				if (this.m_CompiledPassInfos[latestProducerIndex].enableAsyncCompute != ptr.enableAsyncCompute)
				{
					if (ptr.enableAsyncCompute)
					{
						if (latestProducerIndex > lastGraphicsPipeSync)
						{
							this.UpdatePassSynchronization(ref ptr, this.m_CompiledPassInfos[latestProducerIndex], currentPassIndex, latestProducerIndex, ref lastGraphicsPipeSync);
							return;
						}
					}
					else if (latestProducerIndex > lastComputePipeSync)
					{
						this.UpdatePassSynchronization(ref ptr, this.m_CompiledPassInfos[latestProducerIndex], currentPassIndex, latestProducerIndex, ref lastComputePipeSync);
					}
				}
			}
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00005364 File Offset: 0x00003564
		private int GetFirstValidConsumerIndex(int passIndex, in RenderGraph.CompiledResourceInfo info)
		{
			foreach (int num in info.consumers)
			{
				if (num > passIndex && !this.m_CompiledPassInfos[num].culled)
				{
					return num;
				}
			}
			return -1;
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x000053D0 File Offset: 0x000035D0
		private int FindTextureProducer(int consumerPass, in RenderGraph.CompiledResourceInfo info, out int index)
		{
			int num = 0;
			for (index = 0; index < info.producers.Count; index++)
			{
				int num2 = info.producers[index];
				if (!this.m_CompiledPassInfos[num2].culled)
				{
					return num2;
				}
				if (num2 >= consumerPass)
				{
					return num;
				}
				num = num2;
			}
			return num;
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00005428 File Offset: 0x00003628
		private unsafe int GetLatestProducerIndex(int passIndex, in RenderGraph.CompiledResourceInfo info)
		{
			int num = -1;
			foreach (int num2 in info.producers)
			{
				RenderGraph.CompiledPassInfo compiledPassInfo = *this.m_CompiledPassInfos[num2];
				if (num2 >= passIndex || compiledPassInfo.culled || compiledPassInfo.culledByRendererList)
				{
					return num;
				}
				num = num2;
			}
			return num;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x000054AC File Offset: 0x000036AC
		private int GetLatestValidReadIndex(in RenderGraph.CompiledResourceInfo info)
		{
			if (info.consumers.Count == 0)
			{
				return -1;
			}
			List<int> consumers = info.consumers;
			for (int i = consumers.Count - 1; i >= 0; i--)
			{
				if (!this.m_CompiledPassInfos[consumers[i]].culled)
				{
					return consumers[i];
				}
			}
			return -1;
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00005504 File Offset: 0x00003704
		private int GetFirstValidWriteIndex(in RenderGraph.CompiledResourceInfo info)
		{
			if (info.producers.Count == 0)
			{
				return -1;
			}
			List<int> producers = info.producers;
			for (int i = 0; i < producers.Count; i++)
			{
				if (!this.m_CompiledPassInfos[producers[i]].culled)
				{
					return producers[i];
				}
			}
			return -1;
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x0000555C File Offset: 0x0000375C
		private int GetLatestValidWriteIndex(in RenderGraph.CompiledResourceInfo info)
		{
			if (info.producers.Count == 0)
			{
				return -1;
			}
			List<int> producers = info.producers;
			for (int i = producers.Count - 1; i >= 0; i--)
			{
				if (!this.m_CompiledPassInfos[producers[i]].culled)
				{
					return producers[i];
				}
			}
			return -1;
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x000055B4 File Offset: 0x000037B4
		private void CreateRendererLists()
		{
			for (int i = 0; i < this.m_CompiledPassInfos.size; i++)
			{
				ref RenderGraph.CompiledPassInfo ptr = ref this.m_CompiledPassInfos[i];
				if (!ptr.culled)
				{
					this.m_RendererLists.AddRange(ptr.pass.usedRendererListList);
				}
			}
			this.m_Resources.CreateRendererLists(this.m_RendererLists, this.m_RenderGraphContext.renderContext, this.m_RendererListCulling);
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00005624 File Offset: 0x00003824
		internal bool GetImportedFallback(TextureDesc desc, out TextureHandle fallback)
		{
			fallback = TextureHandle.nullHandle;
			if (!desc.bindTextureMS)
			{
				if (desc.depthBufferBits != DepthBits.None)
				{
					fallback = this.defaultResources.whiteTexture;
				}
				else if (desc.clearColor == Color.black || desc.clearColor == default(Color))
				{
					if (desc.dimension == TextureXR.dimension)
					{
						fallback = this.defaultResources.blackTextureXR;
					}
					else if (desc.dimension == TextureDimension.Tex3D)
					{
						fallback = this.defaultResources.blackTexture3DXR;
					}
					else if (desc.dimension == TextureDimension.Tex2D)
					{
						fallback = this.defaultResources.blackTexture;
					}
				}
				else if (desc.clearColor == Color.white)
				{
					if (desc.dimension == TextureXR.dimension)
					{
						fallback = this.defaultResources.whiteTextureXR;
					}
					else if (desc.dimension == TextureDimension.Tex2D)
					{
						fallback = this.defaultResources.whiteTexture;
					}
				}
			}
			return fallback.IsValid();
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x0000573C File Offset: 0x0000393C
		private void AllocateCulledPassResources(ref RenderGraph.CompiledPassInfo passInfo, int passIndex)
		{
			for (int i = 0; i < 2; i++)
			{
				DynamicArray<RenderGraph.CompiledResourceInfo> dynamicArray = this.m_CompiledResourcesInfos[i];
				foreach (ResourceHandle resourceHandle in passInfo.pass.resourceWriteLists[i])
				{
					ref RenderGraph.CompiledResourceInfo ptr = ref dynamicArray[resourceHandle];
					int firstValidConsumerIndex = this.GetFirstValidConsumerIndex(passIndex, in ptr);
					int num2;
					int num = this.FindTextureProducer(firstValidConsumerIndex, in ptr, out num2);
					if (firstValidConsumerIndex != -1 && passIndex == num)
					{
						if (i == 0)
						{
							TextureResource textureResource = this.m_Resources.GetTextureResource(in resourceHandle);
							TextureHandle textureHandle;
							if (!textureResource.desc.disableFallBackToImportedTexture && this.GetImportedFallback(textureResource.desc, out textureHandle))
							{
								ptr.imported = true;
								textureResource.imported = true;
								textureResource.graphicsResource = this.m_Resources.GetTexture(in textureHandle);
								continue;
							}
							textureResource.desc.sizeMode = TextureSizeMode.Explicit;
							textureResource.desc.width = 1;
							textureResource.desc.height = 1;
							textureResource.desc.clearBuffer = true;
						}
						ptr.producers[num2 - 1] = firstValidConsumerIndex;
					}
				}
			}
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00005888 File Offset: 0x00003A88
		private unsafe void UpdateResourceAllocationAndSynchronization()
		{
			int num = -1;
			int num2 = -1;
			for (int i = 0; i < this.m_CompiledPassInfos.size; i++)
			{
				ref RenderGraph.CompiledPassInfo ptr = ref this.m_CompiledPassInfos[i];
				if (ptr.culledByRendererList)
				{
					this.AllocateCulledPassResources(ref ptr, i);
				}
				if (!ptr.culled)
				{
					for (int j = 0; j < 2; j++)
					{
						DynamicArray<RenderGraph.CompiledResourceInfo> dynamicArray = this.m_CompiledResourcesInfos[j];
						foreach (ResourceHandle resourceHandle in ptr.pass.resourceReadLists[j])
						{
							int num3 = resourceHandle;
							this.UpdateResourceSynchronization(ref num, ref num2, i, dynamicArray[num3]);
						}
						foreach (ResourceHandle resourceHandle2 in ptr.pass.resourceWriteLists[j])
						{
							int num4 = resourceHandle2;
							this.UpdateResourceSynchronization(ref num, ref num2, i, dynamicArray[num4]);
						}
					}
				}
			}
			for (int k = 0; k < 2; k++)
			{
				DynamicArray<RenderGraph.CompiledResourceInfo> dynamicArray2 = this.m_CompiledResourcesInfos[k];
				for (int l = 0; l < dynamicArray2.size; l++)
				{
					RenderGraph.CompiledResourceInfo compiledResourceInfo = *dynamicArray2[l];
					bool flag = this.m_Resources.IsRenderGraphResourceShared((RenderGraphResourceType)k, l);
					if (!compiledResourceInfo.imported || flag)
					{
						int firstValidWriteIndex = this.GetFirstValidWriteIndex(in compiledResourceInfo);
						if (firstValidWriteIndex != -1)
						{
							this.m_CompiledPassInfos[firstValidWriteIndex].resourceCreateList[k].Add(l);
						}
						int latestValidReadIndex = this.GetLatestValidReadIndex(in compiledResourceInfo);
						int latestValidWriteIndex = this.GetLatestValidWriteIndex(in compiledResourceInfo);
						int num5 = ((firstValidWriteIndex != -1 || compiledResourceInfo.imported) ? Math.Max(latestValidWriteIndex, latestValidReadIndex) : (-1));
						if (num5 != -1)
						{
							if (this.m_CompiledPassInfos[num5].enableAsyncCompute)
							{
								int num6 = num5;
								int num7 = this.m_CompiledPassInfos[num6].syncFromPassIndex;
								while (num7 == -1 && num6++ < this.m_CompiledPassInfos.size - 1)
								{
									if (this.m_CompiledPassInfos[num6].enableAsyncCompute)
									{
										num7 = this.m_CompiledPassInfos[num6].syncFromPassIndex;
									}
								}
								if (num6 == this.m_CompiledPassInfos.size)
								{
									if (!this.m_CompiledPassInfos[num5].hasSideEffect)
									{
										RenderGraphPass renderGraphPass = this.m_RenderPasses[num5];
										string text = "<unknown>";
										throw new InvalidOperationException(string.Format("{0} resource '{1}' in asynchronous pass '{2}' is missing synchronization on the graphics pipeline.", (RenderGraphResourceType)k, text, renderGraphPass.name));
									}
									num7 = num6;
								}
								int num8 = Math.Max(0, num7 - 1);
								while (this.m_CompiledPassInfos[num8].culled)
								{
									num8 = Math.Max(0, num8 - 1);
								}
								this.m_CompiledPassInfos[num8].resourceReleaseList[k].Add(l);
							}
							else
							{
								this.m_CompiledPassInfos[num5].resourceReleaseList[k].Add(l);
							}
						}
						if (flag && (firstValidWriteIndex != -1 || num5 != -1))
						{
							this.m_Resources.UpdateSharedResourceLastFrameIndex(k, l);
						}
					}
				}
			}
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00005BEC File Offset: 0x00003DEC
		private bool AreRendererListsEmpty(List<RendererListHandle> rendererLists)
		{
			foreach (RendererListHandle rendererListHandle in rendererLists)
			{
				RendererList rendererList = this.m_Resources.GetRendererList(in rendererListHandle);
				if (this.m_RenderGraphContext.renderContext.QueryRendererListStatus(rendererList) == RendererListStatus.kRendererListPopulated)
				{
					return false;
				}
			}
			return rendererLists.Count > 0;
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00005C68 File Offset: 0x00003E68
		private void TryCullPassAtIndex(int passIndex)
		{
			RenderGraphPass pass = this.m_CompiledPassInfos[passIndex].pass;
			if (!this.m_CompiledPassInfos[passIndex].culled && pass.allowPassCulling && pass.allowRendererListCulling && !this.m_CompiledPassInfos[passIndex].hasSideEffect && this.AreRendererListsEmpty(pass.usedRendererListList))
			{
				this.m_CompiledPassInfos[passIndex].culled = (this.m_CompiledPassInfos[passIndex].culledByRendererList = true);
			}
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00005CF4 File Offset: 0x00003EF4
		private void CullRendererLists()
		{
			for (int i = 0; i < this.m_CompiledPassInfos.size; i++)
			{
				if (!this.m_CompiledPassInfos[i].culled && !this.m_CompiledPassInfos[i].hasSideEffect && this.m_CompiledPassInfos[i].pass.usedRendererListList.Count > 0)
				{
					this.TryCullPassAtIndex(i);
				}
			}
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00005D64 File Offset: 0x00003F64
		internal void CompileRenderGraph()
		{
			using (new ProfilingScope(this.m_RenderGraphContext.cmd, ProfilingSampler.Get<RenderGraphProfileId>(RenderGraphProfileId.CompileRenderGraph)))
			{
				this.InitializeCompilationData();
				this.CountReferences();
				this.CullUnusedPasses();
				this.CreateRendererLists();
				if (this.m_RendererListCulling)
				{
					this.CullRendererLists();
				}
				this.UpdateResourceAllocationAndSynchronization();
				this.LogRendererListsCreation();
			}
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00005DDC File Offset: 0x00003FDC
		private ref RenderGraph.CompiledPassInfo CompilePassImmediatly(RenderGraphPass pass)
		{
			if (this.m_CurrentImmediatePassIndex >= this.m_CompiledPassInfos.size)
			{
				this.m_CompiledPassInfos.Resize(this.m_CompiledPassInfos.size * 2, false);
			}
			DynamicArray<RenderGraph.CompiledPassInfo> compiledPassInfos = this.m_CompiledPassInfos;
			int currentImmediatePassIndex = this.m_CurrentImmediatePassIndex;
			this.m_CurrentImmediatePassIndex = currentImmediatePassIndex + 1;
			ref RenderGraph.CompiledPassInfo ptr = ref compiledPassInfos[currentImmediatePassIndex];
			ptr.Reset(pass);
			ptr.enableAsyncCompute = false;
			for (int i = 0; i < 2; i++)
			{
				foreach (ResourceHandle resourceHandle in pass.transientResourceList[i])
				{
					ptr.resourceCreateList[i].Add(resourceHandle);
					ptr.resourceReleaseList[i].Add(resourceHandle);
				}
				foreach (ResourceHandle resourceHandle2 in pass.resourceWriteLists[i])
				{
					if (!pass.transientResourceList[i].Contains(resourceHandle2) && !this.m_Resources.IsGraphicsResourceCreated(in resourceHandle2))
					{
						ptr.resourceCreateList[i].Add(resourceHandle2);
						this.m_ImmediateModeResourceList[i].Add(resourceHandle2);
					}
				}
				foreach (ResourceHandle resourceHandle3 in pass.resourceReadLists[i])
				{
				}
			}
			foreach (RendererListHandle rendererListHandle in pass.usedRendererListList)
			{
				if (!this.m_Resources.IsRendererListCreated(in rendererListHandle))
				{
					this.m_RendererLists.Add(rendererListHandle);
				}
			}
			this.m_Resources.CreateRendererLists(this.m_RendererLists, this.m_RenderGraphContext.renderContext, false);
			this.m_RendererLists.Clear();
			return ref ptr;
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00006004 File Offset: 0x00004204
		private void ExecutePassImmediately(RenderGraphPass pass)
		{
			this.ExecuteCompiledPass(this.CompilePassImmediatly(pass), this.m_CurrentImmediatePassIndex - 1);
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x0000601C File Offset: 0x0000421C
		private void ExecuteCompiledPass(ref RenderGraph.CompiledPassInfo passInfo, int passIndex)
		{
			if (passInfo.culled)
			{
				return;
			}
			if (!passInfo.pass.HasRenderFunc())
			{
				throw new InvalidOperationException(string.Format("RenderPass {0} was not provided with an execute function.", passInfo.pass.name));
			}
			try
			{
				using (new ProfilingScope(this.m_RenderGraphContext.cmd, passInfo.pass.customSampler))
				{
					this.LogRenderPassBegin(in passInfo);
					using (new RenderGraphLogIndent(this.m_FrameInformationLogger, 1))
					{
						this.PreRenderPassExecute(in passInfo, this.m_RenderGraphContext);
						passInfo.pass.Execute(this.m_RenderGraphContext);
						this.PostRenderPassExecute(ref passInfo, this.m_RenderGraphContext);
					}
				}
			}
			catch (Exception ex)
			{
				this.m_ExecutionExceptionWasRaised = true;
				Debug.LogError(string.Format("Render Graph Execution error at pass {0} ({1})", passInfo.pass.name, passIndex));
				Debug.LogException(ex);
				throw;
			}
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00006130 File Offset: 0x00004330
		private void ExecuteRenderGraph()
		{
			using (new ProfilingScope(this.m_RenderGraphContext.cmd, ProfilingSampler.Get<RenderGraphProfileId>(RenderGraphProfileId.ExecuteRenderGraph)))
			{
				for (int i = 0; i < this.m_CompiledPassInfos.size; i++)
				{
					this.ExecuteCompiledPass(this.m_CompiledPassInfos[i], i);
				}
			}
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x000061A0 File Offset: 0x000043A0
		private void PreRenderPassSetRenderTargets(in RenderGraph.CompiledPassInfo passInfo, RenderGraphContext rgContext)
		{
			RenderGraphPass pass = passInfo.pass;
			TextureHandle textureHandle = pass.depthBuffer;
			if (textureHandle.IsValid() || pass.colorBufferMaxIndex != -1)
			{
				RenderTargetIdentifier[] tempArray = rgContext.renderGraphPool.GetTempArray<RenderTargetIdentifier>(pass.colorBufferMaxIndex + 1);
				TextureHandle[] colorBuffers = pass.colorBuffers;
				if (pass.colorBufferMaxIndex > 0)
				{
					for (int i = 0; i <= pass.colorBufferMaxIndex; i++)
					{
						if (!colorBuffers[i].IsValid())
						{
							throw new InvalidOperationException("MRT setup is invalid. Some indices are not used.");
						}
						tempArray[i] = this.m_Resources.GetTexture(in colorBuffers[i]);
					}
					textureHandle = pass.depthBuffer;
					if (textureHandle.IsValid())
					{
						CommandBuffer cmd = rgContext.cmd;
						RenderTargetIdentifier[] array = tempArray;
						RenderGraphResourceRegistry resources = this.m_Resources;
						textureHandle = pass.depthBuffer;
						CoreUtils.SetRenderTarget(cmd, array, resources.GetTexture(in textureHandle));
						return;
					}
					throw new InvalidOperationException("Setting MRTs without a depth buffer is not supported.");
				}
				else
				{
					textureHandle = pass.depthBuffer;
					if (textureHandle.IsValid())
					{
						if (pass.colorBufferMaxIndex > -1)
						{
							CommandBuffer cmd2 = rgContext.cmd;
							RTHandle texture = this.m_Resources.GetTexture(in pass.colorBuffers[0]);
							RenderGraphResourceRegistry resources2 = this.m_Resources;
							textureHandle = pass.depthBuffer;
							CoreUtils.SetRenderTarget(cmd2, texture, resources2.GetTexture(in textureHandle), 0, CubemapFace.Unknown, -1);
							return;
						}
						CommandBuffer cmd3 = rgContext.cmd;
						RenderGraphResourceRegistry resources3 = this.m_Resources;
						textureHandle = pass.depthBuffer;
						CoreUtils.SetRenderTarget(cmd3, resources3.GetTexture(in textureHandle), ClearFlag.None, 0, CubemapFace.Unknown, -1);
						return;
					}
					else
					{
						CoreUtils.SetRenderTarget(rgContext.cmd, this.m_Resources.GetTexture(in pass.colorBuffers[0]), ClearFlag.None, 0, CubemapFace.Unknown, -1);
					}
				}
			}
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00006320 File Offset: 0x00004520
		private void PreRenderPassExecute(in RenderGraph.CompiledPassInfo passInfo, RenderGraphContext rgContext)
		{
			RenderGraphPass pass = passInfo.pass;
			this.m_PreviousCommandBuffer = rgContext.cmd;
			bool flag = false;
			for (int i = 0; i < 2; i++)
			{
				foreach (int num in passInfo.resourceCreateList[i])
				{
					flag |= this.m_Resources.CreatePooledResource(rgContext, i, num);
				}
			}
			this.PreRenderPassSetRenderTargets(in passInfo, rgContext);
			if (passInfo.enableAsyncCompute)
			{
				GraphicsFence graphicsFence = default(GraphicsFence);
				if (flag)
				{
					graphicsFence = rgContext.cmd.CreateGraphicsFence(GraphicsFenceType.AsyncQueueSynchronisation, SynchronisationStageFlags.AllGPUOperations);
				}
				rgContext.renderContext.ExecuteCommandBuffer(rgContext.cmd);
				rgContext.cmd.Clear();
				CommandBuffer commandBuffer = CommandBufferPool.Get(pass.name);
				commandBuffer.SetExecutionFlags(CommandBufferExecutionFlags.AsyncCompute);
				rgContext.cmd = commandBuffer;
				if (flag)
				{
					rgContext.cmd.WaitOnAsyncGraphicsFence(graphicsFence);
				}
			}
			if (passInfo.syncToPassIndex != -1)
			{
				rgContext.cmd.WaitOnAsyncGraphicsFence(this.m_CompiledPassInfos[passInfo.syncToPassIndex].fence);
			}
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00006440 File Offset: 0x00004640
		private void PostRenderPassExecute(ref RenderGraph.CompiledPassInfo passInfo, RenderGraphContext rgContext)
		{
			if (passInfo.needGraphicsFence)
			{
				passInfo.fence = rgContext.cmd.CreateAsyncGraphicsFence();
			}
			if (passInfo.enableAsyncCompute)
			{
				rgContext.renderContext.ExecuteCommandBufferAsync(rgContext.cmd, ComputeQueueType.Background);
				CommandBufferPool.Release(rgContext.cmd);
				rgContext.cmd = this.m_PreviousCommandBuffer;
			}
			this.m_RenderGraphPool.ReleaseAllTempAlloc();
			for (int i = 0; i < 2; i++)
			{
				foreach (int num in passInfo.resourceReleaseList[i])
				{
					this.m_Resources.ReleasePooledResource(rgContext, i, num);
				}
			}
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00006500 File Offset: 0x00004700
		private void ClearRenderPasses()
		{
			foreach (RenderGraphPass renderGraphPass in this.m_RenderPasses)
			{
				renderGraphPass.Release(this.m_RenderGraphPool);
			}
			this.m_RenderPasses.Clear();
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00006564 File Offset: 0x00004764
		private void ReleaseImmediateModeResources()
		{
			for (int i = 0; i < 2; i++)
			{
				foreach (int num in this.m_ImmediateModeResourceList[i])
				{
					this.m_Resources.ReleasePooledResource(this.m_RenderGraphContext, i, num);
				}
			}
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x000065D4 File Offset: 0x000047D4
		private void LogFrameInformation()
		{
			if (this.m_DebugParameters.enableLogging)
			{
				this.m_FrameInformationLogger.LogLine("==== Staring render graph frame for: " + this.m_CurrentExecutionName + " ====", Array.Empty<object>());
				if (!this.m_DebugParameters.immediateMode)
				{
					this.m_FrameInformationLogger.LogLine("Number of passes declared: {0}\n", new object[] { this.m_RenderPasses.Count });
				}
			}
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00006649 File Offset: 0x00004849
		private void LogRendererListsCreation()
		{
			if (this.m_DebugParameters.enableLogging)
			{
				this.m_FrameInformationLogger.LogLine("Number of renderer lists created: {0}\n", new object[] { this.m_RendererLists.Count });
			}
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00006684 File Offset: 0x00004884
		private void LogRenderPassBegin(in RenderGraph.CompiledPassInfo passInfo)
		{
			if (this.m_DebugParameters.enableLogging)
			{
				RenderGraphPass pass = passInfo.pass;
				this.m_FrameInformationLogger.LogLine("[{0}][{1}] \"{2}\"", new object[]
				{
					pass.index,
					pass.enableAsyncCompute ? "Compute" : "Graphics",
					pass.name
				});
				using (new RenderGraphLogIndent(this.m_FrameInformationLogger, 1))
				{
					if (passInfo.syncToPassIndex != -1)
					{
						this.m_FrameInformationLogger.LogLine("Synchronize with [{0}]", new object[] { passInfo.syncToPassIndex });
					}
				}
			}
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00006748 File Offset: 0x00004948
		private void LogCulledPasses()
		{
			if (this.m_DebugParameters.enableLogging)
			{
				this.m_FrameInformationLogger.LogLine("Pass Culling Report:", Array.Empty<object>());
				using (new RenderGraphLogIndent(this.m_FrameInformationLogger, 1))
				{
					for (int i = 0; i < this.m_CompiledPassInfos.size; i++)
					{
						if (this.m_CompiledPassInfos[i].culled)
						{
							RenderGraphPass renderGraphPass = this.m_RenderPasses[i];
							this.m_FrameInformationLogger.LogLine("[{0}] {1}", new object[] { renderGraphPass.index, renderGraphPass.name });
						}
					}
					this.m_FrameInformationLogger.LogLine("\n", Array.Empty<object>());
				}
			}
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00006820 File Offset: 0x00004A20
		private ProfilingSampler GetDefaultProfilingSampler(string name)
		{
			int hashCode = name.GetHashCode();
			ProfilingSampler profilingSampler;
			if (!this.m_DefaultProfilingSamplers.TryGetValue(hashCode, out profilingSampler))
			{
				profilingSampler = new ProfilingSampler(name);
				this.m_DefaultProfilingSamplers.Add(hashCode, profilingSampler);
			}
			return profilingSampler;
		}

		// Token: 0x060000DC RID: 220 RVA: 0x0000685C File Offset: 0x00004A5C
		private void UpdateImportedResourceLifeTime(ref RenderGraphDebugData.ResourceDebugData data, List<int> passList)
		{
			foreach (int num in passList)
			{
				if (data.creationPassIndex == -1)
				{
					data.creationPassIndex = num;
				}
				else
				{
					data.creationPassIndex = Math.Min(data.creationPassIndex, num);
				}
				if (data.releasePassIndex == -1)
				{
					data.releasePassIndex = num;
				}
				else
				{
					data.releasePassIndex = Math.Max(data.releasePassIndex, num);
				}
			}
		}

		// Token: 0x060000DD RID: 221 RVA: 0x000068EC File Offset: 0x00004AEC
		private void GenerateDebugData()
		{
			if (this.m_ExecutionExceptionWasRaised)
			{
				return;
			}
			if (!RenderGraph.requireDebugData)
			{
				this.CleanupDebugData();
				return;
			}
			RenderGraphDebugData renderGraphDebugData;
			if (!this.m_DebugData.TryGetValue(this.m_CurrentExecutionName, out renderGraphDebugData))
			{
				RenderGraph.OnExecutionRegisteredDelegate onExecutionRegisteredDelegate = RenderGraph.onExecutionRegistered;
				if (onExecutionRegisteredDelegate != null)
				{
					onExecutionRegisteredDelegate(this, this.m_CurrentExecutionName);
				}
				renderGraphDebugData = new RenderGraphDebugData();
				this.m_DebugData.Add(this.m_CurrentExecutionName, renderGraphDebugData);
			}
			renderGraphDebugData.Clear();
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < this.m_CompiledResourcesInfos[i].size; j++)
				{
					ref RenderGraph.CompiledResourceInfo ptr = ref this.m_CompiledResourcesInfos[i][j];
					RenderGraphDebugData.ResourceDebugData resourceDebugData = new RenderGraphDebugData.ResourceDebugData
					{
						name = this.m_Resources.GetRenderGraphResourceName((RenderGraphResourceType)i, j),
						imported = this.m_Resources.IsRenderGraphResourceImported((RenderGraphResourceType)i, j),
						creationPassIndex = -1,
						releasePassIndex = -1,
						consumerList = new List<int>(ptr.consumers),
						producerList = new List<int>(ptr.producers)
					};
					if (resourceDebugData.imported)
					{
						this.UpdateImportedResourceLifeTime(ref resourceDebugData, resourceDebugData.consumerList);
						this.UpdateImportedResourceLifeTime(ref resourceDebugData, resourceDebugData.producerList);
					}
					renderGraphDebugData.resourceLists[i].Add(resourceDebugData);
				}
			}
			for (int k = 0; k < this.m_CompiledPassInfos.size; k++)
			{
				ref RenderGraph.CompiledPassInfo ptr2 = ref this.m_CompiledPassInfos[k];
				RenderGraphDebugData.PassDebugData passDebugData = default(RenderGraphDebugData.PassDebugData);
				passDebugData.name = ptr2.pass.name;
				passDebugData.culled = ptr2.culled;
				passDebugData.async = ptr2.enableAsyncCompute;
				passDebugData.generateDebugData = ptr2.pass.generateDebugData;
				passDebugData.resourceReadLists = new List<int>[2];
				passDebugData.resourceWriteLists = new List<int>[2];
				passDebugData.syncFromPassIndex = ptr2.syncFromPassIndex;
				passDebugData.syncToPassIndex = ptr2.syncToPassIndex;
				for (int l = 0; l < 2; l++)
				{
					passDebugData.resourceReadLists[l] = new List<int>();
					passDebugData.resourceWriteLists[l] = new List<int>();
					foreach (ResourceHandle resourceHandle in ptr2.pass.resourceReadLists[l])
					{
						passDebugData.resourceReadLists[l].Add(resourceHandle);
					}
					foreach (ResourceHandle resourceHandle2 in ptr2.pass.resourceWriteLists[l])
					{
						passDebugData.resourceWriteLists[l].Add(resourceHandle2);
					}
					foreach (int num in ptr2.resourceCreateList[l])
					{
						RenderGraphDebugData.ResourceDebugData resourceDebugData2 = renderGraphDebugData.resourceLists[l][num];
						if (!resourceDebugData2.imported)
						{
							resourceDebugData2.creationPassIndex = k;
							renderGraphDebugData.resourceLists[l][num] = resourceDebugData2;
						}
					}
					foreach (int num2 in ptr2.resourceReleaseList[l])
					{
						RenderGraphDebugData.ResourceDebugData resourceDebugData3 = renderGraphDebugData.resourceLists[l][num2];
						if (!resourceDebugData3.imported)
						{
							resourceDebugData3.releasePassIndex = k;
							renderGraphDebugData.resourceLists[l][num2] = resourceDebugData3;
						}
					}
				}
				renderGraphDebugData.passList.Add(passDebugData);
			}
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00006CD0 File Offset: 0x00004ED0
		private void CleanupDebugData()
		{
			foreach (KeyValuePair<string, RenderGraphDebugData> keyValuePair in this.m_DebugData)
			{
				RenderGraph.OnExecutionRegisteredDelegate onExecutionRegisteredDelegate = RenderGraph.onExecutionUnregistered;
				if (onExecutionRegisteredDelegate != null)
				{
					onExecutionRegisteredDelegate(this, keyValuePair.Key);
				}
			}
			this.m_DebugData.Clear();
		}

		// Token: 0x0400006E RID: 110
		public static readonly int kMaxMRTCount = 8;

		// Token: 0x0400006F RID: 111
		private RenderGraphResourceRegistry m_Resources;

		// Token: 0x04000070 RID: 112
		private RenderGraphObjectPool m_RenderGraphPool = new RenderGraphObjectPool();

		// Token: 0x04000071 RID: 113
		private List<RenderGraphPass> m_RenderPasses = new List<RenderGraphPass>(64);

		// Token: 0x04000072 RID: 114
		private List<RendererListHandle> m_RendererLists = new List<RendererListHandle>(32);

		// Token: 0x04000073 RID: 115
		private RenderGraphDebugParams m_DebugParameters = new RenderGraphDebugParams();

		// Token: 0x04000074 RID: 116
		private RenderGraphLogger m_FrameInformationLogger = new RenderGraphLogger();

		// Token: 0x04000075 RID: 117
		private RenderGraphDefaultResources m_DefaultResources = new RenderGraphDefaultResources();

		// Token: 0x04000076 RID: 118
		private Dictionary<int, ProfilingSampler> m_DefaultProfilingSamplers = new Dictionary<int, ProfilingSampler>();

		// Token: 0x04000077 RID: 119
		private bool m_ExecutionExceptionWasRaised;

		// Token: 0x04000078 RID: 120
		private RenderGraphContext m_RenderGraphContext = new RenderGraphContext();

		// Token: 0x04000079 RID: 121
		private CommandBuffer m_PreviousCommandBuffer;

		// Token: 0x0400007A RID: 122
		private int m_CurrentImmediatePassIndex;

		// Token: 0x0400007B RID: 123
		private List<int>[] m_ImmediateModeResourceList = new List<int>[2];

		// Token: 0x0400007C RID: 124
		private DynamicArray<RenderGraph.CompiledResourceInfo>[] m_CompiledResourcesInfos = new DynamicArray<RenderGraph.CompiledResourceInfo>[2];

		// Token: 0x0400007D RID: 125
		private DynamicArray<RenderGraph.CompiledPassInfo> m_CompiledPassInfos = new DynamicArray<RenderGraph.CompiledPassInfo>();

		// Token: 0x0400007E RID: 126
		private Stack<int> m_CullingStack = new Stack<int>();

		// Token: 0x0400007F RID: 127
		private int m_ExecutionCount;

		// Token: 0x04000080 RID: 128
		private int m_CurrentFrameIndex;

		// Token: 0x04000081 RID: 129
		private bool m_HasRenderGraphBegun;

		// Token: 0x04000082 RID: 130
		private string m_CurrentExecutionName;

		// Token: 0x04000083 RID: 131
		private bool m_RendererListCulling;

		// Token: 0x04000084 RID: 132
		private Dictionary<string, RenderGraphDebugData> m_DebugData = new Dictionary<string, RenderGraphDebugData>();

		// Token: 0x04000085 RID: 133
		private static List<RenderGraph> s_RegisteredGraphs = new List<RenderGraph>();

		// Token: 0x02000147 RID: 327
		internal struct CompiledResourceInfo
		{
			// Token: 0x060009A5 RID: 2469 RVA: 0x0002BEBC File Offset: 0x0002A0BC
			public void Reset()
			{
				if (this.producers == null)
				{
					this.producers = new List<int>();
				}
				if (this.consumers == null)
				{
					this.consumers = new List<int>();
				}
				this.producers.Clear();
				this.consumers.Clear();
				this.refCount = 0;
				this.imported = false;
			}

			// Token: 0x040005B8 RID: 1464
			public List<int> producers;

			// Token: 0x040005B9 RID: 1465
			public List<int> consumers;

			// Token: 0x040005BA RID: 1466
			public int refCount;

			// Token: 0x040005BB RID: 1467
			public bool imported;
		}

		// Token: 0x02000148 RID: 328
		[DebuggerDisplay("RenderPass: {pass.name} (Index:{pass.index} Async:{enableAsyncCompute})")]
		internal struct CompiledPassInfo
		{
			// Token: 0x17000149 RID: 329
			// (get) Token: 0x060009A6 RID: 2470 RVA: 0x0002BF13 File Offset: 0x0002A113
			public bool allowPassCulling
			{
				get
				{
					return this.pass.allowPassCulling;
				}
			}

			// Token: 0x060009A7 RID: 2471 RVA: 0x0002BF20 File Offset: 0x0002A120
			public void Reset(RenderGraphPass pass)
			{
				this.pass = pass;
				this.enableAsyncCompute = pass.enableAsyncCompute;
				if (this.resourceCreateList == null)
				{
					this.resourceCreateList = new List<int>[2];
					this.resourceReleaseList = new List<int>[2];
					for (int i = 0; i < 2; i++)
					{
						this.resourceCreateList[i] = new List<int>();
						this.resourceReleaseList[i] = new List<int>();
					}
				}
				for (int j = 0; j < 2; j++)
				{
					this.resourceCreateList[j].Clear();
					this.resourceReleaseList[j].Clear();
				}
				this.refCount = 0;
				this.culled = false;
				this.culledByRendererList = false;
				this.hasSideEffect = false;
				this.syncToPassIndex = -1;
				this.syncFromPassIndex = -1;
				this.needGraphicsFence = false;
			}

			// Token: 0x040005BC RID: 1468
			public RenderGraphPass pass;

			// Token: 0x040005BD RID: 1469
			public List<int>[] resourceCreateList;

			// Token: 0x040005BE RID: 1470
			public List<int>[] resourceReleaseList;

			// Token: 0x040005BF RID: 1471
			public int refCount;

			// Token: 0x040005C0 RID: 1472
			public bool culled;

			// Token: 0x040005C1 RID: 1473
			public bool culledByRendererList;

			// Token: 0x040005C2 RID: 1474
			public bool hasSideEffect;

			// Token: 0x040005C3 RID: 1475
			public int syncToPassIndex;

			// Token: 0x040005C4 RID: 1476
			public int syncFromPassIndex;

			// Token: 0x040005C5 RID: 1477
			public bool needGraphicsFence;

			// Token: 0x040005C6 RID: 1478
			public GraphicsFence fence;

			// Token: 0x040005C7 RID: 1479
			public bool enableAsyncCompute;
		}

		// Token: 0x02000149 RID: 329
		private class ProfilingScopePassData
		{
			// Token: 0x040005C8 RID: 1480
			public ProfilingSampler sampler;
		}

		// Token: 0x0200014A RID: 330
		// (Invoke) Token: 0x060009AA RID: 2474
		internal delegate void OnGraphRegisteredDelegate(RenderGraph graph);

		// Token: 0x0200014B RID: 331
		// (Invoke) Token: 0x060009AE RID: 2478
		internal delegate void OnExecutionRegisteredDelegate(RenderGraph graph, string executionName);
	}
}
