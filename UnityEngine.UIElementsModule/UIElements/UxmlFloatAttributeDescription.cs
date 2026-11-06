using System;
using System.Globalization;

namespace UnityEngine.UIElements
{
	// Token: 0x020003AD RID: 941
	public class UxmlFloatAttributeDescription : TypedUxmlAttributeDescription<float>
	{
		// Token: 0x06001F6E RID: 8046 RVA: 0x0007802C File Offset: 0x0007622C
		public UxmlFloatAttributeDescription()
		{
			base.type = "float";
			base.typeNamespace = "http://www.w3.org/2001/XMLSchema";
			base.defaultValue = 0f;
		}

		// Token: 0x17000750 RID: 1872
		// (get) Token: 0x06001F6F RID: 8047 RVA: 0x0007805C File Offset: 0x0007625C
		public override string defaultValueAsString
		{
			get
			{
				return base.defaultValue.ToString(CultureInfo.InvariantCulture.NumberFormat);
			}
		}

		// Token: 0x06001F70 RID: 8048 RVA: 0x00078088 File Offset: 0x00076288
		public override float GetValueFromBag(IUxmlAttributes bag, CreationContext cc)
		{
			return base.GetValueFromBag<float>(bag, cc, (string s, float f) => UxmlFloatAttributeDescription.ConvertValueToFloat(s, f), base.defaultValue);
		}

		// Token: 0x06001F71 RID: 8049 RVA: 0x000780C8 File Offset: 0x000762C8
		public bool TryGetValueFromBag(IUxmlAttributes bag, CreationContext cc, ref float value)
		{
			return base.TryGetValueFromBag<float>(bag, cc, (string s, float f) => UxmlFloatAttributeDescription.ConvertValueToFloat(s, f), base.defaultValue, ref value);
		}

		// Token: 0x06001F72 RID: 8050 RVA: 0x00078108 File Offset: 0x00076308
		private static float ConvertValueToFloat(string v, float defaultValue)
		{
			float num;
			bool flag = v == null || !float.TryParse(v, NumberStyles.Float, CultureInfo.InvariantCulture, out num);
			float num2;
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
