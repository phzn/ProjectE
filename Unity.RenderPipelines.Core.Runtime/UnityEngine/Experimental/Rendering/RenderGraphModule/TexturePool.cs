using System;
using System.Collections.Generic;
using UnityEngine.Profiling;
using UnityEngine.Rendering;

namespace UnityEngine.Experimental.Rendering.RenderGraphModule
{
	// Token: 0x02000031 RID: 49
	internal class TexturePool : RenderGraphResourcePool<RTHandle>
	{
		// Token: 0x060001E4 RID: 484 RVA: 0x00009A95 File Offset: 0x00007C95
		protected override void ReleaseInternalResource(RTHandle res)
		{
			res.Release();
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x00009A9D File Offset: 0x00007C9D
		protected override string GetResourceName(RTHandle res)
		{
			return res.rt.name;
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x00009AAA File Offset: 0x00007CAA
		protected override long GetResourceSize(RTHandle res)
		{
			return Profiler.GetRuntimeMemorySizeLong(res.rt);
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x00009AB7 File Offset: 0x00007CB7
		protected override string GetResourceTypeName()
		{
			return "Texture";
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x00009ABE File Offset: 0x00007CBE
		protected override int GetSortIndex(RTHandle res)
		{
			return res.GetInstanceID();
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x00009AC8 File Offset: 0x00007CC8
		public override void PurgeUnusedResources(int currentFrameIndex)
		{
			RenderGraphResourcePool<RTHandle>.s_CurrentFrameIndex = currentFrameIndex;
			this.m_RemoveList.Clear();
			foreach (KeyValuePair<int, SortedList<int, ValueTuple<RTHandle, int>>> keyValuePair in this.m_ResourcePool)
			{
				SortedList<int, ValueTuple<RTHandle, int>> value = keyValuePair.Value;
				IList<int> keys = value.Keys;
				IList<ValueTuple<RTHandle, int>> values = value.Values;
				for (int i = 0; i < value.Count; i++)
				{
					ValueTuple<RTHandle, int> valueTuple = values[i];
					if (RenderGraphResourcePool<RTHandle>.ShouldReleaseResource(valueTuple.Item2, RenderGraphResourcePool<RTHandle>.s_CurrentFrameIndex))
					{
						valueTuple.Item1.Release();
						this.m_RemoveList.Add(keys[i]);
					}
				}
				foreach (int num in this.m_RemoveList)
				{
					value.Remove(num);
				}
			}
		}
	}
}
