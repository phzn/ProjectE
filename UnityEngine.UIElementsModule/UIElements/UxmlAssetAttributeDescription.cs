using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020003A6 RID: 934
	internal class UxmlAssetAttributeDescription<T> : TypedUxmlAttributeDescription<T> where T : Object
	{
		// Token: 0x06001F4A RID: 8010 RVA: 0x00077AB0 File Offset: 0x00075CB0
		public UxmlAssetAttributeDescription()
		{
			base.type = "string";
			base.typeNamespace = "http://www.w3.org/2001/XMLSchema";
			base.defaultValue = default(T);
		}

		// Token: 0x17000745 RID: 1861
		// (get) Token: 0x06001F4B RID: 8011 RVA: 0x00077AED File Offset: 0x00075CED
		public override string defaultValueAsString
		{
			get
			{
				T t = base.defaultValue;
				return ((t != null) ? t.ToString() : null) ?? "null";
			}
		}

		// Token: 0x06001F4C RID: 8012 RVA: 0x00077B10 File Offset: 0x00075D10
		public override T GetValueFromBag(IUxmlAttributes bag, CreationContext cc)
		{
			string text = null;
			bool flag = base.TryGetValueFromBag<string>(bag, cc, (string s, string t) => s, null, ref text);
			T t2;
			if (flag)
			{
				VisualTreeAsset visualTreeAsset = cc.visualTreeAsset;
				t2 = ((visualTreeAsset != null) ? visualTreeAsset.GetAsset<T>(text) : default(T));
			}
			else
			{
				t2 = default(T);
			}
			return t2;
		}
	}
}
