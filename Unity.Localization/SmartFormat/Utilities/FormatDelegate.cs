using System;

namespace UnityEngine.Localization.SmartFormat.Utilities
{
	// Token: 0x02000043 RID: 67
	public class FormatDelegate : IFormattable
	{
		// Token: 0x0600025E RID: 606 RVA: 0x0000861D File Offset: 0x0000681D
		public FormatDelegate(Func<string, string> getFormat)
		{
			this.getFormat1 = getFormat;
		}

		// Token: 0x0600025F RID: 607 RVA: 0x0000862C File Offset: 0x0000682C
		public FormatDelegate(Func<string, IFormatProvider, string> getFormat)
		{
			this.getFormat2 = getFormat;
		}

		// Token: 0x06000260 RID: 608 RVA: 0x0000863B File Offset: 0x0000683B
		public string ToString(string format, IFormatProvider formatProvider)
		{
			if (this.getFormat1 == null)
			{
				return this.getFormat2(format, formatProvider);
			}
			return this.getFormat1(format);
		}

		// Token: 0x0400008E RID: 142
		private readonly Func<string, string> getFormat1;

		// Token: 0x0400008F RID: 143
		private readonly Func<string, IFormatProvider, string> getFormat2;
	}
}
