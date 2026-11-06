using System;

namespace UnityEngine.Localization.Metadata
{
	// Token: 0x020000DE RID: 222
	[HideInInspector]
	public interface ISharedMetadata : IMetadata
	{
		// Token: 0x060005FD RID: 1533
		bool Contains(long keyId);

		// Token: 0x060005FE RID: 1534
		void AddEntry(long keyId);

		// Token: 0x060005FF RID: 1535
		void RemoveEntry(long keyId);
	}
}
