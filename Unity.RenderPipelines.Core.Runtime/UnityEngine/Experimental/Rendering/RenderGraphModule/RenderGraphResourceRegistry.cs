using System;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RendererUtils;

namespace UnityEngine.Experimental.Rendering.RenderGraphModule
{
	// Token: 0x02000025 RID: 37
	internal class RenderGraphResourceRegistry
	{
		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000176 RID: 374 RVA: 0x00008340 File Offset: 0x00006540
		// (set) Token: 0x06000177 RID: 375 RVA: 0x00008347 File Offset: 0x00006547
		internal static RenderGraphResourceRegistry current
		{
			get
			{
				return RenderGraphResourceRegistry.m_CurrentRegistry;
			}
			set
			{
				RenderGraphResourceRegistry.m_CurrentRegistry = value;
			}
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00008350 File Offset: 0x00006550
		internal RTHandle GetTexture(in TextureHandle handle)
		{
			TextureHandle textureHandle = handle;
			if (!textureHandle.IsValid())
			{
				return null;
			}
			TextureResource textureResource = this.GetTextureResource(in handle.handle);
			RTHandle graphicsResource = textureResource.graphicsResource;
			if (graphicsResource == null && !textureResource.imported)
			{
				throw new InvalidOperationException("Trying to use a texture (" + textureResource.GetName() + ") that was already released or not yet created. Make sure you declare it for reading in your pass or you don't read it before it's been written to at least once.");
			}
			return graphicsResource;
		}

		// Token: 0x06000179 RID: 377 RVA: 0x000083A8 File Offset: 0x000065A8
		internal bool TextureNeedsFallback(in TextureHandle handle)
		{
			TextureHandle textureHandle = handle;
			return textureHandle.IsValid() && this.GetTextureResource(in handle.handle).NeedsFallBack();
		}

		// Token: 0x0600017A RID: 378 RVA: 0x000083D8 File Offset: 0x000065D8
		internal RendererList GetRendererList(in RendererListHandle handle)
		{
			RendererListHandle rendererListHandle = handle;
			if (!rendererListHandle.IsValid() || handle >= this.m_RendererListResources.size)
			{
				return RendererList.nullRendererList;
			}
			return this.m_RendererListResources[handle].rendererList;
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00008430 File Offset: 0x00006630
		internal ComputeBuffer GetComputeBuffer(in ComputeBufferHandle handle)
		{
			ComputeBufferHandle computeBufferHandle = handle;
			if (!computeBufferHandle.IsValid())
			{
				return null;
			}
			ComputeBuffer graphicsResource = this.GetComputeBufferResource(in handle.handle).graphicsResource;
			if (graphicsResource == null)
			{
				throw new InvalidOperationException("Trying to use a compute buffer ({bufferResource.GetName()}) that was already released or not yet created. Make sure you declare it for reading in your pass or you don't read it before it's been written to at least once.");
			}
			return graphicsResource;
		}

		// Token: 0x0600017C RID: 380 RVA: 0x0000846E File Offset: 0x0000666E
		private RenderGraphResourceRegistry()
		{
		}

		// Token: 0x0600017D RID: 381 RVA: 0x000084A8 File Offset: 0x000066A8
		internal RenderGraphResourceRegistry(RenderGraphDebugParams renderGraphDebug, RenderGraphLogger frameInformationLogger)
		{
			this.m_RenderGraphDebug = renderGraphDebug;
			this.m_FrameInformationLogger = frameInformationLogger;
			for (int i = 0; i < 2; i++)
			{
				this.m_RenderGraphResources[i] = new RenderGraphResourceRegistry.RenderGraphResourcesData();
			}
			this.m_RenderGraphResources[0].createResourceCallback = new RenderGraphResourceRegistry.ResourceCreateCallback(this.CreateTextureCallback);
			this.m_RenderGraphResources[0].releaseResourceCallback = new RenderGraphResourceRegistry.ResourceCallback(this.ReleaseTextureCallback);
			this.m_RenderGraphResources[0].pool = new TexturePool();
			this.m_RenderGraphResources[1].pool = new ComputeBufferPool();
		}

		// Token: 0x0600017E RID: 382 RVA: 0x0000856A File Offset: 0x0000676A
		internal void BeginRenderGraph(int executionCount)
		{
			this.m_ExecutionCount = executionCount;
			ResourceHandle.NewFrame(executionCount);
			if (this.m_RenderGraphDebug.enableLogging)
			{
				this.m_ResourceLogger.Initialize("RenderGraph Resources");
			}
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00008596 File Offset: 0x00006796
		internal void BeginExecute(int currentFrameIndex)
		{
			this.m_CurrentFrameIndex = currentFrameIndex;
			this.ManageSharedRenderGraphResources();
			RenderGraphResourceRegistry.current = this;
		}

		// Token: 0x06000180 RID: 384 RVA: 0x000085AB File Offset: 0x000067AB
		internal void EndExecute()
		{
			RenderGraphResourceRegistry.current = null;
		}

		// Token: 0x06000181 RID: 385 RVA: 0x000085B4 File Offset: 0x000067B4
		private void CheckHandleValidity(in ResourceHandle res)
		{
			RenderGraphResourceType type = res.type;
			ResourceHandle resourceHandle = res;
			this.CheckHandleValidity(type, resourceHandle.index);
		}

		// Token: 0x06000182 RID: 386 RVA: 0x000085DC File Offset: 0x000067DC
		private void CheckHandleValidity(RenderGraphResourceType type, int index)
		{
			DynamicArray<IRenderGraphResource> resourceArray = this.m_RenderGraphResources[(int)type].resourceArray;
			if (index >= resourceArray.size)
			{
				throw new ArgumentException(string.Format("Trying to access resource of type {0} with an invalid resource index {1}", type, index));
			}
		}

		// Token: 0x06000183 RID: 387 RVA: 0x0000861C File Offset: 0x0000681C
		internal unsafe void IncrementWriteCount(in ResourceHandle res)
		{
			this.CheckHandleValidity(in res);
			RenderGraphResourceRegistry.RenderGraphResourcesData[] renderGraphResources = this.m_RenderGraphResources;
			ResourceHandle resourceHandle = res;
			DynamicArray<IRenderGraphResource> resourceArray = renderGraphResources[resourceHandle.iType].resourceArray;
			resourceHandle = res;
			resourceArray[resourceHandle.index]->IncrementWriteCount();
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00008664 File Offset: 0x00006864
		internal unsafe string GetRenderGraphResourceName(in ResourceHandle res)
		{
			this.CheckHandleValidity(in res);
			RenderGraphResourceRegistry.RenderGraphResourcesData[] renderGraphResources = this.m_RenderGraphResources;
			ResourceHandle resourceHandle = res;
			DynamicArray<IRenderGraphResource> resourceArray = renderGraphResources[resourceHandle.iType].resourceArray;
			resourceHandle = res;
			return resourceArray[resourceHandle.index]->GetName();
		}

		// Token: 0x06000185 RID: 389 RVA: 0x000086AB File Offset: 0x000068AB
		internal unsafe string GetRenderGraphResourceName(RenderGraphResourceType type, int index)
		{
			this.CheckHandleValidity(type, index);
			return this.m_RenderGraphResources[(int)type].resourceArray[index]->GetName();
		}

		// Token: 0x06000186 RID: 390 RVA: 0x000086D0 File Offset: 0x000068D0
		internal unsafe bool IsRenderGraphResourceImported(in ResourceHandle res)
		{
			this.CheckHandleValidity(in res);
			RenderGraphResourceRegistry.RenderGraphResourcesData[] renderGraphResources = this.m_RenderGraphResources;
			ResourceHandle resourceHandle = res;
			DynamicArray<IRenderGraphResource> resourceArray = renderGraphResources[resourceHandle.iType].resourceArray;
			resourceHandle = res;
			return resourceArray[resourceHandle.index]->imported;
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00008717 File Offset: 0x00006917
		internal bool IsRenderGraphResourceShared(RenderGraphResourceType type, int index)
		{
			this.CheckHandleValidity(type, index);
			return index < this.m_RenderGraphResources[(int)type].sharedResourcesCount;
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00008734 File Offset: 0x00006934
		internal unsafe bool IsGraphicsResourceCreated(in ResourceHandle res)
		{
			this.CheckHandleValidity(in res);
			RenderGraphResourceRegistry.RenderGraphResourcesData[] renderGraphResources = this.m_RenderGraphResources;
			ResourceHandle resourceHandle = res;
			DynamicArray<IRenderGraphResource> resourceArray = renderGraphResources[resourceHandle.iType].resourceArray;
			resourceHandle = res;
			return resourceArray[resourceHandle.index]->IsCreated();
		}

		// Token: 0x06000189 RID: 393 RVA: 0x0000877B File Offset: 0x0000697B
		internal bool IsRendererListCreated(in RendererListHandle res)
		{
			return this.m_RendererListResources[res].rendererList.isValid;
		}

		// Token: 0x0600018A RID: 394 RVA: 0x0000879D File Offset: 0x0000699D
		internal unsafe bool IsRenderGraphResourceImported(RenderGraphResourceType type, int index)
		{
			this.CheckHandleValidity(type, index);
			return this.m_RenderGraphResources[(int)type].resourceArray[index]->imported;
		}

		// Token: 0x0600018B RID: 395 RVA: 0x000087C0 File Offset: 0x000069C0
		internal unsafe int GetRenderGraphResourceTransientIndex(in ResourceHandle res)
		{
			this.CheckHandleValidity(in res);
			RenderGraphResourceRegistry.RenderGraphResourcesData[] renderGraphResources = this.m_RenderGraphResources;
			ResourceHandle resourceHandle = res;
			DynamicArray<IRenderGraphResource> resourceArray = renderGraphResources[resourceHandle.iType].resourceArray;
			resourceHandle = res;
			return resourceArray[resourceHandle.index]->transientPassIndex;
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00008808 File Offset: 0x00006A08
		internal TextureHandle ImportTexture(RTHandle rt)
		{
			TextureResource textureResource;
			int num = this.m_RenderGraphResources[0].AddNewRenderGraphResource<TextureResource>(out textureResource, true);
			textureResource.graphicsResource = rt;
			textureResource.imported = true;
			return new TextureHandle(num, false);
		}

		// Token: 0x0600018D RID: 397 RVA: 0x0000883C File Offset: 0x00006A3C
		internal unsafe TextureHandle CreateSharedTexture(in TextureDesc desc, bool explicitRelease)
		{
			RenderGraphResourceRegistry.RenderGraphResourcesData renderGraphResourcesData = this.m_RenderGraphResources[0];
			int sharedResourcesCount = renderGraphResourcesData.sharedResourcesCount;
			TextureResource textureResource = null;
			int num = -1;
			for (int i = 0; i < sharedResourcesCount; i++)
			{
				if (!renderGraphResourcesData.resourceArray[i]->shared)
				{
					textureResource = (TextureResource)(*renderGraphResourcesData.resourceArray[i]);
					num = i;
					break;
				}
			}
			if (textureResource == null)
			{
				num = this.m_RenderGraphResources[0].AddNewRenderGraphResource<TextureResource>(out textureResource, false);
				renderGraphResourcesData.sharedResourcesCount++;
			}
			textureResource.imported = true;
			textureResource.shared = true;
			textureResource.sharedExplicitRelease = explicitRelease;
			textureResource.desc = desc;
			return new TextureHandle(num, true);
		}

		// Token: 0x0600018E RID: 398 RVA: 0x000088E8 File Offset: 0x00006AE8
		internal void RefreshSharedTextureDesc(TextureHandle texture, in TextureDesc desc)
		{
			if (!this.IsRenderGraphResourceShared(RenderGraphResourceType.Texture, texture.handle))
			{
				throw new InvalidOperationException(string.Format("Trying to refresh texture {0} that is not a shared resource.", texture));
			}
			TextureResource textureResource = this.GetTextureResource(in texture.handle);
			textureResource.ReleaseGraphicsResource();
			textureResource.desc = desc;
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00008940 File Offset: 0x00006B40
		internal void ReleaseSharedTexture(TextureHandle texture)
		{
			RenderGraphResourceRegistry.RenderGraphResourcesData renderGraphResourcesData = this.m_RenderGraphResources[0];
			if (texture.handle >= renderGraphResourcesData.sharedResourcesCount)
			{
				throw new InvalidOperationException("Tried to release a non shared texture.");
			}
			if (texture.handle == renderGraphResourcesData.sharedResourcesCount - 1)
			{
				renderGraphResourcesData.sharedResourcesCount--;
			}
			TextureResource textureResource = this.GetTextureResource(in texture.handle);
			textureResource.ReleaseGraphicsResource();
			textureResource.Reset(null);
		}

		// Token: 0x06000190 RID: 400 RVA: 0x000089B0 File Offset: 0x00006BB0
		internal TextureHandle ImportBackbuffer(RenderTargetIdentifier rt)
		{
			if (this.m_CurrentBackbuffer != null)
			{
				this.m_CurrentBackbuffer.SetTexture(rt);
			}
			else
			{
				this.m_CurrentBackbuffer = RTHandles.Alloc(rt, "Backbuffer");
			}
			TextureResource textureResource;
			int num = this.m_RenderGraphResources[0].AddNewRenderGraphResource<TextureResource>(out textureResource, true);
			textureResource.graphicsResource = this.m_CurrentBackbuffer;
			textureResource.imported = true;
			return new TextureHandle(num, false);
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00008A10 File Offset: 0x00006C10
		internal TextureHandle CreateTexture(in TextureDesc desc, int transientPassIndex = -1)
		{
			this.ValidateTextureDesc(in desc);
			TextureResource textureResource;
			int num = this.m_RenderGraphResources[0].AddNewRenderGraphResource<TextureResource>(out textureResource, true);
			textureResource.desc = desc;
			textureResource.transientPassIndex = transientPassIndex;
			textureResource.requestFallBack = desc.fallBackToBlackTexture;
			return new TextureHandle(num, false);
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00008A59 File Offset: 0x00006C59
		internal int GetResourceCount(RenderGraphResourceType type)
		{
			return this.m_RenderGraphResources[(int)type].resourceArray.size;
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00008A6D File Offset: 0x00006C6D
		internal int GetTextureResourceCount()
		{
			return this.GetResourceCount(RenderGraphResourceType.Texture);
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00008A76 File Offset: 0x00006C76
		internal unsafe TextureResource GetTextureResource(in ResourceHandle handle)
		{
			return (*this.m_RenderGraphResources[0].resourceArray[handle]) as TextureResource;
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00008A9B File Offset: 0x00006C9B
		internal unsafe TextureDesc GetTextureResourceDesc(in ResourceHandle handle)
		{
			return ((*this.m_RenderGraphResources[0].resourceArray[handle]) as TextureResource).desc;
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00008AC8 File Offset: 0x00006CC8
		internal RendererListHandle CreateRendererList(in RendererListDesc desc)
		{
			this.ValidateRendererListDesc(in desc);
			DynamicArray<RendererListResource> rendererListResources = this.m_RendererListResources;
			RendererListResource rendererListResource = new RendererListResource(in desc);
			return new RendererListHandle(rendererListResources.Add(in rendererListResource));
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00008AF8 File Offset: 0x00006CF8
		internal ComputeBufferHandle ImportComputeBuffer(ComputeBuffer computeBuffer)
		{
			ComputeBufferResource computeBufferResource;
			int num = this.m_RenderGraphResources[1].AddNewRenderGraphResource<ComputeBufferResource>(out computeBufferResource, true);
			computeBufferResource.graphicsResource = computeBuffer;
			computeBufferResource.imported = true;
			return new ComputeBufferHandle(num, false);
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00008B2C File Offset: 0x00006D2C
		internal ComputeBufferHandle CreateComputeBuffer(in ComputeBufferDesc desc, int transientPassIndex = -1)
		{
			this.ValidateComputeBufferDesc(in desc);
			ComputeBufferResource computeBufferResource;
			int num = this.m_RenderGraphResources[1].AddNewRenderGraphResource<ComputeBufferResource>(out computeBufferResource, true);
			computeBufferResource.desc = desc;
			computeBufferResource.transientPassIndex = transientPassIndex;
			return new ComputeBufferHandle(num, false);
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00008B69 File Offset: 0x00006D69
		internal unsafe ComputeBufferDesc GetComputeBufferResourceDesc(in ResourceHandle handle)
		{
			return ((*this.m_RenderGraphResources[1].resourceArray[handle]) as ComputeBufferResource).desc;
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00008B93 File Offset: 0x00006D93
		internal int GetComputeBufferResourceCount()
		{
			return this.GetResourceCount(RenderGraphResourceType.ComputeBuffer);
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00008B9C File Offset: 0x00006D9C
		private unsafe ComputeBufferResource GetComputeBufferResource(in ResourceHandle handle)
		{
			return (*this.m_RenderGraphResources[1].resourceArray[handle]) as ComputeBufferResource;
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00008BC1 File Offset: 0x00006DC1
		internal unsafe void UpdateSharedResourceLastFrameIndex(int type, int index)
		{
			this.m_RenderGraphResources[type].resourceArray[index]->sharedResourceLastFrameUsed = this.m_ExecutionCount;
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00008BE4 File Offset: 0x00006DE4
		private unsafe void ManageSharedRenderGraphResources()
		{
			for (int i = 0; i < 2; i++)
			{
				RenderGraphResourceRegistry.RenderGraphResourcesData renderGraphResourcesData = this.m_RenderGraphResources[i];
				for (int j = 0; j < renderGraphResourcesData.sharedResourcesCount; j++)
				{
					IRenderGraphResource renderGraphResource = *this.m_RenderGraphResources[i].resourceArray[j];
					bool flag = renderGraphResource.IsCreated();
					if (renderGraphResource.sharedResourceLastFrameUsed == this.m_ExecutionCount && !flag)
					{
						renderGraphResource.CreateGraphicsResource(renderGraphResource.GetName());
					}
					else if (flag && !renderGraphResource.sharedExplicitRelease && renderGraphResource.sharedResourceLastFrameUsed + 30 < this.m_ExecutionCount)
					{
						renderGraphResource.ReleaseGraphicsResource();
					}
				}
			}
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00008C7C File Offset: 0x00006E7C
		internal unsafe bool CreatePooledResource(RenderGraphContext rgContext, int type, int index)
		{
			bool? flag = new bool?(false);
			IRenderGraphResource renderGraphResource = *this.m_RenderGraphResources[type].resourceArray[index];
			if (!renderGraphResource.imported)
			{
				renderGraphResource.CreatePooledGraphicsResource();
				if (this.m_RenderGraphDebug.enableLogging)
				{
					renderGraphResource.LogCreation(this.m_FrameInformationLogger);
				}
				RenderGraphResourceRegistry.ResourceCreateCallback createResourceCallback = this.m_RenderGraphResources[type].createResourceCallback;
				flag = ((createResourceCallback != null) ? new bool?(createResourceCallback(rgContext, renderGraphResource)) : null);
			}
			return flag.GetValueOrDefault();
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00008CFD File Offset: 0x00006EFD
		internal bool CreatePooledResource(RenderGraphContext rgContext, ResourceHandle handle)
		{
			return this.CreatePooledResource(rgContext, handle.iType, handle.index);
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00008D14 File Offset: 0x00006F14
		private bool CreateTextureCallback(RenderGraphContext rgContext, IRenderGraphResource res)
		{
			TextureResource textureResource = res as TextureResource;
			FastMemoryDesc fastMemoryDesc = textureResource.desc.fastMemoryDesc;
			if (fastMemoryDesc.inFastMemory)
			{
				textureResource.graphicsResource.SwitchToFastMemory(rgContext.cmd, fastMemoryDesc.residencyFraction, fastMemoryDesc.flags, false);
			}
			bool flag = false;
			if (textureResource.desc.clearBuffer || this.m_RenderGraphDebug.clearRenderTargetsAtCreation)
			{
				bool flag2 = this.m_RenderGraphDebug.clearRenderTargetsAtCreation && !textureResource.desc.clearBuffer;
				using (new ProfilingScope(rgContext.cmd, ProfilingSampler.Get<RenderGraphProfileId>(flag2 ? RenderGraphProfileId.RenderGraphClearDebug : RenderGraphProfileId.RenderGraphClear)))
				{
					ClearFlag clearFlag = ((textureResource.desc.depthBufferBits != DepthBits.None) ? ClearFlag.DepthStencil : ClearFlag.Color);
					Color color = (flag2 ? Color.magenta : textureResource.desc.clearColor);
					CoreUtils.SetRenderTarget(rgContext.cmd, textureResource.graphicsResource, clearFlag, color, 0, CubemapFace.Unknown, -1);
				}
				flag = true;
			}
			return flag;
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00008E18 File Offset: 0x00007018
		internal unsafe void ReleasePooledResource(RenderGraphContext rgContext, int type, int index)
		{
			IRenderGraphResource renderGraphResource = *this.m_RenderGraphResources[type].resourceArray[index];
			if (!renderGraphResource.imported)
			{
				RenderGraphResourceRegistry.ResourceCallback releaseResourceCallback = this.m_RenderGraphResources[type].releaseResourceCallback;
				if (releaseResourceCallback != null)
				{
					releaseResourceCallback(rgContext, renderGraphResource);
				}
				if (this.m_RenderGraphDebug.enableLogging)
				{
					renderGraphResource.LogRelease(this.m_FrameInformationLogger);
				}
				renderGraphResource.ReleasePooledGraphicsResource(this.m_CurrentFrameIndex);
			}
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00008E81 File Offset: 0x00007081
		internal void ReleasePooledResource(RenderGraphContext rgContext, ResourceHandle handle)
		{
			this.ReleasePooledResource(rgContext, handle.iType, handle.index);
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00008E98 File Offset: 0x00007098
		private void ReleaseTextureCallback(RenderGraphContext rgContext, IRenderGraphResource res)
		{
			TextureResource textureResource = res as TextureResource;
			if (this.m_RenderGraphDebug.clearRenderTargetsAtRelease)
			{
				using (new ProfilingScope(rgContext.cmd, ProfilingSampler.Get<RenderGraphProfileId>(RenderGraphProfileId.RenderGraphClearDebug)))
				{
					ClearFlag clearFlag = ((textureResource.desc.depthBufferBits != DepthBits.None) ? ClearFlag.DepthStencil : ClearFlag.Color);
					CoreUtils.SetRenderTarget(rgContext.cmd, textureResource.graphicsResource, clearFlag, Color.magenta, 0, CubemapFace.Unknown, -1);
				}
			}
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00008F18 File Offset: 0x00007118
		private void ValidateTextureDesc(in TextureDesc desc)
		{
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00008F1A File Offset: 0x0000711A
		private void ValidateRendererListDesc(in RendererListDesc desc)
		{
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00008F1C File Offset: 0x0000711C
		private void ValidateComputeBufferDesc(in ComputeBufferDesc desc)
		{
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00008F20 File Offset: 0x00007120
		internal void CreateRendererLists(List<RendererListHandle> rendererLists, ScriptableRenderContext context, bool manualDispatch = false)
		{
			this.m_ActiveRendererLists.Clear();
			foreach (RendererListHandle rendererListHandle in rendererLists)
			{
				ref RendererListResource ptr = ref this.m_RendererListResources[rendererListHandle];
				ref RendererListDesc ptr2 = ref ptr.desc;
				ptr.rendererList = context.CreateRendererList(ptr2);
				this.m_ActiveRendererLists.Add(ptr.rendererList);
			}
			if (manualDispatch)
			{
				context.PrepareRendererListsAsync(this.m_ActiveRendererLists);
			}
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00008FC0 File Offset: 0x000071C0
		internal void Clear(bool onException)
		{
			this.LogResources();
			for (int i = 0; i < 2; i++)
			{
				this.m_RenderGraphResources[i].Clear(onException, this.m_CurrentFrameIndex);
			}
			this.m_RendererListResources.Clear();
			this.m_ActiveRendererLists.Clear();
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x0000900C File Offset: 0x0000720C
		internal void PurgeUnusedGraphicsResources()
		{
			for (int i = 0; i < 2; i++)
			{
				this.m_RenderGraphResources[i].PurgeUnusedGraphicsResources(this.m_CurrentFrameIndex);
			}
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00009038 File Offset: 0x00007238
		internal void Cleanup()
		{
			for (int i = 0; i < 2; i++)
			{
				this.m_RenderGraphResources[i].Cleanup();
			}
			RTHandles.Release(this.m_CurrentBackbuffer);
		}

		// Token: 0x060001AB RID: 427 RVA: 0x00009069 File Offset: 0x00007269
		internal void FlushLogs()
		{
			Debug.Log(this.m_ResourceLogger.GetAllLogs());
		}

		// Token: 0x060001AC RID: 428 RVA: 0x0000907C File Offset: 0x0000727C
		private void LogResources()
		{
			if (this.m_RenderGraphDebug.enableLogging)
			{
				this.m_ResourceLogger.LogLine("==== Allocated Resources ====\n", Array.Empty<object>());
				for (int i = 0; i < 2; i++)
				{
					this.m_RenderGraphResources[i].pool.LogResources(this.m_ResourceLogger);
					this.m_ResourceLogger.LogLine("", Array.Empty<object>());
				}
			}
		}

		// Token: 0x040000CB RID: 203
		private const int kSharedResourceLifetime = 30;

		// Token: 0x040000CC RID: 204
		private static RenderGraphResourceRegistry m_CurrentRegistry;

		// Token: 0x040000CD RID: 205
		private RenderGraphResourceRegistry.RenderGraphResourcesData[] m_RenderGraphResources = new RenderGraphResourceRegistry.RenderGraphResourcesData[2];

		// Token: 0x040000CE RID: 206
		private DynamicArray<RendererListResource> m_RendererListResources = new DynamicArray<RendererListResource>();

		// Token: 0x040000CF RID: 207
		private RenderGraphDebugParams m_RenderGraphDebug;

		// Token: 0x040000D0 RID: 208
		private RenderGraphLogger m_ResourceLogger = new RenderGraphLogger();

		// Token: 0x040000D1 RID: 209
		private RenderGraphLogger m_FrameInformationLogger;

		// Token: 0x040000D2 RID: 210
		private int m_CurrentFrameIndex;

		// Token: 0x040000D3 RID: 211
		private int m_ExecutionCount;

		// Token: 0x040000D4 RID: 212
		private RTHandle m_CurrentBackbuffer;

		// Token: 0x040000D5 RID: 213
		private const int kInitialRendererListCount = 256;

		// Token: 0x040000D6 RID: 214
		private List<RendererList> m_ActiveRendererLists = new List<RendererList>(256);

		// Token: 0x02000151 RID: 337
		// (Invoke) Token: 0x060009C4 RID: 2500
		private delegate bool ResourceCreateCallback(RenderGraphContext rgContext, IRenderGraphResource res);

		// Token: 0x02000152 RID: 338
		// (Invoke) Token: 0x060009C8 RID: 2504
		private delegate void ResourceCallback(RenderGraphContext rgContext, IRenderGraphResource res);

		// Token: 0x02000153 RID: 339
		private class RenderGraphResourcesData
		{
			// Token: 0x060009CB RID: 2507 RVA: 0x0002C13E File Offset: 0x0002A33E
			public void Clear(bool onException, int frameIndex)
			{
				this.resourceArray.Resize(this.sharedResourcesCount, false);
				this.pool.CheckFrameAllocation(onException, frameIndex);
			}

			// Token: 0x060009CC RID: 2508 RVA: 0x0002C160 File Offset: 0x0002A360
			public unsafe void Cleanup()
			{
				for (int i = 0; i < this.sharedResourcesCount; i++)
				{
					IRenderGraphResource renderGraphResource = *this.resourceArray[i];
					if (renderGraphResource != null)
					{
						renderGraphResource.ReleaseGraphicsResource();
					}
				}
				this.pool.Cleanup();
			}

			// Token: 0x060009CD RID: 2509 RVA: 0x0002C1A0 File Offset: 0x0002A3A0
			public void PurgeUnusedGraphicsResources(int frameIndex)
			{
				this.pool.PurgeUnusedResources(frameIndex);
			}

			// Token: 0x060009CE RID: 2510 RVA: 0x0002C1B0 File Offset: 0x0002A3B0
			public unsafe int AddNewRenderGraphResource<ResType>(out ResType outRes, bool pooledResource = true) where ResType : IRenderGraphResource, new()
			{
				int size = this.resourceArray.size;
				this.resourceArray.Resize(this.resourceArray.size + 1, true);
				if (*this.resourceArray[size] == null)
				{
					*this.resourceArray[size] = new ResType();
				}
				outRes = (*this.resourceArray[size]) as ResType;
				outRes.Reset(pooledResource ? this.pool : null);
				return size;
			}

			// Token: 0x040005D3 RID: 1491
			public DynamicArray<IRenderGraphResource> resourceArray = new DynamicArray<IRenderGraphResource>();

			// Token: 0x040005D4 RID: 1492
			public int sharedResourcesCount;

			// Token: 0x040005D5 RID: 1493
			public IRenderGraphResourcePool pool;

			// Token: 0x040005D6 RID: 1494
			public RenderGraphResourceRegistry.ResourceCreateCallback createResourceCallback;

			// Token: 0x040005D7 RID: 1495
			public RenderGraphResourceRegistry.ResourceCallback releaseResourceCallback;
		}
	}
}
