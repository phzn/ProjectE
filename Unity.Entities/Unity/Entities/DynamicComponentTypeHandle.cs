using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x020001C9 RID: 457
	[NativeContainer]
	[NativeContainerSupportsMinMaxWriteRestriction]
	public struct DynamicComponentTypeHandle
	{
		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000AD0 RID: 2768 RVA: 0x00033E97 File Offset: 0x00032097
		public readonly uint GlobalSystemVersion
		{
			get
			{
				return this.m_GlobalSystemVersion;
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000AD1 RID: 2769 RVA: 0x00033E9F File Offset: 0x0003209F
		public readonly bool IsReadOnly
		{
			get
			{
				return this.m_IsReadOnly == 1;
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000AD2 RID: 2770 RVA: 0x00033EAA File Offset: 0x000320AA
		internal readonly bool IsZeroSized
		{
			get
			{
				return this.m_IsZeroSized == 1;
			}
		}

		// Token: 0x06000AD3 RID: 2771 RVA: 0x00033EB8 File Offset: 0x000320B8
		internal DynamicComponentTypeHandle(ComponentType componentType, uint globalSystemVersion)
		{
			this.m_Length = 1;
			this.m_TypeIndex = componentType.TypeIndex;
			this.m_IsZeroSized = (TypeManager.GetTypeInfo(this.m_TypeIndex).IsZeroSized ? 1 : 0);
			this.m_GlobalSystemVersion = globalSystemVersion;
			this.m_IsReadOnly = ((componentType.AccessModeType == ComponentType.AccessMode.ReadOnly) ? 1 : 0);
			this.m_TypeLookupCache = 0;
		}

		// Token: 0x06000AD4 RID: 2772 RVA: 0x00033F15 File Offset: 0x00032115
		public unsafe void Update(SystemBase system)
		{
			this.Update(ref *system.m_StatePtr);
		}

		// Token: 0x06000AD5 RID: 2773 RVA: 0x00033F23 File Offset: 0x00032123
		public void Update(ref SystemState state)
		{
			this.m_GlobalSystemVersion = state.GlobalSystemVersion;
		}

		// Token: 0x06000AD6 RID: 2774 RVA: 0x00033F34 File Offset: 0x00032134
		public FixedString512Bytes ToFixedString()
		{
			FixedString32Bytes fixedString32Bytes = "DynamicComponentTypeHandle[";
			FixedString128Bytes fixedString128Bytes = new FixedString128Bytes(in fixedString32Bytes);
			FixedString128Bytes typeNameFixed = TypeManager.GetTypeNameFixed(this.m_TypeIndex);
			(ref fixedString128Bytes).Append<FixedString128Bytes, FixedString128Bytes>(in typeNameFixed);
			(ref fixedString128Bytes).Append<FixedString128Bytes>(']');
			return in fixedString128Bytes;
		}

		// Token: 0x06000AD7 RID: 2775 RVA: 0x00033F7B File Offset: 0x0003217B
		public DynamicComponentTypeHandle CopyToReadOnly()
		{
			if (this.IsReadOnly)
			{
				throw new InvalidOperationException("Already RO!");
			}
			return new DynamicComponentTypeHandle(ComponentType.ReadOnly(this.m_TypeIndex), this.GlobalSystemVersion);
		}

		// Token: 0x040004DB RID: 1243
		internal readonly TypeIndex m_TypeIndex;

		// Token: 0x040004DC RID: 1244
		internal uint m_GlobalSystemVersion;

		// Token: 0x040004DD RID: 1245
		internal readonly byte m_IsReadOnly;

		// Token: 0x040004DE RID: 1246
		internal readonly byte m_IsZeroSized;

		// Token: 0x040004DF RID: 1247
		internal short m_TypeLookupCache;

		// Token: 0x040004E0 RID: 1248
		private readonly int m_Length;
	}
}
