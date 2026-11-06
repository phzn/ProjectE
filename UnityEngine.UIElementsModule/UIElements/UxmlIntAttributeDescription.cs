using System;
using System.Globalization;

namespace UnityEngine.UIElements
{
	// Token: 0x020003B1 RID: 945
	public class UxmlIntAttributeDescription : TypedUxmlAttributeDescription<int>
	{
		// Token: 0x06001F80 RID: 8064 RVA: 0x0007827F File Offset: 0x0007647F
		public UxmlIntAttributeDescription()
		{
			base.type = "int";
			base.typeNamespace = "http://www.w3.org/2001/XMLSchema";
			base.defaultValue = 0;
		}

		// Token: 0x17000752 RID: 1874
		// (get) Token: 0x06001F81 RID: 8065 RVA: 0x000782AC File Offset: 0x000764AC
		public override string defaultValueAsString
		{
			get
			{
				return base.defaultValue.ToString(CultureInfo.InvariantCulture.NumberFormat);
			}
		}

		// Token: 0x06001F82 RID: 8066 RVA: 0x000782D8 File Offset: 0x000764D8
		public override int GetValueFromBag(IUxmlAttributes bag, CreationContext cc)
		{
			return base.GetValueFromBag<int>(bag, cc, (string s, int i) => UxmlIntAttributeDescription.ConvertValueToInt(s, i), base.defaultValue);
		}

		// Token: 0x06001F83 RID: 8067 RVA: 0x00078318 File Offset: 0x00076518
		public bool TryGetValueFromBag(IUxmlAttributes bag, CreationContext cc, ref int value)
		{
			return base.TryGetValueFromBag<int>(bag, cc, (string s, int i) => UxmlIntAttributeDescription.ConvertValueToInt(s, i), base.defaultValue, ref value);
		}

		// Token: 0x06001F84 RID: 8068 RVA: 0x00078358 File Offset: 0x00076558
		private static int ConvertValueToInt(string v, int defaultValue)
		{
			int num;
			bool flag = v == null || !int.TryParse(v, out num);
			int num2;
			if (flag)
			{
				num2 = defaultValue;
			}
			else
			{
				num2 = num;
			}
			return num2;
		}
	}
}
