using System;
using System.Collections.Generic;
using Unity.Collections;

namespace UnityEngine.UIElements.UIR
{
	// Token: 0x02000466 RID: 1126
	internal class TempAllocator<T> : IDisposable where T : struct
	{
		// Token: 0x06002314 RID: 8980 RVA: 0x000880E0 File Offset: 0x000862E0
		public TempAllocator(int poolCapacity, int excessMinCapacity, int excessMaxCapacity)
		{
			Debug.Assert(poolCapacity >= 1);
			Debug.Assert(excessMinCapacity >= 1);
			Debug.Assert(excessMinCapacity <= excessMaxCapacity);
			this.m_ExcessMinCapacity = excessMinCapacity;
			this.m_ExcessMaxCapacity = excessMaxCapacity;
			this.m_NextExcessSize = this.m_ExcessMinCapacity;
			this.m_Pool = default(TempAllocator<T>.Page);
			this.m_Pool.array = new NativeArray<T>(poolCapacity, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			this.m_Excess = new List<TempAllocator<T>.Page>(8);
		}

		// Token: 0x170007F7 RID: 2039
		// (get) Token: 0x06002315 RID: 8981 RVA: 0x00088161 File Offset: 0x00086361
		// (set) Token: 0x06002316 RID: 8982 RVA: 0x00088169 File Offset: 0x00086369
		private protected bool disposed { protected get; private set; }

		// Token: 0x06002317 RID: 8983 RVA: 0x00088172 File Offset: 0x00086372
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002318 RID: 8984 RVA: 0x00088184 File Offset: 0x00086384
		protected void Dispose(bool disposing)
		{
			bool disposed = this.disposed;
			if (!disposed)
			{
				if (disposing)
				{
					this.ReleaseExcess();
					this.m_Pool.array.Dispose();
					this.m_Pool.used = 0;
				}
				this.disposed = true;
			}
		}

		// Token: 0x06002319 RID: 8985 RVA: 0x000881D4 File Offset: 0x000863D4
		public NativeSlice<T> Alloc(int count)
		{
			Debug.Assert(!this.disposed);
			int num = this.m_Pool.used + count;
			bool flag = num <= this.m_Pool.array.Length;
			NativeSlice<T> nativeSlice2;
			if (flag)
			{
				NativeSlice<T> nativeSlice = this.m_Pool.array.Slice<T>(this.m_Pool.used, count);
				this.m_Pool.used = num;
				nativeSlice2 = nativeSlice;
			}
			else
			{
				bool flag2 = count > this.m_ExcessMaxCapacity;
				if (flag2)
				{
					TempAllocator<T>.Page page = new TempAllocator<T>.Page
					{
						array = new NativeArray<T>(count, Allocator.Persistent, NativeArrayOptions.UninitializedMemory),
						used = count
					};
					this.m_Excess.Add(page);
					nativeSlice2 = page.array.Slice<T>(0, count);
				}
				else
				{
					for (int i = this.m_Excess.Count - 1; i >= 0; i--)
					{
						TempAllocator<T>.Page page2 = this.m_Excess[i];
						num = page2.used + count;
						bool flag3 = num <= page2.array.Length;
						if (flag3)
						{
							NativeSlice<T> nativeSlice3 = page2.array.Slice<T>(page2.used, count);
							page2.used = num;
							this.m_Excess[i] = page2;
							return nativeSlice3;
						}
					}
					while (count > this.m_NextExcessSize)
					{
						this.m_NextExcessSize <<= 1;
					}
					TempAllocator<T>.Page page3 = new TempAllocator<T>.Page
					{
						array = new NativeArray<T>(this.m_NextExcessSize, Allocator.Persistent, NativeArrayOptions.UninitializedMemory),
						used = count
					};
					this.m_Excess.Add(page3);
					this.m_NextExcessSize = Mathf.Min(this.m_NextExcessSize << 1, this.m_ExcessMaxCapacity);
					nativeSlice2 = page3.array.Slice<T>(0, count);
				}
			}
			return nativeSlice2;
		}

		// Token: 0x0600231A RID: 8986 RVA: 0x000883AF File Offset: 0x000865AF
		public void Reset()
		{
			this.ReleaseExcess();
			this.m_Pool.used = 0;
			this.m_NextExcessSize = this.m_ExcessMinCapacity;
		}

		// Token: 0x0600231B RID: 8987 RVA: 0x000883D4 File Offset: 0x000865D4
		private void ReleaseExcess()
		{
			foreach (TempAllocator<T>.Page page in this.m_Excess)
			{
				NativeArray<T> array = page.array;
				array.Dispose();
			}
			this.m_Excess.Clear();
		}

		// Token: 0x0600231C RID: 8988 RVA: 0x00088440 File Offset: 0x00086640
		public TempAllocator<T>.Statistics GatherStatistics()
		{
			TempAllocator<T>.Statistics statistics = new TempAllocator<T>.Statistics
			{
				pool = new TempAllocator<T>.PageStatistics
				{
					size = this.m_Pool.array.Length,
					used = this.m_Pool.used
				},
				excess = new TempAllocator<T>.PageStatistics[this.m_Excess.Count]
			};
			for (int i = 0; i < this.m_Excess.Count; i++)
			{
				statistics.excess[i] = new TempAllocator<T>.PageStatistics
				{
					size = this.m_Excess[i].array.Length,
					used = this.m_Excess[i].used
				};
			}
			return statistics;
		}

		// Token: 0x04001032 RID: 4146
		private readonly int m_ExcessMinCapacity;

		// Token: 0x04001033 RID: 4147
		private readonly int m_ExcessMaxCapacity;

		// Token: 0x04001034 RID: 4148
		private TempAllocator<T>.Page m_Pool;

		// Token: 0x04001035 RID: 4149
		private List<TempAllocator<T>.Page> m_Excess;

		// Token: 0x04001036 RID: 4150
		private int m_NextExcessSize;

		// Token: 0x02000467 RID: 1127
		private struct Page
		{
			// Token: 0x04001038 RID: 4152
			public NativeArray<T> array;

			// Token: 0x04001039 RID: 4153
			public int used;
		}

		// Token: 0x02000468 RID: 1128
		public struct Statistics
		{
			// Token: 0x0400103A RID: 4154
			public TempAllocator<T>.PageStatistics pool;

			// Token: 0x0400103B RID: 4155
			public TempAllocator<T>.PageStatistics[] excess;
		}

		// Token: 0x02000469 RID: 1129
		public struct PageStatistics
		{
			// Token: 0x0400103C RID: 4156
			public int size;

			// Token: 0x0400103D RID: 4157
			public int used;
		}
	}
}
