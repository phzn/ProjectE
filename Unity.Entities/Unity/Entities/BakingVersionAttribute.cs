using System;

namespace Unity.Entities
{
	// Token: 0x02000011 RID: 17
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
	public class BakingVersionAttribute : Attribute
	{
		// Token: 0x06000039 RID: 57 RVA: 0x000022B4 File Offset: 0x000004B4
		public BakingVersionAttribute(string userName, int version)
		{
			this.UserName = userName;
			this.Version = version;
			this.Excluded = false;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000022D1 File Offset: 0x000004D1
		public BakingVersionAttribute(bool excluded)
		{
			if (!excluded)
			{
				throw new ArgumentException("Name and Version must be set if not marked Excluded.");
			}
			this.Excluded = excluded;
			this.UserName = string.Empty;
			this.Version = 0;
		}

		// Token: 0x0400000B RID: 11
		internal string UserName;

		// Token: 0x0400000C RID: 12
		internal int Version;

		// Token: 0x0400000D RID: 13
		internal bool Excluded;
	}
}
