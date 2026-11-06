using System;
using System.Globalization;

namespace UnityEngine.UIElements
{
	// Token: 0x020003B5 RID: 949
	public class UxmlUnsignedLongAttributeDescription : TypedUxmlAttributeDescription<ulong>
	{
		// Token: 0x06001F92 RID: 8082 RVA: 0x000784B1 File Offset: 0x000766B1
		public UxmlUnsignedLongAttributeDescription()
		{
			base.type = "unsignedLong";
			base.typeNamespace = "http://www.w3.org/2001/XMLSchema";
			base.defaultValue = 0UL;
		}

		// Token: 0x17000754 RID: 1876
		// (get) Token: 0x06001F93 RID: 8083 RVA: 0x000784DC File Offset: 0x000766DC
		public override string defaultValueAsString
		{
			get
			{
				return base.defaultValue.ToString(CultureInfo.InvariantCulture.NumberFormat);
			}
		}

		// Token: 0x06001F94 RID: 8084 RVA: 0x00078508 File Offset: 0x00076708
		public override ulong GetValueFromBag(IUxmlAttributes bag, CreationContext cc)
		{
			return base.GetValueFromBag<ulong>(bag, cc, (string s, ulong l) => UxmlUnsignedLongAttributeDescription.ConvertValueToUlong(s, l), base.defaultValue);
		}

		// Token: 0x06001F95 RID: 8085 RVA: 0x00078548 File Offset: 0x00076748
		public bool TryGetValueFromBag(IUxmlAttributes bag, CreationContext cc, ref ulong value)
		{
			return base.TryGetValueFromBag<ulong>(bag, cc, (string s, ulong l) => UxmlUnsignedLongAttributeDescription.ConvertValueToUlong(s, l), base.defaultValue, ref value);
		}

		// Token: 0x06001F96 RID: 8086 RVA: 0x00078588 File Offset: 0x00076788
		private static ulong ConvertValueToUlong(string v, ulong defaultValue)
		{
			ulong num;
			bool flag = v == null || !ulong.TryParse(v, out num);
			ulong num2;
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
