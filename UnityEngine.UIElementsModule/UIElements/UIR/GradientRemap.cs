using System;

namespace UnityEngine.UIElements.UIR
{
	// Token: 0x02000473 RID: 1139
	internal class GradientRemap : LinkedPoolItem<GradientRemap>
	{
		// Token: 0x06002345 RID: 9029 RVA: 0x000890B4 File Offset: 0x000872B4
		public void Reset()
		{
			this.origIndex = 0;
			this.destIndex = 0;
			this.location = default(RectInt);
			this.atlas = TextureId.invalid;
		}

		// Token: 0x04001067 RID: 4199
		public int origIndex;

		// Token: 0x04001068 RID: 4200
		public int destIndex;

		// Token: 0x04001069 RID: 4201
		public RectInt location;

		// Token: 0x0400106A RID: 4202
		public GradientRemap next;

		// Token: 0x0400106B RID: 4203
		public TextureId atlas;
	}
}
