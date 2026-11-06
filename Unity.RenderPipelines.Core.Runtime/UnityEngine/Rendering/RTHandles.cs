using System;
using UnityEngine.Experimental.Rendering;

namespace UnityEngine.Rendering
{
	// Token: 0x020000BB RID: 187
	public static class RTHandles
	{
		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x0600059E RID: 1438 RVA: 0x0001CCA2 File Offset: 0x0001AEA2
		public static int maxWidth
		{
			get
			{
				return RTHandles.s_DefaultInstance.GetMaxWidth();
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x0600059F RID: 1439 RVA: 0x0001CCAE File Offset: 0x0001AEAE
		public static int maxHeight
		{
			get
			{
				return RTHandles.s_DefaultInstance.GetMaxHeight();
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060005A0 RID: 1440 RVA: 0x0001CCBA File Offset: 0x0001AEBA
		public static RTHandleProperties rtHandleProperties
		{
			get
			{
				return RTHandles.s_DefaultInstance.rtHandleProperties;
			}
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x0001CCC8 File Offset: 0x0001AEC8
		public static RTHandle Alloc(int width, int height, int slices = 1, DepthBits depthBufferBits = DepthBits.None, GraphicsFormat colorFormat = GraphicsFormat.R8G8B8A8_SRGB, FilterMode filterMode = FilterMode.Point, TextureWrapMode wrapMode = TextureWrapMode.Repeat, TextureDimension dimension = TextureDimension.Tex2D, bool enableRandomWrite = false, bool useMipMap = false, bool autoGenerateMips = true, bool isShadowMap = false, int anisoLevel = 1, float mipMapBias = 0f, MSAASamples msaaSamples = MSAASamples.None, bool bindTextureMS = false, bool useDynamicScale = false, RenderTextureMemoryless memoryless = RenderTextureMemoryless.None, VRTextureUsage vrUsage = VRTextureUsage.None, string name = "")
		{
			return RTHandles.s_DefaultInstance.Alloc(width, height, slices, depthBufferBits, colorFormat, filterMode, wrapMode, dimension, enableRandomWrite, useMipMap, autoGenerateMips, isShadowMap, anisoLevel, mipMapBias, msaaSamples, bindTextureMS, useDynamicScale, memoryless, vrUsage, name);
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x0001CD04 File Offset: 0x0001AF04
		public static RTHandle Alloc(int width, int height, TextureWrapMode wrapModeU, TextureWrapMode wrapModeV, TextureWrapMode wrapModeW = TextureWrapMode.Repeat, int slices = 1, DepthBits depthBufferBits = DepthBits.None, GraphicsFormat colorFormat = GraphicsFormat.R8G8B8A8_SRGB, FilterMode filterMode = FilterMode.Point, TextureDimension dimension = TextureDimension.Tex2D, bool enableRandomWrite = false, bool useMipMap = false, bool autoGenerateMips = true, bool isShadowMap = false, int anisoLevel = 1, float mipMapBias = 0f, MSAASamples msaaSamples = MSAASamples.None, bool bindTextureMS = false, bool useDynamicScale = false, RenderTextureMemoryless memoryless = RenderTextureMemoryless.None, VRTextureUsage vrUsage = VRTextureUsage.None, string name = "")
		{
			return RTHandles.s_DefaultInstance.Alloc(width, height, wrapModeU, wrapModeV, wrapModeW, slices, depthBufferBits, colorFormat, filterMode, dimension, enableRandomWrite, useMipMap, autoGenerateMips, isShadowMap, anisoLevel, mipMapBias, msaaSamples, bindTextureMS, useDynamicScale, memoryless, vrUsage, name);
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x0001CD44 File Offset: 0x0001AF44
		public static RTHandle Alloc(in RenderTextureDescriptor descriptor, FilterMode filterMode = FilterMode.Point, TextureWrapMode wrapMode = TextureWrapMode.Repeat, bool isShadowMap = false, int anisoLevel = 1, float mipMapBias = 0f, string name = "")
		{
			RTHandleSystem rthandleSystem = RTHandles.s_DefaultInstance;
			int width = descriptor.width;
			int height = descriptor.height;
			int volumeDepth = descriptor.volumeDepth;
			RenderTextureDescriptor renderTextureDescriptor = descriptor;
			DepthBits depthBufferBits = (DepthBits)renderTextureDescriptor.depthBufferBits;
			renderTextureDescriptor = descriptor;
			GraphicsFormat graphicsFormat = renderTextureDescriptor.graphicsFormat;
			TextureDimension dimension = descriptor.dimension;
			renderTextureDescriptor = descriptor;
			bool enableRandomWrite = renderTextureDescriptor.enableRandomWrite;
			renderTextureDescriptor = descriptor;
			bool useMipMap = renderTextureDescriptor.useMipMap;
			renderTextureDescriptor = descriptor;
			bool autoGenerateMips = renderTextureDescriptor.autoGenerateMips;
			MSAASamples msaaSamples = (MSAASamples)descriptor.msaaSamples;
			renderTextureDescriptor = descriptor;
			bool bindMS = renderTextureDescriptor.bindMS;
			renderTextureDescriptor = descriptor;
			return rthandleSystem.Alloc(width, height, volumeDepth, depthBufferBits, graphicsFormat, filterMode, wrapMode, dimension, enableRandomWrite, useMipMap, autoGenerateMips, isShadowMap, anisoLevel, mipMapBias, msaaSamples, bindMS, renderTextureDescriptor.useDynamicScale, descriptor.memoryless, descriptor.vrUsage, name);
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x0001CDF0 File Offset: 0x0001AFF0
		public static RTHandle Alloc(Vector2 scaleFactor, int slices = 1, DepthBits depthBufferBits = DepthBits.None, GraphicsFormat colorFormat = GraphicsFormat.R8G8B8A8_SRGB, FilterMode filterMode = FilterMode.Point, TextureWrapMode wrapMode = TextureWrapMode.Repeat, TextureDimension dimension = TextureDimension.Tex2D, bool enableRandomWrite = false, bool useMipMap = false, bool autoGenerateMips = true, bool isShadowMap = false, int anisoLevel = 1, float mipMapBias = 0f, MSAASamples msaaSamples = MSAASamples.None, bool bindTextureMS = false, bool useDynamicScale = false, RenderTextureMemoryless memoryless = RenderTextureMemoryless.None, VRTextureUsage vrUsage = VRTextureUsage.None, string name = "")
		{
			return RTHandles.s_DefaultInstance.Alloc(scaleFactor, slices, depthBufferBits, colorFormat, filterMode, wrapMode, dimension, enableRandomWrite, useMipMap, autoGenerateMips, isShadowMap, anisoLevel, mipMapBias, msaaSamples, bindTextureMS, useDynamicScale, memoryless, vrUsage, name);
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x0001CE2C File Offset: 0x0001B02C
		public static RTHandle Alloc(Vector2 scaleFactor, in RenderTextureDescriptor descriptor, FilterMode filterMode = FilterMode.Point, TextureWrapMode wrapMode = TextureWrapMode.Repeat, bool isShadowMap = false, int anisoLevel = 1, float mipMapBias = 0f, string name = "")
		{
			RTHandleSystem rthandleSystem = RTHandles.s_DefaultInstance;
			int volumeDepth = descriptor.volumeDepth;
			RenderTextureDescriptor renderTextureDescriptor = descriptor;
			DepthBits depthBufferBits = (DepthBits)renderTextureDescriptor.depthBufferBits;
			renderTextureDescriptor = descriptor;
			GraphicsFormat graphicsFormat = renderTextureDescriptor.graphicsFormat;
			TextureDimension dimension = descriptor.dimension;
			renderTextureDescriptor = descriptor;
			bool enableRandomWrite = renderTextureDescriptor.enableRandomWrite;
			renderTextureDescriptor = descriptor;
			bool useMipMap = renderTextureDescriptor.useMipMap;
			renderTextureDescriptor = descriptor;
			bool autoGenerateMips = renderTextureDescriptor.autoGenerateMips;
			MSAASamples msaaSamples = (MSAASamples)descriptor.msaaSamples;
			renderTextureDescriptor = descriptor;
			bool bindMS = renderTextureDescriptor.bindMS;
			renderTextureDescriptor = descriptor;
			return rthandleSystem.Alloc(scaleFactor, volumeDepth, depthBufferBits, graphicsFormat, filterMode, wrapMode, dimension, enableRandomWrite, useMipMap, autoGenerateMips, isShadowMap, anisoLevel, mipMapBias, msaaSamples, bindMS, renderTextureDescriptor.useDynamicScale, descriptor.memoryless, descriptor.vrUsage, name);
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x0001CED0 File Offset: 0x0001B0D0
		public static RTHandle Alloc(ScaleFunc scaleFunc, int slices = 1, DepthBits depthBufferBits = DepthBits.None, GraphicsFormat colorFormat = GraphicsFormat.R8G8B8A8_SRGB, FilterMode filterMode = FilterMode.Point, TextureWrapMode wrapMode = TextureWrapMode.Repeat, TextureDimension dimension = TextureDimension.Tex2D, bool enableRandomWrite = false, bool useMipMap = false, bool autoGenerateMips = true, bool isShadowMap = false, int anisoLevel = 1, float mipMapBias = 0f, MSAASamples msaaSamples = MSAASamples.None, bool bindTextureMS = false, bool useDynamicScale = false, RenderTextureMemoryless memoryless = RenderTextureMemoryless.None, VRTextureUsage vrUsage = VRTextureUsage.None, string name = "")
		{
			return RTHandles.s_DefaultInstance.Alloc(scaleFunc, slices, depthBufferBits, colorFormat, filterMode, wrapMode, dimension, enableRandomWrite, useMipMap, autoGenerateMips, isShadowMap, anisoLevel, mipMapBias, msaaSamples, bindTextureMS, useDynamicScale, memoryless, vrUsage, name);
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x0001CF0C File Offset: 0x0001B10C
		public static RTHandle Alloc(ScaleFunc scaleFunc, in RenderTextureDescriptor descriptor, FilterMode filterMode = FilterMode.Point, TextureWrapMode wrapMode = TextureWrapMode.Repeat, bool isShadowMap = false, int anisoLevel = 1, float mipMapBias = 0f, string name = "")
		{
			RTHandleSystem rthandleSystem = RTHandles.s_DefaultInstance;
			int volumeDepth = descriptor.volumeDepth;
			RenderTextureDescriptor renderTextureDescriptor = descriptor;
			DepthBits depthBufferBits = (DepthBits)renderTextureDescriptor.depthBufferBits;
			renderTextureDescriptor = descriptor;
			GraphicsFormat graphicsFormat = renderTextureDescriptor.graphicsFormat;
			TextureDimension dimension = descriptor.dimension;
			renderTextureDescriptor = descriptor;
			bool enableRandomWrite = renderTextureDescriptor.enableRandomWrite;
			renderTextureDescriptor = descriptor;
			bool useMipMap = renderTextureDescriptor.useMipMap;
			renderTextureDescriptor = descriptor;
			bool autoGenerateMips = renderTextureDescriptor.autoGenerateMips;
			MSAASamples msaaSamples = (MSAASamples)descriptor.msaaSamples;
			renderTextureDescriptor = descriptor;
			bool bindMS = renderTextureDescriptor.bindMS;
			renderTextureDescriptor = descriptor;
			return rthandleSystem.Alloc(scaleFunc, volumeDepth, depthBufferBits, graphicsFormat, filterMode, wrapMode, dimension, enableRandomWrite, useMipMap, autoGenerateMips, isShadowMap, anisoLevel, mipMapBias, msaaSamples, bindMS, renderTextureDescriptor.useDynamicScale, descriptor.memoryless, descriptor.vrUsage, name);
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x0001CFAE File Offset: 0x0001B1AE
		public static RTHandle Alloc(Texture tex)
		{
			return RTHandles.s_DefaultInstance.Alloc(tex);
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x0001CFBB File Offset: 0x0001B1BB
		public static RTHandle Alloc(RenderTexture tex)
		{
			return RTHandles.s_DefaultInstance.Alloc(tex);
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x0001CFC8 File Offset: 0x0001B1C8
		public static RTHandle Alloc(RenderTargetIdentifier tex)
		{
			return RTHandles.s_DefaultInstance.Alloc(tex);
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x0001CFD5 File Offset: 0x0001B1D5
		public static RTHandle Alloc(RenderTargetIdentifier tex, string name)
		{
			return RTHandles.s_DefaultInstance.Alloc(tex, name);
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x0001CFE3 File Offset: 0x0001B1E3
		private static RTHandle Alloc(RTHandle tex)
		{
			Debug.LogError("Allocation a RTHandle from another one is forbidden.");
			return null;
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x0001CFF0 File Offset: 0x0001B1F0
		public static void Initialize(int width, int height)
		{
			RTHandles.s_DefaultInstance.Initialize(width, height);
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x0001CFFE File Offset: 0x0001B1FE
		[Obsolete("useLegacyDynamicResControl is deprecated. Please use SetHardwareDynamicResolutionState() instead.")]
		public static void Initialize(int width, int height, bool useLegacyDynamicResControl = false)
		{
			RTHandles.s_DefaultInstance.Initialize(width, height, useLegacyDynamicResControl);
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x0001D00D File Offset: 0x0001B20D
		public static void Release(RTHandle rth)
		{
			RTHandles.s_DefaultInstance.Release(rth);
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x0001D01A File Offset: 0x0001B21A
		public static void SetHardwareDynamicResolutionState(bool hwDynamicResRequested)
		{
			RTHandles.s_DefaultInstance.SetHardwareDynamicResolutionState(hwDynamicResRequested);
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x0001D027 File Offset: 0x0001B227
		public static void SetReferenceSize(int width, int height)
		{
			RTHandles.s_DefaultInstance.SetReferenceSize(width, height);
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x0001D035 File Offset: 0x0001B235
		public static void ResetReferenceSize(int width, int height)
		{
			RTHandles.s_DefaultInstance.ResetReferenceSize(width, height);
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x0001D044 File Offset: 0x0001B244
		public static Vector2 CalculateRatioAgainstMaxSize(int width, int height)
		{
			RTHandleSystem rthandleSystem = RTHandles.s_DefaultInstance;
			Vector2Int vector2Int = new Vector2Int(width, height);
			return rthandleSystem.CalculateRatioAgainstMaxSize(in vector2Int);
		}

		// Token: 0x04000414 RID: 1044
		private static RTHandleSystem s_DefaultInstance = new RTHandleSystem();
	}
}
