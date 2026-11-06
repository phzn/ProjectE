using System;
using System.Collections.Generic;
using System.Reflection;

namespace UnityEngine.AddressableAssets.Initialization
{
	// Token: 0x02000026 RID: 38
	public static class AddressablesRuntimeProperties
	{
		// Token: 0x060001CE RID: 462 RVA: 0x000081BA File Offset: 0x000063BA
		private static Assembly[] GetAssemblies()
		{
			return AppDomain.CurrentDomain.GetAssemblies();
		}

		// Token: 0x060001CF RID: 463 RVA: 0x000081C6 File Offset: 0x000063C6
		internal static int GetCachedValueCount()
		{
			return AddressablesRuntimeProperties.s_CachedValues.Count;
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x000081D2 File Offset: 0x000063D2
		public static void SetPropertyValue(string name, string val)
		{
			AddressablesRuntimeProperties.s_CachedValues[name] = val;
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x000081E0 File Offset: 0x000063E0
		public static void ClearCachedPropertyValues()
		{
			AddressablesRuntimeProperties.s_CachedValues.Clear();
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x000081EC File Offset: 0x000063EC
		public static string EvaluateProperty(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return string.Empty;
			}
			string text;
			if (AddressablesRuntimeProperties.s_CachedValues.TryGetValue(name, out text))
			{
				return text;
			}
			int num = name.LastIndexOf('.');
			if (num < 0)
			{
				return name;
			}
			string text2 = name.Substring(0, num);
			string text3 = name.Substring(num + 1);
			Assembly[] assemblies = AddressablesRuntimeProperties.GetAssemblies();
			for (int i = 0; i < assemblies.Length; i++)
			{
				Type type = assemblies[i].GetType(text2, false, false);
				if (!(type == null))
				{
					try
					{
						PropertyInfo property = type.GetProperty(text3, BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
						if (property != null)
						{
							object value = property.GetValue(null, null);
							if (value != null)
							{
								AddressablesRuntimeProperties.s_CachedValues.Add(name, value.ToString());
								return value.ToString();
							}
						}
						FieldInfo field = type.GetField(text3, BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
						if (field != null)
						{
							object value2 = field.GetValue(null);
							if (value2 != null)
							{
								AddressablesRuntimeProperties.s_CachedValues.Add(name, value2.ToString());
								return value2.ToString();
							}
						}
					}
					catch (Exception)
					{
					}
				}
			}
			return name;
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x00008314 File Offset: 0x00006514
		public static string EvaluateString(string input)
		{
			return AddressablesRuntimeProperties.EvaluateString(input, '{', '}', new Func<string, string>(AddressablesRuntimeProperties.EvaluateProperty));
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x0000832C File Offset: 0x0000652C
		public static string EvaluateString(string inputString, char startDelimiter, char endDelimiter, Func<string, string> varFunc)
		{
			if (string.IsNullOrEmpty(inputString))
			{
				return string.Empty;
			}
			string text = inputString;
			Stack<string> stack;
			Stack<int> stack2;
			if (!AddressablesRuntimeProperties.s_StaticStacksAreInUse)
			{
				stack = AddressablesRuntimeProperties.s_TokenStack;
				stack2 = AddressablesRuntimeProperties.s_TokenStartStack;
				AddressablesRuntimeProperties.s_StaticStacksAreInUse = true;
			}
			else
			{
				stack = new Stack<string>(32);
				stack2 = new Stack<int>(32);
			}
			stack.Push(inputString);
			int num = inputString.Length;
			char[] array = new char[] { startDelimiter, endDelimiter };
			bool flag = startDelimiter == endDelimiter;
			int i = inputString.IndexOf(startDelimiter);
			int num2 = -2;
			while (i >= 0)
			{
				char c = inputString[i];
				if (c == startDelimiter && (!flag || stack2.Count == 0))
				{
					stack2.Push(i);
					i++;
				}
				else if (c == endDelimiter && stack2.Count > 0)
				{
					int num3 = stack2.Peek();
					string text2 = inputString.Substring(num3 + 1, i - num3 - 1);
					if (num <= i)
					{
						stack.Pop();
					}
					string text3;
					if (stack.Contains(text2))
					{
						text3 = "#ERROR-CyclicToken#";
					}
					else
					{
						text3 = ((varFunc == null) ? string.Empty : varFunc(text2));
						stack.Push(text2);
					}
					i = stack2.Pop();
					num = i + text3.Length + 1;
					if (i > 0)
					{
						int num4 = i + text2.Length + 2;
						if (num4 == inputString.Length)
						{
							inputString = inputString.Substring(0, i) + text3;
						}
						else
						{
							inputString = inputString.Substring(0, i) + text3 + inputString.Substring(num4);
						}
					}
					else
					{
						inputString = text3 + inputString.Substring(i + text2.Length + 2);
					}
				}
				if (num2 == i)
				{
					return "#ERROR-" + text + " contains unmatched delimiters#";
				}
				num2 = i;
				i = inputString.IndexOfAny(array, i);
			}
			stack.Clear();
			stack2.Clear();
			if (stack == AddressablesRuntimeProperties.s_TokenStack)
			{
				AddressablesRuntimeProperties.s_StaticStacksAreInUse = false;
			}
			return inputString;
		}

		// Token: 0x04000077 RID: 119
		private static Stack<string> s_TokenStack = new Stack<string>(32);

		// Token: 0x04000078 RID: 120
		private static Stack<int> s_TokenStartStack = new Stack<int>(32);

		// Token: 0x04000079 RID: 121
		private static bool s_StaticStacksAreInUse = false;

		// Token: 0x0400007A RID: 122
		private static Dictionary<string, string> s_CachedValues = new Dictionary<string, string>();
	}
}
