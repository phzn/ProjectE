using System;
using System.Collections.Generic;
using System.Reflection;

namespace UnityEngine.InputSystem.Utilities
{
	// Token: 0x0200013B RID: 315
	public struct NamedValue : IEquatable<NamedValue>
	{
		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x0600117C RID: 4476 RVA: 0x00053E6C File Offset: 0x0005206C
		// (set) Token: 0x0600117D RID: 4477 RVA: 0x00053E74 File Offset: 0x00052074
		public string name { readonly get; set; }

		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x0600117E RID: 4478 RVA: 0x00053E7D File Offset: 0x0005207D
		// (set) Token: 0x0600117F RID: 4479 RVA: 0x00053E85 File Offset: 0x00052085
		public PrimitiveValue value { readonly get; set; }

		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x06001180 RID: 4480 RVA: 0x00053E90 File Offset: 0x00052090
		public TypeCode type
		{
			get
			{
				return this.value.type;
			}
		}

		// Token: 0x06001181 RID: 4481 RVA: 0x00053EAC File Offset: 0x000520AC
		public NamedValue ConvertTo(TypeCode type)
		{
			return new NamedValue
			{
				name = this.name,
				value = this.value.ConvertTo(type)
			};
		}

		// Token: 0x06001182 RID: 4482 RVA: 0x00053EE8 File Offset: 0x000520E8
		public static NamedValue From<TValue>(string name, TValue value) where TValue : struct
		{
			return new NamedValue
			{
				name = name,
				value = PrimitiveValue.From<TValue>(value)
			};
		}

		// Token: 0x06001183 RID: 4483 RVA: 0x00053F13 File Offset: 0x00052113
		public override string ToString()
		{
			return string.Format("{0}={1}", this.name, this.value);
		}

		// Token: 0x06001184 RID: 4484 RVA: 0x00053F30 File Offset: 0x00052130
		public bool Equals(NamedValue other)
		{
			return string.Equals(this.name, other.name, StringComparison.InvariantCultureIgnoreCase) && this.value == other.value;
		}

		// Token: 0x06001185 RID: 4485 RVA: 0x00053F5C File Offset: 0x0005215C
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj is NamedValue)
			{
				NamedValue namedValue = (NamedValue)obj;
				return this.Equals(namedValue);
			}
			return false;
		}

		// Token: 0x06001186 RID: 4486 RVA: 0x00053F88 File Offset: 0x00052188
		public override int GetHashCode()
		{
			return (((this.name != null) ? this.name.GetHashCode() : 0) * 397) ^ this.value.GetHashCode();
		}

		// Token: 0x06001187 RID: 4487 RVA: 0x00053FC6 File Offset: 0x000521C6
		public static bool operator ==(NamedValue left, NamedValue right)
		{
			return left.Equals(right);
		}

		// Token: 0x06001188 RID: 4488 RVA: 0x00053FD0 File Offset: 0x000521D0
		public static bool operator !=(NamedValue left, NamedValue right)
		{
			return !left.Equals(right);
		}

		// Token: 0x06001189 RID: 4489 RVA: 0x00053FE0 File Offset: 0x000521E0
		public static NamedValue[] ParseMultiple(string parameterString)
		{
			if (parameterString == null)
			{
				throw new ArgumentNullException("parameterString");
			}
			parameterString = parameterString.Trim();
			if (string.IsNullOrEmpty(parameterString))
			{
				return null;
			}
			int num = parameterString.CountOccurrences(","[0]) + 1;
			NamedValue[] array = new NamedValue[num];
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				NamedValue namedValue = NamedValue.ParseParameter(parameterString, ref num2);
				array[i] = namedValue;
			}
			return array;
		}

		// Token: 0x0600118A RID: 4490 RVA: 0x0005404C File Offset: 0x0005224C
		public static NamedValue Parse(string str)
		{
			int num = 0;
			return NamedValue.ParseParameter(str, ref num);
		}

		// Token: 0x0600118B RID: 4491 RVA: 0x00054064 File Offset: 0x00052264
		private static NamedValue ParseParameter(string parameterString, ref int index)
		{
			NamedValue namedValue = default(NamedValue);
			int length = parameterString.Length;
			while (index < length && char.IsWhiteSpace(parameterString[index]))
			{
				index++;
			}
			int num = index;
			while (index < length)
			{
				char c = parameterString[index];
				if (c == '=' || c == ","[0] || char.IsWhiteSpace(c))
				{
					break;
				}
				index++;
			}
			namedValue.name = parameterString.Substring(num, index - num);
			while (index < length && char.IsWhiteSpace(parameterString[index]))
			{
				index++;
			}
			if (index == length || parameterString[index] != '=')
			{
				namedValue.value = true;
			}
			else
			{
				index++;
				while (index < length && char.IsWhiteSpace(parameterString[index]))
				{
					index++;
				}
				int num2 = index;
				while (index < length && parameterString[index] != ","[0] && !char.IsWhiteSpace(parameterString[index]))
				{
					index++;
				}
				string text = parameterString.Substring(num2, index - num2);
				namedValue.value = PrimitiveValue.FromString(text);
			}
			if (index < length && parameterString[index] == ","[0])
			{
				index++;
			}
			return namedValue;
		}

		// Token: 0x0600118C RID: 4492 RVA: 0x000541B4 File Offset: 0x000523B4
		public void ApplyToObject(object instance)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			Type type = instance.GetType();
			FieldInfo field = type.GetField(this.name, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (field == null)
			{
				throw new ArgumentException(string.Concat(new string[] { "Cannot find public field '", this.name, "' in '", type.Name, "' (while trying to apply parameter)" }), "instance");
			}
			TypeCode typeCode = Type.GetTypeCode(field.FieldType);
			field.SetValue(instance, this.value.ConvertTo(typeCode).ToObject());
		}

		// Token: 0x0600118D RID: 4493 RVA: 0x00054258 File Offset: 0x00052458
		public static void ApplyAllToObject<TParameterList>(object instance, TParameterList parameters) where TParameterList : IEnumerable<NamedValue>
		{
			foreach (NamedValue namedValue in parameters)
			{
				namedValue.ApplyToObject(instance);
			}
		}

		// Token: 0x04000717 RID: 1815
		public const string Separator = ",";
	}
}
