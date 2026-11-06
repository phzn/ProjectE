using System;
using UnityEngine;

namespace Bozo.AnimeCharacters
{
	// Token: 0x02000846 RID: 2118
	[Serializable]
	public class SkinDecalStateValue
	{
		// Token: 0x06003801 RID: 14337 RVA: 0x00147A4E File Offset: 0x00145C4E
		public SkinDecalStateValue(string decalId, bool isEnabled, float opacity, Color tint)
		{
			this.decalId = decalId;
			this.isEnabled = isEnabled;
			this.opacity = opacity;
			this.tint = tint;
		}

		// Token: 0x04003901 RID: 14593
		public string decalId;

		// Token: 0x04003902 RID: 14594
		public bool isEnabled;

		// Token: 0x04003903 RID: 14595
		public float opacity;

		// Token: 0x04003904 RID: 14596
		public Color tint;
	}
}
