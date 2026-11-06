using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000350 RID: 848
	[Serializable]
	internal class StyleRule
	{
		// Token: 0x170006A8 RID: 1704
		// (get) Token: 0x06001C7B RID: 7291 RVA: 0x0006EC9C File Offset: 0x0006CE9C
		// (set) Token: 0x06001C7C RID: 7292 RVA: 0x0006ECB4 File Offset: 0x0006CEB4
		public StyleProperty[] properties
		{
			get
			{
				return this.m_Properties;
			}
			internal set
			{
				this.m_Properties = value;
			}
		}

		// Token: 0x04000BC7 RID: 3015
		[SerializeField]
		private StyleProperty[] m_Properties;

		// Token: 0x04000BC8 RID: 3016
		[SerializeField]
		internal int line;

		// Token: 0x04000BC9 RID: 3017
		[NonSerialized]
		internal int customPropertiesCount;
	}
}
