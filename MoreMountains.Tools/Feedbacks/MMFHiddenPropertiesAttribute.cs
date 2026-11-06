using System;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001B8 RID: 440
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	public class MMFHiddenPropertiesAttribute : Attribute
	{
		// Token: 0x06000A8E RID: 2702 RVA: 0x0002F929 File Offset: 0x0002DB29
		public MMFHiddenPropertiesAttribute(params string[] propertiesNames)
		{
			this.PropertiesNames = propertiesNames;
		}

		// Token: 0x04000C8E RID: 3214
		public string[] PropertiesNames;
	}
}
