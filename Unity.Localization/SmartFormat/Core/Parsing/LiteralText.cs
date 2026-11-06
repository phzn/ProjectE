using System;
using System.Globalization;

namespace UnityEngine.Localization.SmartFormat.Core.Parsing
{
	// Token: 0x02000081 RID: 129
	public class LiteralText : FormatItem
	{
		// Token: 0x06000362 RID: 866 RVA: 0x0000D03F File Offset: 0x0000B23F
		public override string ToString()
		{
			if (!this.SmartSettings.ConvertCharacterStringLiterals)
			{
				return base.RawText;
			}
			return this.ConvertCharacterLiteralsToUnicode();
		}

		// Token: 0x06000363 RID: 867 RVA: 0x0000D05C File Offset: 0x0000B25C
		private string ConvertCharacterLiteralsToUnicode()
		{
			string rawText = base.RawText;
			if (rawText.Length == 0)
			{
				return rawText;
			}
			if (rawText[0] != '\\')
			{
				return rawText;
			}
			if (rawText.Length < 2)
			{
				throw new ArgumentException("Missing escape sequence in literal: \"" + rawText + "\"");
			}
			char c = rawText[1];
			char c2;
			if (c <= '\\')
			{
				if (c <= '\'')
				{
					if (c == '"')
					{
						c2 = '"';
						goto IL_0138;
					}
					if (c == '\'')
					{
						c2 = '\'';
						goto IL_0138;
					}
				}
				else
				{
					if (c == '0')
					{
						c2 = '\0';
						goto IL_0138;
					}
					if (c == '\\')
					{
						c2 = '\\';
						goto IL_0138;
					}
				}
			}
			else if (c <= 'b')
			{
				if (c == 'a')
				{
					c2 = '\a';
					goto IL_0138;
				}
				if (c == 'b')
				{
					c2 = '\b';
					goto IL_0138;
				}
			}
			else
			{
				if (c == 'f')
				{
					c2 = '\f';
					goto IL_0138;
				}
				switch (c)
				{
				case 'n':
					c2 = '\n';
					goto IL_0138;
				case 'r':
					c2 = '\r';
					goto IL_0138;
				case 't':
					c2 = '\t';
					goto IL_0138;
				case 'u':
				{
					int num;
					if (!int.TryParse(rawText.Substring(2, rawText.Length - 2), NumberStyles.HexNumber, null, out num))
					{
						throw new ArgumentException("Failed to parse unicode escape sequence in literal: \"" + rawText + "\"");
					}
					c2 = (char)num;
					goto IL_0138;
				}
				case 'v':
					c2 = '\v';
					goto IL_0138;
				}
			}
			throw new ArgumentException("Unrecognized escape sequence in literal: \"" + rawText + "\"");
			IL_0138:
			return c2.ToString();
		}
	}
}
