using System;
using System.Collections.Generic;
using System.Globalization;

namespace UnityEngine.UIElements
{
	// Token: 0x020003BE RID: 958
	public class UxmlEnumAttributeDescription<T> : TypedUxmlAttributeDescription<T> where T : struct, IConvertible
	{
		// Token: 0x06001FBD RID: 8125 RVA: 0x00078AA0 File Offset: 0x00076CA0
		public UxmlEnumAttributeDescription()
		{
			bool flag = !typeof(T).IsEnum;
			if (flag)
			{
				throw new ArgumentException("T must be an enumerated type");
			}
			base.type = "string";
			base.typeNamespace = "http://www.w3.org/2001/XMLSchema";
			base.defaultValue = new T();
			UxmlEnumeration uxmlEnumeration = new UxmlEnumeration();
			List<string> list = new List<string>();
			foreach (object obj in Enum.GetValues(typeof(T)))
			{
				T t = (T)((object)obj);
				list.Add(t.ToString(CultureInfo.InvariantCulture));
			}
			uxmlEnumeration.values = list;
			base.restriction = uxmlEnumeration;
		}

		// Token: 0x17000759 RID: 1881
		// (get) Token: 0x06001FBE RID: 8126 RVA: 0x00078B88 File Offset: 0x00076D88
		public override string defaultValueAsString
		{
			get
			{
				T defaultValue = base.defaultValue;
				return defaultValue.ToString(CultureInfo.InvariantCulture.NumberFormat);
			}
		}

		// Token: 0x06001FBF RID: 8127 RVA: 0x00078BB8 File Offset: 0x00076DB8
		public override T GetValueFromBag(IUxmlAttributes bag, CreationContext cc)
		{
			return base.GetValueFromBag<T>(bag, cc, (string s, T convertible) => UxmlEnumAttributeDescription<T>.ConvertValueToEnum<T>(s, convertible), base.defaultValue);
		}

		// Token: 0x06001FC0 RID: 8128 RVA: 0x00078BF8 File Offset: 0x00076DF8
		public bool TryGetValueFromBag(IUxmlAttributes bag, CreationContext cc, ref T value)
		{
			return base.TryGetValueFromBag<T>(bag, cc, (string s, T convertible) => UxmlEnumAttributeDescription<T>.ConvertValueToEnum<T>(s, convertible), base.defaultValue, ref value);
		}

		// Token: 0x06001FC1 RID: 8129 RVA: 0x00078C38 File Offset: 0x00076E38
		private static U ConvertValueToEnum<U>(string v, U defaultValue) where U : struct
		{
			try
			{
				bool flag = string.IsNullOrEmpty(v);
				if (flag)
				{
					return defaultValue;
				}
				return (U)((object)Enum.Parse(typeof(U), v, true));
			}
			catch (ArgumentException)
			{
				Debug.LogError(UxmlEnumAttributeDescription<T>.GetEnumNameErrorMessage(v, typeof(U)));
			}
			catch (OverflowException)
			{
				Debug.LogError(UxmlEnumAttributeDescription<T>.GetEnumRangeErrorMessage(v, typeof(U)));
			}
			return defaultValue;
		}

		// Token: 0x06001FC2 RID: 8130 RVA: 0x00078CC8 File Offset: 0x00076EC8
		private static string GetEnumNameErrorMessage(string v, Type enumType)
		{
			return string.Concat(new string[]
			{
				"The ",
				enumType.Name,
				" enum does not contain the value `",
				v,
				"`. Value must be in range [",
				string.Join(" | ", Enum.GetNames(enumType)),
				"]."
			});
		}

		// Token: 0x06001FC3 RID: 8131 RVA: 0x00078D28 File Offset: 0x00076F28
		private static string GetEnumRangeErrorMessage(string v, Type enumType)
		{
			return v + " is outside of the range of possible values for the " + enumType.Name + " enum.";
		}
	}
}
