using System;

namespace Invector
{
	// Token: 0x02000467 RID: 1127
	[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
	public sealed class vClassHeaderAttribute : Attribute
	{
		// Token: 0x06001BED RID: 7149 RVA: 0x000AF704 File Offset: 0x000AD904
		public vClassHeaderAttribute(string header, bool openClose = true, string iconName = "icon_v2", bool useHelpBox = false, string helpBoxText = "")
		{
			this.header = header.ToUpper();
			this.openClose = openClose;
			this.iconName = iconName;
			this.useHelpBox = useHelpBox;
			this.helpBoxText = helpBoxText;
		}

		// Token: 0x06001BEE RID: 7150 RVA: 0x000AF736 File Offset: 0x000AD936
		public vClassHeaderAttribute(string header, string helpBoxText)
		{
			this.header = header.ToUpper();
			this.openClose = true;
			this.iconName = "icon_v2";
			this.useHelpBox = true;
			this.helpBoxText = helpBoxText;
		}

		// Token: 0x04001D4A RID: 7498
		public string header;

		// Token: 0x04001D4B RID: 7499
		public bool openClose;

		// Token: 0x04001D4C RID: 7500
		public string iconName;

		// Token: 0x04001D4D RID: 7501
		public bool useHelpBox;

		// Token: 0x04001D4E RID: 7502
		public string helpBoxText;
	}
}
