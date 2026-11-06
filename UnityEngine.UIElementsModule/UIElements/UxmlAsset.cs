using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x020003D4 RID: 980
	[Serializable]
	internal class UxmlAsset : IUxmlAttributes
	{
		// Token: 0x06002021 RID: 8225 RVA: 0x0007982F File Offset: 0x00077A2F
		public UxmlAsset(string fullTypeName)
		{
			this.m_FullTypeName = fullTypeName;
		}

		// Token: 0x1700077B RID: 1915
		// (get) Token: 0x06002022 RID: 8226 RVA: 0x00079840 File Offset: 0x00077A40
		// (set) Token: 0x06002023 RID: 8227 RVA: 0x00079848 File Offset: 0x00077A48
		public string fullTypeName
		{
			get
			{
				return this.m_FullTypeName;
			}
			set
			{
				this.m_FullTypeName = value;
			}
		}

		// Token: 0x1700077C RID: 1916
		// (get) Token: 0x06002024 RID: 8228 RVA: 0x00079851 File Offset: 0x00077A51
		// (set) Token: 0x06002025 RID: 8229 RVA: 0x00079859 File Offset: 0x00077A59
		public int id
		{
			get
			{
				return this.m_Id;
			}
			set
			{
				this.m_Id = value;
			}
		}

		// Token: 0x1700077D RID: 1917
		// (get) Token: 0x06002026 RID: 8230 RVA: 0x00079862 File Offset: 0x00077A62
		// (set) Token: 0x06002027 RID: 8231 RVA: 0x0007986A File Offset: 0x00077A6A
		public int orderInDocument
		{
			get
			{
				return this.m_OrderInDocument;
			}
			set
			{
				this.m_OrderInDocument = value;
			}
		}

		// Token: 0x1700077E RID: 1918
		// (get) Token: 0x06002028 RID: 8232 RVA: 0x00079873 File Offset: 0x00077A73
		// (set) Token: 0x06002029 RID: 8233 RVA: 0x0007987B File Offset: 0x00077A7B
		public int parentId
		{
			get
			{
				return this.m_ParentId;
			}
			set
			{
				this.m_ParentId = value;
			}
		}

		// Token: 0x0600202A RID: 8234 RVA: 0x00079884 File Offset: 0x00077A84
		public List<string> GetProperties()
		{
			return this.m_Properties;
		}

		// Token: 0x0600202B RID: 8235 RVA: 0x0007988C File Offset: 0x00077A8C
		public bool HasParent()
		{
			return this.m_ParentId != 0;
		}

		// Token: 0x0600202C RID: 8236 RVA: 0x00079898 File Offset: 0x00077A98
		public bool HasAttribute(string attributeName)
		{
			bool flag = this.m_Properties == null || this.m_Properties.Count <= 0;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				for (int i = 0; i < this.m_Properties.Count; i += 2)
				{
					string text = this.m_Properties[i];
					bool flag3 = text == attributeName;
					if (flag3)
					{
						return true;
					}
				}
				flag2 = false;
			}
			return flag2;
		}

		// Token: 0x0600202D RID: 8237 RVA: 0x0007990C File Offset: 0x00077B0C
		public string GetAttributeValue(string attributeName)
		{
			string text;
			this.TryGetAttributeValue(attributeName, out text);
			return text;
		}

		// Token: 0x0600202E RID: 8238 RVA: 0x0007992C File Offset: 0x00077B2C
		public bool TryGetAttributeValue(string propertyName, out string value)
		{
			bool flag = this.m_Properties == null;
			bool flag2;
			if (flag)
			{
				value = null;
				flag2 = false;
			}
			else
			{
				for (int i = 0; i < this.m_Properties.Count - 1; i += 2)
				{
					bool flag3 = this.m_Properties[i] == propertyName;
					if (flag3)
					{
						value = this.m_Properties[i + 1];
						return true;
					}
				}
				value = null;
				flag2 = false;
			}
			return flag2;
		}

		// Token: 0x0600202F RID: 8239 RVA: 0x000799A2 File Offset: 0x00077BA2
		public void SetAttribute(string name, string value)
		{
			this.SetOrAddProperty(name, value);
		}

		// Token: 0x06002030 RID: 8240 RVA: 0x000799B0 File Offset: 0x00077BB0
		public void RemoveAttribute(string attributeName)
		{
			bool flag = this.m_Properties == null || this.m_Properties.Count <= 0;
			if (!flag)
			{
				for (int i = 0; i < this.m_Properties.Count; i += 2)
				{
					string text = this.m_Properties[i];
					bool flag2 = text != attributeName;
					if (!flag2)
					{
						this.m_Properties.RemoveAt(i);
						this.m_Properties.RemoveAt(i);
						break;
					}
				}
			}
		}

		// Token: 0x06002031 RID: 8241 RVA: 0x00079A34 File Offset: 0x00077C34
		private void SetOrAddProperty(string propertyName, string propertyValue)
		{
			bool flag = this.m_Properties == null;
			if (flag)
			{
				this.m_Properties = new List<string>();
			}
			for (int i = 0; i < this.m_Properties.Count - 1; i += 2)
			{
				bool flag2 = this.m_Properties[i] == propertyName;
				if (flag2)
				{
					this.m_Properties[i + 1] = propertyValue;
					return;
				}
			}
			this.m_Properties.Add(propertyName);
			this.m_Properties.Add(propertyValue);
		}

		// Token: 0x04000D41 RID: 3393
		[SerializeField]
		private string m_FullTypeName;

		// Token: 0x04000D42 RID: 3394
		[SerializeField]
		private int m_Id;

		// Token: 0x04000D43 RID: 3395
		[SerializeField]
		private int m_OrderInDocument;

		// Token: 0x04000D44 RID: 3396
		[SerializeField]
		private int m_ParentId;

		// Token: 0x04000D45 RID: 3397
		[SerializeField]
		protected List<string> m_Properties;
	}
}
