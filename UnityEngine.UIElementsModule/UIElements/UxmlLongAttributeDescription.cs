using System;
using System.Globalization;

namespace UnityEngine.UIElements
{
	// Token: 0x020003B7 RID: 951
	public class UxmlLongAttributeDescription : TypedUxmlAttributeDescription<long>
	{
		// Token: 0x06001F9B RID: 8091 RVA: 0x000785C9 File Offset: 0x000767C9
		public UxmlLongAttributeDescription()
		{
			base.type = "long";
			base.typeNamespace = "http://www.w3.org/2001/XMLSchema";
			base.defaultValue = 0L;
		}

		// Token: 0x17000755 RID: 1877
		// (get) Token: 0x06001F9C RID: 8092 RVA: 0x000785F4 File Offset: 0x000767F4
		public override string defaultValueAsString
		{
			get
			{
				return base.defaultValue.ToString(CultureInfo.InvariantCulture.NumberFormat);
			}
		}

		// Token: 0x06001F9D RID: 8093 RVA: 0x00078620 File Offset: 0x00076820
		public override long GetValueFromBag(IUxmlAttributes bag, CreationContext cc)
		{
			return base.GetValueFromBag<long>(bag, cc, (string s, long l) => UxmlLongAttributeDescription.ConvertValueToLong(s, l), base.defaultValue);
		}

		// Token: 0x06001F9E RID: 8094 RVA: 0x00078660 File Offset: 0x00076860
		public bool TryGetValueFromBag(IUxmlAttributes bag, CreationContext cc, ref long value)
		{
			return base.TryGetValueFromBag<long>(bag, cc, (string s, long l) => UxmlLongAttributeDescription.ConvertValueToLong(s, l), base.defaultValue, ref value);
		}

		// Token: 0x06001F9F RID: 8095 RVA: 0x000786A0 File Offset: 0x000768A0
		private static long ConvertValueToLong(string v, long defaultValue)
		{
			long num;
			bool flag = v == null || !long.TryParse(v, out num);
			long num2;
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
