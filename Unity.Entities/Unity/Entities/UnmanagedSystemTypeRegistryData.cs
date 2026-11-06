using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x020002D2 RID: 722
	internal struct UnmanagedSystemTypeRegistryData
	{
		// Token: 0x1700012C RID: 300
		// (get) Token: 0x0600133C RID: 4924 RVA: 0x0004B605 File Offset: 0x00049805
		public bool Constructed
		{
			get
			{
				return this.m_Delegates.Ptr != null;
			}
		}

		// Token: 0x0600133D RID: 4925 RVA: 0x0004B619 File Offset: 0x00049819
		internal void Construct()
		{
			this.m_TypeHashToIndex = new UnsafeParallelHashMap<long, int>(64, Allocator.Persistent);
			this.m_Delegates = new UnsafeList<UnmanagedComponentSystemDelegates>(64, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
		}

		// Token: 0x0600133E RID: 4926 RVA: 0x0004B644 File Offset: 0x00049844
		internal void Dispose()
		{
			if (this.Constructed)
			{
				for (int i = 0; i < this.m_Delegates.Length; i++)
				{
					this.m_Delegates[i].Dispose();
				}
				this.m_Delegates.Dispose();
				this.m_TypeHashToIndex.Dispose();
			}
			this = default(UnmanagedSystemTypeRegistryData);
		}

		// Token: 0x0600133F RID: 4927 RVA: 0x0004B6A0 File Offset: 0x000498A0
		internal int AddSystemType(long typeHash, UnmanagedComponentSystemDelegates delegates)
		{
			int num;
			if (this.m_TypeHashToIndex.TryGetValue(typeHash, out num))
			{
				this.m_Delegates[num] = delegates;
				return num;
			}
			int length = this.m_Delegates.Length;
			this.m_TypeHashToIndex.Add(typeHash, length);
			this.m_Delegates.Add(in delegates);
			return length;
		}

		// Token: 0x06001340 RID: 4928 RVA: 0x0004B6F3 File Offset: 0x000498F3
		internal bool FindSystemMetaIndex(long typeHash, out int index)
		{
			return this.m_TypeHashToIndex.TryGetValue(typeHash, out index);
		}

		// Token: 0x06001341 RID: 4929 RVA: 0x0004B702 File Offset: 0x00049902
		internal readonly ref UnmanagedComponentSystemDelegates GetSystemDelegates(int index)
		{
			return this.m_Delegates.Ptr + index;
		}

		// Token: 0x040006F7 RID: 1783
		private UnsafeParallelHashMap<long, int> m_TypeHashToIndex;

		// Token: 0x040006F8 RID: 1784
		private UnsafeList<UnmanagedComponentSystemDelegates> m_Delegates;
	}
}
