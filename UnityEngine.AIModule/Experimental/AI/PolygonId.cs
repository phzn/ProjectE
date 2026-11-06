using System;

namespace UnityEngine.Experimental.AI
{
	// Token: 0x02000002 RID: 2
	public struct PolygonId : IEquatable<PolygonId>
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public bool IsNull()
		{
			return this.polyRef == 0UL;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x0000206C File Offset: 0x0000026C
		public static bool operator ==(PolygonId x, PolygonId y)
		{
			return x.polyRef == y.polyRef;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x0000208C File Offset: 0x0000028C
		public static bool operator !=(PolygonId x, PolygonId y)
		{
			return x.polyRef != y.polyRef;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020B0 File Offset: 0x000002B0
		public override int GetHashCode()
		{
			return this.polyRef.GetHashCode();
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000020D0 File Offset: 0x000002D0
		public bool Equals(PolygonId rhs)
		{
			return rhs == this;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000020F0 File Offset: 0x000002F0
		public override bool Equals(object obj)
		{
			bool flag = obj == null || !(obj is PolygonId);
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				PolygonId polygonId = (PolygonId)obj;
				flag2 = polygonId == this;
			}
			return flag2;
		}

		// Token: 0x04000001 RID: 1
		internal ulong polyRef;
	}
}
