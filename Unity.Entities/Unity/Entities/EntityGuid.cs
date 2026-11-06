using System;

namespace Unity.Entities
{
	// Token: 0x02000073 RID: 115
	[Serializable]
	public struct EntityGuid : IComponentData, IQueryTypeParameter, IEquatable<EntityGuid>, IComparable<EntityGuid>
	{
		// Token: 0x060002B2 RID: 690 RVA: 0x0000C677 File Offset: 0x0000A877
		public EntityGuid(int originatingId, int originatingSubId, uint namespaceId, uint serial)
		{
			this.a = (ulong)(((long)originatingId & (long)((ulong)(-1))) | ((long)originatingSubId << 32));
			this.b = (ulong)serial | ((ulong)namespaceId << 32);
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060002B3 RID: 691 RVA: 0x0000C699 File Offset: 0x0000A899
		public int OriginatingId
		{
			get
			{
				return (int)(this.a & (ulong)(-1));
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060002B4 RID: 692 RVA: 0x0000C6A5 File Offset: 0x0000A8A5
		public int OriginatingSubId
		{
			get
			{
				return (int)(this.a >> 32);
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060002B5 RID: 693 RVA: 0x0000C6B1 File Offset: 0x0000A8B1
		internal uint FullNamespaceId
		{
			get
			{
				return (uint)(this.b >> 32);
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060002B6 RID: 694 RVA: 0x0000C6BD File Offset: 0x0000A8BD
		public uint Serial
		{
			get
			{
				return (uint)this.b;
			}
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0000C6C6 File Offset: 0x0000A8C6
		public static bool operator ==(in EntityGuid lhs, in EntityGuid rhs)
		{
			return lhs.a == rhs.a && lhs.b == rhs.b;
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000C6E6 File Offset: 0x0000A8E6
		public static bool operator !=(in EntityGuid lhs, in EntityGuid rhs)
		{
			return !((in lhs) == (in rhs));
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000C6F4 File Offset: 0x0000A8F4
		public override bool Equals(object obj)
		{
			if (obj is EntityGuid)
			{
				EntityGuid entityGuid = (EntityGuid)obj;
				return this.Equals(entityGuid);
			}
			return false;
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0000C6C6 File Offset: 0x0000A8C6
		public bool Equals(EntityGuid other)
		{
			return this.a == other.a && this.b == other.b;
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000C719 File Offset: 0x0000A919
		public override int GetHashCode()
		{
			return (this.a.GetHashCode() * 397) ^ this.b.GetHashCode();
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000C738 File Offset: 0x0000A938
		public int CompareTo(EntityGuid other)
		{
			if (this.a != other.a)
			{
				if (this.a <= other.a)
				{
					return -1;
				}
				return 1;
			}
			else
			{
				if (this.b == other.b)
				{
					return 0;
				}
				if (this.b <= other.b)
				{
					return -1;
				}
				return 1;
			}
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0000C788 File Offset: 0x0000A988
		public override string ToString()
		{
			return string.Format("{0}:{1}:{2:x8}:{3:x8}", new object[] { this.OriginatingId, this.OriginatingSubId, this.FullNamespaceId, this.Serial });
		}

		// Token: 0x040000B8 RID: 184
		public ulong a;

		// Token: 0x040000B9 RID: 185
		public ulong b;

		// Token: 0x040000BA RID: 186
		public static readonly EntityGuid Null;
	}
}
