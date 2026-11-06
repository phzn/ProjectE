using System;
using System.Collections.Generic;

namespace UnityEngine.Experimental.Rendering.RenderGraphModule
{
	// Token: 0x02000022 RID: 34
	internal class ComputeBufferPool : RenderGraphResourcePool<ComputeBuffer>
	{
		// Token: 0x0600015C RID: 348 RVA: 0x00007DDD File Offset: 0x00005FDD
		protected override void ReleaseInternalResource(ComputeBuffer res)
		{
			res.Release();
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00007DE5 File Offset: 0x00005FE5
		protected override string GetResourceName(ComputeBuffer res)
		{
			return "ComputeBufferNameNotAvailable";
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00007DEC File Offset: 0x00005FEC
		protected override long GetResourceSize(ComputeBuffer res)
		{
			return (long)(res.count * res.stride);
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00007DFC File Offset: 0x00005FFC
		protected override string GetResourceTypeName()
		{
			return "ComputeBuffer";
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00007E03 File Offset: 0x00006003
		protected override int GetSortIndex(ComputeBuffer res)
		{
			return res.GetHashCode();
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00007E0C File Offset: 0x0000600C
		public override void PurgeUnusedResources(int currentFrameIndex)
		{
			RenderGraphResourcePool<ComputeBuffer>.s_CurrentFrameIndex = currentFrameIndex;
			this.m_RemoveList.Clear();
			foreach (KeyValuePair<int, SortedList<int, ValueTuple<ComputeBuffer, int>>> keyValuePair in this.m_ResourcePool)
			{
				SortedList<int, ValueTuple<ComputeBuffer, int>> value = keyValuePair.Value;
				IList<int> keys = value.Keys;
				IList<ValueTuple<ComputeBuffer, int>> values = value.Values;
				for (int i = 0; i < value.Count; i++)
				{
					ValueTuple<ComputeBuffer, int> valueTuple = values[i];
					if (RenderGraphResourcePool<ComputeBuffer>.ShouldReleaseResource(valueTuple.Item2, RenderGraphResourcePool<ComputeBuffer>.s_CurrentFrameIndex))
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
