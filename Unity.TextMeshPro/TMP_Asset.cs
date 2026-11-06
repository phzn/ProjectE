using System;
using UnityEngine;

namespace TMPro
{
	// Token: 0x0200001F RID: 31
	[Serializable]
	public abstract class TMP_Asset : ScriptableObject
	{
		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000113 RID: 275 RVA: 0x000173D1 File Offset: 0x000155D1
		public int instanceID
		{
			get
			{
				if (this.m_InstanceID == 0)
				{
					this.m_InstanceID = base.GetInstanceID();
				}
				return this.m_InstanceID;
			}
		}

		// Token: 0x04000107 RID: 263
		private int m_InstanceID;

		// Token: 0x04000108 RID: 264
		public int hashCode;

		// Token: 0x04000109 RID: 265
		public Material material;

		// Token: 0x0400010A RID: 266
		public int materialHashCode;
	}
}
