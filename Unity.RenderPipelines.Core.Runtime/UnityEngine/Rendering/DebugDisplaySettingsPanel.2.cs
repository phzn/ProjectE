using System;

namespace UnityEngine.Rendering
{
	// Token: 0x0200005D RID: 93
	public abstract class DebugDisplaySettingsPanel<T> : DebugDisplaySettingsPanel where T : IDebugDisplaySettingsData
	{
		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060002F6 RID: 758 RVA: 0x0000CE8D File Offset: 0x0000B08D
		// (set) Token: 0x060002F7 RID: 759 RVA: 0x0000CE95 File Offset: 0x0000B095
		public T data
		{
			get
			{
				return this.m_Data;
			}
			internal set
			{
				this.m_Data = value;
			}
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0000CE9E File Offset: 0x0000B09E
		protected DebugDisplaySettingsPanel(T data)
		{
			this.m_Data = data;
		}

		// Token: 0x040001A7 RID: 423
		internal T m_Data;
	}
}
