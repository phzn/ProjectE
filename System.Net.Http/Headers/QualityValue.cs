using System;
using System.Collections.Generic;
using System.Globalization;

namespace System.Net.Http.Headers
{
	// Token: 0x02000062 RID: 98
	internal static class QualityValue
	{
		// Token: 0x06000377 RID: 887 RVA: 0x0000BF80 File Offset: 0x0000A180
		public static double? GetValue(List<NameValueHeaderValue> parameters)
		{
			if (parameters == null)
			{
				return null;
			}
			NameValueHeaderValue nameValueHeaderValue = parameters.Find((NameValueHeaderValue l) => string.Equals(l.Name, "q", StringComparison.OrdinalIgnoreCase));
			if (nameValueHeaderValue == null)
			{
				return null;
			}
			double num;
			if (!double.TryParse(nameValueHeaderValue.Value, NumberStyles.Number, NumberFormatInfo.InvariantInfo, out num))
			{
				return null;
			}
			return new double?(num);
		}

		// Token: 0x06000378 RID: 888 RVA: 0x0000BFF4 File Offset: 0x0000A1F4
		public static void SetValue(ref List<NameValueHeaderValue> parameters, double? value)
		{
			double? num = value;
			double num2 = 0.0;
			if (!((num.GetValueOrDefault() < num2) & (num != null)))
			{
				num = value;
				num2 = (double)1;
				if (!((num.GetValueOrDefault() > num2) & (num != null)))
				{
					if (parameters == null)
					{
						parameters = new List<NameValueHeaderValue>();
					}
					parameters.SetValue("q", (value == null) ? null : value.Value.ToString(NumberFormatInfo.InvariantInfo));
					return;
				}
			}
			throw new ArgumentOutOfRangeException("Quality");
		}
	}
}
