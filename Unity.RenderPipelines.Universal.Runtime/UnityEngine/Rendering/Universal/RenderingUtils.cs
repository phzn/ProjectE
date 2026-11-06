using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Experimental.Rendering.RenderGraphModule;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000CC RID: 204
	public static class RenderingUtils
	{
		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000646 RID: 1606 RVA: 0x00026466 File Offset: 0x00024666
		internal static AttachmentDescriptor emptyAttachment
		{
			get
			{
				return RenderingUtils.s_EmptyAttachment;
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000647 RID: 1607 RVA: 0x00026470 File Offset: 0x00024670
		[Obsolete("Use Blitter.BlitCameraTexture instead of CommandBuffer.DrawMesh(fullscreenMesh, ...)")]
		public static Mesh fullscreenMesh
		{
			get
			{
				if (RenderingUtils.s_FullscreenMesh != null)
				{
					return RenderingUtils.s_FullscreenMesh;
				}
				float num = 1f;
				float num2 = 0f;
				RenderingUtils.s_FullscreenMesh = new Mesh
				{
					name = "Fullscreen Quad"
				};
				RenderingUtils.s_FullscreenMesh.SetVertices(new List<Vector3>
				{
					new Vector3(-1f, -1f, 0f),
					new Vector3(-1f, 1f, 0f),
					new Vector3(1f, -1f, 0f),
					new Vector3(1f, 1f, 0f)
				});
				RenderingUtils.s_FullscreenMesh.SetUVs(0, new List<Vector2>
				{
					new Vector2(0f, num2),
					new Vector2(0f, num),
					new Vector2(1f, num2),
					new Vector2(1f, num)
				});
				RenderingUtils.s_FullscreenMesh.SetIndices(new int[] { 0, 1, 2, 2, 1, 3 }, MeshTopology.Triangles, 0, false);
				RenderingUtils.s_FullscreenMesh.UploadMeshData(true);
				return RenderingUtils.s_FullscreenMesh;
			}
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x06000648 RID: 1608 RVA: 0x000265AA File Offset: 0x000247AA
		internal static bool useStructuredBuffer
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x000265AD File Offset: 0x000247AD
		internal static bool SupportsLightLayers(GraphicsDeviceType type)
		{
			return type != GraphicsDeviceType.OpenGLES2;
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x0600064A RID: 1610 RVA: 0x000265B8 File Offset: 0x000247B8
		private static Material errorMaterial
		{
			get
			{
				if (RenderingUtils.s_ErrorMaterial == null)
				{
					try
					{
						RenderingUtils.s_ErrorMaterial = new Material(Shader.Find("Hidden/Universal Render Pipeline/FallbackError"));
					}
					catch
					{
					}
				}
				return RenderingUtils.s_ErrorMaterial;
			}
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x00026600 File Offset: 0x00024800
		public static void SetViewAndProjectionMatrices(CommandBuffer cmd, Matrix4x4 viewMatrix, Matrix4x4 projectionMatrix, bool setInverseMatrices)
		{
			Matrix4x4 matrix4x = projectionMatrix * viewMatrix;
			cmd.SetGlobalMatrix(ShaderPropertyId.viewMatrix, viewMatrix);
			cmd.SetGlobalMatrix(ShaderPropertyId.projectionMatrix, projectionMatrix);
			cmd.SetGlobalMatrix(ShaderPropertyId.viewAndProjectionMatrix, matrix4x);
			if (setInverseMatrices)
			{
				Matrix4x4 matrix4x2 = Matrix4x4.Inverse(viewMatrix);
				Matrix4x4 matrix4x3 = Matrix4x4.Inverse(projectionMatrix);
				Matrix4x4 matrix4x4 = matrix4x2 * matrix4x3;
				cmd.SetGlobalMatrix(ShaderPropertyId.inverseViewMatrix, matrix4x2);
				cmd.SetGlobalMatrix(ShaderPropertyId.inverseProjectionMatrix, matrix4x3);
				cmd.SetGlobalMatrix(ShaderPropertyId.inverseViewAndProjectionMatrix, matrix4x4);
			}
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x00026678 File Offset: 0x00024878
		internal static void SetScaleBiasRt(CommandBuffer cmd, in RenderingData renderingData)
		{
			ScriptableRenderer renderer = renderingData.cameraData.renderer;
			CameraData cameraData = renderingData.cameraData;
			float num = ((cameraData.cameraType != CameraType.Game || !(renderer.cameraColorTargetHandle.nameID == BuiltinRenderTextureType.CameraTarget) || !(cameraData.camera.targetTexture == null)) ? (-1f) : 1f);
			Vector4 vector = ((num < 0f) ? new Vector4(num, 1f, -1f, 1f) : new Vector4(num, 0f, 1f, 1f));
			cmd.SetGlobalVector(Shader.PropertyToID("_ScaleBiasRt"), vector);
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x00026728 File Offset: 0x00024928
		internal static void Blit(CommandBuffer cmd, RTHandle source, Rect viewport, RTHandle destination, RenderBufferLoadAction loadAction, RenderBufferStoreAction storeAction, ClearFlag clearFlag, Color clearColor, Material material, int passIndex = 0)
		{
			Vector2 vector = (source.useScaling ? new Vector2(source.rtHandleProperties.rtHandleScale.x, source.rtHandleProperties.rtHandleScale.y) : Vector2.one);
			CoreUtils.SetRenderTarget(cmd, destination, loadAction, storeAction, ClearFlag.None, Color.clear, 0, CubemapFace.Unknown, -1);
			cmd.SetViewport(viewport);
			Blitter.BlitTexture(cmd, source, vector, material, passIndex);
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x00026798 File Offset: 0x00024998
		internal static void Blit(CommandBuffer cmd, RTHandle source, Rect viewport, RTHandle destinationColor, RenderBufferLoadAction colorLoadAction, RenderBufferStoreAction colorStoreAction, RTHandle destinationDepthStencil, RenderBufferLoadAction depthStencilLoadAction, RenderBufferStoreAction depthStencilStoreAction, ClearFlag clearFlag, Color clearColor, Material material, int passIndex = 0)
		{
			Vector2 vector = (source.useScaling ? new Vector2(source.rtHandleProperties.rtHandleScale.x, source.rtHandleProperties.rtHandleScale.y) : Vector2.one);
			CoreUtils.SetRenderTarget(cmd, destinationColor, colorLoadAction, colorStoreAction, destinationDepthStencil, depthStencilLoadAction, depthStencilStoreAction, clearFlag, clearColor, 0, CubemapFace.Unknown, -1);
			cmd.SetViewport(viewport);
			Blitter.BlitTexture(cmd, source, vector, material, passIndex);
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x0002680C File Offset: 0x00024A0C
		internal static void FinalBlit(CommandBuffer cmd, ref CameraData cameraData, RTHandle source, RTHandle destination, RenderBufferLoadAction loadAction, RenderBufferStoreAction storeAction, Material material, int passIndex)
		{
			bool flag = !cameraData.isSceneViewCamera;
			if (cameraData.xr.enabled)
			{
				flag = new RenderTargetIdentifier(destination.nameID, 0, CubemapFace.Unknown, -1) == new RenderTargetIdentifier(cameraData.xr.renderTarget, 0, CubemapFace.Unknown, -1);
			}
			Vector2 vector = (source.useScaling ? new Vector2(source.rtHandleProperties.rtHandleScale.x, source.rtHandleProperties.rtHandleScale.y) : Vector2.one);
			Vector4 vector2 = ((flag && cameraData.targetTexture == null && SystemInfo.graphicsUVStartsAtTop) ? new Vector4(vector.x, -vector.y, 0f, vector.y) : new Vector4(vector.x, vector.y, 0f, 0f));
			CoreUtils.SetRenderTarget(cmd, destination, loadAction, storeAction, ClearFlag.None, Color.clear, 0, CubemapFace.Unknown, -1);
			if (flag)
			{
				cmd.SetViewport(cameraData.pixelRect);
			}
			if (GL.wireframe && cameraData.isSceneViewCamera)
			{
				cmd.SetRenderTarget(BuiltinRenderTextureType.CameraTarget, loadAction, storeAction, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.DontCare);
				if (SystemInfo.graphicsDeviceType == GraphicsDeviceType.Vulkan)
				{
					cmd.SetWireframe(false);
					cmd.Blit(source, destination);
					cmd.SetWireframe(true);
					return;
				}
				cmd.Blit(source, destination);
				return;
			}
			else
			{
				if (source.rt == null)
				{
					Blitter.BlitTexture(cmd, source.nameID, vector2, material, passIndex);
					return;
				}
				Blitter.BlitTexture(cmd, source, vector2, material, passIndex);
				return;
			}
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x00026990 File Offset: 0x00024B90
		[Conditional("DEVELOPMENT_BUILD")]
		[Conditional("UNITY_EDITOR")]
		internal static void RenderObjectsWithError(ScriptableRenderContext context, ref CullingResults cullResults, Camera camera, FilteringSettings filterSettings, SortingCriteria sortFlags)
		{
			if (RenderingUtils.errorMaterial == null)
			{
				return;
			}
			SortingSettings sortingSettings = new SortingSettings(camera)
			{
				criteria = sortFlags
			};
			DrawingSettings drawingSettings = new DrawingSettings(RenderingUtils.m_LegacyShaderPassNames[0], sortingSettings)
			{
				perObjectData = PerObjectData.None,
				overrideMaterial = RenderingUtils.errorMaterial,
				overrideMaterialPassIndex = 0
			};
			for (int i = 1; i < RenderingUtils.m_LegacyShaderPassNames.Count; i++)
			{
				drawingSettings.SetShaderPassName(i, RenderingUtils.m_LegacyShaderPassNames[i]);
			}
			context.DrawRenderers(cullResults, ref drawingSettings, ref filterSettings);
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x00026A2E File Offset: 0x00024C2E
		internal static void ClearSystemInfoCache()
		{
			RenderingUtils.m_RenderTextureFormatSupport.Clear();
			RenderingUtils.m_GraphicsFormatSupport.Clear();
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x00026A44 File Offset: 0x00024C44
		public static bool SupportsRenderTextureFormat(RenderTextureFormat format)
		{
			bool flag;
			if (!RenderingUtils.m_RenderTextureFormatSupport.TryGetValue(format, out flag))
			{
				flag = SystemInfo.SupportsRenderTextureFormat(format);
				RenderingUtils.m_RenderTextureFormatSupport.Add(format, flag);
			}
			return flag;
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x00026A74 File Offset: 0x00024C74
		public static bool SupportsGraphicsFormat(GraphicsFormat format, FormatUsage usage)
		{
			bool flag = false;
			Dictionary<FormatUsage, bool> dictionary;
			if (!RenderingUtils.m_GraphicsFormatSupport.TryGetValue(format, out dictionary))
			{
				dictionary = new Dictionary<FormatUsage, bool>();
				flag = SystemInfo.IsFormatSupported(format, usage);
				dictionary.Add(usage, flag);
				RenderingUtils.m_GraphicsFormatSupport.Add(format, dictionary);
			}
			else if (!dictionary.TryGetValue(usage, out flag))
			{
				flag = SystemInfo.IsFormatSupported(format, usage);
				dictionary.Add(usage, flag);
			}
			return flag;
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x00026AD4 File Offset: 0x00024CD4
		internal static int GetLastValidColorBufferIndex(RenderTargetIdentifier[] colorBuffers)
		{
			int num = colorBuffers.Length - 1;
			while (num >= 0 && !(colorBuffers[num] != 0))
			{
				num--;
			}
			return num;
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x00026B08 File Offset: 0x00024D08
		[Obsolete("Use RTHandles for colorBuffers")]
		internal static uint GetValidColorBufferCount(RenderTargetIdentifier[] colorBuffers)
		{
			uint num = 0U;
			if (colorBuffers != null)
			{
				for (int i = 0; i < colorBuffers.Length; i++)
				{
					if (colorBuffers[i] != 0)
					{
						num += 1U;
					}
				}
			}
			return num;
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x00026B44 File Offset: 0x00024D44
		internal static uint GetValidColorBufferCount(RTHandle[] colorBuffers)
		{
			uint num = 0U;
			if (colorBuffers != null)
			{
				foreach (RTHandle rthandle in colorBuffers)
				{
					if (rthandle != null && rthandle.nameID != 0)
					{
						num += 1U;
					}
				}
			}
			return num;
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x00026B85 File Offset: 0x00024D85
		[Obsolete("Use RTHandles for colorBuffers")]
		internal static bool IsMRT(RenderTargetIdentifier[] colorBuffers)
		{
			return RenderingUtils.GetValidColorBufferCount(colorBuffers) > 1U;
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x00026B90 File Offset: 0x00024D90
		internal static bool IsMRT(RTHandle[] colorBuffers)
		{
			return RenderingUtils.GetValidColorBufferCount(colorBuffers) > 1U;
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x00026B9C File Offset: 0x00024D9C
		internal static bool Contains(RenderTargetIdentifier[] source, RenderTargetIdentifier value)
		{
			for (int i = 0; i < source.Length; i++)
			{
				if (source[i] == value)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x00026BCC File Offset: 0x00024DCC
		internal static int IndexOf(RenderTargetIdentifier[] source, RenderTargetIdentifier value)
		{
			for (int i = 0; i < source.Length; i++)
			{
				if (source[i] == value)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x00026BFC File Offset: 0x00024DFC
		internal static uint CountDistinct(RenderTargetIdentifier[] source, RenderTargetIdentifier value)
		{
			uint num = 0U;
			for (int i = 0; i < source.Length; i++)
			{
				if (source[i] != value && source[i] != 0)
				{
					num += 1U;
				}
			}
			return num;
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x00026C44 File Offset: 0x00024E44
		internal static int LastValid(RenderTargetIdentifier[] source)
		{
			for (int i = source.Length - 1; i >= 0; i--)
			{
				if (source[i] != 0)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x00026C78 File Offset: 0x00024E78
		internal static bool Contains(ClearFlag a, ClearFlag b)
		{
			return (a & b) == b;
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x00026C80 File Offset: 0x00024E80
		internal static bool SequenceEqual(RenderTargetIdentifier[] left, RenderTargetIdentifier[] right)
		{
			if (left.Length != right.Length)
			{
				return false;
			}
			for (int i = 0; i < left.Length; i++)
			{
				if (left[i] != right[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x00026CBD File Offset: 0x00024EBD
		internal static bool MultisampleDepthResolveSupported()
		{
			return Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.OSXPlayer && SystemInfo.supportsMultisampleResolveDepth && SystemInfo.supportsMultisampleResolveStencil;
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x00026CE0 File Offset: 0x00024EE0
		internal static bool RTHandleNeedsReAlloc(RTHandle handle, in TextureDesc descriptor, bool scaled)
		{
			return handle == null || handle.rt == null || handle.useScaling != scaled || (!scaled && (handle.rt.width != descriptor.width || handle.rt.height != descriptor.height)) || handle.rt.descriptor.depthBufferBits != (int)descriptor.depthBufferBits || (handle.rt.descriptor.depthBufferBits == 0 && !descriptor.isShadowMap && handle.rt.descriptor.graphicsFormat != descriptor.colorFormat) || handle.rt.descriptor.dimension != descriptor.dimension || handle.rt.descriptor.enableRandomWrite != descriptor.enableRandomWrite || handle.rt.descriptor.useMipMap != descriptor.useMipMap || handle.rt.descriptor.autoGenerateMips != descriptor.autoGenerateMips || handle.rt.descriptor.msaaSamples != (int)descriptor.msaaSamples || handle.rt.descriptor.bindMS != descriptor.bindTextureMS || handle.rt.descriptor.useDynamicScale != descriptor.useDynamicScale || handle.rt.descriptor.memoryless != descriptor.memoryless || handle.rt.filterMode != descriptor.filterMode || handle.rt.wrapMode != descriptor.wrapMode || handle.rt.anisoLevel != descriptor.anisoLevel || handle.rt.mipMapBias != descriptor.mipMapBias || handle.name != descriptor.name;
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x00026ED8 File Offset: 0x000250D8
		internal static RenderTargetIdentifier GetCameraTargetIdentifier(ref RenderingData renderingData)
		{
			ref CameraData ptr = ref renderingData.cameraData;
			RenderTargetIdentifier renderTargetIdentifier = ((ptr.targetTexture != null) ? new RenderTargetIdentifier(ptr.targetTexture) : BuiltinRenderTextureType.CameraTarget);
			if (ptr.xr.enabled)
			{
				if (ptr.xr.singlePassEnabled)
				{
					renderTargetIdentifier = ptr.xr.renderTarget;
				}
				else
				{
					int textureArraySlice = ptr.xr.GetTextureArraySlice(0);
					renderTargetIdentifier = new RenderTargetIdentifier(ptr.xr.renderTarget, 0, CubemapFace.Unknown, textureArraySlice);
				}
			}
			return renderTargetIdentifier;
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x00026F5C File Offset: 0x0002515C
		public static bool ReAllocateIfNeeded(ref RTHandle handle, in RenderTextureDescriptor descriptor, FilterMode filterMode = FilterMode.Point, TextureWrapMode wrapMode = TextureWrapMode.Repeat, bool isShadowMap = false, int anisoLevel = 1, float mipMapBias = 0f, string name = "")
		{
			TextureDesc textureDesc = RTHandleResourcePool.CreateTextureDesc(descriptor, TextureSizeMode.Explicit, anisoLevel, 0f, filterMode, wrapMode, name);
			if (!RenderingUtils.RTHandleNeedsReAlloc(handle, in textureDesc, false))
			{
				return false;
			}
			if (handle != null && handle.rt != null)
			{
				RenderingUtils.AddStaleResourceToPoolOrRelease(RTHandleResourcePool.CreateTextureDesc(handle.rt.descriptor, TextureSizeMode.Explicit, handle.rt.anisoLevel, handle.rt.mipMapBias, handle.rt.filterMode, handle.rt.wrapMode, handle.name), handle);
			}
			if (UniversalRenderPipeline.s_RTHandlePool.TryGetResource(in textureDesc, out handle, true))
			{
				return true;
			}
			handle = RTHandles.Alloc(in descriptor, filterMode, wrapMode, isShadowMap, anisoLevel, mipMapBias, name);
			return true;
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x0002701C File Offset: 0x0002521C
		public static bool ReAllocateIfNeeded(ref RTHandle handle, Vector2 scaleFactor, in RenderTextureDescriptor descriptor, FilterMode filterMode = FilterMode.Point, TextureWrapMode wrapMode = TextureWrapMode.Repeat, bool isShadowMap = false, int anisoLevel = 1, float mipMapBias = 0f, string name = "")
		{
			bool flag = handle != null && handle.useScaling && handle.scaleFactor == scaleFactor;
			TextureDesc textureDesc = RTHandleResourcePool.CreateTextureDesc(descriptor, TextureSizeMode.Scale, anisoLevel, 0f, filterMode, wrapMode, "");
			if (flag && !RenderingUtils.RTHandleNeedsReAlloc(handle, in textureDesc, true))
			{
				return false;
			}
			if (handle != null && handle.rt != null)
			{
				RenderingUtils.AddStaleResourceToPoolOrRelease(RTHandleResourcePool.CreateTextureDesc(handle.rt.descriptor, TextureSizeMode.Scale, handle.rt.anisoLevel, handle.rt.mipMapBias, handle.rt.filterMode, handle.rt.wrapMode, ""), handle);
			}
			if (UniversalRenderPipeline.s_RTHandlePool.TryGetResource(in textureDesc, out handle, true))
			{
				return true;
			}
			handle = RTHandles.Alloc(scaleFactor, in descriptor, filterMode, wrapMode, isShadowMap, anisoLevel, mipMapBias, name);
			return true;
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x00027100 File Offset: 0x00025300
		public static bool ReAllocateIfNeeded(ref RTHandle handle, ScaleFunc scaleFunc, in RenderTextureDescriptor descriptor, FilterMode filterMode = FilterMode.Point, TextureWrapMode wrapMode = TextureWrapMode.Repeat, bool isShadowMap = false, int anisoLevel = 1, float mipMapBias = 0f, string name = "")
		{
			bool flag = handle != null && handle.useScaling && handle.scaleFactor == Vector2.zero;
			TextureDesc textureDesc = RTHandleResourcePool.CreateTextureDesc(descriptor, TextureSizeMode.Functor, anisoLevel, 0f, filterMode, wrapMode, "");
			if (flag && !RenderingUtils.RTHandleNeedsReAlloc(handle, in textureDesc, true))
			{
				return false;
			}
			if (handle != null && handle.rt != null)
			{
				RenderingUtils.AddStaleResourceToPoolOrRelease(RTHandleResourcePool.CreateTextureDesc(handle.rt.descriptor, TextureSizeMode.Functor, handle.rt.anisoLevel, handle.rt.mipMapBias, handle.rt.filterMode, handle.rt.wrapMode, ""), handle);
			}
			if (UniversalRenderPipeline.s_RTHandlePool.TryGetResource(in textureDesc, out handle, true))
			{
				return true;
			}
			handle = RTHandles.Alloc(scaleFunc, in descriptor, filterMode, wrapMode, isShadowMap, anisoLevel, mipMapBias, name);
			return true;
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x000271E5 File Offset: 0x000253E5
		public static bool SetMaxRTHandlePoolCapacity(int capacity)
		{
			if (UniversalRenderPipeline.s_RTHandlePool == null)
			{
				return false;
			}
			UniversalRenderPipeline.s_RTHandlePool.staleResourceCapacity = capacity;
			return true;
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x000271FC File Offset: 0x000253FC
		internal static void AddStaleResourceToPoolOrRelease(TextureDesc desc, RTHandle handle)
		{
			if (!UniversalRenderPipeline.s_RTHandlePool.AddResourceToPool(in desc, handle, Time.frameCount))
			{
				RTHandles.Release(handle);
			}
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x00027218 File Offset: 0x00025418
		public static DrawingSettings CreateDrawingSettings(ShaderTagId shaderTagId, ref RenderingData renderingData, SortingCriteria sortingCriteria)
		{
			Camera camera = renderingData.cameraData.camera;
			SortingSettings sortingSettings = new SortingSettings(camera)
			{
				criteria = sortingCriteria
			};
			return new DrawingSettings(shaderTagId, sortingSettings)
			{
				perObjectData = renderingData.perObjectData,
				mainLightIndex = renderingData.lightData.mainLightIndex,
				enableDynamicBatching = renderingData.supportsDynamicBatching,
				enableInstancing = (camera.cameraType != CameraType.Preview)
			};
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x00027290 File Offset: 0x00025490
		public static DrawingSettings CreateDrawingSettings(List<ShaderTagId> shaderTagIdList, ref RenderingData renderingData, SortingCriteria sortingCriteria)
		{
			if (shaderTagIdList == null || shaderTagIdList.Count == 0)
			{
				Debug.LogWarning("ShaderTagId list is invalid. DrawingSettings is created with default pipeline ShaderTagId");
				return RenderingUtils.CreateDrawingSettings(new ShaderTagId("UniversalPipeline"), ref renderingData, sortingCriteria);
			}
			DrawingSettings drawingSettings = RenderingUtils.CreateDrawingSettings(shaderTagIdList[0], ref renderingData, sortingCriteria);
			for (int i = 1; i < shaderTagIdList.Count; i++)
			{
				drawingSettings.SetShaderPassName(i, shaderTagIdList[i]);
			}
			return drawingSettings;
		}

		// Token: 0x04000529 RID: 1321
		private static List<ShaderTagId> m_LegacyShaderPassNames = new List<ShaderTagId>
		{
			new ShaderTagId("Always"),
			new ShaderTagId("ForwardBase"),
			new ShaderTagId("PrepassBase"),
			new ShaderTagId("Vertex"),
			new ShaderTagId("VertexLMRGBM"),
			new ShaderTagId("VertexLM")
		};

		// Token: 0x0400052A RID: 1322
		private static AttachmentDescriptor s_EmptyAttachment = new AttachmentDescriptor(GraphicsFormat.None);

		// Token: 0x0400052B RID: 1323
		private static Mesh s_FullscreenMesh = null;

		// Token: 0x0400052C RID: 1324
		private static Material s_ErrorMaterial;

		// Token: 0x0400052D RID: 1325
		private static Dictionary<RenderTextureFormat, bool> m_RenderTextureFormatSupport = new Dictionary<RenderTextureFormat, bool>();

		// Token: 0x0400052E RID: 1326
		private static Dictionary<GraphicsFormat, Dictionary<FormatUsage, bool>> m_GraphicsFormatSupport = new Dictionary<GraphicsFormat, Dictionary<FormatUsage, bool>>();
	}
}
