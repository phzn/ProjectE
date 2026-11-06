using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020003C0 RID: 960
	public class UxmlHash128AttributeDescription : TypedUxmlAttributeDescription<Hash128>
	{
		// Token: 0x06001FC8 RID: 8136 RVA: 0x00078D68 File Offset: 0x00076F68
		public UxmlHash128AttributeDescription()
		{
			base.type = "string";
			base.typeNamespace = "http://www.w3.org/2001/XMLSchema";
			base.defaultValue = default(Hash128);
		}

		// Token: 0x1700075A RID: 1882
		// (get) Token: 0x06001FC9 RID: 8137 RVA: 0x00078DA8 File Offset: 0x00076FA8
		public override string defaultValueAsString
		{
			get
			{
				return base.defaultValue.ToString();
			}
		}

		// Token: 0x06001FCA RID: 8138 RVA: 0x00078DD0 File Offset: 0x00076FD0
		public override Hash128 GetValueFromBag(IUxmlAttributes bag, CreationContext cc)
		{
			return base.GetValueFromBag<Hash128>(bag, cc, (string s, Hash128 i) => Hash128.Parse(s), base.defaultValue);
		}

		// Token: 0x06001FCB RID: 8139 RVA: 0x00078E10 File Offset: 0x00077010
		public bool TryGetValueFromBag(IUxmlAttributes bag, CreationContext cc, ref Hash128 value)
		{
			return base.TryGetValueFromBag<Hash128>(bag, cc, (string s, Hash128 i) => Hash128.Parse(s), base.defaultValue, ref value);
		}
	}
}
