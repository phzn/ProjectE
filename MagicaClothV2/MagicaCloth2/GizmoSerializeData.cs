using System;

namespace MagicaCloth2
{
	// Token: 0x02000065 RID: 101
	[Serializable]
	public class GizmoSerializeData
	{
		// Token: 0x06000169 RID: 361 RVA: 0x0000F161 File Offset: 0x0000D361
		public GizmoSerializeData()
		{
			this.clothDebugSettings.enable = true;
			this.clothDebugSettings.shape = true;
		}

		// Token: 0x0600016A RID: 362 RVA: 0x0000F18C File Offset: 0x0000D38C
		public bool IsAlways()
		{
			return this.always;
		}

		// Token: 0x04000243 RID: 579
		public bool always;

		// Token: 0x04000244 RID: 580
		public ClothDebugSettings clothDebugSettings = new ClothDebugSettings();
	}
}
