using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020003BD RID: 957
	public class UxmlTypeAttributeDescription<TBase> : TypedUxmlAttributeDescription<Type>
	{
		// Token: 0x06001FB6 RID: 8118 RVA: 0x00078931 File Offset: 0x00076B31
		public UxmlTypeAttributeDescription()
		{
			base.type = "string";
			base.typeNamespace = "http://www.w3.org/2001/XMLSchema";
			base.defaultValue = null;
		}

		// Token: 0x17000758 RID: 1880
		// (get) Token: 0x06001FB7 RID: 8119 RVA: 0x0007895C File Offset: 0x00076B5C
		public override string defaultValueAsString
		{
			get
			{
				return (base.defaultValue == null) ? "null" : base.defaultValue.FullName;
			}
		}

		// Token: 0x06001FB8 RID: 8120 RVA: 0x00078990 File Offset: 0x00076B90
		public override Type GetValueFromBag(IUxmlAttributes bag, CreationContext cc)
		{
			return base.GetValueFromBag<Type>(bag, cc, (string s, Type type1) => this.ConvertValueToType(s, type1), base.defaultValue);
		}

		// Token: 0x06001FB9 RID: 8121 RVA: 0x000789BC File Offset: 0x00076BBC
		public bool TryGetValueFromBag(IUxmlAttributes bag, CreationContext cc, ref Type value)
		{
			return base.TryGetValueFromBag<Type>(bag, cc, (string s, Type type1) => this.ConvertValueToType(s, type1), base.defaultValue, ref value);
		}

		// Token: 0x06001FBA RID: 8122 RVA: 0x000789EC File Offset: 0x00076BEC
		private Type ConvertValueToType(string v, Type defaultValue)
		{
			bool flag = string.IsNullOrEmpty(v);
			Type type;
			if (flag)
			{
				type = defaultValue;
			}
			else
			{
				try
				{
					Type type2 = Type.GetType(v, true);
					bool flag2 = !typeof(TBase).IsAssignableFrom(type2);
					if (!flag2)
					{
						return type2;
					}
					Debug.LogError(string.Concat(new string[]
					{
						"Type: Invalid type \"",
						v,
						"\". Type must derive from ",
						typeof(TBase).FullName,
						"."
					}));
				}
				catch (Exception ex)
				{
					Debug.LogException(ex);
				}
				type = defaultValue;
			}
			return type;
		}
	}
}
