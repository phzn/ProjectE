using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine.InputSystem.Utilities
{
	// Token: 0x0200013A RID: 314
	public struct NameAndParameters
	{
		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x06001172 RID: 4466 RVA: 0x00053B98 File Offset: 0x00051D98
		// (set) Token: 0x06001173 RID: 4467 RVA: 0x00053BA0 File Offset: 0x00051DA0
		public string name { readonly get; set; }

		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x06001174 RID: 4468 RVA: 0x00053BA9 File Offset: 0x00051DA9
		// (set) Token: 0x06001175 RID: 4469 RVA: 0x00053BB1 File Offset: 0x00051DB1
		public ReadOnlyArray<NamedValue> parameters { readonly get; set; }

		// Token: 0x06001176 RID: 4470 RVA: 0x00053BBC File Offset: 0x00051DBC
		public override string ToString()
		{
			if (this.parameters.Count == 0)
			{
				return this.name;
			}
			string text = string.Join(",", this.parameters.Select<NamedValue, string>((NamedValue x) => x.ToString()).ToArray<string>());
			return this.name + "(" + text + ")";
		}

		// Token: 0x06001177 RID: 4471 RVA: 0x00053C38 File Offset: 0x00051E38
		public static IEnumerable<NameAndParameters> ParseMultiple(string text)
		{
			List<NameAndParameters> list = null;
			if (!NameAndParameters.ParseMultiple(text, ref list))
			{
				return Enumerable.Empty<NameAndParameters>();
			}
			return list;
		}

		// Token: 0x06001178 RID: 4472 RVA: 0x00053C58 File Offset: 0x00051E58
		internal static bool ParseMultiple(string text, ref List<NameAndParameters> list)
		{
			text = text.Trim();
			if (string.IsNullOrEmpty(text))
			{
				return false;
			}
			if (list == null)
			{
				list = new List<NameAndParameters>();
			}
			else
			{
				list.Clear();
			}
			int i = 0;
			int length = text.Length;
			while (i < length)
			{
				list.Add(NameAndParameters.ParseNameAndParameters(text, ref i, false));
			}
			return true;
		}

		// Token: 0x06001179 RID: 4473 RVA: 0x00053CAC File Offset: 0x00051EAC
		internal static string ParseName(string text)
		{
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}
			int num = 0;
			return NameAndParameters.ParseNameAndParameters(text, ref num, true).name;
		}

		// Token: 0x0600117A RID: 4474 RVA: 0x00053CDC File Offset: 0x00051EDC
		public static NameAndParameters Parse(string text)
		{
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}
			int num = 0;
			return NameAndParameters.ParseNameAndParameters(text, ref num, false);
		}

		// Token: 0x0600117B RID: 4475 RVA: 0x00053D04 File Offset: 0x00051F04
		private static NameAndParameters ParseNameAndParameters(string text, ref int index, bool nameOnly = false)
		{
			int length = text.Length;
			while (index < length && char.IsWhiteSpace(text[index]))
			{
				index++;
			}
			int num = index;
			while (index < length)
			{
				char c = text[index];
				if (c == '(' || c == ","[0] || char.IsWhiteSpace(c))
				{
					break;
				}
				index++;
			}
			if (index - num == 0)
			{
				throw new ArgumentException(string.Format("Expecting name at position {0} in '{1}'", num, text), "text");
			}
			string text2 = text.Substring(num, index - num);
			if (nameOnly)
			{
				return new NameAndParameters
				{
					name = text2
				};
			}
			while (index < length && char.IsWhiteSpace(text[index]))
			{
				index++;
			}
			NamedValue[] array = null;
			if (index < length && text[index] == '(')
			{
				index++;
				int num2 = text.IndexOf(')', index);
				if (num2 == -1)
				{
					throw new ArgumentException(string.Format("Expecting ')' after '(' at position {0} in '{1}'", index, text), "text");
				}
				array = NamedValue.ParseMultiple(text.Substring(index, num2 - index));
				index = num2 + 1;
			}
			if (index < length && (text[index] == ',' || text[index] == ';'))
			{
				index++;
			}
			return new NameAndParameters
			{
				name = text2,
				parameters = new ReadOnlyArray<NamedValue>(array)
			};
		}
	}
}
