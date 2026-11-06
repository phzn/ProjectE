using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using DA_Assets.Extensions;
using Newtonsoft.Json;

namespace DA_Assets.FCU
{
	// Token: 0x02000010 RID: 16
	public class DAFormatter
	{
		// Token: 0x06000077 RID: 119 RVA: 0x00002E2C File Offset: 0x0000102C
		private static List<string> GetJsonPropertyNames<T>()
		{
			List<string> list = new List<string>();
			FieldInfo[] fields = typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public);
			PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
			foreach (FieldInfo fieldInfo in fields)
			{
				JsonPropertyAttribute customAttribute = fieldInfo.GetCustomAttribute<JsonPropertyAttribute>();
				if (customAttribute != null)
				{
					list.Add(customAttribute.PropertyName ?? fieldInfo.Name);
				}
			}
			foreach (PropertyInfo propertyInfo in properties)
			{
				JsonPropertyAttribute customAttribute2 = propertyInfo.GetCustomAttribute<JsonPropertyAttribute>();
				if (customAttribute2 != null)
				{
					list.Add(customAttribute2.PropertyName ?? propertyInfo.Name);
				}
			}
			return list;
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00002ED9 File Offset: 0x000010D9
		private static string Repeat(int n)
		{
			return new string(' ', n * 4);
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00002EE8 File Offset: 0x000010E8
		public static JFResult Format<T>(string str)
		{
			JFResult jfresult = default(JFResult);
			if (str.IsEmpty())
			{
				return new JFResult
				{
					Json = string.Empty,
					IsValid = false,
					MatchTargetType = false
				};
			}
			foreach (string text in DAFormatter.GetJsonPropertyNames<T>())
			{
				if (str.Contains("\"" + text + "\""))
				{
					jfresult.MatchTargetType = true;
					break;
				}
			}
			bool flag = false;
			bool flag2 = false;
			int num = 0;
			bool flag3 = false;
			StringBuilder stringBuilder = new StringBuilder();
			int i = 0;
			while (i < str.Length)
			{
				char c = str[i];
				if (c <= ':')
				{
					if (c != '"')
					{
						if (c != ',')
						{
							if (c != ':')
							{
								goto IL_01E7;
							}
							stringBuilder.Append(c);
							if (!flag3)
							{
								stringBuilder.Append(" ");
							}
						}
						else
						{
							stringBuilder.Append(c);
							if (!flag3)
							{
								stringBuilder.AppendLine();
								stringBuilder.Append(DAFormatter.Repeat(num));
							}
						}
					}
					else
					{
						stringBuilder.Append(c);
						bool flag4 = false;
						int num2 = i;
						while (num2 > 0 && str[--num2] == '\\')
						{
							flag4 = !flag4;
						}
						if (!flag4)
						{
							flag3 = !flag3;
						}
					}
				}
				else
				{
					if (c <= ']')
					{
						if (c != '[')
						{
							if (c != ']')
							{
								goto IL_01E7;
							}
							goto IL_0132;
						}
					}
					else if (c != '{')
					{
						if (c != '}')
						{
							goto IL_01E7;
						}
						goto IL_0132;
					}
					if (c == '{')
					{
						flag = true;
					}
					stringBuilder.Append(c);
					if (!flag3)
					{
						stringBuilder.AppendLine();
						stringBuilder.Append(DAFormatter.Repeat(++num));
						goto IL_01F1;
					}
					goto IL_01F1;
					IL_0132:
					if (c == '}')
					{
						flag2 = true;
					}
					if (!flag3)
					{
						stringBuilder.AppendLine();
						stringBuilder.Append(DAFormatter.Repeat(--num));
					}
					stringBuilder.Append(c);
				}
				IL_01F1:
				i++;
				continue;
				IL_01E7:
				stringBuilder.Append(c);
				goto IL_01F1;
			}
			jfresult.Json = stringBuilder.ToString();
			jfresult.IsValid = flag && flag2;
			return jfresult;
		}

		// Token: 0x04000058 RID: 88
		private const int indentLength = 4;
	}
}
