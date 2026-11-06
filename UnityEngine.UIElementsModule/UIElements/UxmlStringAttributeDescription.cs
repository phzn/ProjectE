using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020003AB RID: 939
	public class UxmlStringAttributeDescription : TypedUxmlAttributeDescription<string>
	{
		// Token: 0x06001F66 RID: 8038 RVA: 0x00077F57 File Offset: 0x00076157
		public UxmlStringAttributeDescription()
		{
			base.type = "string";
			base.typeNamespace = "http://www.w3.org/2001/XMLSchema";
			base.defaultValue = "";
		}

		// Token: 0x1700074F RID: 1871
		// (get) Token: 0x06001F67 RID: 8039 RVA: 0x00077F88 File Offset: 0x00076188
		public override string defaultValueAsString
		{
			get
			{
				return base.defaultValue;
			}
		}

		// Token: 0x06001F68 RID: 8040 RVA: 0x00077FA0 File Offset: 0x000761A0
		public override string GetValueFromBag(IUxmlAttributes bag, CreationContext cc)
		{
			return base.GetValueFromBag<string>(bag, cc, (string s, string t) => s, base.defaultValue);
		}

		// Token: 0x06001F69 RID: 8041 RVA: 0x00077FE0 File Offset: 0x000761E0
		public bool TryGetValueFromBag(IUxmlAttributes bag, CreationContext cc, ref string value)
		{
			return base.TryGetValueFromBag<string>(bag, cc, (string s, string t) => s, base.defaultValue, ref value);
		}
	}
}
