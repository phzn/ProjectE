using System;

namespace UnityEngine.Rendering
{
	// Token: 0x020000DE RID: 222
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class ReloadAttribute : Attribute
	{
		// Token: 0x06000776 RID: 1910 RVA: 0x00024BEB File Offset: 0x00022DEB
		public ReloadAttribute(string[] paths, ReloadAttribute.Package package = ReloadAttribute.Package.Root)
		{
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x00024BF3 File Offset: 0x00022DF3
		public ReloadAttribute(string path, ReloadAttribute.Package package = ReloadAttribute.Package.Root)
			: this(new string[] { path }, package)
		{
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x00024C06 File Offset: 0x00022E06
		public ReloadAttribute(string pathFormat, int rangeMin, int rangeMax, ReloadAttribute.Package package = ReloadAttribute.Package.Root)
		{
		}

		// Token: 0x020001D5 RID: 469
		public enum Package
		{
			// Token: 0x040007A1 RID: 1953
			Builtin,
			// Token: 0x040007A2 RID: 1954
			Root,
			// Token: 0x040007A3 RID: 1955
			BuiltinExtra
		}
	}
}
