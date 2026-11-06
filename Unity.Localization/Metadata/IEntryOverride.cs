using System;
using UnityEngine.Localization.Tables;

namespace UnityEngine.Localization.Metadata
{
	// Token: 0x020000E2 RID: 226
	public interface IEntryOverride : IMetadata
	{
		// Token: 0x06000611 RID: 1553
		EntryOverrideType GetOverride(out TableReference tableReference, out TableEntryReference tableEntryReference);
	}
}
