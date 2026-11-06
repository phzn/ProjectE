using System;

namespace UnityEngine.Localization.PropertyVariants.TrackedProperties
{
	// Token: 0x020000FF RID: 255
	[Serializable]
	public class StringTrackedProperty : TrackedProperty<string>
	{
		// Token: 0x06000679 RID: 1657 RVA: 0x00015E85 File Offset: 0x00014085
		protected override string ConvertFromString(string value)
		{
			return value;
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x00015E88 File Offset: 0x00014088
		protected override string ConvertToString(string value)
		{
			return value;
		}
	}
}
