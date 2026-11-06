using System;

namespace Unity.Entities
{
	// Token: 0x0200007F RID: 127
	public struct ComponentTypeHash : IEquatable<ComponentTypeHash>
	{
		// Token: 0x060002C6 RID: 710 RVA: 0x0000C879 File Offset: 0x0000AA79
		public bool Equals(ComponentTypeHash other)
		{
			return this.StableTypeHash == other.StableTypeHash && this.Flags == other.Flags;
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0000C89C File Offset: 0x0000AA9C
		public override bool Equals(object obj)
		{
			if (obj is ComponentTypeHash)
			{
				ComponentTypeHash componentTypeHash = (ComponentTypeHash)obj;
				return this.Equals(componentTypeHash);
			}
			return false;
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0000C8C1 File Offset: 0x0000AAC1
		public static bool operator ==(ComponentTypeHash left, ComponentTypeHash right)
		{
			return left.Equals(right);
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x0000C8CB File Offset: 0x0000AACB
		public static bool operator !=(ComponentTypeHash left, ComponentTypeHash right)
		{
			return !left.Equals(right);
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0000C8D8 File Offset: 0x0000AAD8
		public override int GetHashCode()
		{
			return (this.StableTypeHash.GetHashCode() * 397) ^ (int)this.Flags;
		}

		// Token: 0x040000D9 RID: 217
		public ulong StableTypeHash;

		// Token: 0x040000DA RID: 218
		public ComponentTypeFlags Flags;
	}
}
