using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace UnityEngine.UIElements
{
	// Token: 0x020003A4 RID: 932
	[Serializable]
	internal class TemplateAsset : VisualElementAsset
	{
		// Token: 0x17000742 RID: 1858
		// (get) Token: 0x06001F42 RID: 8002 RVA: 0x000779CC File Offset: 0x00075BCC
		// (set) Token: 0x06001F43 RID: 8003 RVA: 0x000779E4 File Offset: 0x00075BE4
		public string templateAlias
		{
			get
			{
				return this.m_TemplateAlias;
			}
			set
			{
				this.m_TemplateAlias = value;
			}
		}

		// Token: 0x17000743 RID: 1859
		// (get) Token: 0x06001F44 RID: 8004 RVA: 0x000779F0 File Offset: 0x00075BF0
		// (set) Token: 0x06001F45 RID: 8005 RVA: 0x00077A20 File Offset: 0x00075C20
		public List<TemplateAsset.AttributeOverride> attributeOverrides
		{
			get
			{
				return (this.m_AttributeOverrides == null) ? (this.m_AttributeOverrides = new List<TemplateAsset.AttributeOverride>()) : this.m_AttributeOverrides;
			}
			set
			{
				this.m_AttributeOverrides = value;
			}
		}

		// Token: 0x17000744 RID: 1860
		// (get) Token: 0x06001F46 RID: 8006 RVA: 0x00077A2C File Offset: 0x00075C2C
		// (set) Token: 0x06001F47 RID: 8007 RVA: 0x00077A44 File Offset: 0x00075C44
		internal List<VisualTreeAsset.SlotUsageEntry> slotUsages
		{
			get
			{
				return this.m_SlotUsages;
			}
			set
			{
				this.m_SlotUsages = value;
			}
		}

		// Token: 0x06001F48 RID: 8008 RVA: 0x00077A4E File Offset: 0x00075C4E
		public TemplateAsset(string templateAlias, string fullTypeName)
			: base(fullTypeName)
		{
			Assert.IsFalse(string.IsNullOrEmpty(templateAlias), "Template alias must not be null or empty");
			this.m_TemplateAlias = templateAlias;
		}

		// Token: 0x06001F49 RID: 8009 RVA: 0x00077A74 File Offset: 0x00075C74
		public void AddSlotUsage(string slotName, int resId)
		{
			bool flag = this.m_SlotUsages == null;
			if (flag)
			{
				this.m_SlotUsages = new List<VisualTreeAsset.SlotUsageEntry>();
			}
			this.m_SlotUsages.Add(new VisualTreeAsset.SlotUsageEntry(slotName, resId));
		}

		// Token: 0x04000CEF RID: 3311
		[SerializeField]
		private string m_TemplateAlias;

		// Token: 0x04000CF0 RID: 3312
		[SerializeField]
		private List<TemplateAsset.AttributeOverride> m_AttributeOverrides;

		// Token: 0x04000CF1 RID: 3313
		[SerializeField]
		private List<VisualTreeAsset.SlotUsageEntry> m_SlotUsages;

		// Token: 0x020003A5 RID: 933
		[Serializable]
		public struct AttributeOverride
		{
			// Token: 0x04000CF2 RID: 3314
			public string m_ElementName;

			// Token: 0x04000CF3 RID: 3315
			public string m_AttributeName;

			// Token: 0x04000CF4 RID: 3316
			public string m_Value;
		}
	}
}
