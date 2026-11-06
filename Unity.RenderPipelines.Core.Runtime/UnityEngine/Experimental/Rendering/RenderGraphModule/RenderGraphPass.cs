using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.Rendering;

namespace UnityEngine.Experimental.Rendering.RenderGraphModule
{
	// Token: 0x0200001C RID: 28
	[DebuggerDisplay("RenderPass: {name} (Index:{index} Async:{enableAsyncCompute})")]
	internal abstract class RenderGraphPass
	{
		// Token: 0x06000121 RID: 289 RVA: 0x00007761 File Offset: 0x00005961
		public RenderFunc<PassData> GetExecuteDelegate<PassData>() where PassData : class, new()
		{
			return ((RenderGraphPass<PassData>)this).renderFunc;
		}

		// Token: 0x06000122 RID: 290
		public abstract void Execute(RenderGraphContext renderGraphContext);

		// Token: 0x06000123 RID: 291
		public abstract void Release(RenderGraphObjectPool pool);

		// Token: 0x06000124 RID: 292
		public abstract bool HasRenderFunc();

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000125 RID: 293 RVA: 0x0000776E File Offset: 0x0000596E
		// (set) Token: 0x06000126 RID: 294 RVA: 0x00007776 File Offset: 0x00005976
		public string name { get; protected set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000127 RID: 295 RVA: 0x0000777F File Offset: 0x0000597F
		// (set) Token: 0x06000128 RID: 296 RVA: 0x00007787 File Offset: 0x00005987
		public int index { get; protected set; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000129 RID: 297 RVA: 0x00007790 File Offset: 0x00005990
		// (set) Token: 0x0600012A RID: 298 RVA: 0x00007798 File Offset: 0x00005998
		public ProfilingSampler customSampler { get; protected set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600012B RID: 299 RVA: 0x000077A1 File Offset: 0x000059A1
		// (set) Token: 0x0600012C RID: 300 RVA: 0x000077A9 File Offset: 0x000059A9
		public bool enableAsyncCompute { get; protected set; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600012D RID: 301 RVA: 0x000077B2 File Offset: 0x000059B2
		// (set) Token: 0x0600012E RID: 302 RVA: 0x000077BA File Offset: 0x000059BA
		public bool allowPassCulling { get; protected set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600012F RID: 303 RVA: 0x000077C3 File Offset: 0x000059C3
		// (set) Token: 0x06000130 RID: 304 RVA: 0x000077CB File Offset: 0x000059CB
		public TextureHandle depthBuffer { get; protected set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000131 RID: 305 RVA: 0x000077D4 File Offset: 0x000059D4
		// (set) Token: 0x06000132 RID: 306 RVA: 0x000077DC File Offset: 0x000059DC
		public TextureHandle[] colorBuffers { get; protected set; } = new TextureHandle[RenderGraph.kMaxMRTCount];

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000133 RID: 307 RVA: 0x000077E5 File Offset: 0x000059E5
		// (set) Token: 0x06000134 RID: 308 RVA: 0x000077ED File Offset: 0x000059ED
		public int colorBufferMaxIndex { get; protected set; } = -1;

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000135 RID: 309 RVA: 0x000077F6 File Offset: 0x000059F6
		// (set) Token: 0x06000136 RID: 310 RVA: 0x000077FE File Offset: 0x000059FE
		public int refCount { get; protected set; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000137 RID: 311 RVA: 0x00007807 File Offset: 0x00005A07
		// (set) Token: 0x06000138 RID: 312 RVA: 0x0000780F File Offset: 0x00005A0F
		public bool generateDebugData { get; protected set; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000139 RID: 313 RVA: 0x00007818 File Offset: 0x00005A18
		// (set) Token: 0x0600013A RID: 314 RVA: 0x00007820 File Offset: 0x00005A20
		public bool allowRendererListCulling { get; protected set; }

		// Token: 0x0600013B RID: 315 RVA: 0x0000782C File Offset: 0x00005A2C
		public RenderGraphPass()
		{
			for (int i = 0; i < 2; i++)
			{
				this.resourceReadLists[i] = new List<ResourceHandle>();
				this.resourceWriteLists[i] = new List<ResourceHandle>();
				this.transientResourceList[i] = new List<ResourceHandle>();
			}
		}

		// Token: 0x0600013C RID: 316 RVA: 0x000078B8 File Offset: 0x00005AB8
		public void Clear()
		{
			this.name = "";
			this.index = -1;
			this.customSampler = null;
			for (int i = 0; i < 2; i++)
			{
				this.resourceReadLists[i].Clear();
				this.resourceWriteLists[i].Clear();
				this.transientResourceList[i].Clear();
			}
			this.usedRendererListList.Clear();
			this.enableAsyncCompute = false;
			this.allowPassCulling = true;
			this.allowRendererListCulling = true;
			this.generateDebugData = true;
			this.refCount = 0;
			this.colorBufferMaxIndex = -1;
			this.depthBuffer = TextureHandle.nullHandle;
			for (int j = 0; j < RenderGraph.kMaxMRTCount; j++)
			{
				this.colorBuffers[j] = TextureHandle.nullHandle;
			}
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00007974 File Offset: 0x00005B74
		public void AddResourceWrite(in ResourceHandle res)
		{
			List<ResourceHandle>[] array = this.resourceWriteLists;
			ResourceHandle resourceHandle = res;
			array[resourceHandle.iType].Add(res);
		}

		// Token: 0x0600013E RID: 318 RVA: 0x000079A4 File Offset: 0x00005BA4
		public void AddResourceRead(in ResourceHandle res)
		{
			List<ResourceHandle>[] array = this.resourceReadLists;
			ResourceHandle resourceHandle = res;
			array[resourceHandle.iType].Add(res);
		}

		// Token: 0x0600013F RID: 319 RVA: 0x000079D4 File Offset: 0x00005BD4
		public void AddTransientResource(in ResourceHandle res)
		{
			List<ResourceHandle>[] array = this.transientResourceList;
			ResourceHandle resourceHandle = res;
			array[resourceHandle.iType].Add(res);
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00007A01 File Offset: 0x00005C01
		public void UseRendererList(RendererListHandle rendererList)
		{
			this.usedRendererListList.Add(rendererList);
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00007A0F File Offset: 0x00005C0F
		public void EnableAsyncCompute(bool value)
		{
			this.enableAsyncCompute = value;
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00007A18 File Offset: 0x00005C18
		public void AllowPassCulling(bool value)
		{
			this.allowPassCulling = value;
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00007A21 File Offset: 0x00005C21
		public void AllowRendererListCulling(bool value)
		{
			this.allowRendererListCulling = value;
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00007A2A File Offset: 0x00005C2A
		public void GenerateDebugData(bool value)
		{
			this.generateDebugData = value;
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00007A33 File Offset: 0x00005C33
		public void SetColorBuffer(TextureHandle resource, int index)
		{
			this.colorBufferMaxIndex = Math.Max(this.colorBufferMaxIndex, index);
			this.colorBuffers[index] = resource;
			this.AddResourceWrite(in resource.handle);
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00007A61 File Offset: 0x00005C61
		public void SetDepthBuffer(TextureHandle resource, DepthAccess flags)
		{
			this.depthBuffer = resource;
			if ((flags & DepthAccess.Read) != (DepthAccess)0)
			{
				this.AddResourceRead(in resource.handle);
			}
			if ((flags & DepthAccess.Write) != (DepthAccess)0)
			{
				this.AddResourceWrite(in resource.handle);
			}
		}

		// Token: 0x040000B5 RID: 181
		public List<ResourceHandle>[] resourceReadLists = new List<ResourceHandle>[2];

		// Token: 0x040000B6 RID: 182
		public List<ResourceHandle>[] resourceWriteLists = new List<ResourceHandle>[2];

		// Token: 0x040000B7 RID: 183
		public List<ResourceHandle>[] transientResourceList = new List<ResourceHandle>[2];

		// Token: 0x040000B8 RID: 184
		public List<RendererListHandle> usedRendererListList = new List<RendererListHandle>();
	}
}
