using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200034F RID: 847
	[Serializable]
	internal class StyleProperty
	{
		// Token: 0x170006A5 RID: 1701
		// (get) Token: 0x06001C74 RID: 7284 RVA: 0x0006EC30 File Offset: 0x0006CE30
		// (set) Token: 0x06001C75 RID: 7285 RVA: 0x0006EC48 File Offset: 0x0006CE48
		public string name
		{
			get
			{
				return this.m_Name;
			}
			internal set
			{
				this.m_Name = value;
			}
		}

		// Token: 0x170006A6 RID: 1702
		// (get) Token: 0x06001C76 RID: 7286 RVA: 0x0006EC54 File Offset: 0x0006CE54
		// (set) Token: 0x06001C77 RID: 7287 RVA: 0x0006EC6C File Offset: 0x0006CE6C
		public int line
		{
			get
			{
				return this.m_Line;
			}
			internal set
			{
				this.m_Line = value;
			}
		}

		// Token: 0x170006A7 RID: 1703
		// (get) Token: 0x06001C78 RID: 7288 RVA: 0x0006EC78 File Offset: 0x0006CE78
		// (set) Token: 0x06001C79 RID: 7289 RVA: 0x0006EC90 File Offset: 0x0006CE90
		public StyleValueHandle[] values
		{
			get
			{
				return this.m_Values;
			}
			internal set
			{
				this.m_Values = value;
			}
		}

		// Token: 0x04000BC2 RID: 3010
		[SerializeField]
		private string m_Name;

		// Token: 0x04000BC3 RID: 3011
		[SerializeField]
		private int m_Line;

		// Token: 0x04000BC4 RID: 3012
		[SerializeField]
		private StyleValueHandle[] m_Values;

		// Token: 0x04000BC5 RID: 3013
		[NonSerialized]
		internal bool isCustomProperty;

		// Token: 0x04000BC6 RID: 3014
		[NonSerialized]
		internal bool requireVariableResolve;
	}
}
