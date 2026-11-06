using System;

namespace Unity.Entities
{
	// Token: 0x020002F7 RID: 759
	internal struct ComponentTypeInArchetype : IEquatable<ComponentTypeInArchetype>
	{
		// Token: 0x17000186 RID: 390
		// (get) Token: 0x0600147F RID: 5247 RVA: 0x0004E8D4 File Offset: 0x0004CAD4
		public bool IsBuffer
		{
			get
			{
				return this.TypeIndex.IsBuffer;
			}
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x06001480 RID: 5248 RVA: 0x0004E8F0 File Offset: 0x0004CAF0
		public bool IsCleanupComponent
		{
			get
			{
				return this.TypeIndex.IsCleanupComponent;
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x06001481 RID: 5249 RVA: 0x0004E90C File Offset: 0x0004CB0C
		public bool IsCleanupSharedComponent
		{
			get
			{
				return this.TypeIndex.IsCleanupSharedComponent;
			}
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x06001482 RID: 5250 RVA: 0x0004E928 File Offset: 0x0004CB28
		public bool IsSharedComponent
		{
			get
			{
				return this.TypeIndex.IsSharedComponentType;
			}
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x06001483 RID: 5251 RVA: 0x0004E944 File Offset: 0x0004CB44
		public bool IsZeroSized
		{
			get
			{
				return this.TypeIndex.IsZeroSized;
			}
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06001484 RID: 5252 RVA: 0x0004E960 File Offset: 0x0004CB60
		public bool IsChunkComponent
		{
			get
			{
				return this.TypeIndex.IsChunkComponent;
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06001485 RID: 5253 RVA: 0x0004E97C File Offset: 0x0004CB7C
		public bool HasEntityReferences
		{
			get
			{
				return this.TypeIndex.HasEntityReferences;
			}
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x06001486 RID: 5254 RVA: 0x0004E998 File Offset: 0x0004CB98
		public bool IsEnableable
		{
			get
			{
				return this.TypeIndex.IsEnableable;
			}
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x06001487 RID: 5255 RVA: 0x0004E9B4 File Offset: 0x0004CBB4
		public bool IsManagedComponent
		{
			get
			{
				return this.TypeIndex.IsManagedComponent;
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x06001488 RID: 5256 RVA: 0x0004E9D0 File Offset: 0x0004CBD0
		public bool IsBakeOnlyType
		{
			get
			{
				return this.TypeIndex.IsBakingOnlyType;
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06001489 RID: 5257 RVA: 0x0004E9EC File Offset: 0x0004CBEC
		public bool IsChunkSerializable
		{
			get
			{
				return this.TypeIndex.IsChunkSerializable;
			}
		}

		// Token: 0x0600148A RID: 5258 RVA: 0x0004EA07 File Offset: 0x0004CC07
		public ComponentTypeInArchetype(ComponentType type)
		{
			this.TypeIndex = type.TypeIndex;
		}

		// Token: 0x0600148B RID: 5259 RVA: 0x0004EA15 File Offset: 0x0004CC15
		public ComponentTypeInArchetype(TypeIndex typeIndex)
		{
			this.TypeIndex = typeIndex;
		}

		// Token: 0x0600148C RID: 5260 RVA: 0x0004EA1E File Offset: 0x0004CC1E
		public static bool operator ==(ComponentTypeInArchetype lhs, ComponentTypeInArchetype rhs)
		{
			return lhs.TypeIndex == rhs.TypeIndex;
		}

		// Token: 0x0600148D RID: 5261 RVA: 0x0004EA31 File Offset: 0x0004CC31
		public static bool operator !=(ComponentTypeInArchetype lhs, ComponentTypeInArchetype rhs)
		{
			return lhs.TypeIndex != rhs.TypeIndex;
		}

		// Token: 0x0600148E RID: 5262 RVA: 0x0004EA44 File Offset: 0x0004CC44
		public static bool operator <(ComponentTypeInArchetype lhs, ComponentTypeInArchetype rhs)
		{
			return lhs.TypeIndex < rhs.TypeIndex;
		}

		// Token: 0x0600148F RID: 5263 RVA: 0x0004EA57 File Offset: 0x0004CC57
		public static bool operator >(ComponentTypeInArchetype lhs, ComponentTypeInArchetype rhs)
		{
			return lhs.TypeIndex > rhs.TypeIndex;
		}

		// Token: 0x06001490 RID: 5264 RVA: 0x0004EA6A File Offset: 0x0004CC6A
		public static bool operator <=(ComponentTypeInArchetype lhs, ComponentTypeInArchetype rhs)
		{
			return !(lhs > rhs);
		}

		// Token: 0x06001491 RID: 5265 RVA: 0x0004EA76 File Offset: 0x0004CC76
		public static bool operator >=(ComponentTypeInArchetype lhs, ComponentTypeInArchetype rhs)
		{
			return !(lhs < rhs);
		}

		// Token: 0x06001492 RID: 5266 RVA: 0x0004EA84 File Offset: 0x0004CC84
		public unsafe static bool CompareArray(ComponentTypeInArchetype* type1, int typeCount1, ComponentTypeInArchetype* type2, int typeCount2)
		{
			if (typeCount1 != typeCount2)
			{
				return false;
			}
			for (int i = 0; i < typeCount1; i++)
			{
				if (type1[i] != type2[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001493 RID: 5267 RVA: 0x0004EAD0 File Offset: 0x0004CCD0
		public ComponentType ToComponentType()
		{
			ComponentType componentType;
			componentType.TypeIndex = this.TypeIndex;
			componentType.AccessModeType = ComponentType.AccessMode.ReadWrite;
			return componentType;
		}

		// Token: 0x06001494 RID: 5268 RVA: 0x0004EAF3 File Offset: 0x0004CCF3
		public override bool Equals(object obj)
		{
			return obj is ComponentTypeInArchetype && (ComponentTypeInArchetype)obj == this;
		}

		// Token: 0x06001495 RID: 5269 RVA: 0x0004EB10 File Offset: 0x0004CD10
		public override int GetHashCode()
		{
			return this.TypeIndex.GetHashCode();
		}

		// Token: 0x06001496 RID: 5270 RVA: 0x0004EA1E File Offset: 0x0004CC1E
		public bool Equals(ComponentTypeInArchetype other)
		{
			return this.TypeIndex == other.TypeIndex;
		}

		// Token: 0x04000789 RID: 1929
		public readonly TypeIndex TypeIndex;
	}
}
