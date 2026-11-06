using System;
using System.Collections.Generic;

namespace UnityEngine.ResourceManagement.ResourceLocations
{
	// Token: 0x02000047 RID: 71
	public interface IResourceLocation
	{
		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060001BA RID: 442
		string InternalId { get; }

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060001BB RID: 443
		string ProviderId { get; }

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060001BC RID: 444
		IList<IResourceLocation> Dependencies { get; }

		// Token: 0x060001BD RID: 445
		int Hash(Type resultType);

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060001BE RID: 446
		int DependencyHashCode { get; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060001BF RID: 447
		bool HasDependencies { get; }

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060001C0 RID: 448
		object Data { get; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060001C1 RID: 449
		string PrimaryKey { get; }

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060001C2 RID: 450
		Type ResourceType { get; }
	}
}
