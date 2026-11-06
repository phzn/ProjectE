using System;
using System.Globalization;

namespace UnityEngine.UIElements
{
	// Token: 0x020003B3 RID: 947
	public class UxmlUnsignedIntAttributeDescription : TypedUxmlAttributeDescription<uint>
	{
		// Token: 0x06001F89 RID: 8073 RVA: 0x00078399 File Offset: 0x00076599
		public UxmlUnsignedIntAttributeDescription()
		{
			base.type = "unsignedInt";
			base.typeNamespace = "http://www.w3.org/2001/XMLSchema";
			base.defaultValue = 0U;
		}

		// Token: 0x17000753 RID: 1875
		// (get) Token: 0x06001F8A RID: 8074 RVA: 0x000783C4 File Offset: 0x000765C4
		public override string defaultValueAsString
		{
			get
			{
				return base.defaultValue.ToString(CultureInfo.InvariantCulture.NumberFormat);
			}
		}

		// Token: 0x06001F8B RID: 8075 RVA: 0x000783F0 File Offset: 0x000765F0
		public override uint GetValueFromBag(IUxmlAttributes bag, CreationContext cc)
		{
			return base.GetValueFromBag<uint>(bag, cc, (string s, uint i) => UxmlUnsignedIntAttributeDescription.ConvertValueToUInt(s, i), base.defaultValue);
		}

		// Token: 0x06001F8C RID: 8076 RVA: 0x00078430 File Offset: 0x00076630
		public bool TryGetValueFromBag(IUxmlAttributes bag, CreationContext cc, ref uint value)
		{
			return base.TryGetValueFromBag<uint>(bag, cc, (string s, uint i) => UxmlUnsignedIntAttributeDescription.ConvertValueToUInt(s, i), base.defaultValue, ref value);
		}

		// Token: 0x06001F8D RID: 8077 RVA: 0x00078470 File Offset: 0x00076670
		private static uint ConvertValueToUInt(string v, uint defaultValue)
		{
			uint num;
			bool flag = v == null || !uint.TryParse(v, out num);
			uint num2;
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
