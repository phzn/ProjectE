using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine.Experimental.Rendering.RenderGraphModule;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000CE RID: 206
	internal class RTHandleResourcePool
	{
		// Token: 0x17000191 RID: 401
		// (get) Token: 0x0600067B RID: 1659 RVA: 0x0002756B File Offset: 0x0002576B
		// (set) Token: 0x0600067C RID: 1660 RVA: 0x00027572 File Offset: 0x00025772
		internal int staleResourceCapacity
		{
			get
			{
				return RTHandleResourcePool.s_StaleResourceMaxCapacity;
			}
			set
			{
				if (RTHandleResourcePool.s_StaleResourceMaxCapacity != value)
				{
					RTHandleResourcePool.s_StaleResourceMaxCapacity = value;
					this.Cleanup();
				}
			}
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x00027588 File Offset: 0x00025788
		internal bool AddResourceToPool(in TextureDesc texDesc, RTHandle resource, int currentFrameIndex)
		{
			if (RTHandleResourcePool.s_CurrentStaleResourceCount >= RTHandleResourcePool.s_StaleResourceMaxCapacity)
			{
				return false;
			}
			int hashCodeWithNameHash = this.GetHashCodeWithNameHash(in texDesc);
			SortedList<int, ValueTuple<RTHandle, int>> sortedList;
			if (!this.m_ResourcePool.TryGetValue(hashCodeWithNameHash, out sortedList))
			{
				sortedList = new SortedList<int, ValueTuple<RTHandle, int>>(RTHandleResourcePool.s_StaleResourceMaxCapacity);
				this.m_ResourcePool.Add(hashCodeWithNameHash, sortedList);
			}
			sortedList.Add(resource.GetInstanceID(), new ValueTuple<RTHandle, int>(resource, currentFrameIndex));
			RTHandleResourcePool.s_CurrentStaleResourceCount++;
			return true;
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x000275F4 File Offset: 0x000257F4
		internal bool TryGetResource(in TextureDesc texDesc, out RTHandle resource, bool usepool = true)
		{
			int hashCodeWithNameHash = this.GetHashCodeWithNameHash(in texDesc);
			SortedList<int, ValueTuple<RTHandle, int>> sortedList;
			if (usepool && this.m_ResourcePool.TryGetValue(hashCodeWithNameHash, out sortedList) && sortedList.Count > 0)
			{
				resource = sortedList.Values[sortedList.Count - 1].Item1;
				sortedList.RemoveAt(sortedList.Count - 1);
				RTHandleResourcePool.s_CurrentStaleResourceCount--;
				return true;
			}
			resource = null;
			return false;
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x00027660 File Offset: 0x00025860
		internal void Cleanup()
		{
			foreach (KeyValuePair<int, SortedList<int, ValueTuple<RTHandle, int>>> keyValuePair in this.m_ResourcePool)
			{
				foreach (KeyValuePair<int, ValueTuple<RTHandle, int>> keyValuePair2 in keyValuePair.Value)
				{
					keyValuePair2.Value.Item1.Release();
				}
			}
			this.m_ResourcePool.Clear();
			RTHandleResourcePool.s_CurrentStaleResourceCount = 0;
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x00027704 File Offset: 0x00025904
		protected static bool ShouldReleaseResource(int lastUsedFrameIndex, int currentFrameIndex)
		{
			return lastUsedFrameIndex + RTHandleResourcePool.s_StaleResourceLifetime < currentFrameIndex;
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x00027710 File Offset: 0x00025910
		internal void PurgeUnusedResources(int currentFrameIndex)
		{
			this.m_RemoveList.Clear();
			foreach (KeyValuePair<int, SortedList<int, ValueTuple<RTHandle, int>>> keyValuePair in this.m_ResourcePool)
			{
				SortedList<int, ValueTuple<RTHandle, int>> value = keyValuePair.Value;
				IList<int> keys = value.Keys;
				IList<ValueTuple<RTHandle, int>> values = value.Values;
				for (int i = 0; i < value.Count; i++)
				{
					ValueTuple<RTHandle, int> valueTuple = values[i];
					if (RTHandleResourcePool.ShouldReleaseResource(valueTuple.Item2, currentFrameIndex))
					{
						valueTuple.Item1.Release();
						this.m_RemoveList.Add(keys[i]);
						RTHandleResourcePool.s_CurrentStaleResourceCount--;
					}
				}
				foreach (int num in this.m_RemoveList)
				{
					value.Remove(num);
				}
			}
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x00027824 File Offset: 0x00025A24
		internal void LogDebugInfo()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("RTHandleResourcePool for frame {0}, Total stale resources {1}", Time.frameCount, RTHandleResourcePool.s_CurrentStaleResourceCount);
			stringBuilder.AppendLine();
			foreach (KeyValuePair<int, SortedList<int, ValueTuple<RTHandle, int>>> keyValuePair in this.m_ResourcePool)
			{
				SortedList<int, ValueTuple<RTHandle, int>> value = keyValuePair.Value;
				IList<int> keys = value.Keys;
				IList<ValueTuple<RTHandle, int>> values = value.Values;
				for (int i = 0; i < value.Count; i++)
				{
					ValueTuple<RTHandle, int> valueTuple = values[i];
					stringBuilder.AppendFormat("Resrouce in pool: Name {0} Last active frame index {1} Size {2} x {3} x {4}", new object[]
					{
						valueTuple.Item1.name,
						valueTuple.Item2,
						valueTuple.Item1.rt.descriptor.width,
						valueTuple.Item1.rt.descriptor.height,
						valueTuple.Item1.rt.descriptor.volumeDepth
					});
					stringBuilder.AppendLine();
				}
			}
			Debug.Log(stringBuilder);
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x00027984 File Offset: 0x00025B84
		internal int GetHashCodeWithNameHash(in TextureDesc texDesc)
		{
			TextureDesc textureDesc = texDesc;
			return textureDesc.GetHashCode() * 23 + texDesc.name.GetHashCode();
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x000279B4 File Offset: 0x00025BB4
		internal static TextureDesc CreateTextureDesc(RenderTextureDescriptor desc, TextureSizeMode textureSizeMode = TextureSizeMode.Explicit, int anisoLevel = 1, float mipMapBias = 0f, FilterMode filterMode = FilterMode.Point, TextureWrapMode wrapMode = TextureWrapMode.Clamp, string name = "")
		{
			return new TextureDesc(desc.width, desc.height, false, false)
			{
				sizeMode = textureSizeMode,
				slices = desc.volumeDepth,
				depthBufferBits = (DepthBits)desc.depthBufferBits,
				colorFormat = desc.graphicsFormat,
				filterMode = filterMode,
				wrapMode = wrapMode,
				dimension = desc.dimension,
				enableRandomWrite = desc.enableRandomWrite,
				useMipMap = desc.useMipMap,
				autoGenerateMips = desc.autoGenerateMips,
				isShadowMap = (desc.shadowSamplingMode != ShadowSamplingMode.None),
				anisoLevel = anisoLevel,
				mipMapBias = mipMapBias,
				msaaSamples = (MSAASamples)desc.msaaSamples,
				bindTextureMS = desc.bindMS,
				useDynamicScale = desc.useDynamicScale,
				memoryless = RenderTextureMemoryless.None,
				vrUsage = VRTextureUsage.None,
				name = name
			};
		}

		// Token: 0x04000532 RID: 1330
		[TupleElementNames(new string[] { "resource", "frameIndex" })]
		protected Dictionary<int, SortedList<int, ValueTuple<RTHandle, int>>> m_ResourcePool = new Dictionary<int, SortedList<int, ValueTuple<RTHandle, int>>>();

		// Token: 0x04000533 RID: 1331
		protected List<int> m_RemoveList = new List<int>(32);

		// Token: 0x04000534 RID: 1332
		protected static int s_CurrentStaleResourceCount = 0;

		// Token: 0x04000535 RID: 1333
		protected static int s_StaleResourceLifetime = 3;

		// Token: 0x04000536 RID: 1334
		protected static int s_StaleResourceMaxCapacity = 32;
	}
}
