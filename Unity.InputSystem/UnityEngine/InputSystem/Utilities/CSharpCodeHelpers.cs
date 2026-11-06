using System;
using System.Linq;
using System.Text;

namespace UnityEngine.InputSystem.Utilities
{
	// Token: 0x0200012F RID: 303
	internal static class CSharpCodeHelpers
	{
		// Token: 0x060010FF RID: 4351 RVA: 0x00051530 File Offset: 0x0004F730
		public static bool IsProperIdentifier(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return false;
			}
			if (char.IsDigit(name[0]))
			{
				return false;
			}
			foreach (char c in name)
			{
				if (!char.IsLetterOrDigit(c) && c != '_')
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001100 RID: 4352 RVA: 0x00051580 File Offset: 0x0004F780
		public static bool IsEmptyOrProperIdentifier(string name)
		{
			return string.IsNullOrEmpty(name) || CSharpCodeHelpers.IsProperIdentifier(name);
		}

		// Token: 0x06001101 RID: 4353 RVA: 0x00051592 File Offset: 0x0004F792
		public static bool IsEmptyOrProperNamespaceName(string name)
		{
			return string.IsNullOrEmpty(name) || name.Split('.', StringSplitOptions.None).All<string>(new Func<string, bool>(CSharpCodeHelpers.IsProperIdentifier));
		}

		// Token: 0x06001102 RID: 4354 RVA: 0x000515B8 File Offset: 0x0004F7B8
		public static string MakeIdentifier(string name, string suffix = "")
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name");
			}
			if (char.IsDigit(name[0]))
			{
				name = "_" + name;
			}
			bool flag = false;
			foreach (char c in name)
			{
				if (!char.IsLetterOrDigit(c) && c != '_')
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (char c2 in name)
				{
					if (char.IsLetterOrDigit(c2) || c2 == '_')
					{
						stringBuilder.Append(c2);
					}
				}
				name = stringBuilder.ToString();
			}
			return name + suffix;
		}

		// Token: 0x06001103 RID: 4355 RVA: 0x00051670 File Offset: 0x0004F870
		public static string MakeTypeName(string name, string suffix = "")
		{
			string text = CSharpCodeHelpers.MakeIdentifier(name, suffix);
			if (char.IsLower(text[0]))
			{
				text = char.ToUpperInvariant(text[0]).ToString() + text.Substring(1);
			}
			return text;
		}
	}
}
