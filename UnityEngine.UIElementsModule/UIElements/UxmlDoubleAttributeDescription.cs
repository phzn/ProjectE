using System;
using System.Globalization;

namespace UnityEngine.UIElements
{
	// Token: 0x020003AF RID: 943
	public class UxmlDoubleAttributeDescription : TypedUxmlAttributeDescription<double>
	{
		// Token: 0x06001F77 RID: 8055 RVA: 0x00078153 File Offset: 0x00076353
		public UxmlDoubleAttributeDescription()
		{
			base.type = "double";
			base.typeNamespace = "http://www.w3.org/2001/XMLSchema";
			base.defaultValue = 0.0;
		}

		// Token: 0x17000751 RID: 1873
		// (get) Token: 0x06001F78 RID: 8056 RVA: 0x00078188 File Offset: 0x00076388
		public override string defaultValueAsString
		{
			get
			{
				return base.defaultValue.ToString(CultureInfo.InvariantCulture.NumberFormat);
			}
		}

		// Token: 0x06001F79 RID: 8057 RVA: 0x000781B4 File Offset: 0x000763B4
		public override double GetValueFromBag(IUxmlAttributes bag, CreationContext cc)
		{
			return base.GetValueFromBag<double>(bag, cc, (string s, double d) => UxmlDoubleAttributeDescription.ConvertValueToDouble(s, d), base.defaultValue);
		}

		// Token: 0x06001F7A RID: 8058 RVA: 0x000781F4 File Offset: 0x000763F4
		public bool TryGetValueFromBag(IUxmlAttributes bag, CreationContext cc, ref double value)
		{
			return base.TryGetValueFromBag<double>(bag, cc, (string s, double d) => UxmlDoubleAttributeDescription.ConvertValueToDouble(s, d), base.defaultValue, ref value);
		}

		// Token: 0x06001F7B RID: 8059 RVA: 0x00078234 File Offset: 0x00076434
		private static double ConvertValueToDouble(string v, double defaultValue)
		{
			double num;
			bool flag = v == null || !double.TryParse(v, NumberStyles.Float, CultureInfo.InvariantCulture, out num);
			double num2;
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
