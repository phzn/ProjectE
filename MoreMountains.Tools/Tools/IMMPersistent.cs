using System;

namespace MoreMountains.Tools
{
	// Token: 0x02000132 RID: 306
	public interface IMMPersistent
	{
		// Token: 0x060006EA RID: 1770
		string GetGuid();

		// Token: 0x060006EB RID: 1771
		string OnSave();

		// Token: 0x060006EC RID: 1772
		void OnLoad(string data);

		// Token: 0x060006ED RID: 1773
		bool ShouldBeSaved();
	}
}
