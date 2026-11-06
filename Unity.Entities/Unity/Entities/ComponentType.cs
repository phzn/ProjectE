using System;
using Unity.Collections;

namespace Unity.Entities
{
	// Token: 0x020002F5 RID: 757
	[GenerateTestsForBurstCompatibility]
	public struct ComponentType : IEquatable<ComponentType>, IComparable<ComponentType>
	{
		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06001450 RID: 5200 RVA: 0x0004E324 File Offset: 0x0004C524
		public bool IsBuffer
		{
			get
			{
				return this.TypeIndex.IsBuffer;
			}
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06001451 RID: 5201 RVA: 0x0004E331 File Offset: 0x0004C531
		[Obsolete("IsSystemStateComponent has been renamed to IsCleanupComponent. IsSystemStateComponent will be removed in a future package release. (UnityUpgradable) -> IsCleanupComponent", false)]
		public bool IsSystemStateComponent
		{
			get
			{
				return this.TypeIndex.IsCleanupComponent;
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06001452 RID: 5202 RVA: 0x0004E331 File Offset: 0x0004C531
		public bool IsCleanupComponent
		{
			get
			{
				return this.TypeIndex.IsCleanupComponent;
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06001453 RID: 5203 RVA: 0x0004E33E File Offset: 0x0004C53E
		[Obsolete("IsSystemStateSharedComponent has been renamed to IsCleanupSharedComponent. IsSystemStateSharedComponent will be removed in a future package release. (UnityUpgradable) -> IsCleanupSharedComponent", false)]
		public bool IsSystemStateSharedComponent
		{
			get
			{
				return this.TypeIndex.IsCleanupSharedComponent;
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06001454 RID: 5204 RVA: 0x0004E33E File Offset: 0x0004C53E
		public bool IsCleanupSharedComponent
		{
			get
			{
				return this.TypeIndex.IsCleanupSharedComponent;
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06001455 RID: 5205 RVA: 0x0004E34B File Offset: 0x0004C54B
		public bool IsCleanupBufferComponent
		{
			get
			{
				return this.TypeIndex.IsCleanupBufferComponent;
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06001456 RID: 5206 RVA: 0x0004E358 File Offset: 0x0004C558
		public bool IsComponent
		{
			get
			{
				return this.TypeIndex.IsComponentType;
			}
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06001457 RID: 5207 RVA: 0x0004E365 File Offset: 0x0004C565
		public bool IsSharedComponent
		{
			get
			{
				return this.TypeIndex.IsSharedComponentType;
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06001458 RID: 5208 RVA: 0x0004E372 File Offset: 0x0004C572
		public bool IsManagedComponent
		{
			get
			{
				return this.TypeIndex.IsManagedComponent;
			}
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06001459 RID: 5209 RVA: 0x0004E37F File Offset: 0x0004C57F
		public bool IsZeroSized
		{
			get
			{
				return this.TypeIndex.IsZeroSized;
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x0600145A RID: 5210 RVA: 0x0004E38C File Offset: 0x0004C58C
		public bool IsChunkComponent
		{
			get
			{
				return this.TypeIndex.IsChunkComponent;
			}
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x0600145B RID: 5211 RVA: 0x0004E399 File Offset: 0x0004C599
		public bool IsEnableable
		{
			get
			{
				return this.TypeIndex.IsEnableable;
			}
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x0600145C RID: 5212 RVA: 0x0004E3A6 File Offset: 0x0004C5A6
		public bool HasEntityReferences
		{
			get
			{
				return this.TypeIndex.HasEntityReferences;
			}
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x0600145D RID: 5213 RVA: 0x0004E3B3 File Offset: 0x0004C5B3
		public bool HasNativeContainer
		{
			get
			{
				return this.TypeIndex.HasNativeContainer;
			}
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x0600145E RID: 5214 RVA: 0x0004E3C0 File Offset: 0x0004C5C0
		public bool IsChunkSerializable
		{
			get
			{
				return this.TypeIndex.IsChunkSerializable;
			}
		}

		// Token: 0x0600145F RID: 5215 RVA: 0x0004E3CD File Offset: 0x0004C5CD
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(Entity) })]
		public static ComponentType ReadWrite<T>()
		{
			return ComponentType.FromTypeIndex(TypeManager.GetTypeIndex<T>());
		}

		// Token: 0x06001460 RID: 5216 RVA: 0x0004E3D9 File Offset: 0x0004C5D9
		[ExcludeFromBurstCompatTesting("Takes a managed Type")]
		public static ComponentType ReadWrite(Type type)
		{
			return ComponentType.FromTypeIndex(TypeManager.GetTypeIndex(type));
		}

		// Token: 0x06001461 RID: 5217 RVA: 0x0004E3E6 File Offset: 0x0004C5E6
		public static ComponentType ReadWrite(TypeIndex typeIndex)
		{
			return ComponentType.FromTypeIndex(typeIndex);
		}

		// Token: 0x06001462 RID: 5218 RVA: 0x0004E3F0 File Offset: 0x0004C5F0
		public static ComponentType FromTypeIndex(TypeIndex typeIndex)
		{
			ComponentType componentType;
			componentType.TypeIndex = typeIndex;
			componentType.AccessModeType = ComponentType.AccessMode.ReadWrite;
			return componentType;
		}

		// Token: 0x06001463 RID: 5219 RVA: 0x0004E410 File Offset: 0x0004C610
		[ExcludeFromBurstCompatTesting("Takes a managed Type")]
		public static ComponentType ReadOnly(Type type)
		{
			ComponentType componentType = ComponentType.FromTypeIndex(TypeManager.GetTypeIndex(type));
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			return componentType;
		}

		// Token: 0x06001464 RID: 5220 RVA: 0x0004E434 File Offset: 0x0004C634
		public static ComponentType ReadOnly(TypeIndex typeIndex)
		{
			ComponentType componentType = ComponentType.FromTypeIndex(typeIndex);
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			return componentType;
		}

		// Token: 0x06001465 RID: 5221 RVA: 0x0004E454 File Offset: 0x0004C654
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(Entity) })]
		public static ComponentType ReadOnly<T>()
		{
			ComponentType componentType = ComponentType.ReadWrite<T>();
			componentType.AccessModeType = ComponentType.AccessMode.ReadOnly;
			return componentType;
		}

		// Token: 0x06001466 RID: 5222 RVA: 0x0004E470 File Offset: 0x0004C670
		[ExcludeFromBurstCompatTesting("Takes a managed Type")]
		public static ComponentType ChunkComponent(Type type)
		{
			return ComponentType.FromTypeIndex(TypeManager.MakeChunkComponentTypeIndex(TypeManager.GetTypeIndex(type)));
		}

		// Token: 0x06001467 RID: 5223 RVA: 0x0004E482 File Offset: 0x0004C682
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(Entity) })]
		public static ComponentType ChunkComponent<T>()
		{
			return ComponentType.FromTypeIndex(TypeManager.MakeChunkComponentTypeIndex(TypeManager.GetTypeIndex<T>()));
		}

		// Token: 0x06001468 RID: 5224 RVA: 0x0004E493 File Offset: 0x0004C693
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(Entity) })]
		public static ComponentType ChunkComponentReadOnly<T>()
		{
			return ComponentType.ReadOnly(TypeManager.MakeChunkComponentTypeIndex(TypeManager.GetTypeIndex<T>()));
		}

		// Token: 0x06001469 RID: 5225 RVA: 0x0004E4A4 File Offset: 0x0004C6A4
		[ExcludeFromBurstCompatTesting("Takes a managed Type")]
		public static ComponentType ChunkComponentReadOnly(Type type)
		{
			return ComponentType.ReadOnly(TypeManager.MakeChunkComponentTypeIndex(TypeManager.GetTypeIndex(type)));
		}

		// Token: 0x0600146A RID: 5226 RVA: 0x0004E4B6 File Offset: 0x0004C6B6
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(Entity) })]
		public static ComponentType ChunkComponentExclude<T>()
		{
			return ComponentType.Exclude(TypeManager.MakeChunkComponentTypeIndex(TypeManager.GetTypeIndex<T>()));
		}

		// Token: 0x0600146B RID: 5227 RVA: 0x0004E4C7 File Offset: 0x0004C6C7
		[ExcludeFromBurstCompatTesting("Takes a managed Type")]
		public static ComponentType ChunkComponentExclude(Type type)
		{
			return ComponentType.Exclude(TypeManager.MakeChunkComponentTypeIndex(TypeManager.GetTypeIndex(type)));
		}

		// Token: 0x0600146C RID: 5228 RVA: 0x0004E4D9 File Offset: 0x0004C6D9
		[ExcludeFromBurstCompatTesting("Takes a managed Type")]
		public static ComponentType Exclude(Type type)
		{
			return ComponentType.Exclude(TypeManager.GetTypeIndex(type));
		}

		// Token: 0x0600146D RID: 5229 RVA: 0x0004E4E8 File Offset: 0x0004C6E8
		public static ComponentType Exclude(TypeIndex typeIndex)
		{
			ComponentType componentType = ComponentType.FromTypeIndex(typeIndex);
			componentType.AccessModeType = ComponentType.AccessMode.Exclude;
			return componentType;
		}

		// Token: 0x0600146E RID: 5230 RVA: 0x0004E505 File Offset: 0x0004C705
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(Entity) })]
		public static ComponentType Exclude<T>()
		{
			return ComponentType.Exclude(TypeManager.GetTypeIndex<T>());
		}

		// Token: 0x0600146F RID: 5231 RVA: 0x0004E511 File Offset: 0x0004C711
		[ExcludeFromBurstCompatTesting("Takes a managed Type")]
		public ComponentType(Type type, ComponentType.AccessMode accessModeType = ComponentType.AccessMode.ReadWrite)
		{
			this.TypeIndex = TypeManager.GetTypeIndex(type);
			this.AccessModeType = accessModeType;
		}

		// Token: 0x06001470 RID: 5232 RVA: 0x0004E526 File Offset: 0x0004C726
		[ExcludeFromBurstCompatTesting("Returns a managed Type")]
		public Type GetManagedType()
		{
			return TypeManager.GetType(this.TypeIndex);
		}

		// Token: 0x06001471 RID: 5233 RVA: 0x0004E533 File Offset: 0x0004C733
		[ExcludeFromBurstCompatTesting("Takes a managed Type")]
		public static implicit operator ComponentType(Type type)
		{
			return new ComponentType(type, ComponentType.AccessMode.ReadWrite);
		}

		// Token: 0x06001472 RID: 5234 RVA: 0x0004E53C File Offset: 0x0004C73C
		public static bool operator <(ComponentType lhs, ComponentType rhs)
		{
			if (lhs.TypeIndex == rhs.TypeIndex)
			{
				return lhs.AccessModeType < rhs.AccessModeType;
			}
			return lhs.TypeIndex < rhs.TypeIndex;
		}

		// Token: 0x06001473 RID: 5235 RVA: 0x0004E571 File Offset: 0x0004C771
		public static bool operator >(ComponentType lhs, ComponentType rhs)
		{
			return rhs < lhs;
		}

		// Token: 0x06001474 RID: 5236 RVA: 0x0004E57A File Offset: 0x0004C77A
		public static bool operator ==(ComponentType lhs, ComponentType rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x06001475 RID: 5237 RVA: 0x0004E584 File Offset: 0x0004C784
		public static bool operator !=(ComponentType lhs, ComponentType rhs)
		{
			return !lhs.Equals(rhs);
		}

		// Token: 0x06001476 RID: 5238 RVA: 0x0004E594 File Offset: 0x0004C794
		internal unsafe static bool CompareArray(ComponentType* type1, int typeCount1, ComponentType* type2, int typeCount2)
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

		// Token: 0x06001477 RID: 5239 RVA: 0x0004E5E0 File Offset: 0x0004C7E0
		[ExcludeFromBurstCompatTesting("Takes a managed array")]
		public unsafe static ComponentType[] Combine(params ComponentType[][] componentTypes)
		{
			int num = 0;
			for (int num2 = 0; num2 != componentTypes.Length; num2++)
			{
				num += componentTypes[num2].Length;
			}
			ComponentType[] array = new ComponentType[num];
			int num3 = 0;
			for (int num4 = 0; num4 != componentTypes.Length; num4++)
			{
				for (int i = 0; i < componentTypes[num4].Length; i++)
				{
					array[num3 + i] = componentTypes[num4][i];
				}
				num3 += componentTypes[num4].Length;
			}
			ComponentType[] array2;
			ComponentType* ptr;
			if ((array2 = array) == null || array2.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &array2[0];
			}
			NativeSortExtension.Sort<ComponentType>(ptr, array.Length);
			array2 = null;
			if (num != 0)
			{
				num3 = 0;
				for (int j = 1; j < num; j++)
				{
					if (array[num3].TypeIndex != array[j].TypeIndex)
					{
						num3++;
						array[num3] = array[j];
					}
					else if (array[j].AccessModeType == ComponentType.AccessMode.Exclude && array[num3].AccessModeType != ComponentType.AccessMode.Exclude)
					{
						throw new ArgumentException(string.Format("ComponentType.Combine {0} and {1} are conflicting.", array[num3], array[j]));
					}
				}
				num3++;
				if (num3 != num)
				{
					Array.Resize<ComponentType>(ref array, num3);
				}
			}
			return array;
		}

		// Token: 0x06001478 RID: 5240 RVA: 0x0004E72C File Offset: 0x0004C92C
		[ExcludeFromBurstCompatTesting("Returns managed string")]
		public override string ToString()
		{
			return this.ToFixedString().ToString();
		}

		// Token: 0x06001479 RID: 5241 RVA: 0x0004E750 File Offset: 0x0004C950
		[GenerateTestsForBurstCompatibility]
		public FixedString128Bytes ToFixedString()
		{
			if (this.TypeIndex == TypeIndex.Null)
			{
				return in ComponentType.kMsg_None;
			}
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			NativeText.ReadOnly debugTypeName = TypeManager.GetTypeInfo(this.TypeIndex).DebugTypeName;
			(ref fixedString128Bytes).Append<FixedString128Bytes, NativeText.ReadOnly>(in debugTypeName);
			if (this.IsBuffer)
			{
				(ref fixedString128Bytes).Append<FixedString128Bytes, FixedString32Bytes>(in ComponentType.kMsg_Space_Buffer);
			}
			if (this.AccessModeType == ComponentType.AccessMode.Exclude)
			{
				(ref fixedString128Bytes).Append<FixedString128Bytes, FixedString32Bytes>(in ComponentType.kMsg_Space_Exclude);
			}
			if (this.AccessModeType == ComponentType.AccessMode.ReadOnly)
			{
				(ref fixedString128Bytes).Append<FixedString128Bytes, FixedString32Bytes>(in ComponentType.kMsg_Space_Readonly);
			}
			return fixedString128Bytes;
		}

		// Token: 0x0600147A RID: 5242 RVA: 0x0004E7DF File Offset: 0x0004C9DF
		public bool Equals(ComponentType other)
		{
			return this.TypeIndex == other.TypeIndex && this.AccessModeType == other.AccessModeType;
		}

		// Token: 0x0600147B RID: 5243 RVA: 0x0004E804 File Offset: 0x0004CA04
		[GenerateTestsForBurstCompatibility]
		public int CompareTo(ComponentType other)
		{
			if (this.TypeIndex != other.TypeIndex)
			{
				return this.TypeIndex.CompareTo(other.TypeIndex);
			}
			int accessModeType = (int)this.AccessModeType;
			return accessModeType.CompareTo((int)other.AccessModeType);
		}

		// Token: 0x0600147C RID: 5244 RVA: 0x0004E84C File Offset: 0x0004CA4C
		[ExcludeFromBurstCompatTesting("Takes managed object")]
		public override bool Equals(object obj)
		{
			if (obj is ComponentType)
			{
				ComponentType componentType = (ComponentType)obj;
				return componentType == this;
			}
			return false;
		}

		// Token: 0x0600147D RID: 5245 RVA: 0x0004E876 File Offset: 0x0004CA76
		public override int GetHashCode()
		{
			return (this.TypeIndex.GetHashCode() * 397) ^ (int)this.AccessModeType;
		}

		// Token: 0x0400077F RID: 1919
		public TypeIndex TypeIndex;

		// Token: 0x04000780 RID: 1920
		public ComponentType.AccessMode AccessModeType;

		// Token: 0x04000781 RID: 1921
		private static readonly FixedString32Bytes kMsg_None = "None";

		// Token: 0x04000782 RID: 1922
		private static readonly FixedString32Bytes kMsg_Space_Buffer = " [Buffer]";

		// Token: 0x04000783 RID: 1923
		private static readonly FixedString32Bytes kMsg_Space_Exclude = " [Exclude]";

		// Token: 0x04000784 RID: 1924
		private static readonly FixedString32Bytes kMsg_Space_Readonly = " [ReadOnly]";

		// Token: 0x020002F6 RID: 758
		public enum AccessMode
		{
			// Token: 0x04000786 RID: 1926
			ReadWrite,
			// Token: 0x04000787 RID: 1927
			ReadOnly,
			// Token: 0x04000788 RID: 1928
			Exclude
		}
	}
}
