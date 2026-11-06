using System;
using System.Collections.Generic;

namespace Bozo.AnimeCharacters
{
	// Token: 0x02000834 RID: 2100
	[Serializable]
	public class CharacterData
	{
		// Token: 0x0400384A RID: 14410
		public string characterName;

		// Token: 0x0400384B RID: 14411
		public List<string> bodyIDs;

		// Token: 0x0400384C RID: 14412
		public List<float> bodyShapes;

		// Token: 0x0400384D RID: 14413
		public List<string> faceIDs;

		// Token: 0x0400384E RID: 14414
		public List<float> faceShapes;

		// Token: 0x0400384F RID: 14415
		public List<string> bodyModsKeys;

		// Token: 0x04003850 RID: 14416
		public List<BodyModData> bodyMods;

		// Token: 0x04003851 RID: 14417
		public List<OutfitData> outfitDatas;

		// Token: 0x04003852 RID: 14418
		public OutfitData bodyData;

		// Token: 0x04003853 RID: 14419
		public float stance;

		// Token: 0x04003854 RID: 14420
		public CharacterGeneralInfo generalInfo;
	}
}
