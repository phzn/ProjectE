using System;
using System.Text.RegularExpressions;

namespace UnityEngine.UIElements.StyleSheets
{
	// Token: 0x02000489 RID: 1161
	internal static class CSSSpec
	{
		// Token: 0x0600245A RID: 9306 RVA: 0x0009706C File Offset: 0x0009526C
		public static int GetSelectorSpecificity(string selector)
		{
			int num = 0;
			StyleSelectorPart[] array;
			bool flag = CSSSpec.ParseSelector(selector, out array);
			if (flag)
			{
				num = CSSSpec.GetSelectorSpecificity(array);
			}
			return num;
		}

		// Token: 0x0600245B RID: 9307 RVA: 0x00097098 File Offset: 0x00095298
		public static int GetSelectorSpecificity(StyleSelectorPart[] parts)
		{
			int num = 1;
			for (int i = 0; i < parts.Length; i++)
			{
				switch (parts[i].type)
				{
				case StyleSelectorType.Type:
					num++;
					break;
				case StyleSelectorType.Class:
				case StyleSelectorType.PseudoClass:
					num += 10;
					break;
				case StyleSelectorType.RecursivePseudoClass:
					throw new ArgumentException("Recursive pseudo classes are not supported");
				case StyleSelectorType.ID:
					num += 100;
					break;
				}
			}
			return num;
		}

		// Token: 0x0600245C RID: 9308 RVA: 0x00097114 File Offset: 0x00095314
		public static bool ParseSelector(string selector, out StyleSelectorPart[] parts)
		{
			MatchCollection matchCollection = CSSSpec.rgx.Matches(selector);
			int count = matchCollection.Count;
			bool flag = count < 1;
			bool flag2;
			if (flag)
			{
				parts = null;
				flag2 = false;
			}
			else
			{
				parts = new StyleSelectorPart[count];
				for (int i = 0; i < count; i++)
				{
					Match match = matchCollection[i];
					StyleSelectorType styleSelectorType = StyleSelectorType.Unknown;
					string text = string.Empty;
					bool flag3 = !string.IsNullOrEmpty(match.Groups["wildcard"].Value);
					if (flag3)
					{
						text = "*";
						styleSelectorType = StyleSelectorType.Wildcard;
					}
					else
					{
						bool flag4 = !string.IsNullOrEmpty(match.Groups["id"].Value);
						if (flag4)
						{
							text = match.Groups["id"].Value.Substring(1);
							styleSelectorType = StyleSelectorType.ID;
						}
						else
						{
							bool flag5 = !string.IsNullOrEmpty(match.Groups["class"].Value);
							if (flag5)
							{
								text = match.Groups["class"].Value.Substring(1);
								styleSelectorType = StyleSelectorType.Class;
							}
							else
							{
								bool flag6 = !string.IsNullOrEmpty(match.Groups["pseudoclass"].Value);
								if (flag6)
								{
									string value = match.Groups["param"].Value;
									bool flag7 = !string.IsNullOrEmpty(value);
									if (flag7)
									{
										text = value;
										styleSelectorType = StyleSelectorType.RecursivePseudoClass;
									}
									else
									{
										text = match.Groups["pseudoclass"].Value.Substring(1);
										styleSelectorType = StyleSelectorType.PseudoClass;
									}
								}
								else
								{
									bool flag8 = !string.IsNullOrEmpty(match.Groups["type"].Value);
									if (flag8)
									{
										text = match.Groups["type"].Value;
										styleSelectorType = StyleSelectorType.Type;
									}
								}
							}
						}
					}
					parts[i] = new StyleSelectorPart
					{
						type = styleSelectorType,
						value = text
					};
				}
				flag2 = true;
			}
			return flag2;
		}

		// Token: 0x0400116D RID: 4461
		private static readonly Regex rgx = new Regex("(?<id>#[-]?\\w[\\w-]*)|(?<class>\\.[\\w-]+)|(?<pseudoclass>:[\\w-]+(\\((?<param>.+)\\))?)|(?<type>([^\\-]\\w+|\\w+))|(?<wildcard>\\*)|\\s+", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		// Token: 0x0400116E RID: 4462
		private const int typeSelectorWeight = 1;

		// Token: 0x0400116F RID: 4463
		private const int classSelectorWeight = 10;

		// Token: 0x04001170 RID: 4464
		private const int idSelectorWeight = 100;
	}
}
