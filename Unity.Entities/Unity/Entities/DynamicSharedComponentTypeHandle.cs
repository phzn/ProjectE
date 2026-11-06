using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x020001CC RID: 460
	[NativeContainer]
	[NativeContainerIsReadOnly]
	public struct DynamicSharedComponentTypeHandle
	{
		// Token: 0x06000AE2 RID: 2786 RVA: 0x000340BB File Offset: 0x000322BB
		internal DynamicSharedComponentTypeHandle(ComponentType componentType)
		{
			this.m_TypeIndex = componentType.TypeIndex;
			this.m_cachedTypeIndexinArchetype = -1;
		}

		// Token: 0x06000AE3 RID: 2787 RVA: 0x000340D0 File Offset: 0x000322D0
		public unsafe void Update(SystemBase system)
		{
			this.Update(ref *system.m_StatePtr);
		}

		// Token: 0x06000AE4 RID: 2788 RVA: 0x00002287 File Offset: 0x00000487
		public void Update(ref SystemState state)
		{
		}

		// Token: 0x06000AE5 RID: 2789 RVA: 0x000340E0 File Offset: 0x000322E0
		public FixedString512Bytes ToFixedString()
		{
			FixedString32Bytes fixedString32Bytes = "DynamicSharedComponentTypeHandle[";
			FixedString128Bytes fixedString128Bytes = new FixedString128Bytes(in fixedString32Bytes);
			FixedString128Bytes typeNameFixed = TypeManager.GetTypeNameFixed(this.m_TypeIndex);
			(ref fixedString128Bytes).Append<FixedString128Bytes, FixedString128Bytes>(in typeNameFixed);
			(ref fixedString128Bytes).Append<FixedString128Bytes>(']');
			return in fixedString128Bytes;
		}

		// Token: 0x040004E7 RID: 1255
		internal readonly TypeIndex m_TypeIndex;

		// Token: 0x040004E8 RID: 1256
		internal short m_cachedTypeIndexinArchetype;
	}
}
