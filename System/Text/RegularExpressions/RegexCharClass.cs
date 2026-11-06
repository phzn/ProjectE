using System;
using System.Collections.Generic;
using System.Globalization;

namespace System.Text.RegularExpressions
{
	// Token: 0x020001FA RID: 506
	internal sealed class RegexCharClass
	{
		// Token: 0x06000DBC RID: 3516 RVA: 0x00037D4C File Offset: 0x00035F4C
		public RegexCharClass()
		{
			this._rangelist = new List<RegexCharClass.SingleRange>(6);
			this._canonical = true;
			this._categories = new StringBuilder();
		}

		// Token: 0x06000DBD RID: 3517 RVA: 0x00037D72 File Offset: 0x00035F72
		private RegexCharClass(bool negate, List<RegexCharClass.SingleRange> ranges, StringBuilder categories, RegexCharClass subtraction)
		{
			this._rangelist = ranges;
			this._categories = categories;
			this._canonical = true;
			this._negate = negate;
			this._subtractor = subtraction;
		}

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06000DBE RID: 3518 RVA: 0x00037D9E File Offset: 0x00035F9E
		public bool CanMerge
		{
			get
			{
				return !this._negate && this._subtractor == null;
			}
		}

		// Token: 0x1700025C RID: 604
		// (set) Token: 0x06000DBF RID: 3519 RVA: 0x00037DB3 File Offset: 0x00035FB3
		public bool Negate
		{
			set
			{
				this._negate = value;
			}
		}

		// Token: 0x06000DC0 RID: 3520 RVA: 0x00037DBC File Offset: 0x00035FBC
		public void AddChar(char c)
		{
			this.AddRange(c, c);
		}

		// Token: 0x06000DC1 RID: 3521 RVA: 0x00037DC8 File Offset: 0x00035FC8
		public void AddCharClass(RegexCharClass cc)
		{
			if (!cc._canonical)
			{
				this._canonical = false;
			}
			else if (this._canonical && this.RangeCount() > 0 && cc.RangeCount() > 0 && cc.GetRangeAt(0).First <= this.GetRangeAt(this.RangeCount() - 1).Last)
			{
				this._canonical = false;
			}
			for (int i = 0; i < cc.RangeCount(); i++)
			{
				this._rangelist.Add(cc.GetRangeAt(i));
			}
			this._categories.Append(cc._categories.ToString());
		}

		// Token: 0x06000DC2 RID: 3522 RVA: 0x00037E64 File Offset: 0x00036064
		private void AddSet(string set)
		{
			if (this._canonical && this.RangeCount() > 0 && set.Length > 0 && set[0] <= this.GetRangeAt(this.RangeCount() - 1).Last)
			{
				this._canonical = false;
			}
			int i;
			for (i = 0; i < set.Length - 1; i += 2)
			{
				this._rangelist.Add(new RegexCharClass.SingleRange(set[i], set[i + 1] - '\u0001'));
			}
			if (i < set.Length)
			{
				this._rangelist.Add(new RegexCharClass.SingleRange(set[i], char.MaxValue));
			}
		}

		// Token: 0x06000DC3 RID: 3523 RVA: 0x00037F09 File Offset: 0x00036109
		public void AddSubtraction(RegexCharClass sub)
		{
			this._subtractor = sub;
		}

		// Token: 0x06000DC4 RID: 3524 RVA: 0x00037F14 File Offset: 0x00036114
		public void AddRange(char first, char last)
		{
			this._rangelist.Add(new RegexCharClass.SingleRange(first, last));
			if (this._canonical && this._rangelist.Count > 0 && first <= this._rangelist[this._rangelist.Count - 1].Last)
			{
				this._canonical = false;
			}
		}

		// Token: 0x06000DC5 RID: 3525 RVA: 0x00037F70 File Offset: 0x00036170
		public void AddCategoryFromName(string categoryName, bool invert, bool caseInsensitive, string pattern)
		{
			string text;
			if (RegexCharClass.s_definedCategories.TryGetValue(categoryName, out text) && !categoryName.Equals(RegexCharClass.s_internalRegexIgnoreCase))
			{
				if (caseInsensitive && (categoryName.Equals("Ll") || categoryName.Equals("Lu") || categoryName.Equals("Lt")))
				{
					text = RegexCharClass.s_definedCategories[RegexCharClass.s_internalRegexIgnoreCase];
				}
				if (invert)
				{
					text = RegexCharClass.NegateCategory(text);
				}
				this._categories.Append(text);
				return;
			}
			this.AddSet(RegexCharClass.SetFromProperty(categoryName, invert, pattern));
		}

		// Token: 0x06000DC6 RID: 3526 RVA: 0x00037FFA File Offset: 0x000361FA
		private void AddCategory(string category)
		{
			this._categories.Append(category);
		}

		// Token: 0x06000DC7 RID: 3527 RVA: 0x0003800C File Offset: 0x0003620C
		public void AddLowercase(CultureInfo culture)
		{
			this._canonical = false;
			int count = this._rangelist.Count;
			for (int i = 0; i < count; i++)
			{
				RegexCharClass.SingleRange singleRange = this._rangelist[i];
				if (singleRange.First == singleRange.Last)
				{
					char c = culture.TextInfo.ToLower(singleRange.First);
					this._rangelist[i] = new RegexCharClass.SingleRange(c, c);
				}
				else
				{
					this.AddLowercaseRange(singleRange.First, singleRange.Last, culture);
				}
			}
		}

		// Token: 0x06000DC8 RID: 3528 RVA: 0x00038090 File Offset: 0x00036290
		private void AddLowercaseRange(char chMin, char chMax, CultureInfo culture)
		{
			int i = 0;
			int num = RegexCharClass.s_lcTable.Length;
			while (i < num)
			{
				int num2 = (i + num) / 2;
				if (RegexCharClass.s_lcTable[num2].ChMax < chMin)
				{
					i = num2 + 1;
				}
				else
				{
					num = num2;
				}
			}
			if (i >= RegexCharClass.s_lcTable.Length)
			{
				return;
			}
			RegexCharClass.LowerCaseMapping lowerCaseMapping;
			while (i < RegexCharClass.s_lcTable.Length && (lowerCaseMapping = RegexCharClass.s_lcTable[i]).ChMin <= chMax)
			{
				char c;
				if ((c = lowerCaseMapping.ChMin) < chMin)
				{
					c = chMin;
				}
				char c2;
				if ((c2 = lowerCaseMapping.ChMax) > chMax)
				{
					c2 = chMax;
				}
				switch (lowerCaseMapping.LcOp)
				{
				case 0:
					c = (char)lowerCaseMapping.Data;
					c2 = (char)lowerCaseMapping.Data;
					break;
				case 1:
					c += (char)lowerCaseMapping.Data;
					c2 += (char)lowerCaseMapping.Data;
					break;
				case 2:
					c |= '\u0001';
					c2 |= '\u0001';
					break;
				case 3:
					c += c & '\u0001';
					c2 += c2 & '\u0001';
					break;
				}
				if (c < chMin || c2 > chMax)
				{
					this.AddRange(c, c2);
				}
				i++;
			}
		}

		// Token: 0x06000DC9 RID: 3529 RVA: 0x000381A7 File Offset: 0x000363A7
		public void AddWord(bool ecma, bool negate)
		{
			if (negate)
			{
				if (ecma)
				{
					this.AddSet("\00:A[_`a{İı");
					return;
				}
				this.AddCategory(RegexCharClass.s_notWord);
				return;
			}
			else
			{
				if (ecma)
				{
					this.AddSet("0:A[_`a{İı");
					return;
				}
				this.AddCategory(RegexCharClass.s_word);
				return;
			}
		}

		// Token: 0x06000DCA RID: 3530 RVA: 0x000381E1 File Offset: 0x000363E1
		public void AddSpace(bool ecma, bool negate)
		{
			if (negate)
			{
				if (ecma)
				{
					this.AddSet("\0\t\u000e !");
					return;
				}
				this.AddCategory(RegexCharClass.s_notSpace);
				return;
			}
			else
			{
				if (ecma)
				{
					this.AddSet("\t\u000e !");
					return;
				}
				this.AddCategory(RegexCharClass.s_space);
				return;
			}
		}

		// Token: 0x06000DCB RID: 3531 RVA: 0x0003821B File Offset: 0x0003641B
		public void AddDigit(bool ecma, bool negate, string pattern)
		{
			if (!ecma)
			{
				this.AddCategoryFromName("Nd", negate, false, pattern);
				return;
			}
			if (negate)
			{
				this.AddSet("\00:");
				return;
			}
			this.AddSet("0:");
		}

		// Token: 0x06000DCC RID: 3532 RVA: 0x0003824C File Offset: 0x0003644C
		public static string ConvertOldStringsToClass(string set, string category)
		{
			StringBuilder stringBuilder = StringBuilderCache.Acquire(set.Length + category.Length + 3);
			if (set.Length >= 2 && set[0] == '\0' && set[1] == '\0')
			{
				stringBuilder.Append('\u0001');
				stringBuilder.Append((char)(set.Length - 2));
				stringBuilder.Append((char)category.Length);
				stringBuilder.Append(set.Substring(2));
			}
			else
			{
				stringBuilder.Append('\0');
				stringBuilder.Append((char)set.Length);
				stringBuilder.Append((char)category.Length);
				stringBuilder.Append(set);
			}
			stringBuilder.Append(category);
			return StringBuilderCache.GetStringAndRelease(stringBuilder);
		}

		// Token: 0x06000DCD RID: 3533 RVA: 0x000382F9 File Offset: 0x000364F9
		public static char SingletonChar(string set)
		{
			return set[3];
		}

		// Token: 0x06000DCE RID: 3534 RVA: 0x00038302 File Offset: 0x00036502
		public static bool IsMergeable(string charClass)
		{
			return !RegexCharClass.IsNegated(charClass) && !RegexCharClass.IsSubtraction(charClass);
		}

		// Token: 0x06000DCF RID: 3535 RVA: 0x00038317 File Offset: 0x00036517
		public static bool IsEmpty(string charClass)
		{
			return charClass[2] == '\0' && charClass[0] == '\0' && charClass[1] == '\0' && !RegexCharClass.IsSubtraction(charClass);
		}

		// Token: 0x06000DD0 RID: 3536 RVA: 0x00038340 File Offset: 0x00036540
		public static bool IsSingleton(string set)
		{
			return set[0] == '\0' && set[2] == '\0' && set[1] == '\u0002' && !RegexCharClass.IsSubtraction(set) && (set[3] == char.MaxValue || set[3] + '\u0001' == set[4]);
		}

		// Token: 0x06000DD1 RID: 3537 RVA: 0x00038394 File Offset: 0x00036594
		public static bool IsSingletonInverse(string set)
		{
			return set[0] == '\u0001' && set[2] == '\0' && set[1] == '\u0002' && !RegexCharClass.IsSubtraction(set) && (set[3] == char.MaxValue || set[3] + '\u0001' == set[4]);
		}

		// Token: 0x06000DD2 RID: 3538 RVA: 0x000383E9 File Offset: 0x000365E9
		private static bool IsSubtraction(string charClass)
		{
			return charClass.Length > (int)('\u0003' + charClass[1] + charClass[2]);
		}

		// Token: 0x06000DD3 RID: 3539 RVA: 0x00038404 File Offset: 0x00036604
		private static bool IsNegated(string set)
		{
			return set != null && set[0] == '\u0001';
		}

		// Token: 0x06000DD4 RID: 3540 RVA: 0x00038415 File Offset: 0x00036615
		public static bool IsECMAWordChar(char ch)
		{
			return RegexCharClass.CharInClass(ch, "\0\n\00:A[_`a{İı");
		}

		// Token: 0x06000DD5 RID: 3541 RVA: 0x00038422 File Offset: 0x00036622
		public static bool IsWordChar(char ch)
		{
			return RegexCharClass.CharInClass(ch, RegexCharClass.WordClass) || ch == '\u200d' || ch == '\u200c';
		}

		// Token: 0x06000DD6 RID: 3542 RVA: 0x00038443 File Offset: 0x00036643
		public static bool CharInClass(char ch, string set)
		{
			return RegexCharClass.CharInClassRecursive(ch, set, 0);
		}

		// Token: 0x06000DD7 RID: 3543 RVA: 0x00038450 File Offset: 0x00036650
		private static bool CharInClassRecursive(char ch, string set, int start)
		{
			int num = (int)set[start + 1];
			int num2 = (int)set[start + 2];
			int num3 = start + 3 + num + num2;
			bool flag = false;
			if (set.Length > num3)
			{
				flag = RegexCharClass.CharInClassRecursive(ch, set, num3);
			}
			bool flag2 = RegexCharClass.CharInClassInternal(ch, set, start, num, num2);
			if (set[start] == '\u0001')
			{
				flag2 = !flag2;
			}
			return flag2 && !flag;
		}

		// Token: 0x06000DD8 RID: 3544 RVA: 0x000384B4 File Offset: 0x000366B4
		private static bool CharInClassInternal(char ch, string set, int start, int mySetLength, int myCategoryLength)
		{
			int num = start + 3;
			int num2 = num + mySetLength;
			while (num != num2)
			{
				int num3 = (num + num2) / 2;
				if (ch < set[num3])
				{
					num2 = num3;
				}
				else
				{
					num = num3 + 1;
				}
			}
			return (num & 1) == (start & 1) || (myCategoryLength != 0 && RegexCharClass.CharInCategory(ch, set, start, mySetLength, myCategoryLength));
		}

		// Token: 0x06000DD9 RID: 3545 RVA: 0x00038504 File Offset: 0x00036704
		private static bool CharInCategory(char ch, string set, int start, int mySetLength, int myCategoryLength)
		{
			UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(ch);
			int i = start + 3 + mySetLength;
			int num = i + myCategoryLength;
			while (i < num)
			{
				int num2 = (int)((short)set[i]);
				if (num2 == 0)
				{
					if (RegexCharClass.CharInCategoryGroup(ch, unicodeCategory, set, ref i))
					{
						return true;
					}
				}
				else if (num2 > 0)
				{
					if (num2 == 100)
					{
						if (char.IsWhiteSpace(ch))
						{
							return true;
						}
						i++;
						continue;
					}
					else
					{
						num2--;
						if (unicodeCategory == (UnicodeCategory)num2)
						{
							return true;
						}
					}
				}
				else if (num2 == -100)
				{
					if (!char.IsWhiteSpace(ch))
					{
						return true;
					}
					i++;
					continue;
				}
				else
				{
					num2 = -1 - num2;
					if (unicodeCategory != (UnicodeCategory)num2)
					{
						return true;
					}
				}
				i++;
			}
			return false;
		}

		// Token: 0x06000DDA RID: 3546 RVA: 0x0003858C File Offset: 0x0003678C
		private static bool CharInCategoryGroup(char ch, UnicodeCategory chcategory, string category, ref int i)
		{
			i++;
			int num = (int)((short)category[i]);
			if (num > 0)
			{
				bool flag = false;
				while (num != 0)
				{
					if (!flag)
					{
						num--;
						if (chcategory == (UnicodeCategory)num)
						{
							flag = true;
						}
					}
					i++;
					num = (int)((short)category[i]);
				}
				return flag;
			}
			bool flag2 = true;
			while (num != 0)
			{
				if (flag2)
				{
					num = -1 - num;
					if (chcategory == (UnicodeCategory)num)
					{
						flag2 = false;
					}
				}
				i++;
				num = (int)((short)category[i]);
			}
			return flag2;
		}

		// Token: 0x06000DDB RID: 3547 RVA: 0x000385F8 File Offset: 0x000367F8
		private static string NegateCategory(string category)
		{
			if (category == null)
			{
				return null;
			}
			StringBuilder stringBuilder = StringBuilderCache.Acquire(category.Length);
			foreach (short num in category)
			{
				stringBuilder.Append((char)(-(char)num));
			}
			return StringBuilderCache.GetStringAndRelease(stringBuilder);
		}

		// Token: 0x06000DDC RID: 3548 RVA: 0x00038640 File Offset: 0x00036840
		public static RegexCharClass Parse(string charClass)
		{
			return RegexCharClass.ParseRecursive(charClass, 0);
		}

		// Token: 0x06000DDD RID: 3549 RVA: 0x0003864C File Offset: 0x0003684C
		private static RegexCharClass ParseRecursive(string charClass, int start)
		{
			int num = (int)charClass[start + 1];
			int num2 = (int)charClass[start + 2];
			int num3 = start + 3 + num + num2;
			List<RegexCharClass.SingleRange> list = new List<RegexCharClass.SingleRange>(num);
			int i = start + 3;
			int num4 = i + num;
			while (i < num4)
			{
				char c = charClass[i];
				i++;
				char c2;
				if (i < num4)
				{
					c2 = charClass[i] - '\u0001';
				}
				else
				{
					c2 = char.MaxValue;
				}
				i++;
				list.Add(new RegexCharClass.SingleRange(c, c2));
			}
			RegexCharClass regexCharClass = null;
			if (charClass.Length > num3)
			{
				regexCharClass = RegexCharClass.ParseRecursive(charClass, num3);
			}
			return new RegexCharClass(charClass[start] == '\u0001', list, new StringBuilder(charClass.Substring(num4, num2)), regexCharClass);
		}

		// Token: 0x06000DDE RID: 3550 RVA: 0x00038705 File Offset: 0x00036905
		private int RangeCount()
		{
			return this._rangelist.Count;
		}

		// Token: 0x06000DDF RID: 3551 RVA: 0x00038714 File Offset: 0x00036914
		public string ToStringClass()
		{
			if (!this._canonical)
			{
				this.Canonicalize();
			}
			int num = this._rangelist.Count * 2;
			StringBuilder stringBuilder = StringBuilderCache.Acquire(num + this._categories.Length + 3);
			int num2;
			if (this._negate)
			{
				num2 = 1;
			}
			else
			{
				num2 = 0;
			}
			stringBuilder.Append((char)num2);
			stringBuilder.Append((char)num);
			stringBuilder.Append((char)this._categories.Length);
			for (int i = 0; i < this._rangelist.Count; i++)
			{
				RegexCharClass.SingleRange singleRange = this._rangelist[i];
				stringBuilder.Append(singleRange.First);
				if (singleRange.Last != '\uffff')
				{
					stringBuilder.Append(singleRange.Last + '\u0001');
				}
			}
			stringBuilder[1] = (char)(stringBuilder.Length - 3);
			stringBuilder.Append(this._categories);
			if (this._subtractor != null)
			{
				stringBuilder.Append(this._subtractor.ToStringClass());
			}
			return StringBuilderCache.GetStringAndRelease(stringBuilder);
		}

		// Token: 0x06000DE0 RID: 3552 RVA: 0x00038813 File Offset: 0x00036A13
		private RegexCharClass.SingleRange GetRangeAt(int i)
		{
			return this._rangelist[i];
		}

		// Token: 0x06000DE1 RID: 3553 RVA: 0x00038824 File Offset: 0x00036A24
		private void Canonicalize()
		{
			this._canonical = true;
			this._rangelist.Sort(RegexCharClass.SingleRangeComparer.Instance);
			if (this._rangelist.Count > 1)
			{
				bool flag = false;
				int num = 1;
				int num2 = 0;
				for (;;)
				{
					IL_002F:
					char c = this._rangelist[num2].Last;
					while (num != this._rangelist.Count && c != '\uffff')
					{
						RegexCharClass.SingleRange singleRange;
						if ((singleRange = this._rangelist[num]).First <= c + '\u0001')
						{
							if (c < singleRange.Last)
							{
								c = singleRange.Last;
							}
							num++;
						}
						else
						{
							IL_008A:
							this._rangelist[num2] = new RegexCharClass.SingleRange(this._rangelist[num2].First, c);
							num2++;
							if (!flag)
							{
								if (num2 < num)
								{
									this._rangelist[num2] = this._rangelist[num];
								}
								num++;
								goto IL_002F;
							}
							goto IL_00DA;
						}
					}
					flag = true;
					goto IL_008A;
				}
				IL_00DA:
				this._rangelist.RemoveRange(num2, this._rangelist.Count - num2);
			}
		}

		// Token: 0x06000DE2 RID: 3554 RVA: 0x00038924 File Offset: 0x00036B24
		private static string SetFromProperty(string capname, bool invert, string pattern)
		{
			int num = 0;
			int num2 = RegexCharClass.s_propTable.Length;
			while (num != num2)
			{
				int num3 = (num + num2) / 2;
				int num4 = string.Compare(capname, RegexCharClass.s_propTable[num3][0], StringComparison.Ordinal);
				if (num4 < 0)
				{
					num2 = num3;
				}
				else if (num4 > 0)
				{
					num = num3 + 1;
				}
				else
				{
					string text = RegexCharClass.s_propTable[num3][1];
					if (!invert)
					{
						return text;
					}
					if (text[0] == '\0')
					{
						return text.Substring(1);
					}
					return "\0" + text;
				}
			}
			throw new ArgumentException(SR.Format("parsing \"{0}\" - {1}", pattern, SR.Format("Unknown property '{0}'.", capname)));
		}

		// Token: 0x04000831 RID: 2097
		private const int FLAGS = 0;

		// Token: 0x04000832 RID: 2098
		private const int SETLENGTH = 1;

		// Token: 0x04000833 RID: 2099
		private const int CATEGORYLENGTH = 2;

		// Token: 0x04000834 RID: 2100
		private const int SETSTART = 3;

		// Token: 0x04000835 RID: 2101
		private const string NullCharString = "\0";

		// Token: 0x04000836 RID: 2102
		private const char NullChar = '\0';

		// Token: 0x04000837 RID: 2103
		private const char LastChar = '\uffff';

		// Token: 0x04000838 RID: 2104
		private const char GroupChar = '\0';

		// Token: 0x04000839 RID: 2105
		private const short SpaceConst = 100;

		// Token: 0x0400083A RID: 2106
		private const short NotSpaceConst = -100;

		// Token: 0x0400083B RID: 2107
		private const char ZeroWidthJoiner = '\u200d';

		// Token: 0x0400083C RID: 2108
		private const char ZeroWidthNonJoiner = '\u200c';

		// Token: 0x0400083D RID: 2109
		private static readonly string s_internalRegexIgnoreCase = "__InternalRegexIgnoreCase__";

		// Token: 0x0400083E RID: 2110
		private static readonly string s_space = "d";

		// Token: 0x0400083F RID: 2111
		private static readonly string s_notSpace = "ﾜ";

		// Token: 0x04000840 RID: 2112
		private static readonly string s_word = "\0\u0002\u0004\u0005\u0003\u0001\u0006\t\u0013\0";

		// Token: 0x04000841 RID: 2113
		private static readonly string s_notWord = "\0\ufffe￼\ufffb\ufffd\uffff\ufffa\ufff7￭\0";

		// Token: 0x04000842 RID: 2114
		public static readonly string SpaceClass = "\0\0\u0001d";

		// Token: 0x04000843 RID: 2115
		public static readonly string NotSpaceClass = "\u0001\0\u0001d";

		// Token: 0x04000844 RID: 2116
		public static readonly string WordClass = "\0\0\n\0\u0002\u0004\u0005\u0003\u0001\u0006\t\u0013\0";

		// Token: 0x04000845 RID: 2117
		public static readonly string NotWordClass = "\u0001\0\n\0\u0002\u0004\u0005\u0003\u0001\u0006\t\u0013\0";

		// Token: 0x04000846 RID: 2118
		public static readonly string DigitClass = "\0\0\u0001\t";

		// Token: 0x04000847 RID: 2119
		public static readonly string NotDigitClass = "\0\0\u0001\ufff7";

		// Token: 0x04000848 RID: 2120
		private const string ECMASpaceSet = "\t\u000e !";

		// Token: 0x04000849 RID: 2121
		private const string NotECMASpaceSet = "\0\t\u000e !";

		// Token: 0x0400084A RID: 2122
		private const string ECMAWordSet = "0:A[_`a{İı";

		// Token: 0x0400084B RID: 2123
		private const string NotECMAWordSet = "\00:A[_`a{İı";

		// Token: 0x0400084C RID: 2124
		private const string ECMADigitSet = "0:";

		// Token: 0x0400084D RID: 2125
		private const string NotECMADigitSet = "\00:";

		// Token: 0x0400084E RID: 2126
		public const string ECMASpaceClass = "\0\u0004\0\t\u000e !";

		// Token: 0x0400084F RID: 2127
		public const string NotECMASpaceClass = "\u0001\u0004\0\t\u000e !";

		// Token: 0x04000850 RID: 2128
		public const string ECMAWordClass = "\0\n\00:A[_`a{İı";

		// Token: 0x04000851 RID: 2129
		public const string NotECMAWordClass = "\u0001\n\00:A[_`a{İı";

		// Token: 0x04000852 RID: 2130
		public const string ECMADigitClass = "\0\u0002\00:";

		// Token: 0x04000853 RID: 2131
		public const string NotECMADigitClass = "\u0001\u0002\00:";

		// Token: 0x04000854 RID: 2132
		public const string AnyClass = "\0\u0001\0\0";

		// Token: 0x04000855 RID: 2133
		public const string EmptyClass = "\0\0\0";

		// Token: 0x04000856 RID: 2134
		private const int DefinedCategoriesCapacity = 38;

		// Token: 0x04000857 RID: 2135
		private static readonly Dictionary<string, string> s_definedCategories = new Dictionary<string, string>(38)
		{
			{ "Cc", "\u000f" },
			{ "Cf", "\u0010" },
			{ "Cn", "\u001e" },
			{ "Co", "\u0012" },
			{ "Cs", "\u0011" },
			{ "C", "\0\u000f\u0010\u001e\u0012\u0011\0" },
			{ "Ll", "\u0002" },
			{ "Lm", "\u0004" },
			{ "Lo", "\u0005" },
			{ "Lt", "\u0003" },
			{ "Lu", "\u0001" },
			{ "L", "\0\u0002\u0004\u0005\u0003\u0001\0" },
			{ "__InternalRegexIgnoreCase__", "\0\u0002\u0003\u0001\0" },
			{ "Mc", "\a" },
			{ "Me", "\b" },
			{ "Mn", "\u0006" },
			{ "M", "\0\a\b\u0006\0" },
			{ "Nd", "\t" },
			{ "Nl", "\n" },
			{ "No", "\v" },
			{ "N", "\0\t\n\v\0" },
			{ "Pc", "\u0013" },
			{ "Pd", "\u0014" },
			{ "Pe", "\u0016" },
			{ "Po", "\u0019" },
			{ "Ps", "\u0015" },
			{ "Pf", "\u0018" },
			{ "Pi", "\u0017" },
			{ "P", "\0\u0013\u0014\u0016\u0019\u0015\u0018\u0017\0" },
			{ "Sc", "\u001b" },
			{ "Sk", "\u001c" },
			{ "Sm", "\u001a" },
			{ "So", "\u001d" },
			{ "S", "\0\u001b\u001c\u001a\u001d\0" },
			{ "Zl", "\r" },
			{ "Zp", "\u000e" },
			{ "Zs", "\f" },
			{ "Z", "\0\r\u000e\f\0" }
		};

		// Token: 0x04000858 RID: 2136
		private static readonly string[][] s_propTable = new string[][]
		{
			new string[] { "IsAlphabeticPresentationForms", "ﬀﭐ" },
			new string[] { "IsArabic", "\u0600܀" },
			new string[] { "IsArabicPresentationForms-A", "ﭐ\ufe00" },
			new string[] { "IsArabicPresentationForms-B", "ﹰ\uff00" },
			new string[] { "IsArmenian", "\u0530\u0590" },
			new string[] { "IsArrows", "←∀" },
			new string[] { "IsBasicLatin", "\0\u0080" },
			new string[] { "IsBengali", "ঀ\u0a00" },
			new string[] { "IsBlockElements", "▀■" },
			new string[] { "IsBopomofo", "\u3100\u3130" },
			new string[] { "IsBopomofoExtended", "ㆠ㇀" },
			new string[] { "IsBoxDrawing", "─▀" },
			new string[] { "IsBraillePatterns", "⠀⤀" },
			new string[] { "IsBuhid", "ᝀᝠ" },
			new string[] { "IsCJKCompatibility", "㌀㐀" },
			new string[] { "IsCJKCompatibilityForms", "︰﹐" },
			new string[] { "IsCJKCompatibilityIdeographs", "豈ﬀ" },
			new string[] { "IsCJKRadicalsSupplement", "⺀⼀" },
			new string[] { "IsCJKSymbolsandPunctuation", "\u3000\u3040" },
			new string[] { "IsCJKUnifiedIdeographs", "一ꀀ" },
			new string[] { "IsCJKUnifiedIdeographsExtensionA", "㐀䷀" },
			new string[] { "IsCherokee", "Ꭰ᐀" },
			new string[] { "IsCombiningDiacriticalMarks", "\u0300Ͱ" },
			new string[] { "IsCombiningDiacriticalMarksforSymbols", "\u20d0℀" },
			new string[] { "IsCombiningHalfMarks", "\ufe20︰" },
			new string[] { "IsCombiningMarksforSymbols", "\u20d0℀" },
			new string[] { "IsControlPictures", "␀⑀" },
			new string[] { "IsCurrencySymbols", "₠\u20d0" },
			new string[] { "IsCyrillic", "ЀԀ" },
			new string[] { "IsCyrillicSupplement", "Ԁ\u0530" },
			new string[] { "IsDevanagari", "\u0900ঀ" },
			new string[] { "IsDingbats", "✀⟀" },
			new string[] { "IsEnclosedAlphanumerics", "①─" },
			new string[] { "IsEnclosedCJKLettersandMonths", "㈀㌀" },
			new string[] { "IsEthiopic", "ሀᎀ" },
			new string[] { "IsGeneralPunctuation", "\u2000⁰" },
			new string[] { "IsGeometricShapes", "■☀" },
			new string[] { "IsGeorgian", "Ⴀᄀ" },
			new string[] { "IsGreek", "ͰЀ" },
			new string[] { "IsGreekExtended", "ἀ\u2000" },
			new string[] { "IsGreekandCoptic", "ͰЀ" },
			new string[] { "IsGujarati", "\u0a80\u0b00" },
			new string[] { "IsGurmukhi", "\u0a00\u0a80" },
			new string[] { "IsHalfwidthandFullwidthForms", "\uff00\ufff0" },
			new string[] { "IsHangulCompatibilityJamo", "\u3130㆐" },
			new string[] { "IsHangulJamo", "ᄀሀ" },
			new string[] { "IsHangulSyllables", "가ힰ" },
			new string[] { "IsHanunoo", "ᜠᝀ" },
			new string[] { "IsHebrew", "\u0590\u0600" },
			new string[] { "IsHighPrivateUseSurrogates", "\udb80\udc00" },
			new string[] { "IsHighSurrogates", "\ud800\udb80" },
			new string[] { "IsHiragana", "\u3040゠" },
			new string[] { "IsIPAExtensions", "ɐʰ" },
			new string[] { "IsIdeographicDescriptionCharacters", "⿰\u3000" },
			new string[] { "IsKanbun", "㆐ㆠ" },
			new string[] { "IsKangxiRadicals", "⼀\u2fe0" },
			new string[] { "IsKannada", "ಀ\u0d00" },
			new string[] { "IsKatakana", "゠\u3100" },
			new string[] { "IsKatakanaPhoneticExtensions", "ㇰ㈀" },
			new string[] { "IsKhmer", "ក᠀" },
			new string[] { "IsKhmerSymbols", "᧠ᨀ" },
			new string[] { "IsLao", "\u0e80ༀ" },
			new string[] { "IsLatin-1Supplement", "\u0080Ā" },
			new string[] { "IsLatinExtended-A", "Āƀ" },
			new string[] { "IsLatinExtended-B", "ƀɐ" },
			new string[] { "IsLatinExtendedAdditional", "Ḁἀ" },
			new string[] { "IsLetterlikeSymbols", "℀⅐" },
			new string[] { "IsLimbu", "ᤀᥐ" },
			new string[] { "IsLowSurrogates", "\udc00\ue000" },
			new string[] { "IsMalayalam", "\u0d00\u0d80" },
			new string[] { "IsMathematicalOperators", "∀⌀" },
			new string[] { "IsMiscellaneousMathematicalSymbols-A", "⟀⟰" },
			new string[] { "IsMiscellaneousMathematicalSymbols-B", "⦀⨀" },
			new string[] { "IsMiscellaneousSymbols", "☀✀" },
			new string[] { "IsMiscellaneousSymbolsandArrows", "⬀Ⰰ" },
			new string[] { "IsMiscellaneousTechnical", "⌀␀" },
			new string[] { "IsMongolian", "᠀ᢰ" },
			new string[] { "IsMyanmar", "ကႠ" },
			new string[] { "IsNumberForms", "⅐←" },
			new string[] { "IsOgham", "\u1680ᚠ" },
			new string[] { "IsOpticalCharacterRecognition", "⑀①" },
			new string[] { "IsOriya", "\u0b00\u0b80" },
			new string[] { "IsPhoneticExtensions", "ᴀᶀ" },
			new string[] { "IsPrivateUse", "\ue000豈" },
			new string[] { "IsPrivateUseArea", "\ue000豈" },
			new string[] { "IsRunic", "ᚠᜀ" },
			new string[] { "IsSinhala", "\u0d80\u0e00" },
			new string[] { "IsSmallFormVariants", "﹐ﹰ" },
			new string[] { "IsSpacingModifierLetters", "ʰ\u0300" },
			new string[] { "IsSpecials", "\ufff0" },
			new string[] { "IsSuperscriptsandSubscripts", "⁰₠" },
			new string[] { "IsSupplementalArrows-A", "⟰⠀" },
			new string[] { "IsSupplementalArrows-B", "⤀⦀" },
			new string[] { "IsSupplementalMathematicalOperators", "⨀⬀" },
			new string[] { "IsSyriac", "܀ݐ" },
			new string[] { "IsTagalog", "ᜀᜠ" },
			new string[] { "IsTagbanwa", "ᝠក" },
			new string[] { "IsTaiLe", "ᥐᦀ" },
			new string[] { "IsTamil", "\u0b80\u0c00" },
			new string[] { "IsTelugu", "\u0c00ಀ" },
			new string[] { "IsThaana", "ހ߀" },
			new string[] { "IsThai", "\u0e00\u0e80" },
			new string[] { "IsTibetan", "ༀက" },
			new string[] { "IsUnifiedCanadianAboriginalSyllabics", "᐀\u1680" },
			new string[] { "IsVariationSelectors", "\ufe00︐" },
			new string[] { "IsYiRadicals", "꒐ꓐ" },
			new string[] { "IsYiSyllables", "ꀀ꒐" },
			new string[] { "IsYijingHexagramSymbols", "䷀一" },
			new string[] { "_xmlC", "-/0;A[_`a{·\u00b8À×Ø÷øĲĴĿŁŉŊſƀǄǍǱǴǶǺȘɐʩʻ\u02c2ː\u02d2\u0300\u0346\u0360\u0362Ά\u038bΌ\u038dΎ\u03a2ΣϏϐϗϚϛϜϝϞϟϠϡϢϴЁЍЎѐёѝў҂\u0483\u0487ҐӅӇӉӋӍӐӬӮӶӸӺԱ\u0557ՙ՚աև\u0591\u05a2\u05a3\u05ba\u05bb־\u05bf׀\u05c1׃\u05c4\u05c5א\u05ebװ׳ءػـ\u0653٠٪\u0670ڸںڿۀۏې۔ە۩\u06eaۮ۰ۺ\u0901ऄअ\u093a\u093c\u094e\u0951\u0955क़।०॰\u0981\u0984অ\u098dএ\u0991ও\u09a9প\u09b1ল\u09b3শ\u09ba\u09bcঽ\u09be\u09c5\u09c7\u09c9\u09cbৎ\u09d7\u09d8ড়\u09deয়\u09e4০৲\u0a02\u0a03ਅ\u0a0bਏ\u0a11ਓ\u0a29ਪ\u0a31ਲ\u0a34ਵ\u0a37ਸ\u0a3a\u0a3c\u0a3d\u0a3e\u0a43\u0a47\u0a49\u0a4b\u0a4eਖ਼\u0a5dਫ਼\u0a5f੦\u0a75\u0a81\u0a84અઌઍ\u0a8eએ\u0a92ઓ\u0aa9પ\u0ab1લ\u0ab4વ\u0aba\u0abc\u0ac6\u0ac7\u0aca\u0acb\u0aceૠૡ૦૰\u0b01\u0b04ଅ\u0b0dଏ\u0b11ଓ\u0b29ପ\u0b31ଲ\u0b34ଶ\u0b3a\u0b3c\u0b44\u0b47\u0b49\u0b4b\u0b4e\u0b56\u0b58ଡ଼\u0b5eୟ\u0b62୦୰\u0b82\u0b84அ\u0b8bஎ\u0b91ஒ\u0b96ங\u0b9bஜ\u0b9dஞ\u0ba0ண\u0ba5ந\u0babமஶஷ\u0bba\u0bbe\u0bc3\u0bc6\u0bc9\u0bca\u0bce\u0bd7\u0bd8௧௰\u0c01\u0c04అ\u0c0dఎ\u0c11ఒ\u0c29పఴవ\u0c3a\u0c3e\u0c45\u0c46\u0c49\u0c4a\u0c4e\u0c55\u0c57ౠ\u0c62౦\u0c70\u0c82಄ಅ\u0c8dಎ\u0c91ಒ\u0ca9ಪ\u0cb4ವ\u0cba\u0cbe\u0cc5\u0cc6\u0cc9\u0cca\u0cce\u0cd5\u0cd7ೞ\u0cdfೠ\u0ce2೦\u0cf0\u0d02ഄഅ\u0d0dഎ\u0d11ഒഩപഺ\u0d3e\u0d44\u0d46\u0d49\u0d4aൎ\u0d57൘ൠ\u0d62൦൰กฯะ\u0e3bเ๏๐๚ກ\u0e83ຄ\u0e85ງຉຊ\u0e8bຍຎດຘນຠມ\u0ea4ລ\u0ea6ວຨສຬອຯະ\u0eba\u0ebb\u0ebeເ\u0ec5ໆ\u0ec7\u0ec8\u0ece໐\u0eda\u0f18༚༠༪\u0f35༶\u0f37༸\u0f39༺\u0f3e\u0f48ཉཪ\u0f71྅\u0f86ྌ\u0f90\u0f96\u0f97\u0f98\u0f99\u0fae\u0fb1\u0fb8\u0fb9\u0fbaႠ\u10c6აჷᄀᄁᄂᄄᄅᄈᄉᄊᄋᄍᄎᄓᄼᄽᄾᄿᅀᅁᅌᅍᅎᅏᅐᅑᅔᅖᅙᅚᅟᅢᅣᅤᅥᅦᅧᅨᅩᅪᅭᅯᅲᅴᅵᅶᆞᆟᆨᆩᆫᆬᆮᆰᆷᆹᆺᆻᆼᇃᇫᇬᇰᇱᇹᇺḀẜẠỺἀ\u1f16Ἐ\u1f1eἠ\u1f46Ὀ\u1f4eὐ\u1f58Ὑ\u1f5aὛ\u1f5cὝ\u1f5eὟ\u1f7eᾀ\u1fb5ᾶ\u1fbdι\u1fbfῂ\u1fc5ῆ\u1fcdῐ\u1fd4ῖ\u1fdcῠ\u1fedῲ\u1ff5ῶ\u1ffd\u20d0\u20dd\u20e1\u20e2Ω℧Kℬ℮ℯↀↃ々〆〇〈〡〰〱〶ぁゕ\u3099\u309bゝゟァ・ーヿㄅㄭ一龦가\ud7a4" },
			new string[] { "_xmlD", "0:٠٪۰ۺ०॰০ৰ੦\u0a70૦૰୦୰௧௰౦\u0c70೦\u0cf0൦൰๐๚໐\u0eda༠༪၀၊፩፲០\u17ea᠐\u181a０：" },
			new string[] { "_xmlI", ":;A[_`a{À×Ø÷øĲĴĿŁŉŊſƀǄǍǱǴǶǺȘɐʩʻ\u02c2Ά·Έ\u038bΌ\u038dΎ\u03a2ΣϏϐϗϚϛϜϝϞϟϠϡϢϴЁЍЎѐёѝў҂ҐӅӇӉӋӍӐӬӮӶӸӺԱ\u0557ՙ՚աևא\u05ebװ׳ءػف\u064bٱڸںڿۀۏې۔ە\u06d6ۥ\u06e7अ\u093aऽ\u093eक़\u0962অ\u098dএ\u0991ও\u09a9প\u09b1ল\u09b3শ\u09baড়\u09deয়\u09e2ৰ৲ਅ\u0a0bਏ\u0a11ਓ\u0a29ਪ\u0a31ਲ\u0a34ਵ\u0a37ਸ\u0a3aਖ਼\u0a5dਫ਼\u0a5fੲ\u0a75અઌઍ\u0a8eએ\u0a92ઓ\u0aa9પ\u0ab1લ\u0ab4વ\u0abaઽ\u0abeૠૡଅ\u0b0dଏ\u0b11ଓ\u0b29ପ\u0b31ଲ\u0b34ଶ\u0b3aଽ\u0b3eଡ଼\u0b5eୟ\u0b62அ\u0b8bஎ\u0b91ஒ\u0b96ங\u0b9bஜ\u0b9dஞ\u0ba0ண\u0ba5ந\u0babமஶஷ\u0bbaఅ\u0c0dఎ\u0c11ఒ\u0c29పఴవ\u0c3aౠ\u0c62ಅ\u0c8dಎ\u0c91ಒ\u0ca9ಪ\u0cb4ವ\u0cbaೞ\u0cdfೠ\u0ce2അ\u0d0dഎ\u0d11ഒഩപഺൠ\u0d62กฯะ\u0e31า\u0e34เๆກ\u0e83ຄ\u0e85ງຉຊ\u0e8bຍຎດຘນຠມ\u0ea4ລ\u0ea6ວຨສຬອຯະ\u0eb1າ\u0eb4ຽ\u0ebeເ\u0ec5ཀ\u0f48ཉཪႠ\u10c6აჷᄀᄁᄂᄄᄅᄈᄉᄊᄋᄍᄎᄓᄼᄽᄾᄿᅀᅁᅌᅍᅎᅏᅐᅑᅔᅖᅙᅚᅟᅢᅣᅤᅥᅦᅧᅨᅩᅪᅭᅯᅲᅴᅵᅶᆞᆟᆨᆩᆫᆬᆮᆰᆷᆹᆺᆻᆼᇃᇫᇬᇰᇱᇹᇺḀẜẠỺἀ\u1f16Ἐ\u1f1eἠ\u1f46Ὀ\u1f4eὐ\u1f58Ὑ\u1f5aὛ\u1f5cὝ\u1f5eὟ\u1f7eᾀ\u1fb5ᾶ\u1fbdι\u1fbfῂ\u1fc5ῆ\u1fcdῐ\u1fd4ῖ\u1fdcῠ\u1fedῲ\u1ff5ῶ\u1ffdΩ℧Kℬ℮ℯↀↃ〇〈〡\u302aぁゕァ・ㄅㄭ一龦가\ud7a4" },
			new string[] { "_xmlW", "$%+,0:<?A[^_`{|}~\u007f¢«¬\u00ad®·\u00b8»¼¿ÀȡȢȴɐʮʰ\u02ef\u0300\u0350\u0360ͰʹͶͺͻ\u0384·Έ\u038bΌ\u038dΎ\u03a2ΣϏϐϷЀ\u0487\u0488ӏӐӶӸӺԀԐԱ\u0557ՙ՚աֈ\u0591\u05a2\u05a3\u05ba\u05bb־\u05bf׀\u05c1׃\u05c4\u05c5א\u05ebװ׳ءػـ\u0656٠٪ٮ۔ە\u06dd۞ۮ۰ۿܐܭ\u0730\u074bހ\u07b2\u0901ऄअ\u093a\u093c\u094eॐ\u0955क़।०॰\u0981\u0984অ\u098dএ\u0991ও\u09a9প\u09b1ল\u09b3শ\u09ba\u09bcঽ\u09be\u09c5\u09c7\u09c9\u09cbৎ\u09d7\u09d8ড়\u09deয়\u09e4০৻\u0a02\u0a03ਅ\u0a0bਏ\u0a11ਓ\u0a29ਪ\u0a31ਲ\u0a34ਵ\u0a37ਸ\u0a3a\u0a3c\u0a3d\u0a3e\u0a43\u0a47\u0a49\u0a4b\u0a4eਖ਼\u0a5dਫ਼\u0a5f੦\u0a75\u0a81\u0a84અઌઍ\u0a8eએ\u0a92ઓ\u0aa9પ\u0ab1લ\u0ab4વ\u0aba\u0abc\u0ac6\u0ac7\u0aca\u0acb\u0aceૐ\u0ad1ૠૡ૦૰\u0b01\u0b04ଅ\u0b0dଏ\u0b11ଓ\u0b29ପ\u0b31ଲ\u0b34ଶ\u0b3a\u0b3c\u0b44\u0b47\u0b49\u0b4b\u0b4e\u0b56\u0b58ଡ଼\u0b5eୟ\u0b62୦ୱ\u0b82\u0b84அ\u0b8bஎ\u0b91ஒ\u0b96ங\u0b9bஜ\u0b9dஞ\u0ba0ண\u0ba5ந\u0babமஶஷ\u0bba\u0bbe\u0bc3\u0bc6\u0bc9\u0bca\u0bce\u0bd7\u0bd8௧௳\u0c01\u0c04అ\u0c0dఎ\u0c11ఒ\u0c29పఴవ\u0c3a\u0c3e\u0c45\u0c46\u0c49\u0c4a\u0c4e\u0c55\u0c57ౠ\u0c62౦\u0c70\u0c82಄ಅ\u0c8dಎ\u0c91ಒ\u0ca9ಪ\u0cb4ವ\u0cba\u0cbe\u0cc5\u0cc6\u0cc9\u0cca\u0cce\u0cd5\u0cd7ೞ\u0cdfೠ\u0ce2೦\u0cf0\u0d02ഄഅ\u0d0dഎ\u0d11ഒഩപഺ\u0d3e\u0d44\u0d46\u0d49\u0d4aൎ\u0d57൘ൠ\u0d62൦൰\u0d82\u0d84අ\u0d97ක\u0db2ඳ\u0dbcල\u0dbeව\u0dc7\u0dca\u0dcb\u0dcf\u0dd5\u0dd6\u0dd7\u0dd8\u0de0\u0df2෴ก\u0e3b฿๏๐๚ກ\u0e83ຄ\u0e85ງຉຊ\u0e8bຍຎດຘນຠມ\u0ea4ລ\u0ea6ວຨສຬອ\u0eba\u0ebb\u0ebeເ\u0ec5ໆ\u0ec7\u0ec8\u0ece໐\u0edaໜໞༀ༄༓༺\u0f3e\u0f48ཉཫ\u0f71྅\u0f86ྌ\u0f90\u0f98\u0f99\u0fbd྾\u0fcd࿏࿐ကဢဣဨဩ\u102b\u102c\u1033\u1036\u103a၀၊ၐၚႠ\u10c6აჹᄀᅚᅟᆣᆨᇺሀሇለቇቈ\u1249ቊ\u124eቐ\u1257ቘ\u1259ቚ\u125eበኇኈ\u1289ኊ\u128eነኯኰ\u12b1ኲ\u12b6ኸ\u12bfዀ\u12c1ዂ\u12c6ወዏዐ\u12d7ዘዯደጏጐ\u1311ጒ\u1316ጘጟጠፇፈ\u135b፩\u137dᎠᏵᐁ᙭ᙯᙷᚁ᚛ᚠ᛫ᛮᛱᜀᜍᜎ\u1715ᜠ᜵ᝀ\u1754ᝠ\u176dᝮ\u1771\u1772\u1774ក។ៗ៘៛\u17dd០\u17ea\u180b\u180e᠐\u181aᠠᡸᢀᢪḀẜẠỺἀ\u1f16Ἐ\u1f1eἠ\u1f46Ὀ\u1f4eὐ\u1f58Ὑ\u1f5aὛ\u1f5cὝ\u1f5eὟ\u1f7eᾀ\u1fb5ᾶ\u1fc5ῆ\u1fd4ῖ\u1fdc\u1fdd\u1ff0ῲ\u1ff5ῶ\u1fff⁄⁅⁒⁓⁰\u2072⁴⁽ⁿ₍₠₲\u20d0\u20eb℀℻ℽ⅌⅓ↄ←〈⌫⎴⎷⏏␀␧⑀\u244b①⓿─☔☖☘☙♾⚀⚊✁✅✆✊✌✨✩❌❍❎❏❓❖❗❘❟❡❨❶➕➘➰➱➿⟐⟦⟰⦃⦙⧘⧜⧼⧾⬀⺀\u2e9a⺛\u2ef4⼀\u2fd6⿰⿼〄〈〒〔〠〰〱〽〾\u3040ぁ\u3097\u3099゠ァ・ー\u3100ㄅㄭㄱ\u318f㆐ㆸㇰ㈝㈠㉄㉑㉼㉿㋌㋐㋿㌀㍷㍻㏞㏠㏿㐀䶶一龦ꀀ\ua48d꒐\ua4c7가\ud7a4豈郞侮恵ﬀ\ufb07ﬓ\ufb18יִ\ufb37טּ\ufb3dמּ\ufb3fנּ\ufb42ףּ\ufb45צּ\ufbb2ﯓ﴾ﵐ\ufd90ﶒ\ufdc8ﷰ﷽\ufe00︐\ufe20\ufe24﹢﹣﹤\ufe67﹩﹪ﹰ\ufe75ﹶ\ufefd＄％＋，０：＜？Ａ［\uff3e\uff3f\uff40｛｜｝～｟ｦ\uffbfￂ\uffc8ￊ\uffd0ￒ\uffd8ￚ\uffdd￠\uffe7￨\uffef￼\ufffe" }
		};

		// Token: 0x04000859 RID: 2137
		private const int LowercaseSet = 0;

		// Token: 0x0400085A RID: 2138
		private const int LowercaseAdd = 1;

		// Token: 0x0400085B RID: 2139
		private const int LowercaseBor = 2;

		// Token: 0x0400085C RID: 2140
		private const int LowercaseBad = 3;

		// Token: 0x0400085D RID: 2141
		private static readonly RegexCharClass.LowerCaseMapping[] s_lcTable = new RegexCharClass.LowerCaseMapping[]
		{
			new RegexCharClass.LowerCaseMapping('A', 'Z', 1, 32),
			new RegexCharClass.LowerCaseMapping('À', 'Þ', 1, 32),
			new RegexCharClass.LowerCaseMapping('Ā', 'Į', 2, 0),
			new RegexCharClass.LowerCaseMapping('İ', 'İ', 0, 105),
			new RegexCharClass.LowerCaseMapping('Ĳ', 'Ķ', 2, 0),
			new RegexCharClass.LowerCaseMapping('Ĺ', 'Ň', 3, 0),
			new RegexCharClass.LowerCaseMapping('Ŋ', 'Ŷ', 2, 0),
			new RegexCharClass.LowerCaseMapping('Ÿ', 'Ÿ', 0, 255),
			new RegexCharClass.LowerCaseMapping('Ź', 'Ž', 3, 0),
			new RegexCharClass.LowerCaseMapping('Ɓ', 'Ɓ', 0, 595),
			new RegexCharClass.LowerCaseMapping('Ƃ', 'Ƅ', 2, 0),
			new RegexCharClass.LowerCaseMapping('Ɔ', 'Ɔ', 0, 596),
			new RegexCharClass.LowerCaseMapping('Ƈ', 'Ƈ', 0, 392),
			new RegexCharClass.LowerCaseMapping('Ɖ', 'Ɗ', 1, 205),
			new RegexCharClass.LowerCaseMapping('Ƌ', 'Ƌ', 0, 396),
			new RegexCharClass.LowerCaseMapping('Ǝ', 'Ǝ', 0, 477),
			new RegexCharClass.LowerCaseMapping('Ə', 'Ə', 0, 601),
			new RegexCharClass.LowerCaseMapping('Ɛ', 'Ɛ', 0, 603),
			new RegexCharClass.LowerCaseMapping('Ƒ', 'Ƒ', 0, 402),
			new RegexCharClass.LowerCaseMapping('Ɠ', 'Ɠ', 0, 608),
			new RegexCharClass.LowerCaseMapping('Ɣ', 'Ɣ', 0, 611),
			new RegexCharClass.LowerCaseMapping('Ɩ', 'Ɩ', 0, 617),
			new RegexCharClass.LowerCaseMapping('Ɨ', 'Ɨ', 0, 616),
			new RegexCharClass.LowerCaseMapping('Ƙ', 'Ƙ', 0, 409),
			new RegexCharClass.LowerCaseMapping('Ɯ', 'Ɯ', 0, 623),
			new RegexCharClass.LowerCaseMapping('Ɲ', 'Ɲ', 0, 626),
			new RegexCharClass.LowerCaseMapping('Ɵ', 'Ɵ', 0, 629),
			new RegexCharClass.LowerCaseMapping('Ơ', 'Ƥ', 2, 0),
			new RegexCharClass.LowerCaseMapping('Ƨ', 'Ƨ', 0, 424),
			new RegexCharClass.LowerCaseMapping('Ʃ', 'Ʃ', 0, 643),
			new RegexCharClass.LowerCaseMapping('Ƭ', 'Ƭ', 0, 429),
			new RegexCharClass.LowerCaseMapping('Ʈ', 'Ʈ', 0, 648),
			new RegexCharClass.LowerCaseMapping('Ư', 'Ư', 0, 432),
			new RegexCharClass.LowerCaseMapping('Ʊ', 'Ʋ', 1, 217),
			new RegexCharClass.LowerCaseMapping('Ƴ', 'Ƶ', 3, 0),
			new RegexCharClass.LowerCaseMapping('Ʒ', 'Ʒ', 0, 658),
			new RegexCharClass.LowerCaseMapping('Ƹ', 'Ƹ', 0, 441),
			new RegexCharClass.LowerCaseMapping('Ƽ', 'Ƽ', 0, 445),
			new RegexCharClass.LowerCaseMapping('Ǆ', 'ǅ', 0, 454),
			new RegexCharClass.LowerCaseMapping('Ǉ', 'ǈ', 0, 457),
			new RegexCharClass.LowerCaseMapping('Ǌ', 'ǋ', 0, 460),
			new RegexCharClass.LowerCaseMapping('Ǎ', 'Ǜ', 3, 0),
			new RegexCharClass.LowerCaseMapping('Ǟ', 'Ǯ', 2, 0),
			new RegexCharClass.LowerCaseMapping('Ǳ', 'ǲ', 0, 499),
			new RegexCharClass.LowerCaseMapping('Ǵ', 'Ǵ', 0, 501),
			new RegexCharClass.LowerCaseMapping('Ǻ', 'Ȗ', 2, 0),
			new RegexCharClass.LowerCaseMapping('Ά', 'Ά', 0, 940),
			new RegexCharClass.LowerCaseMapping('Έ', 'Ί', 1, 37),
			new RegexCharClass.LowerCaseMapping('Ό', 'Ό', 0, 972),
			new RegexCharClass.LowerCaseMapping('Ύ', 'Ώ', 1, 63),
			new RegexCharClass.LowerCaseMapping('Α', 'Ϋ', 1, 32),
			new RegexCharClass.LowerCaseMapping('Ϣ', 'Ϯ', 2, 0),
			new RegexCharClass.LowerCaseMapping('Ё', 'Џ', 1, 80),
			new RegexCharClass.LowerCaseMapping('А', 'Я', 1, 32),
			new RegexCharClass.LowerCaseMapping('Ѡ', 'Ҁ', 2, 0),
			new RegexCharClass.LowerCaseMapping('Ґ', 'Ҿ', 2, 0),
			new RegexCharClass.LowerCaseMapping('Ӂ', 'Ӄ', 3, 0),
			new RegexCharClass.LowerCaseMapping('Ӈ', 'Ӈ', 0, 1224),
			new RegexCharClass.LowerCaseMapping('Ӌ', 'Ӌ', 0, 1228),
			new RegexCharClass.LowerCaseMapping('Ӑ', 'Ӫ', 2, 0),
			new RegexCharClass.LowerCaseMapping('Ӯ', 'Ӵ', 2, 0),
			new RegexCharClass.LowerCaseMapping('Ӹ', 'Ӹ', 0, 1273),
			new RegexCharClass.LowerCaseMapping('Ա', 'Ֆ', 1, 48),
			new RegexCharClass.LowerCaseMapping('Ⴀ', 'Ⴥ', 1, 48),
			new RegexCharClass.LowerCaseMapping('Ḁ', 'Ỹ', 2, 0),
			new RegexCharClass.LowerCaseMapping('Ἀ', 'Ἇ', 1, -8),
			new RegexCharClass.LowerCaseMapping('Ἐ', '\u1f1f', 1, -8),
			new RegexCharClass.LowerCaseMapping('Ἠ', 'Ἧ', 1, -8),
			new RegexCharClass.LowerCaseMapping('Ἰ', 'Ἷ', 1, -8),
			new RegexCharClass.LowerCaseMapping('Ὀ', 'Ὅ', 1, -8),
			new RegexCharClass.LowerCaseMapping('Ὑ', 'Ὑ', 0, 8017),
			new RegexCharClass.LowerCaseMapping('Ὓ', 'Ὓ', 0, 8019),
			new RegexCharClass.LowerCaseMapping('Ὕ', 'Ὕ', 0, 8021),
			new RegexCharClass.LowerCaseMapping('Ὗ', 'Ὗ', 0, 8023),
			new RegexCharClass.LowerCaseMapping('Ὠ', 'Ὧ', 1, -8),
			new RegexCharClass.LowerCaseMapping('ᾈ', 'ᾏ', 1, -8),
			new RegexCharClass.LowerCaseMapping('ᾘ', 'ᾟ', 1, -8),
			new RegexCharClass.LowerCaseMapping('ᾨ', 'ᾯ', 1, -8),
			new RegexCharClass.LowerCaseMapping('Ᾰ', 'Ᾱ', 1, -8),
			new RegexCharClass.LowerCaseMapping('Ὰ', 'Ά', 1, -74),
			new RegexCharClass.LowerCaseMapping('ᾼ', 'ᾼ', 0, 8115),
			new RegexCharClass.LowerCaseMapping('Ὲ', 'Ή', 1, -86),
			new RegexCharClass.LowerCaseMapping('ῌ', 'ῌ', 0, 8131),
			new RegexCharClass.LowerCaseMapping('Ῐ', 'Ῑ', 1, -8),
			new RegexCharClass.LowerCaseMapping('Ὶ', 'Ί', 1, -100),
			new RegexCharClass.LowerCaseMapping('Ῠ', 'Ῡ', 1, -8),
			new RegexCharClass.LowerCaseMapping('Ὺ', 'Ύ', 1, -112),
			new RegexCharClass.LowerCaseMapping('Ῥ', 'Ῥ', 0, 8165),
			new RegexCharClass.LowerCaseMapping('Ὸ', 'Ό', 1, -128),
			new RegexCharClass.LowerCaseMapping('Ὼ', 'Ώ', 1, -126),
			new RegexCharClass.LowerCaseMapping('ῼ', 'ῼ', 0, 8179),
			new RegexCharClass.LowerCaseMapping('Ⅰ', 'Ⅿ', 1, 16),
			new RegexCharClass.LowerCaseMapping('Ⓐ', 'ⓐ', 1, 26),
			new RegexCharClass.LowerCaseMapping('Ａ', 'Ｚ', 1, 32)
		};

		// Token: 0x0400085E RID: 2142
		private List<RegexCharClass.SingleRange> _rangelist;

		// Token: 0x0400085F RID: 2143
		private StringBuilder _categories;

		// Token: 0x04000860 RID: 2144
		private bool _canonical;

		// Token: 0x04000861 RID: 2145
		private bool _negate;

		// Token: 0x04000862 RID: 2146
		private RegexCharClass _subtractor;

		// Token: 0x020001FB RID: 507
		private readonly struct LowerCaseMapping
		{
			// Token: 0x06000DE4 RID: 3556 RVA: 0x0003A200 File Offset: 0x00038400
			internal LowerCaseMapping(char chMin, char chMax, int lcOp, int data)
			{
				this.ChMin = chMin;
				this.ChMax = chMax;
				this.LcOp = lcOp;
				this.Data = data;
			}

			// Token: 0x04000863 RID: 2147
			public readonly char ChMin;

			// Token: 0x04000864 RID: 2148
			public readonly char ChMax;

			// Token: 0x04000865 RID: 2149
			public readonly int LcOp;

			// Token: 0x04000866 RID: 2150
			public readonly int Data;
		}

		// Token: 0x020001FC RID: 508
		private sealed class SingleRangeComparer : IComparer<RegexCharClass.SingleRange>
		{
			// Token: 0x06000DE5 RID: 3557 RVA: 0x0000219B File Offset: 0x0000039B
			private SingleRangeComparer()
			{
			}

			// Token: 0x06000DE6 RID: 3558 RVA: 0x0003A21F File Offset: 0x0003841F
			public int Compare(RegexCharClass.SingleRange x, RegexCharClass.SingleRange y)
			{
				return x.First.CompareTo(y.First);
			}

			// Token: 0x04000867 RID: 2151
			public static readonly RegexCharClass.SingleRangeComparer Instance = new RegexCharClass.SingleRangeComparer();
		}

		// Token: 0x020001FD RID: 509
		private readonly struct SingleRange
		{
			// Token: 0x06000DE8 RID: 3560 RVA: 0x0003A23F File Offset: 0x0003843F
			internal SingleRange(char first, char last)
			{
				this.First = first;
				this.Last = last;
			}

			// Token: 0x04000868 RID: 2152
			public readonly char First;

			// Token: 0x04000869 RID: 2153
			public readonly char Last;
		}
	}
}
