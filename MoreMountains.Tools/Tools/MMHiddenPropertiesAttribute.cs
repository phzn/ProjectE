using System;

namespace MoreMountains.Tools
{
	// Token: 0x020000A5 RID: 165
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	public class MMHiddenPropertiesAttribute : Attribute
	{
		// Token: 0x06000369 RID: 873 RVA: 0x000117B6 File Offset: 0x0000F9B6
		public MMHiddenPropertiesAttribute(params string[] propertiesNames)
		{
			this.PropertiesNames = propertiesNames;
		}

		// Token: 0x040004D7 RID: 1239
		public string[] PropertiesNames;
	}
}
