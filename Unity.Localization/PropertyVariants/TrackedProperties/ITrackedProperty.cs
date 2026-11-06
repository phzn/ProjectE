using System;

namespace UnityEngine.Localization.PropertyVariants.TrackedProperties
{
	// Token: 0x020000EB RID: 235
	public interface ITrackedProperty
	{
		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000646 RID: 1606
		// (set) Token: 0x06000647 RID: 1607
		string PropertyPath { get; set; }

		// Token: 0x06000648 RID: 1608
		bool HasVariant(LocaleIdentifier localeIdentifier);
	}
}
