using System;
using System.Diagnostics;

namespace Unity.Entities.DebugProxies
{
	// Token: 0x020004C1 RID: 1217
	[DebuggerDisplay("{Value,nq}", Name = "{Key,nq}", Type = "{TypeName,nq}")]
	internal struct BlobMember
	{
		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06001D4D RID: 7501 RVA: 0x000663FB File Offset: 0x000645FB
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public string TypeName
		{
			get
			{
				return this.Value.GetType().Name;
			}
		}

		// Token: 0x04000A86 RID: 2694
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public string Key;

		// Token: 0x04000A87 RID: 2695
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public object Value;
	}
}
