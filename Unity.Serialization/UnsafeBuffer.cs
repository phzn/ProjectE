using System;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Serialization
{
	// Token: 0x02000007 RID: 7
	internal readonly struct UnsafeBuffer<[IsUnmanaged] T> where T : struct, ValueType
	{
		// Token: 0x06000007 RID: 7 RVA: 0x00002164 File Offset: 0x00000364
		public unsafe UnsafeBuffer(T* buffer, int length)
		{
			this.Buffer = buffer;
			this.Length = length;
		}

		// Token: 0x04000006 RID: 6
		[NativeDisableUnsafePtrRestriction]
		public unsafe readonly T* Buffer;

		// Token: 0x04000007 RID: 7
		public readonly int Length;
	}
}
