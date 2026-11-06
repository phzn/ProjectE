using System;
using System.Runtime.CompilerServices;
using UnityEngine.Experimental.Rendering;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000D9 RID: 217
	internal sealed class TaaPersistentData
	{
		// Token: 0x17000197 RID: 407
		// (get) Token: 0x060006D0 RID: 1744 RVA: 0x0002898B File Offset: 0x00026B8B
		public RenderTextureDescriptor rtd
		{
			get
			{
				return this.m_RtDesc;
			}
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x00028993 File Offset: 0x00026B93
		public RTHandle accumulationTexture(int index)
		{
			if (index == 0)
			{
				return this.m_AccumulationTexture;
			}
			return this.m_AccumulationTexture2;
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x000289A5 File Offset: 0x00026BA5
		public int GetLastAccumFrameIndex(int index)
		{
			if (index == 0)
			{
				return this.m_LastAccumUpdateFrameIndex;
			}
			return this.m_LastAccumUpdateFrameIndex2;
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x000289B7 File Offset: 0x00026BB7
		public void SetLastAccumFrameIndex(int index, int value)
		{
			if (index != 0)
			{
				this.m_LastAccumUpdateFrameIndex2 = value;
				return;
			}
			this.m_LastAccumUpdateFrameIndex = value;
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x000289D4 File Offset: 0x00026BD4
		public void Init(int sizeX, int sizeY, int volumeDepth, GraphicsFormat format, VRTextureUsage vrUsage, TextureDimension texDim)
		{
			if ((this.m_RtDesc.width != sizeX || this.m_RtDesc.height != sizeY || this.m_RtDesc.volumeDepth != volumeDepth || this.m_AccumulationTexture == null) && sizeX > 0 && sizeY > 0)
			{
				RenderTextureDescriptor renderTextureDescriptor = default(RenderTextureDescriptor);
				FormatUsage formatUsage = FormatUsage.Render;
				renderTextureDescriptor.width = sizeX;
				renderTextureDescriptor.height = sizeY;
				renderTextureDescriptor.msaaSamples = 1;
				renderTextureDescriptor.volumeDepth = volumeDepth;
				renderTextureDescriptor.mipCount = 0;
				renderTextureDescriptor.graphicsFormat = TaaPersistentData.<Init>g__CheckFormat|12_0(format, formatUsage);
				renderTextureDescriptor.sRGB = false;
				renderTextureDescriptor.depthBufferBits = 0;
				renderTextureDescriptor.dimension = texDim;
				renderTextureDescriptor.vrUsage = vrUsage;
				renderTextureDescriptor.memoryless = RenderTextureMemoryless.None;
				renderTextureDescriptor.useMipMap = false;
				renderTextureDescriptor.autoGenerateMips = false;
				renderTextureDescriptor.enableRandomWrite = false;
				renderTextureDescriptor.bindMS = false;
				renderTextureDescriptor.useDynamicScale = false;
				this.m_RtDesc = renderTextureDescriptor;
				this.DeallocateTargets();
			}
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x00028AC4 File Offset: 0x00026CC4
		public bool AllocateTargets(bool xrMultipassEnabled = false)
		{
			bool flag = false;
			if (this.m_AccumulationTexture == null)
			{
				this.m_AccumulationTexture = RTHandles.Alloc(in this.m_RtDesc, FilterMode.Bilinear, TextureWrapMode.Clamp, false, 1, 0f, "_TaaAccumulationTex");
				flag = true;
			}
			if (xrMultipassEnabled && this.m_AccumulationTexture2 == null)
			{
				this.m_AccumulationTexture2 = RTHandles.Alloc(in this.m_RtDesc, FilterMode.Bilinear, TextureWrapMode.Clamp, false, 1, 0f, "_TaaAccumulationTex2");
				flag = true;
			}
			return flag;
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x00028B29 File Offset: 0x00026D29
		public void DeallocateTargets()
		{
			RTHandle accumulationTexture = this.m_AccumulationTexture;
			if (accumulationTexture != null)
			{
				accumulationTexture.Release();
			}
			RTHandle accumulationTexture2 = this.m_AccumulationTexture2;
			if (accumulationTexture2 != null)
			{
				accumulationTexture2.Release();
			}
			this.m_AccumulationTexture = null;
			this.m_AccumulationTexture2 = null;
			this.m_LastAccumUpdateFrameIndex = -1;
			this.m_LastAccumUpdateFrameIndex2 = -1;
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x00028B81 File Offset: 0x00026D81
		[CompilerGenerated]
		internal static GraphicsFormat <Init>g__CheckFormat|12_0(GraphicsFormat format, FormatUsage usage)
		{
			if (!SystemInfo.IsFormatSupported(format, usage))
			{
				return TaaPersistentData.<Init>g__FindFormat|12_1(usage);
			}
			return format;
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x00028B94 File Offset: 0x00026D94
		[CompilerGenerated]
		internal static GraphicsFormat <Init>g__FindFormat|12_1(FormatUsage usage)
		{
			for (int i = 0; i < TaaPersistentData.formatList.Length; i++)
			{
				if (SystemInfo.IsFormatSupported(TaaPersistentData.formatList[i], usage))
				{
					return TaaPersistentData.formatList[i];
				}
			}
			return GraphicsFormat.B8G8R8A8_UNorm;
		}

		// Token: 0x04000566 RID: 1382
		private static GraphicsFormat[] formatList = new GraphicsFormat[]
		{
			GraphicsFormat.R16G16B16A16_SFloat,
			GraphicsFormat.B10G11R11_UFloatPack32,
			GraphicsFormat.R8G8B8A8_UNorm,
			GraphicsFormat.B8G8R8A8_UNorm
		};

		// Token: 0x04000567 RID: 1383
		private RenderTextureDescriptor m_RtDesc;

		// Token: 0x04000568 RID: 1384
		private RTHandle m_AccumulationTexture;

		// Token: 0x04000569 RID: 1385
		private RTHandle m_AccumulationTexture2;

		// Token: 0x0400056A RID: 1386
		private int m_LastAccumUpdateFrameIndex;

		// Token: 0x0400056B RID: 1387
		private int m_LastAccumUpdateFrameIndex2;
	}
}
