using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020003BB RID: 955
	public class UxmlColorAttributeDescription : TypedUxmlAttributeDescription<Color>
	{
		// Token: 0x06001FAD RID: 8109 RVA: 0x000787F8 File Offset: 0x000769F8
		public UxmlColorAttributeDescription()
		{
			base.type = "string";
			base.typeNamespace = "http://www.w3.org/2001/XMLSchema";
			base.defaultValue = new Color(0f, 0f, 0f, 1f);
		}

		// Token: 0x17000757 RID: 1879
		// (get) Token: 0x06001FAE RID: 8110 RVA: 0x00078848 File Offset: 0x00076A48
		public override string defaultValueAsString
		{
			get
			{
				return base.defaultValue.ToString();
			}
		}

		// Token: 0x06001FAF RID: 8111 RVA: 0x00078870 File Offset: 0x00076A70
		public override Color GetValueFromBag(IUxmlAttributes bag, CreationContext cc)
		{
			return base.GetValueFromBag<Color>(bag, cc, (string s, Color color) => UxmlColorAttributeDescription.ConvertValueToColor(s, color), base.defaultValue);
		}

		// Token: 0x06001FB0 RID: 8112 RVA: 0x000788B0 File Offset: 0x00076AB0
		public bool TryGetValueFromBag(IUxmlAttributes bag, CreationContext cc, ref Color value)
		{
			return base.TryGetValueFromBag<Color>(bag, cc, (string s, Color color) => UxmlColorAttributeDescription.ConvertValueToColor(s, color), base.defaultValue, ref value);
		}

		// Token: 0x06001FB1 RID: 8113 RVA: 0x000788F0 File Offset: 0x00076AF0
		private static Color ConvertValueToColor(string v, Color defaultValue)
		{
			Color color;
			bool flag = v == null || !ColorUtility.TryParseHtmlString(v, out color);
			Color color2;
			if (flag)
			{
				color2 = defaultValue;
			}
			else
			{
				color2 = color;
			}
			return color2;
		}
	}
}
