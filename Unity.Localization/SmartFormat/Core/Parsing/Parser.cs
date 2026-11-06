using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Localization.SmartFormat.Core.Settings;

namespace UnityEngine.Localization.SmartFormat.Core.Parsing
{
	// Token: 0x02000082 RID: 130
	[Serializable]
	public class Parser
	{
		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000365 RID: 869 RVA: 0x0000D1B0 File Offset: 0x0000B3B0
		// (set) Token: 0x06000366 RID: 870 RVA: 0x0000D1B8 File Offset: 0x0000B3B8
		public SmartSettings Settings
		{
			get
			{
				return this.m_Settings;
			}
			set
			{
				this.m_Settings = value;
			}
		}

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x06000367 RID: 871 RVA: 0x0000D1C4 File Offset: 0x0000B3C4
		// (remove) Token: 0x06000368 RID: 872 RVA: 0x0000D1FC File Offset: 0x0000B3FC
		public event EventHandler<ParsingErrorEventArgs> OnParsingFailure;

		// Token: 0x06000369 RID: 873 RVA: 0x0000D231 File Offset: 0x0000B431
		public Parser(SmartSettings settings)
		{
			this.m_Settings = settings;
		}

		// Token: 0x0600036A RID: 874 RVA: 0x0000D26E File Offset: 0x0000B46E
		public void AddAlphanumericSelectors()
		{
			this.m_AlphanumericSelectors = true;
		}

		// Token: 0x0600036B RID: 875 RVA: 0x0000D278 File Offset: 0x0000B478
		public void AddAdditionalSelectorChars(string chars)
		{
			foreach (char c in chars)
			{
				if (this.m_AllowedSelectorChars.IndexOf(c) == -1)
				{
					this.m_AllowedSelectorChars += c.ToString();
				}
			}
		}

		// Token: 0x0600036C RID: 876 RVA: 0x0000D2C8 File Offset: 0x0000B4C8
		public void AddOperators(string chars)
		{
			foreach (char c in chars)
			{
				if (this.m_Operators.IndexOf(c) == -1)
				{
					this.m_Operators += c.ToString();
				}
			}
		}

		// Token: 0x0600036D RID: 877 RVA: 0x0000D317 File Offset: 0x0000B517
		public void UseAlternativeEscapeChar(char alternativeEscapeChar = '\\')
		{
			this.m_AlternativeEscapeChar = alternativeEscapeChar;
			this.m_AlternativeEscaping = true;
		}

		// Token: 0x0600036E RID: 878 RVA: 0x0000D327 File Offset: 0x0000B527
		public void UseBraceEscaping()
		{
			this.m_AlternativeEscaping = false;
		}

		// Token: 0x0600036F RID: 879 RVA: 0x0000D330 File Offset: 0x0000B530
		public void UseAlternativeBraces(char opening, char closing)
		{
			this.m_OpeningBrace = opening;
			this.m_ClosingBrace = closing;
		}

		// Token: 0x06000370 RID: 880 RVA: 0x0000D340 File Offset: 0x0000B540
		public Format ParseFormat(string format, IList<string> formatterExtensionNames)
		{
			Format format2 = FormatItemPool.GetFormat(this.Settings, format);
			Format format3 = format2;
			Placeholder placeholder = null;
			int num = -1;
			int num2 = -1;
			int num3 = -1;
			ParsingErrors parsingErrors = ParsingErrorsPool.Get(format2);
			if (Parser.s_ParsingErrorText == null)
			{
				Parser.s_ParsingErrorText = new Parser.ParsingErrorText();
			}
			char openingBrace = this.m_OpeningBrace;
			char closingBrace = this.m_ClosingBrace;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			int i = 0;
			int length = format.Length;
			while (i < length)
			{
				char c = format[i];
				if (placeholder == null)
				{
					if (c == openingBrace)
					{
						if (i != num5)
						{
							format3.Items.Add(FormatItemPool.GetLiteralText(this.Settings, format3, num5, i));
						}
						num5 = i + 1;
						if (!this.m_AlternativeEscaping)
						{
							int num8 = num5;
							if (num8 < length && format[num8] == openingBrace)
							{
								i++;
								goto IL_05ED;
							}
						}
						num4++;
						placeholder = FormatItemPool.GetPlaceholder(this.Settings, format3, i, num4);
						format3.Items.Add(placeholder);
						format3.HasNested = true;
						num6 = i + 1;
						num7 = 0;
						num = -1;
					}
					else if (c == closingBrace)
					{
						if (i != num5)
						{
							format3.Items.Add(FormatItemPool.GetLiteralText(this.Settings, format3, num5, i));
						}
						num5 = i + 1;
						if (!this.m_AlternativeEscaping)
						{
							int num9 = num5;
							if (num9 < length && format[num9] == closingBrace)
							{
								i++;
								goto IL_05ED;
							}
						}
						if (format3.parent == null)
						{
							format3.Items.Add(FormatItemPool.GetLiteralText(this.Settings, format3, i, i + 1));
							parsingErrors.AddIssue(Parser.s_ParsingErrorText[Parser.ParsingError.TooManyClosingBraces], i, i + 1);
						}
						else
						{
							num4--;
							format3.endIndex = i;
							format3.parent.endIndex = i + 1;
							format3 = format3.parent.Parent as Format;
							num = -1;
						}
					}
					else if ((c == '\\' && this.Settings.ConvertCharacterStringLiterals) || (this.m_AlternativeEscaping && c == this.m_AlternativeEscapeChar))
					{
						num = -1;
						int num10 = i + 1;
						if (num10 < length && (format[num10] == openingBrace || format[num10] == closingBrace))
						{
							if (i != num5)
							{
								format3.Items.Add(FormatItemPool.GetLiteralText(this.Settings, format3, num5, i));
							}
							num5 = i + 1;
							i++;
						}
						else
						{
							if (i != num5)
							{
								format3.Items.Add(FormatItemPool.GetLiteralText(this.Settings, format3, num5, i));
							}
							if (i + 1 < format.Length && format[i + 1] == 'u')
							{
								num5 = i + 6;
							}
							else
							{
								num5 = i + 2;
							}
							if (num5 > length)
							{
								num5 = length;
							}
							format3.Items.Add(FormatItemPool.GetLiteralText(this.Settings, format3, i, num5));
							i++;
						}
					}
					else if (num != -1)
					{
						if (c == '(')
						{
							if (num == i)
							{
								num = -1;
							}
							else
							{
								num2 = i;
							}
						}
						else if (c == ')' || c == ':')
						{
							if (c == ')')
							{
								bool flag = num2 != -1;
								int num11 = i + 1;
								bool flag2 = num11 < format.Length && (format[num11] == ':' || format[num11] == closingBrace);
								if (!flag || !flag2)
								{
									num = -1;
									goto IL_05ED;
								}
								num3 = i;
								if (format[num11] == ':')
								{
									i++;
								}
							}
							bool flag3 = num == i;
							bool flag4 = num2 != -1 && num3 == -1;
							if (flag3 || flag4)
							{
								num = -1;
							}
							else
							{
								num5 = i + 1;
								Placeholder parent = format3.parent;
								if (num2 == -1)
								{
									string text = format.Substring(num, i - num);
									if (Parser.FormatterNameExists(text, formatterExtensionNames))
									{
										parent.FormatterName = text;
									}
									else
									{
										num5 = format3.startIndex;
									}
								}
								else
								{
									string text2 = format.Substring(num, num2 - num);
									if (Parser.FormatterNameExists(text2, formatterExtensionNames))
									{
										parent.FormatterName = text2;
										parent.FormatterOptions = format.Substring(num2 + 1, num3 - (num2 + 1));
									}
									else
									{
										num5 = format3.startIndex;
									}
								}
								format3.startIndex = num5;
								num = -1;
							}
						}
					}
				}
				else if (this.m_Operators.IndexOf(c) != -1)
				{
					if (i != num5)
					{
						placeholder.Selectors.Add(FormatItemPool.GetSelector(this.Settings, placeholder, format, num5, i, num6, num7));
						num7++;
						num6 = i;
					}
					num5 = i + 1;
				}
				else if (c == ':')
				{
					if (i != num5)
					{
						placeholder.Selectors.Add(FormatItemPool.GetSelector(this.Settings, placeholder, format, num5, i, num6, num7));
					}
					else if (num6 != i)
					{
						parsingErrors.AddIssue(string.Format("'0x{0:X}': {1}", Convert.ToByte(c), Parser.s_ParsingErrorText[Parser.ParsingError.TrailingOperatorsInSelector]), num6, i);
					}
					num5 = i + 1;
					placeholder.Format = FormatItemPool.GetFormat(this.Settings, placeholder, i + 1);
					format3 = placeholder.Format;
					placeholder = null;
					num = num5;
					num2 = -1;
					num3 = -1;
				}
				else if (c == closingBrace)
				{
					if (i != num5)
					{
						placeholder.Selectors.Add(FormatItemPool.GetSelector(this.Settings, placeholder, format, num5, i, num6, num7));
					}
					else if (num6 != i)
					{
						parsingErrors.AddIssue(string.Format("'0x{0:X}': {1}", Convert.ToByte(c), Parser.s_ParsingErrorText[Parser.ParsingError.TrailingOperatorsInSelector]), num6, i);
					}
					num5 = i + 1;
					num4--;
					placeholder.endIndex = i + 1;
					format3 = placeholder.Parent as Format;
					placeholder = null;
				}
				else if (('0' > c || c > '9') && (!this.m_AlphanumericSelectors || (('a' > c || c > 'z') && ('A' > c || c > 'Z'))) && this.m_AllowedSelectorChars.IndexOf(c) == -1)
				{
					parsingErrors.AddIssue(string.Format("'0x{0:X}': {1}", Convert.ToByte(c), Parser.s_ParsingErrorText[Parser.ParsingError.TrailingOperatorsInSelector]), i, i + 1);
				}
				IL_05ED:
				i++;
			}
			if (format3.parent != null || placeholder != null)
			{
				parsingErrors.AddIssue(Parser.s_ParsingErrorText[Parser.ParsingError.MissingClosingBrace], format.Length, format.Length);
				format3.endIndex = format.Length;
			}
			else if (num5 != format.Length)
			{
				format3.Items.Add(FormatItemPool.GetLiteralText(this.Settings, format3, num5, format.Length));
			}
			while (format3.parent != null)
			{
				format3 = format3.parent.Parent as Format;
				format3.endIndex = format.Length;
			}
			if (parsingErrors.HasIssues)
			{
				EventHandler<ParsingErrorEventArgs> onParsingFailure = this.OnParsingFailure;
				if (onParsingFailure != null)
				{
					onParsingFailure(this, new ParsingErrorEventArgs(parsingErrors, this.Settings.ParseErrorAction == ErrorAction.ThrowError));
				}
				return this.HandleParsingErrors(parsingErrors, format2);
			}
			ParsingErrorsPool.Release(parsingErrors);
			return format2;
		}

		// Token: 0x06000371 RID: 881 RVA: 0x0000DA14 File Offset: 0x0000BC14
		private Format HandleParsingErrors(ParsingErrors parsingErrors, Format currentResult)
		{
			switch (this.Settings.ParseErrorAction)
			{
			case ErrorAction.ThrowError:
				throw parsingErrors;
			case ErrorAction.OutputErrorInResult:
			{
				Format format = FormatItemPool.GetFormat(this.Settings, parsingErrors.Message, 0, parsingErrors.Message.Length);
				format.Items.Add(FormatItemPool.GetLiteralText(this.Settings, format, 0));
				return format;
			}
			case ErrorAction.Ignore:
			{
				int num;
				int j;
				for (j = 0; j < currentResult.Items.Count; j = num + 1)
				{
					Placeholder placeholder = currentResult.Items[j] as Placeholder;
					if (placeholder != null && parsingErrors.Issues.Any<ParsingErrors.ParsingIssue>((ParsingErrors.ParsingIssue errItem) => errItem.Index >= currentResult.Items[j].startIndex && errItem.Index <= currentResult.Items[j].endIndex))
					{
						currentResult.Items[j] = FormatItemPool.GetLiteralText(this.Settings, placeholder.Format ?? FormatItemPool.GetFormat(this.Settings, placeholder.baseString), placeholder.startIndex, placeholder.startIndex);
					}
					num = j;
				}
				return currentResult;
			}
			case ErrorAction.MaintainTokens:
			{
				int num;
				int i;
				for (i = 0; i < currentResult.Items.Count; i = num + 1)
				{
					Placeholder placeholder2 = currentResult.Items[i] as Placeholder;
					if (placeholder2 != null && parsingErrors.Issues.Any<ParsingErrors.ParsingIssue>((ParsingErrors.ParsingIssue errItem) => errItem.Index >= currentResult.Items[i].startIndex && errItem.Index <= currentResult.Items[i].endIndex))
					{
						currentResult.Items[i] = FormatItemPool.GetLiteralText(this.Settings, placeholder2.Format ?? FormatItemPool.GetFormat(this.Settings, placeholder2.baseString), placeholder2.startIndex, placeholder2.endIndex);
					}
					num = i;
				}
				return currentResult;
			}
			default:
				throw new ArgumentException("Illegal type for ParsingErrors", parsingErrors);
			}
		}

		// Token: 0x06000372 RID: 882 RVA: 0x0000DC6C File Offset: 0x0000BE6C
		private static bool FormatterNameExists(string name, IList<string> formatterExtensionNames)
		{
			using (IEnumerator<string> enumerator = formatterExtensionNames.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current == name)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x040000EC RID: 236
		[SerializeField]
		private char m_OpeningBrace = '{';

		// Token: 0x040000ED RID: 237
		[SerializeField]
		private char m_ClosingBrace = '}';

		// Token: 0x040000EE RID: 238
		[SerializeReference]
		[HideInInspector]
		private SmartSettings m_Settings;

		// Token: 0x040000EF RID: 239
		[Tooltip("If false, only digits are allowed as selectors. If true, selectors can be alpha-numeric. This allows optimized alpha-character detection. Specify any additional selector chars in AllowedSelectorChars.")]
		[SerializeField]
		private bool m_AlphanumericSelectors;

		// Token: 0x040000F0 RID: 240
		[Tooltip("A list of allowable selector characters, to support additional selector syntaxes such as math. Digits are always included, and letters can be included with AlphanumericSelectors.")]
		[SerializeField]
		private string m_AllowedSelectorChars = "";

		// Token: 0x040000F1 RID: 241
		[Tooltip("A list of characters that come between selectors. This can be \".\" for dot-notation, \"[]\" for arrays, or even math symbols. By default, there are no operators.")]
		[SerializeField]
		private string m_Operators = "";

		// Token: 0x040000F2 RID: 242
		[Tooltip("If false, double-curly braces are escaped. If true, the AlternativeEscapeChar is used for escaping braces.")]
		[SerializeField]
		private bool m_AlternativeEscaping;

		// Token: 0x040000F3 RID: 243
		[Tooltip("If AlternativeEscaping is true, then this character is used to escape curly braces.")]
		[SerializeField]
		private char m_AlternativeEscapeChar = '\\';

		// Token: 0x040000F4 RID: 244
		[Tooltip("The character literal escape character e.g. for \t (TAB) and others. This is kind of overlapping functionality with `UseAlternativeEscapeChar` Note: In a future release escape characters for placeholders  and character literals should become the same.")]
		[SerializeField]
		internal const char m_CharLiteralEscapeChar = '\\';

		// Token: 0x040000F5 RID: 245
		private static Parser.ParsingErrorText s_ParsingErrorText;

		// Token: 0x02000140 RID: 320
		public enum ParsingError
		{
			// Token: 0x040002DA RID: 730
			TooManyClosingBraces = 1,
			// Token: 0x040002DB RID: 731
			TrailingOperatorsInSelector,
			// Token: 0x040002DC RID: 732
			InvalidCharactersInSelector,
			// Token: 0x040002DD RID: 733
			MissingClosingBrace
		}

		// Token: 0x02000141 RID: 321
		internal class ParsingErrorText
		{
			// Token: 0x060007BE RID: 1982 RVA: 0x00019190 File Offset: 0x00017390
			internal ParsingErrorText()
			{
			}

			// Token: 0x17000189 RID: 393
			public string this[Parser.ParsingError parsingErrorKey]
			{
				get
				{
					return this._errors[parsingErrorKey];
				}
			}

			// Token: 0x040002DE RID: 734
			private readonly Dictionary<Parser.ParsingError, string> _errors = new Dictionary<Parser.ParsingError, string>
			{
				{
					Parser.ParsingError.TooManyClosingBraces,
					"Format string has too many closing braces"
				},
				{
					Parser.ParsingError.TrailingOperatorsInSelector,
					"There are trailing operators in the selector"
				},
				{
					Parser.ParsingError.InvalidCharactersInSelector,
					"Invalid character in the selector"
				},
				{
					Parser.ParsingError.MissingClosingBrace,
					"Format string is missing a closing brace"
				}
			};
		}
	}
}
