using System;
using System.Collections.Generic;
using System.Xml.Xsl.Qil;

namespace System.Xml.Xsl.Xslt
{
	// Token: 0x020003F7 RID: 1015
	internal class StylesheetLevel
	{
		// Token: 0x04001FFD RID: 8189
		public Stylesheet[] Imports;

		// Token: 0x04001FFE RID: 8190
		public Dictionary<QilName, XslFlags> ModeFlags = new Dictionary<QilName, XslFlags>();

		// Token: 0x04001FFF RID: 8191
		public Dictionary<QilName, List<QilFunction>> ApplyFunctions = new Dictionary<QilName, List<QilFunction>>();
	}
}
