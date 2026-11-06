using System;

namespace UnityEngine.Localization.SmartFormat.Core.Parsing
{
	// Token: 0x02000086 RID: 134
	public class Selector : FormatItem
	{
		// Token: 0x0600038E RID: 910 RVA: 0x0000E187 File Offset: 0x0000C387
		public override void Clear()
		{
			base.Clear();
			this.m_Operator = null;
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x0600038F RID: 911 RVA: 0x0000E196 File Offset: 0x0000C396
		// (set) Token: 0x06000390 RID: 912 RVA: 0x0000E19E File Offset: 0x0000C39E
		public int SelectorIndex { get; internal set; }

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000391 RID: 913 RVA: 0x0000E1A7 File Offset: 0x0000C3A7
		public string Operator
		{
			get
			{
				if (this.m_Operator == null)
				{
					this.m_Operator = this.baseString.Substring(this.operatorStart, this.startIndex - this.operatorStart);
				}
				return this.m_Operator;
			}
		}

		// Token: 0x04000101 RID: 257
		private string m_Operator;

		// Token: 0x04000102 RID: 258
		internal int operatorStart;
	}
}
