using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine.Experimental.Rendering;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000BC RID: 188
	internal struct ReflectionProbeManager : IDisposable
	{
		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000604 RID: 1540 RVA: 0x00024810 File Offset: 0x00022A10
		public RenderTexture atlasRT
		{
			get
			{
				return this.m_AtlasTexture0;
			}
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x00024818 File Offset: 0x00022A18
		public static ReflectionProbeManager Create()
		{
			ReflectionProbeManager reflectionProbeManager = default(ReflectionProbeManager);
			reflectionProbeManager.Init();
			return reflectionProbeManager;
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x00024838 File Offset: 0x00022A38
		private void Init()
		{
			int maxVisibleReflectionProbes = UniversalRenderPipeline.maxVisibleReflectionProbes;
			this.m_Resolution = 1;
			GraphicsFormat graphicsFormat = GraphicsFormat.B10G11R11_UFloatPack32;
			if (!SystemInfo.IsFormatSupported(graphicsFormat, FormatUsage.Render))
			{
				graphicsFormat = GraphicsFormat.R16G16B16A16_SFloat;
			}
			this.m_AtlasTexture0 = new RenderTexture(new RenderTextureDescriptor
			{
				width = this.m_Resolution.x,
				height = this.m_Resolution.y,
				volumeDepth = 1,
				dimension = TextureDimension.Tex2D,
				graphicsFormat = graphicsFormat,
				useMipMap = false,
				msaaSamples = 1
			});
			this.m_AtlasTexture0.name = "URP Reflection Probe Atlas";
			this.m_AtlasTexture0.filterMode = FilterMode.Bilinear;
			this.m_AtlasTexture0.hideFlags = HideFlags.HideAndDontSave;
			this.m_AtlasTexture0.Create();
			this.m_AtlasTexture1 = new RenderTexture(this.m_AtlasTexture0.descriptor);
			this.m_AtlasTexture1.name = "URP Reflection Probe Atlas";
			this.m_AtlasTexture1.filterMode = FilterMode.Bilinear;
			this.m_AtlasTexture1.hideFlags = HideFlags.HideAndDontSave;
			this.m_AtlasAllocator = new BuddyAllocator(math.floorlog2(SystemInfo.maxTextureSize) - 2, 2, Allocator.Persistent);
			this.m_Cache = new Dictionary<int, ReflectionProbeManager.CachedProbe>(maxVisibleReflectionProbes);
			this.m_WarningCache = new Dictionary<int, int>(maxVisibleReflectionProbes);
			this.m_NeedsUpdate = new List<int>(maxVisibleReflectionProbes);
			this.m_NeedsRemove = new List<int>(maxVisibleReflectionProbes);
			this.m_BoxMax = new Vector4[maxVisibleReflectionProbes];
			this.m_BoxMin = new Vector4[maxVisibleReflectionProbes];
			this.m_ProbePosition = new Vector4[maxVisibleReflectionProbes];
			this.m_MipScaleOffset = new Vector4[maxVisibleReflectionProbes * 7];
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x000249B8 File Offset: 0x00022BB8
		public unsafe void UpdateGpuData(CommandBuffer cmd, ref RenderingData renderingData)
		{
			NativeArray<VisibleReflectionProbe> visibleReflectionProbes = renderingData.cullResults.visibleReflectionProbes;
			int num = math.min(visibleReflectionProbes.Length, UniversalRenderPipeline.maxVisibleReflectionProbes);
			int renderedFrameCount = Time.renderedFrameCount;
			foreach (KeyValuePair<int, ReflectionProbeManager.CachedProbe> keyValuePair in this.m_Cache)
			{
				int num2;
				ReflectionProbeManager.CachedProbe cachedProbe;
				keyValuePair.Deconstruct(out num2, out cachedProbe);
				int num3 = num2;
				ReflectionProbeManager.CachedProbe cachedProbe2 = cachedProbe;
				if (Math.Abs(cachedProbe2.lastUsed - renderedFrameCount) > 1 || !cachedProbe2.texture || cachedProbe2.size != cachedProbe2.texture.width)
				{
					this.m_NeedsRemove.Add(num3);
					for (int i = 0; i < 7; i++)
					{
						if (*((ref cachedProbe2.dataIndices.FixedElementField) + (IntPtr)i * 4) != -1)
						{
							this.m_AtlasAllocator.Free(new BuddyAllocation(*((ref cachedProbe2.levels.FixedElementField) + (IntPtr)i * 4), *((ref cachedProbe2.dataIndices.FixedElementField) + (IntPtr)i * 4)));
						}
					}
				}
			}
			foreach (int num4 in this.m_NeedsRemove)
			{
				this.m_Cache.Remove(num4);
			}
			this.m_NeedsRemove.Clear();
			foreach (KeyValuePair<int, int> keyValuePair2 in this.m_WarningCache)
			{
				int num2;
				int num5;
				keyValuePair2.Deconstruct(out num2, out num5);
				int num6 = num2;
				if (Math.Abs(num5 - renderedFrameCount) > 1)
				{
					this.m_NeedsRemove.Add(num6);
				}
			}
			foreach (int num7 in this.m_NeedsRemove)
			{
				this.m_WarningCache.Remove(num7);
			}
			this.m_NeedsRemove.Clear();
			bool flag = false;
			int2 @int = math.int2(0, 0);
			for (int j = 0; j < num; j++)
			{
				VisibleReflectionProbe visibleReflectionProbe = visibleReflectionProbes[j];
				Texture texture = visibleReflectionProbe.texture;
				int instanceID = visibleReflectionProbe.reflectionProbe.GetInstanceID();
				ReflectionProbeManager.CachedProbe cachedProbe3;
				bool flag2 = this.m_Cache.TryGetValue(instanceID, out cachedProbe3);
				if (texture)
				{
					if (!flag2)
					{
						cachedProbe3.size = texture.width;
						int num8 = math.ceillog2(cachedProbe3.size * 4) + 1;
						int num9 = this.m_AtlasAllocator.levelCount + 2 - num8;
						cachedProbe3.mipCount = math.min(num8, 7);
						cachedProbe3.texture = texture;
						int k;
						for (k = 0; k < cachedProbe3.mipCount; k++)
						{
							int num10 = math.min(num9 + k, this.m_AtlasAllocator.levelCount - 1);
							BuddyAllocation buddyAllocation;
							if (!this.m_AtlasAllocator.TryAllocate(num10, out buddyAllocation))
							{
								break;
							}
							*((ref cachedProbe3.levels.FixedElementField) + (IntPtr)k * 4) = buddyAllocation.level;
							*((ref cachedProbe3.dataIndices.FixedElementField) + (IntPtr)k * 4) = buddyAllocation.index;
							int4 int2 = (int4)(this.GetScaleOffset(num10, buddyAllocation.index, true, false) * this.m_Resolution.xyxy);
							@int = math.max(@int, int2.zw + int2.xy);
						}
						if (k < cachedProbe3.mipCount)
						{
							if (!this.m_WarningCache.ContainsKey(instanceID))
							{
								flag = true;
							}
							this.m_WarningCache[instanceID] = renderedFrameCount;
							for (int l = 0; l < k; l++)
							{
								this.m_AtlasAllocator.Free(new BuddyAllocation(*((ref cachedProbe3.levels.FixedElementField) + (IntPtr)l * 4), *((ref cachedProbe3.dataIndices.FixedElementField) + (IntPtr)l * 4)));
							}
							for (int m = 0; m < 7; m++)
							{
								*((ref cachedProbe3.dataIndices.FixedElementField) + (IntPtr)m * 4) = -1;
							}
							goto IL_04A6;
						}
						while (k < 7)
						{
							*((ref cachedProbe3.dataIndices.FixedElementField) + (IntPtr)k * 4) = -1;
							k++;
						}
					}
					if ((!flag2 || cachedProbe3.updateCount != texture.updateCount) | (cachedProbe3.hdrData != visibleReflectionProbe.hdrData))
					{
						cachedProbe3.updateCount = texture.updateCount;
						this.m_NeedsUpdate.Add(instanceID);
					}
					if (visibleReflectionProbe.reflectionProbe.refreshMode == ReflectionProbeRefreshMode.EveryFrame)
					{
						cachedProbe3.lastUsed = -1;
					}
					else
					{
						cachedProbe3.lastUsed = renderedFrameCount;
					}
					cachedProbe3.hdrData = visibleReflectionProbe.hdrData;
					this.m_Cache[instanceID] = cachedProbe3;
				}
				IL_04A6:;
			}
			if (math.any(this.m_Resolution < @int))
			{
				@int = math.max(this.m_Resolution, math.ceilpow2(@int));
				RenderTextureDescriptor descriptor = this.m_AtlasTexture0.descriptor;
				descriptor.width = @int.x;
				descriptor.height = @int.y;
				this.m_AtlasTexture1.width = @int.x;
				this.m_AtlasTexture1.height = @int.y;
				this.m_AtlasTexture1.Create();
				if (this.m_AtlasTexture0.width != 1)
				{
					if (SystemInfo.copyTextureSupport != CopyTextureSupport.None)
					{
						Graphics.CopyTexture(this.m_AtlasTexture0, 0, 0, 0, 0, this.m_Resolution.x, this.m_Resolution.y, this.m_AtlasTexture1, 0, 0, 0, 0);
					}
					else
					{
						Graphics.Blit(this.m_AtlasTexture0, this.m_AtlasTexture1, this.m_Resolution / @int, Vector2.zero);
					}
				}
				this.m_AtlasTexture0.Release();
				RenderTexture atlasTexture = this.m_AtlasTexture1;
				RenderTexture atlasTexture2 = this.m_AtlasTexture0;
				this.m_AtlasTexture0 = atlasTexture;
				this.m_AtlasTexture1 = atlasTexture2;
				this.m_Resolution = @int;
			}
			int num11 = 0;
			for (int n = 0; n < num; n++)
			{
				VisibleReflectionProbe visibleReflectionProbe2 = visibleReflectionProbes[n];
				int instanceID2 = visibleReflectionProbe2.reflectionProbe.GetInstanceID();
				int num12 = n - num11;
				ReflectionProbeManager.CachedProbe cachedProbe4;
				if (!this.m_Cache.TryGetValue(instanceID2, out cachedProbe4) || !visibleReflectionProbe2.texture)
				{
					num11++;
				}
				else
				{
					this.m_BoxMax[num12] = new Vector4(visibleReflectionProbe2.bounds.max.x, visibleReflectionProbe2.bounds.max.y, visibleReflectionProbe2.bounds.max.z, visibleReflectionProbe2.blendDistance);
					this.m_BoxMin[num12] = new Vector4(visibleReflectionProbe2.bounds.min.x, visibleReflectionProbe2.bounds.min.y, visibleReflectionProbe2.bounds.min.z, (float)visibleReflectionProbe2.importance);
					this.m_ProbePosition[num12] = new Vector4(visibleReflectionProbe2.localToWorldMatrix.m03, visibleReflectionProbe2.localToWorldMatrix.m13, visibleReflectionProbe2.localToWorldMatrix.m23, (float)((visibleReflectionProbe2.isBoxProjection ? 1 : (-1)) * cachedProbe4.mipCount));
					for (int num13 = 0; num13 < cachedProbe4.mipCount; num13++)
					{
						this.m_MipScaleOffset[num12 * 7 + num13] = this.GetScaleOffset(*((ref cachedProbe4.levels.FixedElementField) + (IntPtr)num13 * 4), *((ref cachedProbe4.dataIndices.FixedElementField) + (IntPtr)num13 * 4), false, false);
					}
				}
			}
			if (flag)
			{
				Debug.LogWarning("A number of reflection probes have been skipped due to the reflection probe atlas being full.\nTo fix this, you can decrease the number or resolution of probes.");
			}
			using (new ProfilingScope(cmd, ProfilingSampler.Get<URPProfileId>(URPProfileId.UpdateReflectionProbeAtlas)))
			{
				cmd.SetRenderTarget(this.m_AtlasTexture0);
				foreach (int num14 in this.m_NeedsUpdate)
				{
					ReflectionProbeManager.CachedProbe cachedProbe5 = this.m_Cache[num14];
					for (int num15 = 0; num15 < cachedProbe5.mipCount; num15++)
					{
						int num16 = *((ref cachedProbe5.levels.FixedElementField) + (IntPtr)num15 * 4);
						int num17 = *((ref cachedProbe5.dataIndices.FixedElementField) + (IntPtr)num15 * 4);
						float4 scaleOffset = this.GetScaleOffset(num16, num17, true, !SystemInfo.graphicsUVStartsAtTop);
						int num18 = (1 << this.m_AtlasAllocator.levelCount + 1 - num16) - 2;
						Blitter.BlitCubeToOctahedral2DQuadWithPadding(cmd, cachedProbe5.texture, new Vector2((float)num18, (float)num18), scaleOffset, num15, true, 2, new Vector4?(cachedProbe5.hdrData));
					}
				}
				cmd.SetGlobalVectorArray(ReflectionProbeManager.ShaderProperties.BoxMin, this.m_BoxMin);
				cmd.SetGlobalVectorArray(ReflectionProbeManager.ShaderProperties.BoxMax, this.m_BoxMax);
				cmd.SetGlobalVectorArray(ReflectionProbeManager.ShaderProperties.ProbePosition, this.m_ProbePosition);
				cmd.SetGlobalVectorArray(ReflectionProbeManager.ShaderProperties.MipScaleOffset, this.m_MipScaleOffset);
				cmd.SetGlobalFloat(ReflectionProbeManager.ShaderProperties.Count, (float)(num - num11));
				cmd.SetGlobalTexture(ReflectionProbeManager.ShaderProperties.Atlas, this.m_AtlasTexture0);
			}
			this.m_NeedsUpdate.Clear();
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x00025394 File Offset: 0x00023594
		private float4 GetScaleOffset(int level, int dataIndex, bool includePadding, bool yflip)
		{
			int num = 1 << this.m_AtlasAllocator.levelCount + 1 - level;
			uint2 @uint = SpaceFillingCurves.DecodeMorton2D((uint)dataIndex);
			float2 @float = (float)(num - (includePadding ? 0 : 2)) / this.m_Resolution;
			float2 float2 = (@uint * (float)num + (float)(includePadding ? 0 : 1)) / this.m_Resolution;
			if (yflip)
			{
				float2.y = 1f - float2.y - @float.y;
			}
			return math.float4(@float, float2);
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x00025426 File Offset: 0x00023626
		public void Dispose()
		{
			if (this.m_AtlasTexture0)
			{
				this.m_AtlasTexture0.Release();
			}
			Object.DestroyImmediate(this.m_AtlasTexture0);
			Object.DestroyImmediate(this.m_AtlasTexture1);
			this = default(ReflectionProbeManager);
		}

		// Token: 0x040004C3 RID: 1219
		private int2 m_Resolution;

		// Token: 0x040004C4 RID: 1220
		private RenderTexture m_AtlasTexture0;

		// Token: 0x040004C5 RID: 1221
		private RenderTexture m_AtlasTexture1;

		// Token: 0x040004C6 RID: 1222
		private BuddyAllocator m_AtlasAllocator;

		// Token: 0x040004C7 RID: 1223
		private Dictionary<int, ReflectionProbeManager.CachedProbe> m_Cache;

		// Token: 0x040004C8 RID: 1224
		private Dictionary<int, int> m_WarningCache;

		// Token: 0x040004C9 RID: 1225
		private List<int> m_NeedsUpdate;

		// Token: 0x040004CA RID: 1226
		private List<int> m_NeedsRemove;

		// Token: 0x040004CB RID: 1227
		private Vector4[] m_BoxMax;

		// Token: 0x040004CC RID: 1228
		private Vector4[] m_BoxMin;

		// Token: 0x040004CD RID: 1229
		private Vector4[] m_ProbePosition;

		// Token: 0x040004CE RID: 1230
		private Vector4[] m_MipScaleOffset;

		// Token: 0x040004CF RID: 1231
		private const int k_MaxMipCount = 7;

		// Token: 0x040004D0 RID: 1232
		private const string k_ReflectionProbeAtlasName = "URP Reflection Probe Atlas";

		// Token: 0x020001AB RID: 427
		private struct CachedProbe
		{
			// Token: 0x04000B25 RID: 2853
			public uint updateCount;

			// Token: 0x04000B26 RID: 2854
			public Hash128 imageContentsHash;

			// Token: 0x04000B27 RID: 2855
			public int size;

			// Token: 0x04000B28 RID: 2856
			public int mipCount;

			// Token: 0x04000B29 RID: 2857
			[FixedBuffer(typeof(int), 7)]
			public ReflectionProbeManager.CachedProbe.<dataIndices>e__FixedBuffer dataIndices;

			// Token: 0x04000B2A RID: 2858
			[FixedBuffer(typeof(int), 7)]
			public ReflectionProbeManager.CachedProbe.<levels>e__FixedBuffer levels;

			// Token: 0x04000B2B RID: 2859
			public Texture texture;

			// Token: 0x04000B2C RID: 2860
			public int lastUsed;

			// Token: 0x04000B2D RID: 2861
			public Vector4 hdrData;

			// Token: 0x02000231 RID: 561
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 28)]
			public struct <dataIndices>e__FixedBuffer
			{
				// Token: 0x04000D3A RID: 3386
				public int FixedElementField;
			}

			// Token: 0x02000232 RID: 562
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 28)]
			public struct <levels>e__FixedBuffer
			{
				// Token: 0x04000D3B RID: 3387
				public int FixedElementField;
			}
		}

		// Token: 0x020001AC RID: 428
		private static class ShaderProperties
		{
			// Token: 0x04000B2E RID: 2862
			public static readonly int BoxMin = Shader.PropertyToID("urp_ReflProbes_BoxMin");

			// Token: 0x04000B2F RID: 2863
			public static readonly int BoxMax = Shader.PropertyToID("urp_ReflProbes_BoxMax");

			// Token: 0x04000B30 RID: 2864
			public static readonly int ProbePosition = Shader.PropertyToID("urp_ReflProbes_ProbePosition");

			// Token: 0x04000B31 RID: 2865
			public static readonly int MipScaleOffset = Shader.PropertyToID("urp_ReflProbes_MipScaleOffset");

			// Token: 0x04000B32 RID: 2866
			public static readonly int Count = Shader.PropertyToID("urp_ReflProbes_Count");

			// Token: 0x04000B33 RID: 2867
			public static readonly int Atlas = Shader.PropertyToID("urp_ReflProbes_Atlas");
		}
	}
}
