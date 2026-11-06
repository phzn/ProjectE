using System;
using System.Text;

namespace DA_Assets.CssNet
{
	// Token: 0x0200000B RID: 11
	public class CssRule
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000031 RID: 49 RVA: 0x0000273F File Offset: 0x0000093F
		public object objectValue
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002748 File Offset: 0x00000948
		public CssRule(string input)
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			CssRule.ParserMode parserMode = CssRule.ParserMode.property;
			bool flag = false;
			int i = 0;
			while (i < input.Length)
			{
				char c = input[i];
				if (c <= ':')
				{
					if (c != '"' && c != '\'')
					{
						if (c != ':')
						{
							goto IL_0072;
						}
						if (!flag)
						{
							parserMode = CssRule.ParserMode.value;
						}
					}
					else
					{
						flag = !flag;
					}
				}
				else
				{
					if (c != ';')
					{
						if (c != '{')
						{
							if (c != '}')
							{
								goto IL_0072;
							}
						}
						else
						{
							if (!flag)
							{
								goto IL_008F;
							}
							goto IL_008F;
						}
					}
					if (flag)
					{
						i = 1000;
					}
				}
				IL_008F:
				i++;
				continue;
				IL_0072:
				if (parserMode == CssRule.ParserMode.property)
				{
					stringBuilder.Append(c);
					goto IL_008F;
				}
				if (parserMode != CssRule.ParserMode.value)
				{
					goto IL_008F;
				}
				stringBuilder2.Append(c);
				goto IL_008F;
			}
			this.rule = stringBuilder.ToString().Trim();
			this.value = stringBuilder2.ToString().Trim();
		}

		// Token: 0x04000019 RID: 25
		public string originalRule;

		// Token: 0x0400001A RID: 26
		public string rule;

		// Token: 0x0400001B RID: 27
		public string value;

		// Token: 0x0200004D RID: 77
		public enum ParserMode
		{
			// Token: 0x040001D5 RID: 469
			property,
			// Token: 0x040001D6 RID: 470
			value
		}
	}
}
