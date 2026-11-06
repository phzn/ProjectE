using System;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace UnityEngine.ResourceManagement.Util
{
	// Token: 0x02000019 RID: 25
	internal sealed class LocationCacheKey : IOperationCacheKey, IEquatable<IOperationCacheKey>
	{
		// Token: 0x060000CF RID: 207 RVA: 0x00004983 File Offset: 0x00002B83
		public LocationCacheKey(IResourceLocation location, Type desiredType)
		{
			if (location == null)
			{
				throw new NullReferenceException("Resource location cannot be null.");
			}
			if (desiredType == null)
			{
				throw new NullReferenceException("Desired type cannot be null.");
			}
			this.m_Location = location;
			this.m_DesiredType = desiredType;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x000049BB File Offset: 0x00002BBB
		public override int GetHashCode()
		{
			return this.m_Location.Hash(this.m_DesiredType);
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x000049CE File Offset: 0x00002BCE
		public override bool Equals(object obj)
		{
			return this.Equals(obj as LocationCacheKey);
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x000049DC File Offset: 0x00002BDC
		public bool Equals(IOperationCacheKey other)
		{
			return this.Equals(other as LocationCacheKey);
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x000049EA File Offset: 0x00002BEA
		private bool Equals(LocationCacheKey other)
		{
			return this == other || (other != null && LocationUtils.LocationEquals(this.m_Location, other.m_Location) && object.Equals(this.m_DesiredType, other.m_DesiredType));
		}

		// Token: 0x0400004C RID: 76
		private readonly IResourceLocation m_Location;

		// Token: 0x0400004D RID: 77
		private readonly Type m_DesiredType;
	}
}
