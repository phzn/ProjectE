using System;

namespace UnityEngine.Localization.SmartFormat.Core.Formatting
{
	// Token: 0x0200008A RID: 138
	internal class DataNotReadyException : Exception
	{
		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060003A1 RID: 929 RVA: 0x0000E2AA File Offset: 0x0000C4AA
		// (set) Token: 0x060003A2 RID: 930 RVA: 0x0000E2B2 File Offset: 0x0000C4B2
		public string Text { get; private set; }

		// Token: 0x060003A3 RID: 931 RVA: 0x0000E2BB File Offset: 0x0000C4BB
		public DataNotReadyException()
		{
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x0000E2C3 File Offset: 0x0000C4C3
		public DataNotReadyException(string text)
		{
			this.Text = text;
		}
	}
}
