using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine.UIElements
{
	// Token: 0x020003A8 RID: 936
	public abstract class UxmlAttributeDescription
	{
		// Token: 0x06001F50 RID: 8016 RVA: 0x00077B86 File Offset: 0x00075D86
		protected UxmlAttributeDescription()
		{
			this.use = UxmlAttributeDescription.Use.Optional;
			this.restriction = null;
		}

		// Token: 0x17000746 RID: 1862
		// (get) Token: 0x06001F51 RID: 8017 RVA: 0x00077BA0 File Offset: 0x00075DA0
		// (set) Token: 0x06001F52 RID: 8018 RVA: 0x00077BA8 File Offset: 0x00075DA8
		public string name { get; set; }

		// Token: 0x17000747 RID: 1863
		// (get) Token: 0x06001F53 RID: 8019 RVA: 0x00077BB4 File Offset: 0x00075DB4
		// (set) Token: 0x06001F54 RID: 8020 RVA: 0x00077BCC File Offset: 0x00075DCC
		public IEnumerable<string> obsoleteNames
		{
			get
			{
				return this.m_ObsoleteNames;
			}
			set
			{
				this.m_ObsoleteNames = value.ToArray<string>();
			}
		}

		// Token: 0x17000748 RID: 1864
		// (get) Token: 0x06001F55 RID: 8021 RVA: 0x00077BDB File Offset: 0x00075DDB
		// (set) Token: 0x06001F56 RID: 8022 RVA: 0x00077BE3 File Offset: 0x00075DE3
		public string type { get; protected set; }

		// Token: 0x17000749 RID: 1865
		// (get) Token: 0x06001F57 RID: 8023 RVA: 0x00077BEC File Offset: 0x00075DEC
		// (set) Token: 0x06001F58 RID: 8024 RVA: 0x00077BF4 File Offset: 0x00075DF4
		public string typeNamespace { get; protected set; }

		// Token: 0x1700074A RID: 1866
		// (get) Token: 0x06001F59 RID: 8025
		public abstract string defaultValueAsString { get; }

		// Token: 0x1700074B RID: 1867
		// (get) Token: 0x06001F5A RID: 8026 RVA: 0x00077BFD File Offset: 0x00075DFD
		// (set) Token: 0x06001F5B RID: 8027 RVA: 0x00077C05 File Offset: 0x00075E05
		public UxmlAttributeDescription.Use use { get; set; }

		// Token: 0x1700074C RID: 1868
		// (get) Token: 0x06001F5C RID: 8028 RVA: 0x00077C0E File Offset: 0x00075E0E
		// (set) Token: 0x06001F5D RID: 8029 RVA: 0x00077C16 File Offset: 0x00075E16
		public UxmlTypeRestriction restriction { get; set; }

		// Token: 0x06001F5E RID: 8030 RVA: 0x00077C20 File Offset: 0x00075E20
		internal bool TryGetValueFromBagAsString(IUxmlAttributes bag, CreationContext cc, out string value)
		{
			bool flag = this.name == null && (this.m_ObsoleteNames == null || this.m_ObsoleteNames.Length == 0);
			bool flag2;
			if (flag)
			{
				Debug.LogError("Attribute description has no name.");
				value = null;
				flag2 = false;
			}
			else
			{
				string text;
				bag.TryGetAttributeValue("name", out text);
				bool flag3 = !string.IsNullOrEmpty(text) && cc.attributeOverrides != null;
				if (flag3)
				{
					for (int i = 0; i < cc.attributeOverrides.Count; i++)
					{
						bool flag4 = cc.attributeOverrides[i].m_ElementName != text;
						if (!flag4)
						{
							bool flag5 = cc.attributeOverrides[i].m_AttributeName != this.name;
							if (flag5)
							{
								bool flag6 = this.m_ObsoleteNames != null;
								if (!flag6)
								{
									goto IL_0147;
								}
								bool flag7 = false;
								for (int j = 0; j < this.m_ObsoleteNames.Length; j++)
								{
									bool flag8 = cc.attributeOverrides[i].m_AttributeName == this.m_ObsoleteNames[j];
									if (flag8)
									{
										flag7 = true;
										break;
									}
								}
								bool flag9 = !flag7;
								if (flag9)
								{
									goto IL_0147;
								}
							}
							value = cc.attributeOverrides[i].m_Value;
							return true;
						}
						IL_0147:;
					}
				}
				bool flag10 = this.name == null;
				if (flag10)
				{
					for (int k = 0; k < this.m_ObsoleteNames.Length; k++)
					{
						bool flag11 = bag.TryGetAttributeValue(this.m_ObsoleteNames[k], out value);
						if (flag11)
						{
							bool flag12 = cc.visualTreeAsset != null;
							if (flag12)
							{
							}
							return true;
						}
					}
					value = null;
					flag2 = false;
				}
				else
				{
					bool flag13 = !bag.TryGetAttributeValue(this.name, out value);
					if (flag13)
					{
						bool flag14 = this.m_ObsoleteNames != null;
						if (flag14)
						{
							for (int l = 0; l < this.m_ObsoleteNames.Length; l++)
							{
								bool flag15 = bag.TryGetAttributeValue(this.m_ObsoleteNames[l], out value);
								if (flag15)
								{
									bool flag16 = cc.visualTreeAsset != null;
									if (flag16)
									{
									}
									return true;
								}
							}
						}
						value = null;
						flag2 = false;
					}
					else
					{
						flag2 = true;
					}
				}
			}
			return flag2;
		}

		// Token: 0x06001F5F RID: 8031 RVA: 0x00077E84 File Offset: 0x00076084
		protected bool TryGetValueFromBag<T>(IUxmlAttributes bag, CreationContext cc, Func<string, T, T> converterFunc, T defaultValue, ref T value)
		{
			string text;
			bool flag = this.TryGetValueFromBagAsString(bag, cc, out text);
			bool flag3;
			if (flag)
			{
				bool flag2 = converterFunc != null;
				if (flag2)
				{
					value = converterFunc(text, defaultValue);
				}
				else
				{
					value = defaultValue;
				}
				flag3 = true;
			}
			else
			{
				flag3 = false;
			}
			return flag3;
		}

		// Token: 0x06001F60 RID: 8032 RVA: 0x00077ED4 File Offset: 0x000760D4
		protected T GetValueFromBag<T>(IUxmlAttributes bag, CreationContext cc, Func<string, T, T> converterFunc, T defaultValue)
		{
			bool flag = converterFunc == null;
			if (flag)
			{
				throw new ArgumentNullException("converterFunc");
			}
			string text;
			bool flag2 = this.TryGetValueFromBagAsString(bag, cc, out text);
			T t;
			if (flag2)
			{
				t = converterFunc(text, defaultValue);
			}
			else
			{
				t = defaultValue;
			}
			return t;
		}

		// Token: 0x04000CF7 RID: 3319
		protected const string xmlSchemaNamespace = "http://www.w3.org/2001/XMLSchema";

		// Token: 0x04000CF9 RID: 3321
		private string[] m_ObsoleteNames;

		// Token: 0x020003A9 RID: 937
		public enum Use
		{
			// Token: 0x04000CFF RID: 3327
			None,
			// Token: 0x04000D00 RID: 3328
			Optional,
			// Token: 0x04000D01 RID: 3329
			Prohibited,
			// Token: 0x04000D02 RID: 3330
			Required
		}
	}
}
