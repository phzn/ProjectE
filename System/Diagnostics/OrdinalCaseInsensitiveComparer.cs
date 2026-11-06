using System;
using System.Collections;

namespace System.Diagnostics
{
	// Token: 0x02000242 RID: 578
	internal class OrdinalCaseInsensitiveComparer : IComparer
	{
		// Token: 0x060011C4 RID: 4548 RVA: 0x0004DCC8 File Offset: 0x0004BEC8
		public int Compare(object a, object b)
		{
			string text = a as string;
			string text2 = b as string;
			if (text != null && text2 != null)
			{
				return string.Compare(text, text2, StringComparison.OrdinalIgnoreCase);
			}
			return Comparer.Default.Compare(a, b);
		}

		// Token: 0x04000A6D RID: 2669
		internal static readonly OrdinalCaseInsensitiveComparer Default = new OrdinalCaseInsensitiveComparer();
	}
}
