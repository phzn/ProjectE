using System;

namespace UnityEngine.Localization.PropertyVariants.TrackedProperties
{
	// Token: 0x020000FE RID: 254
	[Serializable]
	public class BoolTrackedProperty : TrackedProperty<bool>
	{
		// Token: 0x06000676 RID: 1654 RVA: 0x00015E34 File Offset: 0x00014034
		protected override bool ConvertFromString(string value)
		{
			int num;
			if (int.TryParse(value, out num))
			{
				return (bool)Convert.ChangeType(num, typeof(bool));
			}
			return base.ConvertFromString(value);
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x00015E6D File Offset: 0x0001406D
		protected override string ConvertToString(bool value)
		{
			if (!value)
			{
				return "0";
			}
			return "1";
		}
	}
}
