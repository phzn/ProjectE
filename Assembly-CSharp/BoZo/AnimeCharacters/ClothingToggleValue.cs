using System;

namespace Bozo.AnimeCharacters
{
	// Token: 0x02000845 RID: 2117
	[Serializable]
	public class ClothingToggleValue
	{
		// Token: 0x06003800 RID: 14336 RVA: 0x00147A38 File Offset: 0x00145C38
		public ClothingToggleValue(string groupId, bool isEnabled)
		{
			this.groupId = groupId;
			this.isEnabled = isEnabled;
		}

		// Token: 0x040038FF RID: 14591
		public string groupId;

		// Token: 0x04003900 RID: 14592
		public bool isEnabled;
	}
}
