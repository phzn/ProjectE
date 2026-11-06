using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Serialization.Json
{
	// Token: 0x02000045 RID: 69
	internal struct JsonTokenStream : IDisposable
	{
		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000175 RID: 373 RVA: 0x000065D3 File Offset: 0x000047D3
		public unsafe Token* Tokens
		{
			get
			{
				return this.m_Data->Tokens;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000176 RID: 374 RVA: 0x000065E0 File Offset: 0x000047E0
		public unsafe int TokenNextIndex
		{
			get
			{
				return this.m_Data->TokenNextIndex;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000177 RID: 375 RVA: 0x000065ED File Offset: 0x000047ED
		public unsafe int TokenParentIndex
		{
			get
			{
				return this.m_Data->TokenParentIndex;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000178 RID: 376 RVA: 0x000065FA File Offset: 0x000047FA
		public unsafe int* Discard
		{
			get
			{
				return this.m_Data->Discard;
			}
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00006607 File Offset: 0x00004807
		internal unsafe UnsafeJsonTokenStream* GetUnsafePtr()
		{
			return this.m_Data;
		}

		// Token: 0x0600017A RID: 378 RVA: 0x0000660F File Offset: 0x0000480F
		public JsonTokenStream(Allocator allocator)
		{
			this = new JsonTokenStream(1024, allocator);
		}

		// Token: 0x0600017B RID: 379 RVA: 0x0000661D File Offset: 0x0000481D
		public unsafe JsonTokenStream(int initialLength, Allocator allocator)
		{
			this.m_Allocator = allocator;
			this.m_Data = (UnsafeJsonTokenStream*)UnsafeUtility.Malloc((long)sizeof(UnsafeJsonTokenStream), UnsafeUtility.AlignOf<UnsafeJsonTokenStream>(), this.m_Allocator);
			*this.m_Data = new UnsafeJsonTokenStream(initialLength, allocator);
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00006655 File Offset: 0x00004855
		public unsafe void Dispose()
		{
			this.m_Data->Dispose();
			UnsafeUtility.Free((void*)this.m_Data, this.m_Allocator);
			this.m_Data = null;
		}

		// Token: 0x0600017D RID: 381 RVA: 0x0000667B File Offset: 0x0000487B
		public unsafe readonly void Reset()
		{
			this.m_Data->Reset();
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00006688 File Offset: 0x00004888
		public unsafe readonly void DiscardCompleted(int depth = 128)
		{
			this.m_Data->DiscardCompleted(depth);
		}

		// Token: 0x040000AB RID: 171
		private const int k_DefaultBufferSize = 1024;

		// Token: 0x040000AC RID: 172
		private readonly Allocator m_Allocator;

		// Token: 0x040000AD RID: 173
		[NativeDisableUnsafePtrRestriction]
		private unsafe UnsafeJsonTokenStream* m_Data;
	}
}
