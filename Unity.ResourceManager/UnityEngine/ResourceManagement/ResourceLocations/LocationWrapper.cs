using System;
using System.Collections.Generic;

namespace UnityEngine.ResourceManagement.ResourceLocations
{
	// Token: 0x02000049 RID: 73
	internal class LocationWrapper : IResourceLocation
	{
		// Token: 0x060001D1 RID: 465 RVA: 0x000070EC File Offset: 0x000052EC
		public LocationWrapper(IResourceLocation location)
		{
			this.m_InternalLocation = location;
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060001D2 RID: 466 RVA: 0x000070FB File Offset: 0x000052FB
		public string InternalId
		{
			get
			{
				return this.m_InternalLocation.InternalId;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060001D3 RID: 467 RVA: 0x00007108 File Offset: 0x00005308
		public string ProviderId
		{
			get
			{
				return this.m_InternalLocation.ProviderId;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060001D4 RID: 468 RVA: 0x00007115 File Offset: 0x00005315
		public IList<IResourceLocation> Dependencies
		{
			get
			{
				return this.m_InternalLocation.Dependencies;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060001D5 RID: 469 RVA: 0x00007122 File Offset: 0x00005322
		public int DependencyHashCode
		{
			get
			{
				return this.m_InternalLocation.DependencyHashCode;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060001D6 RID: 470 RVA: 0x0000712F File Offset: 0x0000532F
		public bool HasDependencies
		{
			get
			{
				return this.m_InternalLocation.HasDependencies;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060001D7 RID: 471 RVA: 0x0000713C File Offset: 0x0000533C
		public object Data
		{
			get
			{
				return this.m_InternalLocation.Data;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060001D8 RID: 472 RVA: 0x00007149 File Offset: 0x00005349
		public string PrimaryKey
		{
			get
			{
				return this.m_InternalLocation.PrimaryKey;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060001D9 RID: 473 RVA: 0x00007156 File Offset: 0x00005356
		public Type ResourceType
		{
			get
			{
				return this.m_InternalLocation.ResourceType;
			}
		}

		// Token: 0x060001DA RID: 474 RVA: 0x00007163 File Offset: 0x00005363
		public int Hash(Type resultType)
		{
			return this.m_InternalLocation.Hash(resultType);
		}

		// Token: 0x040000AD RID: 173
		private IResourceLocation m_InternalLocation;
	}
}
