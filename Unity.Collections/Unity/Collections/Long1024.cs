using System;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Collections
{
	// Token: 0x02000043 RID: 67
	internal struct Long1024 : IIndexable<long>
	{
		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000166 RID: 358 RVA: 0x00005273 File Offset: 0x00003473
		// (set) Token: 0x06000167 RID: 359 RVA: 0x00002C47 File Offset: 0x00000E47
		public int Length
		{
			get
			{
				return 1024;
			}
			set
			{
			}
		}

		// Token: 0x06000168 RID: 360 RVA: 0x0000527C File Offset: 0x0000347C
		public unsafe ref long ElementAt(int index)
		{
			fixed (Long512* ptr = &this.f0)
			{
				return UnsafeUtility.AsRef<long>((void*)((byte*)ptr + (IntPtr)index * 8));
			}
		}

		// Token: 0x040000A4 RID: 164
		internal Long512 f0;

		// Token: 0x040000A5 RID: 165
		internal Long512 f1;
	}
}
