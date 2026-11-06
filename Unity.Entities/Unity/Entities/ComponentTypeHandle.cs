using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x020001C8 RID: 456
	[NativeContainer]
	[NativeContainerSupportsMinMaxWriteRestriction]
	public struct ComponentTypeHandle<T>
	{
		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000AC9 RID: 2761 RVA: 0x00033DA9 File Offset: 0x00031FA9
		public readonly uint GlobalSystemVersion
		{
			get
			{
				return this.m_GlobalSystemVersion;
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000ACA RID: 2762 RVA: 0x00033DB1 File Offset: 0x00031FB1
		public readonly bool IsReadOnly
		{
			get
			{
				return this.m_IsReadOnly == 1;
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000ACB RID: 2763 RVA: 0x00033DBC File Offset: 0x00031FBC
		internal readonly bool IsZeroSized
		{
			get
			{
				return this.m_IsZeroSized == 1;
			}
		}

		// Token: 0x06000ACC RID: 2764 RVA: 0x00033DC8 File Offset: 0x00031FC8
		internal unsafe ComponentTypeHandle(bool isReadOnly, uint globalSystemVersion)
		{
			TypeIndex typeIndex = TypeManager.GetTypeIndex<T>();
			TypeManager.TypeInfo typeInfo = *TypeManager.GetTypeInfo(typeIndex);
			this.m_Length = 1;
			this.m_TypeIndex = typeIndex;
			this.m_SizeInChunk = typeInfo.SizeInChunk;
			this.m_IsZeroSized = (typeInfo.IsZeroSized ? 1 : 0);
			this.m_GlobalSystemVersion = globalSystemVersion;
			this.m_IsReadOnly = (isReadOnly ? 1 : 0);
			this.m_LookupCache = default(LookupCache);
		}

		// Token: 0x06000ACD RID: 2765 RVA: 0x00033E34 File Offset: 0x00032034
		public void Update(ref SystemState state)
		{
			this.m_GlobalSystemVersion = state.GlobalSystemVersion;
		}

		// Token: 0x06000ACE RID: 2766 RVA: 0x00033E42 File Offset: 0x00032042
		public unsafe void Update(SystemBase system)
		{
			this.Update(ref *system.m_StatePtr);
		}

		// Token: 0x06000ACF RID: 2767 RVA: 0x00033E50 File Offset: 0x00032050
		public FixedString512Bytes ToFixedString()
		{
			FixedString32Bytes fixedString32Bytes = "ComponentTypeHandle[";
			FixedString128Bytes fixedString128Bytes = new FixedString128Bytes(in fixedString32Bytes);
			FixedString128Bytes typeNameFixed = TypeManager.GetTypeNameFixed(this.m_TypeIndex);
			(ref fixedString128Bytes).Append<FixedString128Bytes, FixedString128Bytes>(in typeNameFixed);
			(ref fixedString128Bytes).Append<FixedString128Bytes>(']');
			return in fixedString128Bytes;
		}

		// Token: 0x040004D4 RID: 1236
		internal LookupCache m_LookupCache;

		// Token: 0x040004D5 RID: 1237
		internal readonly TypeIndex m_TypeIndex;

		// Token: 0x040004D6 RID: 1238
		internal readonly int m_SizeInChunk;

		// Token: 0x040004D7 RID: 1239
		internal uint m_GlobalSystemVersion;

		// Token: 0x040004D8 RID: 1240
		internal readonly byte m_IsReadOnly;

		// Token: 0x040004D9 RID: 1241
		internal readonly byte m_IsZeroSized;

		// Token: 0x040004DA RID: 1242
		private readonly int m_Length;
	}
}
