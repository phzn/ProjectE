using System;
using System.Collections.Generic;
using System.Text;

namespace UnityEngine.Localization.SmartFormat.Core.Parsing
{
	// Token: 0x02000085 RID: 133
	public class Placeholder : FormatItem
	{
		// Token: 0x06000380 RID: 896 RVA: 0x0000DF0C File Offset: 0x0000C10C
		public void ReleaseToPool()
		{
			this.Clear();
			if (this.Format != null)
			{
				FormatItemPool.ReleaseFormat(this.Format);
			}
			this.Format = null;
			this.NestedDepth = 0;
			this.Alignment = 0;
			foreach (Selector selector in this.Selectors)
			{
				FormatItemPool.ReleaseSelector(selector);
			}
			this.Selectors.Clear();
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000381 RID: 897 RVA: 0x0000DF98 File Offset: 0x0000C198
		// (set) Token: 0x06000382 RID: 898 RVA: 0x0000DFA0 File Offset: 0x0000C1A0
		public int NestedDepth { get; set; }

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000383 RID: 899 RVA: 0x0000DFA9 File Offset: 0x0000C1A9
		public List<Selector> Selectors { get; } = new List<Selector>();

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000384 RID: 900 RVA: 0x0000DFB1 File Offset: 0x0000C1B1
		// (set) Token: 0x06000385 RID: 901 RVA: 0x0000DFB9 File Offset: 0x0000C1B9
		public int Alignment { get; set; }

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000386 RID: 902 RVA: 0x0000DFC2 File Offset: 0x0000C1C2
		// (set) Token: 0x06000387 RID: 903 RVA: 0x0000DFCA File Offset: 0x0000C1CA
		public string FormatterName { get; set; }

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000388 RID: 904 RVA: 0x0000DFD3 File Offset: 0x0000C1D3
		// (set) Token: 0x06000389 RID: 905 RVA: 0x0000DFDB File Offset: 0x0000C1DB
		public string FormatterOptions { get; set; }

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x0600038A RID: 906 RVA: 0x0000DFE4 File Offset: 0x0000C1E4
		// (set) Token: 0x0600038B RID: 907 RVA: 0x0000DFEC File Offset: 0x0000C1EC
		public Format Format { get; set; }

		// Token: 0x0600038C RID: 908 RVA: 0x0000DFF8 File Offset: 0x0000C1F8
		public override string ToString()
		{
			StringBuilder stringBuilder;
			string text;
			using (StringBuilderPool.Get(out stringBuilder))
			{
				int num = this.endIndex - this.startIndex;
				if (stringBuilder.Capacity < num)
				{
					stringBuilder.Capacity = num;
				}
				stringBuilder.Append('{');
				foreach (Selector selector in this.Selectors)
				{
					stringBuilder.Append(selector.baseString, selector.operatorStart, selector.endIndex - selector.operatorStart);
				}
				if (this.Alignment != 0)
				{
					stringBuilder.Append(',');
					stringBuilder.Append(this.Alignment);
				}
				if (this.FormatterName != "")
				{
					stringBuilder.Append(':');
					stringBuilder.Append(this.FormatterName);
					if (this.FormatterOptions != "")
					{
						stringBuilder.Append('(');
						stringBuilder.Append(this.FormatterOptions);
						stringBuilder.Append(')');
					}
				}
				if (this.Format != null)
				{
					stringBuilder.Append(':');
					stringBuilder.Append(this.Format);
				}
				stringBuilder.Append('}');
				text = stringBuilder.ToString();
			}
			return text;
		}
	}
}
