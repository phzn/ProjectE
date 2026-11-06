using System;
using System.Diagnostics;
using Unity.Burst.CompilerServices;
using Unity.Collections;
using Unity.Mathematics;

namespace Unity.Entities
{
	// Token: 0x020002F9 RID: 761
	[GenerateTestsForBurstCompatibility]
	[DebuggerTypeProxy(typeof(ComponentTypeSetDebugView))]
	public readonly struct ComponentTypeSet
	{
		// Token: 0x17000191 RID: 401
		// (get) Token: 0x06001497 RID: 5271 RVA: 0x0004EB31 File Offset: 0x0004CD31
		public int Length
		{
			get
			{
				return this._sorted.Length;
			}
		}

		// Token: 0x06001498 RID: 5272 RVA: 0x0004EB3E File Offset: 0x0004CD3E
		public TypeIndex GetTypeIndex(int index)
		{
			return this._sorted[index];
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x06001499 RID: 5273 RVA: 0x0004EB4C File Offset: 0x0004CD4C
		internal unsafe TypeIndex* UnsafeTypesPtrRO
		{
			get
			{
				return (TypeIndex*)this._sorted.Buffer;
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x0600149A RID: 5274 RVA: 0x0004EB5C File Offset: 0x0004CD5C
		internal int ChunkComponentCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this._sorted.Length; i++)
				{
					if (this.GetComponentType(i).IsChunkComponent)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x0600149B RID: 5275 RVA: 0x0004EB97 File Offset: 0x0004CD97
		public ComponentType GetComponentType(int index)
		{
			return ComponentType.FromTypeIndex(this._sorted[index]);
		}

		// Token: 0x0600149C RID: 5276 RVA: 0x0004EBAA File Offset: 0x0004CDAA
		public ComponentTypeSet(ComponentType a)
		{
			this._sorted = default(FixedList64Bytes<TypeIndex>);
			this._sorted.Add(in a.TypeIndex);
			this.m_masks = new ComponentTypeSet.Masks(in this._sorted);
		}

		// Token: 0x0600149D RID: 5277 RVA: 0x0004EBDC File Offset: 0x0004CDDC
		public ComponentTypeSet(ComponentType a, ComponentType b)
		{
			this._sorted = default(FixedList64Bytes<TypeIndex>);
			this._sorted.Add(in a.TypeIndex);
			this._sorted.Add(in b.TypeIndex);
			(ref this._sorted).Sort<TypeIndex>();
			this.m_masks = new ComponentTypeSet.Masks(in this._sorted);
		}

		// Token: 0x0600149E RID: 5278 RVA: 0x0004EC38 File Offset: 0x0004CE38
		public ComponentTypeSet(ComponentType a, ComponentType b, ComponentType c)
		{
			this._sorted = default(FixedList64Bytes<TypeIndex>);
			this._sorted.Add(in a.TypeIndex);
			this._sorted.Add(in b.TypeIndex);
			this._sorted.Add(in c.TypeIndex);
			(ref this._sorted).Sort<TypeIndex>();
			this.m_masks = new ComponentTypeSet.Masks(in this._sorted);
		}

		// Token: 0x0600149F RID: 5279 RVA: 0x0004ECA4 File Offset: 0x0004CEA4
		public ComponentTypeSet(ComponentType a, ComponentType b, ComponentType c, ComponentType d)
		{
			this._sorted = default(FixedList64Bytes<TypeIndex>);
			this._sorted.Add(in a.TypeIndex);
			this._sorted.Add(in b.TypeIndex);
			this._sorted.Add(in c.TypeIndex);
			this._sorted.Add(in d.TypeIndex);
			(ref this._sorted).Sort<TypeIndex>();
			this.m_masks = new ComponentTypeSet.Masks(in this._sorted);
		}

		// Token: 0x060014A0 RID: 5280 RVA: 0x0004ED24 File Offset: 0x0004CF24
		public ComponentTypeSet(ComponentType a, ComponentType b, ComponentType c, ComponentType d, ComponentType e)
		{
			this._sorted = default(FixedList64Bytes<TypeIndex>);
			this._sorted.Add(in a.TypeIndex);
			this._sorted.Add(in b.TypeIndex);
			this._sorted.Add(in c.TypeIndex);
			this._sorted.Add(in d.TypeIndex);
			this._sorted.Add(in e.TypeIndex);
			(ref this._sorted).Sort<TypeIndex>();
			this.m_masks = new ComponentTypeSet.Masks(in this._sorted);
		}

		// Token: 0x060014A1 RID: 5281 RVA: 0x0004EDB4 File Offset: 0x0004CFB4
		[ExcludeFromBurstCompatTesting("Takes managed array")]
		public ComponentTypeSet(ComponentType[] types)
		{
			this._sorted = default(FixedList64Bytes<TypeIndex>);
			for (int i = 0; i < types.Length; i++)
			{
				this._sorted.Add(in types[i].TypeIndex);
			}
			(ref this._sorted).Sort<TypeIndex>();
			this.m_masks = new ComponentTypeSet.Masks(in this._sorted);
		}

		// Token: 0x060014A2 RID: 5282 RVA: 0x0004EE10 File Offset: 0x0004D010
		public ComponentTypeSet(in FixedList128Bytes<ComponentType> types)
		{
			this._sorted = default(FixedList64Bytes<TypeIndex>);
			for (int i = 0; i < types.Length; i++)
			{
				this._sorted.Add(in types[i].TypeIndex);
			}
			(ref this._sorted).Sort<TypeIndex>();
			this.m_masks = new ComponentTypeSet.Masks(in this._sorted);
		}

		// Token: 0x060014A3 RID: 5283 RVA: 0x0004EE70 File Offset: 0x0004D070
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private void CheckForDuplicates()
		{
			if (Hint.Unlikely(this._sorted.IsEmpty))
			{
				return;
			}
			TypeIndex typeIndex = this._sorted[0];
			for (int i = 1; i < this._sorted.Length; i++)
			{
				TypeIndex typeIndex2 = this._sorted[i];
				if (Hint.Unlikely(typeIndex == typeIndex2))
				{
					throw new ArgumentException("ComponentTypes cannot contain duplicate types. Remove all but one occurrence of \"" + this.GetComponentType(i).ToString() + "\"");
				}
				typeIndex = typeIndex2;
			}
		}

		// Token: 0x0400078A RID: 1930
		private readonly FixedList64Bytes<TypeIndex> _sorted;

		// Token: 0x0400078B RID: 1931
		internal readonly ComponentTypeSet.Masks m_masks;

		// Token: 0x020002FA RID: 762
		internal readonly struct Masks
		{
			// Token: 0x060014A4 RID: 5284 RVA: 0x0004EEFC File Offset: 0x0004D0FC
			public Masks(in FixedList64Bytes<TypeIndex> sortedTypeIndices)
			{
				this.BufferMask = 0;
				this.CleanupComponentMask = 0;
				this.SharedComponentMask = 0;
				this.ZeroSizedMask = 0;
				for (int i = 0; i < sortedTypeIndices.Length; i++)
				{
					TypeIndex typeIndex = sortedTypeIndices[i];
					ushort num = (ushort)(1 << i);
					if (typeIndex.IsBuffer)
					{
						this.BufferMask |= num;
					}
					if (typeIndex.IsCleanupComponent)
					{
						this.CleanupComponentMask |= num;
					}
					if (typeIndex.IsSharedComponentType)
					{
						this.SharedComponentMask |= num;
					}
					if (typeIndex.IsZeroSized)
					{
						this.ZeroSizedMask |= num;
					}
				}
			}

			// Token: 0x060014A5 RID: 5285 RVA: 0x0004EFA6 File Offset: 0x0004D1A6
			public bool IsSharedComponent(int index)
			{
				return ((int)this.SharedComponentMask & (1 << index)) != 0;
			}

			// Token: 0x060014A6 RID: 5286 RVA: 0x0004EFB8 File Offset: 0x0004D1B8
			public bool IsZeroSized(int index)
			{
				return ((int)this.ZeroSizedMask & (1 << index)) != 0;
			}

			// Token: 0x17000194 RID: 404
			// (get) Token: 0x060014A7 RID: 5287 RVA: 0x0004EFCA File Offset: 0x0004D1CA
			public int Buffers
			{
				get
				{
					return math.countbits((uint)this.BufferMask);
				}
			}

			// Token: 0x17000195 RID: 405
			// (get) Token: 0x060014A8 RID: 5288 RVA: 0x0004EFD7 File Offset: 0x0004D1D7
			public int CleanupComponents
			{
				get
				{
					return math.countbits((uint)this.CleanupComponentMask);
				}
			}

			// Token: 0x17000196 RID: 406
			// (get) Token: 0x060014A9 RID: 5289 RVA: 0x0004EFE4 File Offset: 0x0004D1E4
			public int SharedComponents
			{
				get
				{
					return math.countbits((uint)this.SharedComponentMask);
				}
			}

			// Token: 0x17000197 RID: 407
			// (get) Token: 0x060014AA RID: 5290 RVA: 0x0004EFF1 File Offset: 0x0004D1F1
			public int ZeroSizeds
			{
				get
				{
					return math.countbits((uint)this.ZeroSizedMask);
				}
			}

			// Token: 0x0400078C RID: 1932
			public readonly ushort BufferMask;

			// Token: 0x0400078D RID: 1933
			public readonly ushort CleanupComponentMask;

			// Token: 0x0400078E RID: 1934
			public readonly ushort SharedComponentMask;

			// Token: 0x0400078F RID: 1935
			public readonly ushort ZeroSizedMask;
		}
	}
}
