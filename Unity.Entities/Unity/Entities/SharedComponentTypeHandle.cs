using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x020001CB RID: 459
	[NativeContainer]
	[NativeContainerIsReadOnly]
	public struct SharedComponentTypeHandle<T> where T : struct, ISharedComponentData
	{
		// Token: 0x06000ADE RID: 2782 RVA: 0x00034057 File Offset: 0x00032257
		internal SharedComponentTypeHandle(bool unused)
		{
			this.m_TypeIndex = TypeManager.GetTypeIndex<T>();
		}

		// Token: 0x06000ADF RID: 2783 RVA: 0x00034064 File Offset: 0x00032264
		public unsafe void Update(SystemBase system)
		{
			this.Update(ref *system.m_StatePtr);
		}

		// Token: 0x06000AE0 RID: 2784 RVA: 0x00002287 File Offset: 0x00000487
		public void Update(ref SystemState state)
		{
		}

		// Token: 0x06000AE1 RID: 2785 RVA: 0x00034074 File Offset: 0x00032274
		public FixedString512Bytes ToFixedString()
		{
			FixedString32Bytes fixedString32Bytes = "SharedComponentTypeHandle[";
			FixedString128Bytes fixedString128Bytes = new FixedString128Bytes(in fixedString32Bytes);
			FixedString128Bytes typeNameFixed = TypeManager.GetTypeNameFixed(this.m_TypeIndex);
			(ref fixedString128Bytes).Append<FixedString128Bytes, FixedString128Bytes>(in typeNameFixed);
			(ref fixedString128Bytes).Append<FixedString128Bytes>(']');
			return in fixedString128Bytes;
		}

		// Token: 0x040004E6 RID: 1254
		internal readonly TypeIndex m_TypeIndex;
	}
}
