using System;

namespace UnityEngine.Localization.PropertyVariants.TrackedProperties
{
	// Token: 0x020000EA RID: 234
	public interface IStringProperty : ITrackedProperty
	{
		// Token: 0x06000643 RID: 1603
		string GetValueAsString(LocaleIdentifier localeIdentifier);

		// Token: 0x06000644 RID: 1604
		string GetValueAsString(LocaleIdentifier localeIdentifier, LocaleIdentifier fallback);

		// Token: 0x06000645 RID: 1605
		void SetValueFromString(LocaleIdentifier localeIdentifier, string value);
	}
}
