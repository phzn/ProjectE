using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UnityEngine.Experimental.Rendering.RenderGraphModule
{
	// Token: 0x02000024 RID: 36
	internal abstract class RenderGraphResourcePool<Type> : IRenderGraphResourcePool where Type : class
	{
		// Token: 0x06000168 RID: 360
		protected abstract void ReleaseInternalResource(Type res);

		// Token: 0x06000169 RID: 361
		protected abstract string GetResourceName(Type res);

		// Token: 0x0600016A RID: 362
		protected abstract long GetResourceSize(Type res);

		// Token: 0x0600016B RID: 363
		protected abstract string GetResourceTypeName();

		// Token: 0x0600016C RID: 364
		protected abstract int GetSortIndex(Type res);

		// Token: 0x0600016D RID: 365 RVA: 0x00007F30 File Offset: 0x00006130
		public void ReleaseResource(int hash, Type resource, int currentFrameIndex)
		{
			SortedList<int, ValueTuple<Type, int>> sortedList;
			if (!this.m_ResourcePool.TryGetValue(hash, out sortedList))
			{
				sortedList = new SortedList<int, ValueTuple<Type, int>>();
				this.m_ResourcePool.Add(hash, sortedList);
			}
			sortedList.Add(this.GetSortIndex(resource), new ValueTuple<Type, int>(resource, currentFrameIndex));
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00007F74 File Offset: 0x00006174
		public bool TryGetResource(int hashCode, out Type resource)
		{
			SortedList<int, ValueTuple<Type, int>> sortedList;
			if (this.m_ResourcePool.TryGetValue(hashCode, out sortedList) && sortedList.Count > 0)
			{
				resource = sortedList.Values[sortedList.Count - 1].Item1;
				sortedList.RemoveAt(sortedList.Count - 1);
				return true;
			}
			resource = default(Type);
			return false;
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00007FD0 File Offset: 0x000061D0
		public override void Cleanup()
		{
			foreach (KeyValuePair<int, SortedList<int, ValueTuple<Type, int>>> keyValuePair in this.m_ResourcePool)
			{
				foreach (KeyValuePair<int, ValueTuple<Type, int>> keyValuePair2 in keyValuePair.Value)
				{
					this.ReleaseInternalResource(keyValuePair2.Value.Item1);
				}
			}
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00008064 File Offset: 0x00006264
		public void RegisterFrameAllocation(int hash, Type value)
		{
			if (hash != -1)
			{
				this.m_FrameAllocatedResources.Add(new ValueTuple<int, Type>(hash, value));
			}
		}

		// Token: 0x06000171 RID: 369 RVA: 0x0000807C File Offset: 0x0000627C
		public void UnregisterFrameAllocation(int hash, Type value)
		{
			if (hash != -1)
			{
				this.m_FrameAllocatedResources.Remove(new ValueTuple<int, Type>(hash, value));
			}
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00008098 File Offset: 0x00006298
		public override void CheckFrameAllocation(bool onException, int frameIndex)
		{
			if (this.m_FrameAllocatedResources.Count != 0)
			{
				string text = "";
				if (!onException)
				{
					text = "RenderGraph: Not all resources of type " + this.GetResourceTypeName() + " were released. This can be caused by a resources being allocated but never read by any pass.";
				}
				foreach (ValueTuple<int, Type> valueTuple in this.m_FrameAllocatedResources)
				{
					if (!onException)
					{
						text = text + "\n\t" + this.GetResourceName(valueTuple.Item2);
					}
					this.ReleaseResource(valueTuple.Item1, valueTuple.Item2, frameIndex);
				}
				Debug.LogWarning(text);
			}
			this.m_FrameAllocatedResources.Clear();
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00008154 File Offset: 0x00006354
		public override void LogResources(RenderGraphLogger logger)
		{
			List<RenderGraphResourcePool<Type>.ResourceLogInfo> list = new List<RenderGraphResourcePool<Type>.ResourceLogInfo>();
			foreach (KeyValuePair<int, SortedList<int, ValueTuple<Type, int>>> keyValuePair in this.m_ResourcePool)
			{
				foreach (KeyValuePair<int, ValueTuple<Type, int>> keyValuePair2 in keyValuePair.Value)
				{
					list.Add(new RenderGraphResourcePool<Type>.ResourceLogInfo
					{
						name = this.GetResourceName(keyValuePair2.Value.Item1),
						size = this.GetResourceSize(keyValuePair2.Value.Item1)
					});
				}
			}
			logger.LogLine("== " + this.GetResourceTypeName() + " Resources ==", Array.Empty<object>());
			list.Sort(delegate(RenderGraphResourcePool<Type>.ResourceLogInfo a, RenderGraphResourcePool<Type>.ResourceLogInfo b)
			{
				if (a.size >= b.size)
				{
					return -1;
				}
				return 1;
			});
			int num = 0;
			float num2 = 0f;
			foreach (RenderGraphResourcePool<Type>.ResourceLogInfo resourceLogInfo in list)
			{
				float num3 = (float)resourceLogInfo.size / 1048576f;
				num2 += num3;
				logger.LogLine(string.Format("[{0:D2}]\t[{1:0.00} MB]\t{2}", num++, num3, resourceLogInfo.name), Array.Empty<object>());
			}
			logger.LogLine(string.Format("\nTotal Size [{0:0.00}]", num2), Array.Empty<object>());
		}

		// Token: 0x06000174 RID: 372 RVA: 0x0000830C File Offset: 0x0000650C
		protected static bool ShouldReleaseResource(int lastUsedFrameIndex, int currentFrameIndex)
		{
			return lastUsedFrameIndex + 10 < currentFrameIndex;
		}

		// Token: 0x040000C6 RID: 198
		[TupleElementNames(new string[] { "resource", "frameIndex" })]
		protected Dictionary<int, SortedList<int, ValueTuple<Type, int>>> m_ResourcePool = new Dictionary<int, SortedList<int, ValueTuple<Type, int>>>();

		// Token: 0x040000C7 RID: 199
		protected List<int> m_RemoveList = new List<int>(32);

		// Token: 0x040000C8 RID: 200
		private List<ValueTuple<int, Type>> m_FrameAllocatedResources = new List<ValueTuple<int, Type>>();

		// Token: 0x040000C9 RID: 201
		protected static int s_CurrentFrameIndex;

		// Token: 0x040000CA RID: 202
		private const int kStaleResourceLifetime = 10;

		// Token: 0x0200014F RID: 335
		private struct ResourceLogInfo
		{
			// Token: 0x040005CF RID: 1487
			public string name;

			// Token: 0x040005D0 RID: 1488
			public long size;
		}
	}
}
