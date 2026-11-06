using System;

namespace UnityEngine
{
	// Token: 0x02000004 RID: 4
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
	public class AssetReferenceUIRestriction : Attribute
	{
		// Token: 0x06000006 RID: 6 RVA: 0x000020E4 File Offset: 0x000002E4
		public virtual bool ValidateAsset(Object obj)
		{
			return true;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000020E7 File Offset: 0x000002E7
		public virtual bool ValidateAsset(string path)
		{
			return true;
		}
	}
}
