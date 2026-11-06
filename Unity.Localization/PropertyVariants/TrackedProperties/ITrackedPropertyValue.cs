using System;

namespace UnityEngine.Localization.PropertyVariants.TrackedProperties
{
	// Token: 0x020000EE RID: 238
	public interface ITrackedPropertyValue<T> : ITrackedProperty
	{
		// Token: 0x06000655 RID: 1621
		bool GetValue(LocaleIdentifier localeIdentifier, out T foundValue);

		// Token: 0x06000656 RID: 1622
		bool GetValue(LocaleIdentifier localeIdentifier, LocaleIdentifier fallback, out T foundValue);

		// Token: 0x06000657 RID: 1623
		void SetValue(LocaleIdentifier localeIdentifier, T value);
	}
}
