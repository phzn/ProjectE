using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Burst.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x020001C6 RID: 454
	[NativeContainer]
	public struct BufferAccessor<[global::System.Runtime.CompilerServices.IsUnmanaged] T> where T : struct, ValueType, IBufferElementData
	{
		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000AC3 RID: 2755 RVA: 0x00033C9A File Offset: 0x00031E9A
		public int Length
		{
			get
			{
				return this.m_Length;
			}
		}

		// Token: 0x06000AC4 RID: 2756 RVA: 0x00033CA2 File Offset: 0x00031EA2
		internal unsafe BufferAccessor(byte* basePointer, int length, int stride, int internalCapacity)
		{
			this.m_BasePointer = basePointer;
			this.m_Length = length;
			this.m_Stride = stride;
			this.m_InternalCapacity = internalCapacity;
		}

		// Token: 0x06000AC5 RID: 2757 RVA: 0x00033CC1 File Offset: 0x00031EC1
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private void AssertIndexInRange(int index)
		{
			if (Hint.Unlikely(index < 0 || index >= this.Length))
			{
				throw new InvalidOperationException(string.Format("index {0} out of range in LowLevelBufferAccessor of length {1}", index, this.Length));
			}
		}

		// Token: 0x170000D1 RID: 209
		public unsafe DynamicBuffer<T> this[int index]
		{
			get
			{
				BufferHeader* ptr = (BufferHeader*)(this.m_BasePointer + index * this.m_Stride);
				return new DynamicBuffer<T>(ptr, this.m_InternalCapacity);
			}
		}

		// Token: 0x06000AC7 RID: 2759 RVA: 0x00033D2C File Offset: 0x00031F2C
		public FixedString512Bytes ToFixedString()
		{
			FixedString32Bytes fixedString32Bytes = "BufferAccessor[";
			FixedString128Bytes fixedString128Bytes = new FixedString128Bytes(in fixedString32Bytes);
			FixedString128Bytes typeNameFixed = TypeManager.GetTypeNameFixed(TypeManager.GetTypeIndex<T>());
			(ref fixedString128Bytes).Append<FixedString128Bytes, FixedString128Bytes>(in typeNameFixed);
			(ref fixedString128Bytes).Append<FixedString128Bytes>(']');
			return in fixedString128Bytes;
		}

		// Token: 0x040004D0 RID: 1232
		[NativeDisableUnsafePtrRestriction]
		private unsafe byte* m_BasePointer;

		// Token: 0x040004D1 RID: 1233
		private int m_Length;

		// Token: 0x040004D2 RID: 1234
		private int m_Stride;

		// Token: 0x040004D3 RID: 1235
		private int m_InternalCapacity;
	}
}
