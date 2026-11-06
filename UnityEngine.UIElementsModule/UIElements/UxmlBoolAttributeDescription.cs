using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020003B9 RID: 953
	public class UxmlBoolAttributeDescription : TypedUxmlAttributeDescription<bool>
	{
		// Token: 0x06001FA4 RID: 8100 RVA: 0x000786E1 File Offset: 0x000768E1
		public UxmlBoolAttributeDescription()
		{
			base.type = "boolean";
			base.typeNamespace = "http://www.w3.org/2001/XMLSchema";
			base.defaultValue = false;
		}

		// Token: 0x17000756 RID: 1878
		// (get) Token: 0x06001FA5 RID: 8101 RVA: 0x0007870C File Offset: 0x0007690C
		public override string defaultValueAsString
		{
			get
			{
				return base.defaultValue.ToString().ToLowerInvariant();
			}
		}

		// Token: 0x06001FA6 RID: 8102 RVA: 0x00078734 File Offset: 0x00076934
		public override bool GetValueFromBag(IUxmlAttributes bag, CreationContext cc)
		{
			return base.GetValueFromBag<bool>(bag, cc, (string s, bool b) => UxmlBoolAttributeDescription.ConvertValueToBool(s, b), base.defaultValue);
		}

		// Token: 0x06001FA7 RID: 8103 RVA: 0x00078774 File Offset: 0x00076974
		public bool TryGetValueFromBag(IUxmlAttributes bag, CreationContext cc, ref bool value)
		{
			return base.TryGetValueFromBag<bool>(bag, cc, (string s, bool b) => UxmlBoolAttributeDescription.ConvertValueToBool(s, b), base.defaultValue, ref value);
		}

		// Token: 0x06001FA8 RID: 8104 RVA: 0x000787B4 File Offset: 0x000769B4
		private static bool ConvertValueToBool(string v, bool defaultValue)
		{
			bool flag2;
			bool flag = v == null || !bool.TryParse(v, out flag2);
			bool flag3;
			if (flag)
			{
				flag3 = defaultValue;
			}
			else
			{
				flag3 = flag2;
			}
			return flag3;
		}
	}
}
