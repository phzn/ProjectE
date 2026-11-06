using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x020003DB RID: 987
	[Serializable]
	internal class VisualElementAsset : UxmlAsset, ISerializationCallbackReceiver
	{
		// Token: 0x17000786 RID: 1926
		// (get) Token: 0x0600204D RID: 8269 RVA: 0x00079EEC File Offset: 0x000780EC
		// (set) Token: 0x0600204E RID: 8270 RVA: 0x00079F04 File Offset: 0x00078104
		public int ruleIndex
		{
			get
			{
				return this.m_RuleIndex;
			}
			set
			{
				this.m_RuleIndex = value;
			}
		}

		// Token: 0x17000787 RID: 1927
		// (get) Token: 0x0600204F RID: 8271 RVA: 0x00079F10 File Offset: 0x00078110
		// (set) Token: 0x06002050 RID: 8272 RVA: 0x00079F28 File Offset: 0x00078128
		public string[] classes
		{
			get
			{
				return this.m_Classes;
			}
			set
			{
				this.m_Classes = value;
			}
		}

		// Token: 0x17000788 RID: 1928
		// (get) Token: 0x06002051 RID: 8273 RVA: 0x00079F34 File Offset: 0x00078134
		// (set) Token: 0x06002052 RID: 8274 RVA: 0x00079F5E File Offset: 0x0007815E
		public List<string> stylesheetPaths
		{
			get
			{
				List<string> list;
				if ((list = this.m_StylesheetPaths) == null)
				{
					list = (this.m_StylesheetPaths = new List<string>());
				}
				return list;
			}
			set
			{
				this.m_StylesheetPaths = value;
			}
		}

		// Token: 0x17000789 RID: 1929
		// (get) Token: 0x06002053 RID: 8275 RVA: 0x00079F68 File Offset: 0x00078168
		public bool hasStylesheetPaths
		{
			get
			{
				return this.m_StylesheetPaths != null;
			}
		}

		// Token: 0x1700078A RID: 1930
		// (get) Token: 0x06002054 RID: 8276 RVA: 0x00079F74 File Offset: 0x00078174
		// (set) Token: 0x06002055 RID: 8277 RVA: 0x00079F9E File Offset: 0x0007819E
		public List<StyleSheet> stylesheets
		{
			get
			{
				List<StyleSheet> list;
				if ((list = this.m_Stylesheets) == null)
				{
					list = (this.m_Stylesheets = new List<StyleSheet>());
				}
				return list;
			}
			set
			{
				this.m_Stylesheets = value;
			}
		}

		// Token: 0x1700078B RID: 1931
		// (get) Token: 0x06002056 RID: 8278 RVA: 0x00079FA8 File Offset: 0x000781A8
		public bool hasStylesheets
		{
			get
			{
				return this.m_Stylesheets != null;
			}
		}

		// Token: 0x1700078C RID: 1932
		// (get) Token: 0x06002057 RID: 8279 RVA: 0x00079FB3 File Offset: 0x000781B3
		// (set) Token: 0x06002058 RID: 8280 RVA: 0x00079FBB File Offset: 0x000781BB
		internal bool skipClone
		{
			get
			{
				return this.m_SkipClone;
			}
			set
			{
				this.m_SkipClone = value;
			}
		}

		// Token: 0x06002059 RID: 8281 RVA: 0x00079FC4 File Offset: 0x000781C4
		public VisualElementAsset(string fullTypeName)
			: base(fullTypeName)
		{
			this.m_Name = string.Empty;
			this.m_Text = string.Empty;
			this.m_PickingMode = PickingMode.Position;
		}

		// Token: 0x0600205A RID: 8282 RVA: 0x00003CD2 File Offset: 0x00001ED2
		public void OnBeforeSerialize()
		{
		}

		// Token: 0x0600205B RID: 8283 RVA: 0x00079FF4 File Offset: 0x000781F4
		public void OnAfterDeserialize()
		{
			bool flag = !string.IsNullOrEmpty(this.m_Name) && !this.m_Properties.Contains("name");
			if (flag)
			{
				base.SetAttribute("name", this.m_Name);
			}
			bool flag2 = !string.IsNullOrEmpty(this.m_Text) && !this.m_Properties.Contains("text");
			if (flag2)
			{
				base.SetAttribute("text", this.m_Text);
			}
			bool flag3 = this.m_PickingMode != PickingMode.Position && !this.m_Properties.Contains("picking-mode") && !this.m_Properties.Contains("pickingMode");
			if (flag3)
			{
				base.SetAttribute("picking-mode", this.m_PickingMode.ToString());
			}
		}

		// Token: 0x04000D4D RID: 3405
		[SerializeField]
		private string m_Name;

		// Token: 0x04000D4E RID: 3406
		[SerializeField]
		private int m_RuleIndex = -1;

		// Token: 0x04000D4F RID: 3407
		[SerializeField]
		private string m_Text;

		// Token: 0x04000D50 RID: 3408
		[SerializeField]
		private PickingMode m_PickingMode;

		// Token: 0x04000D51 RID: 3409
		[SerializeField]
		private string[] m_Classes;

		// Token: 0x04000D52 RID: 3410
		[SerializeField]
		private List<string> m_StylesheetPaths;

		// Token: 0x04000D53 RID: 3411
		[SerializeField]
		private List<StyleSheet> m_Stylesheets;

		// Token: 0x04000D54 RID: 3412
		[SerializeField]
		private bool m_SkipClone;
	}
}
