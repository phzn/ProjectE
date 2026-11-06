using System;
using System.Collections.Generic;
using System.Text;

namespace DA_Assets.CssNet
{
	// Token: 0x02000009 RID: 9
	public class CssStylesheet
	{
		// Token: 0x0600002D RID: 45 RVA: 0x00002610 File Offset: 0x00000810
		public CssStylesheet(string stylesheet)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int i = 0;
			int num = 0;
			while (i < stylesheet.Length)
			{
				char c = stylesheet[i];
				if (c != ' ')
				{
					if (c == '{')
					{
						int num2 = stylesheet.IndexOf('}', i);
						string text = stylesheet.Substring(i, num2 - i);
						CssSelector cssSelector = new CssSelector(stringBuilder.ToString().Trim(), text);
						this.selectors.Add(cssSelector);
						i = num2 - 1;
						stringBuilder.Clear();
					}
					else
					{
						stringBuilder.Append(c);
					}
				}
				i++;
				num++;
			}
		}

		// Token: 0x04000016 RID: 22
		public List<CssSelector> selectors = new List<CssSelector>();
	}
}
