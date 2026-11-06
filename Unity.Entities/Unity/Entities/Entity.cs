using System;
using System.Diagnostics;
using Unity.Collections;

namespace Unity.Entities
{
	// Token: 0x020002FB RID: 763
	[DebuggerTypeProxy(typeof(EntityDebugProxy))]
	[DebuggerDisplay("{EntityDebugProxy.GetDebugName(Index, Version)}")]
	public struct Entity : IEquatable<Entity>, IComparable<Entity>
	{
		// Token: 0x060014AB RID: 5291 RVA: 0x0004EFFE File Offset: 0x0004D1FE
		public static bool operator ==(Entity lhs, Entity rhs)
		{
			return lhs.Index == rhs.Index && lhs.Version == rhs.Version;
		}

		// Token: 0x060014AC RID: 5292 RVA: 0x0004F01E File Offset: 0x0004D21E
		public static bool operator !=(Entity lhs, Entity rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x060014AD RID: 5293 RVA: 0x0004F02A File Offset: 0x0004D22A
		public int CompareTo(Entity other)
		{
			return this.Index - other.Index;
		}

		// Token: 0x060014AE RID: 5294 RVA: 0x0004F03C File Offset: 0x0004D23C
		public override bool Equals(object compare)
		{
			if (compare is Entity)
			{
				Entity entity = (Entity)compare;
				return this.Equals(entity);
			}
			return false;
		}

		// Token: 0x060014AF RID: 5295 RVA: 0x0004F061 File Offset: 0x0004D261
		public override int GetHashCode()
		{
			return this.Index;
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x060014B0 RID: 5296 RVA: 0x0004F06C File Offset: 0x0004D26C
		public static Entity Null
		{
			get
			{
				return default(Entity);
			}
		}

		// Token: 0x060014B1 RID: 5297 RVA: 0x0004F082 File Offset: 0x0004D282
		public bool Equals(Entity entity)
		{
			return entity.Index == this.Index && entity.Version == this.Version;
		}

		// Token: 0x060014B2 RID: 5298 RVA: 0x0004F0A2 File Offset: 0x0004D2A2
		public override string ToString()
		{
			if (!this.Equals(Entity.Null))
			{
				return string.Format("Entity({0}:{1})", this.Index, this.Version);
			}
			return "Entity.Null";
		}

		// Token: 0x060014B3 RID: 5299 RVA: 0x0004F0D8 File Offset: 0x0004D2D8
		[GenerateTestsForBurstCompatibility]
		public FixedString64Bytes ToFixedString()
		{
			if (this.Equals(Entity.Null))
			{
				return "Entity.Null";
			}
			FixedString64Bytes fixedString64Bytes = default(FixedString64Bytes);
			FixedString32Bytes fixedString32Bytes = "Entity(";
			(ref fixedString64Bytes).Append<FixedString64Bytes, FixedString32Bytes>(in fixedString32Bytes);
			(ref fixedString64Bytes).Append<FixedString64Bytes>(this.Index);
			(ref fixedString64Bytes).Append<FixedString64Bytes>(':');
			(ref fixedString64Bytes).Append<FixedString64Bytes>(this.Version);
			(ref fixedString64Bytes).Append<FixedString64Bytes>(')');
			return fixedString64Bytes;
		}

		// Token: 0x04000790 RID: 1936
		public int Index;

		// Token: 0x04000791 RID: 1937
		public int Version;
	}
}
