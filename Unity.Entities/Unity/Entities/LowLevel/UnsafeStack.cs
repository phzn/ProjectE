using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;

namespace Unity.Entities.LowLevel
{
	// Token: 0x020004C9 RID: 1225
	[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
	internal struct UnsafeStack<[global::System.Runtime.CompilerServices.IsUnmanaged] T> : INativeDisposable, IDisposable where T : struct, ValueType
	{
		// Token: 0x17000264 RID: 612
		// (get) Token: 0x06001D69 RID: 7529 RVA: 0x00066877 File Offset: 0x00064A77
		public bool IsEmpty
		{
			get
			{
				return this.m_List.IsEmpty;
			}
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x06001D6A RID: 7530 RVA: 0x00066884 File Offset: 0x00064A84
		public int Length
		{
			get
			{
				return this.m_List.Length;
			}
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06001D6B RID: 7531 RVA: 0x00066891 File Offset: 0x00064A91
		public bool IsCreated
		{
			get
			{
				return this.m_List.IsCreated;
			}
		}

		// Token: 0x06001D6C RID: 7532 RVA: 0x0006689E File Offset: 0x00064A9E
		public UnsafeStack(int initialCapacity, Allocator allocator, NativeArrayOptions options = NativeArrayOptions.UninitializedMemory)
		{
			this.m_List = new UnsafeList<T>(initialCapacity, allocator, options);
		}

		// Token: 0x06001D6D RID: 7533 RVA: 0x000668B3 File Offset: 0x00064AB3
		public void Push(in T item)
		{
			this.m_List.Add(in item);
		}

		// Token: 0x06001D6E RID: 7534 RVA: 0x000668C1 File Offset: 0x00064AC1
		public ref T Top()
		{
			return this.m_List.ElementAt(this.m_List.m_length - 1);
		}

		// Token: 0x06001D6F RID: 7535 RVA: 0x000668DB File Offset: 0x00064ADB
		public void Pop()
		{
			this.m_List.Resize(this.m_List.m_length - 1, NativeArrayOptions.UninitializedMemory);
		}

		// Token: 0x06001D70 RID: 7536 RVA: 0x000668F6 File Offset: 0x00064AF6
		public void Clear()
		{
			this.m_List.Clear();
		}

		// Token: 0x06001D71 RID: 7537 RVA: 0x00066903 File Offset: 0x00064B03
		public void Dispose()
		{
			this.m_List.Dispose();
		}

		// Token: 0x06001D72 RID: 7538 RVA: 0x00066910 File Offset: 0x00064B10
		public JobHandle Dispose(JobHandle inputDeps)
		{
			return this.m_List.Dispose(inputDeps);
		}

		// Token: 0x04000A91 RID: 2705
		private UnsafeList<T> m_List;
	}
}
