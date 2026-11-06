using System;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering;

namespace UnityEngine.Rendering
{
	// Token: 0x020000BE RID: 190
	public class RTHandleSystem : IDisposable
	{
		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060005B9 RID: 1465 RVA: 0x0001D071 File Offset: 0x0001B271
		public RTHandleProperties rtHandleProperties
		{
			get
			{
				return this.m_RTHandleProperties;
			}
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x0001D079 File Offset: 0x0001B279
		public RTHandleSystem()
		{
			this.m_AutoSizedRTs = new HashSet<RTHandle>();
			this.m_ResizeOnDemandRTs = new HashSet<RTHandle>();
			this.m_MaxWidths = 1;
			this.m_MaxHeights = 1;
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x0001D0A5 File Offset: 0x0001B2A5
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x0001D0B0 File Offset: 0x0001B2B0
		public void Initialize(int width, int height)
		{
			if (this.m_AutoSizedRTs.Count != 0)
			{
				string text = "Unreleased RTHandles:";
				foreach (RTHandle rthandle in this.m_AutoSizedRTs)
				{
					text = string.Format("{0}\n    {1}", text, rthandle.name);
				}
				Debug.LogError(string.Format("RTHandle.Initialize should only be called once before allocating any Render Texture. This may be caused by an unreleased RTHandle resource.\n{0}\n", text));
			}
			this.m_MaxWidths = width;
			this.m_MaxHeights = height;
			this.m_HardwareDynamicResRequested = DynamicResolutionHandler.instance.RequestsHardwareDynamicResolution();
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x0001D150 File Offset: 0x0001B350
		[Obsolete("useLegacyDynamicResControl is deprecated. Please use SetHardwareDynamicResolutionState() instead.")]
		public void Initialize(int width, int height, bool useLegacyDynamicResControl = false)
		{
			this.Initialize(width, height);
			if (useLegacyDynamicResControl)
			{
				this.m_HardwareDynamicResRequested = true;
			}
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x0001D164 File Offset: 0x0001B364
		public void Release(RTHandle rth)
		{
			if (rth != null)
			{
				rth.Release();
			}
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x0001D16F File Offset: 0x0001B36F
		internal void Remove(RTHandle rth)
		{
			this.m_AutoSizedRTs.Remove(rth);
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x0001D17E File Offset: 0x0001B37E
		public void ResetReferenceSize(int width, int height)
		{
			this.m_MaxWidths = width;
			this.m_MaxHeights = height;
			this.SetReferenceSize(width, height, true);
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x0001D197 File Offset: 0x0001B397
		public void SetReferenceSize(int width, int height)
		{
			this.SetReferenceSize(width, height, false);
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x0001D1A4 File Offset: 0x0001B3A4
		public void SetReferenceSize(int width, int height, bool reset)
		{
			this.m_RTHandleProperties.previousViewportSize = this.m_RTHandleProperties.currentViewportSize;
			this.m_RTHandleProperties.previousRenderTargetSize = this.m_RTHandleProperties.currentRenderTargetSize;
			Vector2 vector = new Vector2((float)this.GetMaxWidth(), (float)this.GetMaxHeight());
			width = Mathf.Max(width, 1);
			height = Mathf.Max(height, 1);
			bool flag = width > this.GetMaxWidth() || height > this.GetMaxHeight() || reset;
			if (flag)
			{
				this.Resize(width, height, flag);
			}
			this.m_RTHandleProperties.currentViewportSize = new Vector2Int(width, height);
			this.m_RTHandleProperties.currentRenderTargetSize = new Vector2Int(this.GetMaxWidth(), this.GetMaxHeight());
			if (this.m_RTHandleProperties.previousViewportSize.x == 0)
			{
				this.m_RTHandleProperties.previousViewportSize = this.m_RTHandleProperties.currentViewportSize;
				this.m_RTHandleProperties.previousRenderTargetSize = this.m_RTHandleProperties.currentRenderTargetSize;
				vector = new Vector2((float)this.GetMaxWidth(), (float)this.GetMaxHeight());
			}
			Vector2 vector2 = this.CalculateRatioAgainstMaxSize(in this.m_RTHandleProperties.currentViewportSize);
			if (DynamicResolutionHandler.instance.HardwareDynamicResIsEnabled() && this.m_HardwareDynamicResRequested)
			{
				this.m_RTHandleProperties.rtHandleScale = new Vector4(vector2.x, vector2.y, this.m_RTHandleProperties.rtHandleScale.x, this.m_RTHandleProperties.rtHandleScale.y);
				return;
			}
			Vector2 vector3 = this.m_RTHandleProperties.previousViewportSize / vector;
			this.m_RTHandleProperties.rtHandleScale = new Vector4(vector2.x, vector2.y, vector3.x, vector3.y);
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x0001D34C File Offset: 0x0001B54C
		internal Vector2 CalculateRatioAgainstMaxSize(in Vector2Int viewportSize)
		{
			Vector2 vector = new Vector2((float)this.GetMaxWidth(), (float)this.GetMaxHeight());
			if (DynamicResolutionHandler.instance.HardwareDynamicResIsEnabled() && this.m_HardwareDynamicResRequested && viewportSize != DynamicResolutionHandler.instance.finalViewport)
			{
				Vector2 vector2 = viewportSize / DynamicResolutionHandler.instance.finalViewport;
				vector = DynamicResolutionHandler.instance.ApplyScalesOnSize(new Vector2Int(this.GetMaxWidth(), this.GetMaxHeight()), vector2);
			}
			Vector2Int vector2Int = viewportSize;
			float num = (float)vector2Int.x / vector.x;
			vector2Int = viewportSize;
			return new Vector2(num, (float)vector2Int.y / vector.y);
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x0001D40C File Offset: 0x0001B60C
		public void SetHardwareDynamicResolutionState(bool enableHWDynamicRes)
		{
			if (enableHWDynamicRes != this.m_HardwareDynamicResRequested)
			{
				this.m_HardwareDynamicResRequested = enableHWDynamicRes;
				Array.Resize<RTHandle>(ref this.m_AutoSizedRTsArray, this.m_AutoSizedRTs.Count);
				this.m_AutoSizedRTs.CopyTo(this.m_AutoSizedRTsArray);
				int i = 0;
				int num = this.m_AutoSizedRTsArray.Length;
				while (i < num)
				{
					RTHandle rthandle = this.m_AutoSizedRTsArray[i];
					RenderTexture rt = rthandle.m_RT;
					if (rt)
					{
						rt.Release();
						rt.useDynamicScale = this.m_HardwareDynamicResRequested && rthandle.m_EnableHWDynamicScale;
						rt.Create();
					}
					i++;
				}
			}
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x0001D4A4 File Offset: 0x0001B6A4
		internal void SwitchResizeMode(RTHandle rth, RTHandleSystem.ResizeMode mode)
		{
			if (!rth.useScaling)
			{
				return;
			}
			if (mode != RTHandleSystem.ResizeMode.Auto)
			{
				if (mode == RTHandleSystem.ResizeMode.OnDemand)
				{
					this.m_AutoSizedRTs.Remove(rth);
					this.m_ResizeOnDemandRTs.Add(rth);
					return;
				}
			}
			else
			{
				if (this.m_ResizeOnDemandRTs.Contains(rth))
				{
					this.DemandResize(rth);
				}
				this.m_ResizeOnDemandRTs.Remove(rth);
				this.m_AutoSizedRTs.Add(rth);
			}
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x0001D50C File Offset: 0x0001B70C
		private void DemandResize(RTHandle rth)
		{
			RenderTexture rt = rth.m_RT;
			rth.referenceSize = new Vector2Int(this.m_MaxWidths, this.m_MaxHeights);
			Vector2Int vector2Int = rth.GetScaledSize(rth.referenceSize);
			vector2Int = Vector2Int.Max(Vector2Int.one, vector2Int);
			if (rt.width != vector2Int.x || rt.height != vector2Int.y)
			{
				rt.Release();
				rt.width = vector2Int.x;
				rt.height = vector2Int.y;
				rt.name = CoreUtils.GetRenderTargetAutoName(rt.width, rt.height, rt.volumeDepth, rt.graphicsFormat, rt.dimension, rth.m_Name, rt.useMipMap, rth.m_EnableMSAA, (MSAASamples)rt.antiAliasing, rt.useDynamicScale);
				rt.Create();
			}
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x0001D5E4 File Offset: 0x0001B7E4
		public int GetMaxWidth()
		{
			return this.m_MaxWidths;
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x0001D5EC File Offset: 0x0001B7EC
		public int GetMaxHeight()
		{
			return this.m_MaxHeights;
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x0001D5F4 File Offset: 0x0001B7F4
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				Array.Resize<RTHandle>(ref this.m_AutoSizedRTsArray, this.m_AutoSizedRTs.Count);
				this.m_AutoSizedRTs.CopyTo(this.m_AutoSizedRTsArray);
				int i = 0;
				int num = this.m_AutoSizedRTsArray.Length;
				while (i < num)
				{
					RTHandle rthandle = this.m_AutoSizedRTsArray[i];
					this.Release(rthandle);
					i++;
				}
				this.m_AutoSizedRTs.Clear();
				Array.Resize<RTHandle>(ref this.m_AutoSizedRTsArray, this.m_ResizeOnDemandRTs.Count);
				this.m_ResizeOnDemandRTs.CopyTo(this.m_AutoSizedRTsArray);
				int j = 0;
				int num2 = this.m_AutoSizedRTsArray.Length;
				while (j < num2)
				{
					RTHandle rthandle2 = this.m_AutoSizedRTsArray[j];
					this.Release(rthandle2);
					j++;
				}
				this.m_ResizeOnDemandRTs.Clear();
				this.m_AutoSizedRTsArray = null;
			}
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x0001D6C0 File Offset: 0x0001B8C0
		private void Resize(int width, int height, bool sizeChanged)
		{
			this.m_MaxWidths = Math.Max(width, this.m_MaxWidths);
			this.m_MaxHeights = Math.Max(height, this.m_MaxHeights);
			Vector2Int vector2Int = new Vector2Int(this.m_MaxWidths, this.m_MaxHeights);
			Array.Resize<RTHandle>(ref this.m_AutoSizedRTsArray, this.m_AutoSizedRTs.Count);
			this.m_AutoSizedRTs.CopyTo(this.m_AutoSizedRTsArray);
			int i = 0;
			int num = this.m_AutoSizedRTsArray.Length;
			while (i < num)
			{
				RTHandle rthandle = this.m_AutoSizedRTsArray[i];
				rthandle.referenceSize = vector2Int;
				RenderTexture rt = rthandle.m_RT;
				rt.Release();
				Vector2Int scaledSize = rthandle.GetScaledSize(vector2Int);
				rt.width = Mathf.Max(scaledSize.x, 1);
				rt.height = Mathf.Max(scaledSize.y, 1);
				rt.name = CoreUtils.GetRenderTargetAutoName(rt.width, rt.height, rt.volumeDepth, rt.graphicsFormat, rt.dimension, rthandle.m_Name, rt.useMipMap, rthandle.m_EnableMSAA, (MSAASamples)rt.antiAliasing, rt.useDynamicScale);
				rt.Create();
				i++;
			}
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x0001D7F0 File Offset: 0x0001B9F0
		public RTHandle Alloc(int width, int height, int slices = 1, DepthBits depthBufferBits = DepthBits.None, GraphicsFormat colorFormat = GraphicsFormat.R8G8B8A8_SRGB, FilterMode filterMode = FilterMode.Point, TextureWrapMode wrapMode = TextureWrapMode.Repeat, TextureDimension dimension = TextureDimension.Tex2D, bool enableRandomWrite = false, bool useMipMap = false, bool autoGenerateMips = true, bool isShadowMap = false, int anisoLevel = 1, float mipMapBias = 0f, MSAASamples msaaSamples = MSAASamples.None, bool bindTextureMS = false, bool useDynamicScale = false, RenderTextureMemoryless memoryless = RenderTextureMemoryless.None, VRTextureUsage vrUsage = VRTextureUsage.None, string name = "")
		{
			return this.Alloc(width, height, wrapMode, wrapMode, wrapMode, slices, depthBufferBits, colorFormat, filterMode, dimension, enableRandomWrite, useMipMap, autoGenerateMips, isShadowMap, anisoLevel, mipMapBias, msaaSamples, bindTextureMS, useDynamicScale, memoryless, vrUsage, name);
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x0001D82C File Offset: 0x0001BA2C
		public RTHandle Alloc(int width, int height, TextureWrapMode wrapModeU, TextureWrapMode wrapModeV, TextureWrapMode wrapModeW = TextureWrapMode.Repeat, int slices = 1, DepthBits depthBufferBits = DepthBits.None, GraphicsFormat colorFormat = GraphicsFormat.R8G8B8A8_SRGB, FilterMode filterMode = FilterMode.Point, TextureDimension dimension = TextureDimension.Tex2D, bool enableRandomWrite = false, bool useMipMap = false, bool autoGenerateMips = true, bool isShadowMap = false, int anisoLevel = 1, float mipMapBias = 0f, MSAASamples msaaSamples = MSAASamples.None, bool bindTextureMS = false, bool useDynamicScale = false, RenderTextureMemoryless memoryless = RenderTextureMemoryless.None, VRTextureUsage vrUsage = VRTextureUsage.None, string name = "")
		{
			bool flag = msaaSamples != MSAASamples.None;
			if (!flag && bindTextureMS)
			{
				Debug.LogWarning("RTHandle allocated without MSAA but with bindMS set to true, forcing bindMS to false.");
				bindTextureMS = false;
			}
			RenderTexture renderTexture;
			if (isShadowMap || depthBufferBits != DepthBits.None)
			{
				RenderTextureFormat renderTextureFormat = (isShadowMap ? RenderTextureFormat.Shadowmap : RenderTextureFormat.Depth);
				GraphicsFormat graphicsFormat = ((!isShadowMap && SystemInfo.IsFormatSupported(GraphicsFormat.R8_UInt, FormatUsage.StencilSampling)) ? GraphicsFormat.R8_UInt : GraphicsFormat.None);
				renderTexture = new RenderTexture(width, height, (int)depthBufferBits, renderTextureFormat, RenderTextureReadWrite.Linear)
				{
					hideFlags = HideFlags.HideAndDontSave,
					volumeDepth = slices,
					filterMode = filterMode,
					wrapModeU = wrapModeU,
					wrapModeV = wrapModeV,
					wrapModeW = wrapModeW,
					dimension = dimension,
					enableRandomWrite = enableRandomWrite,
					useMipMap = useMipMap,
					autoGenerateMips = autoGenerateMips,
					anisoLevel = anisoLevel,
					mipMapBias = mipMapBias,
					stencilFormat = graphicsFormat,
					antiAliasing = (int)msaaSamples,
					bindTextureMS = bindTextureMS,
					useDynamicScale = (this.m_HardwareDynamicResRequested && useDynamicScale),
					memorylessMode = memoryless,
					vrUsage = vrUsage,
					name = CoreUtils.GetRenderTargetAutoName(width, height, slices, renderTextureFormat, name, useMipMap, flag, msaaSamples)
				};
			}
			else
			{
				renderTexture = new RenderTexture(width, height, (int)depthBufferBits, colorFormat)
				{
					hideFlags = HideFlags.HideAndDontSave,
					volumeDepth = slices,
					filterMode = filterMode,
					wrapModeU = wrapModeU,
					wrapModeV = wrapModeV,
					wrapModeW = wrapModeW,
					dimension = dimension,
					enableRandomWrite = enableRandomWrite,
					useMipMap = useMipMap,
					autoGenerateMips = autoGenerateMips,
					anisoLevel = anisoLevel,
					mipMapBias = mipMapBias,
					antiAliasing = (int)msaaSamples,
					bindTextureMS = bindTextureMS,
					useDynamicScale = (this.m_HardwareDynamicResRequested && useDynamicScale),
					memorylessMode = memoryless,
					vrUsage = vrUsage,
					name = CoreUtils.GetRenderTargetAutoName(width, height, slices, colorFormat, dimension, name, useMipMap, flag, msaaSamples, useDynamicScale)
				};
			}
			renderTexture.Create();
			RTHandle rthandle = new RTHandle(this);
			rthandle.SetRenderTexture(renderTexture);
			rthandle.useScaling = false;
			rthandle.m_EnableRandomWrite = enableRandomWrite;
			rthandle.m_EnableMSAA = flag;
			rthandle.m_EnableHWDynamicScale = useDynamicScale;
			rthandle.m_Name = name;
			rthandle.referenceSize = new Vector2Int(width, height);
			return rthandle;
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x0001DA3C File Offset: 0x0001BC3C
		public RTHandle Alloc(Vector2 scaleFactor, int slices = 1, DepthBits depthBufferBits = DepthBits.None, GraphicsFormat colorFormat = GraphicsFormat.R8G8B8A8_SRGB, FilterMode filterMode = FilterMode.Point, TextureWrapMode wrapMode = TextureWrapMode.Repeat, TextureDimension dimension = TextureDimension.Tex2D, bool enableRandomWrite = false, bool useMipMap = false, bool autoGenerateMips = true, bool isShadowMap = false, int anisoLevel = 1, float mipMapBias = 0f, MSAASamples msaaSamples = MSAASamples.None, bool bindTextureMS = false, bool useDynamicScale = false, RenderTextureMemoryless memoryless = RenderTextureMemoryless.None, VRTextureUsage vrUsage = VRTextureUsage.None, string name = "")
		{
			int num = Mathf.Max(Mathf.RoundToInt(scaleFactor.x * (float)this.GetMaxWidth()), 1);
			int num2 = Mathf.Max(Mathf.RoundToInt(scaleFactor.y * (float)this.GetMaxHeight()), 1);
			RTHandle rthandle = this.AllocAutoSizedRenderTexture(num, num2, slices, depthBufferBits, colorFormat, filterMode, wrapMode, dimension, enableRandomWrite, useMipMap, autoGenerateMips, isShadowMap, anisoLevel, mipMapBias, msaaSamples, bindTextureMS, useDynamicScale, memoryless, vrUsage, name);
			rthandle.referenceSize = new Vector2Int(num, num2);
			rthandle.scaleFactor = scaleFactor;
			return rthandle;
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x0001DABC File Offset: 0x0001BCBC
		public RTHandle Alloc(ScaleFunc scaleFunc, int slices = 1, DepthBits depthBufferBits = DepthBits.None, GraphicsFormat colorFormat = GraphicsFormat.R8G8B8A8_SRGB, FilterMode filterMode = FilterMode.Point, TextureWrapMode wrapMode = TextureWrapMode.Repeat, TextureDimension dimension = TextureDimension.Tex2D, bool enableRandomWrite = false, bool useMipMap = false, bool autoGenerateMips = true, bool isShadowMap = false, int anisoLevel = 1, float mipMapBias = 0f, MSAASamples msaaSamples = MSAASamples.None, bool bindTextureMS = false, bool useDynamicScale = false, RenderTextureMemoryless memoryless = RenderTextureMemoryless.None, VRTextureUsage vrUsage = VRTextureUsage.None, string name = "")
		{
			Vector2Int vector2Int = scaleFunc(new Vector2Int(this.GetMaxWidth(), this.GetMaxHeight()));
			int num = Mathf.Max(vector2Int.x, 1);
			int num2 = Mathf.Max(vector2Int.y, 1);
			RTHandle rthandle = this.AllocAutoSizedRenderTexture(num, num2, slices, depthBufferBits, colorFormat, filterMode, wrapMode, dimension, enableRandomWrite, useMipMap, autoGenerateMips, isShadowMap, anisoLevel, mipMapBias, msaaSamples, bindTextureMS, useDynamicScale, memoryless, vrUsage, name);
			rthandle.referenceSize = new Vector2Int(num, num2);
			rthandle.scaleFunc = scaleFunc;
			return rthandle;
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x0001DB3C File Offset: 0x0001BD3C
		private RTHandle AllocAutoSizedRenderTexture(int width, int height, int slices, DepthBits depthBufferBits, GraphicsFormat colorFormat, FilterMode filterMode, TextureWrapMode wrapMode, TextureDimension dimension, bool enableRandomWrite, bool useMipMap, bool autoGenerateMips, bool isShadowMap, int anisoLevel, float mipMapBias, MSAASamples msaaSamples, bool bindTextureMS, bool useDynamicScale, RenderTextureMemoryless memoryless, VRTextureUsage vrUsage, string name)
		{
			bool flag = msaaSamples != MSAASamples.None;
			if (!flag && bindTextureMS)
			{
				Debug.LogWarning("RTHandle allocated without MSAA but with bindMS set to true, forcing bindMS to false.");
				bindTextureMS = false;
			}
			if (flag && enableRandomWrite)
			{
				Debug.LogWarning("RTHandle that is MSAA-enabled cannot allocate MSAA RT with 'enableRandomWrite = true'.");
				enableRandomWrite = false;
			}
			RenderTexture renderTexture;
			if (isShadowMap || depthBufferBits != DepthBits.None)
			{
				RenderTextureFormat renderTextureFormat = (isShadowMap ? RenderTextureFormat.Shadowmap : RenderTextureFormat.Depth);
				GraphicsFormat graphicsFormat = ((!isShadowMap && SystemInfo.IsFormatSupported(GraphicsFormat.R8_UInt, FormatUsage.StencilSampling)) ? GraphicsFormat.R8_UInt : GraphicsFormat.None);
				renderTexture = new RenderTexture(width, height, (int)depthBufferBits, renderTextureFormat, RenderTextureReadWrite.Linear)
				{
					hideFlags = HideFlags.HideAndDontSave,
					volumeDepth = slices,
					filterMode = filterMode,
					wrapMode = wrapMode,
					dimension = dimension,
					enableRandomWrite = enableRandomWrite,
					useMipMap = useMipMap,
					autoGenerateMips = autoGenerateMips,
					anisoLevel = anisoLevel,
					mipMapBias = mipMapBias,
					antiAliasing = (int)msaaSamples,
					bindTextureMS = bindTextureMS,
					useDynamicScale = (this.m_HardwareDynamicResRequested && useDynamicScale),
					memorylessMode = memoryless,
					stencilFormat = graphicsFormat,
					vrUsage = vrUsage,
					name = CoreUtils.GetRenderTargetAutoName(width, height, slices, colorFormat, dimension, name, useMipMap, flag, msaaSamples, useDynamicScale)
				};
			}
			else
			{
				renderTexture = new RenderTexture(width, height, (int)depthBufferBits, colorFormat)
				{
					hideFlags = HideFlags.HideAndDontSave,
					volumeDepth = slices,
					filterMode = filterMode,
					wrapMode = wrapMode,
					dimension = dimension,
					enableRandomWrite = enableRandomWrite,
					useMipMap = useMipMap,
					autoGenerateMips = autoGenerateMips,
					anisoLevel = anisoLevel,
					mipMapBias = mipMapBias,
					antiAliasing = (int)msaaSamples,
					bindTextureMS = bindTextureMS,
					useDynamicScale = (this.m_HardwareDynamicResRequested && useDynamicScale),
					memorylessMode = memoryless,
					vrUsage = vrUsage,
					name = CoreUtils.GetRenderTargetAutoName(width, height, slices, colorFormat, dimension, name, useMipMap, flag, msaaSamples, useDynamicScale)
				};
			}
			renderTexture.Create();
			RTHandle rthandle = new RTHandle(this);
			rthandle.SetRenderTexture(renderTexture);
			rthandle.m_EnableMSAA = flag;
			rthandle.m_EnableRandomWrite = enableRandomWrite;
			rthandle.useScaling = true;
			rthandle.m_EnableHWDynamicScale = useDynamicScale;
			rthandle.m_Name = name;
			this.m_AutoSizedRTs.Add(rthandle);
			return rthandle;
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x0001DD44 File Offset: 0x0001BF44
		public RTHandle Alloc(RenderTexture texture)
		{
			RTHandle rthandle = new RTHandle(this);
			rthandle.SetRenderTexture(texture);
			rthandle.m_EnableMSAA = false;
			rthandle.m_EnableRandomWrite = false;
			rthandle.useScaling = false;
			rthandle.m_EnableHWDynamicScale = false;
			rthandle.m_Name = texture.name;
			return rthandle;
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x0001DD7B File Offset: 0x0001BF7B
		public RTHandle Alloc(Texture texture)
		{
			RTHandle rthandle = new RTHandle(this);
			rthandle.SetTexture(texture);
			rthandle.m_EnableMSAA = false;
			rthandle.m_EnableRandomWrite = false;
			rthandle.useScaling = false;
			rthandle.m_EnableHWDynamicScale = false;
			rthandle.m_Name = texture.name;
			return rthandle;
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x0001DDB2 File Offset: 0x0001BFB2
		public RTHandle Alloc(RenderTargetIdentifier texture)
		{
			return this.Alloc(texture, "");
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x0001DDC0 File Offset: 0x0001BFC0
		public RTHandle Alloc(RenderTargetIdentifier texture, string name)
		{
			RTHandle rthandle = new RTHandle(this);
			rthandle.SetTexture(texture);
			rthandle.m_EnableMSAA = false;
			rthandle.m_EnableRandomWrite = false;
			rthandle.useScaling = false;
			rthandle.m_EnableHWDynamicScale = false;
			rthandle.m_Name = name;
			return rthandle;
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x0001DDF2 File Offset: 0x0001BFF2
		private static RTHandle Alloc(RTHandle tex)
		{
			Debug.LogError("Allocation a RTHandle from another one is forbidden.");
			return null;
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x0001DE00 File Offset: 0x0001C000
		internal string DumpRTInfo()
		{
			string text = "";
			Array.Resize<RTHandle>(ref this.m_AutoSizedRTsArray, this.m_AutoSizedRTs.Count);
			this.m_AutoSizedRTs.CopyTo(this.m_AutoSizedRTsArray);
			int i = 0;
			int num = this.m_AutoSizedRTsArray.Length;
			while (i < num)
			{
				RenderTexture rt = this.m_AutoSizedRTsArray[i].rt;
				text = string.Format("{0}\nRT ({1})\t Format: {2} W: {3} H {4}\n", new object[] { text, i, rt.format, rt.width, rt.height });
				i++;
			}
			return text;
		}

		// Token: 0x0400041A RID: 1050
		private bool m_HardwareDynamicResRequested;

		// Token: 0x0400041B RID: 1051
		private HashSet<RTHandle> m_AutoSizedRTs;

		// Token: 0x0400041C RID: 1052
		private RTHandle[] m_AutoSizedRTsArray;

		// Token: 0x0400041D RID: 1053
		private HashSet<RTHandle> m_ResizeOnDemandRTs;

		// Token: 0x0400041E RID: 1054
		private RTHandleProperties m_RTHandleProperties;

		// Token: 0x0400041F RID: 1055
		private int m_MaxWidths;

		// Token: 0x04000420 RID: 1056
		private int m_MaxHeights;

		// Token: 0x020001C1 RID: 449
		internal enum ResizeMode
		{
			// Token: 0x04000747 RID: 1863
			Auto,
			// Token: 0x04000748 RID: 1864
			OnDemand
		}
	}
}
