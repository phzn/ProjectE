using System;
using System.Collections.Generic;
using System.Text;

namespace Features.CielOS.Apps.Story
{
	// Token: 0x0200143D RID: 5181
	public static class PersonalDiaryTextFormatting
	{
		// Token: 0x06009FF6 RID: 40950 RVA: 0x004204DC File Offset: 0x0041E6DC
		public static string Escape(string plainText)
		{
			if (!string.IsNullOrEmpty(plainText))
			{
				return plainText.Replace("<", "<noparse><</noparse>");
			}
			return "";
		}

		// Token: 0x06009FF7 RID: 40951 RVA: 0x004204FC File Offset: 0x0041E6FC
		public static int PlainToRichIndex(string plainText, int plainIndex)
		{
			plainText = plainText ?? "";
			plainIndex = PersonalDiaryTextFormatting.Clamp(plainIndex, 0, plainText.Length);
			int num = plainIndex;
			for (int i = 0; i < plainIndex; i++)
			{
				if (plainText[i] == '<')
				{
					num += "<noparse><</noparse>".Length - 1;
				}
			}
			return num;
		}

		// Token: 0x06009FF8 RID: 40952 RVA: 0x00420550 File Offset: 0x0041E750
		public static string Normalize(string text)
		{
			List<PersonalDiaryTextFormatting.Character> list = PersonalDiaryTextFormatting.Parse(text);
			return PersonalDiaryTextFormatting.Render(list, 0, list.Count);
		}

		// Token: 0x06009FF9 RID: 40953 RVA: 0x00420574 File Offset: 0x0041E774
		public static string ToPlainText(string richText)
		{
			List<PersonalDiaryTextFormatting.Character> list = PersonalDiaryTextFormatting.Parse(richText);
			StringBuilder stringBuilder = new StringBuilder(list.Count);
			foreach (PersonalDiaryTextFormatting.Character character in list)
			{
				stringBuilder.Append(character.value);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06009FFA RID: 40954 RVA: 0x004205E0 File Offset: 0x0041E7E0
		public static int RawToPlainIndex(string richText, int rawIndex)
		{
			richText = richText ?? "";
			return PersonalDiaryTextFormatting.VisibleBoundary(PersonalDiaryTextFormatting.Parse(richText), PersonalDiaryTextFormatting.Clamp(rawIndex, 0, richText.Length));
		}

		// Token: 0x06009FFB RID: 40955 RVA: 0x00420606 File Offset: 0x0041E806
		public static int PlainToRawIndex(string richText, int plainIndex)
		{
			return PersonalDiaryTextFormatting.RawBoundary(PersonalDiaryTextFormatting.Parse(richText), plainIndex);
		}

		// Token: 0x06009FFC RID: 40956 RVA: 0x00420614 File Offset: 0x0041E814
		public static string Slice(string richText, int rawStart, int rawEnd)
		{
			richText = richText ?? "";
			List<PersonalDiaryTextFormatting.Character> list = PersonalDiaryTextFormatting.Parse(richText);
			int num = PersonalDiaryTextFormatting.VisibleBoundary(list, PersonalDiaryTextFormatting.Clamp(Math.Min(rawStart, rawEnd), 0, richText.Length));
			int num2 = PersonalDiaryTextFormatting.VisibleBoundary(list, PersonalDiaryTextFormatting.Clamp(Math.Max(rawStart, rawEnd), 0, richText.Length));
			return PersonalDiaryTextFormatting.Render(list, num, num2);
		}

		// Token: 0x06009FFD RID: 40957 RVA: 0x00420670 File Offset: 0x0041E870
		public static string Toggle(string text, int selectionStart, int selectionEnd, string tag)
		{
			int num;
			int num2;
			return PersonalDiaryTextFormatting.Toggle(text, selectionStart, selectionEnd, tag, out num, out num2);
		}

		// Token: 0x06009FFE RID: 40958 RVA: 0x0042068C File Offset: 0x0041E88C
		public static string SetStyles(string richText, int rawStart, int rawEnd, int styleMask)
		{
			richText = richText ?? "";
			List<PersonalDiaryTextFormatting.Character> list = PersonalDiaryTextFormatting.Parse(richText);
			int num = PersonalDiaryTextFormatting.VisibleBoundary(list, PersonalDiaryTextFormatting.Clamp(Math.Min(rawStart, rawEnd), 0, richText.Length));
			int num2 = PersonalDiaryTextFormatting.VisibleBoundary(list, PersonalDiaryTextFormatting.Clamp(Math.Max(rawStart, rawEnd), 0, richText.Length));
			for (int i = num; i < num2; i++)
			{
				PersonalDiaryTextFormatting.Character character = list[i];
				character.styles = styleMask & 15;
				list[i] = character;
			}
			return PersonalDiaryTextFormatting.Render(list, 0, list.Count);
		}

		// Token: 0x06009FFF RID: 40959 RVA: 0x00420714 File Offset: 0x0041E914
		public static int StyleAt(string richText, int rawCaret)
		{
			richText = richText ?? "";
			List<PersonalDiaryTextFormatting.Character> list = PersonalDiaryTextFormatting.Parse(richText);
			if (list.Count == 0)
			{
				return 0;
			}
			int num = PersonalDiaryTextFormatting.VisibleBoundary(list, PersonalDiaryTextFormatting.Clamp(rawCaret, 0, richText.Length));
			return list[Math.Max(0, num - 1)].styles;
		}

		// Token: 0x0600A000 RID: 40960 RVA: 0x00420768 File Offset: 0x0041E968
		public static string Toggle(string text, int selectionStart, int selectionEnd, string tag, out int adjustedStart, out int adjustedEnd)
		{
			int num = PersonalDiaryTextFormatting.TagIndex(tag);
			if (num < 0)
			{
				throw new ArgumentException("Supported diary styles are b, i, u and s.", "tag");
			}
			text = text ?? "";
			List<PersonalDiaryTextFormatting.Character> list = PersonalDiaryTextFormatting.Parse(text);
			int num2 = PersonalDiaryTextFormatting.VisibleBoundary(list, PersonalDiaryTextFormatting.Clamp(selectionStart, 0, text.Length));
			int num3 = PersonalDiaryTextFormatting.VisibleBoundary(list, PersonalDiaryTextFormatting.Clamp(selectionEnd, 0, text.Length));
			int num4 = Math.Min(num2, num3);
			int num5 = Math.Max(num2, num3);
			int num6 = 1 << num;
			bool flag = num4 < num5;
			for (int i = num4; i < num5; i++)
			{
				if ((list[i].styles & num6) == 0)
				{
					flag = false;
					break;
				}
			}
			for (int j = num4; j < num5; j++)
			{
				PersonalDiaryTextFormatting.Character character = list[j];
				character.styles = (flag ? (character.styles & ~num6) : (character.styles | num6));
				list[j] = character;
			}
			string text2 = PersonalDiaryTextFormatting.Render(list, 0, list.Count);
			List<PersonalDiaryTextFormatting.Character> list2 = PersonalDiaryTextFormatting.Parse(text2);
			adjustedStart = PersonalDiaryTextFormatting.RawBoundary(list2, num2);
			adjustedEnd = PersonalDiaryTextFormatting.RawBoundary(list2, num3);
			return text2;
		}

		// Token: 0x0600A001 RID: 40961 RVA: 0x0042088C File Offset: 0x0041EA8C
		private static List<PersonalDiaryTextFormatting.Character> Parse(string text)
		{
			text = text ?? "";
			List<PersonalDiaryTextFormatting.Character> list = new List<PersonalDiaryTextFormatting.Character>(text.Length);
			int[] array = new int[PersonalDiaryTextFormatting.Tags.Length];
			int num = 0;
			bool flag = false;
			for (int i = 0; i < text.Length; i++)
			{
				if (text[i] == '<')
				{
					if (flag && PersonalDiaryTextFormatting.Matches(text, i, "</noparse>"))
					{
						i += 10;
						flag = false;
						continue;
					}
					if (!flag && PersonalDiaryTextFormatting.Matches(text, i, "<noparse>"))
					{
						i += 9;
						flag = true;
						continue;
					}
					int num2;
					bool flag2;
					int num3;
					if (!flag && PersonalDiaryTextFormatting.TryReadStyle(text, i, out num2, out flag2, out num3))
					{
						if (flag2)
						{
							array[num2] = Math.Max(0, array[num2] - 1);
						}
						else
						{
							array[num2]++;
						}
						if (array[num2] > 0)
						{
							num |= 1 << num2;
						}
						else
						{
							num &= ~(1 << num2);
						}
						i += num3;
						continue;
					}
				}
				list.Add(new PersonalDiaryTextFormatting.Character
				{
					value = text[i],
					styles = num,
					rawStart = i,
					rawEnd = i + 1
				});
			}
			return list;
		}

		// Token: 0x0600A002 RID: 40962 RVA: 0x004209C8 File Offset: 0x0041EBC8
		private static bool TryReadStyle(string text, int position, out int style, out bool closing, out int length)
		{
			closing = position + 1 < text.Length && text[position + 1] == '/';
			length = (closing ? 4 : 3);
			int num = position + (closing ? 2 : 1);
			style = -1;
			if (position + length > text.Length || text[position + length - 1] != '>')
			{
				return false;
			}
			char c = char.ToLowerInvariant(text[num]);
			if (c <= 'i')
			{
				if (c != 'b')
				{
					if (c == 'i')
					{
						style = 1;
					}
				}
				else
				{
					style = 0;
				}
			}
			else if (c != 's')
			{
				if (c == 'u')
				{
					style = 2;
				}
			}
			else
			{
				style = 3;
			}
			return style >= 0;
		}

		// Token: 0x0600A003 RID: 40963 RVA: 0x00420A70 File Offset: 0x0041EC70
		private static int TagIndex(string tag)
		{
			for (int i = 0; i < PersonalDiaryTextFormatting.Tags.Length; i++)
			{
				if (string.Equals(tag, PersonalDiaryTextFormatting.Tags[i], StringComparison.OrdinalIgnoreCase))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600A004 RID: 40964 RVA: 0x00420AA2 File Offset: 0x0041ECA2
		private static bool Matches(string text, int offset, string token)
		{
			return offset + token.Length <= text.Length && string.Compare(text, offset, token, 0, token.Length, StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x0600A005 RID: 40965 RVA: 0x00420ACC File Offset: 0x0041ECCC
		private static int VisibleBoundary(List<PersonalDiaryTextFormatting.Character> characters, int rawIndex)
		{
			int num = 0;
			while (num < characters.Count && characters[num].rawEnd <= rawIndex)
			{
				num++;
			}
			return num;
		}

		// Token: 0x0600A006 RID: 40966 RVA: 0x00420AFC File Offset: 0x0041ECFC
		private static int RawBoundary(List<PersonalDiaryTextFormatting.Character> characters, int plainIndex)
		{
			if (characters.Count == 0)
			{
				return 0;
			}
			plainIndex = PersonalDiaryTextFormatting.Clamp(plainIndex, 0, characters.Count);
			if (plainIndex != characters.Count)
			{
				return characters[plainIndex].rawStart;
			}
			return characters[characters.Count - 1].rawEnd;
		}

		// Token: 0x0600A007 RID: 40967 RVA: 0x00420B4C File Offset: 0x0041ED4C
		private static string Render(List<PersonalDiaryTextFormatting.Character> characters, int first, int last)
		{
			StringBuilder stringBuilder = new StringBuilder(Math.Max(0, last - first));
			List<int> list = new List<int>(PersonalDiaryTextFormatting.Tags.Length);
			List<int> list2 = new List<int>(PersonalDiaryTextFormatting.Tags.Length);
			for (int i = first; i < last; i++)
			{
				list2.Clear();
				for (int j = 0; j < PersonalDiaryTextFormatting.Tags.Length; j++)
				{
					if ((characters[i].styles & (1 << j)) != 0)
					{
						list2.Add(j);
					}
				}
				int num = 0;
				while (num < list.Count && num < list2.Count && list[num] == list2[num])
				{
					num++;
				}
				for (int k = list.Count - 1; k >= num; k--)
				{
					stringBuilder.Append("</").Append(PersonalDiaryTextFormatting.Tags[list[k]]).Append('>');
				}
				for (int l = num; l < list2.Count; l++)
				{
					stringBuilder.Append('<').Append(PersonalDiaryTextFormatting.Tags[list2[l]]).Append('>');
				}
				list.Clear();
				list.AddRange(list2);
				if (characters[i].value == '<')
				{
					stringBuilder.Append("<noparse><</noparse>");
				}
				else
				{
					stringBuilder.Append(characters[i].value);
				}
			}
			for (int m = list.Count - 1; m >= 0; m--)
			{
				stringBuilder.Append("</").Append(PersonalDiaryTextFormatting.Tags[list[m]]).Append('>');
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600A008 RID: 40968 RVA: 0x00420CF7 File Offset: 0x0041EEF7
		private static int Clamp(int value, int min, int max)
		{
			return Math.Min(max, Math.Max(min, value));
		}

		// Token: 0x0400A3DC RID: 41948
		private const string LiteralLessThan = "<noparse><</noparse>";

		// Token: 0x0400A3DD RID: 41949
		private static readonly string[] Tags = new string[] { "b", "i", "u", "s" };

		// Token: 0x0200143E RID: 5182
		private struct Character
		{
			// Token: 0x0400A3DE RID: 41950
			public char value;

			// Token: 0x0400A3DF RID: 41951
			public int styles;

			// Token: 0x0400A3E0 RID: 41952
			public int rawStart;

			// Token: 0x0400A3E1 RID: 41953
			public int rawEnd;
		}
	}
}
