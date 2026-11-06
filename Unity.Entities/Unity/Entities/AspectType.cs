using System;
using Unity.Collections;

namespace Unity.Entities
{
	// Token: 0x020002E1 RID: 737
	public struct AspectType : IEquatable<AspectType>
	{
		// Token: 0x0600140C RID: 5132 RVA: 0x0004D798 File Offset: 0x0004B998
		public static AspectType FromTypeIndex(int typeIndex)
		{
			return new AspectType
			{
				TypeIndex = typeIndex
			};
		}

		// Token: 0x0600140D RID: 5133 RVA: 0x0004D7B6 File Offset: 0x0004B9B6
		public AspectType(Type type)
		{
			this.TypeIndex = TypeManager.GetAspectTypeIndex(type);
		}

		// Token: 0x0600140E RID: 5134 RVA: 0x0004D7C4 File Offset: 0x0004B9C4
		[ExcludeFromBurstCompatTesting("Returns managed type")]
		public Type GetManagedType()
		{
			return TypeManager.GetAspectType(this.TypeIndex);
		}

		// Token: 0x0600140F RID: 5135 RVA: 0x0004D7D1 File Offset: 0x0004B9D1
		[ExcludeFromBurstCompatTesting("Returns managed type")]
		public static implicit operator AspectType(Type type)
		{
			return new AspectType(type);
		}

		// Token: 0x06001410 RID: 5136 RVA: 0x0004D7D9 File Offset: 0x0004B9D9
		public static bool operator <(AspectType lhs, AspectType rhs)
		{
			return lhs.TypeIndex < rhs.TypeIndex;
		}

		// Token: 0x06001411 RID: 5137 RVA: 0x0004D7E9 File Offset: 0x0004B9E9
		public static bool operator >(AspectType lhs, AspectType rhs)
		{
			return rhs < lhs;
		}

		// Token: 0x06001412 RID: 5138 RVA: 0x0004D7F2 File Offset: 0x0004B9F2
		public static bool operator ==(AspectType lhs, AspectType rhs)
		{
			return lhs.TypeIndex == rhs.TypeIndex;
		}

		// Token: 0x06001413 RID: 5139 RVA: 0x0004D802 File Offset: 0x0004BA02
		public static bool operator !=(AspectType lhs, AspectType rhs)
		{
			return lhs.TypeIndex != rhs.TypeIndex;
		}

		// Token: 0x06001414 RID: 5140 RVA: 0x0004D815 File Offset: 0x0004BA15
		public override string ToString()
		{
			if (this.TypeIndex == 0)
			{
				return "None";
			}
			Type managedType = this.GetManagedType();
			return ((managedType != null) ? managedType.Name : null) ?? string.Empty;
		}

		// Token: 0x06001415 RID: 5141 RVA: 0x0004D7F2 File Offset: 0x0004B9F2
		public bool Equals(AspectType other)
		{
			return this.TypeIndex == other.TypeIndex;
		}

		// Token: 0x06001416 RID: 5142 RVA: 0x0004D840 File Offset: 0x0004BA40
		public override bool Equals(object obj)
		{
			if (obj is AspectType)
			{
				AspectType aspectType = (AspectType)obj;
				return this.Equals(aspectType);
			}
			return false;
		}

		// Token: 0x06001417 RID: 5143 RVA: 0x0004D867 File Offset: 0x0004BA67
		public override int GetHashCode()
		{
			return this.TypeIndex * 5813;
		}

		// Token: 0x04000758 RID: 1880
		public int TypeIndex;
	}
}
