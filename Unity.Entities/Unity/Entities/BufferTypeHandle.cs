using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x020001CA RID: 458
	[NativeContainer]
	[NativeContainerSupportsMinMaxWriteRestriction]
	public struct BufferTypeHandle<[global::System.Runtime.CompilerServices.IsUnmanaged] T> where T : struct, ValueType, IBufferElementData
	{
		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000AD8 RID: 2776 RVA: 0x00033FA6 File Offset: 0x000321A6
		public uint GlobalSystemVersion
		{
			get
			{
				return this.m_GlobalSystemVersion;
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000AD9 RID: 2777 RVA: 0x00033FAE File Offset: 0x000321AE
		public bool IsReadOnly
		{
			get
			{
				return this.m_IsReadOnly == 1;
			}
		}

		// Token: 0x06000ADA RID: 2778 RVA: 0x00033FB9 File Offset: 0x000321B9
		internal BufferTypeHandle(bool isReadOnly, uint globalSystemVersion)
		{
			this.m_LookupCache = default(LookupCache);
			this.m_Length = 1;
			this.m_TypeIndex = TypeManager.GetTypeIndex<T>();
			this.m_GlobalSystemVersion = globalSystemVersion;
			this.m_IsReadOnly = (isReadOnly ? 1 : 0);
		}

		// Token: 0x06000ADB RID: 2779 RVA: 0x00033FED File Offset: 0x000321ED
		public unsafe void Update(SystemBase system)
		{
			this.Update(ref *system.m_StatePtr);
		}

		// Token: 0x06000ADC RID: 2780 RVA: 0x00033FFB File Offset: 0x000321FB
		public unsafe void Update(ref SystemState state)
		{
			this.m_GlobalSystemVersion = state.m_EntityComponentStore->GlobalSystemVersion;
		}

		// Token: 0x06000ADD RID: 2781 RVA: 0x00034010 File Offset: 0x00032210
		public FixedString512Bytes ToFixedString()
		{
			FixedString32Bytes fixedString32Bytes = "BufferTypeHandle[";
			FixedString128Bytes fixedString128Bytes = new FixedString128Bytes(in fixedString32Bytes);
			FixedString128Bytes typeNameFixed = TypeManager.GetTypeNameFixed(this.m_TypeIndex);
			(ref fixedString128Bytes).Append<FixedString128Bytes, FixedString128Bytes>(in typeNameFixed);
			(ref fixedString128Bytes).Append<FixedString128Bytes>(']');
			return in fixedString128Bytes;
		}

		// Token: 0x040004E1 RID: 1249
		internal LookupCache m_LookupCache;

		// Token: 0x040004E2 RID: 1250
		internal readonly TypeIndex m_TypeIndex;

		// Token: 0x040004E3 RID: 1251
		internal uint m_GlobalSystemVersion;

		// Token: 0x040004E4 RID: 1252
		internal readonly byte m_IsReadOnly;

		// Token: 0x040004E5 RID: 1253
		private readonly int m_Length;
	}
}
